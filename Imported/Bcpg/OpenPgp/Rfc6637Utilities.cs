﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.Rfc6637Utilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public sealed class Rfc6637Utilities
    {
        private static readonly byte[] ANONYMOUS_SENDER = Hex.Decode( "416E6F6E796D6F75732053656E64657220202020" );

        private Rfc6637Utilities()
        {
        }

        public static string GetAgreementAlgorithm( PublicKeyPacket pubKeyData )
        {
            ECDHPublicBcpgKey key = (ECDHPublicBcpgKey)pubKeyData.Key;
            switch (key.HashAlgorithm)
            {
                case HashAlgorithmTag.Sha256:
                    return "ECCDHwithSHA256CKDF";
                case HashAlgorithmTag.Sha384:
                    return "ECCDHwithSHA384CKDF";
                case HashAlgorithmTag.Sha512:
                    return "ECCDHwithSHA512CKDF";
                default:
                    throw new ArgumentException( "Unknown hash algorithm specified: " + key.HashAlgorithm );
            }
        }

        public static DerObjectIdentifier GetKeyEncryptionOID( SymmetricKeyAlgorithmTag algID )
        {
            switch (algID)
            {
                case SymmetricKeyAlgorithmTag.Aes128:
                    return NistObjectIdentifiers.IdAes128Wrap;
                case SymmetricKeyAlgorithmTag.Aes192:
                    return NistObjectIdentifiers.IdAes192Wrap;
                case SymmetricKeyAlgorithmTag.Aes256:
                    return NistObjectIdentifiers.IdAes256Wrap;
                default:
                    throw new PgpException( "unknown symmetric algorithm ID: " + algID );
            }
        }

        public static int GetKeyLength( SymmetricKeyAlgorithmTag algID )
        {
            switch (algID)
            {
                case SymmetricKeyAlgorithmTag.Aes128:
                    return 16;
                case SymmetricKeyAlgorithmTag.Aes192:
                    return 24;
                case SymmetricKeyAlgorithmTag.Aes256:
                    return 32;
                default:
                    throw new PgpException( "unknown symmetric algorithm ID: " + algID );
            }
        }

        public static byte[] CreateKey( PublicKeyPacket pubKeyData, ECPoint s )
        {
            byte[] userKeyingMaterial = CreateUserKeyingMaterial( pubKeyData );
            ECDHPublicBcpgKey key = (ECDHPublicBcpgKey)pubKeyData.Key;
            return Kdf( key.HashAlgorithm, s, GetKeyLength( key.SymmetricKeyAlgorithm ), userKeyingMaterial );
        }

        public static byte[] CreateUserKeyingMaterial( PublicKeyPacket pubKeyData )
        {
            MemoryStream memoryStream = new();
            ECDHPublicBcpgKey key = (ECDHPublicBcpgKey)pubKeyData.Key;
            byte[] encoded = key.CurveOid.GetEncoded();
            memoryStream.Write( encoded, 1, encoded.Length - 1 );
            memoryStream.WriteByte( (byte)pubKeyData.Algorithm );
            memoryStream.WriteByte( 3 );
            memoryStream.WriteByte( 1 );
            memoryStream.WriteByte( (byte)key.HashAlgorithm );
            memoryStream.WriteByte( (byte)key.SymmetricKeyAlgorithm );
            memoryStream.Write( ANONYMOUS_SENDER, 0, ANONYMOUS_SENDER.Length );
            byte[] fingerprint = PgpPublicKey.CalculateFingerprint( pubKeyData );
            memoryStream.Write( fingerprint, 0, fingerprint.Length );
            return memoryStream.ToArray();
        }

        private static byte[] Kdf(
          HashAlgorithmTag digestAlg,
          ECPoint s,
          int keyLen,
          byte[] parameters )
        {
            byte[] encoded = s.XCoord.GetEncoded();
            IDigest digest = DigestUtilities.GetDigest( PgpUtilities.GetDigestName( digestAlg ) );
            digest.Update( 0 );
            digest.Update( 0 );
            digest.Update( 0 );
            digest.Update( 1 );
            digest.BlockUpdate( encoded, 0, encoded.Length );
            digest.BlockUpdate( parameters, 0, parameters.Length );
            return Arrays.CopyOfRange( DigestUtilities.DoFinal( digest ), 0, keyLen );
        }
    }
}
