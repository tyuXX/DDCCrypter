// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.AesLightEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class AesLightEngine : IBlockCipher
    {
        private const uint m1 = 2155905152;
        private const uint m2 = 2139062143;
        private const uint m3 = 27;
        private const uint m4 = 3233857728;
        private const uint m5 = 1061109567;
        private const int BLOCK_SIZE = 16;
        private static readonly byte[] S = new byte[256]
        {
       99,
       124,
       119,
       123,
       242,
       107,
       111,
       197,
       48,
       1,
       103,
       43,
       254,
       215,
       171,
       118,
       202,
       130,
       201,
       125,
       250,
       89,
       71,
       240,
       173,
       212,
       162,
       175,
       156,
       164,
       114,
       192,
       183,
       253,
       147,
       38,
       54,
       63,
       247,
       204,
       52,
       165,
       229,
       241,
       113,
       216,
       49,
       21,
       4,
       199,
       35,
       195,
       24,
       150,
       5,
       154,
       7,
       18,
       128,
       226,
       235,
       39,
       178,
       117,
       9,
       131,
       44,
       26,
       27,
       110,
       90,
       160,
       82,
       59,
       214,
       179,
       41,
       227,
       47,
       132,
       83,
       209,
       0,
       237,
       32,
       252,
       177,
       91,
       106,
       203,
       190,
       57,
       74,
       76,
       88,
       207,
       208,
       239,
       170,
       251,
       67,
       77,
       51,
       133,
       69,
       249,
       2,
       127,
       80,
       60,
       159,
       168,
       81,
       163,
       64,
       143,
       146,
       157,
       56,
       245,
       188,
       182,
       218,
       33,
       16,
      byte.MaxValue,
       243,
       210,
       205,
       12,
       19,
       236,
       95,
       151,
       68,
       23,
       196,
       167,
       126,
       61,
       100,
       93,
       25,
       115,
       96,
       129,
       79,
       220,
       34,
       42,
       144,
       136,
       70,
       238,
       184,
       20,
       222,
       94,
       11,
       219,
       224,
       50,
       58,
       10,
       73,
       6,
       36,
       92,
       194,
       211,
       172,
       98,
       145,
       149,
       228,
       121,
       231,
       200,
       55,
       109,
       141,
       213,
       78,
       169,
       108,
       86,
       244,
       234,
       101,
       122,
       174,
       8,
       186,
       120,
       37,
       46,
       28,
       166,
       180,
       198,
       232,
       221,
       116,
       31,
       75,
       189,
       139,
       138,
       112,
       62,
       181,
       102,
       72,
       3,
       246,
       14,
       97,
       53,
       87,
       185,
       134,
       193,
       29,
       158,
       225,
       248,
       152,
       17,
       105,
       217,
       142,
       148,
       155,
       30,
       135,
       233,
       206,
       85,
       40,
       223,
       140,
       161,
       137,
       13,
       191,
       230,
       66,
       104,
       65,
       153,
       45,
       15,
       176,
       84,
       187,
       22
        };
        private static readonly byte[] Si = new byte[256]
        {
       82,
       9,
       106,
       213,
       48,
       54,
       165,
       56,
       191,
       64,
       163,
       158,
       129,
       243,
       215,
       251,
       124,
       227,
       57,
       130,
       155,
       47,
      byte.MaxValue,
       135,
       52,
       142,
       67,
       68,
       196,
       222,
       233,
       203,
       84,
       123,
       148,
       50,
       166,
       194,
       35,
       61,
       238,
       76,
       149,
       11,
       66,
       250,
       195,
       78,
       8,
       46,
       161,
       102,
       40,
       217,
       36,
       178,
       118,
       91,
       162,
       73,
       109,
       139,
       209,
       37,
       114,
       248,
       246,
       100,
       134,
       104,
       152,
       22,
       212,
       164,
       92,
       204,
       93,
       101,
       182,
       146,
       108,
       112,
       72,
       80,
       253,
       237,
       185,
       218,
       94,
       21,
       70,
       87,
       167,
       141,
       157,
       132,
       144,
       216,
       171,
       0,
       140,
       188,
       211,
       10,
       247,
       228,
       88,
       5,
       184,
       179,
       69,
       6,
       208,
       44,
       30,
       143,
       202,
       63,
       15,
       2,
       193,
       175,
       189,
       3,
       1,
       19,
       138,
       107,
       58,
       145,
       17,
       65,
       79,
       103,
       220,
       234,
       151,
       242,
       207,
       206,
       240,
       180,
       230,
       115,
       150,
       172,
       116,
       34,
       231,
       173,
       53,
       133,
       226,
       249,
       55,
       232,
       28,
       117,
       223,
       110,
       71,
       241,
       26,
       113,
       29,
       41,
       197,
       137,
       111,
       183,
       98,
       14,
       170,
       24,
       190,
       27,
       252,
       86,
       62,
       75,
       198,
       210,
       121,
       32,
       154,
       219,
       192,
       254,
       120,
       205,
       90,
       244,
       31,
       221,
       168,
       51,
       136,
       7,
       199,
       49,
       177,
       18,
       16,
       89,
       39,
       128,
       236,
       95,
       96,
       81,
       127,
       169,
       25,
       181,
       74,
       13,
       45,
       229,
       122,
       159,
       147,
       201,
       156,
       239,
       160,
       224,
       59,
       77,
       174,
       42,
       245,
       176,
       200,
       235,
       187,
       60,
       131,
       83,
       153,
       97,
       23,
       43,
       4,
       126,
       186,
       119,
       214,
       38,
       225,
       105,
       20,
       99,
       85,
       33,
       12,
       125
        };
        private static readonly byte[] rcon = new byte[30]
        {
       1,
       2,
       4,
       8,
       16,
       32,
       64,
       128,
       27,
       54,
       108,
       216,
       171,
       77,
       154,
       47,
       94,
       188,
       99,
       198,
       151,
       53,
       106,
       212,
       179,
       125,
       250,
       239,
       197,
       145
        };
        private int ROUNDS;
        private uint[][] WorkingKey;
        private uint C0;
        private uint C1;
        private uint C2;
        private uint C3;
        private bool forEncryption;

        private static uint Shift( uint r, int shift ) => (r >> shift) | (r << (32 - shift));

        private static uint FFmulX( uint x ) => (uint)((((int)x & 2139062143) << 1) ^ ((int)((x & 2155905152U) >> 7) * 27));

        private static uint FFmulX2( uint x )
        {
            uint num1 = (uint)(((int)x & 1061109567) << 2);
            uint num2 = x & 3233857728U;
            uint num3 = num2 ^ (num2 >> 1);
            return num1 ^ (num3 >> 2) ^ (num3 >> 5);
        }

        private static uint Mcol( uint x )
        {
            uint num1 = Shift( x, 8 );
            uint num2 = x ^ num1;
            return Shift( num2, 16 ) ^ num1 ^ FFmulX( num2 );
        }

        private static uint Inv_Mcol( uint x )
        {
            uint r1 = x;
            uint x1 = r1 ^ Shift( r1, 8 );
            uint x2 = r1 ^ FFmulX( x1 );
            uint r2 = x1 ^ FFmulX2( x2 );
            return x2 ^ r2 ^ Shift( r2, 16 );
        }

        private static uint SubWord( uint x ) => (uint)(S[(int)(IntPtr)(x & byte.MaxValue)] | (S[(int)(IntPtr)((x >> 8) & byte.MaxValue)] << 8) | (S[(int)(IntPtr)((x >> 16) & byte.MaxValue)] << 16) | (S[(int)(IntPtr)((x >> 24) & byte.MaxValue)] << 24));

        private uint[][] GenerateWorkingKey( byte[] key, bool forEncryption )
        {
            int length = key.Length;
            if (length < 16 || length > 32 || (length & 7) != 0)
                throw new ArgumentException( "Key length not 128/192/256 bits." );
            int num1 = length >> 2;
            this.ROUNDS = num1 + 6;
            uint[][] workingKey = new uint[this.ROUNDS + 1][];
            for (int index = 0; index <= this.ROUNDS; ++index)
                workingKey[index] = new uint[4];
            switch (num1)
            {
                case 4:
                    uint uint32_1 = Pack.LE_To_UInt32( key, 0 );
                    workingKey[0][0] = uint32_1;
                    uint uint32_2 = Pack.LE_To_UInt32( key, 4 );
                    workingKey[0][1] = uint32_2;
                    uint uint32_3 = Pack.LE_To_UInt32( key, 8 );
                    workingKey[0][2] = uint32_3;
                    uint uint32_4 = Pack.LE_To_UInt32( key, 12 );
                    workingKey[0][3] = uint32_4;
                    for (int index = 1; index <= 10; ++index)
                    {
                        uint num2 = SubWord( Shift( uint32_4, 8 ) ) ^ rcon[index - 1];
                        uint32_1 ^= num2;
                        workingKey[index][0] = uint32_1;
                        uint32_2 ^= uint32_1;
                        workingKey[index][1] = uint32_2;
                        uint32_3 ^= uint32_2;
                        workingKey[index][2] = uint32_3;
                        uint32_4 ^= uint32_3;
                        workingKey[index][3] = uint32_4;
                    }
                    break;
                case 6:
                    uint uint32_5 = Pack.LE_To_UInt32( key, 0 );
                    workingKey[0][0] = uint32_5;
                    uint uint32_6 = Pack.LE_To_UInt32( key, 4 );
                    workingKey[0][1] = uint32_6;
                    uint uint32_7 = Pack.LE_To_UInt32( key, 8 );
                    workingKey[0][2] = uint32_7;
                    uint uint32_8 = Pack.LE_To_UInt32( key, 12 );
                    workingKey[0][3] = uint32_8;
                    uint uint32_9 = Pack.LE_To_UInt32( key, 16 );
                    workingKey[1][0] = uint32_9;
                    uint uint32_10 = Pack.LE_To_UInt32( key, 20 );
                    workingKey[1][1] = uint32_10;
                    uint num3 = 1;
                    uint num4 = SubWord( Shift( uint32_10, 8 ) ) ^ num3;
                    uint num5 = num3 << 1;
                    uint num6 = uint32_5 ^ num4;
                    workingKey[1][2] = num6;
                    uint num7 = uint32_6 ^ num6;
                    workingKey[1][3] = num7;
                    uint num8 = uint32_7 ^ num7;
                    workingKey[2][0] = num8;
                    uint num9 = uint32_8 ^ num8;
                    workingKey[2][1] = num9;
                    uint num10 = uint32_9 ^ num9;
                    workingKey[2][2] = num10;
                    uint r1 = uint32_10 ^ num10;
                    workingKey[2][3] = r1;
                    for (int index = 3; index < 12; index += 3)
                    {
                        uint num11 = SubWord( Shift( r1, 8 ) ) ^ num5;
                        uint num12 = num5 << 1;
                        uint num13 = num6 ^ num11;
                        workingKey[index][0] = num13;
                        uint num14 = num7 ^ num13;
                        workingKey[index][1] = num14;
                        uint num15 = num8 ^ num14;
                        workingKey[index][2] = num15;
                        uint num16 = num9 ^ num15;
                        workingKey[index][3] = num16;
                        uint num17 = num10 ^ num16;
                        workingKey[index + 1][0] = num17;
                        uint r2 = r1 ^ num17;
                        workingKey[index + 1][1] = r2;
                        uint num18 = SubWord( Shift( r2, 8 ) ) ^ num12;
                        num5 = num12 << 1;
                        num6 = num13 ^ num18;
                        workingKey[index + 1][2] = num6;
                        num7 = num14 ^ num6;
                        workingKey[index + 1][3] = num7;
                        num8 = num15 ^ num7;
                        workingKey[index + 2][0] = num8;
                        num9 = num16 ^ num8;
                        workingKey[index + 2][1] = num9;
                        num10 = num17 ^ num9;
                        workingKey[index + 2][2] = num10;
                        r1 = r2 ^ num10;
                        workingKey[index + 2][3] = r1;
                    }
                    uint num19 = SubWord( Shift( r1, 8 ) ) ^ num5;
                    uint num20 = num6 ^ num19;
                    workingKey[12][0] = num20;
                    uint num21 = num7 ^ num20;
                    workingKey[12][1] = num21;
                    uint num22 = num8 ^ num21;
                    workingKey[12][2] = num22;
                    uint num23 = num9 ^ num22;
                    workingKey[12][3] = num23;
                    break;
                case 8:
                    uint uint32_11 = Pack.LE_To_UInt32( key, 0 );
                    workingKey[0][0] = uint32_11;
                    uint uint32_12 = Pack.LE_To_UInt32( key, 4 );
                    workingKey[0][1] = uint32_12;
                    uint uint32_13 = Pack.LE_To_UInt32( key, 8 );
                    workingKey[0][2] = uint32_13;
                    uint uint32_14 = Pack.LE_To_UInt32( key, 12 );
                    workingKey[0][3] = uint32_14;
                    uint uint32_15 = Pack.LE_To_UInt32( key, 16 );
                    workingKey[1][0] = uint32_15;
                    uint uint32_16 = Pack.LE_To_UInt32( key, 20 );
                    workingKey[1][1] = uint32_16;
                    uint uint32_17 = Pack.LE_To_UInt32( key, 24 );
                    workingKey[1][2] = uint32_17;
                    uint uint32_18 = Pack.LE_To_UInt32( key, 28 );
                    workingKey[1][3] = uint32_18;
                    uint num24 = 1;
                    for (int index = 2; index < 14; index += 2)
                    {
                        uint num25 = SubWord( Shift( uint32_18, 8 ) ) ^ num24;
                        num24 <<= 1;
                        uint32_11 ^= num25;
                        workingKey[index][0] = uint32_11;
                        uint32_12 ^= uint32_11;
                        workingKey[index][1] = uint32_12;
                        uint32_13 ^= uint32_12;
                        workingKey[index][2] = uint32_13;
                        uint32_14 ^= uint32_13;
                        workingKey[index][3] = uint32_14;
                        uint num26 = SubWord( uint32_14 );
                        uint32_15 ^= num26;
                        workingKey[index + 1][0] = uint32_15;
                        uint32_16 ^= uint32_15;
                        workingKey[index + 1][1] = uint32_16;
                        uint32_17 ^= uint32_16;
                        workingKey[index + 1][2] = uint32_17;
                        uint32_18 ^= uint32_17;
                        workingKey[index + 1][3] = uint32_18;
                    }
                    uint num27 = SubWord( Shift( uint32_18, 8 ) ) ^ num24;
                    uint num28 = uint32_11 ^ num27;
                    workingKey[14][0] = num28;
                    uint num29 = uint32_12 ^ num28;
                    workingKey[14][1] = num29;
                    uint num30 = uint32_13 ^ num29;
                    workingKey[14][2] = num30;
                    uint num31 = uint32_14 ^ num30;
                    workingKey[14][3] = num31;
                    break;
                default:
                    throw new InvalidOperationException( "Should never get here" );
            }
            if (!forEncryption)
            {
                for (int index1 = 1; index1 < this.ROUNDS; ++index1)
                {
                    uint[] numArray = workingKey[index1];
                    for (int index2 = 0; index2 < 4; ++index2)
                        numArray[index2] = Inv_Mcol( numArray[index2] );
                }
            }
            return workingKey;
        }

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.WorkingKey = parameters is KeyParameter keyParameter ? this.GenerateWorkingKey( keyParameter.GetKey(), forEncryption ) : throw new ArgumentException( "invalid parameter passed to AES init - " + Platform.GetTypeName( parameters ) );
            this.forEncryption = forEncryption;
        }

        public virtual string AlgorithmName => "AES";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 16;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.WorkingKey == null)
                throw new InvalidOperationException( "AES engine not initialised" );
            Check.DataLength( input, inOff, 16, "input buffer too short" );
            Check.OutputLength( output, outOff, 16, "output buffer too short" );
            this.UnPackBlock( input, inOff );
            if (this.forEncryption)
                this.EncryptBlock( this.WorkingKey );
            else
                this.DecryptBlock( this.WorkingKey );
            this.PackBlock( output, outOff );
            return 16;
        }

        public virtual void Reset()
        {
        }

        private void UnPackBlock( byte[] bytes, int off )
        {
            this.C0 = Pack.LE_To_UInt32( bytes, off );
            this.C1 = Pack.LE_To_UInt32( bytes, off + 4 );
            this.C2 = Pack.LE_To_UInt32( bytes, off + 8 );
            this.C3 = Pack.LE_To_UInt32( bytes, off + 12 );
        }

        private void PackBlock( byte[] bytes, int off )
        {
            Pack.UInt32_To_LE( this.C0, bytes, off );
            Pack.UInt32_To_LE( this.C1, bytes, off + 4 );
            Pack.UInt32_To_LE( this.C2, bytes, off + 8 );
            Pack.UInt32_To_LE( this.C3, bytes, off + 12 );
        }

        private void EncryptBlock( uint[][] KW )
        {
            uint[] numArray1 = KW[0];
            uint num1 = this.C0 ^ numArray1[0];
            uint num2 = this.C1 ^ numArray1[1];
            uint num3 = this.C2 ^ numArray1[2];
            uint num4 = this.C3 ^ numArray1[3];
            int num5 = 1;
            while (num5 < this.ROUNDS - 1)
            {
                uint[][] numArray2 = KW;
                int index1 = num5;
                int num6 = index1 + 1;
                uint[] numArray3 = numArray2[index1];
                uint num7 = Mcol( (uint)(S[(int)(IntPtr)(num1 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[0];
                uint num8 = Mcol( (uint)(S[(int)(IntPtr)(num2 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[1];
                uint num9 = Mcol( (uint)(S[(int)(IntPtr)(num3 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[2];
                uint num10 = Mcol( (uint)(S[(int)(IntPtr)(num4 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[3];
                uint[][] numArray4 = KW;
                int index2 = num6;
                num5 = index2 + 1;
                uint[] numArray5 = numArray4[index2];
                num1 = Mcol( (uint)(S[(int)(IntPtr)(num7 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num8 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num9 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num10 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[0];
                num2 = Mcol( (uint)(S[(int)(IntPtr)(num8 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num9 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num10 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num7 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[1];
                num3 = Mcol( (uint)(S[(int)(IntPtr)(num9 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num10 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num7 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num8 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[2];
                num4 = Mcol( (uint)(S[(int)(IntPtr)(num10 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num7 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num8 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num9 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[3];
            }
            uint[][] numArray6 = KW;
            int index3 = num5;
            int index4 = index3 + 1;
            uint[] numArray7 = numArray6[index3];
            uint num11 = Mcol( (uint)(S[(int)(IntPtr)(num1 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray7[0];
            uint num12 = Mcol( (uint)(S[(int)(IntPtr)(num2 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray7[1];
            uint num13 = Mcol( (uint)(S[(int)(IntPtr)(num3 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray7[2];
            uint num14 = Mcol( (uint)(S[(int)(IntPtr)(num4 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray7[3];
            uint[] numArray8 = KW[index4];
            this.C0 = (uint)(S[(int)(IntPtr)(num11 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num12 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num13 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num14 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[0];
            this.C1 = (uint)(S[(int)(IntPtr)(num12 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num13 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num14 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num11 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[1];
            this.C2 = (uint)(S[(int)(IntPtr)(num13 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num14 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num11 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num12 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[2];
            this.C3 = (uint)(S[(int)(IntPtr)(num14 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num11 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num12 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num13 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[3];
        }

        private void DecryptBlock( uint[][] KW )
        {
            uint[] numArray1 = KW[this.ROUNDS];
            uint num1 = this.C0 ^ numArray1[0];
            uint num2 = this.C1 ^ numArray1[1];
            uint num3 = this.C2 ^ numArray1[2];
            uint num4 = this.C3 ^ numArray1[3];
            int num5 = this.ROUNDS - 1;
            while (num5 > 1)
            {
                uint[][] numArray2 = KW;
                int index1 = num5;
                int num6 = index1 - 1;
                uint[] numArray3 = numArray2[index1];
                uint num7 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num1 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[0];
                uint num8 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num2 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[1];
                uint num9 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num3 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[2];
                uint num10 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num4 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray3[3];
                uint[][] numArray4 = KW;
                int index2 = num6;
                num5 = index2 - 1;
                uint[] numArray5 = numArray4[index2];
                num1 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num7 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num10 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num9 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num8 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[0];
                num2 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num8 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num7 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num10 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num9 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[1];
                num3 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num9 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num8 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num7 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num10 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[2];
                num4 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num10 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num9 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num8 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num7 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray5[3];
            }
            uint[] numArray6 = KW[1];
            uint num11 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num1 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray6[0];
            uint num12 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num2 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray6[1];
            uint num13 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num3 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray6[2];
            uint num14 = Inv_Mcol( (uint)(Si[(int)(IntPtr)(num4 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)] << 24)) ) ^ numArray6[3];
            uint[] numArray7 = KW[0];
            this.C0 = (uint)(Si[(int)(IntPtr)(num11 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num14 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num13 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num12 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[0];
            this.C1 = (uint)(Si[(int)(IntPtr)(num12 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num11 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num14 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num13 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[1];
            this.C2 = (uint)(Si[(int)(IntPtr)(num13 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num12 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num11 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num14 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[2];
            this.C3 = (uint)(Si[(int)(IntPtr)(num14 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num13 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num12 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num11 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[3];
        }
    }
}
