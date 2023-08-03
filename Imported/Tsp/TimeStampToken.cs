// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampToken
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampToken
    {
        private readonly CmsSignedData tsToken;
        private readonly SignerInformation tsaSignerInfo;
        private readonly TimeStampTokenInfo tstInfo;
        private readonly TimeStampToken.CertID certID;

        public TimeStampToken( Org.BouncyCastle.Asn1.Cms.ContentInfo contentInfo )
          : this( new CmsSignedData( contentInfo ) )
        {
        }

        public TimeStampToken( CmsSignedData signedData )
        {
            this.tsToken = signedData;
            if (!this.tsToken.SignedContentType.Equals( PkcsObjectIdentifiers.IdCTTstInfo ))
                throw new TspValidationException( "ContentInfo object not for a time stamp." );
            ICollection signers = this.tsToken.GetSignerInfos().GetSigners();
            IEnumerator enumerator = signers.Count == 1 ? signers.GetEnumerator() : throw new ArgumentException( "Time-stamp token signed by " + signers.Count + " signers, but it must contain just the TSA signature." );
            enumerator.MoveNext();
            this.tsaSignerInfo = (SignerInformation)enumerator.Current;
            try
            {
                CmsProcessable signedContent = this.tsToken.SignedContent;
                MemoryStream outStream = new();
                signedContent.Write( outStream );
                this.tstInfo = new TimeStampTokenInfo( TstInfo.GetInstance( Asn1Object.FromByteArray( outStream.ToArray() ) ) );
                Org.BouncyCastle.Asn1.Cms.Attribute signedAttribute = this.tsaSignerInfo.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificate];
                if (signedAttribute != null)
                    this.certID = new TimeStampToken.CertID( EssCertID.GetInstance( SigningCertificate.GetInstance( signedAttribute.AttrValues[0] ).GetCerts()[0] ) );
                else
                    this.certID = new TimeStampToken.CertID( EssCertIDv2.GetInstance( SigningCertificateV2.GetInstance( (this.tsaSignerInfo.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificateV2] ?? throw new TspValidationException( "no signing certificate attribute found, time stamp invalid." )).AttrValues[0] ).GetCerts()[0] ) );
            }
            catch (CmsException ex)
            {
                throw new TspException( ex.Message, ex.InnerException );
            }
        }

        public TimeStampTokenInfo TimeStampInfo => this.tstInfo;

        public SignerID SignerID => this.tsaSignerInfo.SignerID;

        public Org.BouncyCastle.Asn1.Cms.AttributeTable SignedAttributes => this.tsaSignerInfo.SignedAttributes;

        public Org.BouncyCastle.Asn1.Cms.AttributeTable UnsignedAttributes => this.tsaSignerInfo.UnsignedAttributes;

        public IX509Store GetCertificates( string type ) => this.tsToken.GetCertificates( type );

        public IX509Store GetCrls( string type ) => this.tsToken.GetCrls( type );

        public IX509Store GetAttributeCertificates( string type ) => this.tsToken.GetAttributeCertificates( type );

        public void Validate( X509Certificate cert )
        {
            try
            {
                byte[] digest = DigestUtilities.CalculateDigest( this.certID.GetHashAlgorithmName(), cert.GetEncoded() );
                if (!Arrays.ConstantTimeAreEqual( this.certID.GetCertHash(), digest ))
                    throw new TspValidationException( "certificate hash does not match certID hash." );
                if (this.certID.IssuerSerial != null)
                {
                    if (!this.certID.IssuerSerial.Serial.Value.Equals( cert.SerialNumber ))
                        throw new TspValidationException( "certificate serial number does not match certID for signature." );
                    GeneralName[] names = this.certID.IssuerSerial.Issuer.GetNames();
                    X509Name issuerX509Principal = PrincipalUtilities.GetIssuerX509Principal( cert );
                    bool flag = false;
                    for (int index = 0; index != names.Length; ++index)
                    {
                        if (names[index].TagNo == 4 && X509Name.GetInstance( names[index].Name ).Equivalent( issuerX509Principal ))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        throw new TspValidationException( "certificate name does not match certID for signature. " );
                }
                TspUtil.ValidateCertificate( cert );
                cert.CheckValidity( this.tstInfo.GenTime );
                if (!this.tsaSignerInfo.Verify( cert ))
                    throw new TspValidationException( "signature not created by certificate." );
            }
            catch (CmsException ex)
            {
                if (ex.InnerException != null)
                    throw new TspException( ex.Message, ex.InnerException );
                throw new TspException( "CMS exception: " + ex, ex );
            }
            catch (CertificateEncodingException ex)
            {
                throw new TspException( "problem processing certificate: " + ex, ex );
            }
            catch (SecurityUtilityException ex)
            {
                throw new TspException( "cannot find algorithm: " + ex.Message, ex );
            }
        }

        public CmsSignedData ToCmsSignedData() => this.tsToken;

        public byte[] GetEncoded() => this.tsToken.GetEncoded();

        private class CertID
        {
            private EssCertID certID;
            private EssCertIDv2 certIDv2;

            internal CertID( EssCertID certID )
            {
                this.certID = certID;
                this.certIDv2 = null;
            }

            internal CertID( EssCertIDv2 certID )
            {
                this.certIDv2 = certID;
                this.certID = null;
            }

            public string GetHashAlgorithmName()
            {
                if (this.certID != null)
                    return "SHA-1";
                return NistObjectIdentifiers.IdSha256.Equals( certIDv2.HashAlgorithm.Algorithm ) ? "SHA-256" : this.certIDv2.HashAlgorithm.Algorithm.Id;
            }

            public AlgorithmIdentifier GetHashAlgorithm() => this.certID == null ? this.certIDv2.HashAlgorithm : new AlgorithmIdentifier( OiwObjectIdentifiers.IdSha1 );

            public byte[] GetCertHash() => this.certID == null ? this.certIDv2.GetCertHash() : this.certID.GetCertHash();

            public IssuerSerial IssuerSerial => this.certID == null ? this.certIDv2.IssuerSerial : this.certID.IssuerSerial;
        }
    }
}
