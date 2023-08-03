// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.CtsBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class CtsBlockCipher : BufferedBlockCipher
    {
        private readonly int blockSize;

        public CtsBlockCipher( IBlockCipher cipher )
        {
            switch (cipher)
            {
                case OfbBlockCipher _:
                case CfbBlockCipher _:
                    throw new ArgumentException( "CtsBlockCipher can only accept ECB, or CBC ciphers" );
                default:
                    this.cipher = cipher;
                    this.blockSize = cipher.GetBlockSize();
                    this.buf = new byte[this.blockSize * 2];
                    this.bufOff = 0;
                    break;
            }
        }

        public override int GetUpdateOutputSize( int length )
        {
            int num1 = length + this.bufOff;
            int num2 = num1 % this.buf.Length;
            return num2 == 0 ? num1 - this.buf.Length : num1 - num2;
        }

        public override int GetOutputSize( int length ) => length + this.bufOff;

        public override int ProcessByte( byte input, byte[] output, int outOff )
        {
            int num = 0;
            if (this.bufOff == this.buf.Length)
            {
                num = this.cipher.ProcessBlock( this.buf, 0, output, outOff );
                Array.Copy( buf, this.blockSize, buf, 0, this.blockSize );
                this.bufOff = this.blockSize;
            }
            this.buf[this.bufOff++] = (byte)(int)input;
            return num;
        }

        public override int ProcessBytes(
          byte[] input,
          int inOff,
          int length,
          byte[] output,
          int outOff )
        {
            if (length < 0)
                throw new ArgumentException( "Can't have a negative input outLength!" );
            int blockSize = this.GetBlockSize();
            int updateOutputSize = this.GetUpdateOutputSize( length );
            if (updateOutputSize > 0 && outOff + updateOutputSize > output.Length)
                throw new DataLengthException( "output buffer too short" );
            int num = 0;
            int length1 = this.buf.Length - this.bufOff;
            if (length > length1)
            {
                Array.Copy( input, inOff, buf, this.bufOff, length1 );
                num += this.cipher.ProcessBlock( this.buf, 0, output, outOff );
                Array.Copy( buf, blockSize, buf, 0, blockSize );
                this.bufOff = blockSize;
                length -= length1;
                inOff += length1;
                while (length > blockSize)
                {
                    Array.Copy( input, inOff, buf, this.bufOff, blockSize );
                    num += this.cipher.ProcessBlock( this.buf, 0, output, outOff + num );
                    Array.Copy( buf, blockSize, buf, 0, blockSize );
                    length -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy( input, inOff, buf, this.bufOff, length );
            this.bufOff += length;
            return num;
        }

        public override int DoFinal( byte[] output, int outOff )
        {
            if (this.bufOff + outOff > output.Length)
                throw new DataLengthException( "output buffer too small in doFinal" );
            int blockSize = this.cipher.GetBlockSize();
            int length = this.bufOff - blockSize;
            byte[] numArray = new byte[blockSize];
            if (this.forEncryption)
            {
                this.cipher.ProcessBlock( this.buf, 0, numArray, 0 );
                if (this.bufOff < blockSize)
                    throw new DataLengthException( "need at least one block of input for CTS" );
                for (int bufOff = this.bufOff; bufOff != this.buf.Length; ++bufOff)
                    this.buf[bufOff] = numArray[bufOff - blockSize];
                for (int index1 = blockSize; index1 != this.bufOff; ++index1)
                {
                    byte[] buf;
                    IntPtr index2;
                    (buf = this.buf)[(int)(index2 = (IntPtr)index1)] = (byte)(buf[(int)index2] ^ (uint)numArray[index1 - blockSize]);
                }
              (this.cipher is CbcBlockCipher ? ((CbcBlockCipher)this.cipher).GetUnderlyingCipher() : this.cipher).ProcessBlock( this.buf, blockSize, output, outOff );
                Array.Copy( numArray, 0, output, outOff + blockSize, length );
            }
            else
            {
                byte[] sourceArray = new byte[blockSize];
                (this.cipher is CbcBlockCipher ? ((CbcBlockCipher)this.cipher).GetUnderlyingCipher() : this.cipher).ProcessBlock( this.buf, 0, numArray, 0 );
                for (int index = blockSize; index != this.bufOff; ++index)
                    sourceArray[index - blockSize] = (byte)(numArray[index - blockSize] ^ (uint)this.buf[index]);
                Array.Copy( buf, blockSize, numArray, 0, length );
                this.cipher.ProcessBlock( numArray, 0, output, outOff );
                Array.Copy( sourceArray, 0, output, outOff + blockSize, length );
            }
            int bufOff1 = this.bufOff;
            this.Reset();
            return bufOff1;
        }
    }
}
