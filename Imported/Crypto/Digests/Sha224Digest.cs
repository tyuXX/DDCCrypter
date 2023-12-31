﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Sha224Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Sha224Digest : GeneralDigest
    {
        private const int DigestLength = 28;
        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint H5;
        private uint H6;
        private uint H7;
        private uint H8;
        private uint[] X = new uint[64];
        private int xOff;
        internal static readonly uint[] K = new uint[64]
        {
      1116352408U,
      1899447441U,
      3049323471U,
      3921009573U,
      961987163U,
      1508970993U,
      2453635748U,
      2870763221U,
      3624381080U,
      310598401U,
      607225278U,
      1426881987U,
      1925078388U,
      2162078206U,
      2614888103U,
      3248222580U,
      3835390401U,
      4022224774U,
      264347078U,
      604807628U,
      770255983U,
      1249150122U,
      1555081692U,
      1996064986U,
      2554220882U,
      2821834349U,
      2952996808U,
      3210313671U,
      3336571891U,
      3584528711U,
      113926993U,
      338241895U,
      666307205U,
      773529912U,
      1294757372U,
      1396182291U,
      1695183700U,
      1986661051U,
      2177026350U,
      2456956037U,
      2730485921U,
      2820302411U,
      3259730800U,
      3345764771U,
      3516065817U,
      3600352804U,
      4094571909U,
      275423344U,
      430227734U,
      506948616U,
      659060556U,
      883997877U,
      958139571U,
      1322822218U,
      1537002063U,
      1747873779U,
      1955562222U,
      2024104815U,
      2227730452U,
      2361852424U,
      2428436474U,
      2756734187U,
      3204031479U,
      3329325298U
        };

        public Sha224Digest() => this.Reset();

        public Sha224Digest( Sha224Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( Sha224Digest t )
        {
            this.CopyIn( (GeneralDigest)t );
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            this.H6 = t.H6;
            this.H7 = t.H7;
            this.H8 = t.H8;
            Array.Copy( t.X, 0, X, 0, t.X.Length );
            this.xOff = t.xOff;
        }

        public override string AlgorithmName => "SHA-224";

        public override int GetDigestSize() => 28;

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
            Pack.UInt32_To_BE( this.H6, output, outOff + 20 );
            Pack.UInt32_To_BE( this.H7, output, outOff + 24 );
            this.Reset();
            return 28;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 3238371032U;
            this.H2 = 914150663U;
            this.H3 = 812702999U;
            this.H4 = 4144912697U;
            this.H5 = 4290775857U;
            this.H6 = 1750603025U;
            this.H7 = 1694076839U;
            this.H8 = 3204075428U;
            this.xOff = 0;
            Array.Clear( X, 0, this.X.Length );
        }

        internal override void ProcessBlock()
        {
            for (int index = 16; index <= 63; ++index)
                this.X[index] = Theta1( this.X[index - 2] ) + this.X[index - 7] + Theta0( this.X[index - 15] ) + this.X[index - 16];
            uint num1 = this.H1;
            uint num2 = this.H2;
            uint num3 = this.H3;
            uint num4 = this.H4;
            uint num5 = this.H5;
            uint num6 = this.H6;
            uint num7 = this.H7;
            uint num8 = this.H8;
            int index1 = 0;
            for (int index2 = 0; index2 < 8; ++index2)
            {
                uint num9 = num8 + Sum1( num5 ) + Ch( num5, num6, num7 ) + K[index1] + this.X[index1];
                uint num10 = num4 + num9;
                uint num11 = num9 + Sum0( num1 ) + Maj( num1, num2, num3 );
                int index3 = index1 + 1;
                uint num12 = num7 + Sum1( num10 ) + Ch( num10, num5, num6 ) + K[index3] + this.X[index3];
                uint num13 = num3 + num12;
                uint num14 = num12 + Sum0( num11 ) + Maj( num11, num1, num2 );
                int index4 = index3 + 1;
                uint num15 = num6 + Sum1( num13 ) + Ch( num13, num10, num5 ) + K[index4] + this.X[index4];
                uint num16 = num2 + num15;
                uint num17 = num15 + Sum0( num14 ) + Maj( num14, num11, num1 );
                int index5 = index4 + 1;
                uint num18 = num5 + Sum1( num16 ) + Ch( num16, num13, num10 ) + K[index5] + this.X[index5];
                uint num19 = num1 + num18;
                uint num20 = num18 + Sum0( num17 ) + Maj( num17, num14, num11 );
                int index6 = index5 + 1;
                uint num21 = num10 + Sum1( num19 ) + Ch( num19, num16, num13 ) + K[index6] + this.X[index6];
                num8 = num11 + num21;
                num4 = num21 + Sum0( num20 ) + Maj( num20, num17, num14 );
                int index7 = index6 + 1;
                uint num22 = num13 + Sum1( num8 ) + Ch( num8, num19, num16 ) + K[index7] + this.X[index7];
                num7 = num14 + num22;
                num3 = num22 + Sum0( num4 ) + Maj( num4, num20, num17 );
                int index8 = index7 + 1;
                uint num23 = num16 + Sum1( num7 ) + Ch( num7, num8, num19 ) + K[index8] + this.X[index8];
                num6 = num17 + num23;
                num2 = num23 + Sum0( num3 ) + Maj( num3, num4, num20 );
                int index9 = index8 + 1;
                uint num24 = num19 + Sum1( num6 ) + Ch( num6, num7, num8 ) + K[index9] + this.X[index9];
                num5 = num20 + num24;
                num1 = num24 + Sum0( num2 ) + Maj( num2, num3, num4 );
                index1 = index9 + 1;
            }
            this.H1 += num1;
            this.H2 += num2;
            this.H3 += num3;
            this.H4 += num4;
            this.H5 += num5;
            this.H6 += num6;
            this.H7 += num7;
            this.H8 += num8;
            this.xOff = 0;
            Array.Clear( X, 0, 16 );
        }

        private static uint Ch( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) ^ (~(int)x & (int)z));

        private static uint Maj( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) ^ ((int)x & (int)z) ^ ((int)y & (int)z));

        private static uint Sum0( uint x ) => (uint)(((int)(x >> 2) | ((int)x << 30)) ^ ((int)(x >> 13) | ((int)x << 19)) ^ ((int)(x >> 22) | ((int)x << 10)));

        private static uint Sum1( uint x ) => (uint)(((int)(x >> 6) | ((int)x << 26)) ^ ((int)(x >> 11) | ((int)x << 21)) ^ ((int)(x >> 25) | ((int)x << 7)));

        private static uint Theta0( uint x ) => (uint)(((int)(x >> 7) | ((int)x << 25)) ^ ((int)(x >> 18) | ((int)x << 14))) ^ (x >> 3);

        private static uint Theta1( uint x ) => (uint)(((int)(x >> 17) | ((int)x << 15)) ^ ((int)(x >> 19) | ((int)x << 13))) ^ (x >> 10);

        public override IMemoable Copy() => new Sha224Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (Sha224Digest)other );
    }
}
