// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsBlockCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly byte[] randomData;
        protected readonly bool useExplicitIV;
        protected readonly bool encryptThenMac;
        protected readonly IBlockCipher encryptCipher;
        protected readonly IBlockCipher decryptCipher;
        protected readonly TlsMac mWriteMac;
        protected readonly TlsMac mReadMac;

        public virtual TlsMac WriteMac => this.mWriteMac;

        public virtual TlsMac ReadMac => this.mReadMac;

        public TlsBlockCipher(
          TlsContext context,
          IBlockCipher clientWriteCipher,
          IBlockCipher serverWriteCipher,
          IDigest clientWriteDigest,
          IDigest serverWriteDigest,
          int cipherKeySize )
        {
            this.context = context;
            this.randomData = new byte[256];
            context.NonceRandomGenerator.NextBytes( this.randomData );
            this.useExplicitIV = TlsUtilities.IsTlsV11( context );
            this.encryptThenMac = context.SecurityParameters.encryptThenMac;
            int size = (2 * cipherKeySize) + clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
            if (!this.useExplicitIV)
                size += clientWriteCipher.GetBlockSize() + serverWriteCipher.GetBlockSize();
            byte[] keyBlock = TlsUtilities.CalculateKeyBlock( context, size );
            int keyOff1 = 0;
            TlsMac tlsMac1 = new TlsMac( context, clientWriteDigest, keyBlock, keyOff1, clientWriteDigest.GetDigestSize() );
            int keyOff2 = keyOff1 + clientWriteDigest.GetDigestSize();
            TlsMac tlsMac2 = new TlsMac( context, serverWriteDigest, keyBlock, keyOff2, serverWriteDigest.GetDigestSize() );
            int keyOff3 = keyOff2 + serverWriteDigest.GetDigestSize();
            KeyParameter parameters1 = new KeyParameter( keyBlock, keyOff3, cipherKeySize );
            int keyOff4 = keyOff3 + cipherKeySize;
            KeyParameter parameters2 = new KeyParameter( keyBlock, keyOff4, cipherKeySize );
            int from1 = keyOff4 + cipherKeySize;
            byte[] iv1;
            byte[] iv2;
            if (this.useExplicitIV)
            {
                iv1 = new byte[clientWriteCipher.GetBlockSize()];
                iv2 = new byte[serverWriteCipher.GetBlockSize()];
            }
            else
            {
                iv1 = Arrays.CopyOfRange( keyBlock, from1, from1 + clientWriteCipher.GetBlockSize() );
                int from2 = from1 + clientWriteCipher.GetBlockSize();
                iv2 = Arrays.CopyOfRange( keyBlock, from2, from2 + serverWriteCipher.GetBlockSize() );
                from1 = from2 + serverWriteCipher.GetBlockSize();
            }
            if (from1 != size)
                throw new TlsFatalAlert( 80 );
            ICipherParameters parameters3;
            ICipherParameters parameters4;
            if (context.IsServer)
            {
                this.mWriteMac = tlsMac2;
                this.mReadMac = tlsMac1;
                this.encryptCipher = serverWriteCipher;
                this.decryptCipher = clientWriteCipher;
                parameters3 = new ParametersWithIV( parameters2, iv2 );
                parameters4 = new ParametersWithIV( parameters1, iv1 );
            }
            else
            {
                this.mWriteMac = tlsMac1;
                this.mReadMac = tlsMac2;
                this.encryptCipher = clientWriteCipher;
                this.decryptCipher = serverWriteCipher;
                parameters3 = new ParametersWithIV( parameters1, iv1 );
                parameters4 = new ParametersWithIV( parameters2, iv2 );
            }
            this.encryptCipher.Init( true, parameters3 );
            this.decryptCipher.Init( false, parameters4 );
        }

        public virtual int GetPlaintextLimit( int ciphertextLimit )
        {
            int blockSize = this.encryptCipher.GetBlockSize();
            int size = this.mWriteMac.Size;
            int num1 = ciphertextLimit;
            if (this.useExplicitIV)
                num1 -= blockSize;
            int num2;
            if (this.encryptThenMac)
            {
                int num3 = num1 - size;
                num2 = num3 - (num3 % blockSize);
            }
            else
                num2 = num1 - (num1 % blockSize) - size;
            return num2 - 1;
        }

        public virtual byte[] EncodePlaintext(
          long seqNo,
          byte type,
          byte[] plaintext,
          int offset,
          int len )
        {
            int blockSize = this.encryptCipher.GetBlockSize();
            int size = this.mWriteMac.Size;
            ProtocolVersion serverVersion = this.context.ServerVersion;
            int num1 = len;
            if (!this.encryptThenMac)
                num1 += size;
            int num2 = blockSize - 1 - (num1 % blockSize);
            if (!serverVersion.IsDtls && !serverVersion.IsSsl)
            {
                int num3 = this.ChooseExtraPadBlocks( this.context.SecureRandom, (byte.MaxValue - num2) / blockSize );
                num2 += num3 * blockSize;
            }
            int length = len + size + num2 + 1;
            if (this.useExplicitIV)
                length += blockSize;
            byte[] numArray1 = new byte[length];
            int destinationIndex = 0;
            if (this.useExplicitIV)
            {
                byte[] numArray2 = new byte[blockSize];
                this.context.NonceRandomGenerator.NextBytes( numArray2 );
                this.encryptCipher.Init( true, new ParametersWithIV( null, numArray2 ) );
                Array.Copy( numArray2, 0, numArray1, destinationIndex, blockSize );
                destinationIndex += blockSize;
            }
            int num4 = destinationIndex;
            Array.Copy( plaintext, offset, numArray1, destinationIndex, len );
            int num5 = destinationIndex + len;
            if (!this.encryptThenMac)
            {
                byte[] mac = this.mWriteMac.CalculateMac( seqNo, type, plaintext, offset, len );
                Array.Copy( mac, 0, numArray1, num5, mac.Length );
                num5 += mac.Length;
            }
            for (int index = 0; index <= num2; ++index)
                numArray1[num5++] = (byte)num2;
            for (int index = num4; index < num5; index += blockSize)
                this.encryptCipher.ProcessBlock( numArray1, index, numArray1, index );
            if (this.encryptThenMac)
            {
                byte[] mac = this.mWriteMac.CalculateMac( seqNo, type, numArray1, 0, num5 );
                Array.Copy( mac, 0, numArray1, num5, mac.Length );
                int num6 = num5 + mac.Length;
            }
            return numArray1;
        }

        public virtual byte[] DecodeCiphertext(
          long seqNo,
          byte type,
          byte[] ciphertext,
          int offset,
          int len )
        {
            int blockSize = this.decryptCipher.GetBlockSize();
            int size = this.mReadMac.Size;
            int val1 = blockSize;
            int num1 = !this.encryptThenMac ? System.Math.Max( val1, size + 1 ) : val1 + size;
            if (this.useExplicitIV)
                num1 += blockSize;
            int len1 = len >= num1 ? len : throw new TlsFatalAlert( 50 );
            if (this.encryptThenMac)
                len1 -= size;
            if (len1 % blockSize != 0)
                throw new TlsFatalAlert( 21 );
            if (this.encryptThenMac)
            {
                int to = offset + len;
                byte[] b = Arrays.CopyOfRange( ciphertext, to - size, to );
                if (!Arrays.ConstantTimeAreEqual( this.mReadMac.CalculateMac( seqNo, type, ciphertext, offset, len - size ), b ))
                    throw new TlsFatalAlert( 20 );
            }
            if (this.useExplicitIV)
            {
                this.decryptCipher.Init( false, new ParametersWithIV( null, ciphertext, offset, blockSize ) );
                offset += blockSize;
                len1 -= blockSize;
            }
            for (int index = 0; index < len1; index += blockSize)
                this.decryptCipher.ProcessBlock( ciphertext, offset + index, ciphertext, offset + index );
            int num2 = this.CheckPaddingConstantTime( ciphertext, offset, len1, blockSize, this.encryptThenMac ? 0 : size );
            bool flag = num2 == 0;
            int num3 = len1 - num2;
            if (!this.encryptThenMac)
            {
                num3 -= size;
                int length = num3;
                int from = offset + length;
                byte[] b = Arrays.CopyOfRange( ciphertext, from, from + size );
                byte[] macConstantTime = this.mReadMac.CalculateMacConstantTime( seqNo, type, ciphertext, offset, length, len1 - size, this.randomData );
                flag |= !Arrays.ConstantTimeAreEqual( macConstantTime, b );
            }
            if (flag)
                throw new TlsFatalAlert( 20 );
            return Arrays.CopyOfRange( ciphertext, offset, offset + num3 );
        }

        protected virtual int CheckPaddingConstantTime(
          byte[] buf,
          int off,
          int len,
          int blockSize,
          int macSize )
        {
            int num1 = off + len;
            byte num2 = buf[num1 - 1];
            int num3 = (num2 & byte.MaxValue) + 1;
            int num4 = 0;
            byte num5 = 0;
            if ((TlsUtilities.IsSsl( this.context ) && num3 > blockSize) || macSize + num3 > len)
            {
                num3 = 0;
            }
            else
            {
                int num6 = num1 - num3;
                do
                {
                    num5 |= (byte)(buf[num6++] ^ (uint)num2);
                }
                while (num6 < num1);
                num4 = num3;
                if (num5 != 0)
                    num3 = 0;
            }
            byte[] randomData = this.randomData;
            while (num4 < 256)
                num5 |= (byte)(randomData[num4++] ^ (uint)num2);
            byte[] numArray;
            (numArray = randomData)[0] = (byte)(numArray[0] ^ (uint)num5);
            return num3;
        }

        protected virtual int ChooseExtraPadBlocks( SecureRandom r, int max ) => System.Math.Min( this.LowestBitSet( r.NextInt() ), max );

        protected virtual int LowestBitSet( int x )
        {
            if (x == 0)
                return 32;
            uint num1 = (uint)x;
            int num2 = 0;
            for (; ((int)num1 & 1) == 0; num1 >>= 1)
                ++num2;
            return num2;
        }
    }
}
