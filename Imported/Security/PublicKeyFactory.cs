// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.PublicKeyFactory
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
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.IO;

namespace Org.BouncyCastle.Security
{
    public sealed class PublicKeyFactory
    {
        private PublicKeyFactory()
        {
        }

        public static AsymmetricKeyParameter CreateKey( byte[] keyInfoData ) => CreateKey( SubjectPublicKeyInfo.GetInstance( Asn1Object.FromByteArray( keyInfoData ) ) );

        public static AsymmetricKeyParameter CreateKey( Stream inStr ) => CreateKey( SubjectPublicKeyInfo.GetInstance( Asn1Object.FromStream( inStr ) ) );

        public static AsymmetricKeyParameter CreateKey( SubjectPublicKeyInfo keyInfo )
        {
            AlgorithmIdentifier algorithmId = keyInfo.AlgorithmID;
            DerObjectIdentifier algorithm = algorithmId.Algorithm;
            if (algorithm.Equals( PkcsObjectIdentifiers.RsaEncryption ) || algorithm.Equals( X509ObjectIdentifiers.IdEARsa ) || algorithm.Equals( PkcsObjectIdentifiers.IdRsassaPss ) || algorithm.Equals( PkcsObjectIdentifiers.IdRsaesOaep ))
            {
                RsaPublicKeyStructure instance = RsaPublicKeyStructure.GetInstance( keyInfo.GetPublicKey() );
                return new RsaKeyParameters( false, instance.Modulus, instance.PublicExponent );
            }
            if (algorithm.Equals( X9ObjectIdentifiers.DHPublicNumber ))
            {
                Asn1Sequence instance1 = Asn1Sequence.GetInstance( algorithmId.Parameters.ToAsn1Object() );
                BigInteger y = DHPublicKey.GetInstance( keyInfo.GetPublicKey() ).Y.Value;
                if (IsPkcsDHParam( instance1 ))
                    return ReadPkcsDHParam( algorithm, y, instance1 );
                DHDomainParameters instance2 = DHDomainParameters.GetInstance( instance1 );
                BigInteger p = instance2.P.Value;
                BigInteger g = instance2.G.Value;
                BigInteger q = instance2.Q.Value;
                BigInteger j = null;
                if (instance2.J != null)
                    j = instance2.J.Value;
                DHValidationParameters validation = null;
                DHValidationParms validationParms = instance2.ValidationParms;
                if (validationParms != null)
                    validation = new DHValidationParameters( validationParms.Seed.GetBytes(), validationParms.PgenCounter.Value.IntValue );
                return new DHPublicKeyParameters( y, new DHParameters( p, g, q, j, validation ) );
            }
            if (algorithm.Equals( PkcsObjectIdentifiers.DhKeyAgreement ))
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance( algorithmId.Parameters.ToAsn1Object() );
                DerInteger publicKey = (DerInteger)keyInfo.GetPublicKey();
                return ReadPkcsDHParam( algorithm, publicKey.Value, instance );
            }
            if (algorithm.Equals( OiwObjectIdentifiers.ElGamalAlgorithm ))
            {
                ElGamalParameter elGamalParameter = new ElGamalParameter( Asn1Sequence.GetInstance( algorithmId.Parameters.ToAsn1Object() ) );
                return new ElGamalPublicKeyParameters( ((DerInteger)keyInfo.GetPublicKey()).Value, new ElGamalParameters( elGamalParameter.P, elGamalParameter.G ) );
            }
            if (algorithm.Equals( X9ObjectIdentifiers.IdDsa ) || algorithm.Equals( OiwObjectIdentifiers.DsaWithSha1 ))
            {
                DerInteger publicKey = (DerInteger)keyInfo.GetPublicKey();
                Asn1Encodable parameters1 = algorithmId.Parameters;
                DsaParameters parameters2 = null;
                if (parameters1 != null)
                {
                    DsaParameter instance = DsaParameter.GetInstance( parameters1.ToAsn1Object() );
                    parameters2 = new DsaParameters( instance.P, instance.Q, instance.G );
                }
                return new DsaPublicKeyParameters( publicKey.Value, parameters2 );
            }
            if (algorithm.Equals( X9ObjectIdentifiers.IdECPublicKey ))
            {
                X962Parameters x962Parameters = new X962Parameters( algorithmId.Parameters.ToAsn1Object() );
                X9ECParameters x9EcParameters = !x962Parameters.IsNamedCurve ? new X9ECParameters( (Asn1Sequence)x962Parameters.Parameters ) : ECKeyPairGenerator.FindECCurveByOid( (DerObjectIdentifier)x962Parameters.Parameters );
                Asn1OctetString s = new DerOctetString( keyInfo.PublicKeyData.GetBytes() );
                ECPoint point = new X9ECPoint( x9EcParameters.Curve, s ).Point;
                if (x962Parameters.IsNamedCurve)
                    return new ECPublicKeyParameters( "EC", point, (DerObjectIdentifier)x962Parameters.Parameters );
                ECDomainParameters parameters = new ECDomainParameters( x9EcParameters.Curve, x9EcParameters.G, x9EcParameters.N, x9EcParameters.H, x9EcParameters.GetSeed() );
                return new ECPublicKeyParameters( point, parameters );
            }
            if (algorithm.Equals( CryptoProObjectIdentifiers.GostR3410x2001 ))
            {
                Gost3410PublicKeyAlgParameters keyAlgParameters = new Gost3410PublicKeyAlgParameters( (Asn1Sequence)algorithmId.Parameters );
                Asn1OctetString publicKey;
                try
                {
                    publicKey = (Asn1OctetString)keyInfo.GetPublicKey();
                }
                catch (IOException ex)
                {
                    throw new ArgumentException( "invalid info structure in GOST3410 public key" );
                }
                byte[] octets = publicKey.GetOctets();
                byte[] bytes1 = new byte[32];
                byte[] bytes2 = new byte[32];
                for (int index = 0; index != bytes2.Length; ++index)
                    bytes1[index] = octets[31 - index];
                for (int index = 0; index != bytes1.Length; ++index)
                    bytes2[index] = octets[63 - index];
                ECDomainParameters byOid = ECGost3410NamedCurves.GetByOid( keyAlgParameters.PublicKeyParamSet );
                return byOid == null ? null : (AsymmetricKeyParameter)new ECPublicKeyParameters( "ECGOST3410", byOid.Curve.CreatePoint( new BigInteger( 1, bytes1 ), new BigInteger( 1, bytes2 ) ), keyAlgParameters.PublicKeyParamSet );
            }
            if (!algorithm.Equals( CryptoProObjectIdentifiers.GostR3410x94 ))
                throw new SecurityUtilityException( "algorithm identifier in key not recognised: " + algorithm );
            Gost3410PublicKeyAlgParameters keyAlgParameters1 = new Gost3410PublicKeyAlgParameters( (Asn1Sequence)algorithmId.Parameters );
            DerOctetString publicKey1;
            try
            {
                publicKey1 = (DerOctetString)keyInfo.GetPublicKey();
            }
            catch (IOException ex)
            {
                throw new ArgumentException( "invalid info structure in GOST3410 public key" );
            }
            byte[] octets1 = publicKey1.GetOctets();
            byte[] bytes = new byte[octets1.Length];
            for (int index = 0; index != octets1.Length; ++index)
                bytes[index] = octets1[octets1.Length - 1 - index];
            return new Gost3410PublicKeyParameters( new BigInteger( 1, bytes ), keyAlgParameters1.PublicKeyParamSet );
        }

        private static bool IsPkcsDHParam( Asn1Sequence seq )
        {
            if (seq.Count == 2)
                return true;
            return seq.Count <= 3 && DerInteger.GetInstance( seq[2] ).Value.CompareTo( BigInteger.ValueOf( DerInteger.GetInstance( seq[0] ).Value.BitLength ) ) <= 0;
        }

        private static DHPublicKeyParameters ReadPkcsDHParam(
          DerObjectIdentifier algOid,
          BigInteger y,
          Asn1Sequence seq )
        {
            DHParameter dhParameter = new DHParameter( seq );
            BigInteger l = dhParameter.L;
            int intValue = l == null ? 0 : l.IntValue;
            DHParameters parameters = new DHParameters( dhParameter.P, dhParameter.G, null, intValue );
            return new DHPublicKeyParameters( y, parameters, algOid );
        }
    }
}
