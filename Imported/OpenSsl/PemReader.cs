// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.OpenSsl.PemReader
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.OpenSsl
{
    public class PemReader : Org.BouncyCastle.Utilities.IO.Pem.PemReader
    {
        private readonly IPasswordFinder pFinder;

        public PemReader( TextReader reader )
          : this( reader, null )
        {
        }

        public PemReader( TextReader reader, IPasswordFinder pFinder )
          : base( reader )
        {
            this.pFinder = pFinder;
        }

        public object ReadObject()
        {
            PemObject pemObject = this.ReadPemObject();
            if (pemObject == null)
                return null;
            if (Platform.EndsWith( pemObject.Type, "PRIVATE KEY" ))
                return this.ReadPrivateKey( pemObject );
            switch (pemObject.Type)
            {
                case "PUBLIC KEY":
                    return this.ReadPublicKey( pemObject );
                case "RSA PUBLIC KEY":
                    return this.ReadRsaPublicKey( pemObject );
                case "CERTIFICATE REQUEST":
                case "NEW CERTIFICATE REQUEST":
                    return this.ReadCertificateRequest( pemObject );
                case "CERTIFICATE":
                case "X509 CERTIFICATE":
                    return this.ReadCertificate( pemObject );
                case "PKCS7":
                case "CMS":
                    return this.ReadPkcs7( pemObject );
                case "X509 CRL":
                    return this.ReadCrl( pemObject );
                case "ATTRIBUTE CERTIFICATE":
                    return this.ReadAttributeCertificate( pemObject );
                default:
                    throw new IOException( "unrecognised object: " + pemObject.Type );
            }
        }

        private AsymmetricKeyParameter ReadRsaPublicKey( PemObject pemObject )
        {
            RsaPublicKeyStructure instance = RsaPublicKeyStructure.GetInstance( Asn1Object.FromByteArray( pemObject.Content ) );
            return new RsaKeyParameters( false, instance.Modulus, instance.PublicExponent );
        }

        private AsymmetricKeyParameter ReadPublicKey( PemObject pemObject ) => PublicKeyFactory.CreateKey( pemObject.Content );

        private X509Certificate ReadCertificate( PemObject pemObject )
        {
            try
            {
                return new X509CertificateParser().ReadCertificate( pemObject.Content );
            }
            catch (Exception ex)
            {
                throw new PemException( "problem parsing cert: " + ex.ToString() );
            }
        }

        private X509Crl ReadCrl( PemObject pemObject )
        {
            try
            {
                return new X509CrlParser().ReadCrl( pemObject.Content );
            }
            catch (Exception ex)
            {
                throw new PemException( "problem parsing cert: " + ex.ToString() );
            }
        }

        private Pkcs10CertificationRequest ReadCertificateRequest( PemObject pemObject )
        {
            try
            {
                return new Pkcs10CertificationRequest( pemObject.Content );
            }
            catch (Exception ex)
            {
                throw new PemException( "problem parsing cert: " + ex.ToString() );
            }
        }

        private IX509AttributeCertificate ReadAttributeCertificate( PemObject pemObject ) => new X509V2AttributeCertificate( pemObject.Content );

        private Org.BouncyCastle.Asn1.Cms.ContentInfo ReadPkcs7( PemObject pemObject )
        {
            try
            {
                return Asn1.Cms.ContentInfo.GetInstance( Asn1Object.FromByteArray( pemObject.Content ) );
            }
            catch (Exception ex)
            {
                throw new PemException( "problem parsing PKCS7 object: " + ex.ToString() );
            }
        }

        private object ReadPrivateKey( PemObject pemObject )
        {
            string str = pemObject.Type.Substring( 0, pemObject.Type.Length - "PRIVATE KEY".Length ).Trim();
            byte[] bytes = pemObject.Content;
            IDictionary hashtable = Platform.CreateHashtable();
            foreach (PemHeader header in (IEnumerable)pemObject.Headers)
                hashtable[header.Name] = header.Value;
            if ((string)hashtable["Proc-Type"] == "4,ENCRYPTED")
            {
                char[] password = this.pFinder != null ? this.pFinder.GetPassword() : throw new PasswordException( "No password finder specified, but a password is required" );
                if (password == null)
                    throw new PasswordException( "Password is null, but a password is required" );
                string[] strArray = ((string)hashtable["DEK-Info"]).Split( ',' );
                string dekAlgName = strArray[0].Trim();
                byte[] iv = Hex.Decode( strArray[1].Trim() );
                bytes = PemUtilities.Crypt( false, bytes, password, dekAlgName, iv );
            }
            try
            {
                Asn1Sequence instance1 = Asn1Sequence.GetInstance( bytes );
                AsymmetricKeyParameter publicParameter;
                AsymmetricKeyParameter asymmetricKeyParameter;
                switch (str)
                {
                    case "RSA":
                        RsaPrivateKeyStructure privateKeyStructure = instance1.Count == 9 ? RsaPrivateKeyStructure.GetInstance( instance1 ) : throw new PemException( "malformed sequence in RSA private key" );
                        publicParameter = new RsaKeyParameters( false, privateKeyStructure.Modulus, privateKeyStructure.PublicExponent );
                        asymmetricKeyParameter = new RsaPrivateCrtKeyParameters( privateKeyStructure.Modulus, privateKeyStructure.PublicExponent, privateKeyStructure.PrivateExponent, privateKeyStructure.Prime1, privateKeyStructure.Prime2, privateKeyStructure.Exponent1, privateKeyStructure.Exponent2, privateKeyStructure.Coefficient );
                        break;
                    case "DSA":
                        DerInteger derInteger1 = instance1.Count == 6 ? (DerInteger)instance1[1] : throw new PemException( "malformed sequence in DSA private key" );
                        DerInteger derInteger2 = (DerInteger)instance1[2];
                        DerInteger derInteger3 = (DerInteger)instance1[3];
                        DerInteger derInteger4 = (DerInteger)instance1[4];
                        DerInteger derInteger5 = (DerInteger)instance1[5];
                        DsaParameters parameters = new( derInteger1.Value, derInteger2.Value, derInteger3.Value );
                        asymmetricKeyParameter = new DsaPrivateKeyParameters( derInteger5.Value, parameters );
                        publicParameter = new DsaPublicKeyParameters( derInteger4.Value, parameters );
                        break;
                    case "EC":
                        ECPrivateKeyStructure instance2 = ECPrivateKeyStructure.GetInstance( instance1 );
                        AlgorithmIdentifier algID = new( X9ObjectIdentifiers.IdECPublicKey, instance2.GetParameters() );
                        asymmetricKeyParameter = PrivateKeyFactory.CreateKey( new PrivateKeyInfo( algID, instance2.ToAsn1Object() ) );
                        DerBitString publicKey = instance2.GetPublicKey();
                        publicParameter = publicKey == null ? ECKeyPairGenerator.GetCorrespondingPublicKey( (ECPrivateKeyParameters)asymmetricKeyParameter ) : PublicKeyFactory.CreateKey( new SubjectPublicKeyInfo( algID, publicKey.GetBytes() ) );
                        break;
                    case "ENCRYPTED":
                        return PrivateKeyFactory.DecryptKey( this.pFinder.GetPassword() ?? throw new PasswordException( "Password is null, but a password is required" ), EncryptedPrivateKeyInfo.GetInstance( instance1 ) );
                    case "":
                        return PrivateKeyFactory.CreateKey( PrivateKeyInfo.GetInstance( instance1 ) );
                    default:
                        throw new ArgumentException( "Unknown key type: " + str, "type" );
                }
                return new AsymmetricCipherKeyPair( publicParameter, asymmetricKeyParameter );
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PemException( "problem creating " + str + " private key: " + ex.ToString() );
            }
        }

        private static X9ECParameters GetCurveParameters( string name ) => (CustomNamedCurves.GetByName( name ) ?? ECNamedCurveTable.GetByName( name )) ?? throw new Exception( "unknown curve name: " + name );
    }
}
