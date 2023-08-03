// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsAeadCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsAeadCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly int macSize;
        protected readonly int nonce_explicit_length;
        protected readonly IAeadBlockCipher encryptCipher;
        protected readonly IAeadBlockCipher decryptCipher;
        protected readonly byte[] encryptImplicitNonce;
        protected readonly byte[] decryptImplicitNonce;

        public TlsAeadCipher(
          TlsContext context,
          IAeadBlockCipher clientWriteCipher,
          IAeadBlockCipher serverWriteCipher,
          int cipherKeySize,
          int macSize )
        {
            this.context = TlsUtilities.IsTlsV12( context ) ? context : throw new TlsFatalAlert( 80 );
            this.macSize = macSize;
            this.nonce_explicit_length = 8;
            int num = 4;
            int size = (2 * cipherKeySize) + (2 * num);
            byte[] keyBlock = TlsUtilities.CalculateKeyBlock( context, size );
            int keyOff1 = 0;
            KeyParameter keyParameter1 = new( keyBlock, keyOff1, cipherKeySize );
            int keyOff2 = keyOff1 + cipherKeySize;
            KeyParameter keyParameter2 = new( keyBlock, keyOff2, cipherKeySize );
            int from1 = keyOff2 + cipherKeySize;
            byte[] numArray1 = Arrays.CopyOfRange( keyBlock, from1, from1 + num );
            int from2 = from1 + num;
            byte[] numArray2 = Arrays.CopyOfRange( keyBlock, from2, from2 + num );
            if (from2 + num != size)
                throw new TlsFatalAlert( 80 );
            KeyParameter key1;
            KeyParameter key2;
            if (context.IsServer)
            {
                this.encryptCipher = serverWriteCipher;
                this.decryptCipher = clientWriteCipher;
                this.encryptImplicitNonce = numArray2;
                this.decryptImplicitNonce = numArray1;
                key1 = keyParameter2;
                key2 = keyParameter1;
            }
            else
            {
                this.encryptCipher = clientWriteCipher;
                this.decryptCipher = serverWriteCipher;
                this.encryptImplicitNonce = numArray1;
                this.decryptImplicitNonce = numArray2;
                key1 = keyParameter1;
                key2 = keyParameter2;
            }
            byte[] nonce = new byte[num + this.nonce_explicit_length];
            this.encryptCipher.Init( true, new AeadParameters( key1, 8 * macSize, nonce ) );
            this.decryptCipher.Init( false, new AeadParameters( key2, 8 * macSize, nonce ) );
        }

        public virtual int GetPlaintextLimit( int ciphertextLimit ) => ciphertextLimit - this.macSize - this.nonce_explicit_length;

        public virtual byte[] EncodePlaintext(
          long seqNo,
          byte type,
          byte[] plaintext,
          int offset,
          int len )
        {
            byte[] numArray1 = new byte[this.encryptImplicitNonce.Length + this.nonce_explicit_length];
            Array.Copy( encryptImplicitNonce, 0, numArray1, 0, this.encryptImplicitNonce.Length );
            TlsUtilities.WriteUint64( seqNo, numArray1, this.encryptImplicitNonce.Length );
            int inOff = offset;
            int len1 = len;
            byte[] numArray2 = new byte[this.nonce_explicit_length + this.encryptCipher.GetOutputSize( len1 )];
            Array.Copy( numArray1, this.encryptImplicitNonce.Length, numArray2, 0, this.nonce_explicit_length );
            int nonceExplicitLength = this.nonce_explicit_length;
            byte[] additionalData = this.GetAdditionalData( seqNo, type, len1 );
            AeadParameters parameters = new( null, 8 * this.macSize, numArray1, additionalData );
            int num;
            try
            {
                this.encryptCipher.Init( true, parameters );
                int outOff = nonceExplicitLength + this.encryptCipher.ProcessBytes( plaintext, inOff, len1, numArray2, nonceExplicitLength );
                num = outOff + this.encryptCipher.DoFinal( numArray2, outOff );
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 80, ex );
            }
            if (num != numArray2.Length)
                throw new TlsFatalAlert( 80 );
            return numArray2;
        }

        public virtual byte[] DecodeCiphertext(
          long seqNo,
          byte type,
          byte[] ciphertext,
          int offset,
          int len )
        {
            if (this.GetPlaintextLimit( len ) < 0)
                throw new TlsFatalAlert( 50 );
            byte[] numArray = new byte[this.decryptImplicitNonce.Length + this.nonce_explicit_length];
            Array.Copy( decryptImplicitNonce, 0, numArray, 0, this.decryptImplicitNonce.Length );
            Array.Copy( ciphertext, offset, numArray, this.decryptImplicitNonce.Length, this.nonce_explicit_length );
            int inOff = offset + this.nonce_explicit_length;
            int len1 = len - this.nonce_explicit_length;
            int outputSize = this.decryptCipher.GetOutputSize( len1 );
            byte[] outBytes = new byte[outputSize];
            int outOff1 = 0;
            byte[] additionalData = this.GetAdditionalData( seqNo, type, outputSize );
            AeadParameters parameters = new( null, 8 * this.macSize, numArray, additionalData );
            int num;
            try
            {
                this.decryptCipher.Init( false, parameters );
                int outOff2 = outOff1 + this.decryptCipher.ProcessBytes( ciphertext, inOff, len1, outBytes, outOff1 );
                num = outOff2 + this.decryptCipher.DoFinal( outBytes, outOff2 );
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 20, ex );
            }
            if (num != outBytes.Length)
                throw new TlsFatalAlert( 80 );
            return outBytes;
        }

        protected virtual byte[] GetAdditionalData( long seqNo, byte type, int len )
        {
            byte[] buf = new byte[13];
            TlsUtilities.WriteUint64( seqNo, buf, 0 );
            TlsUtilities.WriteUint8( type, buf, 8 );
            TlsUtilities.WriteVersion( this.context.ServerVersion, buf, 9 );
            TlsUtilities.WriteUint16( len, buf, 11 );
            return buf;
        }
    }
}
