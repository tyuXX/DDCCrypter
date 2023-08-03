// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.TwofishEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public sealed class TwofishEngine : IBlockCipher
    {
        private const int P_00 = 1;
        private const int P_01 = 0;
        private const int P_02 = 0;
        private const int P_03 = 1;
        private const int P_04 = 1;
        private const int P_10 = 0;
        private const int P_11 = 0;
        private const int P_12 = 1;
        private const int P_13 = 1;
        private const int P_14 = 0;
        private const int P_20 = 1;
        private const int P_21 = 1;
        private const int P_22 = 0;
        private const int P_23 = 0;
        private const int P_24 = 0;
        private const int P_30 = 0;
        private const int P_31 = 1;
        private const int P_32 = 1;
        private const int P_33 = 0;
        private const int P_34 = 1;
        private const int GF256_FDBK = 361;
        private const int GF256_FDBK_2 = 180;
        private const int GF256_FDBK_4 = 90;
        private const int RS_GF_FDBK = 333;
        private const int ROUNDS = 16;
        private const int MAX_ROUNDS = 16;
        private const int BLOCK_SIZE = 16;
        private const int MAX_KEY_BITS = 256;
        private const int INPUT_WHITEN = 0;
        private const int OUTPUT_WHITEN = 4;
        private const int ROUND_SUBKEYS = 8;
        private const int TOTAL_SUBKEYS = 40;
        private const int SK_STEP = 33686018;
        private const int SK_BUMP = 16843009;
        private const int SK_ROTL = 9;
        private static readonly byte[,] P = new byte[2, 256]
        {
      {
         169,
         103,
         179,
         232,
         4,
         253,
         163,
         118,
         154,
         146,
         128,
         120,
         228,
         221,
         209,
         56,
         13,
         198,
         53,
         152,
         24,
         247,
         236,
         108,
         67,
         117,
         55,
         38,
         250,
         19,
         148,
         72,
         242,
         208,
         139,
         48,
         132,
         84,
         223,
         35,
         25,
         91,
         61,
         89,
         243,
         174,
         162,
         130,
         99,
         1,
         131,
         46,
         217,
         81,
         155,
         124,
         166,
         235,
         165,
         190,
         22,
         12,
         227,
         97,
         192,
         140,
         58,
         245,
         115,
         44,
         37,
         11,
         187,
         78,
         137,
         107,
         83,
         106,
         180,
         241,
         225,
         230,
         189,
         69,
         226,
         244,
         182,
         102,
         204,
         149,
         3,
         86,
         212,
         28,
         30,
         215,
         251,
         195,
         142,
         181,
         233,
         207,
         191,
         186,
         234,
         119,
         57,
         175,
         51,
         201,
         98,
         113,
         129,
         121,
         9,
         173,
         36,
         205,
         249,
         216,
         229,
         197,
         185,
         77,
         68,
         8,
         134,
         231,
         161,
         29,
         170,
         237,
         6,
         112,
         178,
         210,
         65,
         123,
         160,
         17,
         49,
         194,
         39,
         144,
         32,
         246,
         96,
        byte.MaxValue,
         150,
         92,
         177,
         171,
         158,
         156,
         82,
         27,
         95,
         147,
         10,
         239,
         145,
         133,
         73,
         238,
         45,
         79,
         143,
         59,
         71,
         135,
         109,
         70,
         214,
         62,
         105,
         100,
         42,
         206,
         203,
         47,
         252,
         151,
         5,
         122,
         172,
         127,
         213,
         26,
         75,
         14,
         167,
         90,
         40,
         20,
         63,
         41,
         136,
         60,
         76,
         2,
         184,
         218,
         176,
         23,
         85,
         31,
         138,
         125,
         87,
         199,
         141,
         116,
         183,
         196,
         159,
         114,
         126,
         21,
         34,
         18,
         88,
         7,
         153,
         52,
         110,
         80,
         222,
         104,
         101,
         188,
         219,
         248,
         200,
         168,
         43,
         64,
         220,
         254,
         50,
         164,
         202,
         16,
         33,
         240,
         211,
         93,
         15,
         0,
         111,
         157,
         54,
         66,
         74,
         94,
         193,
         224
      },
      {
         117,
         243,
         198,
         244,
         219,
         123,
         251,
         200,
         74,
         211,
         230,
         107,
         69,
         125,
         232,
         75,
         214,
         50,
         216,
         253,
         55,
         113,
         241,
         225,
         48,
         15,
         248,
         27,
         135,
         250,
         6,
         63,
         94,
         186,
         174,
         91,
         138,
         0,
         188,
         157,
         109,
         193,
         177,
         14,
         128,
         93,
         210,
         213,
         160,
         132,
         7,
         20,
         181,
         144,
         44,
         163,
         178,
         115,
         76,
         84,
         146,
         116,
         54,
         81,
         56,
         176,
         189,
         90,
         252,
         96,
         98,
         150,
         108,
         66,
         247,
         16,
         124,
         40,
         39,
         140,
         19,
         149,
         156,
         199,
         36,
         70,
         59,
         112,
         202,
         227,
         133,
         203,
         17,
         208,
         147,
         184,
         166,
         131,
         32,
        byte.MaxValue,
         159,
         119,
         195,
         204,
         3,
         111,
         8,
         191,
         64,
         231,
         43,
         226,
         121,
         12,
         170,
         130,
         65,
         58,
         234,
         185,
         228,
         154,
         164,
         151,
         126,
         218,
         122,
         23,
         102,
         148,
         161,
         29,
         61,
         240,
         222,
         179,
         11,
         114,
         167,
         28,
         239,
         209,
         83,
         62,
         143,
         51,
         38,
         95,
         236,
         118,
         42,
         73,
         129,
         136,
         238,
         33,
         196,
         26,
         235,
         217,
         197,
         57,
         153,
         205,
         173,
         49,
         139,
         1,
         24,
         35,
         221,
         31,
         78,
         45,
         249,
         72,
         79,
         242,
         101,
         142,
         120,
         92,
         88,
         25,
         141,
         229,
         152,
         87,
         103,
         127,
         5,
         100,
         175,
         99,
         182,
         254,
         245,
         183,
         60,
         165,
         206,
         233,
         104,
         68,
         224,
         77,
         67,
         105,
         41,
         46,
         172,
         21,
         89,
         168,
         10,
         158,
         110,
         71,
         223,
         52,
         53,
         106,
         207,
         220,
         34,
         201,
         192,
         155,
         137,
         212,
         237,
         171,
         18,
         162,
         13,
         82,
         187,
         2,
         47,
         169,
         215,
         97,
         30,
         180,
         80,
         4,
         246,
         194,
         22,
         37,
         134,
         86,
         85,
         9,
         190,
         145
      }
        };
        private bool encrypting;
        private int[] gMDS0 = new int[256];
        private int[] gMDS1 = new int[256];
        private int[] gMDS2 = new int[256];
        private int[] gMDS3 = new int[256];
        private int[] gSubKeys;
        private int[] gSBox;
        private int k64Cnt;
        private byte[] workingKey;

        public TwofishEngine()
        {
            int[] numArray1 = new int[2];
            int[] numArray2 = new int[2];
            int[] numArray3 = new int[2];
            for (int index = 0; index < 256; ++index)
            {
                int x1 = P[0, index] & byte.MaxValue;
                numArray1[0] = x1;
                numArray2[0] = this.Mx_X( x1 ) & byte.MaxValue;
                numArray3[0] = this.Mx_Y( x1 ) & byte.MaxValue;
                int x2 = P[1, index] & byte.MaxValue;
                numArray1[1] = x2;
                numArray2[1] = this.Mx_X( x2 ) & byte.MaxValue;
                numArray3[1] = this.Mx_Y( x2 ) & byte.MaxValue;
                this.gMDS0[index] = numArray1[1] | (numArray2[1] << 8) | (numArray3[1] << 16) | (numArray3[1] << 24);
                this.gMDS1[index] = numArray3[0] | (numArray3[0] << 8) | (numArray2[0] << 16) | (numArray1[0] << 24);
                this.gMDS2[index] = numArray2[1] | (numArray3[1] << 8) | (numArray1[1] << 16) | (numArray3[1] << 24);
                this.gMDS3[index] = numArray2[0] | (numArray1[0] << 8) | (numArray3[0] << 16) | (numArray2[0] << 24);
            }
        }

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "invalid parameter passed to Twofish init - " + Platform.GetTypeName( parameters ) );
            this.encrypting = forEncryption;
            this.workingKey = ((KeyParameter)parameters).GetKey();
            this.k64Cnt = this.workingKey.Length / 8;
            this.SetKey( this.workingKey );
        }

        public string AlgorithmName => "Twofish";

        public bool IsPartialBlockOkay => false;

        public int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.workingKey == null)
                throw new InvalidOperationException( "Twofish not initialised" );
            Check.DataLength( input, inOff, 16, "input buffer too short" );
            Check.OutputLength( output, outOff, 16, "output buffer too short" );
            if (this.encrypting)
                this.EncryptBlock( input, inOff, output, outOff );
            else
                this.DecryptBlock( input, inOff, output, outOff );
            return 16;
        }

        public void Reset()
        {
            if (this.workingKey == null)
                return;
            this.SetKey( this.workingKey );
        }

        public int GetBlockSize() => 16;

        private void SetKey( byte[] key )
        {
            int[] k32_1 = new int[4];
            int[] k32_2 = new int[4];
            int[] numArray = new int[4];
            this.gSubKeys = new int[40];
            if (this.k64Cnt < 1)
                throw new ArgumentException( "Key size less than 64 bits" );
            if (this.k64Cnt > 4)
                throw new ArgumentException( "Key size larger than 256 bits" );
            int index1 = 0;
            for (; index1 < this.k64Cnt; ++index1)
            {
                int p = index1 * 8;
                k32_1[index1] = this.BytesTo32Bits( key, p );
                k32_2[index1] = this.BytesTo32Bits( key, p + 4 );
                numArray[this.k64Cnt - 1 - index1] = this.RS_MDS_Encode( k32_1[index1], k32_2[index1] );
            }
            for (int index2 = 0; index2 < 20; ++index2)
            {
                int x = index2 * 33686018;
                int num1 = this.F32( x, k32_1 );
                int num2 = this.F32( x + 16843009, k32_2 );
                int num3 = (num2 << 8) | num2 >>> 24;
                int num4 = num1 + num3;
                this.gSubKeys[index2 * 2] = num4;
                int num5 = num4 + num3;
                this.gSubKeys[(index2 * 2) + 1] = (num5 << 9) | num5 >>> 23;
            }
            int x1 = numArray[0];
            int x2 = numArray[1];
            int x3 = numArray[2];
            int x4 = numArray[3];
            this.gSBox = new int[1024];
            for (int index3 = 0; index3 < 256; ++index3)
            {
                int num;
                int index4 = num = index3;
                int index5 = num;
                int index6 = num;
                int index7 = num;
                switch (this.k64Cnt & 3)
                {
                    case 0:
                        index7 = (P[1, index7] & byte.MaxValue) ^ this.M_b0( x4 );
                        index6 = (P[0, index6] & byte.MaxValue) ^ this.M_b1( x4 );
                        index5 = (P[0, index5] & byte.MaxValue) ^ this.M_b2( x4 );
                        index4 = (P[1, index4] & byte.MaxValue) ^ this.M_b3( x4 );
                        goto case 3;
                    case 1:
                        this.gSBox[index3 * 2] = this.gMDS0[(P[0, index7] & byte.MaxValue) ^ this.M_b0( x1 )];
                        this.gSBox[(index3 * 2) + 1] = this.gMDS1[(P[0, index6] & byte.MaxValue) ^ this.M_b1( x1 )];
                        this.gSBox[(index3 * 2) + 512] = this.gMDS2[(P[1, index5] & byte.MaxValue) ^ this.M_b2( x1 )];
                        this.gSBox[(index3 * 2) + 513] = this.gMDS3[(P[1, index4] & byte.MaxValue) ^ this.M_b3( x1 )];
                        break;
                    case 2:
                        this.gSBox[index3 * 2] = this.gMDS0[(P[0, (P[0, index7] & byte.MaxValue) ^ this.M_b0( x2 )] & byte.MaxValue) ^ this.M_b0( x1 )];
                        this.gSBox[(index3 * 2) + 1] = this.gMDS1[(P[0, (P[1, index6] & byte.MaxValue) ^ this.M_b1( x2 )] & byte.MaxValue) ^ this.M_b1( x1 )];
                        this.gSBox[(index3 * 2) + 512] = this.gMDS2[(P[1, (P[0, index5] & byte.MaxValue) ^ this.M_b2( x2 )] & byte.MaxValue) ^ this.M_b2( x1 )];
                        this.gSBox[(index3 * 2) + 513] = this.gMDS3[(P[1, (P[1, index4] & byte.MaxValue) ^ this.M_b3( x2 )] & byte.MaxValue) ^ this.M_b3( x1 )];
                        break;
                    case 3:
                        index7 = (P[1, index7] & byte.MaxValue) ^ this.M_b0( x3 );
                        index6 = (P[1, index6] & byte.MaxValue) ^ this.M_b1( x3 );
                        index5 = (P[0, index5] & byte.MaxValue) ^ this.M_b2( x3 );
                        index4 = (P[0, index4] & byte.MaxValue) ^ this.M_b3( x3 );
                        goto case 2;
                }
            }
        }

        private void EncryptBlock( byte[] src, int srcIndex, byte[] dst, int dstIndex )
        {
            int x1 = this.BytesTo32Bits( src, srcIndex ) ^ this.gSubKeys[0];
            int x2 = this.BytesTo32Bits( src, srcIndex + 4 ) ^ this.gSubKeys[1];
            int x3 = this.BytesTo32Bits( src, srcIndex + 8 ) ^ this.gSubKeys[2];
            int x4 = this.BytesTo32Bits( src, srcIndex + 12 ) ^ this.gSubKeys[3];
            int num1 = 8;
            for (int index1 = 0; index1 < 16; index1 += 2)
            {
                int num2 = this.Fe32_0( x1 );
                int num3 = this.Fe32_3( x2 );
                int num4 = x3;
                int num5 = num2 + num3;
                int[] gSubKeys1 = this.gSubKeys;
                int index2 = num1;
                int num6 = index2 + 1;
                int num7 = gSubKeys1[index2];
                int num8 = num5 + num7;
                int num9 = num4 ^ num8;
                x3 = num9 >>> 1 | (num9 << 31);
                int num10 = (x4 << 1) | x4 >>> 31;
                int num11 = num2 + (2 * num3);
                int[] gSubKeys2 = this.gSubKeys;
                int index3 = num6;
                int num12 = index3 + 1;
                int num13 = gSubKeys2[index3];
                int num14 = num11 + num13;
                x4 = num10 ^ num14;
                int num15 = this.Fe32_0( x3 );
                int num16 = this.Fe32_3( x4 );
                int num17 = x1;
                int num18 = num15 + num16;
                int[] gSubKeys3 = this.gSubKeys;
                int index4 = num12;
                int num19 = index4 + 1;
                int num20 = gSubKeys3[index4];
                int num21 = num18 + num20;
                int num22 = num17 ^ num21;
                x1 = num22 >>> 1 | (num22 << 31);
                int num23 = (x2 << 1) | x2 >>> 31;
                int num24 = num15 + (2 * num16);
                int[] gSubKeys4 = this.gSubKeys;
                int index5 = num19;
                num1 = index5 + 1;
                int num25 = gSubKeys4[index5];
                int num26 = num24 + num25;
                x2 = num23 ^ num26;
            }
            this.Bits32ToBytes( x3 ^ this.gSubKeys[4], dst, dstIndex );
            this.Bits32ToBytes( x4 ^ this.gSubKeys[5], dst, dstIndex + 4 );
            this.Bits32ToBytes( x1 ^ this.gSubKeys[6], dst, dstIndex + 8 );
            this.Bits32ToBytes( x2 ^ this.gSubKeys[7], dst, dstIndex + 12 );
        }

        private void DecryptBlock( byte[] src, int srcIndex, byte[] dst, int dstIndex )
        {
            int x1 = this.BytesTo32Bits( src, srcIndex ) ^ this.gSubKeys[4];
            int x2 = this.BytesTo32Bits( src, srcIndex + 4 ) ^ this.gSubKeys[5];
            int x3 = this.BytesTo32Bits( src, srcIndex + 8 ) ^ this.gSubKeys[6];
            int x4 = this.BytesTo32Bits( src, srcIndex + 12 ) ^ this.gSubKeys[7];
            int num1 = 39;
            for (int index1 = 0; index1 < 16; index1 += 2)
            {
                int num2 = this.Fe32_0( x1 );
                int num3 = this.Fe32_3( x2 );
                int num4 = x4;
                int num5 = num2 + (2 * num3);
                int[] gSubKeys1 = this.gSubKeys;
                int index2 = num1;
                int num6 = index2 - 1;
                int num7 = gSubKeys1[index2];
                int num8 = num5 + num7;
                int num9 = num4 ^ num8;
                int num10 = (x3 << 1) | x3 >>> 31;
                int num11 = num2 + num3;
                int[] gSubKeys2 = this.gSubKeys;
                int index3 = num6;
                int num12 = index3 - 1;
                int num13 = gSubKeys2[index3];
                int num14 = num11 + num13;
                x3 = num10 ^ num14;
                x4 = num9 >>> 1 | (num9 << 31);
                int num15 = this.Fe32_0( x3 );
                int num16 = this.Fe32_3( x4 );
                int num17 = x2;
                int num18 = num15 + (2 * num16);
                int[] gSubKeys3 = this.gSubKeys;
                int index4 = num12;
                int num19 = index4 - 1;
                int num20 = gSubKeys3[index4];
                int num21 = num18 + num20;
                int num22 = num17 ^ num21;
                int num23 = (x1 << 1) | x1 >>> 31;
                int num24 = num15 + num16;
                int[] gSubKeys4 = this.gSubKeys;
                int index5 = num19;
                num1 = index5 - 1;
                int num25 = gSubKeys4[index5];
                int num26 = num24 + num25;
                x1 = num23 ^ num26;
                x2 = num22 >>> 1 | (num22 << 31);
            }
            this.Bits32ToBytes( x3 ^ this.gSubKeys[0], dst, dstIndex );
            this.Bits32ToBytes( x4 ^ this.gSubKeys[1], dst, dstIndex + 4 );
            this.Bits32ToBytes( x1 ^ this.gSubKeys[2], dst, dstIndex + 8 );
            this.Bits32ToBytes( x2 ^ this.gSubKeys[3], dst, dstIndex + 12 );
        }

        private int F32( int x, int[] k32 )
        {
            int index1 = this.M_b0( x );
            int index2 = this.M_b1( x );
            int index3 = this.M_b2( x );
            int index4 = this.M_b3( x );
            int x1 = k32[0];
            int x2 = k32[1];
            int x3 = k32[2];
            int x4 = k32[3];
            int num = 0;
            switch (this.k64Cnt & 3)
            {
                case 0:
                    index1 = (P[1, index1] & byte.MaxValue) ^ this.M_b0( x4 );
                    index2 = (P[0, index2] & byte.MaxValue) ^ this.M_b1( x4 );
                    index3 = (P[0, index3] & byte.MaxValue) ^ this.M_b2( x4 );
                    index4 = (P[1, index4] & byte.MaxValue) ^ this.M_b3( x4 );
                    goto case 3;
                case 1:
                    num = this.gMDS0[(P[0, index1] & byte.MaxValue) ^ this.M_b0( x1 )] ^ this.gMDS1[(P[0, index2] & byte.MaxValue) ^ this.M_b1( x1 )] ^ this.gMDS2[(P[1, index3] & byte.MaxValue) ^ this.M_b2( x1 )] ^ this.gMDS3[(P[1, index4] & byte.MaxValue) ^ this.M_b3( x1 )];
                    break;
                case 2:
                    num = this.gMDS0[(P[0, (P[0, index1] & byte.MaxValue) ^ this.M_b0( x2 )] & byte.MaxValue) ^ this.M_b0( x1 )] ^ this.gMDS1[(P[0, (P[1, index2] & byte.MaxValue) ^ this.M_b1( x2 )] & byte.MaxValue) ^ this.M_b1( x1 )] ^ this.gMDS2[(P[1, (P[0, index3] & byte.MaxValue) ^ this.M_b2( x2 )] & byte.MaxValue) ^ this.M_b2( x1 )] ^ this.gMDS3[(P[1, (P[1, index4] & byte.MaxValue) ^ this.M_b3( x2 )] & byte.MaxValue) ^ this.M_b3( x1 )];
                    break;
                case 3:
                    index1 = (P[1, index1] & byte.MaxValue) ^ this.M_b0( x3 );
                    index2 = (P[1, index2] & byte.MaxValue) ^ this.M_b1( x3 );
                    index3 = (P[0, index3] & byte.MaxValue) ^ this.M_b2( x3 );
                    index4 = (P[0, index4] & byte.MaxValue) ^ this.M_b3( x3 );
                    goto case 2;
            }
            return num;
        }

        private int RS_MDS_Encode( int k0, int k1 )
        {
            int x1 = k1;
            for (int index = 0; index < 4; ++index)
                x1 = this.RS_rem( x1 );
            int x2 = x1 ^ k0;
            for (int index = 0; index < 4; ++index)
                x2 = this.RS_rem( x2 );
            return x2;
        }

        private int RS_rem( int x )
        {
            int num1 = x >>> 24 & byte.MaxValue;
            int num2 = ((num1 << 1) ^ ((num1 & 128) != 0 ? 333 : 0)) & byte.MaxValue;
            int num3 = num1 >>> 1 ^ ((num1 & 1) != 0 ? 166 : 0) ^ num2;
            return (x << 8) ^ (num3 << 24) ^ (num2 << 16) ^ (num3 << 8) ^ num1;
        }

        private int LFSR1( int x ) => (x >> 1) ^ ((x & 1) != 0 ? 180 : 0);

        private int LFSR2( int x ) => (x >> 2) ^ ((x & 2) != 0 ? 180 : 0) ^ ((x & 1) != 0 ? 90 : 0);

        private int Mx_X( int x ) => x ^ this.LFSR2( x );

        private int Mx_Y( int x ) => x ^ this.LFSR1( x ) ^ this.LFSR2( x );

        private int M_b0( int x ) => x & byte.MaxValue;

        private int M_b1( int x ) => x >>> 8 & byte.MaxValue;

        private int M_b2( int x ) => x >>> 16 & byte.MaxValue;

        private int M_b3( int x ) => x >>> 24 & byte.MaxValue;

        private int Fe32_0( int x ) => this.gSBox[2 * (x & byte.MaxValue)] ^ this.gSBox[1 + (2 * (x >>> 8 & byte.MaxValue))] ^ this.gSBox[512 + (2 * (x >>> 16 & byte.MaxValue))] ^ this.gSBox[513 + (2 * (x >>> 24 & byte.MaxValue))];

        private int Fe32_3( int x ) => this.gSBox[2 * (x >>> 24 & byte.MaxValue)] ^ this.gSBox[1 + (2 * (x & byte.MaxValue))] ^ this.gSBox[512 + (2 * (x >>> 8 & byte.MaxValue))] ^ this.gSBox[513 + (2 * (x >>> 16 & byte.MaxValue))];

        private int BytesTo32Bits( byte[] b, int p ) => (b[p] & byte.MaxValue) | ((b[p + 1] & byte.MaxValue) << 8) | ((b[p + 2] & byte.MaxValue) << 16) | ((b[p + 3] & byte.MaxValue) << 24);

        private void Bits32ToBytes( int inData, byte[] b, int offset )
        {
            b[offset] = (byte)inData;
            b[offset + 1] = (byte)(inData >> 8);
            b[offset + 2] = (byte)(inData >> 16);
            b[offset + 3] = (byte)(inData >> 24);
        }
    }
}
