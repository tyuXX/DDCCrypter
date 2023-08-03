// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.WhirlpoolDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public sealed class WhirlpoolDigest : IDigest, IMemoable
    {
        private const int BYTE_LENGTH = 64;
        private const int DIGEST_LENGTH_BYTES = 64;
        private const int ROUNDS = 10;
        private const int REDUCTION_POLYNOMIAL = 285;
        private const int BITCOUNT_ARRAY_SIZE = 32;
        private static readonly int[] SBOX = new int[256]
        {
      24,
      35,
      198,
      232,
      135,
      184,
      1,
      79,
      54,
      166,
      210,
      245,
      121,
      111,
      145,
      82,
      96,
      188,
      155,
      142,
      163,
      12,
      123,
      53,
      29,
      224,
      215,
      194,
      46,
      75,
      254,
      87,
      21,
      119,
      55,
      229,
      159,
      240,
      74,
      218,
      88,
      201,
      41,
      10,
      177,
      160,
      107,
      133,
      189,
      93,
      16,
      244,
      203,
      62,
      5,
      103,
      228,
      39,
      65,
      139,
      167,
      125,
      149,
      216,
      251,
      238,
      124,
      102,
      221,
      23,
      71,
      158,
      202,
      45,
      191,
      7,
      173,
      90,
      131,
      51,
      99,
      2,
      170,
      113,
      200,
      25,
      73,
      217,
      242,
      227,
      91,
      136,
      154,
      38,
      50,
      176,
      233,
      15,
      213,
      128,
      190,
      205,
      52,
      72,
       byte.MaxValue,
      122,
      144,
      95,
      32,
      104,
      26,
      174,
      180,
      84,
      147,
      34,
      100,
      241,
      115,
      18,
      64,
      8,
      195,
      236,
      219,
      161,
      141,
      61,
      151,
      0,
      207,
      43,
      118,
      130,
      214,
      27,
      181,
      175,
      106,
      80,
      69,
      243,
      48,
      239,
      63,
      85,
      162,
      234,
      101,
      186,
      47,
      192,
      222,
      28,
      253,
      77,
      146,
      117,
      6,
      138,
      178,
      230,
      14,
      31,
      98,
      212,
      168,
      150,
      249,
      197,
      37,
      89,
      132,
      114,
      57,
      76,
      94,
      120,
      56,
      140,
      209,
      165,
      226,
      97,
      179,
      33,
      156,
      30,
      67,
      199,
      252,
      4,
      81,
      153,
      109,
      13,
      250,
      223,
      126,
      36,
      59,
      171,
      206,
      17,
      143,
      78,
      183,
      235,
      60,
      129,
      148,
      247,
      185,
      19,
      44,
      211,
      231,
      110,
      196,
      3,
      86,
      68,
       sbyte.MaxValue,
      169,
      42,
      187,
      193,
      83,
      220,
      11,
      157,
      108,
      49,
      116,
      246,
      70,
      172,
      137,
      20,
      225,
      22,
      58,
      105,
      9,
      112,
      182,
      208,
      237,
      204,
      66,
      152,
      164,
      40,
      92,
      248,
      134
        };
        private static readonly long[] C0 = new long[256];
        private static readonly long[] C1 = new long[256];
        private static readonly long[] C2 = new long[256];
        private static readonly long[] C3 = new long[256];
        private static readonly long[] C4 = new long[256];
        private static readonly long[] C5 = new long[256];
        private static readonly long[] C6 = new long[256];
        private static readonly long[] C7 = new long[256];
        private readonly long[] _rc = new long[11];
        private static readonly short[] EIGHT = new short[32];
        private byte[] _buffer = new byte[64];
        private int _bufferPos;
        private short[] _bitCount = new short[32];
        private long[] _hash = new long[8];
        private long[] _K = new long[8];
        private long[] _L = new long[8];
        private long[] _block = new long[8];
        private long[] _state = new long[8];

        static WhirlpoolDigest()
        {
            EIGHT[31] = 8;
            for (int index = 0; index < 256; ++index)
            {
                int num1 = SBOX[index];
                int num2 = maskWithReductionPolynomial( num1 << 1 );
                int num3 = maskWithReductionPolynomial( num2 << 1 );
                int num4 = num3 ^ num1;
                int num5 = maskWithReductionPolynomial( num3 << 1 );
                int num6 = num5 ^ num1;
                C0[index] = packIntoLong( num1, num1, num3, num1, num5, num4, num2, num6 );
                C1[index] = packIntoLong( num6, num1, num1, num3, num1, num5, num4, num2 );
                C2[index] = packIntoLong( num2, num6, num1, num1, num3, num1, num5, num4 );
                C3[index] = packIntoLong( num4, num2, num6, num1, num1, num3, num1, num5 );
                C4[index] = packIntoLong( num5, num4, num2, num6, num1, num1, num3, num1 );
                C5[index] = packIntoLong( num1, num5, num4, num2, num6, num1, num1, num3 );
                C6[index] = packIntoLong( num3, num1, num5, num4, num2, num6, num1, num1 );
                C7[index] = packIntoLong( num1, num3, num1, num5, num4, num2, num6, num1 );
            }
        }

        public WhirlpoolDigest()
        {
            this._rc[0] = 0L;
            for (int index1 = 1; index1 <= 10; ++index1)
            {
                int index2 = 8 * (index1 - 1);
                this._rc[index1] = (C0[index2] & -72057594037927936L) ^ (C1[index2 + 1] & 71776119061217280L) ^ (C2[index2 + 2] & 280375465082880L) ^ (C3[index2 + 3] & 1095216660480L) ^ (C4[index2 + 4] & 4278190080L) ^ (C5[index2 + 5] & 16711680L) ^ (C6[index2 + 6] & 65280L) ^ (C7[index2 + 7] & byte.MaxValue);
            }
        }

        private static long packIntoLong(
          int b7,
          int b6,
          int b5,
          int b4,
          int b3,
          int b2,
          int b1,
          int b0 )
        {
            return ((long)b7 << 56) ^ ((long)b6 << 48) ^ ((long)b5 << 40) ^ ((long)b4 << 32) ^ ((long)b3 << 24) ^ ((long)b2 << 16) ^ ((long)b1 << 8) ^ b0;
        }

        private static int maskWithReductionPolynomial( int input )
        {
            int num = input;
            if (num >= 256)
                num ^= 285;
            return num;
        }

        public WhirlpoolDigest( WhirlpoolDigest originalDigest ) => this.Reset( originalDigest );

        public string AlgorithmName => "Whirlpool";

        public int GetDigestSize() => 64;

        public int DoFinal( byte[] output, int outOff )
        {
            this.finish();
            for (int index = 0; index < 8; ++index)
                convertLongToByteArray( this._hash[index], output, outOff + (index * 8) );
            this.Reset();
            return this.GetDigestSize();
        }

        public void Reset()
        {
            this._bufferPos = 0;
            Array.Clear( _bitCount, 0, this._bitCount.Length );
            Array.Clear( _buffer, 0, this._buffer.Length );
            Array.Clear( _hash, 0, this._hash.Length );
            Array.Clear( _K, 0, this._K.Length );
            Array.Clear( _L, 0, this._L.Length );
            Array.Clear( _block, 0, this._block.Length );
            Array.Clear( _state, 0, this._state.Length );
        }

        private void processFilledBuffer()
        {
            for (int index = 0; index < this._state.Length; ++index)
                this._block[index] = bytesToLongFromBuffer( this._buffer, index * 8 );
            this.processBlock();
            this._bufferPos = 0;
            Array.Clear( _buffer, 0, this._buffer.Length );
        }

        private static long bytesToLongFromBuffer( byte[] buffer, int startPos ) => ((buffer[startPos] & (long)byte.MaxValue) << 56) | ((buffer[startPos + 1] & (long)byte.MaxValue) << 48) | ((buffer[startPos + 2] & (long)byte.MaxValue) << 40) | ((buffer[startPos + 3] & (long)byte.MaxValue) << 32) | ((buffer[startPos + 4] & (long)byte.MaxValue) << 24) | ((buffer[startPos + 5] & (long)byte.MaxValue) << 16) | ((buffer[startPos + 6] & (long)byte.MaxValue) << 8) | (buffer[startPos + 7] & (long)byte.MaxValue);

        private static void convertLongToByteArray( long inputLong, byte[] outputArray, int offSet )
        {
            for (int index = 0; index < 8; ++index)
                outputArray[offSet + index] = (byte)((ulong)(inputLong >> (56 - (index * 8))) & byte.MaxValue);
        }

        private void processBlock()
        {
            for (int index = 0; index < 8; ++index)
                this._state[index] = this._block[index] ^ (this._K[index] = this._hash[index]);
            for (int index1 = 1; index1 <= 10; ++index1)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    this._L[index2] = 0L;
                    this._L[index2] ^= C0[(int)(this._K[index2 & 7] >> 56) & byte.MaxValue];
                    this._L[index2] ^= C1[(int)(this._K[(index2 - 1) & 7] >> 48) & byte.MaxValue];
                    this._L[index2] ^= C2[(int)(this._K[(index2 - 2) & 7] >> 40) & byte.MaxValue];
                    this._L[index2] ^= C3[(int)(this._K[(index2 - 3) & 7] >> 32) & byte.MaxValue];
                    this._L[index2] ^= C4[(int)(this._K[(index2 - 4) & 7] >> 24) & byte.MaxValue];
                    this._L[index2] ^= C5[(int)(this._K[(index2 - 5) & 7] >> 16) & byte.MaxValue];
                    this._L[index2] ^= C6[(int)(this._K[(index2 - 6) & 7] >> 8) & byte.MaxValue];
                    this._L[index2] ^= C7[(int)this._K[(index2 - 7) & 7] & byte.MaxValue];
                }
                Array.Copy( _L, 0, _K, 0, this._K.Length );
                long[] k;
                (k = this._K)[0] = k[0] ^ this._rc[index1];
                for (int index3 = 0; index3 < 8; ++index3)
                {
                    this._L[index3] = this._K[index3];
                    this._L[index3] ^= C0[(int)(this._state[index3 & 7] >> 56) & byte.MaxValue];
                    this._L[index3] ^= C1[(int)(this._state[(index3 - 1) & 7] >> 48) & byte.MaxValue];
                    this._L[index3] ^= C2[(int)(this._state[(index3 - 2) & 7] >> 40) & byte.MaxValue];
                    this._L[index3] ^= C3[(int)(this._state[(index3 - 3) & 7] >> 32) & byte.MaxValue];
                    this._L[index3] ^= C4[(int)(this._state[(index3 - 4) & 7] >> 24) & byte.MaxValue];
                    this._L[index3] ^= C5[(int)(this._state[(index3 - 5) & 7] >> 16) & byte.MaxValue];
                    this._L[index3] ^= C6[(int)(this._state[(index3 - 6) & 7] >> 8) & byte.MaxValue];
                    this._L[index3] ^= C7[(int)this._state[(index3 - 7) & 7] & byte.MaxValue];
                }
                Array.Copy( _L, 0, _state, 0, this._state.Length );
            }
            for (int index = 0; index < 8; ++index)
                this._hash[index] ^= this._state[index] ^ this._block[index];
        }

        public void Update( byte input )
        {
            this._buffer[this._bufferPos] = input;
            ++this._bufferPos;
            if (this._bufferPos == this._buffer.Length)
                this.processFilledBuffer();
            this.increment();
        }

        private void increment()
        {
            int num1 = 0;
            for (int index = this._bitCount.Length - 1; index >= 0; --index)
            {
                int num2 = (this._bitCount[index] & byte.MaxValue) + EIGHT[index] + num1;
                num1 = num2 >> 8;
                this._bitCount[index] = (short)(num2 & byte.MaxValue);
            }
        }

        public void BlockUpdate( byte[] input, int inOff, int length )
        {
            for (; length > 0; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
        }

        private void finish()
        {
            byte[] sourceArray = this.copyBitLength();
            byte[] buffer;
            byte[] numArray = buffer = this._buffer;
            int num1 = this._bufferPos++;
            int index1;
            int index2 = index1 = num1;
            int num2 = (byte)(numArray[(int)(IntPtr)index2] | 128U);
            buffer[index1] = (byte)num2;
            if (this._bufferPos == this._buffer.Length)
                this.processFilledBuffer();
            if (this._bufferPos > 32)
            {
                while (this._bufferPos != 0)
                    this.Update( 0 );
            }
            while (this._bufferPos <= 32)
                this.Update( 0 );
            Array.Copy( sourceArray, 0, _buffer, 32, sourceArray.Length );
            this.processFilledBuffer();
        }

        private byte[] copyBitLength()
        {
            byte[] numArray = new byte[32];
            for (int index = 0; index < numArray.Length; ++index)
                numArray[index] = (byte)((uint)this._bitCount[index] & byte.MaxValue);
            return numArray;
        }

        public int GetByteLength() => 64;

        public IMemoable Copy() => new WhirlpoolDigest( this );

        public void Reset( IMemoable other )
        {
            WhirlpoolDigest whirlpoolDigest = (WhirlpoolDigest)other;
            Array.Copy( whirlpoolDigest._rc, 0, _rc, 0, this._rc.Length );
            Array.Copy( whirlpoolDigest._buffer, 0, _buffer, 0, this._buffer.Length );
            this._bufferPos = whirlpoolDigest._bufferPos;
            Array.Copy( whirlpoolDigest._bitCount, 0, _bitCount, 0, this._bitCount.Length );
            Array.Copy( whirlpoolDigest._hash, 0, _hash, 0, this._hash.Length );
            Array.Copy( whirlpoolDigest._K, 0, _K, 0, this._K.Length );
            Array.Copy( whirlpoolDigest._L, 0, _L, 0, this._L.Length );
            Array.Copy( whirlpoolDigest._block, 0, _block, 0, this._block.Length );
            Array.Copy( whirlpoolDigest._state, 0, _state, 0, this._state.Length );
        }
    }
}
