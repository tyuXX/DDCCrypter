// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.PaddedBufferedBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public class PaddedBufferedBlockCipher : BufferedBlockCipher
    {
        private readonly IBlockCipherPadding padding;

        public PaddedBufferedBlockCipher( IBlockCipher cipher, IBlockCipherPadding padding )
        {
            this.cipher = cipher;
            this.padding = padding;
            this.buf = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public PaddedBufferedBlockCipher( IBlockCipher cipher )
          : this( cipher, new Pkcs7Padding() )
        {
        }

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.forEncryption = forEncryption;
            SecureRandom random = null;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                random = parametersWithRandom.Random;
                parameters = parametersWithRandom.Parameters;
            }
            this.Reset();
            this.padding.Init( random );
            this.cipher.Init( forEncryption, parameters );
        }

        public override int GetOutputSize( int length )
        {
            int num1 = length + this.bufOff;
            int num2 = num1 % this.buf.Length;
            if (num2 != 0)
                return num1 - num2 + this.buf.Length;
            return this.forEncryption ? num1 + this.buf.Length : num1;
        }

        public override int GetUpdateOutputSize( int length )
        {
            int num1 = length + this.bufOff;
            int num2 = num1 % this.buf.Length;
            return num2 == 0 ? num1 - this.buf.Length : num1 - num2;
        }

        public override int ProcessByte( byte input, byte[] output, int outOff )
        {
            int num = 0;
            if (this.bufOff == this.buf.Length)
            {
                num = this.cipher.ProcessBlock( this.buf, 0, output, outOff );
                this.bufOff = 0;
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
                throw new ArgumentException( "Can't have a negative input length!" );
            int blockSize = this.GetBlockSize();
            int updateOutputSize = this.GetUpdateOutputSize( length );
            if (updateOutputSize > 0)
                Check.OutputLength( output, outOff, updateOutputSize, "output buffer too short" );
            int num = 0;
            int length1 = this.buf.Length - this.bufOff;
            if (length > length1)
            {
                Array.Copy( input, inOff, buf, this.bufOff, length1 );
                num += this.cipher.ProcessBlock( this.buf, 0, output, outOff );
                this.bufOff = 0;
                length -= length1;
                inOff += length1;
                while (length > this.buf.Length)
                {
                    num += this.cipher.ProcessBlock( input, inOff, output, outOff + num );
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
            int blockSize = this.cipher.GetBlockSize();
            int num = 0;
            int length;
            if (this.forEncryption)
            {
                if (this.bufOff == blockSize)
                {
                    if (outOff + (2 * blockSize) > output.Length)
                    {
                        this.Reset();
                        throw new OutputLengthException( "output buffer too short" );
                    }
                    num = this.cipher.ProcessBlock( this.buf, 0, output, outOff );
                    this.bufOff = 0;
                }
                this.padding.AddPadding( this.buf, this.bufOff );
                length = num + this.cipher.ProcessBlock( this.buf, 0, output, outOff + num );
                this.Reset();
            }
            else if (this.bufOff == blockSize)
            {
                length = this.cipher.ProcessBlock( this.buf, 0, this.buf, 0 );
                this.bufOff = 0;
                try
                {
                    length -= this.padding.PadCount( this.buf );
                    Array.Copy( buf, 0, output, outOff, length );
                }
                finally
                {
                    this.Reset();
                }
            }
            else
            {
                this.Reset();
                throw new DataLengthException( "last block incomplete in decryption" );
            }
            return length;
        }
    }
}
