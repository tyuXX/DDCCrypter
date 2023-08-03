// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsNullCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsNullCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly TlsMac writeMac;
        protected readonly TlsMac readMac;

        public TlsNullCipher( TlsContext context )
        {
            this.context = context;
            this.writeMac = null;
            this.readMac = null;
        }

        public TlsNullCipher( TlsContext context, IDigest clientWriteDigest, IDigest serverWriteDigest )
        {
            if (clientWriteDigest == null != (serverWriteDigest == null))
                throw new TlsFatalAlert( 80 );
            this.context = context;
            TlsMac tlsMac1 = null;
            TlsMac tlsMac2 = null;
            if (clientWriteDigest != null)
            {
                int size = clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
                byte[] keyBlock = TlsUtilities.CalculateKeyBlock( context, size );
                int keyOff1 = 0;
                tlsMac1 = new TlsMac( context, clientWriteDigest, keyBlock, keyOff1, clientWriteDigest.GetDigestSize() );
                int keyOff2 = keyOff1 + clientWriteDigest.GetDigestSize();
                tlsMac2 = new TlsMac( context, serverWriteDigest, keyBlock, keyOff2, serverWriteDigest.GetDigestSize() );
                if (keyOff2 + serverWriteDigest.GetDigestSize() != size)
                    throw new TlsFatalAlert( 80 );
            }
            if (context.IsServer)
            {
                this.writeMac = tlsMac2;
                this.readMac = tlsMac1;
            }
            else
            {
                this.writeMac = tlsMac1;
                this.readMac = tlsMac2;
            }
        }

        public virtual int GetPlaintextLimit( int ciphertextLimit )
        {
            int plaintextLimit = ciphertextLimit;
            if (this.writeMac != null)
                plaintextLimit -= this.writeMac.Size;
            return plaintextLimit;
        }

        public virtual byte[] EncodePlaintext(
          long seqNo,
          byte type,
          byte[] plaintext,
          int offset,
          int len )
        {
            if (this.writeMac == null)
                return Arrays.CopyOfRange( plaintext, offset, offset + len );
            byte[] mac = this.writeMac.CalculateMac( seqNo, type, plaintext, offset, len );
            byte[] destinationArray = new byte[len + mac.Length];
            Array.Copy( plaintext, offset, destinationArray, 0, len );
            Array.Copy( mac, 0, destinationArray, len, mac.Length );
            return destinationArray;
        }

        public virtual byte[] DecodeCiphertext(
          long seqNo,
          byte type,
          byte[] ciphertext,
          int offset,
          int len )
        {
            if (this.readMac == null)
                return Arrays.CopyOfRange( ciphertext, offset, offset + len );
            int size = this.readMac.Size;
            if (len < size)
                throw new TlsFatalAlert( 50 );
            int length = len - size;
            if (!Arrays.ConstantTimeAreEqual( Arrays.CopyOfRange( ciphertext, offset + length, offset + len ), this.readMac.CalculateMac( seqNo, type, ciphertext, offset, length ) ))
                throw new TlsFatalAlert( 20 );
            return Arrays.CopyOfRange( ciphertext, offset, offset + length );
        }
    }
}
