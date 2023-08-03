// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.CipherUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Kisa;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Ntt;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class CipherUtilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();
        private static readonly IDictionary oids = Platform.CreateHashtable();

        static CipherUtilities()
        {
            ((CipherUtilities.CipherAlgorithm)Enums.GetArbitraryValue( typeof( CipherUtilities.CipherAlgorithm ) )).ToString();
            ((CipherUtilities.CipherMode)Enums.GetArbitraryValue( typeof( CipherUtilities.CipherMode ) )).ToString();
            ((CipherUtilities.CipherPadding)Enums.GetArbitraryValue( typeof( CipherUtilities.CipherPadding ) )).ToString();
            algorithms[NistObjectIdentifiers.IdAes128Ecb.Id] = "AES/ECB/PKCS7PADDING";
            algorithms[NistObjectIdentifiers.IdAes192Ecb.Id] = "AES/ECB/PKCS7PADDING";
            algorithms[NistObjectIdentifiers.IdAes256Ecb.Id] = "AES/ECB/PKCS7PADDING";
            algorithms["AES//PKCS7"] = "AES/ECB/PKCS7PADDING";
            algorithms["AES//PKCS7PADDING"] = "AES/ECB/PKCS7PADDING";
            algorithms["AES//PKCS5"] = "AES/ECB/PKCS7PADDING";
            algorithms["AES//PKCS5PADDING"] = "AES/ECB/PKCS7PADDING";
            algorithms[NistObjectIdentifiers.IdAes128Cbc.Id] = "AES/CBC/PKCS7PADDING";
            algorithms[NistObjectIdentifiers.IdAes192Cbc.Id] = "AES/CBC/PKCS7PADDING";
            algorithms[NistObjectIdentifiers.IdAes256Cbc.Id] = "AES/CBC/PKCS7PADDING";
            algorithms[NistObjectIdentifiers.IdAes128Ofb.Id] = "AES/OFB/NOPADDING";
            algorithms[NistObjectIdentifiers.IdAes192Ofb.Id] = "AES/OFB/NOPADDING";
            algorithms[NistObjectIdentifiers.IdAes256Ofb.Id] = "AES/OFB/NOPADDING";
            algorithms[NistObjectIdentifiers.IdAes128Cfb.Id] = "AES/CFB/NOPADDING";
            algorithms[NistObjectIdentifiers.IdAes192Cfb.Id] = "AES/CFB/NOPADDING";
            algorithms[NistObjectIdentifiers.IdAes256Cfb.Id] = "AES/CFB/NOPADDING";
            algorithms["RSA/ECB/PKCS1"] = "RSA//PKCS1PADDING";
            algorithms["RSA/ECB/PKCS1PADDING"] = "RSA//PKCS1PADDING";
            algorithms[PkcsObjectIdentifiers.RsaEncryption.Id] = "RSA//PKCS1PADDING";
            algorithms[PkcsObjectIdentifiers.IdRsaesOaep.Id] = "RSA//OAEPPADDING";
            algorithms[OiwObjectIdentifiers.DesCbc.Id] = "DES/CBC";
            algorithms[OiwObjectIdentifiers.DesCfb.Id] = "DES/CFB";
            algorithms[OiwObjectIdentifiers.DesEcb.Id] = "DES/ECB";
            algorithms[OiwObjectIdentifiers.DesOfb.Id] = "DES/OFB";
            algorithms[OiwObjectIdentifiers.DesEde.Id] = "DESEDE";
            algorithms["TDEA"] = "DESEDE";
            algorithms[PkcsObjectIdentifiers.DesEde3Cbc.Id] = "DESEDE/CBC";
            algorithms[PkcsObjectIdentifiers.RC2Cbc.Id] = "RC2/CBC";
            algorithms["1.3.6.1.4.1.188.7.1.1.2"] = "IDEA/CBC";
            algorithms["1.2.840.113533.7.66.10"] = "CAST5/CBC";
            algorithms["RC4"] = "ARC4";
            algorithms["ARCFOUR"] = "ARC4";
            algorithms["1.2.840.113549.3.4"] = "ARC4";
            algorithms["PBEWITHSHA1AND128BITRC4"] = "PBEWITHSHAAND128BITRC4";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4.Id] = "PBEWITHSHAAND128BITRC4";
            algorithms["PBEWITHSHA1AND40BITRC4"] = "PBEWITHSHAAND40BITRC4";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4.Id] = "PBEWITHSHAAND40BITRC4";
            algorithms["PBEWITHSHA1ANDDES"] = "PBEWITHSHA1ANDDES-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithSha1AndDesCbc.Id] = "PBEWITHSHA1ANDDES-CBC";
            algorithms["PBEWITHSHA1ANDRC2"] = "PBEWITHSHA1ANDRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc.Id] = "PBEWITHSHA1ANDRC2-CBC";
            algorithms["PBEWITHSHA1AND3-KEYTRIPLEDES-CBC"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
            algorithms["PBEWITHSHAAND3KEYTRIPLEDES"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
            algorithms["PBEWITHSHA1ANDDESEDE"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
            algorithms["PBEWITHSHA1AND2-KEYTRIPLEDES-CBC"] = "PBEWITHSHAAND2-KEYTRIPLEDES-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc.Id] = "PBEWITHSHAAND2-KEYTRIPLEDES-CBC";
            algorithms["PBEWITHSHA1AND128BITRC2-CBC"] = "PBEWITHSHAAND128BITRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc.Id] = "PBEWITHSHAAND128BITRC2-CBC";
            algorithms["PBEWITHSHA1AND40BITRC2-CBC"] = "PBEWITHSHAAND40BITRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc.Id] = "PBEWITHSHAAND40BITRC2-CBC";
            algorithms["PBEWITHSHA1AND128BITAES-CBC-BC"] = "PBEWITHSHAAND128BITAES-CBC-BC";
            algorithms["PBEWITHSHA-1AND128BITAES-CBC-BC"] = "PBEWITHSHAAND128BITAES-CBC-BC";
            algorithms["PBEWITHSHA1AND192BITAES-CBC-BC"] = "PBEWITHSHAAND192BITAES-CBC-BC";
            algorithms["PBEWITHSHA-1AND192BITAES-CBC-BC"] = "PBEWITHSHAAND192BITAES-CBC-BC";
            algorithms["PBEWITHSHA1AND256BITAES-CBC-BC"] = "PBEWITHSHAAND256BITAES-CBC-BC";
            algorithms["PBEWITHSHA-1AND256BITAES-CBC-BC"] = "PBEWITHSHAAND256BITAES-CBC-BC";
            algorithms["PBEWITHSHA-256AND128BITAES-CBC-BC"] = "PBEWITHSHA256AND128BITAES-CBC-BC";
            algorithms["PBEWITHSHA-256AND192BITAES-CBC-BC"] = "PBEWITHSHA256AND192BITAES-CBC-BC";
            algorithms["PBEWITHSHA-256AND256BITAES-CBC-BC"] = "PBEWITHSHA256AND256BITAES-CBC-BC";
            algorithms["GOST"] = "GOST28147";
            algorithms["GOST-28147"] = "GOST28147";
            algorithms[CryptoProObjectIdentifiers.GostR28147Cbc.Id] = "GOST28147/CBC/PKCS7PADDING";
            algorithms["RC5-32"] = "RC5";
            algorithms[NttObjectIdentifiers.IdCamellia128Cbc.Id] = "CAMELLIA/CBC/PKCS7PADDING";
            algorithms[NttObjectIdentifiers.IdCamellia192Cbc.Id] = "CAMELLIA/CBC/PKCS7PADDING";
            algorithms[NttObjectIdentifiers.IdCamellia256Cbc.Id] = "CAMELLIA/CBC/PKCS7PADDING";
            algorithms[KisaObjectIdentifiers.IdSeedCbc.Id] = "SEED/CBC/PKCS7PADDING";
            algorithms["1.3.6.1.4.1.3029.1.2"] = "BLOWFISH/CBC";
        }

        private CipherUtilities()
        {
        }

        public static DerObjectIdentifier GetObjectIdentifier( string mechanism )
        {
            mechanism = mechanism != null ? Platform.ToUpperInvariant( mechanism ) : throw new ArgumentNullException( nameof( mechanism ) );
            string algorithm = (string)algorithms[mechanism];
            if (algorithm != null)
                mechanism = algorithm;
            return (DerObjectIdentifier)oids[mechanism];
        }

        public static ICollection Algorithms => oids.Keys;

        public static IBufferedCipher GetCipher( DerObjectIdentifier oid ) => GetCipher( oid.Id );

        public static IBufferedCipher GetCipher( string algorithm )
        {
            algorithm = algorithm != null ? Platform.ToUpperInvariant( algorithm ) : throw new ArgumentNullException( nameof( algorithm ) );
            string algorithm1 = (string)algorithms[algorithm];
            if (algorithm1 != null)
                algorithm = algorithm1;
            IBasicAgreement agree = null;
            switch (algorithm)
            {
                case "IES":
                    agree = new DHBasicAgreement();
                    break;
                case "ECIES":
                    agree = new ECDHBasicAgreement();
                    break;
            }
            if (agree != null)
                return new BufferedIesCipher( new IesEngine( agree, new Kdf2BytesGenerator( new Sha1Digest() ), new HMac( new Sha1Digest() ) ) );
            if (Platform.StartsWith( algorithm, "PBE" ))
            {
                if (Platform.EndsWith( algorithm, "-CBC" ))
                {
                    switch (algorithm)
                    {
                        case "PBEWITHSHA1ANDDES-CBC":
                            return new PaddedBufferedBlockCipher( new CbcBlockCipher( new DesEngine() ) );
                        case "PBEWITHSHA1ANDRC2-CBC":
                            return new PaddedBufferedBlockCipher( new CbcBlockCipher( new RC2Engine() ) );
                        default:
                            if (Strings.IsOneOf( algorithm, "PBEWITHSHAAND2-KEYTRIPLEDES-CBC", "PBEWITHSHAAND3-KEYTRIPLEDES-CBC" ))
                                return new PaddedBufferedBlockCipher( new CbcBlockCipher( new DesEdeEngine() ) );
                            if (Strings.IsOneOf( algorithm, "PBEWITHSHAAND128BITRC2-CBC", "PBEWITHSHAAND40BITRC2-CBC" ))
                                return new PaddedBufferedBlockCipher( new CbcBlockCipher( new RC2Engine() ) );
                            break;
                    }
                }
                else if (Platform.EndsWith( algorithm, "-BC" ) || Platform.EndsWith( algorithm, "-OPENSSL" ))
                {
                    if (Strings.IsOneOf( algorithm, "PBEWITHSHAAND128BITAES-CBC-BC", "PBEWITHSHAAND192BITAES-CBC-BC", "PBEWITHSHAAND256BITAES-CBC-BC", "PBEWITHSHA256AND128BITAES-CBC-BC", "PBEWITHSHA256AND192BITAES-CBC-BC", "PBEWITHSHA256AND256BITAES-CBC-BC", "PBEWITHMD5AND128BITAES-CBC-OPENSSL", "PBEWITHMD5AND192BITAES-CBC-OPENSSL", "PBEWITHMD5AND256BITAES-CBC-OPENSSL" ))
                        return new PaddedBufferedBlockCipher( new CbcBlockCipher( new AesFastEngine() ) );
                }
            }
            string[] strArray = algorithm.Split( '/' );
            IBlockCipher blockCipher = null;
            IAsymmetricBlockCipher cipher1 = null;
            IStreamCipher cipher2 = null;
            string str = strArray[0];
            string algorithm2 = (string)algorithms[str];
            if (algorithm2 != null)
                str = algorithm2;
            CipherUtilities.CipherAlgorithm enumValue;
            try
            {
                enumValue = (CipherUtilities.CipherAlgorithm)Enums.GetEnumValue( typeof( CipherUtilities.CipherAlgorithm ), str );
            }
            catch (ArgumentException ex)
            {
                throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
            }
            switch (enumValue)
            {
                case CipherAlgorithm.AES:
                    blockCipher = new AesFastEngine();
                    break;
                case CipherAlgorithm.ARC4:
                    cipher2 = new RC4Engine();
                    break;
                case CipherAlgorithm.BLOWFISH:
                    blockCipher = new BlowfishEngine();
                    break;
                case CipherAlgorithm.CAMELLIA:
                    blockCipher = new CamelliaEngine();
                    break;
                case CipherAlgorithm.CAST5:
                    blockCipher = new Cast5Engine();
                    break;
                case CipherAlgorithm.CAST6:
                    blockCipher = new Cast6Engine();
                    break;
                case CipherAlgorithm.DES:
                    blockCipher = new DesEngine();
                    break;
                case CipherAlgorithm.DESEDE:
                    blockCipher = new DesEdeEngine();
                    break;
                case CipherAlgorithm.ELGAMAL:
                    cipher1 = new ElGamalEngine();
                    break;
                case CipherAlgorithm.GOST28147:
                    blockCipher = new Gost28147Engine();
                    break;
                case CipherAlgorithm.HC128:
                    cipher2 = new HC128Engine();
                    break;
                case CipherAlgorithm.HC256:
                    cipher2 = new HC256Engine();
                    break;
                case CipherAlgorithm.IDEA:
                    blockCipher = new IdeaEngine();
                    break;
                case CipherAlgorithm.NOEKEON:
                    blockCipher = new NoekeonEngine();
                    break;
                case CipherAlgorithm.PBEWITHSHAAND128BITRC4:
                case CipherAlgorithm.PBEWITHSHAAND40BITRC4:
                    cipher2 = new RC4Engine();
                    break;
                case CipherAlgorithm.RC2:
                    blockCipher = new RC2Engine();
                    break;
                case CipherAlgorithm.RC5:
                    blockCipher = new RC532Engine();
                    break;
                case CipherAlgorithm.RC5_64:
                    blockCipher = new RC564Engine();
                    break;
                case CipherAlgorithm.RC6:
                    blockCipher = new RC6Engine();
                    break;
                case CipherAlgorithm.RIJNDAEL:
                    blockCipher = new RijndaelEngine();
                    break;
                case CipherAlgorithm.RSA:
                    cipher1 = new RsaBlindedEngine();
                    break;
                case CipherAlgorithm.SALSA20:
                    cipher2 = new Salsa20Engine();
                    break;
                case CipherAlgorithm.SEED:
                    blockCipher = new SeedEngine();
                    break;
                case CipherAlgorithm.SERPENT:
                    blockCipher = new SerpentEngine();
                    break;
                case CipherAlgorithm.SKIPJACK:
                    blockCipher = new SkipjackEngine();
                    break;
                case CipherAlgorithm.TEA:
                    blockCipher = new TeaEngine();
                    break;
                case CipherAlgorithm.THREEFISH_256:
                    blockCipher = new ThreefishEngine( 256 );
                    break;
                case CipherAlgorithm.THREEFISH_512:
                    blockCipher = new ThreefishEngine( 512 );
                    break;
                case CipherAlgorithm.THREEFISH_1024:
                    blockCipher = new ThreefishEngine( 1024 );
                    break;
                case CipherAlgorithm.TNEPRES:
                    blockCipher = new TnepresEngine();
                    break;
                case CipherAlgorithm.TWOFISH:
                    blockCipher = new TwofishEngine();
                    break;
                case CipherAlgorithm.VMPC:
                    cipher2 = new VmpcEngine();
                    break;
                case CipherAlgorithm.VMPC_KSA3:
                    cipher2 = new VmpcKsa3Engine();
                    break;
                case CipherAlgorithm.XTEA:
                    blockCipher = new XteaEngine();
                    break;
                default:
                    throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
            }
            if (cipher2 != null)
            {
                if (strArray.Length > 1)
                    throw new ArgumentException( "Modes and paddings not used for stream ciphers" );
                return new BufferedStreamCipher( cipher2 );
            }
            bool flag1 = false;
            bool flag2 = true;
            IBlockCipherPadding padding = null;
            IAeadBlockCipher cipher3 = null;
            if (strArray.Length > 2)
            {
                if (cipher2 != null)
                    throw new ArgumentException( "Paddings not used for stream ciphers" );
                string s = strArray[2];
                CipherUtilities.CipherPadding cipherPadding;
                switch (s)
                {
                    case "":
                        cipherPadding = CipherPadding.RAW;
                        break;
                    case "X9.23PADDING":
                        cipherPadding = CipherPadding.X923PADDING;
                        break;
                    default:
                        try
                        {
                            cipherPadding = (CipherUtilities.CipherPadding)Enums.GetEnumValue( typeof( CipherUtilities.CipherPadding ), s );
                            break;
                        }
                        catch (ArgumentException ex)
                        {
                            throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
                        }
                }
                switch (cipherPadding)
                {
                    case CipherPadding.NOPADDING:
                        flag2 = false;
                        break;
                    case CipherPadding.RAW:
                        break;
                    case CipherPadding.ISO10126PADDING:
                    case CipherPadding.ISO10126D2PADDING:
                    case CipherPadding.ISO10126_2PADDING:
                        padding = new ISO10126d2Padding();
                        break;
                    case CipherPadding.ISO7816_4PADDING:
                    case CipherPadding.ISO9797_1PADDING:
                        padding = new ISO7816d4Padding();
                        break;
                    case CipherPadding.ISO9796_1:
                    case CipherPadding.ISO9796_1PADDING:
                        cipher1 = new ISO9796d1Encoding( cipher1 );
                        break;
                    case CipherPadding.OAEP:
                    case CipherPadding.OAEPPADDING:
                        cipher1 = new OaepEncoding( cipher1 );
                        break;
                    case CipherPadding.OAEPWITHMD5ANDMGF1PADDING:
                        cipher1 = new OaepEncoding( cipher1, new MD5Digest() );
                        break;
                    case CipherPadding.OAEPWITHSHA1ANDMGF1PADDING:
                    case CipherPadding.OAEPWITHSHA_1ANDMGF1PADDING:
                        cipher1 = new OaepEncoding( cipher1, new Sha1Digest() );
                        break;
                    case CipherPadding.OAEPWITHSHA224ANDMGF1PADDING:
                    case CipherPadding.OAEPWITHSHA_224ANDMGF1PADDING:
                        cipher1 = new OaepEncoding( cipher1, new Sha224Digest() );
                        break;
                    case CipherPadding.OAEPWITHSHA256ANDMGF1PADDING:
                    case CipherPadding.OAEPWITHSHA_256ANDMGF1PADDING:
                        cipher1 = new OaepEncoding( cipher1, new Sha256Digest() );
                        break;
                    case CipherPadding.OAEPWITHSHA384ANDMGF1PADDING:
                    case CipherPadding.OAEPWITHSHA_384ANDMGF1PADDING:
                        cipher1 = new OaepEncoding( cipher1, new Sha384Digest() );
                        break;
                    case CipherPadding.OAEPWITHSHA512ANDMGF1PADDING:
                    case CipherPadding.OAEPWITHSHA_512ANDMGF1PADDING:
                        cipher1 = new OaepEncoding( cipher1, new Sha512Digest() );
                        break;
                    case CipherPadding.PKCS1:
                    case CipherPadding.PKCS1PADDING:
                        cipher1 = new Pkcs1Encoding( cipher1 );
                        break;
                    case CipherPadding.PKCS5:
                    case CipherPadding.PKCS5PADDING:
                    case CipherPadding.PKCS7:
                    case CipherPadding.PKCS7PADDING:
                        padding = new Pkcs7Padding();
                        break;
                    case CipherPadding.TBCPADDING:
                        padding = new TbcPadding();
                        break;
                    case CipherPadding.WITHCTS:
                        flag1 = true;
                        break;
                    case CipherPadding.X923PADDING:
                        padding = new X923Padding();
                        break;
                    case CipherPadding.ZEROBYTEPADDING:
                        padding = new ZeroBytePadding();
                        break;
                    default:
                        throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
                }
            }
            if (strArray.Length > 1)
            {
                string s1 = strArray[1];
                int digitIndex = GetDigitIndex( s1 );
                string s2 = digitIndex >= 0 ? s1.Substring( 0, digitIndex ) : s1;
                try
                {
                    switch (s2 == "" ? 1 : Convert.ToInt32( Enums.GetEnumValue( typeof( CipherMode ), s2 ) ))
                    {
                        case 0:
                        case 1:
                            break;
                        case 2:
                            blockCipher = new CbcBlockCipher( blockCipher );
                            break;
                        case 3:
                            cipher3 = new CcmBlockCipher( blockCipher );
                            break;
                        case 4:
                            int bitBlockSize = digitIndex < 0 ? 8 * blockCipher.GetBlockSize() : int.Parse( s1.Substring( digitIndex ) );
                            blockCipher = new CfbBlockCipher( blockCipher, bitBlockSize );
                            break;
                        case 5:
                            blockCipher = new SicBlockCipher( blockCipher );
                            break;
                        case 6:
                            flag1 = true;
                            blockCipher = new CbcBlockCipher( blockCipher );
                            break;
                        case 7:
                            cipher3 = new EaxBlockCipher( blockCipher );
                            break;
                        case 8:
                            cipher3 = new GcmBlockCipher( blockCipher );
                            break;
                        case 9:
                            blockCipher = new GOfbBlockCipher( blockCipher );
                            break;
                        case 10:
                            cipher3 = new OcbBlockCipher( blockCipher, CreateBlockCipher( enumValue ) );
                            break;
                        case 11:
                            int blockSize = digitIndex < 0 ? 8 * blockCipher.GetBlockSize() : int.Parse( s1.Substring( digitIndex ) );
                            blockCipher = new OfbBlockCipher( blockCipher, blockSize );
                            break;
                        case 12:
                            blockCipher = new OpenPgpCfbBlockCipher( blockCipher );
                            break;
                        case 13:
                            blockCipher = blockCipher.GetBlockSize() >= 16 ? (IBlockCipher)new SicBlockCipher( blockCipher ) : throw new ArgumentException( "Warning: SIC-Mode can become a twotime-pad if the blocksize of the cipher is too small. Use a cipher with a block size of at least 128 bits (e.g. AES)" );
                            break;
                        default:
                            throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
                    }
                }
                catch (ArgumentException ex)
                {
                    throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
                }
            }
            if (cipher3 != null)
            {
                if (flag1)
                    throw new SecurityUtilityException( "CTS mode not valid for AEAD ciphers." );
                if (flag2 && strArray.Length > 2 && strArray[2] != "")
                    throw new SecurityUtilityException( "Bad padding specified for AEAD cipher." );
                return new BufferedAeadBlockCipher( cipher3 );
            }
            if (blockCipher != null)
            {
                if (flag1)
                    return new CtsBlockCipher( blockCipher );
                if (padding != null)
                    return new PaddedBufferedBlockCipher( blockCipher, padding );
                return !flag2 || blockCipher.IsPartialBlockOkay ? new BufferedBlockCipher( blockCipher ) : (IBufferedCipher)new PaddedBufferedBlockCipher( blockCipher );
            }
            return cipher1 != null ? (IBufferedCipher)new BufferedAsymmetricBlockCipher( cipher1 ) : throw new SecurityUtilityException( "Cipher " + algorithm + " not recognised." );
        }

        public static string GetAlgorithmName( DerObjectIdentifier oid ) => (string)algorithms[oid.Id];

        private static int GetDigitIndex( string s )
        {
            for (int index = 0; index < s.Length; ++index)
            {
                if (char.IsDigit( s[index] ))
                    return index;
            }
            return -1;
        }

        private static IBlockCipher CreateBlockCipher( CipherUtilities.CipherAlgorithm cipherAlgorithm )
        {
            switch (cipherAlgorithm)
            {
                case CipherAlgorithm.AES:
                    return new AesFastEngine();
                case CipherAlgorithm.BLOWFISH:
                    return new BlowfishEngine();
                case CipherAlgorithm.CAMELLIA:
                    return new CamelliaEngine();
                case CipherAlgorithm.CAST5:
                    return new Cast5Engine();
                case CipherAlgorithm.CAST6:
                    return new Cast6Engine();
                case CipherAlgorithm.DES:
                    return new DesEngine();
                case CipherAlgorithm.DESEDE:
                    return new DesEdeEngine();
                case CipherAlgorithm.GOST28147:
                    return new Gost28147Engine();
                case CipherAlgorithm.IDEA:
                    return new IdeaEngine();
                case CipherAlgorithm.NOEKEON:
                    return new NoekeonEngine();
                case CipherAlgorithm.RC2:
                    return new RC2Engine();
                case CipherAlgorithm.RC5:
                    return new RC532Engine();
                case CipherAlgorithm.RC5_64:
                    return new RC564Engine();
                case CipherAlgorithm.RC6:
                    return new RC6Engine();
                case CipherAlgorithm.RIJNDAEL:
                    return new RijndaelEngine();
                case CipherAlgorithm.SEED:
                    return new SeedEngine();
                case CipherAlgorithm.SERPENT:
                    return new SerpentEngine();
                case CipherAlgorithm.SKIPJACK:
                    return new SkipjackEngine();
                case CipherAlgorithm.TEA:
                    return new TeaEngine();
                case CipherAlgorithm.THREEFISH_256:
                    return new ThreefishEngine( 256 );
                case CipherAlgorithm.THREEFISH_512:
                    return new ThreefishEngine( 512 );
                case CipherAlgorithm.THREEFISH_1024:
                    return new ThreefishEngine( 1024 );
                case CipherAlgorithm.TNEPRES:
                    return new TnepresEngine();
                case CipherAlgorithm.TWOFISH:
                    return new TwofishEngine();
                case CipherAlgorithm.XTEA:
                    return new XteaEngine();
                default:
                    throw new SecurityUtilityException( "Cipher " + cipherAlgorithm + " not recognised or not a block cipher" );
            }
        }

        private enum CipherAlgorithm
        {
            AES,
            ARC4,
            BLOWFISH,
            CAMELLIA,
            CAST5,
            CAST6,
            DES,
            DESEDE,
            ELGAMAL,
            GOST28147,
            HC128,
            HC256,
            IDEA,
            NOEKEON,
            PBEWITHSHAAND128BITRC4,
            PBEWITHSHAAND40BITRC4,
            RC2,
            RC5,
            RC5_64,
            RC6,
            RIJNDAEL,
            RSA,
            SALSA20,
            SEED,
            SERPENT,
            SKIPJACK,
            TEA,
            THREEFISH_256,
            THREEFISH_512,
            THREEFISH_1024,
            TNEPRES,
            TWOFISH,
            VMPC,
            VMPC_KSA3,
            XTEA,
        }

        private enum CipherMode
        {
            ECB,
            NONE,
            CBC,
            CCM,
            CFB,
            CTR,
            CTS,
            EAX,
            GCM,
            GOFB,
            OCB,
            OFB,
            OPENPGPCFB,
            SIC,
        }

        private enum CipherPadding
        {
            NOPADDING,
            RAW,
            ISO10126PADDING,
            ISO10126D2PADDING,
            ISO10126_2PADDING,
            ISO7816_4PADDING,
            ISO9797_1PADDING,
            ISO9796_1,
            ISO9796_1PADDING,
            OAEP,
            OAEPPADDING,
            OAEPWITHMD5ANDMGF1PADDING,
            OAEPWITHSHA1ANDMGF1PADDING,
            OAEPWITHSHA_1ANDMGF1PADDING,
            OAEPWITHSHA224ANDMGF1PADDING,
            OAEPWITHSHA_224ANDMGF1PADDING,
            OAEPWITHSHA256ANDMGF1PADDING,
            OAEPWITHSHA_256ANDMGF1PADDING,
            OAEPWITHSHA384ANDMGF1PADDING,
            OAEPWITHSHA_384ANDMGF1PADDING,
            OAEPWITHSHA512ANDMGF1PADDING,
            OAEPWITHSHA_512ANDMGF1PADDING,
            PKCS1,
            PKCS1PADDING,
            PKCS5,
            PKCS5PADDING,
            PKCS7,
            PKCS7PADDING,
            TBCPADDING,
            WITHCTS,
            X923PADDING,
            ZEROBYTEPADDING,
        }
    }
}
