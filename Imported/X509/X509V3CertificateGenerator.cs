// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509V3CertificateGenerator
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
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;

namespace Org.BouncyCastle.X509
{
    public class X509V3CertificateGenerator
    {
        private readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();
        private V3TbsCertificateGenerator tbsGen;
        private DerObjectIdentifier sigOid;
        private AlgorithmIdentifier sigAlgId;
        private string signatureAlgorithm;

        public X509V3CertificateGenerator() => this.tbsGen = new V3TbsCertificateGenerator();

        public void Reset()
        {
            this.tbsGen = new V3TbsCertificateGenerator();
            this.extGenerator.Reset();
        }

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

        public void SetPublicKey( AsymmetricKeyParameter publicKey ) => this.tbsGen.SetSubjectPublicKeyInfo( SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( publicKey ) );

        [Obsolete( "Not needed if Generate used with an ISignatureFactory" )]
        public void SetSignatureAlgorithm( string signatureAlgorithm )
        {
            this.signatureAlgorithm = signatureAlgorithm;
            try
            {
                this.sigOid = X509Utilities.GetAlgorithmOid( signatureAlgorithm );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "Unknown signature type requested: " + signatureAlgorithm );
            }
            this.sigAlgId = X509Utilities.GetSigAlgID( this.sigOid, signatureAlgorithm );
            this.tbsGen.SetSignature( this.sigAlgId );
        }

        public void SetSubjectUniqueID( bool[] uniqueID ) => this.tbsGen.SetSubjectUniqueID( this.booleanToBitString( uniqueID ) );

        public void SetIssuerUniqueID( bool[] uniqueID ) => this.tbsGen.SetIssuerUniqueID( this.booleanToBitString( uniqueID ) );

        private DerBitString booleanToBitString( bool[] id )
        {
            byte[] data = new byte[(id.Length + 7) / 8];
            for (int index1 = 0; index1 != id.Length; ++index1)
            {
                if (id[index1])
                {
                    byte[] numArray;
                    IntPtr index2;
                    (numArray = data)[(int)(index2 = (IntPtr)(index1 / 8))] = (byte)(numArray[(int)index2] | (uint)(byte)(1 << (7 - (index1 % 8))));
                }
            }
            int num = id.Length % 8;
            return num == 0 ? new DerBitString( data ) : new DerBitString( data, 8 - num );
        }

        public void AddExtension( string oid, bool critical, Asn1Encodable extensionValue ) => this.extGenerator.AddExtension( new DerObjectIdentifier( oid ), critical, extensionValue );

        public void AddExtension( DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue ) => this.extGenerator.AddExtension( oid, critical, extensionValue );

        public void AddExtension( string oid, bool critical, byte[] extensionValue ) => this.extGenerator.AddExtension( new DerObjectIdentifier( oid ), critical, new DerOctetString( extensionValue ) );

        public void AddExtension( DerObjectIdentifier oid, bool critical, byte[] extensionValue ) => this.extGenerator.AddExtension( oid, critical, new DerOctetString( extensionValue ) );

        public void CopyAndAddExtension( string oid, bool critical, X509Certificate cert ) => this.CopyAndAddExtension( new DerObjectIdentifier( oid ), critical, cert );

        public void CopyAndAddExtension( DerObjectIdentifier oid, bool critical, X509Certificate cert )
        {
            Asn1OctetString extensionValue1 = cert.GetExtensionValue( oid );
            if (extensionValue1 == null)
                throw new CertificateParsingException( "extension " + oid + " not present" );
            try
            {
                Asn1Encodable extensionValue2 = X509ExtensionUtilities.FromExtensionValue( extensionValue1 );
                this.AddExtension( oid, critical, extensionValue2 );
            }
            catch (Exception ex)
            {
                throw new CertificateParsingException( ex.Message, ex );
            }
        }

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public X509Certificate Generate( AsymmetricKeyParameter privateKey ) => this.Generate( privateKey, null );

        [Obsolete( "Use Generate with an ISignatureFactory" )]
        public X509Certificate Generate( AsymmetricKeyParameter privateKey, SecureRandom random ) => this.Generate( new Asn1SignatureFactory( this.signatureAlgorithm, privateKey, random ) );

        public X509Certificate Generate( ISignatureFactory signatureCalculatorFactory )
        {
            this.tbsGen.SetSignature( (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails );
            if (!this.extGenerator.IsEmpty)
                this.tbsGen.SetExtensions( this.extGenerator.Generate() );
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
