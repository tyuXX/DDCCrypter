// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsStreamCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsStreamCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly IStreamCipher encryptCipher;
        protected readonly IStreamCipher decryptCipher;
        protected readonly TlsMac writeMac;
        protected readonly TlsMac readMac;
        protected readonly bool usesNonce;

        public TlsStreamCipher(
          TlsContext context,
          IStreamCipher clientWriteCipher,
          IStreamCipher serverWriteCipher,
          IDigest clientWriteDigest,
          IDigest serverWriteDigest,
          int cipherKeySize,
          bool usesNonce )
        {
            bool isServer = context.IsServer;
            this.context = context;
            this.usesNonce = usesNonce;
            this.encryptCipher = clientWriteCipher;
            this.decryptCipher = serverWriteCipher;
            int size = (2 * cipherKeySize) + clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
            byte[] keyBlock = TlsUtilities.CalculateKeyBlock( context, size );
            int keyOff1 = 0;
            TlsMac tlsMac1 = new TlsMac( context, clientWriteDigest, keyBlock, keyOff1, clientWriteDigest.GetDigestSize() );
            int keyOff2 = keyOff1 + clientWriteDigest.GetDigestSize();
            TlsMac tlsMac2 = new TlsMac( context, serverWriteDigest, keyBlock, keyOff2, serverWriteDigest.GetDigestSize() );
            int keyOff3 = keyOff2 + serverWriteDigest.GetDigestSize();
            KeyParameter keyParameter1 = new KeyParameter( keyBlock, keyOff3, cipherKeySize );
            int keyOff4 = keyOff3 + cipherKeySize;
            KeyParameter keyParameter2 = new KeyParameter( keyBlock, keyOff4, cipherKeySize );
            if (keyOff4 + cipherKeySize != size)
                throw new TlsFatalAlert( 80 );
            ICipherParameters parameters1;
            ICipherParameters parameters2;
            if (isServer)
            {
                this.writeMac = tlsMac2;
                this.readMac = tlsMac1;
                this.encryptCipher = serverWriteCipher;
                this.decryptCipher = clientWriteCipher;
                parameters1 = keyParameter2;
                parameters2 = keyParameter1;
            }
            else
            {
                this.writeMac = tlsMac1;
                this.readMac = tlsMac2;
                this.encryptCipher = clientWriteCipher;
                this.decryptCipher = serverWriteCipher;
                parameters1 = keyParameter1;
                parameters2 = keyParameter2;
            }
            if (usesNonce)
            {
                byte[] iv = new byte[8];
                parameters1 = new ParametersWithIV( parameters1, iv );
                parameters2 = new ParametersWithIV( parameters2, iv );
            }
            this.encryptCipher.Init( true, parameters1 );
            this.decryptCipher.Init( false, parameters2 );
        }

        public virtual int GetPlaintextLimit( int ciphertextLimit ) => ciphertextLimit - this.writeMac.Size;

        public virtual byte[] EncodePlaintext(
          long seqNo,
          byte type,
          byte[] plaintext,
          int offset,
          int len )
        {
            if (this.usesNonce)
                this.UpdateIV( this.encryptCipher, true, seqNo );
            byte[] output = new byte[len + this.writeMac.Size];
            this.encryptCipher.ProcessBytes( plaintext, offset, len, output, 0 );
            byte[] mac = this.writeMac.CalculateMac( seqNo, type, plaintext, offset, len );
            this.encryptCipher.ProcessBytes( mac, 0, mac.Length, output, len );
            return output;
        }

        public virtual byte[] DecodeCiphertext(
          long seqNo,
          byte type,
          byte[] ciphertext,
          int offset,
          int len )
        {
            if (this.usesNonce)
                this.UpdateIV( this.decryptCipher, false, seqNo );
            int size = this.readMac.Size;
            if (len < size)
                throw new TlsFatalAlert( 50 );
            int num = len - size;
            byte[] numArray = new byte[len];
            this.decryptCipher.ProcessBytes( ciphertext, offset, len, numArray, 0 );
            this.CheckMac( seqNo, type, numArray, num, len, numArray, 0, num );
            return Arrays.CopyOfRange( numArray, 0, num );
        }

        protected virtual void CheckMac(
          long seqNo,
          byte type,
          byte[] recBuf,
          int recStart,
          int recEnd,
          byte[] calcBuf,
          int calcOff,
          int calcLen )
        {
            if (!Arrays.ConstantTimeAreEqual( Arrays.CopyOfRange( recBuf, recStart, recEnd ), this.readMac.CalculateMac( seqNo, type, calcBuf, calcOff, calcLen ) ))
                throw new TlsFatalAlert( 20 );
        }

        protected virtual void UpdateIV( IStreamCipher cipher, bool forEncryption, long seqNo )
        {
            byte[] numArray = new byte[8];
            TlsUtilities.WriteUint64( seqNo, numArray, 0 );
            cipher.Init( forEncryption, new ParametersWithIV( null, numArray ) );
        }
    }
}
