// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RC532Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RC532Engine : IBlockCipher
    {
        private int _noRounds;
        private int[] _S;
        private static readonly int P32 = -1209970333;
        private static readonly int Q32 = -1640531527;
        private bool forEncryption;

        public RC532Engine() => this._noRounds = 12;

        public virtual string AlgorithmName => "RC5-32";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 8;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (typeof( RC5Parameters ).IsInstanceOfType( parameters ))
            {
                RC5Parameters rc5Parameters = (RC5Parameters)parameters;
                this._noRounds = rc5Parameters.Rounds;
                this.SetKey( rc5Parameters.GetKey() );
            }
            else
            {
                if (!typeof( KeyParameter ).IsInstanceOfType( parameters ))
                    throw new ArgumentException( "invalid parameter passed to RC532 init - " + Platform.GetTypeName( parameters ) );
                this.SetKey( ((KeyParameter)parameters).GetKey() );
            }
            this.forEncryption = forEncryption;
        }

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff ) => !this.forEncryption ? this.DecryptBlock( input, inOff, output, outOff ) : this.EncryptBlock( input, inOff, output, outOff );

        public virtual void Reset()
        {
        }

        private void SetKey( byte[] key )
        {
            int[] numArray = new int[(key.Length + 3) / 4];
            for (int index = 0; index != key.Length; ++index)
                numArray[index / 4] += (key[index] & byte.MaxValue) << (8 * (index % 4));
            this._S = new int[2 * (this._noRounds + 1)];
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
            int num1 = this.BytesToWord( input, inOff ) + this._S[0];
            int num2 = this.BytesToWord( input, inOff + 4 ) + this._S[1];
            for (int index = 1; index <= this._noRounds; ++index)
            {
                num1 = this.RotateLeft( num1 ^ num2, num2 ) + this._S[2 * index];
                num2 = this.RotateLeft( num2 ^ num1, num1 ) + this._S[(2 * index) + 1];
            }
            this.WordToBytes( num1, outBytes, outOff );
            this.WordToBytes( num2, outBytes, outOff + 4 );
            return 8;
        }

        private int DecryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            int y1 = this.BytesToWord( input, inOff );
            int y2 = this.BytesToWord( input, inOff + 4 );
            for (int noRounds = this._noRounds; noRounds >= 1; --noRounds)
            {
                y2 = this.RotateRight( y2 - this._S[(2 * noRounds) + 1], y1 ) ^ y1;
                y1 = this.RotateRight( y1 - this._S[2 * noRounds], y2 ) ^ y2;
            }
            this.WordToBytes( y1 - this._S[0], outBytes, outOff );
            this.WordToBytes( y2 - this._S[1], outBytes, outOff + 4 );
            return 8;
        }

        private int RotateLeft( int x, int y ) => (x << y) | x >>> 32 - (y & 31);

        private int RotateRight( int x, int y ) => x >>> (y & 31) | (x << (32 - (y & 31)));

        private int BytesToWord( byte[] src, int srcOff ) => (src[srcOff] & byte.MaxValue) | ((src[srcOff + 1] & byte.MaxValue) << 8) | ((src[srcOff + 2] & byte.MaxValue) << 16) | ((src[srcOff + 3] & byte.MaxValue) << 24);

        private void WordToBytes( int word, byte[] dst, int dstOff )
        {
            dst[dstOff] = (byte)word;
            dst[dstOff + 1] = (byte)(word >> 8);
            dst[dstOff + 2] = (byte)(word >> 16);
            dst[dstOff + 3] = (byte)(word >> 24);
        }
    }
}
