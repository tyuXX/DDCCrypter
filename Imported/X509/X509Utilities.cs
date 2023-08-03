﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509Utilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.X509
{
    internal class X509Utilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();
        private static readonly IDictionary exParams = Platform.CreateHashtable();
        private static readonly ISet noParams = new HashSet();

        static X509Utilities()
        {
            algorithms.Add( "MD2WITHRSAENCRYPTION", PkcsObjectIdentifiers.MD2WithRsaEncryption );
            algorithms.Add( "MD2WITHRSA", PkcsObjectIdentifiers.MD2WithRsaEncryption );
            algorithms.Add( "MD5WITHRSAENCRYPTION", PkcsObjectIdentifiers.MD5WithRsaEncryption );
            algorithms.Add( "MD5WITHRSA", PkcsObjectIdentifiers.MD5WithRsaEncryption );
            algorithms.Add( "SHA1WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha1WithRsaEncryption );
            algorithms.Add( "SHA1WITHRSA", PkcsObjectIdentifiers.Sha1WithRsaEncryption );
            algorithms.Add( "SHA224WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha224WithRsaEncryption );
            algorithms.Add( "SHA224WITHRSA", PkcsObjectIdentifiers.Sha224WithRsaEncryption );
            algorithms.Add( "SHA256WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha256WithRsaEncryption );
            algorithms.Add( "SHA256WITHRSA", PkcsObjectIdentifiers.Sha256WithRsaEncryption );
            algorithms.Add( "SHA384WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha384WithRsaEncryption );
            algorithms.Add( "SHA384WITHRSA", PkcsObjectIdentifiers.Sha384WithRsaEncryption );
            algorithms.Add( "SHA512WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha512WithRsaEncryption );
            algorithms.Add( "SHA512WITHRSA", PkcsObjectIdentifiers.Sha512WithRsaEncryption );
            algorithms.Add( "SHA1WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss );
            algorithms.Add( "SHA224WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss );
            algorithms.Add( "SHA256WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss );
            algorithms.Add( "SHA384WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss );
            algorithms.Add( "SHA512WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss );
            algorithms.Add( "RIPEMD160WITHRSAENCRYPTION", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD160 );
            algorithms.Add( "RIPEMD160WITHRSA", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD160 );
            algorithms.Add( "RIPEMD128WITHRSAENCRYPTION", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD128 );
            algorithms.Add( "RIPEMD128WITHRSA", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD128 );
            algorithms.Add( "RIPEMD256WITHRSAENCRYPTION", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD256 );
            algorithms.Add( "RIPEMD256WITHRSA", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD256 );
            algorithms.Add( "SHA1WITHDSA", X9ObjectIdentifiers.IdDsaWithSha1 );
            algorithms.Add( "DSAWITHSHA1", X9ObjectIdentifiers.IdDsaWithSha1 );
            algorithms.Add( "SHA224WITHDSA", NistObjectIdentifiers.DsaWithSha224 );
            algorithms.Add( "SHA256WITHDSA", NistObjectIdentifiers.DsaWithSha256 );
            algorithms.Add( "SHA384WITHDSA", NistObjectIdentifiers.DsaWithSha384 );
            algorithms.Add( "SHA512WITHDSA", NistObjectIdentifiers.DsaWithSha512 );
            algorithms.Add( "SHA1WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha1 );
            algorithms.Add( "ECDSAWITHSHA1", X9ObjectIdentifiers.ECDsaWithSha1 );
            algorithms.Add( "SHA224WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha224 );
            algorithms.Add( "SHA256WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha256 );
            algorithms.Add( "SHA384WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha384 );
            algorithms.Add( "SHA512WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha512 );
            algorithms.Add( "GOST3411WITHGOST3410", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94 );
            algorithms.Add( "GOST3411WITHGOST3410-94", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94 );
            algorithms.Add( "GOST3411WITHECGOST3410", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001 );
            algorithms.Add( "GOST3411WITHECGOST3410-2001", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001 );
            algorithms.Add( "GOST3411WITHGOST3410-2001", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001 );
            noParams.Add( X9ObjectIdentifiers.ECDsaWithSha1 );
            noParams.Add( X9ObjectIdentifiers.ECDsaWithSha224 );
            noParams.Add( X9ObjectIdentifiers.ECDsaWithSha256 );
            noParams.Add( X9ObjectIdentifiers.ECDsaWithSha384 );
            noParams.Add( X9ObjectIdentifiers.ECDsaWithSha512 );
            noParams.Add( X9ObjectIdentifiers.IdDsaWithSha1 );
            noParams.Add( NistObjectIdentifiers.DsaWithSha224 );
            noParams.Add( NistObjectIdentifiers.DsaWithSha256 );
            noParams.Add( NistObjectIdentifiers.DsaWithSha384 );
            noParams.Add( NistObjectIdentifiers.DsaWithSha512 );
            noParams.Add( CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94 );
            noParams.Add( CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001 );
            AlgorithmIdentifier hashAlgId1 = new( OiwObjectIdentifiers.IdSha1, DerNull.Instance );
            exParams.Add( "SHA1WITHRSAANDMGF1", CreatePssParams( hashAlgId1, 20 ) );
            AlgorithmIdentifier hashAlgId2 = new( NistObjectIdentifiers.IdSha224, DerNull.Instance );
            exParams.Add( "SHA224WITHRSAANDMGF1", CreatePssParams( hashAlgId2, 28 ) );
            AlgorithmIdentifier hashAlgId3 = new( NistObjectIdentifiers.IdSha256, DerNull.Instance );
            exParams.Add( "SHA256WITHRSAANDMGF1", CreatePssParams( hashAlgId3, 32 ) );
            AlgorithmIdentifier hashAlgId4 = new( NistObjectIdentifiers.IdSha384, DerNull.Instance );
            exParams.Add( "SHA384WITHRSAANDMGF1", CreatePssParams( hashAlgId4, 48 ) );
            AlgorithmIdentifier hashAlgId5 = new( NistObjectIdentifiers.IdSha512, DerNull.Instance );
            exParams.Add( "SHA512WITHRSAANDMGF1", CreatePssParams( hashAlgId5, 64 ) );
        }

        private static RsassaPssParameters CreatePssParams( AlgorithmIdentifier hashAlgId, int saltSize ) => new( hashAlgId, new AlgorithmIdentifier( PkcsObjectIdentifiers.IdMgf1, hashAlgId ), new DerInteger( saltSize ), new DerInteger( 1 ) );

        internal static DerObjectIdentifier GetAlgorithmOid( string algorithmName )
        {
            algorithmName = Platform.ToUpperInvariant( algorithmName );
            return algorithms.Contains( algorithmName ) ? (DerObjectIdentifier)algorithms[algorithmName] : new DerObjectIdentifier( algorithmName );
        }

        internal static AlgorithmIdentifier GetSigAlgID(
          DerObjectIdentifier sigOid,
          string algorithmName )
        {
            if (noParams.Contains( sigOid ))
                return new AlgorithmIdentifier( sigOid );
            algorithmName = Platform.ToUpperInvariant( algorithmName );
            return exParams.Contains( algorithmName ) ? new AlgorithmIdentifier( sigOid, (Asn1Encodable)exParams[algorithmName] ) : new AlgorithmIdentifier( sigOid, DerNull.Instance );
        }

        internal static IEnumerable GetAlgNames() => new EnumerableProxy( algorithms.Keys );

        internal static byte[] GetSignatureForObject(
          DerObjectIdentifier sigOid,
          string sigName,
          AsymmetricKeyParameter privateKey,
          SecureRandom random,
          Asn1Encodable ae )
        {
            if (sigOid == null)
                throw new ArgumentNullException( nameof( sigOid ) );
            ISigner signer = SignerUtilities.GetSigner( sigName );
            if (random != null)
                signer.Init( true, new ParametersWithRandom( privateKey, random ) );
            else
                signer.Init( true, privateKey );
            byte[] derEncoded = ae.GetDerEncoded();
            signer.BlockUpdate( derEncoded, 0, derEncoded.Length );
            return signer.GenerateSignature();
        }
    }
}
