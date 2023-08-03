// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.RipeMD128Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class RipeMD128Digest : GeneralDigest
    {
        private const int DigestLength = 16;
        private int H0;
        private int H1;
        private int H2;
        private int H3;
        private int[] X = new int[16];
        private int xOff;

        public RipeMD128Digest() => this.Reset();

        public RipeMD128Digest( RipeMD128Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( RipeMD128Digest t )
        {
            this.CopyIn( (GeneralDigest)t );
            this.H0 = t.H0;
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
        }

        public override string AlgorithmName => "RIPEMD128";

        public override int GetDigestSize() => 16;

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
            this.Reset();
            return 16;
        }

        public override void Reset()
        {
            base.Reset();
            this.H0 = 1732584193;
            this.H1 = -271733879;
            this.H2 = -1732584194;
            this.H3 = 271733878;
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
            int h0;
            int a = h0 = this.H0;
            int h1;
            int num1 = h1 = this.H1;
            int h2;
            int num2 = h2 = this.H2;
            int h3;
            int num3 = h3 = this.H3;
            int num4 = this.F1( a, num1, num2, num3, this.X[0], 11 );
            int num5 = this.F1( num3, num4, num1, num2, this.X[1], 14 );
            int num6 = this.F1( num2, num5, num4, num1, this.X[2], 15 );
            int num7 = this.F1( num1, num6, num5, num4, this.X[3], 12 );
            int num8 = this.F1( num4, num7, num6, num5, this.X[4], 5 );
            int num9 = this.F1( num5, num8, num7, num6, this.X[5], 8 );
            int num10 = this.F1( num6, num9, num8, num7, this.X[6], 7 );
            int num11 = this.F1( num7, num10, num9, num8, this.X[7], 9 );
            int num12 = this.F1( num8, num11, num10, num9, this.X[8], 11 );
            int num13 = this.F1( num9, num12, num11, num10, this.X[9], 13 );
            int num14 = this.F1( num10, num13, num12, num11, this.X[10], 14 );
            int num15 = this.F1( num11, num14, num13, num12, this.X[11], 15 );
            int num16 = this.F1( num12, num15, num14, num13, this.X[12], 6 );
            int num17 = this.F1( num13, num16, num15, num14, this.X[13], 7 );
            int num18 = this.F1( num14, num17, num16, num15, this.X[14], 9 );
            int num19 = this.F1( num15, num18, num17, num16, this.X[15], 8 );
            int num20 = this.F2( num16, num19, num18, num17, this.X[7], 7 );
            int num21 = this.F2( num17, num20, num19, num18, this.X[4], 6 );
            int num22 = this.F2( num18, num21, num20, num19, this.X[13], 8 );
            int num23 = this.F2( num19, num22, num21, num20, this.X[1], 13 );
            int num24 = this.F2( num20, num23, num22, num21, this.X[10], 11 );
            int num25 = this.F2( num21, num24, num23, num22, this.X[6], 9 );
            int num26 = this.F2( num22, num25, num24, num23, this.X[15], 7 );
            int num27 = this.F2( num23, num26, num25, num24, this.X[3], 15 );
            int num28 = this.F2( num24, num27, num26, num25, this.X[12], 7 );
            int num29 = this.F2( num25, num28, num27, num26, this.X[0], 12 );
            int num30 = this.F2( num26, num29, num28, num27, this.X[9], 15 );
            int num31 = this.F2( num27, num30, num29, num28, this.X[5], 9 );
            int num32 = this.F2( num28, num31, num30, num29, this.X[2], 11 );
            int num33 = this.F2( num29, num32, num31, num30, this.X[14], 7 );
            int num34 = this.F2( num30, num33, num32, num31, this.X[11], 13 );
            int num35 = this.F2( num31, num34, num33, num32, this.X[8], 12 );
            int num36 = this.F3( num32, num35, num34, num33, this.X[3], 11 );
            int num37 = this.F3( num33, num36, num35, num34, this.X[10], 13 );
            int num38 = this.F3( num34, num37, num36, num35, this.X[14], 6 );
            int num39 = this.F3( num35, num38, num37, num36, this.X[4], 7 );
            int num40 = this.F3( num36, num39, num38, num37, this.X[9], 14 );
            int num41 = this.F3( num37, num40, num39, num38, this.X[15], 9 );
            int num42 = this.F3( num38, num41, num40, num39, this.X[8], 13 );
            int num43 = this.F3( num39, num42, num41, num40, this.X[1], 15 );
            int num44 = this.F3( num40, num43, num42, num41, this.X[2], 14 );
            int num45 = this.F3( num41, num44, num43, num42, this.X[7], 8 );
            int num46 = this.F3( num42, num45, num44, num43, this.X[0], 13 );
            int num47 = this.F3( num43, num46, num45, num44, this.X[6], 6 );
            int num48 = this.F3( num44, num47, num46, num45, this.X[13], 5 );
            int num49 = this.F3( num45, num48, num47, num46, this.X[11], 12 );
            int num50 = this.F3( num46, num49, num48, num47, this.X[5], 7 );
            int num51 = this.F3( num47, num50, num49, num48, this.X[12], 5 );
            int num52 = this.F4( num48, num51, num50, num49, this.X[1], 11 );
            int num53 = this.F4( num49, num52, num51, num50, this.X[9], 12 );
            int num54 = this.F4( num50, num53, num52, num51, this.X[11], 14 );
            int num55 = this.F4( num51, num54, num53, num52, this.X[10], 15 );
            int num56 = this.F4( num52, num55, num54, num53, this.X[0], 14 );
            int num57 = this.F4( num53, num56, num55, num54, this.X[8], 15 );
            int num58 = this.F4( num54, num57, num56, num55, this.X[12], 9 );
            int num59 = this.F4( num55, num58, num57, num56, this.X[4], 8 );
            int num60 = this.F4( num56, num59, num58, num57, this.X[13], 9 );
            int num61 = this.F4( num57, num60, num59, num58, this.X[3], 14 );
            int num62 = this.F4( num58, num61, num60, num59, this.X[7], 5 );
            int num63 = this.F4( num59, num62, num61, num60, this.X[15], 6 );
            int num64 = this.F4( num60, num63, num62, num61, this.X[14], 8 );
            int num65 = this.F4( num61, num64, num63, num62, this.X[5], 6 );
            int b1 = this.F4( num62, num65, num64, num63, this.X[6], 5 );
            int num66 = this.F4( num63, b1, num65, num64, this.X[2], 12 );
            int num67 = this.FF4( h0, h1, h2, h3, this.X[5], 8 );
            int num68 = this.FF4( h3, num67, h1, h2, this.X[14], 9 );
            int num69 = this.FF4( h2, num68, num67, h1, this.X[7], 9 );
            int num70 = this.FF4( h1, num69, num68, num67, this.X[0], 11 );
            int num71 = this.FF4( num67, num70, num69, num68, this.X[9], 13 );
            int num72 = this.FF4( num68, num71, num70, num69, this.X[2], 15 );
            int num73 = this.FF4( num69, num72, num71, num70, this.X[11], 15 );
            int num74 = this.FF4( num70, num73, num72, num71, this.X[4], 5 );
            int num75 = this.FF4( num71, num74, num73, num72, this.X[13], 7 );
            int num76 = this.FF4( num72, num75, num74, num73, this.X[6], 7 );
            int num77 = this.FF4( num73, num76, num75, num74, this.X[15], 8 );
            int num78 = this.FF4( num74, num77, num76, num75, this.X[8], 11 );
            int num79 = this.FF4( num75, num78, num77, num76, this.X[1], 14 );
            int num80 = this.FF4( num76, num79, num78, num77, this.X[10], 14 );
            int num81 = this.FF4( num77, num80, num79, num78, this.X[3], 12 );
            int num82 = this.FF4( num78, num81, num80, num79, this.X[12], 6 );
            int num83 = this.FF3( num79, num82, num81, num80, this.X[6], 9 );
            int num84 = this.FF3( num80, num83, num82, num81, this.X[11], 13 );
            int num85 = this.FF3( num81, num84, num83, num82, this.X[3], 15 );
            int num86 = this.FF3( num82, num85, num84, num83, this.X[7], 7 );
            int num87 = this.FF3( num83, num86, num85, num84, this.X[0], 12 );
            int num88 = this.FF3( num84, num87, num86, num85, this.X[13], 8 );
            int num89 = this.FF3( num85, num88, num87, num86, this.X[5], 9 );
            int num90 = this.FF3( num86, num89, num88, num87, this.X[10], 11 );
            int num91 = this.FF3( num87, num90, num89, num88, this.X[14], 7 );
            int num92 = this.FF3( num88, num91, num90, num89, this.X[15], 7 );
            int num93 = this.FF3( num89, num92, num91, num90, this.X[8], 12 );
            int num94 = this.FF3( num90, num93, num92, num91, this.X[12], 7 );
            int num95 = this.FF3( num91, num94, num93, num92, this.X[4], 6 );
            int num96 = this.FF3( num92, num95, num94, num93, this.X[9], 15 );
            int num97 = this.FF3( num93, num96, num95, num94, this.X[1], 13 );
            int num98 = this.FF3( num94, num97, num96, num95, this.X[2], 11 );
            int num99 = this.FF2( num95, num98, num97, num96, this.X[15], 9 );
            int num100 = this.FF2( num96, num99, num98, num97, this.X[5], 7 );
            int num101 = this.FF2( num97, num100, num99, num98, this.X[1], 15 );
            int num102 = this.FF2( num98, num101, num100, num99, this.X[3], 11 );
            int num103 = this.FF2( num99, num102, num101, num100, this.X[7], 8 );
            int num104 = this.FF2( num100, num103, num102, num101, this.X[14], 6 );
            int num105 = this.FF2( num101, num104, num103, num102, this.X[6], 6 );
            int num106 = this.FF2( num102, num105, num104, num103, this.X[9], 14 );
            int num107 = this.FF2( num103, num106, num105, num104, this.X[11], 12 );
            int num108 = this.FF2( num104, num107, num106, num105, this.X[8], 13 );
            int num109 = this.FF2( num105, num108, num107, num106, this.X[12], 5 );
            int num110 = this.FF2( num106, num109, num108, num107, this.X[2], 14 );
            int num111 = this.FF2( num107, num110, num109, num108, this.X[10], 13 );
            int num112 = this.FF2( num108, num111, num110, num109, this.X[0], 13 );
            int num113 = this.FF2( num109, num112, num111, num110, this.X[4], 7 );
            int num114 = this.FF2( num110, num113, num112, num111, this.X[13], 5 );
            int num115 = this.FF1( num111, num114, num113, num112, this.X[8], 15 );
            int num116 = this.FF1( num112, num115, num114, num113, this.X[6], 5 );
            int num117 = this.FF1( num113, num116, num115, num114, this.X[4], 8 );
            int num118 = this.FF1( num114, num117, num116, num115, this.X[1], 11 );
            int num119 = this.FF1( num115, num118, num117, num116, this.X[3], 14 );
            int num120 = this.FF1( num116, num119, num118, num117, this.X[11], 14 );
            int num121 = this.FF1( num117, num120, num119, num118, this.X[15], 6 );
            int num122 = this.FF1( num118, num121, num120, num119, this.X[0], 14 );
            int num123 = this.FF1( num119, num122, num121, num120, this.X[5], 6 );
            int num124 = this.FF1( num120, num123, num122, num121, this.X[12], 9 );
            int num125 = this.FF1( num121, num124, num123, num122, this.X[2], 12 );
            int num126 = this.FF1( num122, num125, num124, num123, this.X[13], 9 );
            int num127 = this.FF1( num123, num126, num125, num124, this.X[9], 12 );
            int num128 = this.FF1( num124, num127, num126, num125, this.X[7], 5 );
            int b2 = this.FF1( num125, num128, num127, num126, this.X[10], 15 );
            int num129 = this.FF1( num126, b2, num128, num127, this.X[14], 8 );
            int num130 = num128 + b1 + this.H1;
            this.H1 = this.H2 + num65 + num127;
            this.H2 = this.H3 + num64 + num129;
            this.H3 = this.H0 + num66 + b2;
            this.H0 = num130;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        public override IMemoable Copy() => new RipeMD128Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (RipeMD128Digest)other );
    }
}
