// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509V1CertificateGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.X509
{
    public class X509V1CertificateGenerator
    {
        private V1TbsCertificateGenerator tbsGen;
        private DerObjectIdentifier sigOID;
        private AlgorithmIdentifier sigAlgId;
        private string signatureAlgorithm;

        public X509V1CertificateGenerator() => this.tbsGen = new V1TbsCertificateGenerator();

        public void Reset() => this.tbsGen = new V1TbsCertificateGenerator();

        public void SetSerialNumber( BigInteger serialNumber )
        {
            if (serialNumber.SignValue <= 0)
                throw new ArgumentException( "serial number must be a positive integer", nameof( serialNumber ) );
            this.tbsGen.SetSerialNumber( new DerInteger( serialNumber ) );
        }

        public void SetIssuerDN( X509Name issuer ) => this.tbsGen.SetIssuer( issuer );

        public void SetNotBefore( DateTime date ) => this.tbsGen.SetStartDate( new Time( date ) );

        public void SetNotAfter( DateTime date ) => this.tbsGen.SetEndDate( new Time( date ) );

        public void SetSubjectDN( X509Name subject ) => this.tbsGen.SetSubject( subject );

        public void SetPublicKey( AsymmetricKeyParameter publicKey )
        {
            try
            {
                this.tbsGen.SetSubjectPublicKeyInfo( SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( publicKey ) );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "unable to process key - " + ex.ToString() );
            }
        }

        [Obsolete( "Not needed if Generate used with an ISignatureFactory" )]
        public void SetSignatureAlgorithm( string signatureAlgorithm )
        {
            this.signatureAlgorithm = signatureAlgorithm;
            try
            {
                this.sigOID = X509Utilities.GetAlgorithmOid( signatureAlgorithm );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "Unknown signature type requested", nameof( signatureAlgorithm ) );
            }
            this.sigAlgId = X509Utilities.GetSigAlgID( this.sigOID, signatureAlgorithm );
            this.tbsGen.SetSignature( this.sigAlgId );
        }

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public X509Certificate Generate( AsymmetricKeyParameter privateKey ) => this.Generate( privateKey, null );

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public X509Certificate Generate( AsymmetricKeyParameter privateKey, SecureRandom random ) => this.Generate( new Asn1SignatureFactory( this.signatureAlgorithm, privateKey, random ) );

        public X509Certificate Generate( ISignatureFactory signatureCalculatorFactory )
        {
            this.tbsGen.SetSignature( (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails );
            TbsCertificateStructure tbsCertificate = this.tbsGen.GenerateTbsCertificate();
            IStreamCalculator calculator = signatureCalculatorFactory.CreateCalculator();
            byte[] derEncoded = tbsCertificate.GetDerEncoded();
            calculator.Stream.Write( derEncoded, 0, derEncoded.Length );
            Platform.Dispose( calculator.Stream );
            return this.GenerateJcaObject( tbsCertificate, (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails, ((IBlockResult)calculator.GetResult()).Collect() );
        }

        private X509Certificate GenerateJcaObject(
          TbsCertificateStructure tbsCert,
          AlgorithmIdentifier sigAlg,
          byte[] signature )
        {
            return new X509Certificate( new X509CertificateStructure( tbsCert, sigAlg, new DerBitString( signature ) ) );
        }

        public IEnumerable SignatureAlgNames => X509Utilities.GetAlgNames();
    }
}
