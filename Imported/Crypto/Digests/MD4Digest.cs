// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.MD4Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class MD4Digest : GeneralDigest
    {
        private const int DigestLength = 16;
        private const int S11 = 3;
        private const int S12 = 7;
        private const int S13 = 11;
        private const int S14 = 19;
        private const int S21 = 3;
        private const int S22 = 5;
        private const int S23 = 9;
        private const int S24 = 13;
        private const int S31 = 3;
        private const int S32 = 9;
        private const int S33 = 11;
        private const int S34 = 15;
        private int H1;
        private int H2;
        private int H3;
        private int H4;
        private int[] X = new int[16];
        private int xOff;

        public MD4Digest() => this.Reset();

        public MD4Digest( MD4Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( MD4Digest t )
        {
            this.CopyIn( (GeneralDigest)t );
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
        }

        public override string AlgorithmName => "MD4";

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
            this.UnpackWord( this.H1, output, outOff );
            this.UnpackWord( this.H2, output, outOff + 4 );
            this.UnpackWord( this.H3, output, outOff + 8 );
            this.UnpackWord( this.H4, output, outOff + 12 );
            this.Reset();
            return 16;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 1732584193;
            this.H2 = -271733879;
            this.H3 = -1732584194;
            this.H4 = 271733878;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        private int RotateLeft( int x, int n ) => (x << n) | x >>> 32 - n;

        private int F( int u, int v, int w ) => (u & v) | (~u & w);

        private int G( int u, int v, int w ) => (u & v) | (u & w) | (v & w);

        private int H( int u, int v, int w ) => u ^ v ^ w;

        internal override void ProcessBlock()
        {
            int h1 = this.H1;
            int h2 = this.H2;
            int h3 = this.H3;
            int h4 = this.H4;
            int num1 = this.RotateLeft( h1 + this.F( h2, h3, h4 ) + this.X[0], 3 );
            int num2 = this.RotateLeft( h4 + this.F( num1, h2, h3 ) + this.X[1], 7 );
            int num3 = this.RotateLeft( h3 + this.F( num2, num1, h2 ) + this.X[2], 11 );
            int num4 = this.RotateLeft( h2 + this.F( num3, num2, num1 ) + this.X[3], 19 );
            int num5 = this.RotateLeft( num1 + this.F( num4, num3, num2 ) + this.X[4], 3 );
            int num6 = this.RotateLeft( num2 + this.F( num5, num4, num3 ) + this.X[5], 7 );
            int num7 = this.RotateLeft( num3 + this.F( num6, num5, num4 ) + this.X[6], 11 );
            int num8 = this.RotateLeft( num4 + this.F( num7, num6, num5 ) + this.X[7], 19 );
            int num9 = this.RotateLeft( num5 + this.F( num8, num7, num6 ) + this.X[8], 3 );
            int num10 = this.RotateLeft( num6 + this.F( num9, num8, num7 ) + this.X[9], 7 );
            int num11 = this.RotateLeft( num7 + this.F( num10, num9, num8 ) + this.X[10], 11 );
            int num12 = this.RotateLeft( num8 + this.F( num11, num10, num9 ) + this.X[11], 19 );
            int num13 = this.RotateLeft( num9 + this.F( num12, num11, num10 ) + this.X[12], 3 );
            int num14 = this.RotateLeft( num10 + this.F( num13, num12, num11 ) + this.X[13], 7 );
            int num15 = this.RotateLeft( num11 + this.F( num14, num13, num12 ) + this.X[14], 11 );
            int num16 = this.RotateLeft( num12 + this.F( num15, num14, num13 ) + this.X[15], 19 );
            int num17 = this.RotateLeft( num13 + this.G( num16, num15, num14 ) + this.X[0] + 1518500249, 3 );
            int num18 = this.RotateLeft( num14 + this.G( num17, num16, num15 ) + this.X[4] + 1518500249, 5 );
            int num19 = this.RotateLeft( num15 + this.G( num18, num17, num16 ) + this.X[8] + 1518500249, 9 );
            int num20 = this.RotateLeft( num16 + this.G( num19, num18, num17 ) + this.X[12] + 1518500249, 13 );
            int num21 = this.RotateLeft( num17 + this.G( num20, num19, num18 ) + this.X[1] + 1518500249, 3 );
            int num22 = this.RotateLeft( num18 + this.G( num21, num20, num19 ) + this.X[5] + 1518500249, 5 );
            int num23 = this.RotateLeft( num19 + this.G( num22, num21, num20 ) + this.X[9] + 1518500249, 9 );
            int num24 = this.RotateLeft( num20 + this.G( num23, num22, num21 ) + this.X[13] + 1518500249, 13 );
            int num25 = this.RotateLeft( num21 + this.G( num24, num23, num22 ) + this.X[2] + 1518500249, 3 );
            int num26 = this.RotateLeft( num22 + this.G( num25, num24, num23 ) + this.X[6] + 1518500249, 5 );
            int num27 = this.RotateLeft( num23 + this.G( num26, num25, num24 ) + this.X[10] + 1518500249, 9 );
            int num28 = this.RotateLeft( num24 + this.G( num27, num26, num25 ) + this.X[14] + 1518500249, 13 );
            int num29 = this.RotateLeft( num25 + this.G( num28, num27, num26 ) + this.X[3] + 1518500249, 3 );
            int num30 = this.RotateLeft( num26 + this.G( num29, num28, num27 ) + this.X[7] + 1518500249, 5 );
            int num31 = this.RotateLeft( num27 + this.G( num30, num29, num28 ) + this.X[11] + 1518500249, 9 );
            int num32 = this.RotateLeft( num28 + this.G( num31, num30, num29 ) + this.X[15] + 1518500249, 13 );
            int num33 = this.RotateLeft( num29 + this.H( num32, num31, num30 ) + this.X[0] + 1859775393, 3 );
            int num34 = this.RotateLeft( num30 + this.H( num33, num32, num31 ) + this.X[8] + 1859775393, 9 );
            int num35 = this.RotateLeft( num31 + this.H( num34, num33, num32 ) + this.X[4] + 1859775393, 11 );
            int num36 = this.RotateLeft( num32 + this.H( num35, num34, num33 ) + this.X[12] + 1859775393, 15 );
            int num37 = this.RotateLeft( num33 + this.H( num36, num35, num34 ) + this.X[2] + 1859775393, 3 );
            int num38 = this.RotateLeft( num34 + this.H( num37, num36, num35 ) + this.X[10] + 1859775393, 9 );
            int num39 = this.RotateLeft( num35 + this.H( num38, num37, num36 ) + this.X[6] + 1859775393, 11 );
            int num40 = this.RotateLeft( num36 + this.H( num39, num38, num37 ) + this.X[14] + 1859775393, 15 );
            int num41 = this.RotateLeft( num37 + this.H( num40, num39, num38 ) + this.X[1] + 1859775393, 3 );
            int num42 = this.RotateLeft( num38 + this.H( num41, num40, num39 ) + this.X[9] + 1859775393, 9 );
            int num43 = this.RotateLeft( num39 + this.H( num42, num41, num40 ) + this.X[5] + 1859775393, 11 );
            int num44 = this.RotateLeft( num40 + this.H( num43, num42, num41 ) + this.X[13] + 1859775393, 15 );
            int num45 = this.RotateLeft( num41 + this.H( num44, num43, num42 ) + this.X[3] + 1859775393, 3 );
            int num46 = this.RotateLeft( num42 + this.H( num45, num44, num43 ) + this.X[11] + 1859775393, 9 );
            int u = this.RotateLeft( num43 + this.H( num46, num45, num44 ) + this.X[7] + 1859775393, 11 );
            int num47 = this.RotateLeft( num44 + this.H( u, num46, num45 ) + this.X[15] + 1859775393, 15 );
            this.H1 += num45;
            this.H2 += num47;
            this.H3 += u;
            this.H4 += num46;
            this.xOff = 0;
            for (int index = 0; index != this.X.Length; ++index)
                this.X[index] = 0;
        }

        public override IMemoable Copy() => new MD4Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (MD4Digest)other );
    }
}
