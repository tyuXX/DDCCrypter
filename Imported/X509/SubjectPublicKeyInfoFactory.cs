// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.SubjectPublicKeyInfoFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.X509
{
    public sealed class SubjectPublicKeyInfoFactory
    {
        private SubjectPublicKeyInfoFactory()
        {
        }

        public static SubjectPublicKeyInfo CreateSubjectPublicKeyInfo( AsymmetricKeyParameter key )
        {
            if (key == null)
                throw new ArgumentNullException( nameof( key ) );
            if (key.IsPrivate)
                throw new ArgumentException( "Private key passed - public key expected.", nameof( key ) );
            switch (key)
            {
                case ElGamalPublicKeyParameters _:
                    ElGamalPublicKeyParameters publicKeyParameters1 = (ElGamalPublicKeyParameters)key;
                    ElGamalParameters parameters1 = publicKeyParameters1.Parameters;
                    return new SubjectPublicKeyInfo( new AlgorithmIdentifier( OiwObjectIdentifiers.ElGamalAlgorithm, new ElGamalParameter( parameters1.P, parameters1.G ).ToAsn1Object() ), new DerInteger( publicKeyParameters1.Y ) );
                case DsaPublicKeyParameters _:
                    DsaPublicKeyParameters publicKeyParameters2 = (DsaPublicKeyParameters)key;
                    DsaParameters parameters2 = publicKeyParameters2.Parameters;
                    Asn1Encodable asn1Object1 = parameters2 == null ? null : (Asn1Encodable)new DsaParameter( parameters2.P, parameters2.Q, parameters2.G ).ToAsn1Object();
                    return new SubjectPublicKeyInfo( new AlgorithmIdentifier( X9ObjectIdentifiers.IdDsa, asn1Object1 ), new DerInteger( publicKeyParameters2.Y ) );
                case DHPublicKeyParameters _:
                    DHPublicKeyParameters publicKeyParameters3 = (DHPublicKeyParameters)key;
                    DHParameters parameters3 = publicKeyParameters3.Parameters;
                    return new SubjectPublicKeyInfo( new AlgorithmIdentifier( publicKeyParameters3.AlgorithmOid, new DHParameter( parameters3.P, parameters3.G, parameters3.L ).ToAsn1Object() ), new DerInteger( publicKeyParameters3.Y ) );
                case RsaKeyParameters _:
                    RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)key;
                    return new SubjectPublicKeyInfo( new AlgorithmIdentifier( PkcsObjectIdentifiers.RsaEncryption, DerNull.Instance ), new RsaPublicKeyStructure( rsaKeyParameters.Modulus, rsaKeyParameters.Exponent ).ToAsn1Object() );
                case ECPublicKeyParameters _:
                    ECPublicKeyParameters publicKeyParameters4 = (ECPublicKeyParameters)key;
                    if (publicKeyParameters4.AlgorithmName == "ECGOST3410")
                    {
                        ECPoint ecPoint = publicKeyParameters4.PublicKeyParamSet != null ? publicKeyParameters4.Q.Normalize() : throw Platform.CreateNotImplementedException( "Not a CryptoPro parameter set" );
                        BigInteger bigInteger1 = ecPoint.AffineXCoord.ToBigInteger();
                        BigInteger bigInteger2 = ecPoint.AffineYCoord.ToBigInteger();
                        byte[] numArray = new byte[64];
                        ExtractBytes( numArray, 0, bigInteger1 );
                        ExtractBytes( numArray, 32, bigInteger2 );
                        Gost3410PublicKeyAlgParameters keyAlgParameters = new( publicKeyParameters4.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet );
                        return new SubjectPublicKeyInfo( new AlgorithmIdentifier( CryptoProObjectIdentifiers.GostR3410x2001, keyAlgParameters.ToAsn1Object() ), new DerOctetString( numArray ) );
                    }
                    X962Parameters x962Parameters;
                    if (publicKeyParameters4.PublicKeyParamSet == null)
                    {
                        ECDomainParameters parameters4 = publicKeyParameters4.Parameters;
                        x962Parameters = new X962Parameters( new X9ECParameters( parameters4.Curve, parameters4.G, parameters4.N, parameters4.H, parameters4.GetSeed() ) );
                    }
                    else
                        x962Parameters = new X962Parameters( publicKeyParameters4.PublicKeyParamSet );
                    Asn1OctetString asn1Object2 = (Asn1OctetString)new X9ECPoint( publicKeyParameters4.Q ).ToAsn1Object();
                    return new SubjectPublicKeyInfo( new AlgorithmIdentifier( X9ObjectIdentifiers.IdECPublicKey, x962Parameters.ToAsn1Object() ), asn1Object2.GetOctets() );
                case Gost3410PublicKeyParameters _:
                    Gost3410PublicKeyParameters publicKeyParameters5 = (Gost3410PublicKeyParameters)key;
                    byte[] numArray1 = publicKeyParameters5.PublicKeyParamSet != null ? publicKeyParameters5.Y.ToByteArrayUnsigned() : throw Platform.CreateNotImplementedException( "Not a CryptoPro parameter set" );
                    byte[] str = new byte[numArray1.Length];
                    for (int index = 0; index != str.Length; ++index)
                        str[index] = numArray1[numArray1.Length - 1 - index];
                    Gost3410PublicKeyAlgParameters keyAlgParameters1 = new( publicKeyParameters5.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet );
                    return new SubjectPublicKeyInfo( new AlgorithmIdentifier( CryptoProObjectIdentifiers.GostR3410x94, keyAlgParameters1.ToAsn1Object() ), new DerOctetString( str ) );
                default:
                    throw new ArgumentException( "Class provided no convertible: " + Platform.GetTypeName( key ) );
            }
        }

        private static void ExtractBytes( byte[] encKey, int offset, BigInteger bI )
        {
            byte[] byteArray = bI.ToByteArray();
            int num = (bI.BitLength + 7) / 8;
            for (int index = 0; index < num; ++index)
                encKey[offset + index] = byteArray[byteArray.Length - 1 - index];
        }
    }
}
