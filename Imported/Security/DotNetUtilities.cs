// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.DotNetUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System;
using System.Security.Cryptography;

namespace Org.BouncyCastle.Security
{
    public sealed class DotNetUtilities
    {
        private DotNetUtilities()
        {
        }

        public static System.Security.Cryptography.X509Certificates.X509Certificate ToX509Certificate(
          X509CertificateStructure x509Struct )
        {
            return new System.Security.Cryptography.X509Certificates.X509Certificate( x509Struct.GetDerEncoded() );
        }

        public static System.Security.Cryptography.X509Certificates.X509Certificate ToX509Certificate(
          Org.BouncyCastle.X509.X509Certificate x509Cert )
        {
            return new System.Security.Cryptography.X509Certificates.X509Certificate( x509Cert.GetEncoded() );
        }

        public static Org.BouncyCastle.X509.X509Certificate FromX509Certificate( System.Security.Cryptography.X509Certificates.X509Certificate x509Cert ) => new X509CertificateParser().ReadCertificate( x509Cert.GetRawCertData() );

        public static AsymmetricCipherKeyPair GetDsaKeyPair( DSA dsa ) => GetDsaKeyPair( dsa.ExportParameters( true ) );

        public static AsymmetricCipherKeyPair GetDsaKeyPair( DSAParameters dp )
        {
            DsaValidationParameters parameters1 = dp.Seed != null ? new DsaValidationParameters( dp.Seed, dp.Counter ) : null;
            DsaParameters parameters2 = new DsaParameters( new BigInteger( 1, dp.P ), new BigInteger( 1, dp.Q ), new BigInteger( 1, dp.G ), parameters1 );
            return new AsymmetricCipherKeyPair( new DsaPublicKeyParameters( new BigInteger( 1, dp.Y ), parameters2 ), new DsaPrivateKeyParameters( new BigInteger( 1, dp.X ), parameters2 ) );
        }

        public static DsaPublicKeyParameters GetDsaPublicKey( DSA dsa ) => GetDsaPublicKey( dsa.ExportParameters( false ) );

        public static DsaPublicKeyParameters GetDsaPublicKey( DSAParameters dp )
        {
            DsaValidationParameters parameters1 = dp.Seed != null ? new DsaValidationParameters( dp.Seed, dp.Counter ) : null;
            DsaParameters parameters2 = new DsaParameters( new BigInteger( 1, dp.P ), new BigInteger( 1, dp.Q ), new BigInteger( 1, dp.G ), parameters1 );
            return new DsaPublicKeyParameters( new BigInteger( 1, dp.Y ), parameters2 );
        }

        public static AsymmetricCipherKeyPair GetRsaKeyPair( RSA rsa ) => GetRsaKeyPair( rsa.ExportParameters( true ) );

        public static AsymmetricCipherKeyPair GetRsaKeyPair( RSAParameters rp )
        {
            BigInteger modulus = new BigInteger( 1, rp.Modulus );
            BigInteger bigInteger = new BigInteger( 1, rp.Exponent );
            return new AsymmetricCipherKeyPair( new RsaKeyParameters( false, modulus, bigInteger ), new RsaPrivateCrtKeyParameters( modulus, bigInteger, new BigInteger( 1, rp.D ), new BigInteger( 1, rp.P ), new BigInteger( 1, rp.Q ), new BigInteger( 1, rp.DP ), new BigInteger( 1, rp.DQ ), new BigInteger( 1, rp.InverseQ ) ) );
        }

        public static RsaKeyParameters GetRsaPublicKey( RSA rsa ) => GetRsaPublicKey( rsa.ExportParameters( false ) );

        public static RsaKeyParameters GetRsaPublicKey( RSAParameters rp ) => new RsaKeyParameters( false, new BigInteger( 1, rp.Modulus ), new BigInteger( 1, rp.Exponent ) );

