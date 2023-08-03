// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RC2Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RC2Engine : IBlockCipher
    {
        private const int BLOCK_SIZE = 8;
        private static readonly byte[] piTable = new byte[256]
        {
       217,
       120,
       249,
       196,
       25,
       221,
       181,
       237,
       40,
       233,
       253,
       121,
       74,
       160,
       216,
       157,
       198,
       126,
       55,
       131,
       43,
       118,
       83,
       142,
       98,
       76,
       100,
       136,
       68,
       139,
       251,
       162,
       23,
       154,
       89,
       245,
       135,
       179,
       79,
       19,
       97,
       69,
       109,
       141,
       9,
       129,
       125,
       50,
       189,
       143,
       64,
       235,
       134,
       183,
       123,
       11,
       240,
       149,
       33,
       34,
       92,
       107,
       78,
       130,
       84,
       214,
       101,
       147,
       206,
       96,
       178,
       28,
       115,
       86,
       192,
       20,
       167,
       140,
       241,
       220,
       18,
       117,
       202,
       31,
       59,
       190,
       228,
       209,
       66,
       61,
       212,
       48,
       163,
       60,
       182,
       38,
       111,
       191,
       14,
       218,
       70,
       105,
       7,
       87,
       39,
       242,
       29,
       155,
       188,
       148,
       67,
       3,
       248,
       17,
       199,
       246,
       144,
       239,
       62,
       231,
       6,
       195,
       213,
       47,
       200,
       102,
       30,
       215,
       8,
       232,
       234,
       222,
       128,
       82,
       238,
       247,
       132,
       170,
       114,
       172,
       53,
       77,
       106,
       42,
       150,
       26,
       210,
       113,
       90,
       21,
       73,
       116,
       75,
       159,
       208,
       94,
       4,
       24,
       164,
       236,
       194,
       224,
       65,
       110,
       15,
       81,
       203,
       204,
       36,
       145,
       175,
       80,
       161,
       244,
       112,
       57,
       153,
       124,
       58,
       133,
       35,
       184,
       180,
       122,
       252,
       2,
       54,
       91,
       37,
       85,
       151,
       49,
       45,
       93,
       250,
       152,
       227,
       138,
       146,
       174,
       5,
       223,
       41,
       16,
       103,
       108,
       186,
       201,
       211,
       0,
       230,
       207,
       225,
       158,
       168,
       44,
       99,
       22,
       1,
       63,
       88,
       226,
       137,
       169,
       13,
       56,
       52,
       27,
       171,
       51,
      byte.MaxValue,
       176,
       187,
       72,
       12,
       95,
       185,
       177,
       205,
       46,
       197,
       243,
       219,
       71,
       229,
       165,
       156,
       119,
       10,
       166,
       32,
       104,
       254,
       127,
       193,
       173
        };
        private int[] workingKey;
        private bool encrypting;

        private int[] GenerateWorkingKey( byte[] key, int bits )
        {
            int[] numArray = new int[128];
            for (int index = 0; index != key.Length; ++index)
                numArray[index] = key[index] & byte.MaxValue;
            int length = key.Length;
            if (length < 128)
            {
                int num1 = 0;
                int num2 = numArray[length - 1];
                do
                {
                    num2 = piTable[(num2 + numArray[num1++]) & byte.MaxValue] & byte.MaxValue;
                    numArray[length++] = num2;
                }
                while (length < 128);
            }
            int num3 = (bits + 7) >> 3;
            int num4 = piTable[numArray[128 - num3] & (byte.MaxValue >> (7 & -bits))] & byte.MaxValue;
            numArray[128 - num3] = num4;
            for (int index = 128 - num3 - 1; index >= 0; --index)
            {
                num4 = piTable[num4 ^ numArray[index + num3]] & byte.MaxValue;
                numArray[index] = num4;
            }
            int[] workingKey = new int[64];
            for (int index = 0; index != workingKey.Length; ++index)
                workingKey[index] = numArray[2 * index] + (numArray[(2 * index) + 1] << 8);
            return workingKey;
        }

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.encrypting = forEncryption;
            switch (parameters)
            {
                case RC2Parameters _:
                    RC2Parameters rc2Parameters = (RC2Parameters)parameters;
                    this.workingKey = this.GenerateWorkingKey( rc2Parameters.GetKey(), rc2Parameters.EffectiveKeyBits );
                    break;
                case KeyParameter _:
                    byte[] key = ((KeyParameter)parameters).GetKey();
                    this.workingKey = this.GenerateWorkingKey( key, key.Length * 8 );
                    break;
                default:
                    throw new ArgumentException( "invalid parameter passed to RC2 init - " + Platform.GetTypeName( parameters ) );
            }
        }

        public virtual void Reset()
        {
        }

        public virtual string AlgorithmName => "RC2";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 8;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.workingKey == null)
                throw new InvalidOperationException( "RC2 engine not initialised" );
            Check.DataLength( input, inOff, 8, "input buffer too short" );
            Check.OutputLength( output, outOff, 8, "output buffer too short" );
            if (this.encrypting)
                this.EncryptBlock( input, inOff, output, outOff );
            else
                this.DecryptBlock( input, inOff, output, outOff );
            return 8;
        }

        private int RotateWordLeft( int x, int y )
        {
            x &= ushort.MaxValue;
            return (x << y) | (x >> (16 - y));
        }

        private void EncryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            int num1 = ((input[inOff + 7] & byte.MaxValue) << 8) + (input[inOff + 6] & byte.MaxValue);
            int num2 = ((input[inOff + 5] & byte.MaxValue) << 8) + (input[inOff + 4] & byte.MaxValue);
            int num3 = ((input[inOff + 3] & byte.MaxValue) << 8) + (input[inOff + 2] & byte.MaxValue);
            int num4 = ((input[inOff + 1] & byte.MaxValue) << 8) + (input[inOff] & byte.MaxValue);
            for (int index = 0; index <= 16; index += 4)
            {
                num4 = this.RotateWordLeft( num4 + (num3 & ~num1) + (num2 & num1) + this.workingKey[index], 1 );
                num3 = this.RotateWordLeft( num3 + (num2 & ~num4) + (num1 & num4) + this.workingKey[index + 1], 2 );
                num2 = this.RotateWordLeft( num2 + (num1 & ~num3) + (num4 & num3) + this.workingKey[index + 2], 3 );
                num1 = this.RotateWordLeft( num1 + (num4 & ~num2) + (num3 & num2) + this.workingKey[index + 3], 5 );
            }
            int num5 = num4 + this.workingKey[num1 & 63];
            int num6 = num3 + this.workingKey[num5 & 63];
            int num7 = num2 + this.workingKey[num6 & 63];
            int num8 = num1 + this.workingKey[num7 & 63];
            for (int index = 20; index <= 40; index += 4)
            {
                num5 = this.RotateWordLeft( num5 + (num6 & ~num8) + (num7 & num8) + this.workingKey[index], 1 );
                num6 = this.RotateWordLeft( num6 + (num7 & ~num5) + (num8 & num5) + this.workingKey[index + 1], 2 );
                num7 = this.RotateWordLeft( num7 + (num8 & ~num6) + (num5 & num6) + this.workingKey[index + 2], 3 );
                num8 = this.RotateWordLeft( num8 + (num5 & ~num7) + (num6 & num7) + this.workingKey[index + 3], 5 );
            }
            int num9 = num5 + this.workingKey[num8 & 63];
            int num10 = num6 + this.workingKey[num9 & 63];
            int num11 = num7 + this.workingKey[num10 & 63];
            int num12 = num8 + this.workingKey[num11 & 63];
            for (int index = 44; index < 64; index += 4)
            {
                num9 = this.RotateWordLeft( num9 + (num10 & ~num12) + (num11 & num12) + this.workingKey[index], 1 );
                num10 = this.RotateWordLeft( num10 + (num11 & ~num9) + (num12 & num9) + this.workingKey[index + 1], 2 );
                num11 = this.RotateWordLeft( num11 + (num12 & ~num10) + (num9 & num10) + this.workingKey[index + 2], 3 );
                num12 = this.RotateWordLeft( num12 + (num9 & ~num11) + (num10 & num11) + this.workingKey[index + 3], 5 );
            }
            outBytes[outOff] = (byte)num9;
            outBytes[outOff + 1] = (byte)(num9 >> 8);
            outBytes[outOff + 2] = (byte)num10;
            outBytes[outOff + 3] = (byte)(num10 >> 8);
            outBytes[outOff + 4] = (byte)num11;
            outBytes[outOff + 5] = (byte)(num11 >> 8);
            outBytes[outOff + 6] = (byte)num12;
            outBytes[outOff + 7] = (byte)(num12 >> 8);
        }

        private void DecryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            int x1 = ((input[inOff + 7] & byte.MaxValue) << 8) + (input[inOff + 6] & byte.MaxValue);
            int x2 = ((input[inOff + 5] & byte.MaxValue) << 8) + (input[inOff + 4] & byte.MaxValue);
            int x3 = ((input[inOff + 3] & byte.MaxValue) << 8) + (input[inOff + 2] & byte.MaxValue);
            int x4 = ((input[inOff + 1] & byte.MaxValue) << 8) + (input[inOff] & byte.MaxValue);
            for (int index = 60; index >= 44; index -= 4)
            {
                x1 = this.RotateWordLeft( x1, 11 ) - ((x4 & ~x2) + (x3 & x2) + this.workingKey[index + 3]);
                x2 = this.RotateWordLeft( x2, 13 ) - ((x1 & ~x3) + (x4 & x3) + this.workingKey[index + 2]);
                x3 = this.RotateWordLeft( x3, 14 ) - ((x2 & ~x4) + (x1 & x4) + this.workingKey[index + 1]);
                x4 = this.RotateWordLeft( x4, 15 ) - ((x3 & ~x1) + (x2 & x1) + this.workingKey[index]);
            }
            int x5 = x1 - this.workingKey[x2 & 63];
            int x6 = x2 - this.workingKey[x3 & 63];
            int x7 = x3 - this.workingKey[x4 & 63];
            int x8 = x4 - this.workingKey[x5 & 63];
            for (int index = 40; index >= 20; index -= 4)
            {
                x5 = this.RotateWordLeft( x5, 11 ) - ((x8 & ~x6) + (x7 & x6) + this.workingKey[index + 3]);
                x6 = this.RotateWordLeft( x6, 13 ) - ((x5 & ~x7) + (x8 & x7) + this.workingKey[index + 2]);
                x7 = this.RotateWordLeft( x7, 14 ) - ((x6 & ~x8) + (x5 & x8) + this.workingKey[index + 1]);
                x8 = this.RotateWordLeft( x8, 15 ) - ((x7 & ~x5) + (x6 & x5) + this.workingKey[index]);
            }
            int x9 = x5 - this.workingKey[x6 & 63];
            int x10 = x6 - this.workingKey[x7 & 63];
            int x11 = x7 - this.workingKey[x8 & 63];
            int x12 = x8 - this.workingKey[x9 & 63];
            for (int index = 16; index >= 0; index -= 4)
            {
                x9 = this.RotateWordLeft( x9, 11 ) - ((x12 & ~x10) + (x11 & x10) + this.workingKey[index + 3]);
                x10 = this.RotateWordLeft( x10, 13 ) - ((x9 & ~x11) + (x12 & x11) + this.workingKey[index + 2]);
                x11 = this.RotateWordLeft( x11, 14 ) - ((x10 & ~x12) + (x9 & x12) + this.workingKey[index + 1]);
                x12 = this.RotateWordLeft( x12, 15 ) - ((x11 & ~x9) + (x10 & x9) + this.workingKey[index]);
            }
            outBytes[outOff] = (byte)x12;
            outBytes[outOff + 1] = (byte)(x12 >> 8);
            outBytes[outOff + 2] = (byte)x11;
            outBytes[outOff + 3] = (byte)(x11 >> 8);
            outBytes[outOff + 4] = (byte)x10;
            outBytes[outOff + 5] = (byte)(x10 >> 8);
            outBytes[outOff + 6] = (byte)x9;
            outBytes[outOff + 7] = (byte)(x9 >> 8);
        }
    }
}
