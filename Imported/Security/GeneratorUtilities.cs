// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.GeneratorUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Iana;
using Org.BouncyCastle.Asn1.Kisa;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Ntt;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class GeneratorUtilities
    {
        private static readonly IDictionary kgAlgorithms = Platform.CreateHashtable();
        private static readonly IDictionary kpgAlgorithms = Platform.CreateHashtable();
        private static readonly IDictionary defaultKeySizes = Platform.CreateHashtable();

        private GeneratorUtilities()
        {
        }

        static GeneratorUtilities()
        {
            AddKgAlgorithm( "AES", "AESWRAP" );
            AddKgAlgorithm( "AES128", "2.16.840.1.101.3.4.2", NistObjectIdentifiers.IdAes128Cbc, NistObjectIdentifiers.IdAes128Cfb, NistObjectIdentifiers.IdAes128Ecb, NistObjectIdentifiers.IdAes128Ofb, NistObjectIdentifiers.IdAes128Wrap );
            AddKgAlgorithm( "AES192", "2.16.840.1.101.3.4.22", NistObjectIdentifiers.IdAes192Cbc, NistObjectIdentifiers.IdAes192Cfb, NistObjectIdentifiers.IdAes192Ecb, NistObjectIdentifiers.IdAes192Ofb, NistObjectIdentifiers.IdAes192Wrap );
            AddKgAlgorithm( "AES256", "2.16.840.1.101.3.4.42", NistObjectIdentifiers.IdAes256Cbc, NistObjectIdentifiers.IdAes256Cfb, NistObjectIdentifiers.IdAes256Ecb, NistObjectIdentifiers.IdAes256Ofb, NistObjectIdentifiers.IdAes256Wrap );
            AddKgAlgorithm( "BLOWFISH", "1.3.6.1.4.1.3029.1.2" );
            AddKgAlgorithm( "CAMELLIA", "CAMELLIAWRAP" );
            AddKgAlgorithm( "CAMELLIA128", NttObjectIdentifiers.IdCamellia128Cbc, NttObjectIdentifiers.IdCamellia128Wrap );
            AddKgAlgorithm( "CAMELLIA192", NttObjectIdentifiers.IdCamellia192Cbc, NttObjectIdentifiers.IdCamellia192Wrap );
            AddKgAlgorithm( "CAMELLIA256", NttObjectIdentifiers.IdCamellia256Cbc, NttObjectIdentifiers.IdCamellia256Wrap );
            AddKgAlgorithm( "CAST5", "1.2.840.113533.7.66.10" );
            AddKgAlgorithm( "CAST6" );
            AddKgAlgorithm( "DES", OiwObjectIdentifiers.DesCbc, OiwObjectIdentifiers.DesCfb, OiwObjectIdentifiers.DesEcb, OiwObjectIdentifiers.DesOfb );
            AddKgAlgorithm( "DESEDE", "DESEDEWRAP", "TDEA", OiwObjectIdentifiers.DesEde );
            AddKgAlgorithm( "DESEDE3", PkcsObjectIdentifiers.DesEde3Cbc, PkcsObjectIdentifiers.IdAlgCms3DesWrap );
            AddKgAlgorithm( "GOST28147", "GOST", "GOST-28147", CryptoProObjectIdentifiers.GostR28147Cbc );
            AddKgAlgorithm( "HC128" );
            AddKgAlgorithm( "HC256" );
            AddKgAlgorithm( "IDEA", "1.3.6.1.4.1.188.7.1.1.2" );
            AddKgAlgorithm( "NOEKEON" );
            AddKgAlgorithm( "RC2", PkcsObjectIdentifiers.RC2Cbc, PkcsObjectIdentifiers.IdAlgCmsRC2Wrap );
            AddKgAlgorithm( "RC4", "ARC4", "1.2.840.113549.3.4" );
            AddKgAlgorithm( "RC5", "RC5-32" );
            AddKgAlgorithm( "RC5-64" );
            AddKgAlgorithm( "RC6" );
            AddKgAlgorithm( "RIJNDAEL" );
            AddKgAlgorithm( "SALSA20" );
            AddKgAlgorithm( "SEED", KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap, KisaObjectIdentifiers.IdSeedCbc );
            AddKgAlgorithm( "SERPENT" );
            AddKgAlgorithm( "SKIPJACK" );
            AddKgAlgorithm( "TEA" );
            AddKgAlgorithm( "THREEFISH-256" );
            AddKgAlgorithm( "THREEFISH-512" );
            AddKgAlgorithm( "THREEFISH-1024" );
            AddKgAlgorithm( "TNEPRES" );
            AddKgAlgorithm( "TWOFISH" );
            AddKgAlgorithm( "VMPC" );
            AddKgAlgorithm( "VMPC-KSA3" );
            AddKgAlgorithm( "XTEA" );
            AddHMacKeyGenerator( "MD2" );
            AddHMacKeyGenerator( "MD4" );
            AddHMacKeyGenerator( "MD5", IanaObjectIdentifiers.HmacMD5 );
            AddHMacKeyGenerator( "SHA1", PkcsObjectIdentifiers.IdHmacWithSha1, IanaObjectIdentifiers.HmacSha1 );
            AddHMacKeyGenerator( "SHA224", PkcsObjectIdentifiers.IdHmacWithSha224 );
            AddHMacKeyGenerator( "SHA256", PkcsObjectIdentifiers.IdHmacWithSha256 );
            AddHMacKeyGenerator( "SHA384", PkcsObjectIdentifiers.IdHmacWithSha384 );
            AddHMacKeyGenerator( "SHA512", PkcsObjectIdentifiers.IdHmacWithSha512 );
            AddHMacKeyGenerator( "SHA512/224" );
            AddHMacKeyGenerator( "SHA512/256" );
            AddHMacKeyGenerator( "SHA3-224" );
            AddHMacKeyGenerator( "SHA3-256" );
            AddHMacKeyGenerator( "SHA3-384" );
            AddHMacKeyGenerator( "SHA3-512" );
            AddHMacKeyGenerator( "RIPEMD128" );
            AddHMacKeyGenerator( "RIPEMD160", IanaObjectIdentifiers.HmacRipeMD160 );
            AddHMacKeyGenerator( "TIGER", IanaObjectIdentifiers.HmacTiger );
            AddKpgAlgorithm( "DH", "DIFFIEHELLMAN" );
            AddKpgAlgorithm( "DSA" );
            AddKpgAlgorithm( "EC", X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme );
            AddKpgAlgorithm( "ECDH", "ECIES" );
            AddKpgAlgorithm( "ECDHC" );
            AddKpgAlgorithm( "ECMQV", X9ObjectIdentifiers.MqvSinglePassSha1KdfScheme );
            AddKpgAlgorithm( "ECDSA" );
            AddKpgAlgorithm( "ECGOST3410", "ECGOST-3410", "GOST-3410-2001" );
            AddKpgAlgorithm( "ELGAMAL" );
            AddKpgAlgorithm( "GOST3410", "GOST-3410", "GOST-3410-94" );
            AddKpgAlgorithm( "RSA", "1.2.840.113549.1.1.1" );
            AddDefaultKeySizeEntries( 64, "DES" );
            AddDefaultKeySizeEntries( 80, "SKIPJACK" );
            AddDefaultKeySizeEntries( 128, "AES128", "BLOWFISH", "CAMELLIA128", "CAST5", "DESEDE", "HC128", "HMACMD2", "HMACMD4", "HMACMD5", "HMACRIPEMD128", "IDEA", "NOEKEON", "RC2", "RC4", "RC5", "SALSA20", "SEED", "TEA", "XTEA", "VMPC", "VMPC-KSA3" );
            AddDefaultKeySizeEntries( 160, "HMACRIPEMD160", "HMACSHA1" );
            AddDefaultKeySizeEntries( 192, "AES", "AES192", "CAMELLIA192", "DESEDE3", "HMACTIGER", "RIJNDAEL", "SERPENT", "TNEPRES" );
            AddDefaultKeySizeEntries( 224, "HMACSHA224", "HMACSHA512/224" );
            AddDefaultKeySizeEntries( 256, "AES256", "CAMELLIA", "CAMELLIA256", "CAST6", "GOST28147", "HC256", "HMACSHA256", "HMACSHA512/256", "RC5-64", "RC6", "THREEFISH-256", "TWOFISH" );
            AddDefaultKeySizeEntries( 384, "HMACSHA384" );
            AddDefaultKeySizeEntries( 512, "HMACSHA512", "THREEFISH-512" );
            AddDefaultKeySizeEntries( 1024, "THREEFISH-1024" );
        }

        private static void AddDefaultKeySizeEntries( int size, params string[] algorithms )
        {
            foreach (string algorithm in algorithms)
                defaultKeySizes.Add( algorithm, size );
        }

        private static void AddKgAlgorithm( string canonicalName, params object[] aliases )
        {
            kgAlgorithms[canonicalName] = canonicalName;
            foreach (object alias in aliases)
                kgAlgorithms[alias.ToString()] = canonicalName;
        }

        private static void AddKpgAlgorithm( string canonicalName, params object[] aliases )
        {
            kpgAlgorithms[canonicalName] = canonicalName;
            foreach (object alias in aliases)
                kpgAlgorithms[alias.ToString()] = canonicalName;
        }

        private static void AddHMacKeyGenerator( string algorithm, params object[] aliases )
        {
            string key = "HMAC" + algorithm;
            kgAlgorithms[key] = key;
            kgAlgorithms["HMAC-" + algorithm] = key;
            kgAlgorithms["HMAC/" + algorithm] = key;
            foreach (object alias in aliases)
                kgAlgorithms[alias.ToString()] = key;
        }

        internal static string GetCanonicalKeyGeneratorAlgorithm( string algorithm ) => (string)kgAlgorithms[Platform.ToUpperInvariant( algorithm )];

        internal static string GetCanonicalKeyPairGeneratorAlgorithm( string algorithm ) => (string)kpgAlgorithms[Platform.ToUpperInvariant( algorithm )];

        public static CipherKeyGenerator GetKeyGenerator( DerObjectIdentifier oid ) => GetKeyGenerator( oid.Id );

        public static CipherKeyGenerator GetKeyGenerator( string algorithm )
        {
            string generatorAlgorithm = GetCanonicalKeyGeneratorAlgorithm( algorithm );
            int defaultStrength = generatorAlgorithm != null ? FindDefaultKeySize( generatorAlgorithm ) : throw new SecurityUtilityException( "KeyGenerator " + algorithm + " not recognised." );
            if (defaultStrength == -1)
                throw new SecurityUtilityException( "KeyGenerator " + algorithm + " (" + generatorAlgorithm + ") not supported." );
            switch (generatorAlgorithm)
            {
                case "DES":
                    return new DesKeyGenerator( defaultStrength );
                case "DESEDE":
                case "DESEDE3":
                    return new DesEdeKeyGenerator( defaultStrength );
                default:
                    return new CipherKeyGenerator( defaultStrength );
            }
        }

        public static IAsymmetricCipherKeyPairGenerator GetKeyPairGenerator( DerObjectIdentifier oid ) => GetKeyPairGenerator( oid.Id );

        public static IAsymmetricCipherKeyPairGenerator GetKeyPairGenerator( string algorithm )
        {
            string generatorAlgorithm = GetCanonicalKeyPairGeneratorAlgorithm( algorithm );
            switch (generatorAlgorithm)
            {
                case null:
                    throw new SecurityUtilityException( "KeyPairGenerator " + algorithm + " not recognised." );
                case "DH":
                    return new DHKeyPairGenerator();
                case "DSA":
                    return new DsaKeyPairGenerator();
                default:
                    if (Platform.StartsWith( generatorAlgorithm, "EC" ))
                        return new ECKeyPairGenerator( generatorAlgorithm );
                    switch (generatorAlgorithm)
                    {
                        case "ELGAMAL":
                            return new ElGamalKeyPairGenerator();
                        case "GOST3410":
                            return new Gost3410KeyPairGenerator();
                        case "RSA":
                            return new RsaKeyPairGenerator();
                        default:
                            throw new SecurityUtilityException( "KeyPairGenerator " + algorithm + " (" + generatorAlgorithm + ") not supported." );
                    }
            }
        }

        internal static int GetDefaultKeySize( DerObjectIdentifier oid ) => GetDefaultKeySize( oid.Id );

        internal static int GetDefaultKeySize( string algorithm )
        {
            string generatorAlgorithm = GetCanonicalKeyGeneratorAlgorithm( algorithm );
            int num = generatorAlgorithm != null ? FindDefaultKeySize( generatorAlgorithm ) : throw new SecurityUtilityException( "KeyGenerator " + algorithm + " not recognised." );
            return num != -1 ? num : throw new SecurityUtilityException( "KeyGenerator " + algorithm + " (" + generatorAlgorithm + ") not supported." );
        }

        private static int FindDefaultKeySize( string canonicalName ) => !defaultKeySizes.Contains( canonicalName ) ? -1 : (int)defaultKeySizes[canonicalName];
    }
}
