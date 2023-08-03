// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Sha1Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Sha1Digest : GeneralDigest
    {
        private const int DigestLength = 20;
        private const uint Y1 = 1518500249;
        private const uint Y2 = 1859775393;
        private const uint Y3 = 2400959708;
        private const uint Y4 = 3395469782;
        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint H5;
        private uint[] X = new uint[80];
        private int xOff;

        public Sha1Digest() => this.Reset();

        public Sha1Digest( Sha1Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( Sha1Digest t )
        {
            this.CopyIn( (GeneralDigest)t );
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
        }

        public override string AlgorithmName => "SHA-1";

        public override int GetDigestSize() => 20;

        internal override void ProcessWord( byte[] input, int inOff )
        {
            this.X[this.xOff] = Pack.BE_To_UInt32( input, inOff );
            if (++this.xOff != 16)
                return;
            this.ProcessBlock();
        }

        internal override void ProcessLength( long bitLength )
        {
            if (this.xOff > 14)
                this.ProcessBlock();
            this.X[14] = (uint)(bitLength >>> 32);
            this.X[15] = (uint)bitLength;
        }

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            Pack.UInt32_To_BE( this.H1, output, outOff );
            Pack.UInt32_To_BE( this.H2, output, outOff + 4 );
            Pack.UInt32_To_BE( this.H3, output, outOff + 8 );
            Pack.UInt32_To_BE( this.H4, output, outOff + 12 );
            Pack.UInt32_To_BE( this.H5, output, outOff + 16 );
            this.Reset();
            return 20;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 1732584193U;
            this.H2 = 4023233417U;
            this.H3 = 2562383102U;
            this.H4 = 271733878U;
            this.H5 = 3285377520U;
            this.xOff = 0;
            Array.Clear( X, 0, this.X.Length );
        }

        private static uint F( uint u, uint v, uint w ) => (uint)(((int)u & (int)v) | (~(int)u & (int)w));

        private static uint H( uint u, uint v, uint w ) => u ^ v ^ w;

        private static uint G( uint u, uint v, uint w ) => (uint)(((int)u & (int)v) | ((int)u & (int)w) | ((int)v & (int)w));

        internal override void ProcessBlock()
        {
            for (int index = 16; index < 80; ++index)
            {
                uint num = this.X[index - 3] ^ this.X[index - 8] ^ this.X[index - 14] ^ this.X[index - 16];
                this.X[index] = (num << 1) | (num >> 31);
            }
            uint u1 = this.H1;
            uint u2 = this.H2;
            uint num1 = this.H3;
            uint num2 = this.H4;
            uint num3 = this.H5;
            int num4 = 0;
            for (int index1 = 0; index1 < 4; ++index1)
            {
                int num5 = (int)num3;
                int num6 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)F( u2, num1, num2 );
                uint[] x1 = this.X;
                int index2 = num4;
                int num7 = index2 + 1;
                int num8 = (int)x1[index2];
                int num9 = num6 + num8 + 1518500249;
                uint u3 = (uint)(num5 + num9);
                uint num10 = (u2 << 30) | (u2 >> 2);
                int num11 = (int)num2;
                int num12 = (((int)u3 << 5) | (int)(u3 >> 27)) + (int)F( u1, num10, num1 );
                uint[] x2 = this.X;
                int index3 = num7;
                int num13 = index3 + 1;
                int num14 = (int)x2[index3];
                int num15 = num12 + num14 + 1518500249;
                uint u4 = (uint)(num11 + num15);
                uint num16 = (u1 << 30) | (u1 >> 2);
                int num17 = (int)num1;
                int num18 = (((int)u4 << 5) | (int)(u4 >> 27)) + (int)F( u3, num16, num10 );
                uint[] x3 = this.X;
                int index4 = num13;
                int num19 = index4 + 1;
                int num20 = (int)x3[index4];
                int num21 = num18 + num20 + 1518500249;
                uint u5 = (uint)(num17 + num21);
                num3 = (u3 << 30) | (u3 >> 2);
                int num22 = (int)num10;
                int num23 = (((int)u5 << 5) | (int)(u5 >> 27)) + (int)F( u4, num3, num16 );
                uint[] x4 = this.X;
                int index5 = num19;
                int num24 = index5 + 1;
                int num25 = (int)x4[index5];
                int num26 = num23 + num25 + 1518500249;
                u2 = (uint)(num22 + num26);
                num2 = (u4 << 30) | (u4 >> 2);
                int num27 = (int)num16;
                int num28 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)F( u5, num2, num3 );
                uint[] x5 = this.X;
                int index6 = num24;
                num4 = index6 + 1;
                int num29 = (int)x5[index6];
                int num30 = num28 + num29 + 1518500249;
                u1 = (uint)(num27 + num30);
                num1 = (u5 << 30) | (u5 >> 2);
            }
            for (int index7 = 0; index7 < 4; ++index7)
            {
                int num31 = (int)num3;
                int num32 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)H( u2, num1, num2 );
                uint[] x6 = this.X;
                int index8 = num4;
                int num33 = index8 + 1;
                int num34 = (int)x6[index8];
                int num35 = num32 + num34 + 1859775393;
                uint u6 = (uint)(num31 + num35);
                uint num36 = (u2 << 30) | (u2 >> 2);
                int num37 = (int)num2;
                int num38 = (((int)u6 << 5) | (int)(u6 >> 27)) + (int)H( u1, num36, num1 );
                uint[] x7 = this.X;
                int index9 = num33;
                int num39 = index9 + 1;
                int num40 = (int)x7[index9];
                int num41 = num38 + num40 + 1859775393;
                uint u7 = (uint)(num37 + num41);
                uint num42 = (u1 << 30) | (u1 >> 2);
                int num43 = (int)num1;
                int num44 = (((int)u7 << 5) | (int)(u7 >> 27)) + (int)H( u6, num42, num36 );
                uint[] x8 = this.X;
                int index10 = num39;
                int num45 = index10 + 1;
                int num46 = (int)x8[index10];
                int num47 = num44 + num46 + 1859775393;
                uint u8 = (uint)(num43 + num47);
                num3 = (u6 << 30) | (u6 >> 2);
                int num48 = (int)num36;
                int num49 = (((int)u8 << 5) | (int)(u8 >> 27)) + (int)H( u7, num3, num42 );
                uint[] x9 = this.X;
                int index11 = num45;
                int num50 = index11 + 1;
                int num51 = (int)x9[index11];
                int num52 = num49 + num51 + 1859775393;
                u2 = (uint)(num48 + num52);
                num2 = (u7 << 30) | (u7 >> 2);
                int num53 = (int)num42;
                int num54 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)H( u8, num2, num3 );
                uint[] x10 = this.X;
                int index12 = num50;
                num4 = index12 + 1;
                int num55 = (int)x10[index12];
                int num56 = num54 + num55 + 1859775393;
                u1 = (uint)(num53 + num56);
                num1 = (u8 << 30) | (u8 >> 2);
            }
            for (int index13 = 0; index13 < 4; ++index13)
            {
                int num57 = (int)num3;
                int num58 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)G( u2, num1, num2 );
                uint[] x11 = this.X;
                int index14 = num4;
                int num59 = index14 + 1;
                int num60 = (int)x11[index14];
                int num61 = num58 + num60 - 1894007588;
                uint u9 = (uint)(num57 + num61);
                uint num62 = (u2 << 30) | (u2 >> 2);
                int num63 = (int)num2;
                int num64 = (((int)u9 << 5) | (int)(u9 >> 27)) + (int)G( u1, num62, num1 );
                uint[] x12 = this.X;
                int index15 = num59;
                int num65 = index15 + 1;
                int num66 = (int)x12[index15];
                int num67 = num64 + num66 - 1894007588;
                uint u10 = (uint)(num63 + num67);
                uint num68 = (u1 << 30) | (u1 >> 2);
                int num69 = (int)num1;
                int num70 = (((int)u10 << 5) | (int)(u10 >> 27)) + (int)G( u9, num68, num62 );
                uint[] x13 = this.X;
                int index16 = num65;
                int num71 = index16 + 1;
                int num72 = (int)x13[index16];
                int num73 = num70 + num72 - 1894007588;
                uint u11 = (uint)(num69 + num73);
                num3 = (u9 << 30) | (u9 >> 2);
                int num74 = (int)num62;
                int num75 = (((int)u11 << 5) | (int)(u11 >> 27)) + (int)G( u10, num3, num68 );
                uint[] x14 = this.X;
                int index17 = num71;
                int num76 = index17 + 1;
                int num77 = (int)x14[index17];
                int num78 = num75 + num77 - 1894007588;
                u2 = (uint)(num74 + num78);
                num2 = (u10 << 30) | (u10 >> 2);
                int num79 = (int)num68;
                int num80 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)G( u11, num2, num3 );
                uint[] x15 = this.X;
                int index18 = num76;
                num4 = index18 + 1;
                int num81 = (int)x15[index18];
                int num82 = num80 + num81 - 1894007588;
                u1 = (uint)(num79 + num82);
                num1 = (u11 << 30) | (u11 >> 2);
            }
            for (int index19 = 0; index19 < 4; ++index19)
            {
                int num83 = (int)num3;
                int num84 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)H( u2, num1, num2 );
                uint[] x16 = this.X;
                int index20 = num4;
                int num85 = index20 + 1;
                int num86 = (int)x16[index20];
                int num87 = num84 + num86 - 899497514;
                uint u12 = (uint)(num83 + num87);
                uint num88 = (u2 << 30) | (u2 >> 2);
                int num89 = (int)num2;
                int num90 = (((int)u12 << 5) | (int)(u12 >> 27)) + (int)H( u1, num88, num1 );
                uint[] x17 = this.X;
                int index21 = num85;
                int num91 = index21 + 1;
                int num92 = (int)x17[index21];
                int num93 = num90 + num92 - 899497514;
                uint u13 = (uint)(num89 + num93);
                uint num94 = (u1 << 30) | (u1 >> 2);
                int num95 = (int)num1;
                int num96 = (((int)u13 << 5) | (int)(u13 >> 27)) + (int)H( u12, num94, num88 );
                uint[] x18 = this.X;
                int index22 = num91;
                int num97 = index22 + 1;
                int num98 = (int)x18[index22];
                int num99 = num96 + num98 - 899497514;
                uint u14 = (uint)(num95 + num99);
                num3 = (u12 << 30) | (u12 >> 2);
                int num100 = (int)num88;
                int num101 = (((int)u14 << 5) | (int)(u14 >> 27)) + (int)H( u13, num3, num94 );
                uint[] x19 = this.X;
                int index23 = num97;
                int num102 = index23 + 1;
                int num103 = (int)x19[index23];
                int num104 = num101 + num103 - 899497514;
                u2 = (uint)(num100 + num104);
                num2 = (u13 << 30) | (u13 >> 2);
                int num105 = (int)num94;
                int num106 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)H( u14, num2, num3 );
                uint[] x20 = this.X;
                int index24 = num102;
                num4 = index24 + 1;
                int num107 = (int)x20[index24];
                int num108 = num106 + num107 - 899497514;
                u1 = (uint)(num105 + num108);
                num1 = (u14 << 30) | (u14 >> 2);
            }
            this.H1 += u1;
            this.H2 += u2;
            this.H3 += num1;
            this.H4 += num2;
            this.H5 += num3;
            this.xOff = 0;
            Array.Clear( X, 0, 16 );
        }

        public override IMemoable Copy() => new Sha1Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (Sha1Digest)other );
    }
}
