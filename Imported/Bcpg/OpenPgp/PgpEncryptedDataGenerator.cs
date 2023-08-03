// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpEncryptedDataGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpEncryptedDataGenerator : IStreamGenerator
    {
        private BcpgOutputStream pOut;
        private CipherStream cOut;
        private IBufferedCipher c;
        private bool withIntegrityPacket;
        private bool oldFormat;
        private DigestStream digestOut;
        private readonly IList methods = Platform.CreateArrayList();
        private readonly SymmetricKeyAlgorithmTag defAlgorithm;
        private readonly SecureRandom rand;

        public PgpEncryptedDataGenerator( SymmetricKeyAlgorithmTag encAlgorithm )
        {
            this.defAlgorithm = encAlgorithm;
            this.rand = new SecureRandom();
        }

        public PgpEncryptedDataGenerator(
          SymmetricKeyAlgorithmTag encAlgorithm,
          bool withIntegrityPacket )
        {
            this.defAlgorithm = encAlgorithm;
            this.withIntegrityPacket = withIntegrityPacket;
            this.rand = new SecureRandom();
        }

        public PgpEncryptedDataGenerator( SymmetricKeyAlgorithmTag encAlgorithm, SecureRandom rand )
        {
            this.defAlgorithm = encAlgorithm;
            this.rand = rand;
        }

        public PgpEncryptedDataGenerator(
          SymmetricKeyAlgorithmTag encAlgorithm,
          bool withIntegrityPacket,
          SecureRandom rand )
        {
            this.defAlgorithm = encAlgorithm;
            this.rand = rand;
            this.withIntegrityPacket = withIntegrityPacket;
        }

        public PgpEncryptedDataGenerator(
          SymmetricKeyAlgorithmTag encAlgorithm,
          SecureRandom rand,
          bool oldFormat )
        {
            this.defAlgorithm = encAlgorithm;
            this.rand = rand;
            this.oldFormat = oldFormat;
        }

        [Obsolete( "Use version that takes an explicit s2kDigest parameter" )]
        public void AddMethod( char[] passPhrase ) => this.AddMethod( passPhrase, HashAlgorithmTag.Sha1 );

        public void AddMethod( char[] passPhrase, HashAlgorithmTag s2kDigest ) => this.DoAddMethod( PgpUtilities.EncodePassPhrase( passPhrase, false ), true, s2kDigest );

        public void AddMethodUtf8( char[] passPhrase, HashAlgorithmTag s2kDigest ) => this.DoAddMethod( PgpUtilities.EncodePassPhrase( passPhrase, true ), true, s2kDigest );

        public void AddMethodRaw( byte[] rawPassPhrase, HashAlgorithmTag s2kDigest ) => this.DoAddMethod( rawPassPhrase, false, s2kDigest );

        internal void DoAddMethod(
          byte[] rawPassPhrase,
          bool clearPassPhrase,
          HashAlgorithmTag s2kDigest )
        {
            S2k s2k = PgpUtilities.GenerateS2k( s2kDigest, 96, this.rand );
            this.methods.Add( new PgpEncryptedDataGenerator.PbeMethod( this.defAlgorithm, s2k, PgpUtilities.DoMakeKeyFromPassPhrase( this.defAlgorithm, s2k, rawPassPhrase, clearPassPhrase ) ) );
        }

        public void AddMethod( PgpPublicKey key )
        {
            if (!key.IsEncryptionKey)
                throw new ArgumentException( "passed in key not an encryption key!" );
            this.methods.Add( new PgpEncryptedDataGenerator.PubMethod( key ) );
        }

        private void AddCheckSum( byte[] sessionInfo )
        {
            int num = 0;
            for (int index = 1; index < sessionInfo.Length - 2; ++index)
                num += sessionInfo[index];
            sessionInfo[sessionInfo.Length - 2] = (byte)(num >> 8);
            sessionInfo[sessionInfo.Length - 1] = (byte)num;
        }

        private byte[] CreateSessionInfo( SymmetricKeyAlgorithmTag algorithm, KeyParameter key )
        {
            byte[] key1 = key.GetKey();
            byte[] sessionInfo = new byte[key1.Length + 3];
            sessionInfo[0] = (byte)algorithm;
            key1.CopyTo( sessionInfo, 1 );
            this.AddCheckSum( sessionInfo );
            return sessionInfo;
        }

        private Stream Open( Stream outStr, long length, byte[] buffer )
        {
            if (this.cOut != null)
                throw new InvalidOperationException( "generator already in open state" );
            if (this.methods.Count == 0)
                throw new InvalidOperationException( "No encryption methods specified" );
            this.pOut = outStr != null ? new BcpgOutputStream( outStr ) : throw new ArgumentNullException( nameof( outStr ) );
            KeyParameter keyParameter;
            if (this.methods.Count == 1)
            {
                if (this.methods[0] is PgpEncryptedDataGenerator.PbeMethod)
                {
                    keyParameter = ((PgpEncryptedDataGenerator.PbeMethod)this.methods[0]).GetKey();
                }
                else
                {
                    keyParameter = PgpUtilities.MakeRandomKey( this.defAlgorithm, this.rand );
                    byte[] sessionInfo = this.CreateSessionInfo( this.defAlgorithm, keyParameter );
                    PgpEncryptedDataGenerator.PubMethod method = (PgpEncryptedDataGenerator.PubMethod)this.methods[0];
                    try
                    {
                        method.AddSessionInfo( sessionInfo, this.rand );
                    }
                    catch (Exception ex)
                    {
                        throw new PgpException( "exception encrypting session key", ex );
                    }
                }
                this.pOut.WritePacket( (ContainedPacket)this.methods[0] );
            }
            else
            {
                keyParameter = PgpUtilities.MakeRandomKey( this.defAlgorithm, this.rand );
                byte[] sessionInfo = this.CreateSessionInfo( this.defAlgorithm, keyParameter );
                for (int index = 0; index != this.methods.Count; ++index)
                {
                    PgpEncryptedDataGenerator.EncMethod method = (PgpEncryptedDataGenerator.EncMethod)this.methods[index];
                    try
                    {
                        method.AddSessionInfo( sessionInfo, this.rand );
                    }
                    catch (Exception ex)
                    {
                        throw new PgpException( "exception encrypting session key", ex );
                    }
                    this.pOut.WritePacket( method );
                }
            }
            string symmetricCipherName = PgpUtilities.GetSymmetricCipherName( this.defAlgorithm );
            if (symmetricCipherName == null)
                throw new PgpException( "null cipher specified" );
            try
            {
                this.c = CipherUtilities.GetCipher( !this.withIntegrityPacket ? symmetricCipherName + "/OpenPGPCFB/NoPadding" : symmetricCipherName + "/CFB/NoPadding" );
                byte[] iv = new byte[this.c.GetBlockSize()];
                this.c.Init( true, new ParametersWithRandom( new ParametersWithIV( keyParameter, iv ), this.rand ) );
                if (buffer == null)
                {
                    if (this.withIntegrityPacket)
                    {
                        this.pOut = new BcpgOutputStream( outStr, PacketTag.SymmetricEncryptedIntegrityProtected, length + this.c.GetBlockSize() + 2L + 1L + 22L );
                        this.pOut.WriteByte( 1 );
                    }
                    else
                        this.pOut = new BcpgOutputStream( outStr, PacketTag.SymmetricKeyEncrypted, length + this.c.GetBlockSize() + 2L, this.oldFormat );
                }
                else if (this.withIntegrityPacket)
                {
                    this.pOut = new BcpgOutputStream( outStr, PacketTag.SymmetricEncryptedIntegrityProtected, buffer );
                    this.pOut.WriteByte( 1 );
                }
                else
                    this.pOut = new BcpgOutputStream( outStr, PacketTag.SymmetricKeyEncrypted, buffer );
                int blockSize = this.c.GetBlockSize();
                byte[] numArray = new byte[blockSize + 2];
                this.rand.NextBytes( numArray, 0, blockSize );
                Array.Copy( numArray, numArray.Length - 4, numArray, numArray.Length - 2, 2 );
                Stream stream = this.cOut = new CipherStream( pOut, null, this.c );
                if (this.withIntegrityPacket)
                {
                    IDigest digest = DigestUtilities.GetDigest( PgpUtilities.GetDigestName( HashAlgorithmTag.Sha1 ) );
                    stream = this.digestOut = new DigestStream( stream, null, digest );
                }
                stream.Write( numArray, 0, numArray.Length );
                return new WrappedGeneratorStream( this, stream );
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception creating cipher", ex );
            }
        }

        public Stream Open( Stream outStr, long length ) => this.Open( outStr, length, null );

        public Stream Open( Stream outStr, byte[] buffer ) => this.Open( outStr, 0L, buffer );

        public void Close()
        {
            if (this.cOut == null)
                return;
            if (this.digestOut != null)
            {
                new BcpgOutputStream( digestOut, PacketTag.ModificationDetectionCode, 20L ).Flush();
                this.digestOut.Flush();
                byte[] buffer = DigestUtilities.DoFinal( this.digestOut.WriteDigest() );
                this.cOut.Write( buffer, 0, buffer.Length );
            }
            this.cOut.Flush();
            try
            {
                this.pOut.Write( this.c.DoFinal() );
                this.pOut.Finish();
            }
            catch (Exception ex)
            {
                throw new IOException( ex.Message, ex );
            }
            this.cOut = null;
            this.pOut = null;
        }

        private abstract class EncMethod : ContainedPacket
        {
            protected byte[] sessionInfo;
            protected SymmetricKeyAlgorithmTag encAlgorithm;
            protected KeyParameter key;

            public abstract void AddSessionInfo( byte[] si, SecureRandom random );
        }

        private class PbeMethod : PgpEncryptedDataGenerator.EncMethod
        {
            private S2k s2k;

            internal PbeMethod( SymmetricKeyAlgorithmTag encAlgorithm, S2k s2k, KeyParameter key )
            {
                this.encAlgorithm = encAlgorithm;
                this.s2k = s2k;
                this.key = key;
            }

            public KeyParameter GetKey() => this.key;

            public override void AddSessionInfo( byte[] si, SecureRandom random )
            {
                IBufferedCipher cipher = CipherUtilities.GetCipher( PgpUtilities.GetSymmetricCipherName( this.encAlgorithm ) + "/CFB/NoPadding" );
                byte[] iv = new byte[cipher.GetBlockSize()];
                cipher.Init( true, new ParametersWithRandom( new ParametersWithIV( key, iv ), random ) );
                this.sessionInfo = cipher.DoFinal( si, 0, si.Length - 2 );
            }

            public override void Encode( BcpgOutputStream pOut )
            {
                SymmetricKeyEncSessionPacket p = new SymmetricKeyEncSessionPacket( this.encAlgorithm, this.s2k, this.sessionInfo );
                pOut.WritePacket( p );
            }
        }

        private class PubMethod : PgpEncryptedDataGenerator.EncMethod
        {
            internal PgpPublicKey pubKey;
            internal byte[][] data;

            internal PubMethod( PgpPublicKey pubKey ) => this.pubKey = pubKey;

            public override void AddSessionInfo( byte[] sessionInfo, SecureRandom random ) => this.data = this.ProcessSessionInfo( this.EncryptSessionInfo( sessionInfo, random ) );

            private byte[] EncryptSessionInfo( byte[] sessionInfo, SecureRandom random )
            {
                if (this.pubKey.Algorithm != PublicKeyAlgorithmTag.EC)
                {
                    IBufferedCipher cipher;
                    switch (this.pubKey.Algorithm)
                    {
                        case PublicKeyAlgorithmTag.RsaGeneral:
                        case PublicKeyAlgorithmTag.RsaEncrypt:
                            cipher = CipherUtilities.GetCipher( "RSA//PKCS1Padding" );
                            break;
                        case PublicKeyAlgorithmTag.ElGamalEncrypt:
                        case PublicKeyAlgorithmTag.ElGamalGeneral:
                            cipher = CipherUtilities.GetCipher( "ElGamal/ECB/PKCS1Padding" );
                            break;
                        case PublicKeyAlgorithmTag.Dsa:
                            throw new PgpException( "Can't use DSA for encryption." );
                        case PublicKeyAlgorithmTag.ECDsa:
                            throw new PgpException( "Can't use ECDSA for encryption." );
                        default:
                            throw new PgpException( "unknown asymmetric algorithm: " + pubKey.Algorithm );
                    }
                    AsymmetricKeyParameter key = this.pubKey.GetKey();
                    cipher.Init( true, new ParametersWithRandom( key, random ) );
                    return cipher.DoFinal( sessionInfo );
                }
                ECDHPublicBcpgKey key1 = (ECDHPublicBcpgKey)this.pubKey.PublicKeyPacket.Key;
                IAsymmetricCipherKeyPairGenerator keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator( "ECDH" );
                keyPairGenerator.Init( new ECKeyGenerationParameters( key1.CurveOid, random ) );
                AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
                ECPrivateKeyParameters privateKeyParameters = (ECPrivateKeyParameters)keyPair.Private;
                ECPublicKeyParameters publicKeyParameters = (ECPublicKeyParameters)keyPair.Public;
                KeyParameter parameters = new KeyParameter( Rfc6637Utilities.CreateKey( this.pubKey.PublicKeyPacket, ((ECPublicKeyParameters)this.pubKey.GetKey()).Q.Multiply( privateKeyParameters.D ).Normalize() ) );
                IWrapper wrapper = PgpUtilities.CreateWrapper( key1.SymmetricKeyAlgorithm );
                wrapper.Init( true, new ParametersWithRandom( parameters, random ) );
                byte[] input = PgpPad.PadSessionData( sessionInfo );
                byte[] sourceArray = wrapper.Wrap( input, 0, input.Length );
                byte[] encoded = new MPInteger( new BigInteger( 1, publicKeyParameters.Q.GetEncoded( false ) ) ).GetEncoded();
                byte[] destinationArray = new byte[encoded.Length + 1 + sourceArray.Length];
                Array.Copy( encoded, 0, destinationArray, 0, encoded.Length );
                destinationArray[encoded.Length] = (byte)sourceArray.Length;
                Array.Copy( sourceArray, 0, destinationArray, encoded.Length + 1, sourceArray.Length );
                return destinationArray;
            }

            private byte[][] ProcessSessionInfo( byte[] encryptedSessionInfo )
            {
                switch (this.pubKey.Algorithm)
                {
                    case PublicKeyAlgorithmTag.RsaGeneral:
                    case PublicKeyAlgorithmTag.RsaEncrypt:
                        return new byte[1][]
                        {
              this.ConvertToEncodedMpi(encryptedSessionInfo)
                        };
                    case PublicKeyAlgorithmTag.ElGamalEncrypt:
                    case PublicKeyAlgorithmTag.ElGamalGeneral:
                        int length = encryptedSessionInfo.Length / 2;
                        byte[] numArray1 = new byte[length];
                        byte[] numArray2 = new byte[length];
                        Array.Copy( encryptedSessionInfo, 0, numArray1, 0, length );
                        Array.Copy( encryptedSessionInfo, length, numArray2, 0, length );
                        return new byte[2][]
                        {
              this.ConvertToEncodedMpi(numArray1),
              this.ConvertToEncodedMpi(numArray2)
                        };
                    case PublicKeyAlgorithmTag.EC:
                        return new byte[1][] { encryptedSessionInfo };
                    default:
                        throw new PgpException( "unknown asymmetric algorithm: " + pubKey.Algorithm );
                }
            }

            private byte[] ConvertToEncodedMpi( byte[] encryptedSessionInfo )
            {
                try
                {
                    return new MPInteger( new BigInteger( 1, encryptedSessionInfo ) ).GetEncoded();
                }
                catch (IOException ex)
                {
                    throw new PgpException( "Invalid MPI encoding: " + ex.Message, ex );
                }
            }

            public override void Encode( BcpgOutputStream pOut )
            {
                PublicKeyEncSessionPacket p = new PublicKeyEncSessionPacket( this.pubKey.KeyId, this.pubKey.Algorithm, this.data );
                pOut.WritePacket( p );
            }
        }
    }
}
