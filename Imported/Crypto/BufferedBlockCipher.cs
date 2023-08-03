// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.BufferedBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto
{
    public class BufferedBlockCipher : BufferedCipherBase
    {
        internal byte[] buf;
        internal int bufOff;
        internal bool forEncryption;
        internal IBlockCipher cipher;

        protected BufferedBlockCipher()
        {
        }

        public BufferedBlockCipher( IBlockCipher cipher )
        {
            this.cipher = cipher != null ? cipher : throw new ArgumentNullException( nameof( cipher ) );
            this.buf = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public override string AlgorithmName => this.cipher.AlgorithmName;

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.forEncryption = forEncryption;
            if (parameters is ParametersWithRandom parametersWithRandom)
                parameters = parametersWithRandom.Parameters;
            this.Reset();
            this.cipher.Init( forEncryption, parameters );
        }

        public override int GetBlockSize() => this.cipher.GetBlockSize();

        public override int GetUpdateOutputSize( int length )
        {
            int num1 = length + this.bufOff;
            int num2 = num1 % this.buf.Length;
            return num1 - num2;
        }

        public override int GetOutputSize( int length ) => length + this.bufOff;

        public override int ProcessByte( byte input, byte[] output, int outOff )
        {
            this.buf[this.bufOff++] = input;
            if (this.bufOff != this.buf.Length)
                return 0;
            if (outOff + this.buf.Length > output.Length)
                throw new DataLengthException( "output buffer too short" );
            this.bufOff = 0;
            return this.cipher.ProcessBlock( this.buf, 0, output, outOff );
        }

        public override byte[] ProcessByte( byte input )
        {
            int updateOutputSize = this.GetUpdateOutputSize( 1 );
            byte[] numArray = updateOutputSize > 0 ? new byte[updateOutputSize] : null;
            int length = this.ProcessByte( input, numArray, 0 );
            if (updateOutputSize > 0 && length < updateOutputSize)
            {
                byte[] destinationArray = new byte[length];
                Array.Copy( numArray, 0, destinationArray, 0, length );
                numArray = destinationArray;
            }
            return numArray;
        }

        public override byte[] ProcessBytes( byte[] input, int inOff, int length )
        {
            if (input == null)
                throw new ArgumentNullException( nameof( input ) );
            if (length < 1)
                return null;
            int updateOutputSize = this.GetUpdateOutputSize( length );
            byte[] numArray = updateOutputSize > 0 ? new byte[updateOutputSize] : null;
            int length1 = this.ProcessBytes( input, inOff, length, numArray, 0 );
            if (updateOutputSize > 0 && length1 < updateOutputSize)
            {
                byte[] destinationArray = new byte[length1];
                Array.Copy( numArray, 0, destinationArray, 0, length1 );
                numArray = destinationArray;
            }
            return numArray;
        }

        public override int ProcessBytes(
          byte[] input,
          int inOff,
          int length,
          byte[] output,
          int outOff )
        {
            if (length < 1)
            {
                if (length < 0)
                    throw new ArgumentException( "Can't have a negative input length!" );
                return 0;
            }
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
            if (this.bufOff == this.buf.Length)
            {
                num += this.cipher.ProcessBlock( this.buf, 0, output, outOff + num );
                this.bufOff = 0;
            }
            return num;
        }

        public override byte[] DoFinal()
        {
            byte[] numArray = EmptyBuffer;
            int outputSize = this.GetOutputSize( 0 );
            if (outputSize > 0)
            {
                numArray = new byte[outputSize];
                int length = this.DoFinal( numArray, 0 );
                if (length < numArray.Length)
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy( numArray, 0, destinationArray, 0, length );
                    numArray = destinationArray;
                }
            }
            else
                this.Reset();
            return numArray;
        }

        public override byte[] DoFinal( byte[] input, int inOff, int inLen )
        {
            if (input == null)
                throw new ArgumentNullException( nameof( input ) );
            int outputSize = this.GetOutputSize( inLen );
            byte[] numArray = EmptyBuffer;
            if (outputSize > 0)
            {
                numArray = new byte[outputSize];
                int outOff = inLen > 0 ? this.ProcessBytes( input, inOff, inLen, numArray, 0 ) : 0;
                int length = outOff + this.DoFinal( numArray, outOff );
                if (length < numArray.Length)
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy( numArray, 0, destinationArray, 0, length );
                    numArray = destinationArray;
                }
            }
            else
                this.Reset();
            return numArray;
        }

        public override int DoFinal( byte[] output, int outOff )
        {
            try
            {
                if (this.bufOff != 0)
                {
                    Check.DataLength( !this.cipher.IsPartialBlockOkay, "data not block size aligned" );
                    Check.OutputLength( output, outOff, this.bufOff, "output buffer too short for DoFinal()" );
                    this.cipher.ProcessBlock( this.buf, 0, this.buf, 0 );
                    Array.Copy( buf, 0, output, outOff, this.bufOff );
                }
                return this.bufOff;
            }
            finally
            {
                this.Reset();
            }
        }

        public override void Reset()
        {
            Array.Clear( buf, 0, this.buf.Length );
            this.bufOff = 0;
            this.cipher.Reset();
        }
    }
}
