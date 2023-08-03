// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.MD5Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class MD5Digest : GeneralDigest
    {
        private const int DigestLength = 16;
        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint[] X = new uint[16];
        private int xOff;
        private static readonly int S11 = 7;
        private static readonly int S12 = 12;
        private static readonly int S13 = 17;
        private static readonly int S14 = 22;
        private static readonly int S21 = 5;
        private static readonly int S22 = 9;
        private static readonly int S23 = 14;
        private static readonly int S24 = 20;
        private static readonly int S31 = 4;
        private static readonly int S32 = 11;
        private static readonly int S33 = 16;
        private static readonly int S34 = 23;
        private static readonly int S41 = 6;
        private static readonly int S42 = 10;
        private static readonly int S43 = 15;
        private static readonly int S44 = 21;

        public MD5Digest() => this.Reset();

        public MD5Digest( MD5Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( MD5Digest t )
        {
            this.CopyIn( (GeneralDigest)t );
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
        }

        public override string AlgorithmName => "MD5";

        public override int GetDigestSize() => 16;

        internal override void ProcessWord( byte[] input, int inOff )
        {
            this.X[this.xOff] = Pack.LE_To_UInt32( input, inOff );
            if (++this.xOff != 16)
                return;
            this.ProcessBlock();
        }

        internal override void ProcessLength( long bitLength )
        {
            if (this.xOff > 14)
            {
                if (this.xOff == 15)
                    this.X[15] = 0U;
                this.ProcessBlock();
            }
            for (int xOff = this.xOff; xOff < 14; ++xOff)
                this.X[xOff] = 0U;
            this.X[14] = (uint)bitLength;
            this.X[15] = (uint)(bitLength >>> 32);
        }

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            Pack.UInt32_To_LE( this.H1, output, outOff );
            Pack.UInt32_To_LE( this.H2, output, outOff + 4 );
            Pack.UInt32_To_LE( this.H3, output, outOff + 8 );
            Pack.UInt32_To_LE( this.H4, output, outOff + 12 );
            this.Reset();
            return 16;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 1732584193U;
            this.H2 = 4023233417U;
            this.H3 = 2562383102U;
            this.H4 = 271733878U;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0U;
        }

        private static uint RotateLeft( uint x, int n ) => (x << n) | (x >> (32 - n));

        private static uint F( uint u, uint v, uint w ) => (uint)(((int)u & (int)v) | (~(int)u & (int)w));

        private static uint G( uint u, uint v, uint w ) => (uint)(((int)u & (int)w) | ((int)v & ~(int)w));

        private static uint H( uint u, uint v, uint w ) => u ^ v ^ w;

        private static uint K( uint u, uint v, uint w ) => v ^ (u | ~w);

        internal override void ProcessBlock()
        {
            uint h1 = this.H1;
            uint h2 = this.H2;
            uint h3 = this.H3;
            uint h4 = this.H4;
            uint num1 = RotateLeft( (uint)((int)h1 + (int)F( h2, h3, h4 ) + (int)this.X[0] - 680876936), S11 ) + h2;
            uint num2 = RotateLeft( (uint)((int)h4 + (int)F( num1, h2, h3 ) + (int)this.X[1] - 389564586), S12 ) + num1;
            uint num3 = RotateLeft( (uint)((int)h3 + (int)F( num2, num1, h2 ) + (int)this.X[2] + 606105819), S13 ) + num2;
            uint num4 = RotateLeft( (uint)((int)h2 + (int)F( num3, num2, num1 ) + (int)this.X[3] - 1044525330), S14 ) + num3;
            uint num5 = RotateLeft( (uint)((int)num1 + (int)F( num4, num3, num2 ) + (int)this.X[4] - 176418897), S11 ) + num4;
            uint num6 = RotateLeft( (uint)((int)num2 + (int)F( num5, num4, num3 ) + (int)this.X[5] + 1200080426), S12 ) + num5;
            uint num7 = RotateLeft( (uint)((int)num3 + (int)F( num6, num5, num4 ) + (int)this.X[6] - 1473231341), S13 ) + num6;
            uint num8 = RotateLeft( (uint)((int)num4 + (int)F( num7, num6, num5 ) + (int)this.X[7] - 45705983), S14 ) + num7;
            uint num9 = RotateLeft( (uint)((int)num5 + (int)F( num8, num7, num6 ) + (int)this.X[8] + 1770035416), S11 ) + num8;
            uint num10 = RotateLeft( (uint)((int)num6 + (int)F( num9, num8, num7 ) + (int)this.X[9] - 1958414417), S12 ) + num9;
            uint num11 = RotateLeft( (uint)((int)num7 + (int)F( num10, num9, num8 ) + (int)this.X[10] - 42063), S13 ) + num10;
            uint num12 = RotateLeft( (uint)((int)num8 + (int)F( num11, num10, num9 ) + (int)this.X[11] - 1990404162), S14 ) + num11;
            uint num13 = RotateLeft( (uint)((int)num9 + (int)F( num12, num11, num10 ) + (int)this.X[12] + 1804603682), S11 ) + num12;
            uint num14 = RotateLeft( (uint)((int)num10 + (int)F( num13, num12, num11 ) + (int)this.X[13] - 40341101), S12 ) + num13;
            uint num15 = RotateLeft( (uint)((int)num11 + (int)F( num14, num13, num12 ) + (int)this.X[14] - 1502002290), S13 ) + num14;
            uint num16 = RotateLeft( (uint)((int)num12 + (int)F( num15, num14, num13 ) + (int)this.X[15] + 1236535329), S14 ) + num15;
            uint num17 = RotateLeft( (uint)((int)num13 + (int)G( num16, num15, num14 ) + (int)this.X[1] - 165796510), S21 ) + num16;
            uint num18 = RotateLeft( (uint)((int)num14 + (int)G( num17, num16, num15 ) + (int)this.X[6] - 1069501632), S22 ) + num17;
            uint num19 = RotateLeft( (uint)((int)num15 + (int)G( num18, num17, num16 ) + (int)this.X[11] + 643717713), S23 ) + num18;
            uint num20 = RotateLeft( (uint)((int)num16 + (int)G( num19, num18, num17 ) + (int)this.X[0] - 373897302), S24 ) + num19;
            uint num21 = RotateLeft( (uint)((int)num17 + (int)G( num20, num19, num18 ) + (int)this.X[5] - 701558691), S21 ) + num20;
            uint num22 = RotateLeft( (uint)((int)num18 + (int)G( num21, num20, num19 ) + (int)this.X[10] + 38016083), S22 ) + num21;
            uint num23 = RotateLeft( (uint)((int)num19 + (int)G( num22, num21, num20 ) + (int)this.X[15] - 660478335), S23 ) + num22;
            uint num24 = RotateLeft( (uint)((int)num20 + (int)G( num23, num22, num21 ) + (int)this.X[4] - 405537848), S24 ) + num23;
            uint num25 = RotateLeft( (uint)((int)num21 + (int)G( num24, num23, num22 ) + (int)this.X[9] + 568446438), S21 ) + num24;
            uint num26 = RotateLeft( (uint)((int)num22 + (int)G( num25, num24, num23 ) + (int)this.X[14] - 1019803690), S22 ) + num25;
            uint num27 = RotateLeft( (uint)((int)num23 + (int)G( num26, num25, num24 ) + (int)this.X[3] - 187363961), S23 ) + num26;
            uint num28 = RotateLeft( (uint)((int)num24 + (int)G( num27, num26, num25 ) + (int)this.X[8] + 1163531501), S24 ) + num27;
            uint num29 = RotateLeft( (uint)((int)num25 + (int)G( num28, num27, num26 ) + (int)this.X[13] - 1444681467), S21 ) + num28;
            uint num30 = RotateLeft( (uint)((int)num26 + (int)G( num29, num28, num27 ) + (int)this.X[2] - 51403784), S22 ) + num29;
            uint num31 = RotateLeft( (uint)((int)num27 + (int)G( num30, num29, num28 ) + (int)this.X[7] + 1735328473), S23 ) + num30;
            uint num32 = RotateLeft( (uint)((int)num28 + (int)G( num31, num30, num29 ) + (int)this.X[12] - 1926607734), S24 ) + num31;
            uint num33 = RotateLeft( (uint)((int)num29 + (int)H( num32, num31, num30 ) + (int)this.X[5] - 378558), S31 ) + num32;
            uint num34 = RotateLeft( (uint)((int)num30 + (int)H( num33, num32, num31 ) + (int)this.X[8] - 2022574463), S32 ) + num33;
            uint num35 = RotateLeft( (uint)((int)num31 + (int)H( num34, num33, num32 ) + (int)this.X[11] + 1839030562), S33 ) + num34;
            uint num36 = RotateLeft( (uint)((int)num32 + (int)H( num35, num34, num33 ) + (int)this.X[14] - 35309556), S34 ) + num35;
            uint num37 = RotateLeft( (uint)((int)num33 + (int)H( num36, num35, num34 ) + (int)this.X[1] - 1530992060), S31 ) + num36;
            uint num38 = RotateLeft( (uint)((int)num34 + (int)H( num37, num36, num35 ) + (int)this.X[4] + 1272893353), S32 ) + num37;
            uint num39 = RotateLeft( (uint)((int)num35 + (int)H( num38, num37, num36 ) + (int)this.X[7] - 155497632), S33 ) + num38;
            uint num40 = RotateLeft( (uint)((int)num36 + (int)H( num39, num38, num37 ) + (int)this.X[10] - 1094730640), S34 ) + num39;
            uint num41 = RotateLeft( (uint)((int)num37 + (int)H( num40, num39, num38 ) + (int)this.X[13] + 681279174), S31 ) + num40;
            uint num42 = RotateLeft( (uint)((int)num38 + (int)H( num41, num40, num39 ) + (int)this.X[0] - 358537222), S32 ) + num41;
            uint num43 = RotateLeft( (uint)((int)num39 + (int)H( num42, num41, num40 ) + (int)this.X[3] - 722521979), S33 ) + num42;
            uint num44 = RotateLeft( (uint)((int)num40 + (int)H( num43, num42, num41 ) + (int)this.X[6] + 76029189), S34 ) + num43;
            uint num45 = RotateLeft( (uint)((int)num41 + (int)H( num44, num43, num42 ) + (int)this.X[9] - 640364487), S31 ) + num44;
            uint num46 = RotateLeft( (uint)((int)num42 + (int)H( num45, num44, num43 ) + (int)this.X[12] - 421815835), S32 ) + num45;
            uint num47 = RotateLeft( (uint)((int)num43 + (int)H( num46, num45, num44 ) + (int)this.X[15] + 530742520), S33 ) + num46;
            uint num48 = RotateLeft( (uint)((int)num44 + (int)H( num47, num46, num45 ) + (int)this.X[2] - 995338651), S34 ) + num47;
            uint num49 = RotateLeft( (uint)((int)num45 + (int)K( num48, num47, num46 ) + (int)this.X[0] - 198630844), S41 ) + num48;
            uint num50 = RotateLeft( (uint)((int)num46 + (int)K( num49, num48, num47 ) + (int)this.X[7] + 1126891415), S42 ) + num49;
            uint num51 = RotateLeft( (uint)((int)num47 + (int)K( num50, num49, num48 ) + (int)this.X[14] - 1416354905), S43 ) + num50;
            uint num52 = RotateLeft( (uint)((int)num48 + (int)K( num51, num50, num49 ) + (int)this.X[5] - 57434055), S44 ) + num51;
            uint num53 = RotateLeft( (uint)((int)num49 + (int)K( num52, num51, num50 ) + (int)this.X[12] + 1700485571), S41 ) + num52;
            uint num54 = RotateLeft( (uint)((int)num50 + (int)K( num53, num52, num51 ) + (int)this.X[3] - 1894986606), S42 ) + num53;
            uint num55 = RotateLeft( (uint)((int)num51 + (int)K( num54, num53, num52 ) + (int)this.X[10] - 1051523), S43 ) + num54;
            uint num56 = RotateLeft( (uint)((int)num52 + (int)K( num55, num54, num53 ) + (int)this.X[1] - 2054922799), S44 ) + num55;
            uint num57 = RotateLeft( (uint)((int)num53 + (int)K( num56, num55, num54 ) + (int)this.X[8] + 1873313359), S41 ) + num56;
            uint num58 = RotateLeft( (uint)((int)num54 + (int)K( num57, num56, num55 ) + (int)this.X[15] - 30611744), S42 ) + num57;
            uint num59 = RotateLeft( (uint)((int)num55 + (int)K( num58, num57, num56 ) + (int)this.X[6] - 1560198380), S43 ) + num58;
            uint num60 = RotateLeft( (uint)((int)num56 + (int)K( num59, num58, num57 ) + (int)this.X[13] + 1309151649), S44 ) + num59;
            uint num61 = RotateLeft( (uint)((int)num57 + (int)K( num60, num59, num58 ) + (int)this.X[4] - 145523070), S41 ) + num60;
            uint num62 = RotateLeft( (uint)((int)num58 + (int)K( num61, num60, num59 ) + (int)this.X[11] - 1120210379), S42 ) + num61;
            uint u = RotateLeft( (uint)((int)num59 + (int)K( num62, num61, num60 ) + (int)this.X[2] + 718787259), S43 ) + num62;
            uint num63 = RotateLeft( (uint)((int)num60 + (int)K( u, num62, num61 ) + (int)this.X[9] - 343485551), S44 ) + u;
            this.H1 += num61;
            this.H2 += num63;
            this.H3 += u;
            this.H4 += num62;
            this.xOff = 0;
        }

        public override IMemoable Copy() => new MD5Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (MD5Digest)other );
    }
}
