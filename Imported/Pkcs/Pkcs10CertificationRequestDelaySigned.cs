// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.Pkcs10CertificationRequestDelaySigned
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System.IO;

namespace Org.BouncyCastle.Pkcs
{
    public class Pkcs10CertificationRequestDelaySigned : Pkcs10CertificationRequest
    {
        protected Pkcs10CertificationRequestDelaySigned()
        {
        }

        public Pkcs10CertificationRequestDelaySigned( byte[] encoded )
          : base( encoded )
        {
        }

        public Pkcs10CertificationRequestDelaySigned( Asn1Sequence seq )
          : base( seq )
        {
        }

        public Pkcs10CertificationRequestDelaySigned( Stream input )
          : base( input )
        {
        }

        public Pkcs10CertificationRequestDelaySigned(
          string signatureAlgorithm,
          X509Name subject,
          AsymmetricKeyParameter publicKey,
          Asn1Set attributes,
          AsymmetricKeyParameter signingKey )
          : base( signatureAlgorithm, subject, publicKey, attributes, signingKey )
        {
        }

        public Pkcs10CertificationRequestDelaySigned(
          string signatureAlgorithm,
          X509Name subject,
          AsymmetricKeyParameter publicKey,
          Asn1Set attributes )
        {
            if (signatureAlgorithm == null)
                throw new ArgumentNullException( nameof( signatureAlgorithm ) );
            if (subject == null)
                throw new ArgumentNullException( nameof( subject ) );
            if (publicKey == null)
                throw new ArgumentNullException( nameof( publicKey ) );
            if (publicKey.IsPrivate)
                throw new ArgumentException( "expected public key", nameof( publicKey ) );
            string upperInvariant = Platform.ToUpperInvariant( signatureAlgorithm );
            DerObjectIdentifier objectIdentifier = (DerObjectIdentifier)algorithms[upperInvariant];
            if (objectIdentifier == null)
            {
                try
                {
                    objectIdentifier = new DerObjectIdentifier( upperInvariant );
                }
                catch (Exception ex)
                {
                    throw new ArgumentException( "Unknown signature type requested", ex );
                }
            }
            if (noParams.Contains( objectIdentifier ))
                this.sigAlgId = new AlgorithmIdentifier( objectIdentifier );
            else if (exParams.Contains( upperInvariant ))
                this.sigAlgId = new AlgorithmIdentifier( objectIdentifier, (Asn1Encodable)exParams[upperInvariant] );
            else
                this.sigAlgId = new AlgorithmIdentifier( objectIdentifier, DerNull.Instance );
            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( publicKey );
            this.reqInfo = new CertificationRequestInfo( subject, subjectPublicKeyInfo, attributes );
        }

        public byte[] GetDataToSign() => this.reqInfo.GetDerEncoded();

        public void SignRequest( byte[] signedData ) => this.sigBits = new DerBitString( signedData );

        public void SignRequest( DerBitString signedData ) => this.sigBits = signedData;
    }
}
