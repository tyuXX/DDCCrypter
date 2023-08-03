// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsEccUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.Field;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsEccUtilities
    {
        private static readonly string[] CurveNames = new string[28]
        {
      "sect163k1",
      "sect163r1",
      "sect163r2",
      "sect193r1",
      "sect193r2",
      "sect233k1",
      "sect233r1",
      "sect239k1",
      "sect283k1",
      "sect283r1",
      "sect409k1",
      "sect409r1",
      "sect571k1",
      "sect571r1",
      "secp160k1",
      "secp160r1",
      "secp160r2",
      "secp192k1",
      "secp192r1",
      "secp224k1",
      "secp224r1",
      "secp256k1",
      "secp256r1",
      "secp384r1",
      "secp521r1",
      "brainpoolP256r1",
      "brainpoolP384r1",
      "brainpoolP512r1"
        };

        public static void AddSupportedEllipticCurvesExtension(
          IDictionary extensions,
          int[] namedCurves )
        {
            extensions[10] = CreateSupportedEllipticCurvesExtension( namedCurves );
        }

        public static void AddSupportedPointFormatsExtension(
          IDictionary extensions,
          byte[] ecPointFormats )
        {
            extensions[11] = CreateSupportedPointFormatsExtension( ecPointFormats );
        }

        public static int[] GetSupportedEllipticCurvesExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 10 );
            return extensionData != null ? ReadSupportedEllipticCurvesExtension( extensionData ) : null;
        }

        public static byte[] GetSupportedPointFormatsExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 11 );
            return extensionData != null ? ReadSupportedPointFormatsExtension( extensionData ) : null;
        }

        public static byte[] CreateSupportedEllipticCurvesExtension( int[] namedCurves ) => namedCurves != null && namedCurves.Length >= 1 ? TlsUtilities.EncodeUint16ArrayWithUint16Length( namedCurves ) : throw new TlsFatalAlert( 80 );

        public static byte[] CreateSupportedPointFormatsExtension( byte[] ecPointFormats )
        {
            if (ecPointFormats == null || !Arrays.Contains( ecPointFormats, 0 ))
                ecPointFormats = Arrays.Append( ecPointFormats, 0 );
            return TlsUtilities.EncodeUint8ArrayWithUint8Length( ecPointFormats );
        }

        public static int[] ReadSupportedEllipticCurvesExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            int num = TlsUtilities.ReadUint16( memoryStream );
            int[] numArray = num >= 2 && (num & 1) == 0 ? TlsUtilities.ReadUint16Array( num / 2, memoryStream ) : throw new TlsFatalAlert( 50 );
            TlsProtocol.AssertEmpty( memoryStream );
            return numArray;
        }

        public static byte[] ReadSupportedPointFormatsExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            byte count = TlsUtilities.ReadUint8( memoryStream );
            byte[] a = count >= 1 ? TlsUtilities.ReadUint8Array( count, memoryStream ) : throw new TlsFatalAlert( 50 );
            TlsProtocol.AssertEmpty( memoryStream );
            return Arrays.Contains( a, 0 ) ? a : throw new TlsFatalAlert( 47 );
        }

        public static string GetNameOfNamedCurve( int namedCurve ) => !IsSupportedNamedCurve( namedCurve ) ? null : CurveNames[namedCurve - 1];

        public static ECDomainParameters GetParametersForNamedCurve( int namedCurve )
        {
            string nameOfNamedCurve = GetNameOfNamedCurve( namedCurve );
            if (nameOfNamedCurve == null)
                return null;
            X9ECParameters byName = CustomNamedCurves.GetByName( nameOfNamedCurve );
            if (byName == null)
            {
                byName = ECNamedCurveTable.GetByName( nameOfNamedCurve );
                if (byName == null)
                    return null;
            }
            return new ECDomainParameters( byName.Curve, byName.G, byName.N, byName.H, byName.GetSeed() );
        }

        public static bool HasAnySupportedNamedCurves() => CurveNames.Length > 0;

        public static bool ContainsEccCipherSuites( int[] cipherSuites )
        {
            for (int index = 0; index < cipherSuites.Length; ++index)
            {
                if (IsEccCipherSuite( cipherSuites[index] ))
                    return true;
            }
            return false;
        }

        public static bool IsEccCipherSuite( int cipherSuite )
        {
            switch (cipherSuite)
            {
                case 49153:
                case 49154:
                case 49155:
                case 49156:
                case 49157:
                case 49158:
                case 49159:
                case 49160:
                case 49161:
                case 49162:
                case 49163:
                case 49164:
                case 49165:
                case 49166:
                case 49167:
                case 49168:
                case 49169:
                case 49170:
                case 49171:
                case 49172:
                case 49173:
                case 49174:
                case 49175:
                case 49176:
                case 49177:
                case 49187:
                case 49188:
                case 49189:
                case 49190:
                case 49191:
                case 49192:
                case 49193:
                case 49194:
                case 49195:
                case 49196:
                case 49197:
                case 49198:
                case 49199:
                case 49200:
                case 49201:
                case 49202:
                case 49203:
                case 49204:
                case 49205:
                case 49206:
                case 49207:
                case 49208:
                case 49209:
                case 49210:
                case 49211:
                case 49266:
                case 49267:
                case 49268:
                case 49269:
                case 49270:
                case 49271:
                case 49272:
                case 49273:
                case 49286:
                case 49287:
                case 49288:
                case 49289:
                case 49290:
                case 49291:
                case 49292:
                case 49293:
                case 49306:
                case 49307:
                case 49324:
                case 49325:
                case 49326:
                case 49327:
                case 52243:
                case 52244:
                case 58386:
                case 58387:
                case 58388:
                case 58389:
                case 58392:
                case 58393:
                    return true;
                default:
                    return false;
            }
        }

        public static bool AreOnSameCurve( ECDomainParameters a, ECDomainParameters b ) => a.Curve.Equals( b.Curve ) && a.G.Equals( b.G ) && a.N.Equals( b.N ) && a.H.Equals( b.H );

        public static bool IsSupportedNamedCurve( int namedCurve ) => namedCurve > 0 && namedCurve <= CurveNames.Length;

        public static bool IsCompressionPreferred( byte[] ecPointFormats, byte compressionFormat )
        {
            if (ecPointFormats == null)
                return false;
            for (int index = 0; index < ecPointFormats.Length; ++index)
            {
                byte ecPointFormat = ecPointFormats[index];
                if (ecPointFormat == 0)
                    return false;
                if (ecPointFormat == compressionFormat)
                    return true;
            }
            return false;
        }

        public static byte[] SerializeECFieldElement( int fieldSize, BigInteger x ) => BigIntegers.AsUnsignedByteArray( (fieldSize + 7) / 8, x );

        public static byte[] SerializeECPoint( byte[] ecPointFormats, ECPoint point )
        {
            ECCurve curve = point.Curve;
            bool compressed = false;
            if (ECAlgorithms.IsFpCurve( curve ))
                compressed = IsCompressionPreferred( ecPointFormats, 1 );
            else if (ECAlgorithms.IsF2mCurve( curve ))
                compressed = IsCompressionPreferred( ecPointFormats, 2 );
            return point.GetEncoded( compressed );
        }

        public static byte[] SerializeECPublicKey(
          byte[] ecPointFormats,
          ECPublicKeyParameters keyParameters )
        {
            return SerializeECPoint( ecPointFormats, keyParameters.Q );
        }

        public static BigInteger DeserializeECFieldElement( int fieldSize, byte[] encoding )
        {
            int num = (fieldSize + 7) / 8;
            if (encoding.Length != num)
                throw new TlsFatalAlert( 50 );
            return new BigInteger( 1, encoding );
        }

        public static ECPoint DeserializeECPoint( byte[] ecPointFormats, ECCurve curve, byte[] encoding )
        {
            if (encoding == null || encoding.Length < 1)
                throw new TlsFatalAlert( 47 );
            byte n;
            switch (encoding[0])
            {
                case 2:
                case 3:
                    if (ECAlgorithms.IsF2mCurve( curve ))
                    {
                        n = 2;
                        break;
                    }
                    if (!ECAlgorithms.IsFpCurve( curve ))
                        throw new TlsFatalAlert( 47 );
                    n = 1;
                    break;
                case 4:
                    n = 0;
                    break;
                default:
                    throw new TlsFatalAlert( 47 );
            }
            if (n != 0 && (ecPointFormats == null || !Arrays.Contains( ecPointFormats, n )))
                throw new TlsFatalAlert( 47 );
            return curve.DecodePoint( encoding );
        }

        public static ECPublicKeyParameters DeserializeECPublicKey(
          byte[] ecPointFormats,
          ECDomainParameters curve_params,
          byte[] encoding )
        {
            try
            {
                return new ECPublicKeyParameters( DeserializeECPoint( ecPointFormats, curve_params.Curve, encoding ), curve_params );
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 47, ex );
            }
        }

        public static byte[] CalculateECDHBasicAgreement(
          ECPublicKeyParameters publicKey,
          ECPrivateKeyParameters privateKey )
        {
            ECDHBasicAgreement ecdhBasicAgreement = new();
            ecdhBasicAgreement.Init( privateKey );
            BigInteger agreement = ecdhBasicAgreement.CalculateAgreement( publicKey );
            return BigIntegers.AsUnsignedByteArray( ecdhBasicAgreement.GetFieldSize(), agreement );
        }

        public static AsymmetricCipherKeyPair GenerateECKeyPair(
          SecureRandom random,
          ECDomainParameters ecParams )
        {
            ECKeyPairGenerator keyPairGenerator = new();
            keyPairGenerator.Init( new ECKeyGenerationParameters( ecParams, random ) );
            return keyPairGenerator.GenerateKeyPair();
        }

        public static ECPrivateKeyParameters GenerateEphemeralClientKeyExchange(
          SecureRandom random,
          byte[] ecPointFormats,
          ECDomainParameters ecParams,
          Stream output )
        {
            AsymmetricCipherKeyPair ecKeyPair = GenerateECKeyPair( random, ecParams );
            ECPublicKeyParameters publicKeyParameters = (ECPublicKeyParameters)ecKeyPair.Public;
            WriteECPoint( ecPointFormats, publicKeyParameters.Q, output );
            return (ECPrivateKeyParameters)ecKeyPair.Private;
        }

        internal static ECPrivateKeyParameters GenerateEphemeralServerKeyExchange(
          SecureRandom random,
          int[] namedCurves,
          byte[] ecPointFormats,
          Stream output )
        {
            int namedCurve1 = -1;
            if (namedCurves == null)
            {
                namedCurve1 = 23;
            }
            else
            {
                for (int index = 0; index < namedCurves.Length; ++index)
                {
                    int namedCurve2 = namedCurves[index];
                    if (NamedCurve.IsValid( namedCurve2 ) && IsSupportedNamedCurve( namedCurve2 ))
                    {
                        namedCurve1 = namedCurve2;
                        break;
                    }
                }
            }
            ECDomainParameters domainParameters = null;
            if (namedCurve1 >= 0)
                domainParameters = GetParametersForNamedCurve( namedCurve1 );
            else if (Arrays.Contains( namedCurves, 65281 ))
                domainParameters = GetParametersForNamedCurve( 23 );
            else if (Arrays.Contains( namedCurves, 65282 ))
                domainParameters = GetParametersForNamedCurve( 10 );
            if (domainParameters == null)
                throw new TlsFatalAlert( 80 );
            if (namedCurve1 < 0)
                WriteExplicitECParameters( ecPointFormats, domainParameters, output );
            else
                WriteNamedECParameters( namedCurve1, output );
            return GenerateEphemeralClientKeyExchange( random, ecPointFormats, domainParameters, output );
        }

        public static ECPublicKeyParameters ValidateECPublicKey( ECPublicKeyParameters key ) => key;

        public static int ReadECExponent( int fieldSize, Stream input )
        {
            BigInteger bigInteger = ReadECParameter( input );
            int num = bigInteger.BitLength < 32 ? bigInteger.IntValue : throw new TlsFatalAlert( 47 );
            if (num > 0 && num < fieldSize)
                return num;
            throw new ArgumentException( "Error" );
        }

        public static BigInteger ReadECFieldElement( int fieldSize, Stream input ) => DeserializeECFieldElement( fieldSize, TlsUtilities.ReadOpaque8( input ) );

        public static BigInteger ReadECParameter( Stream input ) => new( 1, TlsUtilities.ReadOpaque8( input ) );

        public static ECDomainParameters ReadECParameters(
          int[] namedCurves,
          byte[] ecPointFormats,
          Stream input )
        {
            try
            {
                switch (TlsUtilities.ReadUint8( input ))
                {
                    case 1:
                        CheckNamedCurve( namedCurves, 65281 );
                        BigInteger q = ReadECParameter( input );
                        BigInteger a1 = ReadECFieldElement( q.BitLength, input );
                        BigInteger b1 = ReadECFieldElement( q.BitLength, input );
                        byte[] encoding1 = TlsUtilities.ReadOpaque8( input );
                        BigInteger bigInteger1 = ReadECParameter( input );
                        BigInteger bigInteger2 = ReadECParameter( input );
                        ECCurve curve1 = new FpCurve( q, a1, b1, bigInteger1, bigInteger2 );
                        ECPoint g1 = DeserializeECPoint( ecPointFormats, curve1, encoding1 );
                        return new ECDomainParameters( curve1, g1, bigInteger1, bigInteger2 );
                    case 2:
                        CheckNamedCurve( namedCurves, 65282 );
                        int num1 = TlsUtilities.ReadUint16( input );
                        byte ecBasisType = TlsUtilities.ReadUint8( input );
                        if (!ECBasisType.IsValid( ecBasisType ))
                            throw new TlsFatalAlert( 47 );
                        int num2 = ReadECExponent( num1, input );
                        int k2 = -1;
                        int k3 = -1;
                        if (ecBasisType == 2)
                        {
                            k2 = ReadECExponent( num1, input );
                            k3 = ReadECExponent( num1, input );
                        }
                        BigInteger a2 = ReadECFieldElement( num1, input );
                        BigInteger b2 = ReadECFieldElement( num1, input );
                        byte[] encoding2 = TlsUtilities.ReadOpaque8( input );
                        BigInteger bigInteger3 = ReadECParameter( input );
                        BigInteger bigInteger4 = ReadECParameter( input );
                        ECCurve curve2 = ecBasisType == 2 ? new F2mCurve( num1, num2, k2, k3, a2, b2, bigInteger3, bigInteger4 ) : (ECCurve)new F2mCurve( num1, num2, a2, b2, bigInteger3, bigInteger4 );
                        ECPoint g2 = DeserializeECPoint( ecPointFormats, curve2, encoding2 );
                        return new ECDomainParameters( curve2, g2, bigInteger3, bigInteger4 );
                    case 3:
                        int namedCurve = TlsUtilities.ReadUint16( input );
                        if (!NamedCurve.RefersToASpecificNamedCurve( namedCurve ))
                            throw new TlsFatalAlert( 47 );
                        CheckNamedCurve( namedCurves, namedCurve );
                        return GetParametersForNamedCurve( namedCurve );
                    default:
                        throw new TlsFatalAlert( 47 );
                }
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 47, ex );
            }
        }

        private static void CheckNamedCurve( int[] namedCurves, int namedCurve )
        {
            if (namedCurves != null && !Arrays.Contains( namedCurves, namedCurve ))
                throw new TlsFatalAlert( 47 );
        }

        public static void WriteECExponent( int k, Stream output ) => WriteECParameter( BigInteger.ValueOf( k ), output );

        public static void WriteECFieldElement( ECFieldElement x, Stream output ) => TlsUtilities.WriteOpaque8( x.GetEncoded(), output );

        public static void WriteECFieldElement( int fieldSize, BigInteger x, Stream output ) => TlsUtilities.WriteOpaque8( SerializeECFieldElement( fieldSize, x ), output );

        public static void WriteECParameter( BigInteger x, Stream output ) => TlsUtilities.WriteOpaque8( BigIntegers.AsUnsignedByteArray( x ), output );

        public static void WriteExplicitECParameters(
          byte[] ecPointFormats,
          ECDomainParameters ecParameters,
          Stream output )
        {
            ECCurve curve = ecParameters.Curve;
            if (ECAlgorithms.IsFpCurve( curve ))
            {
                TlsUtilities.WriteUint8( 1, output );
                WriteECParameter( curve.Field.Characteristic, output );
            }
            else
            {
                if (!ECAlgorithms.IsF2mCurve( curve ))
                    throw new ArgumentException( "'ecParameters' not a known curve type" );
                int[] exponentsPresent = ((IPolynomialExtensionField)curve.Field).MinimalPolynomial.GetExponentsPresent();
                TlsUtilities.WriteUint8( 2, output );
                int i = exponentsPresent[exponentsPresent.Length - 1];
                TlsUtilities.CheckUint16( i );
                TlsUtilities.WriteUint16( i, output );
                if (exponentsPresent.Length == 3)
                {
                    TlsUtilities.WriteUint8( 1, output );
                    WriteECExponent( exponentsPresent[1], output );
                }
                else
                {
                    if (exponentsPresent.Length != 5)
                        throw new ArgumentException( "Only trinomial and pentomial curves are supported" );
                    TlsUtilities.WriteUint8( 2, output );
                    WriteECExponent( exponentsPresent[1], output );
                    WriteECExponent( exponentsPresent[2], output );
                    WriteECExponent( exponentsPresent[3], output );
                }
            }
            WriteECFieldElement( curve.A, output );
            WriteECFieldElement( curve.B, output );
            TlsUtilities.WriteOpaque8( SerializeECPoint( ecPointFormats, ecParameters.G ), output );
            WriteECParameter( ecParameters.N, output );
            WriteECParameter( ecParameters.H, output );
        }

        public static void WriteECPoint( byte[] ecPointFormats, ECPoint point, Stream output ) => TlsUtilities.WriteOpaque8( SerializeECPoint( ecPointFormats, point ), output );

        public static void WriteNamedECParameters( int namedCurve, Stream output )
        {
            if (!NamedCurve.RefersToASpecificNamedCurve( namedCurve ))
                throw new TlsFatalAlert( 80 );
            TlsUtilities.WriteUint8( 3, output );
            TlsUtilities.CheckUint16( namedCurve );
            TlsUtilities.WriteUint16( namedCurve, output );
        }
    }
}
