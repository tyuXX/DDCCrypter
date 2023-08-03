// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.RipeMD256Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class RipeMD256Digest : GeneralDigest
    {
        private const int DigestLength = 32;
        private int H0;
        private int H1;
        private int H2;
        private int H3;
        private int H4;
        private int H5;
        private int H6;
        private int H7;
        private int[] X = new int[16];
        private int xOff;

        public override string AlgorithmName => "RIPEMD256";

        public override int GetDigestSize() => 32;

        public RipeMD256Digest() => this.Reset();

        public RipeMD256Digest( RipeMD256Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( RipeMD256Digest t )
        {
            this.CopyIn( (GeneralDigest)t );
            this.H0 = t.H0;
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            this.H6 = t.H6;
            this.H7 = t.H7;
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
        }

        internal override void ProcessWord( byte[] input, int inOff )
        {
            this.X[this.xOff++] = (input[inOff] & byte.MaxValue) | ((input[inOff + 1] & byte.MaxValue) << 8) | ((input[inOff + 2] & byte.MaxValue) << 16) | ((input[inOff + 3] & byte.MaxValue) << 24);
            if (this.xOff != 16)
                return;
            this.ProcessBlock();
        }

        internal override void ProcessLength( long bitLength )
        {
            if (this.xOff > 14)
                this.ProcessBlock();
            this.X[14] = (int)(bitLength & uint.MaxValue);
            this.X[15] = (int)(bitLength >>> 32);
        }

        private void UnpackWord( int word, byte[] outBytes, int outOff )
        {
            outBytes[outOff] = (byte)word;
            outBytes[outOff + 1] = (byte)(word >>> 8);
            outBytes[outOff + 2] = (byte)(word >>> 16);
            outBytes[outOff + 3] = (byte)(word >>> 24);
        }

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            this.UnpackWord( this.H0, output, outOff );
            this.UnpackWord( this.H1, output, outOff + 4 );
            this.UnpackWord( this.H2, output, outOff + 8 );
            this.UnpackWord( this.H3, output, outOff + 12 );
            this.UnpackWord( this.H4, output, outOff + 16 );
            this.UnpackWord( this.H5, output, outOff + 20 );
            this.UnpackWord( this.H6, output, outOff + 24 );
            this.UnpackWord( this.H7, output, outOff + 28 );
            this.Reset();
            return 32;
        }

        public override void Reset()
        {
            base.Reset();
            this.H0 = 1732584193;
            this.H1 = -271733879;
            this.H2 = -1732584194;
            this.H3 = 271733878;
            this.H4 = 1985229328;
            this.H5 = -19088744;
            this.H6 = -1985229329;
            this.H7 = 19088743;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        private int RL( int x, int n ) => (x << n) | x >>> 32 - n;

        private int F1( int x, int y, int z ) => x ^ y ^ z;

        private int F2( int x, int y, int z ) => (x & y) | (~x & z);

        private int F3( int x, int y, int z ) => (x | ~y) ^ z;

        private int F4( int x, int y, int z ) => (x & z) | (y & ~z);

        private int F1( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F1( b, c, d ) + x, s );

        private int F2( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F2( b, c, d ) + x + 1518500249, s );

        private int F3( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F3( b, c, d ) + x + 1859775393, s );

        private int F4( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F4( b, c, d ) + x - 1894007588, s );

        private int FF1( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F1( b, c, d ) + x, s );

        private int FF2( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F2( b, c, d ) + x + 1836072691, s );

        private int FF3( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F3( b, c, d ) + x + 1548603684, s );

        private int FF4( int a, int b, int c, int d, int x, int s ) => this.RL( a + this.F4( b, c, d ) + x + 1352829926, s );

        internal override void ProcessBlock()
        {
            int h0 = this.H0;
            int h1 = this.H1;
            int h2 = this.H2;
            int h3 = this.H3;
            int h4 = this.H4;
            int h5 = this.H5;
            int h6 = this.H6;
            int h7 = this.H7;
            int num1 = this.F1( h0, h1, h2, h3, this.X[0], 11 );
            int num2 = this.F1( h3, num1, h1, h2, this.X[1], 14 );
            int num3 = this.F1( h2, num2, num1, h1, this.X[2], 15 );
            int num4 = this.F1( h1, num3, num2, num1, this.X[3], 12 );
            int num5 = this.F1( num1, num4, num3, num2, this.X[4], 5 );
            int num6 = this.F1( num2, num5, num4, num3, this.X[5], 8 );
            int num7 = this.F1( num3, num6, num5, num4, this.X[6], 7 );
            int num8 = this.F1( num4, num7, num6, num5, this.X[7], 9 );
            int num9 = this.F1( num5, num8, num7, num6, this.X[8], 11 );
            int num10 = this.F1( num6, num9, num8, num7, this.X[9], 13 );
            int num11 = this.F1( num7, num10, num9, num8, this.X[10], 14 );
            int num12 = this.F1( num8, num11, num10, num9, this.X[11], 15 );
            int num13 = this.F1( num9, num12, num11, num10, this.X[12], 6 );
            int num14 = this.F1( num10, num13, num12, num11, this.X[13], 7 );
            int num15 = this.F1( num11, num14, num13, num12, this.X[14], 9 );
            int num16 = this.F1( num12, num15, num14, num13, this.X[15], 8 );
            int num17 = this.FF4( h4, h5, h6, h7, this.X[5], 8 );
            int num18 = this.FF4( h7, num17, h5, h6, this.X[14], 9 );
            int num19 = this.FF4( h6, num18, num17, h5, this.X[7], 9 );
            int num20 = this.FF4( h5, num19, num18, num17, this.X[0], 11 );
            int num21 = this.FF4( num17, num20, num19, num18, this.X[9], 13 );
            int num22 = this.FF4( num18, num21, num20, num19, this.X[2], 15 );
            int num23 = this.FF4( num19, num22, num21, num20, this.X[11], 15 );
            int num24 = this.FF4( num20, num23, num22, num21, this.X[4], 5 );
            int num25 = this.FF4( num21, num24, num23, num22, this.X[13], 7 );
            int num26 = this.FF4( num22, num25, num24, num23, this.X[6], 7 );
            int num27 = this.FF4( num23, num26, num25, num24, this.X[15], 8 );
            int num28 = this.FF4( num24, num27, num26, num25, this.X[8], 11 );
            int num29 = this.FF4( num25, num28, num27, num26, this.X[1], 14 );
            int num30 = this.FF4( num26, num29, num28, num27, this.X[10], 14 );
            int num31 = this.FF4( num27, num30, num29, num28, this.X[3], 12 );
            int num32 = this.FF4( num28, num31, num30, num29, this.X[12], 6 );
            int num33 = num13;
            int a1 = num29;
            int a2 = num33;
            int num34 = this.F2( a1, num16, num15, num14, this.X[7], 7 );
            int num35 = this.F2( num14, num34, num16, num15, this.X[4], 6 );
            int num36 = this.F2( num15, num35, num34, num16, this.X[13], 8 );
            int num37 = this.F2( num16, num36, num35, num34, this.X[1], 13 );
            int num38 = this.F2( num34, num37, num36, num35, this.X[10], 11 );
            int num39 = this.F2( num35, num38, num37, num36, this.X[6], 9 );
            int num40 = this.F2( num36, num39, num38, num37, this.X[15], 7 );
            int num41 = this.F2( num37, num40, num39, num38, this.X[3], 15 );
            int num42 = this.F2( num38, num41, num40, num39, this.X[12], 7 );
            int num43 = this.F2( num39, num42, num41, num40, this.X[0], 12 );
            int num44 = this.F2( num40, num43, num42, num41, this.X[9], 15 );
            int num45 = this.F2( num41, num44, num43, num42, this.X[5], 9 );
            int num46 = this.F2( num42, num45, num44, num43, this.X[2], 11 );
            int num47 = this.F2( num43, num46, num45, num44, this.X[14], 7 );
            int num48 = this.F2( num44, num47, num46, num45, this.X[11], 13 );
            int num49 = this.F2( num45, num48, num47, num46, this.X[8], 12 );
            int num50 = this.FF3( a2, num32, num31, num30, this.X[6], 9 );
            int num51 = this.FF3( num30, num50, num32, num31, this.X[11], 13 );
            int num52 = this.FF3( num31, num51, num50, num32, this.X[3], 15 );
            int num53 = this.FF3( num32, num52, num51, num50, this.X[7], 7 );
            int num54 = this.FF3( num50, num53, num52, num51, this.X[0], 12 );
            int num55 = this.FF3( num51, num54, num53, num52, this.X[13], 8 );
            int num56 = this.FF3( num52, num55, num54, num53, this.X[5], 9 );
            int num57 = this.FF3( num53, num56, num55, num54, this.X[10], 11 );
            int num58 = this.FF3( num54, num57, num56, num55, this.X[14], 7 );
            int num59 = this.FF3( num55, num58, num57, num56, this.X[15], 7 );
            int num60 = this.FF3( num56, num59, num58, num57, this.X[8], 12 );
            int num61 = this.FF3( num57, num60, num59, num58, this.X[12], 7 );
            int num62 = this.FF3( num58, num61, num60, num59, this.X[4], 6 );
            int num63 = this.FF3( num59, num62, num61, num60, this.X[9], 15 );
            int num64 = this.FF3( num60, num63, num62, num61, this.X[1], 13 );
            int num65 = this.FF3( num61, num64, num63, num62, this.X[2], 11 );
            int num66 = num49;
            int num67 = num65;
            int num68 = num66;
            int num69 = this.F3( num46, num67, num48, num47, this.X[3], 11 );
            int num70 = this.F3( num47, num69, num67, num48, this.X[10], 13 );
            int num71 = this.F3( num48, num70, num69, num67, this.X[14], 6 );
            int num72 = this.F3( num67, num71, num70, num69, this.X[4], 7 );
            int num73 = this.F3( num69, num72, num71, num70, this.X[9], 14 );
            int num74 = this.F3( num70, num73, num72, num71, this.X[15], 9 );
            int num75 = this.F3( num71, num74, num73, num72, this.X[8], 13 );
            int num76 = this.F3( num72, num75, num74, num73, this.X[1], 15 );
            int num77 = this.F3( num73, num76, num75, num74, this.X[2], 14 );
            int num78 = this.F3( num74, num77, num76, num75, this.X[7], 8 );
            int num79 = this.F3( num75, num78, num77, num76, this.X[0], 13 );
            int num80 = this.F3( num76, num79, num78, num77, this.X[6], 6 );
            int num81 = this.F3( num77, num80, num79, num78, this.X[13], 5 );
            int num82 = this.F3( num78, num81, num80, num79, this.X[11], 12 );
            int b1 = this.F3( num79, num82, num81, num80, this.X[5], 7 );
            int num83 = this.F3( num80, b1, num82, num81, this.X[12], 5 );
            int num84 = this.FF2( num62, num68, num64, num63, this.X[15], 9 );
            int num85 = this.FF2( num63, num84, num68, num64, this.X[5], 7 );
            int num86 = this.FF2( num64, num85, num84, num68, this.X[1], 15 );
            int num87 = this.FF2( num68, num86, num85, num84, this.X[3], 11 );
            int num88 = this.FF2( num84, num87, num86, num85, this.X[7], 8 );
            int num89 = this.FF2( num85, num88, num87, num86, this.X[14], 6 );
            int num90 = this.FF2( num86, num89, num88, num87, this.X[6], 6 );
            int num91 = this.FF2( num87, num90, num89, num88, this.X[9], 14 );
            int num92 = this.FF2( num88, num91, num90, num89, this.X[11], 12 );
            int num93 = this.FF2( num89, num92, num91, num90, this.X[8], 13 );
            int num94 = this.FF2( num90, num93, num92, num91, this.X[12], 5 );
            int num95 = this.FF2( num91, num94, num93, num92, this.X[2], 14 );
            int num96 = this.FF2( num92, num95, num94, num93, this.X[10], 13 );
            int num97 = this.FF2( num93, num96, num95, num94, this.X[0], 13 );
            int b2 = this.FF2( num94, num97, num96, num95, this.X[4], 7 );
            int num98 = this.FF2( num95, b2, num97, num96, this.X[13], 5 );
            int num99 = b1;
            int num100 = b2;
            int num101 = num99;
            int num102 = this.F4( num81, num83, num100, num82, this.X[1], 11 );
            int num103 = this.F4( num82, num102, num83, num100, this.X[9], 12 );
            int num104 = this.F4( num100, num103, num102, num83, this.X[11], 14 );
            int num105 = this.F4( num83, num104, num103, num102, this.X[10], 15 );
            int num106 = this.F4( num102, num105, num104, num103, this.X[0], 14 );
            int num107 = this.F4( num103, num106, num105, num104, this.X[8], 15 );
            int num108 = this.F4( num104, num107, num106, num105, this.X[12], 9 );
            int num109 = this.F4( num105, num108, num107, num106, this.X[4], 8 );
            int num110 = this.F4( num106, num109, num108, num107, this.X[13], 9 );
            int num111 = this.F4( num107, num110, num109, num108, this.X[3], 14 );
            int num112 = this.F4( num108, num111, num110, num109, this.X[7], 5 );
            int num113 = this.F4( num109, num112, num111, num110, this.X[15], 6 );
            int num114 = this.F4( num110, num113, num112, num111, this.X[14], 8 );
            int num115 = this.F4( num111, num114, num113, num112, this.X[5], 6 );
            int b3 = this.F4( num112, num115, num114, num113, this.X[6], 5 );
            int num116 = this.F4( num113, b3, num115, num114, this.X[2], 12 );
            int num117 = this.FF1( num96, num98, num101, num97, this.X[8], 15 );
            int num118 = this.FF1( num97, num117, num98, num101, this.X[6], 5 );
            int num119 = this.FF1( num101, num118, num117, num98, this.X[4], 8 );
            int num120 = this.FF1( num98, num119, num118, num117, this.X[1], 11 );
            int num121 = this.FF1( num117, num120, num119, num118, this.X[3], 14 );
            int num122 = this.FF1( num118, num121, num120, num119, this.X[11], 14 );
            int num123 = this.FF1( num119, num122, num121, num120, this.X[15], 6 );
            int num124 = this.FF1( num120, num123, num122, num121, this.X[0], 14 );
            int num125 = this.FF1( num121, num124, num123, num122, this.X[5], 6 );
            int num126 = this.FF1( num122, num125, num124, num123, this.X[12], 9 );
            int num127 = this.FF1( num123, num126, num125, num124, this.X[2], 12 );
            int num128 = this.FF1( num124, num127, num126, num125, this.X[13], 9 );
            int num129 = this.FF1( num125, num128, num127, num126, this.X[9], 12 );
            int num130 = this.FF1( num126, num129, num128, num127, this.X[7], 5 );
            int b4 = this.FF1( num127, num130, num129, num128, this.X[10], 15 );
            int num131 = this.FF1( num128, b4, num130, num129, this.X[14], 8 );
            int num132 = num115;
            int num133 = num130;
            int num134 = num132;
            this.H0 += num114;
            this.H1 += num116;
            this.H2 += b3;
            this.H3 += num133;
            this.H4 += num129;
            this.H5 += num131;
            this.H6 += b4;
            this.H7 += num134;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        public override IMemoable Copy() => new RipeMD256Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (RipeMD256Digest)other );
    }
}
