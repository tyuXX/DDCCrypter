// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsSignedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsSignedData
    {
        private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;
        private readonly CmsProcessable signedContent;
        private SignedData signedData;
        private ContentInfo contentInfo;
        private SignerInformationStore signerInfoStore;
        private IX509Store attrCertStore;
        private IX509Store certificateStore;
        private IX509Store crlStore;
        private IDictionary hashes;

        private CmsSignedData( CmsSignedData c )
        {
            this.signedData = c.signedData;
            this.contentInfo = c.contentInfo;
            this.signedContent = c.signedContent;
            this.signerInfoStore = c.signerInfoStore;
        }

        public CmsSignedData( byte[] sigBlock )
          : this( CmsUtilities.ReadContentInfo( new MemoryStream( sigBlock, false ) ) )
        {
        }

        public CmsSignedData( CmsProcessable signedContent, byte[] sigBlock )
          : this( signedContent, CmsUtilities.ReadContentInfo( new MemoryStream( sigBlock, false ) ) )
        {
        }

        public CmsSignedData( IDictionary hashes, byte[] sigBlock )
          : this( hashes, CmsUtilities.ReadContentInfo( sigBlock ) )
        {
        }

        public CmsSignedData( CmsProcessable signedContent, Stream sigData )
          : this( signedContent, CmsUtilities.ReadContentInfo( sigData ) )
        {
        }

        public CmsSignedData( Stream sigData )
          : this( CmsUtilities.ReadContentInfo( sigData ) )
        {
        }

        public CmsSignedData( CmsProcessable signedContent, ContentInfo sigData )
        {
            this.signedContent = signedContent;
            this.contentInfo = sigData;
            this.signedData = SignedData.GetInstance( contentInfo.Content );
        }

        public CmsSignedData( IDictionary hashes, ContentInfo sigData )
        {
            this.hashes = hashes;
            this.contentInfo = sigData;
            this.signedData = SignedData.GetInstance( contentInfo.Content );
        }

        public CmsSignedData( ContentInfo sigData )
        {
            this.contentInfo = sigData;
            this.signedData = SignedData.GetInstance( contentInfo.Content );
            if (this.signedData.EncapContentInfo.Content == null)
                return;
            this.signedContent = new CmsProcessableByteArray( ((Asn1OctetString)this.signedData.EncapContentInfo.Content).GetOctets() );
        }

        public int Version => this.signedData.Version.Value.IntValue;

        public SignerInformationStore GetSignerInfos()
        {
            if (this.signerInfoStore == null)
            {
                IList arrayList = Platform.CreateArrayList();
                foreach (object signerInfo in this.signedData.SignerInfos)
                {
                    SignerInfo instance = SignerInfo.GetInstance( signerInfo );
                    DerObjectIdentifier contentType = this.signedData.EncapContentInfo.ContentType;
                    if (this.hashes == null)
                    {
                        arrayList.Add( new SignerInformation( instance, contentType, this.signedContent, null ) );
                    }
                    else
                    {
                        byte[] hash = (byte[])this.hashes[instance.DigestAlgorithm.Algorithm.Id];
                        arrayList.Add( new SignerInformation( instance, contentType, null, new BaseDigestCalculator( hash ) ) );
                    }
                }
                this.signerInfoStore = new SignerInformationStore( arrayList );
            }
            return this.signerInfoStore;
        }

        public IX509Store GetAttributeCertificates( string type )
        {
            if (this.attrCertStore == null)
                this.attrCertStore = Helper.CreateAttributeStore( type, this.signedData.Certificates );
            return this.attrCertStore;
        }

        public IX509Store GetCertificates( string type )
        {
            if (this.certificateStore == null)
                this.certificateStore = Helper.CreateCertificateStore( type, this.signedData.Certificates );
            return this.certificateStore;
        }

        public IX509Store GetCrls( string type )
        {
            if (this.crlStore == null)
                this.crlStore = Helper.CreateCrlStore( type, this.signedData.CRLs );
            return this.crlStore;
        }

        [Obsolete( "Use 'SignedContentType' property instead." )]
        public string SignedContentTypeOid => this.signedData.EncapContentInfo.ContentType.Id;

        public DerObjectIdentifier SignedContentType => this.signedData.EncapContentInfo.ContentType;

        public CmsProcessable SignedContent => this.signedContent;

        public ContentInfo ContentInfo => this.contentInfo;

        public byte[] GetEncoded() => this.contentInfo.GetEncoded();

        public static CmsSignedData ReplaceSigners(
          CmsSignedData signedData,
          SignerInformationStore signerInformationStore )
        {
            CmsSignedData cmsSignedData = new( signedData )
            {
                signerInfoStore = signerInformationStore
            };
            Asn1EncodableVector v1 = new( new Asn1Encodable[0] );
            Asn1EncodableVector v2 = new( new Asn1Encodable[0] );
            foreach (SignerInformation signer in (IEnumerable)signerInformationStore.GetSigners())
            {
                v1.Add( Helper.FixAlgID( signer.DigestAlgorithmID ) );
                v2.Add( signer.ToSignerInfo() );
            }
            Asn1Set asn1Set1 = new DerSet( v1 );
            Asn1Set asn1Set2 = new DerSet( v2 );
            Asn1Sequence asn1Object = (Asn1Sequence)signedData.signedData.ToAsn1Object();
            Asn1EncodableVector v3 = new( new Asn1Encodable[2]
            {
        asn1Object[0],
         asn1Set1
            } );
            for (int index = 2; index != asn1Object.Count - 1; ++index)
                v3.Add( asn1Object[index] );
            v3.Add( asn1Set2 );
            cmsSignedData.signedData = SignedData.GetInstance( new BerSequence( v3 ) );
            cmsSignedData.contentInfo = new ContentInfo( cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData );
            return cmsSignedData;
        }

        public static CmsSignedData ReplaceCertificatesAndCrls(
          CmsSignedData signedData,
          IX509Store x509Certs,
          IX509Store x509Crls,
          IX509Store x509AttrCerts )
        {
            if (x509AttrCerts != null)
                throw Platform.CreateNotImplementedException( "Currently can't replace attribute certificates" );
            CmsSignedData cmsSignedData = new( signedData );
            Asn1Set certificates = null;
            try
            {
                Asn1Set berSetFromList = CmsUtilities.CreateBerSetFromList( CmsUtilities.GetCertificatesFromStore( x509Certs ) );
                if (berSetFromList.Count != 0)
                    certificates = berSetFromList;
            }
            catch (X509StoreException ex)
            {
                throw new CmsException( "error getting certificates from store", ex );
            }
            Asn1Set crls = null;
            try
            {
                Asn1Set berSetFromList = CmsUtilities.CreateBerSetFromList( CmsUtilities.GetCrlsFromStore( x509Crls ) );
                if (berSetFromList.Count != 0)
                    crls = berSetFromList;
            }
            catch (X509StoreException ex)
            {
                throw new CmsException( "error getting CRLs from store", ex );
            }
            SignedData signedData1 = signedData.signedData;
            cmsSignedData.signedData = new SignedData( signedData1.DigestAlgorithms, signedData1.EncapContentInfo, certificates, crls, signedData1.SignerInfos );
            cmsSignedData.contentInfo = new ContentInfo( cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData );
            return cmsSignedData;
        }
    }
}
