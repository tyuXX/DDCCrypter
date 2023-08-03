// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsSignedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsSignedDataParser : CmsContentInfoParser
    {
        private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;
        private SignedDataParser _signedData;
        private DerObjectIdentifier _signedContentType;
        private CmsTypedStream _signedContent;
        private IDictionary _digests;
        private ISet _digestOids;
        private SignerInformationStore _signerInfoStore;
        private Asn1Set _certSet;
        private Asn1Set _crlSet;
        private bool _isCertCrlParsed;
        private IX509Store _attributeStore;
        private IX509Store _certificateStore;
        private IX509Store _crlStore;

        public CmsSignedDataParser( byte[] sigBlock )
          : this( new MemoryStream( sigBlock, false ) )
        {
        }

        public CmsSignedDataParser( CmsTypedStream signedContent, byte[] sigBlock )
          : this( signedContent, new MemoryStream( sigBlock, false ) )
        {
        }

        public CmsSignedDataParser( Stream sigData )
          : this( null, sigData )
        {
        }

        public CmsSignedDataParser( CmsTypedStream signedContent, Stream sigData )
          : base( sigData )
        {
            try
            {
                this._signedContent = signedContent;
                this._signedData = SignedDataParser.GetInstance( this.contentInfo.GetContent( 16 ) );
                this._digests = Platform.CreateHashtable();
                this._digestOids = new HashSet();
                Asn1SetParser digestAlgorithms = this._signedData.GetDigestAlgorithms();
                IAsn1Convertible asn1Convertible;
                while ((asn1Convertible = digestAlgorithms.ReadObject()) != null)
                {
                    AlgorithmIdentifier instance = AlgorithmIdentifier.GetInstance( asn1Convertible.ToAsn1Object() );
                    try
                    {
                        string id = instance.Algorithm.Id;
                        string digestAlgName = Helper.GetDigestAlgName( id );
                        if (!this._digests.Contains( digestAlgName ))
                        {
                            this._digests[digestAlgName] = Helper.GetDigestInstance( digestAlgName );
                            this._digestOids.Add( id );
                        }
                    }
                    catch (SecurityUtilityException ex)
                    {
                    }
                }
                ContentInfoParser encapContentInfo = this._signedData.GetEncapContentInfo();
                Asn1OctetStringParser content = (Asn1OctetStringParser)encapContentInfo.GetContent( 4 );
                if (content != null)
                {
                    CmsTypedStream cmsTypedStream = new CmsTypedStream( encapContentInfo.ContentType.Id, content.GetOctetStream() );
                    if (this._signedContent == null)
                        this._signedContent = cmsTypedStream;
                    else
                        cmsTypedStream.Drain();
                }
                this._signedContentType = this._signedContent == null ? encapContentInfo.ContentType : new DerObjectIdentifier( this._signedContent.ContentType );
            }
            catch (IOException ex)
            {
                throw new CmsException( "io exception: " + ex.Message, ex );
            }
        }

        public int Version => this._signedData.Version.Value.IntValue;

        public ISet DigestOids => new HashSet( _digestOids );

        public SignerInformationStore GetSignerInfos()
        {
            if (this._signerInfoStore == null)
            {
                this.PopulateCertCrlSets();
                IList arrayList = Platform.CreateArrayList();
                IDictionary hashtable = Platform.CreateHashtable();
                foreach (object key in (IEnumerable)this._digests.Keys)
                    hashtable[key] = DigestUtilities.DoFinal( (IDigest)this._digests[key] );
                try
                {
                    Asn1SetParser signerInfos = this._signedData.GetSignerInfos();
                    IAsn1Convertible asn1Convertible;
                    while ((asn1Convertible = signerInfos.ReadObject()) != null)
                    {
                        SignerInfo instance = SignerInfo.GetInstance( asn1Convertible.ToAsn1Object() );
                        string digestAlgName = Helper.GetDigestAlgName( instance.DigestAlgorithm.Algorithm.Id );
                        byte[] digest = (byte[])hashtable[digestAlgName];
                        arrayList.Add( new SignerInformation( instance, this._signedContentType, null, new BaseDigestCalculator( digest ) ) );
                    }
                }
                catch (IOException ex)
                {
                    throw new CmsException( "io exception: " + ex.Message, ex );
                }
                this._signerInfoStore = new SignerInformationStore( arrayList );
            }
            return this._signerInfoStore;
        }

        public IX509Store GetAttributeCertificates( string type )
        {
            if (this._attributeStore == null)
            {
                this.PopulateCertCrlSets();
                this._attributeStore = Helper.CreateAttributeStore( type, this._certSet );
            }
            return this._attributeStore;
        }

        public IX509Store GetCertificates( string type )
        {
            if (this._certificateStore == null)
            {
                this.PopulateCertCrlSets();
                this._certificateStore = Helper.CreateCertificateStore( type, this._certSet );
            }
            return this._certificateStore;
        }

        public IX509Store GetCrls( string type )
        {
            if (this._crlStore == null)
            {
                this.PopulateCertCrlSets();
                this._crlStore = Helper.CreateCrlStore( type, this._crlSet );
            }
            return this._crlStore;
        }

        private void PopulateCertCrlSets()
        {
            if (this._isCertCrlParsed)
                return;
            this._isCertCrlParsed = true;
            try
            {
                this._certSet = GetAsn1Set( this._signedData.GetCertificates() );
                this._crlSet = GetAsn1Set( this._signedData.GetCrls() );
            }
            catch (IOException ex)
            {
                throw new CmsException( "problem parsing cert/crl sets", ex );
            }
        }

        public DerObjectIdentifier SignedContentType => this._signedContentType;

        public CmsTypedStream GetSignedContent()
        {
            if (this._signedContent == null)
                return null;
            Stream stream = this._signedContent.ContentStream;
            foreach (IDigest readDigest in (IEnumerable)this._digests.Values)
                stream = new DigestStream( stream, readDigest, null );
            return new CmsTypedStream( this._signedContent.ContentType, stream );
        }

        public static Stream ReplaceSigners(
          Stream original,
          SignerInformationStore signerInformationStore,
          Stream outStr )
        {
            CmsSignedDataStreamGenerator dataStreamGenerator = new CmsSignedDataStreamGenerator();
            CmsSignedDataParser signedDataParser = new CmsSignedDataParser( original );
            dataStreamGenerator.AddSigners( signerInformationStore );
            CmsTypedStream signedContent = signedDataParser.GetSignedContent();
            bool encapsulate = signedContent != null;
            Stream stream = dataStreamGenerator.Open( outStr, signedDataParser.SignedContentType.Id, encapsulate );
            if (encapsulate)
                Streams.PipeAll( signedContent.ContentStream, stream );
            dataStreamGenerator.AddAttributeCertificates( signedDataParser.GetAttributeCertificates( "Collection" ) );
            dataStreamGenerator.AddCertificates( signedDataParser.GetCertificates( "Collection" ) );
            dataStreamGenerator.AddCrls( signedDataParser.GetCrls( "Collection" ) );
            Platform.Dispose( stream );
            return outStr;
        }

        public static Stream ReplaceCertificatesAndCrls(
          Stream original,
          IX509Store x509Certs,
          IX509Store x509Crls,
          IX509Store x509AttrCerts,
          Stream outStr )
        {
            CmsSignedDataStreamGenerator dataStreamGenerator = new CmsSignedDataStreamGenerator();
            CmsSignedDataParser signedDataParser = new CmsSignedDataParser( original );
            dataStreamGenerator.AddDigests( signedDataParser.DigestOids );
            CmsTypedStream signedContent = signedDataParser.GetSignedContent();
            bool encapsulate = signedContent != null;
            Stream stream = dataStreamGenerator.Open( outStr, signedDataParser.SignedContentType.Id, encapsulate );
            if (encapsulate)
                Streams.PipeAll( signedContent.ContentStream, stream );
            if (x509AttrCerts != null)
                dataStreamGenerator.AddAttributeCertificates( x509AttrCerts );
            if (x509Certs != null)
                dataStreamGenerator.AddCertificates( x509Certs );
            if (x509Crls != null)
                dataStreamGenerator.AddCrls( x509Crls );
            dataStreamGenerator.AddSigners( signedDataParser.GetSignerInfos() );
            Platform.Dispose( stream );
            return outStr;
        }

        private static Asn1Set GetAsn1Set( Asn1SetParser asn1SetParser ) => asn1SetParser != null ? Asn1Set.GetInstance( asn1SetParser.ToAsn1Object() ) : null;
    }
}
