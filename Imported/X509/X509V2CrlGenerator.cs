// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509V2CrlGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.X509
{
    public class X509V2CrlGenerator
    {
        private readonly X509ExtensionsGenerator extGenerator = new();
        private V2TbsCertListGenerator tbsGen;
        private DerObjectIdentifier sigOID;
        private AlgorithmIdentifier sigAlgId;
        private string signatureAlgorithm;

        public X509V2CrlGenerator() => this.tbsGen = new V2TbsCertListGenerator();

        public void Reset()
        {
            this.tbsGen = new V2TbsCertListGenerator();
            this.extGenerator.Reset();
        }

        public void SetIssuerDN( X509Name issuer ) => this.tbsGen.SetIssuer( issuer );

        public void SetThisUpdate( DateTime date ) => this.tbsGen.SetThisUpdate( new Time( date ) );

        public void SetNextUpdate( DateTime date ) => this.tbsGen.SetNextUpdate( new Time( date ) );

        public void AddCrlEntry( BigInteger userCertificate, DateTime revocationDate, int reason ) => this.tbsGen.AddCrlEntry( new DerInteger( userCertificate ), new Time( revocationDate ), reason );

        public void AddCrlEntry(
          BigInteger userCertificate,
          DateTime revocationDate,
          int reason,
          DateTime invalidityDate )
        {
            this.tbsGen.AddCrlEntry( new DerInteger( userCertificate ), new Time( revocationDate ), reason, new DerGeneralizedTime( invalidityDate ) );
        }

        public void AddCrlEntry(
          BigInteger userCertificate,
          DateTime revocationDate,
          X509Extensions extensions )
        {
            this.tbsGen.AddCrlEntry( new DerInteger( userCertificate ), new Time( revocationDate ), extensions );
        }

        public void AddCrl( X509Crl other )
        {
            ISet set = other != null ? other.GetRevokedCertificates() : throw new ArgumentNullException( nameof( other ) );
            if (set == null)
                return;
            foreach (X509CrlEntry x509CrlEntry in (IEnumerable)set)
            {
                try
                {
                    this.tbsGen.AddCrlEntry( Asn1Sequence.GetInstance( Asn1Object.FromByteArray( x509CrlEntry.GetEncoded() ) ) );
                }
                catch (IOException ex)
                {
                    throw new CrlException( "exception processing encoding of CRL", ex );
                }
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
                throw new ArgumentException( "Unknown signature type requested", ex );
            }
            this.sigAlgId = X509Utilities.GetSigAlgID( this.sigOID, signatureAlgorithm );
            this.tbsGen.SetSignature( this.sigAlgId );
        }

        public void AddExtension( string oid, bool critical, Asn1Encodable extensionValue ) => this.extGenerator.AddExtension( new DerObjectIdentifier( oid ), critical, extensionValue );

        public void AddExtension( DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue ) => this.extGenerator.AddExtension( oid, critical, extensionValue );

        public void AddExtension( string oid, bool critical, byte[] extensionValue ) => this.extGenerator.AddExtension( new DerObjectIdentifier( oid ), critical, new DerOctetString( extensionValue ) );

        public void AddExtension( DerObjectIdentifier oid, bool critical, byte[] extensionValue ) => this.extGenerator.AddExtension( oid, critical, new DerOctetString( extensionValue ) );

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public X509Crl Generate( AsymmetricKeyParameter privateKey ) => this.Generate( privateKey, null );

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public X509Crl Generate( AsymmetricKeyParameter privateKey, SecureRandom random ) => this.Generate( new Asn1SignatureFactory( this.signatureAlgorithm, privateKey, random ) );

        public X509Crl Generate( ISignatureFactory signatureCalculatorFactory )
        {
            this.tbsGen.SetSignature( (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails );
            TbsCertificateList certList = this.GenerateCertList();
            IStreamCalculator calculator = signatureCalculatorFactory.CreateCalculator();
            byte[] derEncoded = certList.GetDerEncoded();
            calculator.Stream.Write( derEncoded, 0, derEncoded.Length );
            Platform.Dispose( calculator.Stream );
            return this.GenerateJcaObject( certList, (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails, ((IBlockResult)calculator.GetResult()).Collect() );
        }

        private TbsCertificateList GenerateCertList()
        {
            if (!this.extGenerator.IsEmpty)
                this.tbsGen.SetExtensions( this.extGenerator.Generate() );
            return this.tbsGen.GenerateTbsCertList();
        }

        private X509Crl GenerateJcaObject(
          TbsCertificateList tbsCrl,
          AlgorithmIdentifier algId,
          byte[] signature )
        {
            return new X509Crl( CertificateList.GetInstance( new DerSequence( new Asn1Encodable[3]
            {
         tbsCrl,
         algId,
         new DerBitString(signature)
            } ) ) );
        }

        public IEnumerable SignatureAlgNames => X509Utilities.GetAlgNames();
    }
}
