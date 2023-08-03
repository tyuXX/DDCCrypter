// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.SM3Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class SM3Digest : GeneralDigest
    {
        private const int DIGEST_LENGTH = 32;
        private const int BLOCK_SIZE = 16;
        private uint[] V = new uint[8];
        private uint[] inwords = new uint[16];
        private int xOff;
        private uint[] W = new uint[68];
        private uint[] W1 = new uint[64];
        private static readonly uint[] T = new uint[64];

        static SM3Digest()
        {
            for (int index = 0; index < 16; ++index)
            {
                uint num = 2043430169;
                T[index] = (num << index) | (num >> (32 - index));
            }
            for (int index = 16; index < 64; ++index)
            {
                int num1 = index % 32;
                uint num2 = 2055708042;
                T[index] = (num2 << num1) | (num2 >> (32 - num1));
            }
        }

        public SM3Digest() => this.Reset();

        public SM3Digest( SM3Digest t )
          : base( t )
        {
            this.CopyIn( t );
        }

        private void CopyIn( SM3Digest t )
        {
            Array.Copy( t.V, 0, V, 0, this.V.Length );
            Array.Copy( t.inwords, 0, inwords, 0, this.inwords.Length );
            this.xOff = t.xOff;
        }

        public override string AlgorithmName => "SM3";

        public override int GetDigestSize() => 32;

        public override IMemoable Copy() => new SM3Digest( this );

        public override void Reset( IMemoable other )
        {
            SM3Digest t = (SM3Digest)other;
            this.CopyIn( (GeneralDigest)t );
            this.CopyIn( t );
        }

        public override void Reset()
        {
            base.Reset();
            this.V[0] = 1937774191U;
            this.V[1] = 1226093241U;
            this.V[2] = 388252375U;
            this.V[3] = 3666478592U;
            this.V[4] = 2842636476U;
            this.V[5] = 372324522U;
            this.V[6] = 3817729613U;
            this.V[7] = 2969243214U;
            this.xOff = 0;
        }

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            Pack.UInt32_To_BE( this.V[0], output, outOff );
            Pack.UInt32_To_BE( this.V[1], output, outOff + 4 );
            Pack.UInt32_To_BE( this.V[2], output, outOff + 8 );
            Pack.UInt32_To_BE( this.V[3], output, outOff + 12 );
            Pack.UInt32_To_BE( this.V[4], output, outOff + 16 );
            Pack.UInt32_To_BE( this.V[5], output, outOff + 20 );
            Pack.UInt32_To_BE( this.V[6], output, outOff + 24 );
            Pack.UInt32_To_BE( this.V[7], output, outOff + 28 );
            this.Reset();
            return 32;
        }

        internal override void ProcessWord( byte[] input, int inOff )
        {
            this.inwords[this.xOff] = Pack.BE_To_UInt32( input, inOff );
            ++this.xOff;
            if (this.xOff < 16)
                return;
            this.ProcessBlock();
        }

        internal override void ProcessLength( long bitLength )
        {
            if (this.xOff > 14)
            {
                this.inwords[this.xOff] = 0U;
                ++this.xOff;
                this.ProcessBlock();
            }
            for (; this.xOff < 14; ++this.xOff)
                this.inwords[this.xOff] = 0U;
            this.inwords[this.xOff++] = (uint)(bitLength >> 32);
            this.inwords[this.xOff++] = (uint)bitLength;
        }

        private uint P0( uint x )
        {
            uint num1 = (x << 9) | (x >> 23);
            uint num2 = (x << 17) | (x >> 15);
            return x ^ num1 ^ num2;
        }

        private uint P1( uint x )
        {
            uint num1 = (x << 15) | (x >> 17);
            uint num2 = (x << 23) | (x >> 9);
            return x ^ num1 ^ num2;
        }

        private uint FF0( uint x, uint y, uint z ) => x ^ y ^ z;

        private uint FF1( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) | ((int)x & (int)z) | ((int)y & (int)z));

        private uint GG0( uint x, uint y, uint z ) => x ^ y ^ z;

        private uint GG1( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) | (~(int)x & (int)z));

        internal override void ProcessBlock()
        {
            for (int index = 0; index < 16; ++index)
                this.W[index] = this.inwords[index];
            for (int index = 16; index < 68; ++index)
            {
                uint num1 = this.W[index - 3];
                uint num2 = (num1 << 15) | (num1 >> 17);
                uint num3 = this.W[index - 13];
                uint num4 = (num3 << 7) | (num3 >> 25);
                this.W[index] = this.P1( this.W[index - 16] ^ this.W[index - 9] ^ num2 ) ^ num4 ^ this.W[index - 6];
            }
            for (int index = 0; index < 64; ++index)
                this.W1[index] = this.W[index] ^ this.W[index + 4];
            uint x1 = this.V[0];
            uint y1 = this.V[1];
            uint z1 = this.V[2];
            uint num5 = this.V[3];
            uint x2 = this.V[4];
            uint y2 = this.V[5];
            uint z2 = this.V[6];
            uint num6 = this.V[7];
            for (int index = 0; index < 16; ++index)
            {
                uint num7 = (x1 << 12) | (x1 >> 20);
                uint num8 = num7 + x2 + T[index];
                uint num9 = (num8 << 7) | (num8 >> 25);
                uint num10 = num9 ^ num7;
                uint num11 = this.FF0( x1, y1, z1 ) + num5 + num10 + this.W1[index];
                uint x3 = this.GG0( x2, y2, z2 ) + num6 + num9 + this.W[index];
                num5 = z1;
                z1 = (y1 << 9) | (y1 >> 23);
                y1 = x1;
                x1 = num11;
                num6 = z2;
                z2 = (y2 << 19) | (y2 >> 13);
                y2 = x2;
                x2 = this.P0( x3 );
            }
            for (int index = 16; index < 64; ++index)
            {
                uint num12 = (x1 << 12) | (x1 >> 20);
                uint num13 = num12 + x2 + T[index];
                uint num14 = (num13 << 7) | (num13 >> 25);
                uint num15 = num14 ^ num12;
                uint num16 = this.FF1( x1, y1, z1 ) + num5 + num15 + this.W1[index];
                uint x4 = this.GG1( x2, y2, z2 ) + num6 + num14 + this.W[index];
                num5 = z1;
                z1 = (y1 << 9) | (y1 >> 23);
                y1 = x1;
                x1 = num16;
                num6 = z2;
                z2 = (y2 << 19) | (y2 >> 13);
                y2 = x2;
                x2 = this.P0( x4 );
            }
            uint[] v1;
            (v1 = this.V)[0] = v1[0] ^ x1;
            uint[] v2;
            (v2 = this.V)[1] = v2[1] ^ y1;
            uint[] v3;
            (v3 = this.V)[2] = v3[2] ^ z1;
            uint[] v4;
            (v4 = this.V)[3] = v4[3] ^ num5;
            uint[] v5;
            (v5 = this.V)[4] = v5[4] ^ x2;
            uint[] v6;
            (v6 = this.V)[5] = v6[5] ^ y2;
            uint[] v7;
            (v7 = this.V)[6] = v7[6] ^ z2;
            uint[] v8;
            (v8 = this.V)[7] = v8[7] ^ num6;
            this.xOff = 0;
        }
    }
}
