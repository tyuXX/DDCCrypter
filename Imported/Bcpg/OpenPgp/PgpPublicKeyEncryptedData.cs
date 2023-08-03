// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPublicKeyEncryptedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpPublicKeyEncryptedData : PgpEncryptedData
    {
        private PublicKeyEncSessionPacket keyData;

        internal PgpPublicKeyEncryptedData( PublicKeyEncSessionPacket keyData, InputStreamPacket encData )
          : base( encData )
        {
            this.keyData = keyData;
        }

        private static IBufferedCipher GetKeyCipher( PublicKeyAlgorithmTag algorithm )
        {
            try
            {
                switch (algorithm)
                {
                    case PublicKeyAlgorithmTag.RsaGeneral:
                    case PublicKeyAlgorithmTag.RsaEncrypt:
                        return CipherUtilities.GetCipher( "RSA//PKCS1Padding" );
                    case PublicKeyAlgorithmTag.ElGamalEncrypt:
                    case PublicKeyAlgorithmTag.ElGamalGeneral:
                        return CipherUtilities.GetCipher( "ElGamal/ECB/PKCS1Padding" );
                    default:
                        throw new PgpException( "unknown asymmetric algorithm: " + algorithm );
                }
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception creating cipher", ex );
            }
        }

        private bool ConfirmCheckSum( byte[] sessionInfo )
        {
            int num = 0;
            for (int index = 1; index != sessionInfo.Length - 2; ++index)
                num += sessionInfo[index] & byte.MaxValue;
            return sessionInfo[sessionInfo.Length - 2] == (byte)(num >> 8) && sessionInfo[sessionInfo.Length - 1] == (byte)num;
        }

        public long KeyId => this.keyData.KeyId;

        public SymmetricKeyAlgorithmTag GetSymmetricAlgorithm( PgpPrivateKey privKey ) => (SymmetricKeyAlgorithmTag)this.RecoverSessionData( privKey )[0];

        public Stream GetDataStream( PgpPrivateKey privKey )
        {
            byte[] numArray1 = this.RecoverSessionData( privKey );
            SymmetricKeyAlgorithmTag algorithm = this.ConfirmCheckSum( numArray1 ) ? (SymmetricKeyAlgorithmTag)numArray1[0] : throw new PgpKeyValidationException( "key checksum failed" );
            if (algorithm == SymmetricKeyAlgorithmTag.Null)
                return this.encData.GetInputStream();
            string symmetricCipherName = PgpUtilities.GetSymmetricCipherName( algorithm );
            string str = symmetricCipherName;
            IBufferedCipher cipher;
            try
            {
                cipher = CipherUtilities.GetCipher( !(this.encData is SymmetricEncIntegrityPacket) ? str + "/OpenPGPCFB/NoPadding" : str + "/CFB/NoPadding" );
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "exception creating cipher", ex );
            }
            try
            {
                KeyParameter keyParameter = ParameterUtilities.CreateKeyParameter( symmetricCipherName, numArray1, 1, numArray1.Length - 3 );
                byte[] numArray2 = new byte[cipher.GetBlockSize()];
                cipher.Init( false, new ParametersWithIV( keyParameter, numArray2 ) );
                this.encStream = BcpgInputStream.Wrap( new CipherStream( this.encData.GetInputStream(), cipher, null ) );
                if (this.encData is SymmetricEncIntegrityPacket)
                {
                    this.truncStream = new PgpEncryptedData.TruncatedStream( this.encStream );
                    this.encStream = new DigestStream( truncStream, DigestUtilities.GetDigest( PgpUtilities.GetDigestName( HashAlgorithmTag.Sha1 ) ), null );
                }
                if (Streams.ReadFully( this.encStream, numArray2, 0, numArray2.Length ) < numArray2.Length)
                    throw new EndOfStreamException( "unexpected end of stream." );
                int num1 = this.encStream.ReadByte();
                int num2 = this.encStream.ReadByte();
                if (num1 < 0 || num2 < 0)
                    throw new EndOfStreamException( "unexpected end of stream." );
                return this.encStream;
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception starting decryption", ex );
            }
        }

        private byte[] RecoverSessionData( PgpPrivateKey privKey )
        {
            byte[][] encSessionKey = this.keyData.GetEncSessionKey();
            if (this.keyData.Algorithm == PublicKeyAlgorithmTag.EC)
            {
                ECDHPublicBcpgKey key = (ECDHPublicBcpgKey)privKey.PublicKeyPacket.Key;
                X9ECParameters ecCurveByOid = ECKeyPairGenerator.FindECCurveByOid( key.CurveOid );
                byte[] sourceArray = encSessionKey[0];
                int length = (((sourceArray[0] & byte.MaxValue) << 8) + (sourceArray[1] & byte.MaxValue) + 7) / 8;
                byte[] numArray1 = new byte[length];
                Array.Copy( sourceArray, 2, numArray1, 0, length );
                byte[] numArray2 = new byte[(sourceArray[length + 2])];
                Array.Copy( sourceArray, 2 + length + 1, numArray2, 0, numArray2.Length );
                ECPoint s = ecCurveByOid.Curve.DecodePoint( numArray1 ).Multiply( ((ECPrivateKeyParameters)privKey.Key).D ).Normalize();
                KeyParameter parameters = new( Rfc6637Utilities.CreateKey( privKey.PublicKeyPacket, s ) );
                IWrapper wrapper = PgpUtilities.CreateWrapper( key.SymmetricKeyAlgorithm );
                wrapper.Init( false, parameters );
                return PgpPad.UnpadSessionData( wrapper.Unwrap( numArray2, 0, numArray2.Length ) );
            }
            IBufferedCipher keyCipher = GetKeyCipher( this.keyData.Algorithm );
            try
            {
                keyCipher.Init( false, privKey.Key );
            }
            catch (InvalidKeyException ex)
            {
                throw new PgpException( "error setting asymmetric cipher", ex );
            }
            if (this.keyData.Algorithm == PublicKeyAlgorithmTag.RsaEncrypt || this.keyData.Algorithm == PublicKeyAlgorithmTag.RsaGeneral)
            {
                byte[] input = encSessionKey[0];
                keyCipher.ProcessBytes( input, 2, input.Length - 2 );
            }
            else
            {
                int size = (((ElGamalKeyParameters)privKey.Key).Parameters.P.BitLength + 7) / 8;
                ProcessEncodedMpi( keyCipher, size, encSessionKey[0] );
                ProcessEncodedMpi( keyCipher, size, encSessionKey[1] );
            }
            try
            {
                return keyCipher.DoFinal();
            }
            catch (Exception ex)
            {
                throw new PgpException( "exception decrypting secret key", ex );
            }
        }

        private static void ProcessEncodedMpi( IBufferedCipher cipher, int size, byte[] mpiEnc )
        {
            if (mpiEnc.Length - 2 > size)
            {
                cipher.ProcessBytes( mpiEnc, 3, mpiEnc.Length - 3 );
            }
            else
            {
                byte[] numArray = new byte[size];
                Array.Copy( mpiEnc, 2, numArray, numArray.Length - (mpiEnc.Length - 2), mpiEnc.Length - 2 );
                cipher.ProcessBytes( numArray, 0, numArray.Length );
            }
        }
    }
}