        public static AsymmetricCipherKeyPair GetKeyPair( AsymmetricAlgorithm privateKey )
        {
            switch (privateKey)
            {
                case DSA _:
                    return GetDsaKeyPair( (DSA)privateKey );
                case RSA _:
                    return GetRsaKeyPair( (RSA)privateKey );
                default:
                    throw new ArgumentException( "Unsupported algorithm specified", nameof( privateKey ) );
            }
        }

        public static RSA ToRSA( RsaKeyParameters rsaKey ) => CreateRSAProvider( ToRSAParameters( rsaKey ) );

        public static RSA ToRSA( RsaPrivateCrtKeyParameters privKey ) => CreateRSAProvider( ToRSAParameters( privKey ) );

        public static RSA ToRSA( RsaPrivateKeyStructure privKey ) => CreateRSAProvider( ToRSAParameters( privKey ) );

        public static RSAParameters ToRSAParameters( RsaKeyParameters rsaKey )
        {
            RSAParameters rsaParameters = new RSAParameters
            {
                Modulus = rsaKey.Modulus.ToByteArrayUnsigned()
            };
            if (rsaKey.IsPrivate)
                rsaParameters.D = ConvertRSAParametersField( rsaKey.Exponent, rsaParameters.Modulus.Length );
            else
                rsaParameters.Exponent = rsaKey.Exponent.ToByteArrayUnsigned();
            return rsaParameters;
        }

        public static RSAParameters ToRSAParameters( RsaPrivateCrtKeyParameters privKey )
        {
            RSAParameters rsaParameters = new RSAParameters()
            {
                Modulus = privKey.Modulus.ToByteArrayUnsigned(),
                Exponent = privKey.PublicExponent.ToByteArrayUnsigned(),
                P = privKey.P.ToByteArrayUnsigned(),
                Q = privKey.Q.ToByteArrayUnsigned()
            };
            rsaParameters.D = ConvertRSAParametersField( privKey.Exponent, rsaParameters.Modulus.Length );
            rsaParameters.DP = ConvertRSAParametersField( privKey.DP, rsaParameters.P.Length );
            rsaParameters.DQ = ConvertRSAParametersField( privKey.DQ, rsaParameters.Q.Length );
            rsaParameters.InverseQ = ConvertRSAParametersField( privKey.QInv, rsaParameters.Q.Length );
            return rsaParameters;
        }

        public static RSAParameters ToRSAParameters( RsaPrivateKeyStructure privKey )
        {
            RSAParameters rsaParameters = new RSAParameters()
            {
                Modulus = privKey.Modulus.ToByteArrayUnsigned(),
                Exponent = privKey.PublicExponent.ToByteArrayUnsigned(),
                P = privKey.Prime1.ToByteArrayUnsigned(),
                Q = privKey.Prime2.ToByteArrayUnsigned()
            };
            rsaParameters.D = ConvertRSAParametersField( privKey.PrivateExponent, rsaParameters.Modulus.Length );
            rsaParameters.DP = ConvertRSAParametersField( privKey.Exponent1, rsaParameters.P.Length );
            rsaParameters.DQ = ConvertRSAParametersField( privKey.Exponent2, rsaParameters.Q.Length );
            rsaParameters.InverseQ = ConvertRSAParametersField( privKey.Coefficient, rsaParameters.Q.Length );
            return rsaParameters;
        }

        private static byte[] ConvertRSAParametersField( BigInteger n, int size )
        {
            byte[] byteArrayUnsigned = n.ToByteArrayUnsigned();
            if (byteArrayUnsigned.Length == size)
                return byteArrayUnsigned;
            if (byteArrayUnsigned.Length > size)
                throw new ArgumentException( "Specified size too small", nameof( size ) );
            byte[] destinationArray = new byte[size];
            Array.Copy( byteArrayUnsigned, 0, destinationArray, size - byteArrayUnsigned.Length, byteArrayUnsigned.Length );
            return destinationArray;
        }

        private static RSA CreateRSAProvider( RSAParameters rp )
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider( new CspParameters()
            {
                KeyContainerName = string.Format( "BouncyCastle-{0}", Guid.NewGuid() )
            } );
            rsaProvider.ImportParameters( rp );
            return rsaProvider;
        }
    }
}
