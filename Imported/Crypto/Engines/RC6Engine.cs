// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RC6Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RC6Engine : IBlockCipher
    {
        private static readonly int wordSize = 32;
        private static readonly int bytesPerWord = wordSize / 8;
        private static readonly int _noRounds = 20;
        private int[] _S;
        private static readonly int P32 = -1209970333;
        private static readonly int Q32 = -1640531527;
        private static readonly int LGW = 5;
        private bool forEncryption;

        public virtual string AlgorithmName => "RC6";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 4 * bytesPerWord;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "invalid parameter passed to RC6 init - " + Platform.GetTypeName( parameters ) );
            this.forEncryption = forEncryption;
            this.SetKey( ((KeyParameter)parameters).GetKey() );
        }

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            int blockSize = this.GetBlockSize();
            if (this._S == null)
                throw new InvalidOperationException( "RC6 engine not initialised" );
            Check.DataLength( input, inOff, blockSize, "input buffer too short" );
            Check.OutputLength( output, outOff, blockSize, "output buffer too short" );
            return !this.forEncryption ? this.DecryptBlock( input, inOff, output, outOff ) : this.EncryptBlock( input, inOff, output, outOff );
        }

        public virtual void Reset()
        {
        }

        private void SetKey( byte[] key )
        {
            if ((key.Length + (bytesPerWord - 1)) / bytesPerWord == 0)
                ;
            int[] numArray = new int[(key.Length + bytesPerWord - 1) / bytesPerWord];
            for (int index = key.Length - 1; index >= 0; --index)
                numArray[index / bytesPerWord] = (numArray[index / bytesPerWord] << 8) + (key[index] & byte.MaxValue);
            this._S = new int[2 + (2 * _noRounds) + 2];
            this._S[0] = P32;
            for (int index = 1; index < this._S.Length; ++index)
                this._S[index] = this._S[index - 1] + Q32;
            int num1 = numArray.Length <= this._S.Length ? 3 * this._S.Length : 3 * numArray.Length;
            int num2 = 0;
            int num3 = 0;
            int index1 = 0;
            int index2 = 0;
            for (int index3 = 0; index3 < num1; ++index3)
            {
                num2 = this._S[index1] = this.RotateLeft( this._S[index1] + num2 + num3, 3 );
                num3 = numArray[index2] = this.RotateLeft( numArray[index2] + num2 + num3, num2 + num3 );
                index1 = (index1 + 1) % this._S.Length;
                index2 = (index2 + 1) % numArray.Length;
            }
        }

        private int EncryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            int num1 = this.BytesToWord( input, inOff );
            int word1 = this.BytesToWord( input, inOff + bytesPerWord );
            int num2 = this.BytesToWord( input, inOff + (bytesPerWord * 2) );
            int word2 = this.BytesToWord( input, inOff + (bytesPerWord * 3) );
            int word3 = word1 + this._S[0];
            int word4 = word2 + this._S[1];
            for (int index = 1; index <= _noRounds; ++index)
            {
                int y1 = this.RotateLeft( word3 * ((2 * word3) + 1), 5 );
                int y2 = this.RotateLeft( word4 * ((2 * word4) + 1), 5 );
                int num3 = this.RotateLeft( num1 ^ y1, y2 ) + this._S[2 * index];
                int num4 = this.RotateLeft( num2 ^ y2, y1 ) + this._S[(2 * index) + 1];
                int num5 = num3;
                num1 = word3;
                word3 = num4;
                num2 = word4;
                word4 = num5;
            }
            int word5 = num1 + this._S[(2 * _noRounds) + 2];
            int word6 = num2 + this._S[(2 * _noRounds) + 3];
            this.WordToBytes( word5, outBytes, outOff );
            this.WordToBytes( word3, outBytes, outOff + bytesPerWord );
            this.WordToBytes( word6, outBytes, outOff + (bytesPerWord * 2) );
            this.WordToBytes( word4, outBytes, outOff + (bytesPerWord * 3) );
            return 4 * bytesPerWord;
        }

        private int DecryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            int word1 = this.BytesToWord( input, inOff );
            int num1 = this.BytesToWord( input, inOff + bytesPerWord );
            int word2 = this.BytesToWord( input, inOff + (bytesPerWord * 2) );
            int num2 = this.BytesToWord( input, inOff + (bytesPerWord * 3) );
            int word3 = word2 - this._S[(2 * _noRounds) + 3];
            int word4 = word1 - this._S[(2 * _noRounds) + 2];
            for (int noRounds = _noRounds; noRounds >= 1; --noRounds)
            {
                int num3 = num2;
                num2 = word3;
                int num4 = num1;
                num1 = word4;
                int num5 = num3;
                int y1 = this.RotateLeft( num1 * ((2 * num1) + 1), LGW );
                int y2 = this.RotateLeft( num2 * ((2 * num2) + 1), LGW );
                word3 = this.RotateRight( num4 - this._S[(2 * noRounds) + 1], y1 ) ^ y2;
                word4 = this.RotateRight( num5 - this._S[2 * noRounds], y2 ) ^ y1;
            }
            int word5 = num2 - this._S[1];
            int word6 = num1 - this._S[0];
            this.WordToBytes( word4, outBytes, outOff );
            this.WordToBytes( word6, outBytes, outOff + bytesPerWord );
            this.WordToBytes( word3, outBytes, outOff + (bytesPerWord * 2) );
            this.WordToBytes( word5, outBytes, outOff + (bytesPerWord * 3) );
            return 4 * bytesPerWord;
        }

        private int RotateLeft( int x, int y ) => (x << (y & (wordSize - 1))) | x >>> wordSize - (y & (wordSize - 1));

        private int RotateRight( int x, int y ) => x >>> (y & (wordSize - 1)) | (x << (wordSize - (y & (wordSize - 1))));

        private int BytesToWord( byte[] src, int srcOff )
        {
            int word = 0;
            for (int index = bytesPerWord - 1; index >= 0; --index)
                word = (word << 8) + (src[index + srcOff] & byte.MaxValue);
            return word;
        }

        private void WordToBytes( int word, byte[] dst, int dstOff )
        {
            for (int index = 0; index < bytesPerWord; ++index)
            {
                dst[index + dstOff] = (byte)word;
                word >>>= 8;
            }
        }
    }
}
