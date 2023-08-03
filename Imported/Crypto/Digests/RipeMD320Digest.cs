// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.RipeMD320Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class RipeMD320Digest : GeneralDigest
    {
        private const int DigestLength = 40;
        private int H0;
        private int H1;
        private int H2;
        private int H3;
        private int H4;
        private int H5;
        private int H6;
        private int H7;
        private int H8;
        private int H9;
        private int[] X = new int[16];
        private int xOff;

        public override string AlgorithmName => "RIPEMD320";

        public override int GetDigestSize() => 40;

        public RipeMD320Digest() => this.Reset();

        public RipeMD320Digest( RipeMD320Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( RipeMD320Digest t )
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
            this.H8 = t.H8;
            this.H9 = t.H9;
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
            this.UnpackWord( this.H8, output, outOff + 32 );
            this.UnpackWord( this.H9, output, outOff + 36 );
            this.Reset();
            return 40;
        }

        public override void Reset()
        {
            base.Reset();
            this.H0 = 1732584193;
            this.H1 = -271733879;
            this.H2 = -1732584194;
            this.H3 = 271733878;
            this.H4 = -1009589776;
            this.H5 = 1985229328;
            this.H6 = -19088744;
            this.H7 = -1985229329;
            this.H8 = 19088743;
            this.H9 = 1009589775;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        private int RL( int x, int n ) => (x << n) | x >>> 32 - n;

        private int F1( int x, int y, int z ) => x ^ y ^ z;

        private int F2( int x, int y, int z ) => (x & y) | (~x & z);

        private int F3( int x, int y, int z ) => (x | ~y) ^ z;

        private int F4( int x, int y, int z ) => (x & z) | (y & ~z);

        private int F5( int x, int y, int z ) => x ^ (y | ~z);

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
            int h8 = this.H8;
            int h9 = this.H9;
            int num1 = this.RL( h0 + this.F1( h1, h2, h3 ) + this.X[0], 11 ) + h4;
            int z1 = this.RL( h2, 10 );
            int num2 = this.RL( h4 + this.F1( num1, h1, z1 ) + this.X[1], 14 ) + h3;
            int z2 = this.RL( h1, 10 );
            int num3 = this.RL( h3 + this.F1( num2, num1, z2 ) + this.X[2], 15 ) + z1;
            int z3 = this.RL( num1, 10 );
            int num4 = this.RL( z1 + this.F1( num3, num2, z3 ) + this.X[3], 12 ) + z2;
            int z4 = this.RL( num2, 10 );
            int num5 = this.RL( z2 + this.F1( num4, num3, z4 ) + this.X[4], 5 ) + z3;
            int z5 = this.RL( num3, 10 );
            int num6 = this.RL( z3 + this.F1( num5, num4, z5 ) + this.X[5], 8 ) + z4;
            int z6 = this.RL( num4, 10 );
            int num7 = this.RL( z4 + this.F1( num6, num5, z6 ) + this.X[6], 7 ) + z5;
            int z7 = this.RL( num5, 10 );
            int num8 = this.RL( z5 + this.F1( num7, num6, z7 ) + this.X[7], 9 ) + z6;
            int z8 = this.RL( num6, 10 );
            int num9 = this.RL( z6 + this.F1( num8, num7, z8 ) + this.X[8], 11 ) + z7;
            int z9 = this.RL( num7, 10 );
            int num10 = this.RL( z7 + this.F1( num9, num8, z9 ) + this.X[9], 13 ) + z8;
            int z10 = this.RL( num8, 10 );
            int num11 = this.RL( z8 + this.F1( num10, num9, z10 ) + this.X[10], 14 ) + z9;
            int z11 = this.RL( num9, 10 );
            int num12 = this.RL( z9 + this.F1( num11, num10, z11 ) + this.X[11], 15 ) + z10;
            int z12 = this.RL( num10, 10 );
            int num13 = this.RL( z10 + this.F1( num12, num11, z12 ) + this.X[12], 6 ) + z11;
            int z13 = this.RL( num11, 10 );
            int num14 = this.RL( z11 + this.F1( num13, num12, z13 ) + this.X[13], 7 ) + z12;
            int z14 = this.RL( num12, 10 );
            int num15 = this.RL( z12 + this.F1( num14, num13, z14 ) + this.X[14], 9 ) + z13;
            int z15 = this.RL( num13, 10 );
            int num16 = this.RL( z13 + this.F1( num15, num14, z15 ) + this.X[15], 8 ) + z14;
            int z16 = this.RL( num14, 10 );
            int num17 = this.RL( h5 + this.F5( h6, h7, h8 ) + this.X[5] + 1352829926, 8 ) + h9;
            int z17 = this.RL( h7, 10 );
            int num18 = this.RL( h9 + this.F5( num17, h6, z17 ) + this.X[14] + 1352829926, 9 ) + h8;
            int z18 = this.RL( h6, 10 );
            int num19 = this.RL( h8 + this.F5( num18, num17, z18 ) + this.X[7] + 1352829926, 9 ) + z17;
            int z19 = this.RL( num17, 10 );
            int num20 = this.RL( z17 + this.F5( num19, num18, z19 ) + this.X[0] + 1352829926, 11 ) + z18;
            int z20 = this.RL( num18, 10 );
            int num21 = this.RL( z18 + this.F5( num20, num19, z20 ) + this.X[9] + 1352829926, 13 ) + z19;
            int z21 = this.RL( num19, 10 );
            int num22 = this.RL( z19 + this.F5( num21, num20, z21 ) + this.X[2] + 1352829926, 15 ) + z20;
            int z22 = this.RL( num20, 10 );
            int num23 = this.RL( z20 + this.F5( num22, num21, z22 ) + this.X[11] + 1352829926, 15 ) + z21;
            int z23 = this.RL( num21, 10 );
            int num24 = this.RL( z21 + this.F5( num23, num22, z23 ) + this.X[4] + 1352829926, 5 ) + z22;
            int z24 = this.RL( num22, 10 );
            int num25 = this.RL( z22 + this.F5( num24, num23, z24 ) + this.X[13] + 1352829926, 7 ) + z23;
            int z25 = this.RL( num23, 10 );
            int num26 = this.RL( z23 + this.F5( num25, num24, z25 ) + this.X[6] + 1352829926, 7 ) + z24;
            int z26 = this.RL( num24, 10 );
            int num27 = this.RL( z24 + this.F5( num26, num25, z26 ) + this.X[15] + 1352829926, 8 ) + z25;
            int z27 = this.RL( num25, 10 );
            int num28 = this.RL( z25 + this.F5( num27, num26, z27 ) + this.X[8] + 1352829926, 11 ) + z26;
            int z28 = this.RL( num26, 10 );
            int num29 = this.RL( z26 + this.F5( num28, num27, z28 ) + this.X[1] + 1352829926, 14 ) + z27;
            int z29 = this.RL( num27, 10 );
            int num30 = this.RL( z27 + this.F5( num29, num28, z29 ) + this.X[10] + 1352829926, 14 ) + z28;
            int z30 = this.RL( num28, 10 );
            int num31 = this.RL( z28 + this.F5( num30, num29, z30 ) + this.X[3] + 1352829926, 12 ) + z29;
            int z31 = this.RL( num29, 10 );
            int num32 = this.RL( z29 + this.F5( num31, num30, z31 ) + this.X[12] + 1352829926, 6 ) + z30;
            int z32 = this.RL( num30, 10 );
            int num33 = num16;
            int num34 = num32;
            int num35 = num33;
            int num36 = this.RL( z14 + this.F2( num34, num15, z16 ) + this.X[7] + 1518500249, 7 ) + z15;
            int z33 = this.RL( num15, 10 );
            int num37 = this.RL( z15 + this.F2( num36, num34, z33 ) + this.X[4] + 1518500249, 6 ) + z16;
            int z34 = this.RL( num34, 10 );
            int num38 = this.RL( z16 + this.F2( num37, num36, z34 ) + this.X[13] + 1518500249, 8 ) + z33;
            int z35 = this.RL( num36, 10 );
            int num39 = this.RL( z33 + this.F2( num38, num37, z35 ) + this.X[1] + 1518500249, 13 ) + z34;
            int z36 = this.RL( num37, 10 );
            int num40 = this.RL( z34 + this.F2( num39, num38, z36 ) + this.X[10] + 1518500249, 11 ) + z35;
            int z37 = this.RL( num38, 10 );
            int num41 = this.RL( z35 + this.F2( num40, num39, z37 ) + this.X[6] + 1518500249, 9 ) + z36;
            int z38 = this.RL( num39, 10 );
            int num42 = this.RL( z36 + this.F2( num41, num40, z38 ) + this.X[15] + 1518500249, 7 ) + z37;
            int z39 = this.RL( num40, 10 );
            int num43 = this.RL( z37 + this.F2( num42, num41, z39 ) + this.X[3] + 1518500249, 15 ) + z38;
            int z40 = this.RL( num41, 10 );
            int num44 = this.RL( z38 + this.F2( num43, num42, z40 ) + this.X[12] + 1518500249, 7 ) + z39;
            int z41 = this.RL( num42, 10 );
            int num45 = this.RL( z39 + this.F2( num44, num43, z41 ) + this.X[0] + 1518500249, 12 ) + z40;
            int z42 = this.RL( num43, 10 );
            int num46 = this.RL( z40 + this.F2( num45, num44, z42 ) + this.X[9] + 1518500249, 15 ) + z41;
            int z43 = this.RL( num44, 10 );
            int num47 = this.RL( z41 + this.F2( num46, num45, z43 ) + this.X[5] + 1518500249, 9 ) + z42;
            int z44 = this.RL( num45, 10 );
            int num48 = this.RL( z42 + this.F2( num47, num46, z44 ) + this.X[2] + 1518500249, 11 ) + z43;
            int z45 = this.RL( num46, 10 );
            int num49 = this.RL( z43 + this.F2( num48, num47, z45 ) + this.X[14] + 1518500249, 7 ) + z44;
            int z46 = this.RL( num47, 10 );
            int num50 = this.RL( z44 + this.F2( num49, num48, z46 ) + this.X[11] + 1518500249, 13 ) + z45;
            int z47 = this.RL( num48, 10 );
            int num51 = this.RL( z45 + this.F2( num50, num49, z47 ) + this.X[8] + 1518500249, 12 ) + z46;
            int num52 = this.RL( num49, 10 );
            int num53 = this.RL( z30 + this.F4( num35, num31, z32 ) + this.X[6] + 1548603684, 9 ) + z31;
            int z48 = this.RL( num31, 10 );
            int num54 = this.RL( z31 + this.F4( num53, num35, z48 ) + this.X[11] + 1548603684, 13 ) + z32;
            int z49 = this.RL( num35, 10 );
            int num55 = this.RL( z32 + this.F4( num54, num53, z49 ) + this.X[3] + 1548603684, 15 ) + z48;
            int z50 = this.RL( num53, 10 );
            int num56 = this.RL( z48 + this.F4( num55, num54, z50 ) + this.X[7] + 1548603684, 7 ) + z49;
            int z51 = this.RL( num54, 10 );
            int num57 = this.RL( z49 + this.F4( num56, num55, z51 ) + this.X[0] + 1548603684, 12 ) + z50;
            int z52 = this.RL( num55, 10 );
            int num58 = this.RL( z50 + this.F4( num57, num56, z52 ) + this.X[13] + 1548603684, 8 ) + z51;
            int z53 = this.RL( num56, 10 );
            int num59 = this.RL( z51 + this.F4( num58, num57, z53 ) + this.X[5] + 1548603684, 9 ) + z52;
            int z54 = this.RL( num57, 10 );
            int num60 = this.RL( z52 + this.F4( num59, num58, z54 ) + this.X[10] + 1548603684, 11 ) + z53;
            int z55 = this.RL( num58, 10 );
            int num61 = this.RL( z53 + this.F4( num60, num59, z55 ) + this.X[14] + 1548603684, 7 ) + z54;
            int z56 = this.RL( num59, 10 );
            int num62 = this.RL( z54 + this.F4( num61, num60, z56 ) + this.X[15] + 1548603684, 7 ) + z55;
            int z57 = this.RL( num60, 10 );
            int num63 = this.RL( z55 + this.F4( num62, num61, z57 ) + this.X[8] + 1548603684, 12 ) + z56;
            int z58 = this.RL( num61, 10 );
            int num64 = this.RL( z56 + this.F4( num63, num62, z58 ) + this.X[12] + 1548603684, 7 ) + z57;
            int z59 = this.RL( num62, 10 );
            int num65 = this.RL( z57 + this.F4( num64, num63, z59 ) + this.X[4] + 1548603684, 6 ) + z58;
            int z60 = this.RL( num63, 10 );
            int num66 = this.RL( z58 + this.F4( num65, num64, z60 ) + this.X[9] + 1548603684, 15 ) + z59;
            int z61 = this.RL( num64, 10 );
            int num67 = this.RL( z59 + this.F4( num66, num65, z61 ) + this.X[1] + 1548603684, 13 ) + z60;
            int z62 = this.RL( num65, 10 );
            int num68 = this.RL( z60 + this.F4( num67, num66, z62 ) + this.X[2] + 1548603684, 11 ) + z61;
            int num69 = this.RL( num66, 10 );
            int num70 = num52;
            int z63 = num69;
            int z64 = num70;
            int num71 = this.RL( z46 + this.F3( num51, num50, z63 ) + this.X[3] + 1859775393, 11 ) + z47;
            int z65 = this.RL( num50, 10 );
            int num72 = this.RL( z47 + this.F3( num71, num51, z65 ) + this.X[10] + 1859775393, 13 ) + z63;
            int z66 = this.RL( num51, 10 );
            int num73 = this.RL( z63 + this.F3( num72, num71, z66 ) + this.X[14] + 1859775393, 6 ) + z65;
            int z67 = this.RL( num71, 10 );
            int num74 = this.RL( z65 + this.F3( num73, num72, z67 ) + this.X[4] + 1859775393, 7 ) + z66;
            int z68 = this.RL( num72, 10 );
            int num75 = this.RL( z66 + this.F3( num74, num73, z68 ) + this.X[9] + 1859775393, 14 ) + z67;
            int z69 = this.RL( num73, 10 );
            int num76 = this.RL( z67 + this.F3( num75, num74, z69 ) + this.X[15] + 1859775393, 9 ) + z68;
            int z70 = this.RL( num74, 10 );
            int num77 = this.RL( z68 + this.F3( num76, num75, z70 ) + this.X[8] + 1859775393, 13 ) + z69;
            int z71 = this.RL( num75, 10 );
            int num78 = this.RL( z69 + this.F3( num77, num76, z71 ) + this.X[1] + 1859775393, 15 ) + z70;
            int z72 = this.RL( num76, 10 );
            int num79 = this.RL( z70 + this.F3( num78, num77, z72 ) + this.X[2] + 1859775393, 14 ) + z71;
            int z73 = this.RL( num77, 10 );
            int num80 = this.RL( z71 + this.F3( num79, num78, z73 ) + this.X[7] + 1859775393, 8 ) + z72;
            int z74 = this.RL( num78, 10 );
            int num81 = this.RL( z72 + this.F3( num80, num79, z74 ) + this.X[0] + 1859775393, 13 ) + z73;
            int z75 = this.RL( num79, 10 );
            int num82 = this.RL( z73 + this.F3( num81, num80, z75 ) + this.X[6] + 1859775393, 6 ) + z74;
            int z76 = this.RL( num80, 10 );
            int num83 = this.RL( z74 + this.F3( num82, num81, z76 ) + this.X[13] + 1859775393, 5 ) + z75;
            int z77 = this.RL( num81, 10 );
            int num84 = this.RL( z75 + this.F3( num83, num82, z77 ) + this.X[11] + 1859775393, 12 ) + z76;
            int z78 = this.RL( num82, 10 );
            int num85 = this.RL( z76 + this.F3( num84, num83, z78 ) + this.X[5] + 1859775393, 7 ) + z77;
            int z79 = this.RL( num83, 10 );
            int num86 = this.RL( z77 + this.F3( num85, num84, z79 ) + this.X[12] + 1859775393, 5 ) + z78;
            int z80 = this.RL( num84, 10 );
            int num87 = this.RL( z61 + this.F3( num68, num67, z64 ) + this.X[15] + 1836072691, 9 ) + z62;
            int z81 = this.RL( num67, 10 );
            int num88 = this.RL( z62 + this.F3( num87, num68, z81 ) + this.X[5] + 1836072691, 7 ) + z64;
            int z82 = this.RL( num68, 10 );
            int num89 = this.RL( z64 + this.F3( num88, num87, z82 ) + this.X[1] + 1836072691, 15 ) + z81;
            int z83 = this.RL( num87, 10 );
            int num90 = this.RL( z81 + this.F3( num89, num88, z83 ) + this.X[3] + 1836072691, 11 ) + z82;
            int z84 = this.RL( num88, 10 );
            int num91 = this.RL( z82 + this.F3( num90, num89, z84 ) + this.X[7] + 1836072691, 8 ) + z83;
            int z85 = this.RL( num89, 10 );
            int num92 = this.RL( z83 + this.F3( num91, num90, z85 ) + this.X[14] + 1836072691, 6 ) + z84;
            int z86 = this.RL( num90, 10 );
            int num93 = this.RL( z84 + this.F3( num92, num91, z86 ) + this.X[6] + 1836072691, 6 ) + z85;
            int z87 = this.RL( num91, 10 );
            int num94 = this.RL( z85 + this.F3( num93, num92, z87 ) + this.X[9] + 1836072691, 14 ) + z86;
            int z88 = this.RL( num92, 10 );
            int num95 = this.RL( z86 + this.F3( num94, num93, z88 ) + this.X[11] + 1836072691, 12 ) + z87;
            int z89 = this.RL( num93, 10 );
            int num96 = this.RL( z87 + this.F3( num95, num94, z89 ) + this.X[8] + 1836072691, 13 ) + z88;
            int z90 = this.RL( num94, 10 );
            int num97 = this.RL( z88 + this.F3( num96, num95, z90 ) + this.X[12] + 1836072691, 5 ) + z89;
            int z91 = this.RL( num95, 10 );
            int num98 = this.RL( z89 + this.F3( num97, num96, z91 ) + this.X[2] + 1836072691, 14 ) + z90;
            int z92 = this.RL( num96, 10 );
            int num99 = this.RL( z90 + this.F3( num98, num97, z92 ) + this.X[10] + 1836072691, 13 ) + z91;
            int z93 = this.RL( num97, 10 );
            int num100 = this.RL( z91 + this.F3( num99, num98, z93 ) + this.X[0] + 1836072691, 13 ) + z92;
            int z94 = this.RL( num98, 10 );
            int num101 = this.RL( z92 + this.F3( num100, num99, z94 ) + this.X[4] + 1836072691, 7 ) + z93;
            int z95 = this.RL( num99, 10 );
            int num102 = this.RL( z93 + this.F3( num101, num100, z95 ) + this.X[13] + 1836072691, 5 ) + z94;
            int z96 = this.RL( num100, 10 );
            int num103 = z78;
            int num104 = z94;
            int num105 = num103;
            int num106 = this.RL( num104 + this.F4( num86, num85, z80 ) + this.X[1] - 1894007588, 11 ) + z79;
            int z97 = this.RL( num85, 10 );
            int num107 = this.RL( z79 + this.F4( num106, num86, z97 ) + this.X[9] - 1894007588, 12 ) + z80;
            int z98 = this.RL( num86, 10 );
            int num108 = this.RL( z80 + this.F4( num107, num106, z98 ) + this.X[11] - 1894007588, 14 ) + z97;
            int z99 = this.RL( num106, 10 );
            int num109 = this.RL( z97 + this.F4( num108, num107, z99 ) + this.X[10] - 1894007588, 15 ) + z98;
            int z100 = this.RL( num107, 10 );
            int num110 = this.RL( z98 + this.F4( num109, num108, z100 ) + this.X[0] - 1894007588, 14 ) + z99;
            int z101 = this.RL( num108, 10 );
            int num111 = this.RL( z99 + this.F4( num110, num109, z101 ) + this.X[8] - 1894007588, 15 ) + z100;
            int z102 = this.RL( num109, 10 );
            int num112 = this.RL( z100 + this.F4( num111, num110, z102 ) + this.X[12] - 1894007588, 9 ) + z101;
            int z103 = this.RL( num110, 10 );
            int num113 = this.RL( z101 + this.F4( num112, num111, z103 ) + this.X[4] - 1894007588, 8 ) + z102;
            int z104 = this.RL( num111, 10 );
            int num114 = this.RL( z102 + this.F4( num113, num112, z104 ) + this.X[13] - 1894007588, 9 ) + z103;
            int z105 = this.RL( num112, 10 );
            int num115 = this.RL( z103 + this.F4( num114, num113, z105 ) + this.X[3] - 1894007588, 14 ) + z104;
            int z106 = this.RL( num113, 10 );
            int num116 = this.RL( z104 + this.F4( num115, num114, z106 ) + this.X[7] - 1894007588, 5 ) + z105;
            int z107 = this.RL( num114, 10 );
            int num117 = this.RL( z105 + this.F4( num116, num115, z107 ) + this.X[15] - 1894007588, 6 ) + z106;
            int z108 = this.RL( num115, 10 );
            int num118 = this.RL( z106 + this.F4( num117, num116, z108 ) + this.X[14] - 1894007588, 8 ) + z107;
            int z109 = this.RL( num116, 10 );
            int num119 = this.RL( z107 + this.F4( num118, num117, z109 ) + this.X[5] - 1894007588, 6 ) + z108;
            int z110 = this.RL( num117, 10 );
            int x1 = this.RL( z108 + this.F4( num119, num118, z110 ) + this.X[6] - 1894007588, 5 ) + z109;
            int z111 = this.RL( num118, 10 );
            int num120 = this.RL( z109 + this.F4( x1, num119, z111 ) + this.X[2] - 1894007588, 12 ) + z110;
            int z112 = this.RL( num119, 10 );
            int num121 = this.RL( num105 + this.F2( num102, num101, z96 ) + this.X[8] + 2053994217, 15 ) + z95;
            int z113 = this.RL( num101, 10 );
            int num122 = this.RL( z95 + this.F2( num121, num102, z113 ) + this.X[6] + 2053994217, 5 ) + z96;
            int z114 = this.RL( num102, 10 );
            int num123 = this.RL( z96 + this.F2( num122, num121, z114 ) + this.X[4] + 2053994217, 8 ) + z113;
            int z115 = this.RL( num121, 10 );
            int num124 = this.RL( z113 + this.F2( num123, num122, z115 ) + this.X[1] + 2053994217, 11 ) + z114;
            int z116 = this.RL( num122, 10 );
            int num125 = this.RL( z114 + this.F2( num124, num123, z116 ) + this.X[3] + 2053994217, 14 ) + z115;
            int z117 = this.RL( num123, 10 );
            int num126 = this.RL( z115 + this.F2( num125, num124, z117 ) + this.X[11] + 2053994217, 14 ) + z116;
            int z118 = this.RL( num124, 10 );
            int num127 = this.RL( z116 + this.F2( num126, num125, z118 ) + this.X[15] + 2053994217, 6 ) + z117;
            int z119 = this.RL( num125, 10 );
            int num128 = this.RL( z117 + this.F2( num127, num126, z119 ) + this.X[0] + 2053994217, 14 ) + z118;
            int z120 = this.RL( num126, 10 );
            int num129 = this.RL( z118 + this.F2( num128, num127, z120 ) + this.X[5] + 2053994217, 6 ) + z119;
            int z121 = this.RL( num127, 10 );
            int num130 = this.RL( z119 + this.F2( num129, num128, z121 ) + this.X[12] + 2053994217, 9 ) + z120;
            int z122 = this.RL( num128, 10 );
            int num131 = this.RL( z120 + this.F2( num130, num129, z122 ) + this.X[2] + 2053994217, 12 ) + z121;
            int z123 = this.RL( num129, 10 );
            int num132 = this.RL( z121 + this.F2( num131, num130, z123 ) + this.X[13] + 2053994217, 9 ) + z122;
            int z124 = this.RL( num130, 10 );
            int num133 = this.RL( z122 + this.F2( num132, num131, z124 ) + this.X[9] + 2053994217, 12 ) + z123;
            int z125 = this.RL( num131, 10 );
            int num134 = this.RL( z123 + this.F2( num133, num132, z125 ) + this.X[7] + 2053994217, 5 ) + z124;
            int z126 = this.RL( num132, 10 );
            int x2 = this.RL( z124 + this.F2( num134, num133, z126 ) + this.X[10] + 2053994217, 15 ) + z125;
            int z127 = this.RL( num133, 10 );
            int num135 = this.RL( z125 + this.F2( x2, num134, z127 ) + this.X[14] + 2053994217, 8 ) + z126;
            int z128 = this.RL( num134, 10 );
            int num136 = x1;
            int num137 = x2;
            int num138 = num136;
            int num139 = this.RL( z110 + this.F5( num120, num137, z112 ) + this.X[4] - 1454113458, 9 ) + z111;
            int z129 = this.RL( num137, 10 );
            int num140 = this.RL( z111 + this.F5( num139, num120, z129 ) + this.X[0] - 1454113458, 15 ) + z112;
            int z130 = this.RL( num120, 10 );
            int num141 = this.RL( z112 + this.F5( num140, num139, z130 ) + this.X[5] - 1454113458, 5 ) + z129;
            int z131 = this.RL( num139, 10 );
            int num142 = this.RL( z129 + this.F5( num141, num140, z131 ) + this.X[9] - 1454113458, 11 ) + z130;
            int z132 = this.RL( num140, 10 );
            int num143 = this.RL( z130 + this.F5( num142, num141, z132 ) + this.X[7] - 1454113458, 6 ) + z131;
            int z133 = this.RL( num141, 10 );
            int num144 = this.RL( z131 + this.F5( num143, num142, z133 ) + this.X[12] - 1454113458, 8 ) + z132;
            int z134 = this.RL( num142, 10 );
            int num145 = this.RL( z132 + this.F5( num144, num143, z134 ) + this.X[2] - 1454113458, 13 ) + z133;
            int z135 = this.RL( num143, 10 );
            int num146 = this.RL( z133 + this.F5( num145, num144, z135 ) + this.X[10] - 1454113458, 12 ) + z134;
            int z136 = this.RL( num144, 10 );
            int num147 = this.RL( z134 + this.F5( num146, num145, z136 ) + this.X[14] - 1454113458, 5 ) + z135;
            int z137 = this.RL( num145, 10 );
            int num148 = this.RL( z135 + this.F5( num147, num146, z137 ) + this.X[1] - 1454113458, 12 ) + z136;
            int z138 = this.RL( num146, 10 );
            int num149 = this.RL( z136 + this.F5( num148, num147, z138 ) + this.X[3] - 1454113458, 13 ) + z137;
            int z139 = this.RL( num147, 10 );
            int num150 = this.RL( z137 + this.F5( num149, num148, z139 ) + this.X[8] - 1454113458, 14 ) + z138;
            int z140 = this.RL( num148, 10 );
            int num151 = this.RL( z138 + this.F5( num150, num149, z140 ) + this.X[11] - 1454113458, 11 ) + z139;
            int z141 = this.RL( num149, 10 );
            int num152 = this.RL( z139 + this.F5( num151, num150, z141 ) + this.X[6] - 1454113458, 8 ) + z140;
            int z142 = this.RL( num150, 10 );
            int x3 = this.RL( z140 + this.F5( num152, num151, z142 ) + this.X[15] - 1454113458, 5 ) + z141;
            int z143 = this.RL( num151, 10 );
            int num153 = this.RL( z141 + this.F5( x3, num152, z143 ) + this.X[13] - 1454113458, 6 ) + z142;
            int num154 = this.RL( num152, 10 );
            int num155 = this.RL( z126 + this.F1( num135, num138, z128 ) + this.X[12], 8 ) + z127;
            int z144 = this.RL( num138, 10 );
            int num156 = this.RL( z127 + this.F1( num155, num135, z144 ) + this.X[15], 5 ) + z128;
            int z145 = this.RL( num135, 10 );
            int num157 = this.RL( z128 + this.F1( num156, num155, z145 ) + this.X[10], 12 ) + z144;
            int z146 = this.RL( num155, 10 );
            int num158 = this.RL( z144 + this.F1( num157, num156, z146 ) + this.X[4], 9 ) + z145;
            int z147 = this.RL( num156, 10 );
            int num159 = this.RL( z145 + this.F1( num158, num157, z147 ) + this.X[1], 12 ) + z146;
            int z148 = this.RL( num157, 10 );
            int num160 = this.RL( z146 + this.F1( num159, num158, z148 ) + this.X[5], 5 ) + z147;
            int z149 = this.RL( num158, 10 );
            int num161 = this.RL( z147 + this.F1( num160, num159, z149 ) + this.X[8], 14 ) + z148;
            int z150 = this.RL( num159, 10 );
            int num162 = this.RL( z148 + this.F1( num161, num160, z150 ) + this.X[7], 6 ) + z149;
            int z151 = this.RL( num160, 10 );
            int num163 = this.RL( z149 + this.F1( num162, num161, z151 ) + this.X[6], 8 ) + z150;
            int z152 = this.RL( num161, 10 );
            int num164 = this.RL( z150 + this.F1( num163, num162, z152 ) + this.X[2], 13 ) + z151;
            int z153 = this.RL( num162, 10 );
            int num165 = this.RL( z151 + this.F1( num164, num163, z153 ) + this.X[13], 6 ) + z152;
            int z154 = this.RL( num163, 10 );
            int num166 = this.RL( z152 + this.F1( num165, num164, z154 ) + this.X[14], 5 ) + z153;
            int z155 = this.RL( num164, 10 );
            int num167 = this.RL( z153 + this.F1( num166, num165, z155 ) + this.X[0], 15 ) + z154;
            int z156 = this.RL( num165, 10 );
            int num168 = this.RL( z154 + this.F1( num167, num166, z156 ) + this.X[3], 13 ) + z155;
            int z157 = this.RL( num166, 10 );
            int x4 = this.RL( z155 + this.F1( num168, num167, z157 ) + this.X[9], 11 ) + z156;
            int z158 = this.RL( num167, 10 );
            int num169 = this.RL( z156 + this.F1( x4, num168, z158 ) + this.X[11], 11 ) + z157;
            int num170 = this.RL( num168, 10 );
            this.H0 += z142;
            this.H1 += num153;
            this.H2 += x3;
            this.H3 += num154;
            this.H4 += z158;
            this.H5 += z157;
            this.H6 += num169;
            this.H7 += x4;
            this.H8 += num170;
            this.H9 += z143;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        public override IMemoable Copy() => new RipeMD320Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (RipeMD320Digest)other );
    }
}
