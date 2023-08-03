// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.ParameterUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Kisa;
using Org.BouncyCastle.Asn1.Misc;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Ntt;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class ParameterUtilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();
        private static readonly IDictionary basicIVSizes = Platform.CreateHashtable();

        private ParameterUtilities()
        {
        }

        static ParameterUtilities()
        {
            AddAlgorithm( "AES", "AESWRAP" );
            AddAlgorithm( "AES128", "2.16.840.1.101.3.4.2", NistObjectIdentifiers.IdAes128Cbc, NistObjectIdentifiers.IdAes128Cfb, NistObjectIdentifiers.IdAes128Ecb, NistObjectIdentifiers.IdAes128Ofb, NistObjectIdentifiers.IdAes128Wrap );
            AddAlgorithm( "AES192", "2.16.840.1.101.3.4.22", NistObjectIdentifiers.IdAes192Cbc, NistObjectIdentifiers.IdAes192Cfb, NistObjectIdentifiers.IdAes192Ecb, NistObjectIdentifiers.IdAes192Ofb, NistObjectIdentifiers.IdAes192Wrap );
            AddAlgorithm( "AES256", "2.16.840.1.101.3.4.42", NistObjectIdentifiers.IdAes256Cbc, NistObjectIdentifiers.IdAes256Cfb, NistObjectIdentifiers.IdAes256Ecb, NistObjectIdentifiers.IdAes256Ofb, NistObjectIdentifiers.IdAes256Wrap );
            AddAlgorithm( "BLOWFISH", "1.3.6.1.4.1.3029.1.2" );
            AddAlgorithm( "CAMELLIA", "CAMELLIAWRAP" );
            AddAlgorithm( "CAMELLIA128", NttObjectIdentifiers.IdCamellia128Cbc, NttObjectIdentifiers.IdCamellia128Wrap );
            AddAlgorithm( "CAMELLIA192", NttObjectIdentifiers.IdCamellia192Cbc, NttObjectIdentifiers.IdCamellia192Wrap );
            AddAlgorithm( "CAMELLIA256", NttObjectIdentifiers.IdCamellia256Cbc, NttObjectIdentifiers.IdCamellia256Wrap );
            AddAlgorithm( "CAST5", "1.2.840.113533.7.66.10" );
            AddAlgorithm( "CAST6" );
            AddAlgorithm( "DES", OiwObjectIdentifiers.DesCbc, OiwObjectIdentifiers.DesCfb, OiwObjectIdentifiers.DesEcb, OiwObjectIdentifiers.DesOfb );
            AddAlgorithm( "DESEDE", "DESEDEWRAP", "TDEA", OiwObjectIdentifiers.DesEde, PkcsObjectIdentifiers.IdAlgCms3DesWrap );
            AddAlgorithm( "DESEDE3", PkcsObjectIdentifiers.DesEde3Cbc );
            AddAlgorithm( "GOST28147", "GOST", "GOST-28147", CryptoProObjectIdentifiers.GostR28147Cbc );
            AddAlgorithm( "HC128" );
            AddAlgorithm( "HC256" );
            AddAlgorithm( "IDEA", "1.3.6.1.4.1.188.7.1.1.2" );
            AddAlgorithm( "NOEKEON" );
            AddAlgorithm( "RC2", PkcsObjectIdentifiers.RC2Cbc, PkcsObjectIdentifiers.IdAlgCmsRC2Wrap );
            AddAlgorithm( "RC4", "ARC4", "1.2.840.113549.3.4" );
            AddAlgorithm( "RC5", "RC5-32" );
            AddAlgorithm( "RC5-64" );
            AddAlgorithm( "RC6" );
            AddAlgorithm( "RIJNDAEL" );
            AddAlgorithm( "SALSA20" );
            AddAlgorithm( "SEED", KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap, KisaObjectIdentifiers.IdSeedCbc );
            AddAlgorithm( "SERPENT" );
            AddAlgorithm( "SKIPJACK" );
            AddAlgorithm( "TEA" );
            AddAlgorithm( "THREEFISH-256" );
            AddAlgorithm( "THREEFISH-512" );
            AddAlgorithm( "THREEFISH-1024" );
            AddAlgorithm( "TNEPRES" );
            AddAlgorithm( "TWOFISH" );
            AddAlgorithm( "VMPC" );
            AddAlgorithm( "VMPC-KSA3" );
            AddAlgorithm( "XTEA" );
            AddBasicIVSizeEntries( 8, "BLOWFISH", "DES", "DESEDE", "DESEDE3" );
            AddBasicIVSizeEntries( 16, "AES", "AES128", "AES192", "AES256", "CAMELLIA", "CAMELLIA128", "CAMELLIA192", "CAMELLIA256", "NOEKEON", "SEED" );
        }

        private static void AddAlgorithm( string canonicalName, params object[] aliases )
        {
            algorithms[canonicalName] = canonicalName;
            foreach (object alias in aliases)
                algorithms[alias.ToString()] = canonicalName;
        }

        private static void AddBasicIVSizeEntries( int size, params string[] algorithms )
        {
            foreach (string algorithm in algorithms)
                basicIVSizes.Add( algorithm, size );
        }

        public static string GetCanonicalAlgorithmName( string algorithm ) => (string)algorithms[Platform.ToUpperInvariant( algorithm )];

        public static KeyParameter CreateKeyParameter( DerObjectIdentifier algOid, byte[] keyBytes ) => CreateKeyParameter( algOid.Id, keyBytes, 0, keyBytes.Length );

        public static KeyParameter CreateKeyParameter( string algorithm, byte[] keyBytes ) => CreateKeyParameter( algorithm, keyBytes, 0, keyBytes.Length );

        public static KeyParameter CreateKeyParameter(
          DerObjectIdentifier algOid,
          byte[] keyBytes,
          int offset,
          int length )
        {
            return CreateKeyParameter( algOid.Id, keyBytes, offset, length );
        }

        public static KeyParameter CreateKeyParameter(
          string algorithm,
          byte[] keyBytes,
          int offset,
          int length )
        {
            if (algorithm == null)
                throw new ArgumentNullException( nameof( algorithm ) );
            switch (GetCanonicalAlgorithmName( algorithm ))
            {
                case null:
                    throw new SecurityUtilityException( "Algorithm " + algorithm + " not recognised." );
                case "DES":
                    return new DesParameters( keyBytes, offset, length );
                case "DESEDE":
                case "DESEDE3":
                    return new DesEdeParameters( keyBytes, offset, length );
                case "RC2":
                    return new RC2Parameters( keyBytes, offset, length );
                default:
                    return new KeyParameter( keyBytes, offset, length );
            }
        }

        public static ICipherParameters GetCipherParameters(
          DerObjectIdentifier algOid,
          ICipherParameters key,
          Asn1Object asn1Params )
        {
            return GetCipherParameters( algOid.Id, key, asn1Params );
        }

        public static ICipherParameters GetCipherParameters(
          string algorithm,
          ICipherParameters key,
          Asn1Object asn1Params )
        {
            string canonicalName = algorithm != null ? GetCanonicalAlgorithmName( algorithm ) : throw new ArgumentNullException( nameof( algorithm ) );
            if (canonicalName == null)
                throw new SecurityUtilityException( "Algorithm " + algorithm + " not recognised." );
            byte[] iv = null;
            try
            {
                if (FindBasicIVSize( canonicalName ) == -1)
                {
                    switch (canonicalName)
                    {
                        case "RIJNDAEL":
                        case "SKIPJACK":
                        case "TWOFISH":
                            break;
                        case "CAST5":
                            iv = Cast5CbcParameters.GetInstance( asn1Params ).GetIV();
                            goto label_12;
                        case "IDEA":
                            iv = IdeaCbcPar.GetInstance( asn1Params ).GetIV();
                            goto label_12;
                        case "RC2":
                            iv = RC2CbcParameter.GetInstance( asn1Params ).GetIV();
                            goto label_12;
                        default:
                            goto label_12;
                    }
                }
                iv = ((Asn1OctetString)asn1Params).GetOctets();
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "Could not process ASN.1 parameters", ex );
            }
        label_12:
            return iv != null ? (ICipherParameters)new ParametersWithIV( key, iv ) : throw new SecurityUtilityException( "Algorithm " + algorithm + " not recognised." );
        }

        public static Asn1Encodable GenerateParameters( DerObjectIdentifier algID, SecureRandom random ) => GenerateParameters( algID.Id, random );

        public static Asn1Encodable GenerateParameters( string algorithm, SecureRandom random )
        {
            string canonicalName = algorithm != null ? GetCanonicalAlgorithmName( algorithm ) : throw new ArgumentNullException( nameof( algorithm ) );
            int ivLength = canonicalName != null ? FindBasicIVSize( canonicalName ) : throw new SecurityUtilityException( "Algorithm " + algorithm + " not recognised." );
            if (ivLength != -1)
                return CreateIVOctetString( random, ivLength );
            switch (canonicalName)
            {
                case "CAST5":
                    return new Cast5CbcParameters( CreateIV( random, 8 ), 128 );
                case "IDEA":
                    return new IdeaCbcPar( CreateIV( random, 8 ) );
                case "RC2":
                    return new RC2CbcParameter( CreateIV( random, 8 ) );
                default:
                    throw new SecurityUtilityException( "Algorithm " + algorithm + " not recognised." );
            }
        }

        private static Asn1OctetString CreateIVOctetString( SecureRandom random, int ivLength ) => new DerOctetString( CreateIV( random, ivLength ) );

        private static byte[] CreateIV( SecureRandom random, int ivLength )
        {
            byte[] buffer = new byte[ivLength];
            random.NextBytes( buffer );
            return buffer;
        }

        private static int FindBasicIVSize( string canonicalName ) => !basicIVSizes.Contains( canonicalName ) ? -1 : (int)basicIVSizes[canonicalName];
    }
}
