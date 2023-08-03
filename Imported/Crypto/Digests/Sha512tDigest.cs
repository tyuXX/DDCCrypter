// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Sha512tDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Sha512tDigest : LongDigest
    {
        private const ulong A5 = 11936128518282651045;
        private readonly int digestLength;
        private ulong H1t;
        private ulong H2t;
        private ulong H3t;
        private ulong H4t;
        private ulong H5t;
        private ulong H6t;
        private ulong H7t;
        private ulong H8t;

        public Sha512tDigest( int bitLength )
        {
            if (bitLength >= 512)
                throw new ArgumentException( "cannot be >= 512", nameof( bitLength ) );
            if (bitLength % 8 != 0)
                throw new ArgumentException( "needs to be a multiple of 8", nameof( bitLength ) );
            if (bitLength == 384)
                throw new ArgumentException( "cannot be 384 use SHA384 instead", nameof( bitLength ) );
            this.digestLength = bitLength / 8;
            this.tIvGenerate( this.digestLength * 8 );
            this.Reset();
        }

        public Sha512tDigest( Sha512tDigest t )
          : base( t )
        {
            this.digestLength = t.digestLength;
            this.Reset( t );
        }

        public override string AlgorithmName => "SHA-512/" + (this.digestLength * 8);

        public override int GetDigestSize() => this.digestLength;

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            UInt64_To_BE( this.H1, output, outOff, this.digestLength );
            UInt64_To_BE( this.H2, output, outOff + 8, this.digestLength - 8 );
            UInt64_To_BE( this.H3, output, outOff + 16, this.digestLength - 16 );
            UInt64_To_BE( this.H4, output, outOff + 24, this.digestLength - 24 );
            UInt64_To_BE( this.H5, output, outOff + 32, this.digestLength - 32 );
            UInt64_To_BE( this.H6, output, outOff + 40, this.digestLength - 40 );
            UInt64_To_BE( this.H7, output, outOff + 48, this.digestLength - 48 );
            UInt64_To_BE( this.H8, output, outOff + 56, this.digestLength - 56 );
            this.Reset();
            return this.digestLength;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = this.H1t;
            this.H2 = this.H2t;
            this.H3 = this.H3t;
            this.H4 = this.H4t;
            this.H5 = this.H5t;
            this.H6 = this.H6t;
            this.H7 = this.H7t;
            this.H8 = this.H8t;
        }

        private void tIvGenerate( int bitLength )
        {
            this.H1 = 14964410163792538797UL;
            this.H2 = 2216346199247487646UL;
            this.H3 = 11082046791023156622UL;
            this.H4 = 65953792586715988UL;
            this.H5 = 17630457682085488500UL;
            this.H6 = 4512832404995164602UL;
            this.H7 = 13413544941332994254UL;
            this.H8 = 18322165818757711068UL;
            this.Update( 83 );
            this.Update( 72 );
            this.Update( 65 );
            this.Update( 45 );
            this.Update( 53 );
            this.Update( 49 );
            this.Update( 50 );
            this.Update( 47 );
            if (bitLength > 100)
            {
                this.Update( (byte)((bitLength / 100) + 48) );
                bitLength %= 100;
                this.Update( (byte)((bitLength / 10) + 48) );
                bitLength %= 10;
                this.Update( (byte)(bitLength + 48) );
            }
            else if (bitLength > 10)
            {
                this.Update( (byte)((bitLength / 10) + 48) );
                bitLength %= 10;
                this.Update( (byte)(bitLength + 48) );
            }
            else
                this.Update( (byte)(bitLength + 48) );
            this.Finish();
            this.H1t = this.H1;
            this.H2t = this.H2;
            this.H3t = this.H3;
            this.H4t = this.H4;
            this.H5t = this.H5;
            this.H6t = this.H6;
            this.H7t = this.H7;
            this.H8t = this.H8;
        }

        private static void UInt64_To_BE( ulong n, byte[] bs, int off, int max )
        {
            if (max <= 0)
                return;
            UInt32_To_BE( (uint)(n >> 32), bs, off, max );
            if (max <= 4)
                return;
            UInt32_To_BE( (uint)n, bs, off + 4, max - 4 );
        }

        private static void UInt32_To_BE( uint n, byte[] bs, int off, int max )
        {
            int num1 = System.Math.Min( 4, max );
            while (--num1 >= 0)
            {
                int num2 = 8 * (3 - num1);
                bs[off + num1] = (byte)(n >> num2);
            }
        }

        public override IMemoable Copy() => new Sha512tDigest( this );

        public override void Reset( IMemoable other )
        {
            Sha512tDigest t = (Sha512tDigest)other;
            if (this.digestLength != t.digestLength)
                throw new MemoableResetException( "digestLength inappropriate in other" );
            this.CopyIn( t );
            this.H1t = t.H1t;
            this.H2t = t.H2t;
            this.H3t = t.H3t;
            this.H4t = t.H4t;
            this.H5t = t.H5t;
            this.H6t = t.H6t;
            this.H7t = t.H7t;
            this.H8t = t.H8t;
        }
    }
}
