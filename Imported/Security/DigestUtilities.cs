﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.DigestUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class DigestUtilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();
        private static readonly IDictionary oids = Platform.CreateHashtable();

        private DigestUtilities()
        {
        }

        static DigestUtilities()
        {
            ((DigestUtilities.DigestAlgorithm)Enums.GetArbitraryValue( typeof( DigestUtilities.DigestAlgorithm ) )).ToString();
            algorithms[PkcsObjectIdentifiers.MD2.Id] = "MD2";
            algorithms[PkcsObjectIdentifiers.MD4.Id] = "MD4";
            algorithms[PkcsObjectIdentifiers.MD5.Id] = "MD5";
            algorithms["SHA1"] = "SHA-1";
            algorithms[OiwObjectIdentifiers.IdSha1.Id] = "SHA-1";
            algorithms["SHA224"] = "SHA-224";
            algorithms[NistObjectIdentifiers.IdSha224.Id] = "SHA-224";
            algorithms["SHA256"] = "SHA-256";
            algorithms[NistObjectIdentifiers.IdSha256.Id] = "SHA-256";
            algorithms["SHA384"] = "SHA-384";
            algorithms[NistObjectIdentifiers.IdSha384.Id] = "SHA-384";
            algorithms["SHA512"] = "SHA-512";
            algorithms[NistObjectIdentifiers.IdSha512.Id] = "SHA-512";
            algorithms["SHA512/224"] = "SHA-512/224";
            algorithms[NistObjectIdentifiers.IdSha512_224.Id] = "SHA-512/224";
            algorithms["SHA512/256"] = "SHA-512/256";
            algorithms[NistObjectIdentifiers.IdSha512_256.Id] = "SHA-512/256";
            algorithms["RIPEMD-128"] = "RIPEMD128";
            algorithms[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "RIPEMD128";
            algorithms["RIPEMD-160"] = "RIPEMD160";
            algorithms[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "RIPEMD160";
            algorithms["RIPEMD-256"] = "RIPEMD256";
            algorithms[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "RIPEMD256";
            algorithms["RIPEMD-320"] = "RIPEMD320";
            algorithms[CryptoProObjectIdentifiers.GostR3411.Id] = "GOST3411";
            algorithms[NistObjectIdentifiers.IdSha3_224.Id] = "SHA3-224";
            algorithms[NistObjectIdentifiers.IdSha3_256.Id] = "SHA3-256";
            algorithms[NistObjectIdentifiers.IdSha3_384.Id] = "SHA3-384";
            algorithms[NistObjectIdentifiers.IdSha3_512.Id] = "SHA3-512";
            algorithms[NistObjectIdentifiers.IdShake128.Id] = "SHAKE128";
            algorithms[NistObjectIdentifiers.IdShake256.Id] = "SHAKE256";
            oids["MD2"] = PkcsObjectIdentifiers.MD2;
            oids["MD4"] = PkcsObjectIdentifiers.MD4;
            oids["MD5"] = PkcsObjectIdentifiers.MD5;
            oids["SHA-1"] = OiwObjectIdentifiers.IdSha1;
            oids["SHA-224"] = NistObjectIdentifiers.IdSha224;
            oids["SHA-256"] = NistObjectIdentifiers.IdSha256;
            oids["SHA-384"] = NistObjectIdentifiers.IdSha384;
            oids["SHA-512"] = NistObjectIdentifiers.IdSha512;
            oids["SHA-512/224"] = NistObjectIdentifiers.IdSha512_224;
            oids["SHA-512/256"] = NistObjectIdentifiers.IdSha512_256;
            oids["SHA3-224"] = NistObjectIdentifiers.IdSha3_224;
            oids["SHA3-256"] = NistObjectIdentifiers.IdSha3_256;
            oids["SHA3-384"] = NistObjectIdentifiers.IdSha3_384;
            oids["SHA3-512"] = NistObjectIdentifiers.IdSha3_512;
            oids["SHAKE128"] = NistObjectIdentifiers.IdShake128;
            oids["SHAKE256"] = NistObjectIdentifiers.IdShake256;
            oids["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
            oids["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
            oids["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
            oids["GOST3411"] = CryptoProObjectIdentifiers.GostR3411;
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

        public static IDigest GetDigest( DerObjectIdentifier id ) => GetDigest( id.Id );

        public static IDigest GetDigest( string algorithm )
        {
            string upperInvariant = Platform.ToUpperInvariant( algorithm );
            string s = (string)algorithms[upperInvariant] ?? upperInvariant;
            try
            {
                switch ((DigestUtilities.DigestAlgorithm)Enums.GetEnumValue( typeof( DigestUtilities.DigestAlgorithm ), s ))
                {
                    case DigestAlgorithm.GOST3411:
                        return new Gost3411Digest();
                    case DigestAlgorithm.KECCAK_224:
                        return new KeccakDigest( 224 );
                    case DigestAlgorithm.KECCAK_256:
                        return new KeccakDigest( 256 );
                    case DigestAlgorithm.KECCAK_288:
                        return new KeccakDigest( 288 );
                    case DigestAlgorithm.KECCAK_384:
                        return new KeccakDigest( 384 );
                    case DigestAlgorithm.KECCAK_512:
                        return new KeccakDigest( 512 );
                    case DigestAlgorithm.MD2:
                        return new MD2Digest();
                    case DigestAlgorithm.MD4:
                        return new MD4Digest();
                    case DigestAlgorithm.MD5:
                        return new MD5Digest();
                    case DigestAlgorithm.RIPEMD128:
                        return new RipeMD128Digest();
                    case DigestAlgorithm.RIPEMD160:
                        return new RipeMD160Digest();
                    case DigestAlgorithm.RIPEMD256:
                        return new RipeMD256Digest();
                    case DigestAlgorithm.RIPEMD320:
                        return new RipeMD320Digest();
                    case DigestAlgorithm.SHA_1:
                        return new Sha1Digest();
                    case DigestAlgorithm.SHA_224:
                        return new Sha224Digest();
                    case DigestAlgorithm.SHA_256:
                        return new Sha256Digest();
                    case DigestAlgorithm.SHA_384:
                        return new Sha384Digest();
                    case DigestAlgorithm.SHA_512:
                        return new Sha512Digest();
                    case DigestAlgorithm.SHA_512_224:
                        return new Sha512tDigest( 224 );
                    case DigestAlgorithm.SHA_512_256:
                        return new Sha512tDigest( 256 );
                    case DigestAlgorithm.SHA3_224:
                        return new Sha3Digest( 224 );
                    case DigestAlgorithm.SHA3_256:
                        return new Sha3Digest( 256 );
                    case DigestAlgorithm.SHA3_384:
                        return new Sha3Digest( 384 );
                    case DigestAlgorithm.SHA3_512:
                        return new Sha3Digest( 512 );
                    case DigestAlgorithm.SHAKE128:
                        return new ShakeDigest( 128 );
                    case DigestAlgorithm.SHAKE256:
                        return new ShakeDigest( 256 );
                    case DigestAlgorithm.TIGER:
                        return new TigerDigest();
                    case DigestAlgorithm.WHIRLPOOL:
                        return new WhirlpoolDigest();
                }
            }
            catch (ArgumentException ex)
            {
            }
            throw new SecurityUtilityException( "Digest " + s + " not recognised." );
        }

        public static string GetAlgorithmName( DerObjectIdentifier oid ) => (string)algorithms[oid.Id];

        public static byte[] CalculateDigest( string algorithm, byte[] input )
        {
            IDigest digest = GetDigest( algorithm );
            digest.BlockUpdate( input, 0, input.Length );
            return DoFinal( digest );
        }

        public static byte[] DoFinal( IDigest digest )
        {
            byte[] output = new byte[digest.GetDigestSize()];
            digest.DoFinal( output, 0 );
            return output;
        }

        public static byte[] DoFinal( IDigest digest, byte[] input )
        {
            digest.BlockUpdate( input, 0, input.Length );
            return DoFinal( digest );
        }

        private enum DigestAlgorithm
        {
            GOST3411,
            KECCAK_224,
            KECCAK_256,
            KECCAK_288,
            KECCAK_384,
            KECCAK_512,
            MD2,
            MD4,
            MD5,
            RIPEMD128,
            RIPEMD160,
            RIPEMD256,
            RIPEMD320,
            SHA_1,
            SHA_224,
            SHA_256,
            SHA_384,
            SHA_512,
            SHA_512_224,
            SHA_512_256,
            SHA3_224,
            SHA3_256,
            SHA3_384,
            SHA3_512,
            SHAKE128,
            SHAKE256,
            TIGER,
            WHIRLPOOL,
        }
    }
}
