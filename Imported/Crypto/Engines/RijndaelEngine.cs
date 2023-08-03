// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RijndaelEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RijndaelEngine : IBlockCipher
    {
        private static readonly int MAXROUNDS = 14;
        private static readonly int MAXKC = 64;
        private static readonly byte[] Logtable = new byte[256]
        {
       0,
       0,
       25,
       1,
       50,
       2,
       26,
       198,
       75,
       199,
       27,
       104,
       51,
       238,
       223,
       3,
       100,
       4,
       224,
       14,
       52,
       141,
       129,
       239,
       76,
       113,
       8,
       200,
       248,
       105,
       28,
       193,
       125,
       194,
       29,
       181,
       249,
       185,
       39,
       106,
       77,
       228,
       166,
       114,
       154,
       201,
       9,
       120,
       101,
       47,
       138,
       5,
       33,
       15,
       225,
       36,
       18,
       240,
       130,
       69,
       53,
       147,
       218,
       142,
       150,
       143,
       219,
       189,
       54,
       208,
       206,
       148,
       19,
       92,
       210,
       241,
       64,
       70,
       131,
       56,
       102,
       221,
       253,
       48,
       191,
       6,
       139,
       98,
       179,
       37,
       226,
       152,
       34,
       136,
       145,
       16,
       126,
       110,
       72,
       195,
       163,
       182,
       30,
       66,
       58,
       107,
       40,
       84,
       250,
       133,
       61,
       186,
       43,
       121,
       10,
       21,
       155,
       159,
       94,
       202,
       78,
       212,
       172,
       229,
       243,
       115,
       167,
       87,
       175,
       88,
       168,
       80,
       244,
       234,
       214,
       116,
       79,
       174,
       233,
       213,
       231,
       230,
       173,
       232,
       44,
       215,
       117,
       122,
       235,
       22,
       11,
       245,
       89,
       203,
       95,
       176,
       156,
       169,
       81,
       160,
       127,
       12,
       246,
       111,
       23,
       196,
       73,
       236,
       216,
       67,
       31,
       45,
       164,
       118,
       123,
       183,
       204,
       187,
       62,
       90,
       251,
       96,
       177,
       134,
       59,
       82,
       161,
       108,
       170,
       85,
       41,
       157,
       151,
       178,
       135,
       144,
       97,
       190,
       220,
       252,
       188,
       149,
       207,
       205,
       55,
       63,
       91,
       209,
       83,
       57,
       132,
       60,
       65,
       162,
       109,
       71,
       20,
       42,
       158,
       93,
       86,
       242,
       211,
       171,
       68,
       17,
       146,
       217,
       35,
       32,
       46,
       137,
       180,
       124,
       184,
       38,
       119,
       153,
       227,
       165,
       103,
       74,
       237,
       222,
       197,
       49,
       254,
       24,
       13,
       99,
       140,
       128,
       192,
       247,
       112,
       7
        };
        private static readonly byte[] Alogtable = new byte[511]
        {
       0,
       3,
       5,
       15,
       17,
       51,
       85,
      byte.MaxValue,
       26,
       46,
       114,
       150,
       161,
       248,
       19,
       53,
       95,
       225,
       56,
       72,
       216,
       115,
       149,
       164,
       247,
       2,
       6,
       10,
       30,
       34,
       102,
       170,
       229,
       52,
       92,
       228,
       55,
       89,
       235,
       38,
       106,
       190,
       217,
       112,
       144,
       171,
       230,
       49,
       83,
       245,
       4,
       12,
       20,
       60,
       68,
       204,
       79,
       209,
       104,
       184,
       211,
       110,
       178,
       205,
       76,
       212,
       103,
       169,
       224,
       59,
       77,
       215,
       98,
       166,
       241,
       8,
       24,
       40,
       120,
       136,
       131,
       158,
       185,
       208,
       107,
       189,
       220,
       127,
       129,
       152,
       179,
       206,
       73,
       219,
       118,
       154,
       181,
       196,
       87,
       249,
       16,
       48,
       80,
       240,
       11,
       29,
       39,
       105,
       187,
       214,
       97,
       163,
       254,
       25,
       43,
       125,
       135,
       146,
       173,
       236,
       47,
       113,
       147,
       174,
       233,
       32,
       96,
       160,
       251,
       22,
       58,
       78,
       210,
       109,
       183,
       194,
       93,
       231,
       50,
       86,
       250,
       21,
       63,
       65,
       195,
       94,
       226,
       61,
       71,
       201,
       64,
       192,
       91,
       237,
       44,
       116,
       156,
       191,
       218,
       117,
       159,
       186,
       213,
       100,
       172,
       239,
       42,
       126,
       130,
       157,
       188,
       223,
       122,
       142,
       137,
       128,
       155,
       182,
       193,
       88,
       232,
       35,
       101,
       175,
       234,
       37,
       111,
       177,
       200,
       67,
       197,
       84,
       252,
       31,
       33,
       99,
       165,
       244,
       7,
       9,
       27,
       45,
       119,
       153,
       176,
       203,
       70,
       202,
       69,
       207,
       74,
       222,
       121,
       139,
       134,
       145,
       168,
       227,
       62,
       66,
       198,
       81,
       243,
       14,
       18,
       54,
       90,
       238,
       41,
       123,
       141,
       140,
       143,
       138,
       133,
       148,
       167,
       242,
       13,
       23,
       57,
       75,
       221,
       124,
       132,
       151,
       162,
       253,
       28,
       36,
       108,
       180,
       199,
       82,
       246,
       1,
       3,
       5,
       15,
       17,
       51,
       85,
      byte.MaxValue,
       26,
       46,
       114,
       150,
       161,
       248,
       19,
       53,
       95,
       225,
       56,
       72,
       216,
       115,
       149,
       164,
       247,
       2,
       6,
       10,
       30,
       34,
       102,
       170,
       229,
       52,
       92,
       228,
       55,
       89,
       235,
       38,
       106,
       190,
       217,
       112,
       144,
       171,
       230,
       49,
       83,
       245,
       4,
       12,
       20,
       60,
       68,
       204,
       79,
       209,
       104,
       184,
       211,
       110,
       178,
       205,
       76,
       212,
       103,
       169,
       224,
       59,
       77,
       215,
       98,
       166,
       241,
       8,
       24,
       40,
       120,
       136,
       131,
       158,
       185,
       208,
       107,
       189,
       220,
       127,
       129,
       152,
       179,
       206,
       73,
       219,
       118,
       154,
       181,
       196,
       87,
       249,
       16,
       48,
       80,
       240,
       11,
       29,
       39,
       105,
       187,
       214,
       97,
       163,
       254,
       25,
       43,
       125,
       135,
       146,
       173,
       236,
       47,
       113,
       147,
       174,
       233,
       32,
       96,
       160,
       251,
       22,
       58,
       78,
       210,
       109,
       183,
       194,
       93,
       231,
       50,
       86,
       250,
       21,
       63,
       65,
       195,
       94,
       226,
       61,
       71,
       201,
       64,
       192,
       91,
       237,
       44,
       116,
       156,
       191,
       218,
       117,
       159,
       186,
       213,
       100,
       172,
       239,
       42,
       126,
       130,
       157,
       188,
       223,
       122,
       142,
       137,
       128,
       155,
       182,
       193,
       88,
       232,
       35,
       101,
       175,
       234,
       37,
       111,
       177,
       200,
       67,
       197,
       84,
       252,
       31,
       33,
       99,
       165,
       244,
       7,
       9,
       27,
       45,
       119,
       153,
       176,
       203,
       70,
       202,
       69,
       207,
       74,
       222,
       121,
       139,
       134,
       145,
       168,
       227,
       62,
       66,
       198,
       81,
       243,
       14,
       18,
       54,
       90,
       238,
       41,
       123,
       141,
       140,
       143,
       138,
       133,
       148,
       167,
       242,
       13,
       23,
       57,
       75,
       221,
       124,
       132,
       151,
       162,
       253,
       28,
       36,
       108,
       180,
       199,
       82,
       246,
       1
        };
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
        private static readonly byte[][] shifts0 = new byte[5][]
        {
      new byte[4]{  0,  8,  16,  24 },
      new byte[4]{  0,  8,  16,  24 },
      new byte[4]{  0,  8,  16,  24 },
      new byte[4]{  0,  8,  16,  32 },
      new byte[4]{ 0, 8, 24, 32 }
        };
        private static readonly byte[][] shifts1 = new byte[5][]
        {
      new byte[4]{  0,  24,  16,  8 },
      new byte[4]{  0,  32,  24,  16 },
      new byte[4]{  0,  40,  32,  24 },
      new byte[4]{  0,  48,  40,  24 },
      new byte[4]{ 0, 56, 40, 32 }
        };
        private int BC;
        private long BC_MASK;
        private int ROUNDS;
        private int blockBits;
        private long[][] workingKey;
        private long A0;
        private long A1;
        private long A2;
        private long A3;
        private bool forEncryption;
        private byte[] shifts0SC;
        private byte[] shifts1SC;

        private byte Mul0x2( int b ) => b != 0 ? Alogtable[25 + (Logtable[b] & byte.MaxValue)] : (byte)0;

        private byte Mul0x3( int b ) => b != 0 ? Alogtable[1 + (Logtable[b] & byte.MaxValue)] : (byte)0;

        private byte Mul0x9( int b ) => b >= 0 ? Alogtable[199 + b] : (byte)0;

        private byte Mul0xb( int b ) => b >= 0 ? Alogtable[104 + b] : (byte)0;

        private byte Mul0xd( int b ) => b >= 0 ? Alogtable[238 + b] : (byte)0;

        private byte Mul0xe( int b ) => b >= 0 ? Alogtable[223 + b] : (byte)0;

        private void KeyAddition( long[] rk )
        {
            this.A0 ^= rk[0];
            this.A1 ^= rk[1];
            this.A2 ^= rk[2];
            this.A3 ^= rk[3];
        }

        private long Shift( long r, int shift )
        {
            ulong num = (ulong)(r >>> shift);
            if (shift > 31)
                num &= uint.MaxValue;
            return ((long)num | (r << (this.BC - shift))) & this.BC_MASK;
        }

        private void ShiftRow( byte[] shiftsSC )
        {
            this.A1 = this.Shift( this.A1, shiftsSC[1] );
            this.A2 = this.Shift( this.A2, shiftsSC[2] );
            this.A3 = this.Shift( this.A3, shiftsSC[3] );
        }

        private long ApplyS( long r, byte[] box )
        {
            long num = 0;
            for (int index = 0; index < this.BC; index += 8)
                num |= (long)(box[(int)((r >> index) & byte.MaxValue)] & byte.MaxValue) << index;
            return num;
        }

        private void Substitution( byte[] box )
        {
            this.A0 = this.ApplyS( this.A0, box );
            this.A1 = this.ApplyS( this.A1, box );
            this.A2 = this.ApplyS( this.A2, box );
            this.A3 = this.ApplyS( this.A3, box );
        }

        private void MixColumn()
        {
            long num1;
            long num2 = num1 = 0L;
            long num3 = num1;
            long num4 = num1;
            long num5 = num1;
            for (int index = 0; index < this.BC; index += 8)
            {
                int b1 = (int)((this.A0 >> index) & byte.MaxValue);
                int b2 = (int)((this.A1 >> index) & byte.MaxValue);
                int b3 = (int)((this.A2 >> index) & byte.MaxValue);
                int b4 = (int)((this.A3 >> index) & byte.MaxValue);
                num5 |= (long)((this.Mul0x2( b1 ) ^ this.Mul0x3( b2 ) ^ b3 ^ b4) & byte.MaxValue) << index;
                num4 |= (long)((this.Mul0x2( b2 ) ^ this.Mul0x3( b3 ) ^ b4 ^ b1) & byte.MaxValue) << index;
                num3 |= (long)((this.Mul0x2( b3 ) ^ this.Mul0x3( b4 ) ^ b1 ^ b2) & byte.MaxValue) << index;
                num2 |= (long)((this.Mul0x2( b4 ) ^ this.Mul0x3( b1 ) ^ b2 ^ b3) & byte.MaxValue) << index;
            }
            this.A0 = num5;
            this.A1 = num4;
            this.A2 = num3;
            this.A3 = num2;
        }

        private void InvMixColumn()
        {
            long num1;
            long num2 = num1 = 0L;
            long num3 = num1;
            long num4 = num1;
            long num5 = num1;
            for (int index = 0; index < this.BC; index += 8)
            {
                int num6 = (int)((this.A0 >> index) & byte.MaxValue);
                int num7 = (int)((this.A1 >> index) & byte.MaxValue);
                int num8 = (int)((this.A2 >> index) & byte.MaxValue);
                int num9 = (int)((this.A3 >> index) & byte.MaxValue);
                int b1 = num6 != 0 ? Logtable[num6 & byte.MaxValue] & byte.MaxValue : -1;
                int b2 = num7 != 0 ? Logtable[num7 & byte.MaxValue] & byte.MaxValue : -1;
                int b3 = num8 != 0 ? Logtable[num8 & byte.MaxValue] & byte.MaxValue : -1;
                int b4 = num9 != 0 ? Logtable[num9 & byte.MaxValue] & byte.MaxValue : -1;
                num5 |= (long)((this.Mul0xe( b1 ) ^ this.Mul0xb( b2 ) ^ this.Mul0xd( b3 ) ^ this.Mul0x9( b4 )) & byte.MaxValue) << index;
                num4 |= (long)((this.Mul0xe( b2 ) ^ this.Mul0xb( b3 ) ^ this.Mul0xd( b4 ) ^ this.Mul0x9( b1 )) & byte.MaxValue) << index;
                num3 |= (long)((this.Mul0xe( b3 ) ^ this.Mul0xb( b4 ) ^ this.Mul0xd( b1 ) ^ this.Mul0x9( b2 )) & byte.MaxValue) << index;
                num2 |= (long)((this.Mul0xe( b4 ) ^ this.Mul0xb( b1 ) ^ this.Mul0xd( b2 ) ^ this.Mul0x9( b3 )) & byte.MaxValue) << index;
            }
            this.A0 = num5;
            this.A1 = num4;
            this.A2 = num3;
            this.A3 = num2;
        }

        private long[][] GenerateWorkingKey( byte[] key )
        {
            int num1 = 0;
            int num2 = key.Length * 8;
            byte[,] numArray1 = new byte[4, MAXKC];
            long[][] workingKey = new long[MAXROUNDS + 1][];
            for (int index = 0; index < MAXROUNDS + 1; ++index)
                workingKey[index] = new long[4];
            int num3;
            switch (num2)
            {
                case 128:
                    num3 = 4;
                    break;
                case 160:
                    num3 = 5;
                    break;
                case 192:
                    num3 = 6;
                    break;
                case 224:
                    num3 = 7;
                    break;
                case 256:
                    num3 = 8;
                    break;
                default:
                    throw new ArgumentException( "Key length not 128/160/192/224/256 bits." );
            }
            this.ROUNDS = num2 < this.blockBits ? (this.BC / 8) + 6 : num3 + 6;
            int num4 = 0;
            for (int index = 0; index < key.Length; ++index)
                numArray1[index % 4, index / 4] = key[num4++];
            int num5 = 0;
            for (int index1 = 0; index1 < num3 && num5 < (this.ROUNDS + 1) * (this.BC / 8); ++num5)
            {
                for (int index2 = 0; index2 < 4; ++index2)
                    workingKey[num5 / (this.BC / 8)][index2] |= (long)(numArray1[index2, index1] & byte.MaxValue) << (num5 * 8 % this.BC);
                ++index1;
            }
        label_50:
            while (num5 < (this.ROUNDS + 1) * (this.BC / 8))
            {
                for (int index3 = 0; index3 < 4; ++index3)
                {
                    byte[,] numArray2;
                    IntPtr index4;
                    (numArray2 = numArray1)[(int)(index4 = (IntPtr)index3), 0] = (byte)(numArray2[(int)index4, 0] ^ (uint)S[numArray1[(index3 + 1) % 4, num3 - 1] & byte.MaxValue]);
                }
                byte[,] numArray3;
                (numArray3 = numArray1)[0, 0] = (byte)(numArray3[0, 0] ^ rcon[num1++]);
                if (num3 <= 6)
                {
                    for (int index5 = 1; index5 < num3; ++index5)
                    {
                        for (int index6 = 0; index6 < 4; ++index6)
                        {
                            byte[,] numArray4;
                            IntPtr index7;
                            IntPtr index8;
                            (numArray4 = numArray1)[(int)(index7 = (IntPtr)index6), (int)(index8 = (IntPtr)index5)] = (byte)(numArray4[(int)index7, (int)index8] ^ (uint)numArray1[index6, index5 - 1]);
                        }
                    }
                }
                else
                {
                    for (int index9 = 1; index9 < 4; ++index9)
                    {
                        for (int index10 = 0; index10 < 4; ++index10)
                        {
                            byte[,] numArray5;
                            IntPtr index11;
                            IntPtr index12;
                            (numArray5 = numArray1)[(int)(index11 = (IntPtr)index10), (int)(index12 = (IntPtr)index9)] = (byte)(numArray5[(int)index11, (int)index12] ^ (uint)numArray1[index10, index9 - 1]);
                        }
                    }
                    for (int index13 = 0; index13 < 4; ++index13)
                    {
                        byte[,] numArray6;
                        IntPtr index14;
                        (numArray6 = numArray1)[(int)(index14 = (IntPtr)index13), 4] = (byte)(numArray6[(int)index14, 4] ^ (uint)S[numArray1[index13, 3] & byte.MaxValue]);
                    }
                    for (int index15 = 5; index15 < num3; ++index15)
                    {
                        for (int index16 = 0; index16 < 4; ++index16)
                        {
                            byte[,] numArray7;
                            IntPtr index17;
                            IntPtr index18;
                            (numArray7 = numArray1)[(int)(index17 = (IntPtr)index16), (int)(index18 = (IntPtr)index15)] = (byte)(numArray7[(int)index17, (int)index18] ^ (uint)numArray1[index16, index15 - 1]);
                        }
                    }
                }
                int index19 = 0;
                while (true)
                {
                    if (index19 < num3 && num5 < (this.ROUNDS + 1) * (this.BC / 8))
                    {
                        for (int index20 = 0; index20 < 4; ++index20)
                            workingKey[num5 / (this.BC / 8)][index20] |= (long)(numArray1[index20, index19] & byte.MaxValue) << (num5 * 8 % this.BC);
                        ++index19;
                        ++num5;
                    }
                    else
                        goto label_50;
                }
            }
            return workingKey;
        }

        public RijndaelEngine()
          : this( 128 )
        {
        }

        public RijndaelEngine( int blockBits )
        {
            switch (blockBits)
            {
                case 128:
                    this.BC = 32;
                    this.BC_MASK = uint.MaxValue;
                    this.shifts0SC = shifts0[0];
                    this.shifts1SC = shifts1[0];
                    break;
                case 160:
                    this.BC = 40;
                    this.BC_MASK = 1099511627775L;
                    this.shifts0SC = shifts0[1];
                    this.shifts1SC = shifts1[1];
                    break;
                case 192:
                    this.BC = 48;
                    this.BC_MASK = 281474976710655L;
                    this.shifts0SC = shifts0[2];
                    this.shifts1SC = shifts1[2];
                    break;
                case 224:
                    this.BC = 56;
                    this.BC_MASK = 72057594037927935L;
                    this.shifts0SC = shifts0[3];
                    this.shifts1SC = shifts1[3];
                    break;
                case 256:
                    this.BC = 64;
                    this.BC_MASK = -1L;
                    this.shifts0SC = shifts0[4];
                    this.shifts1SC = shifts1[4];
                    break;
                default:
                    throw new ArgumentException( "unknown blocksize to Rijndael" );
            }
            this.blockBits = blockBits;
        }

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.workingKey = typeof( KeyParameter ).IsInstanceOfType( parameters ) ? this.GenerateWorkingKey( ((KeyParameter)parameters).GetKey() ) : throw new ArgumentException( "invalid parameter passed to Rijndael init - " + Platform.GetTypeName( parameters ) );
            this.forEncryption = forEncryption;
        }

        public virtual string AlgorithmName => "Rijndael";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => this.BC / 2;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.workingKey == null)
                throw new InvalidOperationException( "Rijndael engine not initialised" );
            Check.DataLength( input, inOff, this.BC / 2, "input buffer too short" );
            Check.OutputLength( output, outOff, this.BC / 2, "output buffer too short" );
            this.UnPackBlock( input, inOff );
            if (this.forEncryption)
                this.EncryptBlock( this.workingKey );
            else
                this.DecryptBlock( this.workingKey );
            this.PackBlock( output, outOff );
            return this.BC / 2;
        }

        public virtual void Reset()
        {
        }

        private void UnPackBlock( byte[] bytes, int off )
        {
            int num1 = off;
            byte[] numArray1 = bytes;
            int index1 = num1;
            int num2 = index1 + 1;
            this.A0 = numArray1[index1] & byte.MaxValue;
            byte[] numArray2 = bytes;
            int index2 = num2;
            int num3 = index2 + 1;
            this.A1 = numArray2[index2] & byte.MaxValue;
            byte[] numArray3 = bytes;
            int index3 = num3;
            int num4 = index3 + 1;
            this.A2 = numArray3[index3] & byte.MaxValue;
            byte[] numArray4 = bytes;
            int index4 = num4;
            int num5 = index4 + 1;
            this.A3 = numArray4[index4] & byte.MaxValue;
            for (int index5 = 8; index5 != this.BC; index5 += 8)
            {
                RijndaelEngine rijndaelEngine1 = this;
                long a0 = rijndaelEngine1.A0;
                byte[] numArray5 = bytes;
                int index6 = num5;
                int num6 = index6 + 1;
                long num7 = (long)(numArray5[index6] & byte.MaxValue) << index5;
                rijndaelEngine1.A0 = a0 | num7;
                RijndaelEngine rijndaelEngine2 = this;
                long a1 = rijndaelEngine2.A1;
                byte[] numArray6 = bytes;
                int index7 = num6;
                int num8 = index7 + 1;
                long num9 = (long)(numArray6[index7] & byte.MaxValue) << index5;
                rijndaelEngine2.A1 = a1 | num9;
                RijndaelEngine rijndaelEngine3 = this;
                long a2 = rijndaelEngine3.A2;
                byte[] numArray7 = bytes;
                int index8 = num8;
                int num10 = index8 + 1;
                long num11 = (long)(numArray7[index8] & byte.MaxValue) << index5;
                rijndaelEngine3.A2 = a2 | num11;
                RijndaelEngine rijndaelEngine4 = this;
                long a3 = rijndaelEngine4.A3;
                byte[] numArray8 = bytes;
                int index9 = num10;
                num5 = index9 + 1;
                long num12 = (long)(numArray8[index9] & byte.MaxValue) << index5;
                rijndaelEngine4.A3 = a3 | num12;
            }
        }

        private void PackBlock( byte[] bytes, int off )
        {
            int num1 = off;
            for (int index1 = 0; index1 != this.BC; index1 += 8)
            {
                byte[] numArray1 = bytes;
                int index2 = num1;
                int num2 = index2 + 1;
                int num3 = (byte)(this.A0 >> index1);
                numArray1[index2] = (byte)num3;
                byte[] numArray2 = bytes;
                int index3 = num2;
                int num4 = index3 + 1;
                int num5 = (byte)(this.A1 >> index1);
                numArray2[index3] = (byte)num5;
                byte[] numArray3 = bytes;
                int index4 = num4;
                int num6 = index4 + 1;
                int num7 = (byte)(this.A2 >> index1);
                numArray3[index4] = (byte)num7;
                byte[] numArray4 = bytes;
                int index5 = num6;
                num1 = index5 + 1;
                int num8 = (byte)(this.A3 >> index1);
                numArray4[index5] = (byte)num8;
            }
        }

        private void EncryptBlock( long[][] rk )
        {
            this.KeyAddition( rk[0] );
            for (int index = 1; index < this.ROUNDS; ++index)
            {
                this.Substitution( S );
                this.ShiftRow( this.shifts0SC );
                this.MixColumn();
                this.KeyAddition( rk[index] );
            }
            this.Substitution( S );
            this.ShiftRow( this.shifts0SC );
            this.KeyAddition( rk[this.ROUNDS] );
        }

        private void DecryptBlock( long[][] rk )
        {
            this.KeyAddition( rk[this.ROUNDS] );
            this.Substitution( Si );
            this.ShiftRow( this.shifts1SC );
            for (int index = this.ROUNDS - 1; index > 0; --index)
            {
                this.KeyAddition( rk[index] );
                this.InvMixColumn();
                this.Substitution( Si );
                this.ShiftRow( this.shifts1SC );
            }
            this.KeyAddition( rk[0] );
        }
    }
}
