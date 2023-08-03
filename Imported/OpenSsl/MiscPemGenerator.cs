// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.OpenSsl.MiscPemGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.OpenSsl
{
    public class MiscPemGenerator : PemObjectGenerator
    {
        private object obj;
        private string algorithm;
        private char[] password;
        private SecureRandom random;

        public MiscPemGenerator( object obj ) => this.obj = obj;

        public MiscPemGenerator( object obj, string algorithm, char[] password, SecureRandom random )
        {
            this.obj = obj;
            this.algorithm = algorithm;
            this.password = password;
            this.random = random;
        }

        private static PemObject CreatePemObject( object obj )
        {
            string type;
            byte[] content;
            switch (obj)
            {
                case null:
                    throw new ArgumentNullException( nameof( obj ) );
                case AsymmetricCipherKeyPair _:
                    return CreatePemObject( ((AsymmetricCipherKeyPair)obj).Private );
                case PemObject _:
                    return (PemObject)obj;
                case PemObjectGenerator _:
                    return ((PemObjectGenerator)obj).Generate();
                case X509Certificate _:
                    type = "CERTIFICATE";
                    try
                    {
                        content = ((X509Certificate)obj).GetEncoded();
                        break;
                    }
                    catch (CertificateEncodingException ex)
                    {
                        throw new IOException( "Cannot Encode object: " + ex.ToString() );
                    }
                case X509Crl _:
                    type = "X509 CRL";
                    try
                    {
                        content = ((X509Crl)obj).GetEncoded();
                        break;
                    }
                    catch (CrlException ex)
                    {
                        throw new IOException( "Cannot Encode object: " + ex.ToString() );
                    }
                case AsymmetricKeyParameter _:
                    AsymmetricKeyParameter asymmetricKeyParameter = (AsymmetricKeyParameter)obj;
                    if (asymmetricKeyParameter.IsPrivate)
                    {
                        string keyType;
                        content = EncodePrivateKey( asymmetricKeyParameter, out keyType );
                        type = keyType + " PRIVATE KEY";
                        break;
                    }
                    type = "PUBLIC KEY";
                    content = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( asymmetricKeyParameter ).GetDerEncoded();
                    break;
                case IX509AttributeCertificate _:
                    type = "ATTRIBUTE CERTIFICATE";
                    content = ((X509V2AttributeCertificate)obj).GetEncoded();
                    break;
                case Pkcs10CertificationRequest _:
                    type = "CERTIFICATE REQUEST";
                    content = ((Asn1Encodable)obj).GetEncoded();
                    break;
                case Org.BouncyCastle.Asn1.Cms.ContentInfo _:
                    type = "PKCS7";
                    content = ((Asn1Encodable)obj).GetEncoded();
                    break;
                default:
                    throw new PemGenerationException( "Object type not supported: " + Platform.GetTypeName( obj ) );
            }
            return new PemObject( type, content );
        }

        private static PemObject CreatePemObject(
          object obj,
          string algorithm,
          char[] password,
          SecureRandom random )
        {
            if (obj == null)
                throw new ArgumentNullException( nameof( obj ) );
            if (algorithm == null)
                throw new ArgumentNullException( nameof( algorithm ) );
            if (password == null)
                throw new ArgumentNullException( nameof( password ) );
            if (random == null)
                throw new ArgumentNullException( nameof( random ) );
            if (obj is AsymmetricCipherKeyPair)
                return CreatePemObject( ((AsymmetricCipherKeyPair)obj).Private, algorithm, password, random );
            string type = null;
            byte[] bytes = null;
            if (obj is AsymmetricKeyParameter)
            {
                AsymmetricKeyParameter akp = (AsymmetricKeyParameter)obj;
                if (akp.IsPrivate)
                {
                    string keyType;
                    bytes = EncodePrivateKey( akp, out keyType );
                    type = keyType + " PRIVATE KEY";
                }
            }
            if (type == null || bytes == null)
                throw new PemGenerationException( "Object type not supported: " + Platform.GetTypeName( obj ) );
            string str = Platform.ToUpperInvariant( algorithm );
            if (str == "DESEDE")
                str = "DES-EDE3-CBC";
            byte[] numArray = new byte[Platform.StartsWith( str, "AES-" ) ? 16 : 8];
            random.NextBytes( numArray );
            byte[] content = PemUtilities.Crypt( true, bytes, password, str, numArray );
            IList arrayList = Platform.CreateArrayList( 2 );
            arrayList.Add( new PemHeader( "Proc-Type", "4,ENCRYPTED" ) );
            arrayList.Add( new PemHeader( "DEK-Info", str + "," + Hex.ToHexString( numArray ) ) );
            return new PemObject( type, arrayList, content );
        }

        private static byte[] EncodePrivateKey( AsymmetricKeyParameter akp, out string keyType )
        {
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo( akp );
            AlgorithmIdentifier privateKeyAlgorithm = privateKeyInfo.PrivateKeyAlgorithm;
            DerObjectIdentifier algorithm = privateKeyAlgorithm.Algorithm;
            if (algorithm.Equals( X9ObjectIdentifiers.IdDsa ))
            {
                keyType = "DSA";
                DsaParameter instance = DsaParameter.GetInstance( privateKeyAlgorithm.Parameters );
                BigInteger x = ((DsaPrivateKeyParameters)akp).X;
                BigInteger bigInteger = instance.G.ModPow( x, instance.P );
                return new DerSequence( new Asn1Encodable[6]
                {
           new DerInteger(0),
           new DerInteger(instance.P),
           new DerInteger(instance.Q),
           new DerInteger(instance.G),
           new DerInteger(bigInteger),
           new DerInteger(x)
                } ).GetEncoded();
            }
            if (algorithm.Equals( PkcsObjectIdentifiers.RsaEncryption ))
            {
                keyType = "RSA";
            }
            else
            {
                if (!algorithm.Equals( CryptoProObjectIdentifiers.GostR3410x2001 ) && !algorithm.Equals( X9ObjectIdentifiers.IdECPublicKey ))
                    throw new ArgumentException( "Cannot handle private key of type: " + Platform.GetTypeName( akp ), nameof( akp ) );
                keyType = "EC";
            }
            return privateKeyInfo.ParsePrivateKey().GetEncoded();
        }

        public PemObject Generate()
        {
            try
            {
                return this.algorithm != null ? CreatePemObject( this.obj, this.algorithm, this.password, this.random ) : CreatePemObject( this.obj );
            }
            catch (IOException ex)
            {
                throw new PemGenerationException( "encoding exception", ex );
            }
        }
    }
}
