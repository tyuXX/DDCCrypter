// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509V2AttributeCertificateGenerator
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
using System.Collections;

namespace Org.BouncyCastle.X509
{
    public class X509V2AttributeCertificateGenerator
    {
        private readonly X509ExtensionsGenerator extGenerator = new();
        private V2AttributeCertificateInfoGenerator acInfoGen;
        private DerObjectIdentifier sigOID;
        private AlgorithmIdentifier sigAlgId;
        private string signatureAlgorithm;

        public X509V2AttributeCertificateGenerator() => this.acInfoGen = new V2AttributeCertificateInfoGenerator();

        public void Reset()
        {
            this.acInfoGen = new V2AttributeCertificateInfoGenerator();
            this.extGenerator.Reset();
        }

        public void SetHolder( AttributeCertificateHolder holder ) => this.acInfoGen.SetHolder( holder.holder );

        public void SetIssuer( AttributeCertificateIssuer issuer ) => this.acInfoGen.SetIssuer( AttCertIssuer.GetInstance( issuer.form ) );

        public void SetSerialNumber( BigInteger serialNumber ) => this.acInfoGen.SetSerialNumber( new DerInteger( serialNumber ) );

        public void SetNotBefore( DateTime date ) => this.acInfoGen.SetStartDate( new DerGeneralizedTime( date ) );

        public void SetNotAfter( DateTime date ) => this.acInfoGen.SetEndDate( new DerGeneralizedTime( date ) );

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
                throw new ArgumentException( "Unknown signature type requested" );
            }
            this.sigAlgId = X509Utilities.GetSigAlgID( this.sigOID, signatureAlgorithm );
            this.acInfoGen.SetSignature( this.sigAlgId );
        }

        public void AddAttribute( X509Attribute attribute ) => this.acInfoGen.AddAttribute( AttributeX509.GetInstance( attribute.ToAsn1Object() ) );

        public void SetIssuerUniqueId( bool[] iui ) => throw Platform.CreateNotImplementedException( "SetIssuerUniqueId()" );

        public void AddExtension( string oid, bool critical, Asn1Encodable extensionValue ) => this.extGenerator.AddExtension( new DerObjectIdentifier( oid ), critical, extensionValue );

        public void AddExtension( string oid, bool critical, byte[] extensionValue ) => this.extGenerator.AddExtension( new DerObjectIdentifier( oid ), critical, extensionValue );

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public IX509AttributeCertificate Generate( AsymmetricKeyParameter privateKey ) => this.Generate( privateKey, null );

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public IX509AttributeCertificate Generate(
          AsymmetricKeyParameter privateKey,
          SecureRandom random )
        {
            return this.Generate( new Asn1SignatureFactory( this.signatureAlgorithm, privateKey, random ) );
        }

        public IX509AttributeCertificate Generate( ISignatureFactory signatureCalculatorFactory )
        {
            if (!this.extGenerator.IsEmpty)
                this.acInfoGen.SetExtensions( this.extGenerator.Generate() );
            AttributeCertificateInfo attributeCertificateInfo = this.acInfoGen.GenerateAttributeCertificateInfo();
            byte[] derEncoded = attributeCertificateInfo.GetDerEncoded();
            IStreamCalculator calculator = signatureCalculatorFactory.CreateCalculator();
            calculator.Stream.Write( derEncoded, 0, derEncoded.Length );
            Platform.Dispose( calculator.Stream );
            Asn1EncodableVector v = new( new Asn1Encodable[0] )
            {
                { attributeCertificateInfo, (Asn1Encodable)signatureCalculatorFactory.AlgorithmDetails }
            };
            try
            {
                v.Add( new DerBitString( ((IBlockResult)calculator.GetResult()).Collect() ) );
                return new X509V2AttributeCertificate( AttributeCertificate.GetInstance( new DerSequence( v ) ) );
            }
            catch (Exception ex)
            {
                throw new CertificateEncodingException( "constructed invalid certificate", ex );
            }
        }

        public IEnumerable SignatureAlgNames => X509Utilities.GetAlgNames();
    }
}
