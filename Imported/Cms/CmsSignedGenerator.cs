// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsSignedGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class CmsSignedGenerator
    {
        public static readonly string Data = CmsObjectIdentifiers.Data.Id;
        public static readonly string DigestSha1 = OiwObjectIdentifiers.IdSha1.Id;
        public static readonly string DigestSha224 = NistObjectIdentifiers.IdSha224.Id;
        public static readonly string DigestSha256 = NistObjectIdentifiers.IdSha256.Id;
        public static readonly string DigestSha384 = NistObjectIdentifiers.IdSha384.Id;
        public static readonly string DigestSha512 = NistObjectIdentifiers.IdSha512.Id;
        public static readonly string DigestMD5 = PkcsObjectIdentifiers.MD5.Id;
        public static readonly string DigestGost3411 = CryptoProObjectIdentifiers.GostR3411.Id;
        public static readonly string DigestRipeMD128 = TeleTrusTObjectIdentifiers.RipeMD128.Id;
        public static readonly string DigestRipeMD160 = TeleTrusTObjectIdentifiers.RipeMD160.Id;
        public static readonly string DigestRipeMD256 = TeleTrusTObjectIdentifiers.RipeMD256.Id;
        public static readonly string EncryptionRsa = PkcsObjectIdentifiers.RsaEncryption.Id;
        public static readonly string EncryptionDsa = X9ObjectIdentifiers.IdDsaWithSha1.Id;
        public static readonly string EncryptionECDsa = X9ObjectIdentifiers.ECDsaWithSha1.Id;
        public static readonly string EncryptionRsaPss = PkcsObjectIdentifiers.IdRsassaPss.Id;
        public static readonly string EncryptionGost3410 = CryptoProObjectIdentifiers.GostR3410x94.Id;
        public static readonly string EncryptionECGost3410 = CryptoProObjectIdentifiers.GostR3410x2001.Id;
        internal IList _certs = Platform.CreateArrayList();
        internal IList _crls = Platform.CreateArrayList();
        internal IList _signers = Platform.CreateArrayList();
        internal IDictionary _digests = Platform.CreateHashtable();
        protected readonly SecureRandom rand;

        protected CmsSignedGenerator()
          : this( new SecureRandom() )
        {
        }

        protected CmsSignedGenerator( SecureRandom rand ) => this.rand = rand;

        protected internal virtual IDictionary GetBaseParameters(
          DerObjectIdentifier contentType,
          AlgorithmIdentifier digAlgId,
          byte[] hash )
        {
            IDictionary hashtable = Platform.CreateHashtable();
            if (contentType != null)
                hashtable[CmsAttributeTableParameter.ContentType] = contentType;
            hashtable[CmsAttributeTableParameter.DigestAlgorithmIdentifier] = digAlgId;
            hashtable[CmsAttributeTableParameter.Digest] = hash.Clone();
            return hashtable;
        }

        protected internal virtual Asn1Set GetAttributeSet( Org.BouncyCastle.Asn1.Cms.AttributeTable attr ) => attr != null ? new DerSet( attr.ToAsn1EncodableVector() ) : (Asn1Set)null;

        public void AddCertificates( IX509Store certStore ) => CollectionUtilities.AddRange( this._certs, CmsUtilities.GetCertificatesFromStore( certStore ) );

        public void AddCrls( IX509Store crlStore ) => CollectionUtilities.AddRange( this._crls, CmsUtilities.GetCrlsFromStore( crlStore ) );

        public void AddAttributeCertificates( IX509Store store )
        {
            try
            {
                foreach (IX509AttributeCertificate match in (IEnumerable)store.GetMatches( null ))
                    this._certs.Add( new DerTaggedObject( false, 2, AttributeCertificate.GetInstance( Asn1Object.FromByteArray( match.GetEncoded() ) ) ) );
            }
            catch (Exception ex)
            {
                throw new CmsException( "error processing attribute certs", ex );
            }
        }

        public void AddSigners( SignerInformationStore signerStore )
        {
            foreach (SignerInformation signer in (IEnumerable)signerStore.GetSigners())
            {
                this._signers.Add( signer );
                this.AddSignerCallback( signer );
            }
        }

        public IDictionary GetGeneratedDigests() => Platform.CreateHashtable( this._digests );

        internal virtual void AddSignerCallback( SignerInformation si )
        {
        }

        internal static SignerIdentifier GetSignerIdentifier( X509Certificate cert ) => new SignerIdentifier( CmsUtilities.GetIssuerAndSerialNumber( cert ) );

        internal static SignerIdentifier GetSignerIdentifier( byte[] subjectKeyIdentifier ) => new SignerIdentifier( new DerOctetString( subjectKeyIdentifier ) );
    }
}
