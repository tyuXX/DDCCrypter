// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.PrivateKeyFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Security
{
    public sealed class PrivateKeyFactory
    {
        private PrivateKeyFactory()
        {
        }

        public static AsymmetricKeyParameter CreateKey( byte[] privateKeyInfoData ) => CreateKey( PrivateKeyInfo.GetInstance( Asn1Object.FromByteArray( privateKeyInfoData ) ) );

        public static AsymmetricKeyParameter CreateKey( Stream inStr ) => CreateKey( PrivateKeyInfo.GetInstance( Asn1Object.FromStream( inStr ) ) );

        public static AsymmetricKeyParameter CreateKey( PrivateKeyInfo keyInfo )
        {
            AlgorithmIdentifier privateKeyAlgorithm = keyInfo.PrivateKeyAlgorithm;
            DerObjectIdentifier algorithm = privateKeyAlgorithm.Algorithm;
            if (algorithm.Equals( PkcsObjectIdentifiers.RsaEncryption ) || algorithm.Equals( X509ObjectIdentifiers.IdEARsa ) || algorithm.Equals( PkcsObjectIdentifiers.IdRsassaPss ) || algorithm.Equals( PkcsObjectIdentifiers.IdRsaesOaep ))
            {
                RsaPrivateKeyStructure instance = RsaPrivateKeyStructure.GetInstance( keyInfo.ParsePrivateKey() );
                return new RsaPrivateCrtKeyParameters( instance.Modulus, instance.PublicExponent, instance.PrivateExponent, instance.Prime1, instance.Prime2, instance.Exponent1, instance.Exponent2, instance.Coefficient );
            }
            if (algorithm.Equals( PkcsObjectIdentifiers.DhKeyAgreement ))
            {
                DHParameter dhParameter = new( Asn1Sequence.GetInstance( privateKeyAlgorithm.Parameters.ToAsn1Object() ) );
                DerInteger privateKey = (DerInteger)keyInfo.ParsePrivateKey();
                BigInteger l = dhParameter.L;
                int intValue = l == null ? 0 : l.IntValue;
                DHParameters parameters = new( dhParameter.P, dhParameter.G, null, intValue );
                return new DHPrivateKeyParameters( privateKey.Value, parameters, algorithm );
            }
            if (algorithm.Equals( OiwObjectIdentifiers.ElGamalAlgorithm ))
            {
                ElGamalParameter elGamalParameter = new( Asn1Sequence.GetInstance( privateKeyAlgorithm.Parameters.ToAsn1Object() ) );
                return new ElGamalPrivateKeyParameters( ((DerInteger)keyInfo.ParsePrivateKey()).Value, new ElGamalParameters( elGamalParameter.P, elGamalParameter.G ) );
            }
            if (algorithm.Equals( X9ObjectIdentifiers.IdDsa ))
            {
                DerInteger privateKey = (DerInteger)keyInfo.ParsePrivateKey();
                Asn1Encodable parameters1 = privateKeyAlgorithm.Parameters;
                DsaParameters parameters2 = null;
                if (parameters1 != null)
                {
                    DsaParameter instance = DsaParameter.GetInstance( parameters1.ToAsn1Object() );
                    parameters2 = new DsaParameters( instance.P, instance.Q, instance.G );
                }
                return new DsaPrivateKeyParameters( privateKey.Value, parameters2 );
            }
            if (algorithm.Equals( X9ObjectIdentifiers.IdECPublicKey ))
            {
                X962Parameters x962Parameters = new( privateKeyAlgorithm.Parameters.ToAsn1Object() );
                X9ECParameters x9EcParameters = !x962Parameters.IsNamedCurve ? new X9ECParameters( (Asn1Sequence)x962Parameters.Parameters ) : ECKeyPairGenerator.FindECCurveByOid( (DerObjectIdentifier)x962Parameters.Parameters );
                BigInteger key = ECPrivateKeyStructure.GetInstance( keyInfo.ParsePrivateKey() ).GetKey();
                if (x962Parameters.IsNamedCurve)
                    return new ECPrivateKeyParameters( "EC", key, (DerObjectIdentifier)x962Parameters.Parameters );
                ECDomainParameters parameters = new( x9EcParameters.Curve, x9EcParameters.G, x9EcParameters.N, x9EcParameters.H, x9EcParameters.GetSeed() );
                return new ECPrivateKeyParameters( key, parameters );
            }
            if (algorithm.Equals( CryptoProObjectIdentifiers.GostR3410x2001 ))
            {
                Gost3410PublicKeyAlgParameters keyAlgParameters = new( Asn1Sequence.GetInstance( privateKeyAlgorithm.Parameters.ToAsn1Object() ) );
                ECDomainParameters byOid = ECGost3410NamedCurves.GetByOid( keyAlgParameters.PublicKeyParamSet );
                if (byOid == null)
                    throw new ArgumentException( "Unrecognized curve OID for GostR3410x2001 private key" );
                Asn1Object privateKey = keyInfo.ParsePrivateKey();
                return new ECPrivateKeyParameters( "ECGOST3410", (!(privateKey is DerInteger) ? ECPrivateKeyStructure.GetInstance( privateKey ) : new ECPrivateKeyStructure( byOid.N.BitLength, ((DerInteger)privateKey).Value )).GetKey(), keyAlgParameters.PublicKeyParamSet );
            }
            if (!algorithm.Equals( CryptoProObjectIdentifiers.GostR3410x94 ))
                throw new SecurityUtilityException( "algorithm identifier in key not recognised" );
            Gost3410PublicKeyAlgParameters keyAlgParameters1 = new( Asn1Sequence.GetInstance( privateKeyAlgorithm.Parameters.ToAsn1Object() ) );
            return new Gost3410PrivateKeyParameters( new BigInteger( 1, Arrays.Reverse( ((Asn1OctetString)keyInfo.ParsePrivateKey()).GetOctets() ) ), keyAlgParameters1.PublicKeyParamSet );
        }

        public static AsymmetricKeyParameter DecryptKey(
          char[] passPhrase,
          EncryptedPrivateKeyInfo encInfo )
        {
            return CreateKey( PrivateKeyInfoFactory.CreatePrivateKeyInfo( passPhrase, encInfo ) );
        }

        public static AsymmetricKeyParameter DecryptKey(
          char[] passPhrase,
          byte[] encryptedPrivateKeyInfoData )
        {
            return DecryptKey( passPhrase, Asn1Object.FromByteArray( encryptedPrivateKeyInfoData ) );
        }

        public static AsymmetricKeyParameter DecryptKey(
          char[] passPhrase,
          Stream encryptedPrivateKeyInfoStream )
        {
            return DecryptKey( passPhrase, Asn1Object.FromStream( encryptedPrivateKeyInfoStream ) );
        }

        private static AsymmetricKeyParameter DecryptKey( char[] passPhrase, Asn1Object asn1Object ) => DecryptKey( passPhrase, EncryptedPrivateKeyInfo.GetInstance( asn1Object ) );

        public static byte[] EncryptKey(
          DerObjectIdentifier algorithm,
          char[] passPhrase,
          byte[] salt,
          int iterationCount,
          AsymmetricKeyParameter key )
        {
            return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo( algorithm, passPhrase, salt, iterationCount, key ).GetEncoded();
        }

        public static byte[] EncryptKey(
          string algorithm,
          char[] passPhrase,
          byte[] salt,
          int iterationCount,
          AsymmetricKeyParameter key )
        {
            return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo( algorithm, passPhrase, salt, iterationCount, key ).GetEncoded();
        }
    }
}
