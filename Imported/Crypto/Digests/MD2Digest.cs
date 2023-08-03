// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.MD2Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class MD2Digest : IDigest, IMemoable
    {
        private const int DigestLength = 16;
        private const int BYTE_LENGTH = 16;
        private byte[] X = new byte[48];
        private int xOff;
        private byte[] M = new byte[16];
        private int mOff;
        private byte[] C = new byte[16];
        private int COff;
        private static readonly byte[] S = new byte[256]
        {
       41,
       46,
       67,
       201,
       162,
       216,
       124,
       1,
       61,
       54,
       84,
       161,
       236,
       240,
       6,
       19,
       98,
       167,
       5,
       243,
       192,
       199,
       115,
       140,
       152,
       147,
       43,
       217,
       188,
       76,
       130,
       202,
       30,
       155,
       87,
       60,
       253,
       212,
       224,
       22,
       103,
       66,
       111,
       24,
       138,
       23,
       229,
       18,
       190,
       78,
       196,
       214,
       218,
       158,
       222,
       73,
       160,
       251,
       245,
       142,
       187,
       47,
       238,
       122,
       169,
       104,
       121,
       145,
       21,
       178,
       7,
       63,
       148,
       194,
       16,
       137,
       11,
       34,
       95,
       33,
       128,
       127,
       93,
       154,
       90,
       144,
       50,
       39,
       53,
       62,
       204,
       231,
       191,
       247,
       151,
       3,
      byte.MaxValue,
       25,
       48,
       179,
       72,
       165,
       181,
       209,
       215,
       94,
       146,
       42,
       172,
       86,
       170,
       198,
       79,
       184,
       56,
       210,
       150,
       164,
       125,
       182,
       118,
       252,
       107,
       226,
       156,
       116,
       4,
       241,
       69,
       157,
       112,
       89,
       100,
       113,
       135,
       32,
       134,
       91,
       207,
       101,
       230,
       45,
       168,
       2,
       27,
       96,
       37,
       173,
       174,
       176,
       185,
       246,
       28,
       70,
       97,
       105,
       52,
       64,
       126,
       15,
       85,
       71,
       163,
       35,
       221,
       81,
       175,
       58,
       195,
       92,
       249,
       206,
       186,
       197,
       234,
       38,
       44,
       83,
       13,
       110,
       133,
       40,
       132,
       9,
       211,
       223,
       205,
       244,
       65,
       129,
       77,
       82,
       106,
       220,
       55,
       200,
       108,
       193,
       171,
       250,
       36,
       225,
       123,
       8,
       12,
       189,
       177,
       74,
       120,
       136,
       149,
       139,
       227,
       99,
       232,
       109,
       233,
       203,
       213,
       254,
       59,
       0,
       29,
       57,
       242,
       239,
       183,
       14,
       102,
       88,
       208,
       228,
       166,
       119,
       114,
       248,
       235,
       117,
       75,
       10,
       49,
       68,
       80,
       180,
       143,
       237,
       31,
       26,
       219,
       153,
       141,
       51,
       159,
       17,
       131,
       20
        };

        public MD2Digest() => this.Reset();

        public MD2Digest( MD2Digest t ) => this.CopyIn( t );

        private void CopyIn( MD2Digest t )
        {
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
            Array.Copy( t.M, 0, M, 0, t.M.Length );
            this.mOff = t.mOff;
            Array.Copy( t.C, 0, C, 0, t.C.Length );
            this.COff = t.COff;
        }

        public string AlgorithmName => "MD2";

        public int GetDigestSize() => 16;

        public int GetByteLength() => 16;

        public int DoFinal( byte[] output, int outOff )
        {
            byte num = (byte)(this.M.Length - this.mOff);
            for (int mOff = this.mOff; mOff < this.M.Length; ++mOff)
                this.M[mOff] = num;
            this.ProcessChecksum( this.M );
            this.ProcessBlock( this.M );
            this.ProcessBlock( this.C );
            Array.Copy( X, this.xOff, output, outOff, 16 );
            this.Reset();
            return 16;
        }

        public void Reset()
        {
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
            this.mOff = 0;
            for (int index = 0; index != this.M.Length; ++index)
                this.M[index] = 0;
            this.COff = 0;
            for (int index = 0; index != this.C.Length; ++index)
                this.C[index] = 0;
        }

        public void Update( byte input )
        {
            this.M[this.mOff++] = input;
            if (this.mOff != 16)
                return;
            this.ProcessChecksum( this.M );
            this.ProcessBlock( this.M );
            this.mOff = 0;
        }

        public void BlockUpdate( byte[] input, int inOff, int length )
        {
            for (; this.mOff != 0 && length > 0; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
            while (length > 16)
            {
                Array.Copy( input, inOff, M, 0, 16 );
                this.ProcessChecksum( this.M );
                this.ProcessBlock( this.M );
                length -= 16;
                inOff += 16;
            }
            for (; length > 0; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
        }

        internal void ProcessChecksum( byte[] m )
        {
            int num = this.C[15];
            for (int index1 = 0; index1 < 16; ++index1)
            {
                byte[] c;
                IntPtr index2;
                (c = this.C)[(int)(index2 = (IntPtr)index1)] = (byte)(c[(int)index2] ^ (uint)S[(m[index1] ^ num) & byte.MaxValue]);
                num = this.C[index1];
            }
        }

        internal void ProcessBlock( byte[] m )
        {
            for (int index = 0; index < 16; ++index)
            {
                this.X[index + 16] = m[index];
                this.X[index + 32] = (byte)(m[index] ^ (uint)this.X[index]);
            }
            int index1 = 0;
            for (int index2 = 0; index2 < 18; ++index2)
            {
                for (int index3 = 0; index3 < 48; ++index3)
                {
                    byte[] x;
                    IntPtr index4;
                    index1 = ((x = this.X)[(int)(index4 = (IntPtr)index3)] = (byte)(x[(int)index4] ^ (uint)S[index1])) & byte.MaxValue;
                }
                index1 = (index1 + index2) % 256;
            }
        }

        public IMemoable Copy() => new MD2Digest( this );

        public void Reset( IMemoable other ) => this.CopyIn( (MD2Digest)other );
    }
}
