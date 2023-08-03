// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.PrivateKeyInfoFactory
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
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Pkcs
{
    public sealed class PrivateKeyInfoFactory
    {
        private PrivateKeyInfoFactory()
        {
        }

        public static PrivateKeyInfo CreatePrivateKeyInfo( AsymmetricKeyParameter key )
        {
            if (key == null)
                throw new ArgumentNullException( nameof( key ) );
            if (!key.IsPrivate)
                throw new ArgumentException( "Public key passed - private key expected", nameof( key ) );
            switch (key)
            {
                case ElGamalPrivateKeyParameters _:
                    ElGamalPrivateKeyParameters privateKeyParameters1 = (ElGamalPrivateKeyParameters)key;
                    return new PrivateKeyInfo( new AlgorithmIdentifier( OiwObjectIdentifiers.ElGamalAlgorithm, new ElGamalParameter( privateKeyParameters1.Parameters.P, privateKeyParameters1.Parameters.G ).ToAsn1Object() ), new DerInteger( privateKeyParameters1.X ) );
                case DsaPrivateKeyParameters _:
                    DsaPrivateKeyParameters privateKeyParameters2 = (DsaPrivateKeyParameters)key;
                    return new PrivateKeyInfo( new AlgorithmIdentifier( X9ObjectIdentifiers.IdDsa, new DsaParameter( privateKeyParameters2.Parameters.P, privateKeyParameters2.Parameters.Q, privateKeyParameters2.Parameters.G ).ToAsn1Object() ), new DerInteger( privateKeyParameters2.X ) );
                case DHPrivateKeyParameters _:
                    DHPrivateKeyParameters privateKeyParameters3 = (DHPrivateKeyParameters)key;
                    DHParameter dhParameter = new( privateKeyParameters3.Parameters.P, privateKeyParameters3.Parameters.G, privateKeyParameters3.Parameters.L );
                    return new PrivateKeyInfo( new AlgorithmIdentifier( privateKeyParameters3.AlgorithmOid, dhParameter.ToAsn1Object() ), new DerInteger( privateKeyParameters3.X ) );
                case RsaKeyParameters _:
                    AlgorithmIdentifier algID1 = new( PkcsObjectIdentifiers.RsaEncryption, DerNull.Instance );
                    RsaPrivateKeyStructure privateKeyStructure;
                    if (key is RsaPrivateCrtKeyParameters)
                    {
                        RsaPrivateCrtKeyParameters crtKeyParameters = (RsaPrivateCrtKeyParameters)key;
                        privateKeyStructure = new RsaPrivateKeyStructure( crtKeyParameters.Modulus, crtKeyParameters.PublicExponent, crtKeyParameters.Exponent, crtKeyParameters.P, crtKeyParameters.Q, crtKeyParameters.DP, crtKeyParameters.DQ, crtKeyParameters.QInv );
                    }
                    else
                    {
                        RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)key;
                        privateKeyStructure = new RsaPrivateKeyStructure( rsaKeyParameters.Modulus, BigInteger.Zero, rsaKeyParameters.Exponent, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero );
                    }
                    return new PrivateKeyInfo( algID1, privateKeyStructure.ToAsn1Object() );
                case ECPrivateKeyParameters _:
                    ECPrivateKeyParameters privateKeyParameters4 = (ECPrivateKeyParameters)key;
                    ECDomainParameters parameters1 = privateKeyParameters4.Parameters;
                    int bitLength = parameters1.N.BitLength;
                    AlgorithmIdentifier algID2;
                    ECPrivateKeyStructure privateKey;
                    if (privateKeyParameters4.AlgorithmName == "ECGOST3410")
                    {
                        if (privateKeyParameters4.PublicKeyParamSet == null)
                            throw Platform.CreateNotImplementedException( "Not a CryptoPro parameter set" );
                        Gost3410PublicKeyAlgParameters parameters2 = new( privateKeyParameters4.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet );
                        algID2 = new AlgorithmIdentifier( CryptoProObjectIdentifiers.GostR3410x2001, parameters2 );
                        privateKey = new ECPrivateKeyStructure( bitLength, privateKeyParameters4.D );
                    }
                    else
                    {
                        X962Parameters parameters3 = privateKeyParameters4.PublicKeyParamSet != null ? new X962Parameters( privateKeyParameters4.PublicKeyParamSet ) : new X962Parameters( new X9ECParameters( parameters1.Curve, parameters1.G, parameters1.N, parameters1.H, parameters1.GetSeed() ) );
                        privateKey = new ECPrivateKeyStructure( bitLength, privateKeyParameters4.D, parameters3 );
                        algID2 = new AlgorithmIdentifier( X9ObjectIdentifiers.IdECPublicKey, parameters3 );
                    }
                    return new PrivateKeyInfo( algID2, privateKey );
                case Gost3410PrivateKeyParameters _:
                    Gost3410PrivateKeyParameters privateKeyParameters5 = (Gost3410PrivateKeyParameters)key;
                    byte[] numArray = privateKeyParameters5.PublicKeyParamSet != null ? privateKeyParameters5.X.ToByteArrayUnsigned() : throw Platform.CreateNotImplementedException( "Not a CryptoPro parameter set" );
                    byte[] str = new byte[numArray.Length];
                    for (int index = 0; index != str.Length; ++index)
                        str[index] = numArray[numArray.Length - 1 - index];
                    Gost3410PublicKeyAlgParameters keyAlgParameters = new( privateKeyParameters5.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet, null );
                    return new PrivateKeyInfo( new AlgorithmIdentifier( CryptoProObjectIdentifiers.GostR3410x94, keyAlgParameters.ToAsn1Object() ), new DerOctetString( str ) );
                default:
                    throw new ArgumentException( "Class provided is not convertible: " + Platform.GetTypeName( key ) );
            }
        }

        public static PrivateKeyInfo CreatePrivateKeyInfo(
          char[] passPhrase,
          EncryptedPrivateKeyInfo encInfo )
        {
            return CreatePrivateKeyInfo( passPhrase, false, encInfo );
        }

        public static PrivateKeyInfo CreatePrivateKeyInfo(
          char[] passPhrase,
          bool wrongPkcs12Zero,
          EncryptedPrivateKeyInfo encInfo )
        {
            AlgorithmIdentifier encryptionAlgorithm = encInfo.EncryptionAlgorithm;
            if (!(PbeUtilities.CreateEngine( encryptionAlgorithm ) is IBufferedCipher engine))
                throw new Exception( "Unknown encryption algorithm: " + encryptionAlgorithm.Algorithm );
            ICipherParameters cipherParameters = PbeUtilities.GenerateCipherParameters( encryptionAlgorithm, passPhrase, wrongPkcs12Zero );
            engine.Init( false, cipherParameters );
            return PrivateKeyInfo.GetInstance( engine.DoFinal( encInfo.GetEncryptedData() ) );
        }
    }
}
