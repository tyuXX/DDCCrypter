﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.PbeUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.BC;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class PbeUtilities
    {
        private const string Pkcs5S1 = "Pkcs5S1";
        private const string Pkcs5S2 = "Pkcs5S2";
        private const string Pkcs12 = "Pkcs12";
        private const string OpenSsl = "OpenSsl";
        private static readonly IDictionary algorithms = Platform.CreateHashtable();
        private static readonly IDictionary algorithmType = Platform.CreateHashtable();
        private static readonly IDictionary oids = Platform.CreateHashtable();

        private PbeUtilities()
        {
        }

        static PbeUtilities()
        {
            algorithms["PKCS5SCHEME1"] = "Pkcs5scheme1";
            algorithms["PKCS5SCHEME2"] = "Pkcs5scheme2";
            algorithms[PkcsObjectIdentifiers.IdPbeS2.Id] = "Pkcs5scheme2";
            algorithms["PBEWITHMD2ANDDES-CBC"] = "PBEwithMD2andDES-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithMD2AndDesCbc.Id] = "PBEwithMD2andDES-CBC";
            algorithms["PBEWITHMD2ANDRC2-CBC"] = "PBEwithMD2andRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithMD2AndRC2Cbc.Id] = "PBEwithMD2andRC2-CBC";
            algorithms["PBEWITHMD5ANDDES-CBC"] = "PBEwithMD5andDES-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithMD5AndDesCbc.Id] = "PBEwithMD5andDES-CBC";
            algorithms["PBEWITHMD5ANDRC2-CBC"] = "PBEwithMD5andRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithMD5AndRC2Cbc.Id] = "PBEwithMD5andRC2-CBC";
            algorithms["PBEWITHSHA1ANDDES"] = "PBEwithSHA-1andDES-CBC";
            algorithms["PBEWITHSHA-1ANDDES"] = "PBEwithSHA-1andDES-CBC";
            algorithms["PBEWITHSHA1ANDDES-CBC"] = "PBEwithSHA-1andDES-CBC";
            algorithms["PBEWITHSHA-1ANDDES-CBC"] = "PBEwithSHA-1andDES-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithSha1AndDesCbc.Id] = "PBEwithSHA-1andDES-CBC";
            algorithms["PBEWITHSHA1ANDRC2"] = "PBEwithSHA-1andRC2-CBC";
            algorithms["PBEWITHSHA-1ANDRC2"] = "PBEwithSHA-1andRC2-CBC";
            algorithms["PBEWITHSHA1ANDRC2-CBC"] = "PBEwithSHA-1andRC2-CBC";
            algorithms["PBEWITHSHA-1ANDRC2-CBC"] = "PBEwithSHA-1andRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc.Id] = "PBEwithSHA-1andRC2-CBC";
            algorithms["PKCS12"] = nameof( Pkcs12 );
            algorithms[BCObjectIdentifiers.bc_pbe_sha1_pkcs12_aes128_cbc.Id] = "PBEwithSHA-1and128bitAES-CBC-BC";
            algorithms[BCObjectIdentifiers.bc_pbe_sha1_pkcs12_aes192_cbc.Id] = "PBEwithSHA-1and192bitAES-CBC-BC";
            algorithms[BCObjectIdentifiers.bc_pbe_sha1_pkcs12_aes256_cbc.Id] = "PBEwithSHA-1and256bitAES-CBC-BC";
            algorithms[BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes128_cbc.Id] = "PBEwithSHA-256and128bitAES-CBC-BC";
            algorithms[BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes192_cbc.Id] = "PBEwithSHA-256and192bitAES-CBC-BC";
            algorithms[BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes256_cbc.Id] = "PBEwithSHA-256and256bitAES-CBC-BC";
            algorithms["PBEWITHSHAAND128BITRC4"] = "PBEwithSHA-1and128bitRC4";
            algorithms["PBEWITHSHA1AND128BITRC4"] = "PBEwithSHA-1and128bitRC4";
            algorithms["PBEWITHSHA-1AND128BITRC4"] = "PBEwithSHA-1and128bitRC4";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4.Id] = "PBEwithSHA-1and128bitRC4";
            algorithms["PBEWITHSHAAND40BITRC4"] = "PBEwithSHA-1and40bitRC4";
            algorithms["PBEWITHSHA1AND40BITRC4"] = "PBEwithSHA-1and40bitRC4";
            algorithms["PBEWITHSHA-1AND40BITRC4"] = "PBEwithSHA-1and40bitRC4";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4.Id] = "PBEwithSHA-1and40bitRC4";
            algorithms["PBEWITHSHAAND3-KEYDESEDE-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms["PBEWITHSHAAND3-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms["PBEWITHSHA1AND3-KEYDESEDE-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms["PBEWITHSHA1AND3-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms["PBEWITHSHA-1AND3-KEYDESEDE-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms["PBEWITHSHA-1AND3-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id] = "PBEwithSHA-1and3-keyDESEDE-CBC";
            algorithms["PBEWITHSHAAND2-KEYDESEDE-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms["PBEWITHSHAAND2-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms["PBEWITHSHA1AND2-KEYDESEDE-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms["PBEWITHSHA1AND2-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms["PBEWITHSHA-1AND2-KEYDESEDE-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms["PBEWITHSHA-1AND2-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc.Id] = "PBEwithSHA-1and2-keyDESEDE-CBC";
            algorithms["PBEWITHSHAAND128BITRC2-CBC"] = "PBEwithSHA-1and128bitRC2-CBC";
            algorithms["PBEWITHSHA1AND128BITRC2-CBC"] = "PBEwithSHA-1and128bitRC2-CBC";
            algorithms["PBEWITHSHA-1AND128BITRC2-CBC"] = "PBEwithSHA-1and128bitRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc.Id] = "PBEwithSHA-1and128bitRC2-CBC";
            algorithms["PBEWITHSHAAND40BITRC2-CBC"] = "PBEwithSHA-1and40bitRC2-CBC";
            algorithms["PBEWITHSHA1AND40BITRC2-CBC"] = "PBEwithSHA-1and40bitRC2-CBC";
            algorithms["PBEWITHSHA-1AND40BITRC2-CBC"] = "PBEwithSHA-1and40bitRC2-CBC";
            algorithms[PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc.Id] = "PBEwithSHA-1and40bitRC2-CBC";
            algorithms["PBEWITHSHAAND128BITAES-CBC-BC"] = "PBEwithSHA-1and128bitAES-CBC-BC";
            algorithms["PBEWITHSHA1AND128BITAES-CBC-BC"] = "PBEwithSHA-1and128bitAES-CBC-BC";
            algorithms["PBEWITHSHA-1AND128BITAES-CBC-BC"] = "PBEwithSHA-1and128bitAES-CBC-BC";
            algorithms["PBEWITHSHAAND192BITAES-CBC-BC"] = "PBEwithSHA-1and192bitAES-CBC-BC";
            algorithms["PBEWITHSHA1AND192BITAES-CBC-BC"] = "PBEwithSHA-1and192bitAES-CBC-BC";
            algorithms["PBEWITHSHA-1AND192BITAES-CBC-BC"] = "PBEwithSHA-1and192bitAES-CBC-BC";
            algorithms["PBEWITHSHAAND256BITAES-CBC-BC"] = "PBEwithSHA-1and256bitAES-CBC-BC";
            algorithms["PBEWITHSHA1AND256BITAES-CBC-BC"] = "PBEwithSHA-1and256bitAES-CBC-BC";
            algorithms["PBEWITHSHA-1AND256BITAES-CBC-BC"] = "PBEwithSHA-1and256bitAES-CBC-BC";
            algorithms["PBEWITHSHA256AND128BITAES-CBC-BC"] = "PBEwithSHA-256and128bitAES-CBC-BC";
            algorithms["PBEWITHSHA-256AND128BITAES-CBC-BC"] = "PBEwithSHA-256and128bitAES-CBC-BC";
            algorithms["PBEWITHSHA256AND192BITAES-CBC-BC"] = "PBEwithSHA-256and192bitAES-CBC-BC";
            algorithms["PBEWITHSHA-256AND192BITAES-CBC-BC"] = "PBEwithSHA-256and192bitAES-CBC-BC";
            algorithms["PBEWITHSHA256AND256BITAES-CBC-BC"] = "PBEwithSHA-256and256bitAES-CBC-BC";
            algorithms["PBEWITHSHA-256AND256BITAES-CBC-BC"] = "PBEwithSHA-256and256bitAES-CBC-BC";
            algorithms["PBEWITHSHAANDIDEA"] = "PBEwithSHA-1andIDEA-CBC";
            algorithms["PBEWITHSHAANDIDEA-CBC"] = "PBEwithSHA-1andIDEA-CBC";
            algorithms["PBEWITHSHAANDTWOFISH"] = "PBEwithSHA-1andTWOFISH-CBC";
            algorithms["PBEWITHSHAANDTWOFISH-CBC"] = "PBEwithSHA-1andTWOFISH-CBC";
            algorithms["PBEWITHHMACSHA1"] = "PBEwithHmacSHA-1";
            algorithms["PBEWITHHMACSHA-1"] = "PBEwithHmacSHA-1";
            algorithms[OiwObjectIdentifiers.IdSha1.Id] = "PBEwithHmacSHA-1";
            algorithms["PBEWITHHMACSHA224"] = "PBEwithHmacSHA-224";
            algorithms["PBEWITHHMACSHA-224"] = "PBEwithHmacSHA-224";
            algorithms[NistObjectIdentifiers.IdSha224.Id] = "PBEwithHmacSHA-224";
            algorithms["PBEWITHHMACSHA256"] = "PBEwithHmacSHA-256";
            algorithms["PBEWITHHMACSHA-256"] = "PBEwithHmacSHA-256";
            algorithms[NistObjectIdentifiers.IdSha256.Id] = "PBEwithHmacSHA-256";
            algorithms["PBEWITHHMACRIPEMD128"] = "PBEwithHmacRipeMD128";
            algorithms[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "PBEwithHmacRipeMD128";
            algorithms["PBEWITHHMACRIPEMD160"] = "PBEwithHmacRipeMD160";
            algorithms[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "PBEwithHmacRipeMD160";
            algorithms["PBEWITHHMACRIPEMD256"] = "PBEwithHmacRipeMD256";
            algorithms[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "PBEwithHmacRipeMD256";
            algorithms["PBEWITHHMACTIGER"] = "PBEwithHmacTiger";
            algorithms["PBEWITHMD5AND128BITAES-CBC-OPENSSL"] = "PBEwithMD5and128bitAES-CBC-OpenSSL";
            algorithms["PBEWITHMD5AND192BITAES-CBC-OPENSSL"] = "PBEwithMD5and192bitAES-CBC-OpenSSL";
            algorithms["PBEWITHMD5AND256BITAES-CBC-OPENSSL"] = "PBEwithMD5and256bitAES-CBC-OpenSSL";
            algorithmType["Pkcs5scheme1"] = nameof( Pkcs5S1 );
            algorithmType["Pkcs5scheme2"] = nameof( Pkcs5S2 );
            algorithmType["PBEwithMD2andDES-CBC"] = nameof( Pkcs5S1 );
            algorithmType["PBEwithMD2andRC2-CBC"] = nameof( Pkcs5S1 );
            algorithmType["PBEwithMD5andDES-CBC"] = nameof( Pkcs5S1 );
            algorithmType["PBEwithMD5andRC2-CBC"] = nameof( Pkcs5S1 );
            algorithmType["PBEwithSHA-1andDES-CBC"] = nameof( Pkcs5S1 );
            algorithmType["PBEwithSHA-1andRC2-CBC"] = nameof( Pkcs5S1 );
            algorithmType[nameof( Pkcs12 )] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and128bitRC4"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and40bitRC4"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and3-keyDESEDE-CBC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and2-keyDESEDE-CBC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and128bitRC2-CBC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and40bitRC2-CBC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and128bitAES-CBC-BC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and192bitAES-CBC-BC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1and256bitAES-CBC-BC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-256and128bitAES-CBC-BC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-256and192bitAES-CBC-BC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-256and256bitAES-CBC-BC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1andIDEA-CBC"] = nameof( Pkcs12 );
            algorithmType["PBEwithSHA-1andTWOFISH-CBC"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacSHA-1"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacSHA-224"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacSHA-256"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacRipeMD128"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacRipeMD160"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacRipeMD256"] = nameof( Pkcs12 );
            algorithmType["PBEwithHmacTiger"] = nameof( Pkcs12 );
            algorithmType["PBEwithMD5and128bitAES-CBC-OpenSSL"] = nameof( OpenSsl );
            algorithmType["PBEwithMD5and192bitAES-CBC-OpenSSL"] = nameof( OpenSsl );
            algorithmType["PBEwithMD5and256bitAES-CBC-OpenSSL"] = nameof( OpenSsl );
            oids["PBEwithMD2andDES-CBC"] = PkcsObjectIdentifiers.PbeWithMD2AndDesCbc;
            oids["PBEwithMD2andRC2-CBC"] = PkcsObjectIdentifiers.PbeWithMD2AndRC2Cbc;
            oids["PBEwithMD5andDES-CBC"] = PkcsObjectIdentifiers.PbeWithMD5AndDesCbc;
            oids["PBEwithMD5andRC2-CBC"] = PkcsObjectIdentifiers.PbeWithMD5AndRC2Cbc;
            oids["PBEwithSHA-1andDES-CBC"] = PkcsObjectIdentifiers.PbeWithSha1AndDesCbc;
            oids["PBEwithSHA-1andRC2-CBC"] = PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc;
            oids["PBEwithSHA-1and128bitRC4"] = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4;
            oids["PBEwithSHA-1and40bitRC4"] = PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4;
            oids["PBEwithSHA-1and3-keyDESEDE-CBC"] = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc;
            oids["PBEwithSHA-1and2-keyDESEDE-CBC"] = PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc;
            oids["PBEwithSHA-1and128bitRC2-CBC"] = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc;
            oids["PBEwithSHA-1and40bitRC2-CBC"] = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc;
            oids["PBEwithHmacSHA-1"] = OiwObjectIdentifiers.IdSha1;
            oids["PBEwithHmacSHA-224"] = NistObjectIdentifiers.IdSha224;
            oids["PBEwithHmacSHA-256"] = NistObjectIdentifiers.IdSha256;
            oids["PBEwithHmacRipeMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
            oids["PBEwithHmacRipeMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
            oids["PBEwithHmacRipeMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
            oids["Pkcs5scheme2"] = PkcsObjectIdentifiers.IdPbeS2;
        }

        private static PbeParametersGenerator MakePbeGenerator(
          string type,
          IDigest digest,
          byte[] key,
          byte[] salt,
          int iterationCount )
        {
            PbeParametersGenerator parametersGenerator;
            switch (type)
            {
                case "Pkcs5S1":
                    parametersGenerator = new Pkcs5S1ParametersGenerator( digest );
                    break;
                case "Pkcs5S2":
                    parametersGenerator = new Pkcs5S2ParametersGenerator();
                    break;
                case "Pkcs12":
                    parametersGenerator = new Pkcs12ParametersGenerator( digest );
                    break;
                case "OpenSsl":
                    parametersGenerator = new OpenSslPbeParametersGenerator();
                    break;
                default:
                    throw new ArgumentException( "Unknown PBE type: " + type, nameof( type ) );
            }
            parametersGenerator.Init( key, salt, iterationCount );
            return parametersGenerator;
        }

        public static DerObjectIdentifier GetObjectIdentifier( string mechanism )
        {
            mechanism = (string)algorithms[Platform.ToUpperInvariant( mechanism )];
            return mechanism != null ? (DerObjectIdentifier)oids[mechanism] : null;
        }

        public static ICollection Algorithms => oids.Keys;

        public static bool IsPkcs12( string algorithm )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            return algorithm1 != null && "Pkcs12".Equals( algorithmType[algorithm1] );
        }

        public static bool IsPkcs5Scheme1( string algorithm )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            return algorithm1 != null && "Pkcs5S1".Equals( algorithmType[algorithm1] );
        }

        public static bool IsPkcs5Scheme2( string algorithm )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            return algorithm1 != null && "Pkcs5S2".Equals( algorithmType[algorithm1] );
        }

        public static bool IsOpenSsl( string algorithm )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            return algorithm1 != null && "OpenSsl".Equals( algorithmType[algorithm1] );
        }

        public static bool IsPbeAlgorithm( string algorithm )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            return algorithm1 != null && algorithmType[algorithm1] != null;
        }

        public static Asn1Encodable GenerateAlgorithmParameters(
          DerObjectIdentifier algorithmOid,
          byte[] salt,
          int iterationCount )
        {
            return GenerateAlgorithmParameters( algorithmOid.Id, salt, iterationCount );
        }

        public static Asn1Encodable GenerateAlgorithmParameters(
          string algorithm,
          byte[] salt,
          int iterationCount )
        {
            if (IsPkcs12( algorithm ))
                return new Pkcs12PbeParams( salt, iterationCount );
            return IsPkcs5Scheme2( algorithm ) ? new Pbkdf2Params( salt, iterationCount ) : (Asn1Encodable)new PbeParameter( salt, iterationCount );
        }

        public static ICipherParameters GenerateCipherParameters(
          DerObjectIdentifier algorithmOid,
          char[] password,
          Asn1Encodable pbeParameters )
        {
            return GenerateCipherParameters( algorithmOid.Id, password, false, pbeParameters );
        }

        public static ICipherParameters GenerateCipherParameters(
          DerObjectIdentifier algorithmOid,
          char[] password,
          bool wrongPkcs12Zero,
          Asn1Encodable pbeParameters )
        {
            return GenerateCipherParameters( algorithmOid.Id, password, wrongPkcs12Zero, pbeParameters );
        }

        public static ICipherParameters GenerateCipherParameters(
          AlgorithmIdentifier algID,
          char[] password )
        {
            return GenerateCipherParameters( algID.Algorithm.Id, password, false, algID.Parameters );
        }

        public static ICipherParameters GenerateCipherParameters(
          AlgorithmIdentifier algID,
          char[] password,
          bool wrongPkcs12Zero )
        {
            return GenerateCipherParameters( algID.Algorithm.Id, password, wrongPkcs12Zero, algID.Parameters );
        }

        public static ICipherParameters GenerateCipherParameters(
          string algorithm,
          char[] password,
          Asn1Encodable pbeParameters )
        {
            return GenerateCipherParameters( algorithm, password, false, pbeParameters );
        }

        public static ICipherParameters GenerateCipherParameters(
          string algorithm,
          char[] password,
          bool wrongPkcs12Zero,
          Asn1Encodable pbeParameters )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            byte[] key = null;
            byte[] salt1 = null;
            int iterationCount = 0;
            if (IsPkcs12( algorithm1 ))
            {
                Pkcs12PbeParams instance = Pkcs12PbeParams.GetInstance( pbeParameters );
                salt1 = instance.GetIV();
                iterationCount = instance.Iterations.IntValue;
                key = PbeParametersGenerator.Pkcs12PasswordToBytes( password, wrongPkcs12Zero );
            }
            else if (!IsPkcs5Scheme2( algorithm1 ))
            {
                PbeParameter instance = PbeParameter.GetInstance( pbeParameters );
                salt1 = instance.GetSalt();
                iterationCount = instance.IterationCount.IntValue;
                key = PbeParametersGenerator.Pkcs5PasswordToBytes( password );
            }
            ICipherParameters parameters = null;
            if (IsPkcs5Scheme2( algorithm1 ))
            {
                PbeS2Parameters instance1 = PbeS2Parameters.GetInstance( pbeParameters.ToAsn1Object() );
                AlgorithmIdentifier encryptionScheme = instance1.EncryptionScheme;
                DerObjectIdentifier algorithm2 = encryptionScheme.Algorithm;
                Asn1Object asn1Object = encryptionScheme.Parameters.ToAsn1Object();
                Pbkdf2Params instance2 = Pbkdf2Params.GetInstance( instance1.KeyDerivationFunc.Parameters.ToAsn1Object() );
                byte[] numArray = !algorithm2.Equals( PkcsObjectIdentifiers.RC2Cbc ) ? Asn1OctetString.GetInstance( asn1Object ).GetOctets() : RC2CbcParameter.GetInstance( asn1Object ).GetIV();
                byte[] salt2 = instance2.GetSalt();
                int intValue = instance2.IterationCount.IntValue;
                key = PbeParametersGenerator.Pkcs5PasswordToBytes( password );
                int keySize = instance2.KeyLength != null ? instance2.KeyLength.IntValue * 8 : GeneratorUtilities.GetDefaultKeySize( algorithm2 );
                parameters = MakePbeGenerator( (string)algorithmType[algorithm1], null, key, salt2, intValue ).GenerateDerivedParameters( algorithm2.Id, keySize );
                if (numArray != null && !Arrays.AreEqual( numArray, new byte[numArray.Length] ))
                    parameters = new ParametersWithIV( parameters, numArray );
            }
            else if (Platform.StartsWith( algorithm1, "PBEwithSHA-1" ))
            {
                PbeParametersGenerator parametersGenerator = MakePbeGenerator( (string)algorithmType[algorithm1], new Sha1Digest(), key, salt1, iterationCount );
                switch (algorithm1)
                {
                    case "PBEwithSHA-1and128bitAES-CBC-BC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 128, 128 );
                        break;
                    case "PBEwithSHA-1and192bitAES-CBC-BC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 192, 128 );
                        break;
                    case "PBEwithSHA-1and256bitAES-CBC-BC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 256, 128 );
                        break;
                    case "PBEwithSHA-1and128bitRC4":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC4", 128 );
                        break;
                    case "PBEwithSHA-1and40bitRC4":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC4", 40 );
                        break;
                    case "PBEwithSHA-1and3-keyDESEDE-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "DESEDE", 192, 64 );
                        break;
                    case "PBEwithSHA-1and2-keyDESEDE-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "DESEDE", 128, 64 );
                        break;
                    case "PBEwithSHA-1and128bitRC2-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC2", 128, 64 );
                        break;
                    case "PBEwithSHA-1and40bitRC2-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC2", 40, 64 );
                        break;
                    case "PBEwithSHA-1andDES-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "DES", 64, 64 );
                        break;
                    case "PBEwithSHA-1andRC2-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC2", 64, 64 );
                        break;
                }
            }
            else if (Platform.StartsWith( algorithm1, "PBEwithSHA-256" ))
            {
                PbeParametersGenerator parametersGenerator = MakePbeGenerator( (string)algorithmType[algorithm1], new Sha256Digest(), key, salt1, iterationCount );
                switch (algorithm1)
                {
                    case "PBEwithSHA-256and128bitAES-CBC-BC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 128, 128 );
                        break;
                    case "PBEwithSHA-256and192bitAES-CBC-BC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 192, 128 );
                        break;
                    case "PBEwithSHA-256and256bitAES-CBC-BC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 256, 128 );
                        break;
                }
            }
            else if (Platform.StartsWith( algorithm1, "PBEwithMD5" ))
            {
                PbeParametersGenerator parametersGenerator = MakePbeGenerator( (string)algorithmType[algorithm1], new MD5Digest(), key, salt1, iterationCount );
                switch (algorithm1)
                {
                    case "PBEwithMD5andDES-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "DES", 64, 64 );
                        break;
                    case "PBEwithMD5andRC2-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC2", 64, 64 );
                        break;
                    case "PBEwithMD5and128bitAES-CBC-OpenSSL":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 128, 128 );
                        break;
                    case "PBEwithMD5and192bitAES-CBC-OpenSSL":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 192, 128 );
                        break;
                    case "PBEwithMD5and256bitAES-CBC-OpenSSL":
                        parameters = parametersGenerator.GenerateDerivedParameters( "AES", 256, 128 );
                        break;
                }
            }
            else if (Platform.StartsWith( algorithm1, "PBEwithMD2" ))
            {
                PbeParametersGenerator parametersGenerator = MakePbeGenerator( (string)algorithmType[algorithm1], new MD2Digest(), key, salt1, iterationCount );
                switch (algorithm1)
                {
                    case "PBEwithMD2andDES-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "DES", 64, 64 );
                        break;
                    case "PBEwithMD2andRC2-CBC":
                        parameters = parametersGenerator.GenerateDerivedParameters( "RC2", 64, 64 );
                        break;
                }
            }
            else if (Platform.StartsWith( algorithm1, "PBEwithHmac" ))
            {
                IDigest digest = DigestUtilities.GetDigest( algorithm1.Substring( "PBEwithHmac".Length ) );
                parameters = MakePbeGenerator( (string)algorithmType[algorithm1], digest, key, salt1, iterationCount ).GenerateDerivedMacParameters( digest.GetDigestSize() * 8 );
            }
            Array.Clear( key, 0, key.Length );
            return FixDesParity( algorithm1, parameters );
        }

        public static object CreateEngine( DerObjectIdentifier algorithmOid ) => CreateEngine( algorithmOid.Id );

        public static object CreateEngine( AlgorithmIdentifier algID )
        {
            string id = algID.Algorithm.Id;
            return IsPkcs5Scheme2( id ) ? CipherUtilities.GetCipher( PbeS2Parameters.GetInstance( algID.Parameters.ToAsn1Object() ).EncryptionScheme.Algorithm ) : CreateEngine( id );
        }

        public static object CreateEngine( string algorithm )
        {
            string algorithm1 = (string)algorithms[Platform.ToUpperInvariant( algorithm )];
            if (Platform.StartsWith( algorithm1, "PBEwithHmac" ))
                return MacUtilities.GetMac( "HMAC/" + algorithm1.Substring( "PBEwithHmac".Length ) );
            if (Platform.StartsWith( algorithm1, "PBEwithMD2" ) || Platform.StartsWith( algorithm1, "PBEwithMD5" ) || Platform.StartsWith( algorithm1, "PBEwithSHA-1" ) || Platform.StartsWith( algorithm1, "PBEwithSHA-256" ))
            {
                if (Platform.EndsWith( algorithm1, "AES-CBC-BC" ) || Platform.EndsWith( algorithm1, "AES-CBC-OPENSSL" ))
                    return CipherUtilities.GetCipher( "AES/CBC" );
                if (Platform.EndsWith( algorithm1, "DES-CBC" ))
                    return CipherUtilities.GetCipher( "DES/CBC" );
                if (Platform.EndsWith( algorithm1, "DESEDE-CBC" ))
                    return CipherUtilities.GetCipher( "DESEDE/CBC" );
                if (Platform.EndsWith( algorithm1, "RC2-CBC" ))
                    return CipherUtilities.GetCipher( "RC2/CBC" );
                if (Platform.EndsWith( algorithm1, "RC4" ))
                    return CipherUtilities.GetCipher( "RC4" );
            }
            return null;
        }

        public static string GetEncodingName( DerObjectIdentifier oid ) => (string)algorithms[oid.Id];

        private static ICipherParameters FixDesParity( string mechanism, ICipherParameters parameters )
        {
            if (!Platform.EndsWith( mechanism, "DES-CBC" ) && !Platform.EndsWith( mechanism, "DESEDE-CBC" ))
                return parameters;
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                return new ParametersWithIV( FixDesParity( mechanism, parametersWithIv.Parameters ), parametersWithIv.GetIV() );
            }
            byte[] key = ((KeyParameter)parameters).GetKey();
            DesParameters.SetOddParity( key );
            return new KeyParameter( key );
        }
    }
}
