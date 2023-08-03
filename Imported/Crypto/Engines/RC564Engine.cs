// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RC564Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RC564Engine : IBlockCipher
    {
        private static readonly int wordSize = 64;
        private static readonly int bytesPerWord = wordSize / 8;
        private int _noRounds;
        private long[] _S;
        private static readonly long P64 = -5196783011329398165;
        private static readonly long Q64 = -7046029254386353131;
        private bool forEncryption;

        public RC564Engine() => this._noRounds = 12;

        public virtual string AlgorithmName => "RC5-64";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 2 * bytesPerWord;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            RC5Parameters rc5Parameters = typeof( RC5Parameters ).IsInstanceOfType( parameters ) ? (RC5Parameters)parameters : throw new ArgumentException( "invalid parameter passed to RC564 init - " + Platform.GetTypeName( parameters ) );
            this.forEncryption = forEncryption;
            this._noRounds = rc5Parameters.Rounds;
            this.SetKey( rc5Parameters.GetKey() );
        }

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff ) => !this.forEncryption ? this.DecryptBlock( input, inOff, output, outOff ) : this.EncryptBlock( input, inOff, output, outOff );

        public virtual void Reset()
        {
        }

        private void SetKey( byte[] key )
        {
            long[] numArray = new long[(key.Length + (bytesPerWord - 1)) / bytesPerWord];
            for (int index = 0; index != key.Length; ++index)
                numArray[index / bytesPerWord] += (long)(key[index] & byte.MaxValue) << (8 * (index % bytesPerWord));
            this._S = new long[2 * (this._noRounds + 1)];
            this._S[0] = P64;
            for (int index = 1; index < this._S.Length; ++index)
                this._S[index] = this._S[index - 1] + Q64;
            int num1 = numArray.Length <= this._S.Length ? 3 * this._S.Length : 3 * numArray.Length;
            long num2 = 0;
            long num3 = 0;
            int index1 = 0;
            int index2 = 0;
            for (int index3 = 0; index3 < num1; ++index3)
            {
                num2 = this._S[index1] = this.RotateLeft( this._S[index1] + num2 + num3, 3L );
                num3 = numArray[index2] = this.RotateLeft( numArray[index2] + num2 + num3, num2 + num3 );
                index1 = (index1 + 1) % this._S.Length;
                index2 = (index2 + 1) % numArray.Length;
            }
        }

        private int EncryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            long num1 = this.BytesToWord( input, inOff ) + this._S[0];
            long num2 = this.BytesToWord( input, inOff + bytesPerWord ) + this._S[1];
            for (int index = 1; index <= this._noRounds; ++index)
            {
                num1 = this.RotateLeft( num1 ^ num2, num2 ) + this._S[2 * index];
                num2 = this.RotateLeft( num2 ^ num1, num1 ) + this._S[(2 * index) + 1];
            }
            this.WordToBytes( num1, outBytes, outOff );
            this.WordToBytes( num2, outBytes, outOff + bytesPerWord );
            return 2 * bytesPerWord;
        }

        private int DecryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            long y1 = this.BytesToWord( input, inOff );
            long y2 = this.BytesToWord( input, inOff + bytesPerWord );
            for (int noRounds = this._noRounds; noRounds >= 1; --noRounds)
            {
                y2 = this.RotateRight( y2 - this._S[(2 * noRounds) + 1], y1 ) ^ y1;
                y1 = this.RotateRight( y1 - this._S[2 * noRounds], y2 ) ^ y2;
            }
            this.WordToBytes( y1 - this._S[0], outBytes, outOff );
            this.WordToBytes( y2 - this._S[1], outBytes, outOff + bytesPerWord );
            return 2 * bytesPerWord;
        }

        private long RotateLeft( long x, long y ) => (x << (int)(y & wordSize - 1)) | x >>> (int)(wordSize - (y & wordSize - 1));

        private long RotateRight( long x, long y ) => x >>> (int)(y & wordSize - 1) | (x << (int)(wordSize - (y & wordSize - 1)));

        private long BytesToWord( byte[] src, int srcOff )
        {
            long word = 0;
            for (int index = bytesPerWord - 1; index >= 0; --index)
                word = (word << 8) + (src[index + srcOff] & byte.MaxValue);
            return word;
        }

        private void WordToBytes( long word, byte[] dst, int dstOff )
        {
            for (int index = 0; index < bytesPerWord; ++index)
            {
                dst[index + dstOff] = (byte)word;
                word >>>= 8;
            }
        }
    }
}
