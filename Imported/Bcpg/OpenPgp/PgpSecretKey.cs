// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSecretKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSecretKey
    {
        private readonly SecretKeyPacket secret;
        private readonly PgpPublicKey pub;

        internal PgpSecretKey( SecretKeyPacket secret, PgpPublicKey pub )
        {
            this.secret = secret;
            this.pub = pub;
        }

        internal PgpSecretKey(
          PgpPrivateKey privKey,
          PgpPublicKey pubKey,
          SymmetricKeyAlgorithmTag encAlgorithm,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          bool useSha1,
          SecureRandom rand,
          bool isMasterKey )
        {
            this.pub = pubKey;
            BcpgObject bcpgObject;
            switch (pubKey.Algorithm)
            {
                case PublicKeyAlgorithmTag.RsaGeneral:
                case PublicKeyAlgorithmTag.RsaEncrypt:
                case PublicKeyAlgorithmTag.RsaSign:
                    RsaPrivateCrtKeyParameters key = (RsaPrivateCrtKeyParameters)privKey.Key;
                    bcpgObject = new RsaSecretBcpgKey( key.Exponent, key.P, key.Q );
                    break;
                case PublicKeyAlgorithmTag.ElGamalEncrypt:
                case PublicKeyAlgorithmTag.ElGamalGeneral:
                    bcpgObject = new ElGamalSecretBcpgKey( ((ElGamalPrivateKeyParameters)privKey.Key).X );
                    break;
                case PublicKeyAlgorithmTag.Dsa:
                    bcpgObject = new DsaSecretBcpgKey( ((DsaPrivateKeyParameters)privKey.Key).X );
                    break;
                case PublicKeyAlgorithmTag.EC:
                case PublicKeyAlgorithmTag.ECDsa:
                    bcpgObject = new ECSecretBcpgKey( ((ECPrivateKeyParameters)privKey.Key).D );
                    break;
                default:
                    throw new PgpException( "unknown key class" );
            }
            try
            {
                MemoryStream outStr = new();
                new BcpgOutputStream( outStr ).WriteObject( bcpgObject );
                byte[] array = outStr.ToArray();
                byte[] b = Checksum( useSha1, array, array.Length );
                byte[] numArray = Arrays.Concatenate( array, b );
                if (encAlgorithm == SymmetricKeyAlgorithmTag.Null)
                {
                    if (isMasterKey)
                        this.secret = new SecretKeyPacket( this.pub.publicPk, encAlgorithm, null, null, numArray );
                    else
                        this.secret = new SecretSubkeyPacket( this.pub.publicPk, encAlgorithm, null, null, numArray );
                }
                else
                {
                    S2k s2k;
                    byte[] iv;
                    byte[] secKeyData = this.pub.Version < 4 ? EncryptKeyDataV3( numArray, encAlgorithm, rawPassPhrase, clearPassPhrase, rand, out s2k, out iv ) : EncryptKeyDataV4( numArray, encAlgorithm, HashAlgorithmTag.Sha1, rawPassPhrase, clearPassPhrase, rand, out s2k, out iv );
                    int s2kUsage = useSha1 ? 254 : byte.MaxValue;
                    if (isMasterKey)
                        this.secret = new SecretKeyPacket( this.pub.publicPk, encAlgorithm, s2kUsage, s2k, iv, secKeyData );
                    else
                        this.secret = new SecretSubkeyPacket( this.pub.publicPk, encAlgorithm, s2kUsage, s2k, iv, secKeyData );
                }
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception encrypting key", ex );
            }
        }

        [Obsolete( "Use the constructor taking an explicit 'useSha1' parameter instead" )]
        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          char[] passPhrase,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, passPhrase, false, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, false, passPhrase, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          bool utf8PassPhrase,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, PgpUtilities.EncodePassPhrase( passPhrase, utf8PassPhrase ), true, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          byte[] rawPassPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, rawPassPhrase, false, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        internal PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( keyPair.PrivateKey, CertifiedPublicKey( certificationLevel, keyPair, id, hashedPackets, unhashedPackets ), encAlgorithm, rawPassPhrase, clearPassPhrase, useSha1, rand, true )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, hashAlgorithm, false, passPhrase, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          bool utf8PassPhrase,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, hashAlgorithm, PgpUtilities.EncodePassPhrase( passPhrase, utf8PassPhrase ), true, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          byte[] rawPassPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, keyPair, id, encAlgorithm, hashAlgorithm, rawPassPhrase, false, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        internal PgpSecretKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( keyPair.PrivateKey, CertifiedPublicKey( certificationLevel, keyPair, id, hashedPackets, unhashedPackets, hashAlgorithm ), encAlgorithm, rawPassPhrase, clearPassPhrase, useSha1, rand, true )
        {
        }

        private static PgpPublicKey CertifiedPublicKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets )
        {
            PgpSignatureGenerator signatureGenerator;
            try
            {
                signatureGenerator = new PgpSignatureGenerator( keyPair.PublicKey.Algorithm, HashAlgorithmTag.Sha1 );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Creating signature generator: " + ex.Message, ex );
            }
            signatureGenerator.InitSign( certificationLevel, keyPair.PrivateKey );
            signatureGenerator.SetHashedSubpackets( hashedPackets );
            signatureGenerator.SetUnhashedSubpackets( unhashedPackets );
            try
            {
                PgpSignature certification = signatureGenerator.GenerateCertification( id, keyPair.PublicKey );
                return PgpPublicKey.AddCertification( keyPair.PublicKey, id, certification );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception doing certification: " + ex.Message, ex );
            }
        }

        private static PgpPublicKey CertifiedPublicKey(
          int certificationLevel,
          PgpKeyPair keyPair,
          string id,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          HashAlgorithmTag hashAlgorithm )
        {
            PgpSignatureGenerator signatureGenerator;
            try
            {
                signatureGenerator = new PgpSignatureGenerator( keyPair.PublicKey.Algorithm, hashAlgorithm );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Creating signature generator: " + ex.Message, ex );
            }
            signatureGenerator.InitSign( certificationLevel, keyPair.PrivateKey );
            signatureGenerator.SetHashedSubpackets( hashedPackets );
            signatureGenerator.SetUnhashedSubpackets( unhashedPackets );
            try
            {
                PgpSignature certification = signatureGenerator.GenerateCertification( id, keyPair.PublicKey );
                return PgpPublicKey.AddCertification( keyPair.PublicKey, id, certification );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception doing certification: " + ex.Message, ex );
            }
        }

        public PgpSecretKey(
          int certificationLevel,
          PublicKeyAlgorithmTag algorithm,
          AsymmetricKeyParameter pubKey,
          AsymmetricKeyParameter privKey,
          DateTime time,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          char[] passPhrase,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, new PgpKeyPair( algorithm, pubKey, privKey, time ), id, encAlgorithm, passPhrase, false, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpSecretKey(
          int certificationLevel,
          PublicKeyAlgorithmTag algorithm,
          AsymmetricKeyParameter pubKey,
          AsymmetricKeyParameter privKey,
          DateTime time,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, new PgpKeyPair( algorithm, pubKey, privKey, time ), id, encAlgorithm, passPhrase, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public bool IsSigningKey
        {
            get
            {
                switch (this.pub.Algorithm)
                {
                    case PublicKeyAlgorithmTag.RsaGeneral:
                    case PublicKeyAlgorithmTag.RsaSign:
                    case PublicKeyAlgorithmTag.Dsa:
                    case PublicKeyAlgorithmTag.ECDsa:
                    case PublicKeyAlgorithmTag.ElGamalGeneral:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsMasterKey => this.pub.IsMasterKey;

        public bool IsPrivateKeyEmpty
        {
            get
            {
                byte[] secretKeyData = this.secret.GetSecretKeyData();
                return secretKeyData == null || secretKeyData.Length < 1;
            }
        }

        public SymmetricKeyAlgorithmTag KeyEncryptionAlgorithm => this.secret.EncAlgorithm;

        public long KeyId => this.pub.KeyId;

        public int S2kUsage => this.secret.S2kUsage;

        public S2k S2k => this.secret.S2k;

        public PgpPublicKey PublicKey => this.pub;

        public IEnumerable UserIds => this.pub.GetUserIds();

        public IEnumerable UserAttributes => this.pub.GetUserAttributes();

        private byte[] ExtractKeyData( byte[] rawPassPhrase, bool clearPassPhrase )
        {
            SymmetricKeyAlgorithmTag encAlgorithm = this.secret.EncAlgorithm;
            byte[] secretKeyData = this.secret.GetSecretKeyData();
            if (encAlgorithm == SymmetricKeyAlgorithmTag.Null)
                return secretKeyData;
            try
            {
                KeyParameter key = PgpUtilities.DoMakeKeyFromPassPhrase( this.secret.EncAlgorithm, this.secret.S2k, rawPassPhrase, clearPassPhrase );
                byte[] iv = this.secret.GetIV();
                byte[] keyData;
                if (this.secret.PublicKeyPacket.Version >= 4)
                {
                    keyData = RecoverKeyData( encAlgorithm, "/CFB/NoPadding", key, iv, secretKeyData, 0, secretKeyData.Length );
                    bool useSha1 = this.secret.S2kUsage == 254;
                    byte[] numArray = Checksum( useSha1, keyData, useSha1 ? keyData.Length - 20 : keyData.Length - 2 );
                    for (int index = 0; index != numArray.Length; ++index)
                    {
                        if (numArray[index] != keyData[keyData.Length - numArray.Length + index])
                            throw new PgpException( "Checksum mismatch at " + index + " of " + numArray.Length );
                    }
                }
                else
                {
                    keyData = new byte[secretKeyData.Length];
                    byte[] numArray = Arrays.Clone( iv );
                    int index1 = 0;
                    for (int index2 = 0; index2 != 4; ++index2)
                    {
                        int num1 = (((secretKeyData[index1] << 8) | (secretKeyData[index1 + 1] & byte.MaxValue)) + 7) / 8;
                        keyData[index1] = secretKeyData[index1];
                        keyData[index1 + 1] = secretKeyData[index1 + 1];
                        int num2 = index1 + 2;
                        Array.Copy( RecoverKeyData( encAlgorithm, "/CFB/NoPadding", key, numArray, secretKeyData, num2, num1 ), 0, keyData, num2, num1 );
                        index1 = num2 + num1;
                        if (index2 != 3)
                            Array.Copy( secretKeyData, index1 - numArray.Length, numArray, 0, numArray.Length );
                    }
                    keyData[index1] = secretKeyData[index1];
                    keyData[index1 + 1] = secretKeyData[index1 + 1];
                    int num3 = ((secretKeyData[index1] << 8) & 65280) | (secretKeyData[index1 + 1] & byte.MaxValue);
                    int num4 = 0;
                    for (int index3 = 0; index3 < index1; ++index3)
                        num4 += keyData[index3] & byte.MaxValue;
                    int num5 = num4 & ushort.MaxValue;
                    if (num5 != num3)
                        throw new PgpException( "Checksum mismatch: passphrase wrong, expected " + num3.ToString( "X" ) + " found " + num5.ToString( "X" ) );
                }
                return keyData;
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception decrypting key", ex );
            }
        }

        private static byte[] RecoverKeyData(
          SymmetricKeyAlgorithmTag encAlgorithm,
          string modeAndPadding,
          KeyParameter key,
          byte[] iv,
          byte[] keyData,
          int keyOff,
          int keyLen )
        {
            IBufferedCipher cipher;
            try
            {
                cipher = CipherUtilities.GetCipher( PgpUtilities.GetSymmetricCipherName( encAlgorithm ) + modeAndPadding );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception creating cipher", ex );
            }
            cipher.Init( false, new ParametersWithIV( key, iv ) );
            return cipher.DoFinal( keyData, keyOff, keyLen );
        }

        public PgpPrivateKey ExtractPrivateKey( char[] passPhrase ) => this.DoExtractPrivateKey( PgpUtilities.EncodePassPhrase( passPhrase, false ), true );

        public PgpPrivateKey ExtractPrivateKeyUtf8( char[] passPhrase ) => this.DoExtractPrivateKey( PgpUtilities.EncodePassPhrase( passPhrase, true ), true );

        public PgpPrivateKey ExtractPrivateKeyRaw( byte[] rawPassPhrase ) => this.DoExtractPrivateKey( rawPassPhrase, false );

        internal PgpPrivateKey DoExtractPrivateKey( byte[] rawPassPhrase, bool clearPassPhrase )
        {
            if (this.IsPrivateKeyEmpty)
                return null;
            PublicKeyPacket publicKeyPacket = this.secret.PublicKeyPacket;
            try
            {
                BcpgInputStream bcpgIn = BcpgInputStream.Wrap( new MemoryStream( this.ExtractKeyData( rawPassPhrase, clearPassPhrase ), false ) );
                AsymmetricKeyParameter privateKey;
                switch (publicKeyPacket.Algorithm)
                {
                    case PublicKeyAlgorithmTag.RsaGeneral:
                    case PublicKeyAlgorithmTag.RsaEncrypt:
                    case PublicKeyAlgorithmTag.RsaSign:
                        RsaPublicBcpgKey key1 = (RsaPublicBcpgKey)publicKeyPacket.Key;
                        RsaSecretBcpgKey rsaSecretBcpgKey = new( bcpgIn );
                        privateKey = new RsaPrivateCrtKeyParameters( rsaSecretBcpgKey.Modulus, key1.PublicExponent, rsaSecretBcpgKey.PrivateExponent, rsaSecretBcpgKey.PrimeP, rsaSecretBcpgKey.PrimeQ, rsaSecretBcpgKey.PrimeExponentP, rsaSecretBcpgKey.PrimeExponentQ, rsaSecretBcpgKey.CrtCoefficient );
                        break;
                    case PublicKeyAlgorithmTag.ElGamalEncrypt:
                    case PublicKeyAlgorithmTag.ElGamalGeneral:
                        ElGamalPublicBcpgKey key2 = (ElGamalPublicBcpgKey)publicKeyPacket.Key;
                        privateKey = new ElGamalPrivateKeyParameters( new ElGamalSecretBcpgKey( bcpgIn ).X, new ElGamalParameters( key2.P, key2.G ) );
                        break;
                    case PublicKeyAlgorithmTag.Dsa:
                        DsaPublicBcpgKey key3 = (DsaPublicBcpgKey)publicKeyPacket.Key;
                        privateKey = new DsaPrivateKeyParameters( new DsaSecretBcpgKey( bcpgIn ).X, new DsaParameters( key3.P, key3.Q, key3.G ) );
                        break;
                    case PublicKeyAlgorithmTag.EC:
                        privateKey = this.GetECKey( "ECDH", bcpgIn );
                        break;
                    case PublicKeyAlgorithmTag.ECDsa:
                        privateKey = this.GetECKey( "ECDSA", bcpgIn );
                        break;
                    default:
                        throw new PgpException( "unknown public key algorithm encountered" );
                }
                return new PgpPrivateKey( this.KeyId, publicKeyPacket, privateKey );
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception constructing key", ex );
            }
        }

        private ECPrivateKeyParameters GetECKey( string algorithm, BcpgInputStream bcpgIn )
        {
            ECPublicBcpgKey key = (ECPublicBcpgKey)this.secret.PublicKeyPacket.Key;
            ECSecretBcpgKey ecSecretBcpgKey = new( bcpgIn );
            return new ECPrivateKeyParameters( algorithm, ecSecretBcpgKey.X, key.CurveOid );
        }

        private static byte[] Checksum( bool useSha1, byte[] bytes, int length )
        {
            if (useSha1)
            {
                try
                {
                    IDigest digest = DigestUtilities.GetDigest( "SHA1" );
                    digest.BlockUpdate( bytes, 0, length );
                    return DigestUtilities.DoFinal( digest );
                }
                catch (Exception ex)
                {
                    throw new PgpException( "Can't find SHA-1", ex );
                }
            }
            else
            {
                int num = 0;
                for (int index = 0; index != length; ++index)
                    num += bytes[index];
                return new byte[2] { (byte)(num >> 8), (byte)num };
            }
        }

        public byte[] GetEncoded()
        {
            MemoryStream outStr = new();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public void Encode( Stream outStr )
        {
            BcpgOutputStream outStream = BcpgOutputStream.Wrap( outStr );
            outStream.WritePacket( secret );
            if (this.pub.trustPk != null)
                outStream.WritePacket( pub.trustPk );
            if (this.pub.subSigs == null)
            {
                foreach (PgpSignature keySig in (IEnumerable)this.pub.keySigs)
                    keySig.Encode( outStream );
                for (int index = 0; index != this.pub.ids.Count; ++index)
                {
                    object id1 = this.pub.ids[index];
                    if (id1 is string)
                    {
                        string id2 = (string)id1;
                        outStream.WritePacket( new UserIdPacket( id2 ) );
                    }
                    else
                    {
                        PgpUserAttributeSubpacketVector attributeSubpacketVector = (PgpUserAttributeSubpacketVector)id1;
                        outStream.WritePacket( new UserAttributePacket( attributeSubpacketVector.ToSubpacketArray() ) );
                    }
                    if (this.pub.idTrusts[index] != null)
                        outStream.WritePacket( (ContainedPacket)this.pub.idTrusts[index] );
                    foreach (PgpSignature pgpSignature in (IEnumerable)this.pub.idSigs[index])
                        pgpSignature.Encode( outStream );
                }
            }
            else
            {
                foreach (PgpSignature subSig in (IEnumerable)this.pub.subSigs)
                    subSig.Encode( outStream );
            }
        }

        public static PgpSecretKey CopyWithNewPassword(
          PgpSecretKey key,
          char[] oldPassPhrase,
          char[] newPassPhrase,
          SymmetricKeyAlgorithmTag newEncAlgorithm,
          SecureRandom rand )
        {
            return DoCopyWithNewPassword( key, PgpUtilities.EncodePassPhrase( oldPassPhrase, false ), PgpUtilities.EncodePassPhrase( newPassPhrase, false ), true, newEncAlgorithm, rand );
        }

        public static PgpSecretKey CopyWithNewPasswordUtf8(
          PgpSecretKey key,
          char[] oldPassPhrase,
          char[] newPassPhrase,
          SymmetricKeyAlgorithmTag newEncAlgorithm,
          SecureRandom rand )
        {
            return DoCopyWithNewPassword( key, PgpUtilities.EncodePassPhrase( oldPassPhrase, true ), PgpUtilities.EncodePassPhrase( newPassPhrase, true ), true, newEncAlgorithm, rand );
        }

        public static PgpSecretKey CopyWithNewPasswordRaw(
          PgpSecretKey key,
          byte[] rawOldPassPhrase,
          byte[] rawNewPassPhrase,
          SymmetricKeyAlgorithmTag newEncAlgorithm,
          SecureRandom rand )
        {
            return DoCopyWithNewPassword( key, rawOldPassPhrase, rawNewPassPhrase, false, newEncAlgorithm, rand );
        }

        internal static PgpSecretKey DoCopyWithNewPassword(
          PgpSecretKey key,
          byte[] rawOldPassPhrase,
          byte[] rawNewPassPhrase,
          bool clearPassPhrase,
          SymmetricKeyAlgorithmTag newEncAlgorithm,
          SecureRandom rand )
        {
            if (key.IsPrivateKeyEmpty)
                throw new PgpException( "no private key in this SecretKey - public key present only." );
            byte[] keyData = key.ExtractKeyData( rawOldPassPhrase, clearPassPhrase );
            int s2kUsage = key.secret.S2kUsage;
            byte[] iv = null;
            S2k s2k = null;
            PublicKeyPacket publicKeyPacket = key.secret.PublicKeyPacket;
            byte[] numArray1;
            if (newEncAlgorithm == SymmetricKeyAlgorithmTag.Null)
            {
                s2kUsage = 0;
                if (key.secret.S2kUsage == 254)
                {
                    numArray1 = new byte[keyData.Length - 18];
                    Array.Copy( keyData, 0, numArray1, 0, numArray1.Length - 2 );
                    byte[] numArray2 = Checksum( false, numArray1, numArray1.Length - 2 );
                    numArray1[numArray1.Length - 2] = numArray2[0];
                    numArray1[numArray1.Length - 1] = numArray2[1];
                }
                else
                    numArray1 = keyData;
            }
            else
            {
                if (s2kUsage == 0)
                    s2kUsage = byte.MaxValue;
                try
                {
                    numArray1 = publicKeyPacket.Version < 4 ? EncryptKeyDataV3( keyData, newEncAlgorithm, rawNewPassPhrase, clearPassPhrase, rand, out s2k, out iv ) : EncryptKeyDataV4( keyData, newEncAlgorithm, HashAlgorithmTag.Sha1, rawNewPassPhrase, clearPassPhrase, rand, out s2k, out iv );
                }
                catch (PgpException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new PgpException( "Exception encrypting key", ex );
                }
            }
            return new PgpSecretKey( !(key.secret is SecretSubkeyPacket) ? new SecretKeyPacket( publicKeyPacket, newEncAlgorithm, s2kUsage, s2k, iv, numArray1 ) : new SecretSubkeyPacket( publicKeyPacket, newEncAlgorithm, s2kUsage, s2k, iv, numArray1 ), key.pub );
        }

        public static PgpSecretKey ReplacePublicKey( PgpSecretKey secretKey, PgpPublicKey publicKey )
        {
            if (publicKey.KeyId != secretKey.KeyId)
                throw new ArgumentException( "KeyId's do not match" );
            return new PgpSecretKey( secretKey.secret, publicKey );
        }

        private static byte[] EncryptKeyDataV3(
          byte[] rawKeyData,
          SymmetricKeyAlgorithmTag encAlgorithm,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          SecureRandom random,
          out S2k s2k,
          out byte[] iv )
        {
            s2k = null;
            iv = null;
            KeyParameter key = PgpUtilities.DoMakeKeyFromPassPhrase( encAlgorithm, s2k, rawPassPhrase, clearPassPhrase );
            byte[] numArray = new byte[rawKeyData.Length];
            int to = 0;
            for (int index = 0; index != 4; ++index)
            {
                int dataLen = (((rawKeyData[to] << 8) | (rawKeyData[to + 1] & byte.MaxValue)) + 7) / 8;
                numArray[to] = rawKeyData[to];
                numArray[to + 1] = rawKeyData[to + 1];
                byte[] sourceArray;
                if (index == 0)
                {
                    sourceArray = EncryptData( encAlgorithm, key, rawKeyData, to + 2, dataLen, random, ref iv );
                }
                else
                {
                    byte[] iv1 = Arrays.CopyOfRange( numArray, to - iv.Length, to );
                    sourceArray = EncryptData( encAlgorithm, key, rawKeyData, to + 2, dataLen, random, ref iv1 );
                }
                Array.Copy( sourceArray, 0, numArray, to + 2, sourceArray.Length );
                to += 2 + dataLen;
            }
            numArray[to] = rawKeyData[to];
            numArray[to + 1] = rawKeyData[to + 1];
            return numArray;
        }

        private static byte[] EncryptKeyDataV4(
          byte[] rawKeyData,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          SecureRandom random,
          out S2k s2k,
          out byte[] iv )
        {
            s2k = PgpUtilities.GenerateS2k( hashAlgorithm, 96, random );
            KeyParameter key = PgpUtilities.DoMakeKeyFromPassPhrase( encAlgorithm, s2k, rawPassPhrase, clearPassPhrase );
            iv = null;
            return EncryptData( encAlgorithm, key, rawKeyData, 0, rawKeyData.Length, random, ref iv );
        }

        private static byte[] EncryptData(
          SymmetricKeyAlgorithmTag encAlgorithm,
          KeyParameter key,
          byte[] data,
          int dataOff,
          int dataLen,
          SecureRandom random,
          ref byte[] iv )
        {
            IBufferedCipher cipher;
            try
            {
                cipher = CipherUtilities.GetCipher( PgpUtilities.GetSymmetricCipherName( encAlgorithm ) + "/CFB/NoPadding" );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception creating cipher", ex );
            }
            if (iv == null)
                iv = PgpUtilities.GenerateIV( cipher.GetBlockSize(), random );
            cipher.Init( true, new ParametersWithRandom( new ParametersWithIV( key, iv ), random ) );
            return cipher.DoFinal( data, dataOff, dataLen );
        }

        public static PgpSecretKey ParseSecretKeyFromSExpr(
          Stream inputStream,
          char[] passPhrase,
          PgpPublicKey pubKey )
        {
            return DoParseSecretKeyFromSExpr( inputStream, PgpUtilities.EncodePassPhrase( passPhrase, false ), true, pubKey );
        }

        public static PgpSecretKey ParseSecretKeyFromSExprUtf8(
          Stream inputStream,
          char[] passPhrase,
          PgpPublicKey pubKey )
        {
            return DoParseSecretKeyFromSExpr( inputStream, PgpUtilities.EncodePassPhrase( passPhrase, true ), true, pubKey );
        }

        public static PgpSecretKey ParseSecretKeyFromSExprRaw(
          Stream inputStream,
          byte[] rawPassPhrase,
          PgpPublicKey pubKey )
        {
            return DoParseSecretKeyFromSExpr( inputStream, rawPassPhrase, false, pubKey );
        }

        internal static PgpSecretKey DoParseSecretKeyFromSExpr(
          Stream inputStream,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          PgpPublicKey pubKey )
        {
            SXprUtilities.SkipOpenParenthesis( inputStream );
            if (!SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "protected-private-key" ))
                throw new PgpException( "unknown key type found" );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            if (!SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "ecc" ))
                throw new PgpException( "no curve details found" );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            SXprUtilities.ReadString( inputStream, inputStream.ReadByte() );
            string curveName = SXprUtilities.ReadString( inputStream, inputStream.ReadByte() );
            SXprUtilities.SkipCloseParenthesis( inputStream );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            if (!SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "q" ))
                throw new PgpException( "no q value found" );
            SXprUtilities.ReadBytes( inputStream, inputStream.ReadByte() );
            SXprUtilities.SkipCloseParenthesis( inputStream );
            byte[] dvalue = GetDValue( inputStream, rawPassPhrase, clearPassPhrase, curveName );
            return new PgpSecretKey( new SecretKeyPacket( pubKey.PublicKeyPacket, SymmetricKeyAlgorithmTag.Null, null, null, new ECSecretBcpgKey( new BigInteger( 1, dvalue ) ).GetEncoded() ), pubKey );
        }

        public static PgpSecretKey ParseSecretKeyFromSExpr( Stream inputStream, char[] passPhrase ) => DoParseSecretKeyFromSExpr( inputStream, PgpUtilities.EncodePassPhrase( passPhrase, false ), true );

        public static PgpSecretKey ParseSecretKeyFromSExprUtf8( Stream inputStream, char[] passPhrase ) => DoParseSecretKeyFromSExpr( inputStream, PgpUtilities.EncodePassPhrase( passPhrase, true ), true );

        public static PgpSecretKey ParseSecretKeyFromSExprRaw( Stream inputStream, byte[] rawPassPhrase ) => DoParseSecretKeyFromSExpr( inputStream, rawPassPhrase, false );

        internal static PgpSecretKey DoParseSecretKeyFromSExpr(
          Stream inputStream,
          byte[] rawPassPhrase,
          bool clearPassPhrase )
        {
            SXprUtilities.SkipOpenParenthesis( inputStream );
            if (!SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "protected-private-key" ))
                throw new PgpException( "unknown key type found" );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            if (!SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "ecc" ))
                throw new PgpException( "no curve details found" );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            SXprUtilities.ReadString( inputStream, inputStream.ReadByte() );
            string str = SXprUtilities.ReadString( inputStream, inputStream.ReadByte() );
            if (Platform.StartsWith( str, "NIST " ))
                str = str.Substring( "NIST ".Length );
            SXprUtilities.SkipCloseParenthesis( inputStream );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            byte[] bytes = SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "q" ) ? SXprUtilities.ReadBytes( inputStream, inputStream.ReadByte() ) : throw new PgpException( "no q value found" );
            PublicKeyPacket publicKeyPacket = new( PublicKeyAlgorithmTag.ECDsa, DateTime.UtcNow, new ECDsaPublicBcpgKey( ECNamedCurveTable.GetOid( str ), new BigInteger( 1, bytes ) ) );
            SXprUtilities.SkipCloseParenthesis( inputStream );
            byte[] dvalue = GetDValue( inputStream, rawPassPhrase, clearPassPhrase, str );
            return new PgpSecretKey( new SecretKeyPacket( publicKeyPacket, SymmetricKeyAlgorithmTag.Null, null, null, new ECSecretBcpgKey( new BigInteger( 1, dvalue ) ).GetEncoded() ), new PgpPublicKey( publicKeyPacket ) );
        }

        private static byte[] GetDValue(
          Stream inputStream,
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          string curveName )
        {
            SXprUtilities.SkipOpenParenthesis( inputStream );
            if (!SXprUtilities.ReadString( inputStream, inputStream.ReadByte() ).Equals( "protected" ))
                throw new PgpException( "protected block not found" );
            SXprUtilities.ReadString( inputStream, inputStream.ReadByte() );
            SXprUtilities.SkipOpenParenthesis( inputStream );
            S2k s2k = SXprUtilities.ParseS2k( inputStream );
            byte[] iv = SXprUtilities.ReadBytes( inputStream, inputStream.ReadByte() );
            SXprUtilities.SkipCloseParenthesis( inputStream );
            byte[] keyData = SXprUtilities.ReadBytes( inputStream, inputStream.ReadByte() );
            Stream input = new MemoryStream( RecoverKeyData( SymmetricKeyAlgorithmTag.Aes128, "/CBC/NoPadding", PgpUtilities.DoMakeKeyFromPassPhrase( SymmetricKeyAlgorithmTag.Aes128, s2k, rawPassPhrase, clearPassPhrase ), iv, keyData, 0, keyData.Length ), false );
            SXprUtilities.SkipOpenParenthesis( input );
            SXprUtilities.SkipOpenParenthesis( input );
            SXprUtilities.SkipOpenParenthesis( input );
            SXprUtilities.ReadString( input, input.ReadByte() );
            return SXprUtilities.ReadBytes( input, input.ReadByte() );
        }
    }
}
