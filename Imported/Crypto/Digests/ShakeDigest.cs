// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.ShakeDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class ShakeDigest : KeccakDigest, IXof, IDigest
    {
        private static int CheckBitLength( int bitLength )
        {
            switch (bitLength)
            {
                case 128:
                case 256:
                    return bitLength;
                default:
                    throw new ArgumentException( bitLength.ToString() + " not supported for SHAKE", nameof( bitLength ) );
            }
        }

        public ShakeDigest()
          : this( 128 )
        {
        }

        public ShakeDigest( int bitLength )
          : base( CheckBitLength( bitLength ) )
        {
        }

        public ShakeDigest( ShakeDigest source )
          : base( source )
        {
        }

        public override string AlgorithmName => "SHAKE" + fixedOutputLength;

        public override int DoFinal( byte[] output, int outOff ) => this.DoFinal( output, outOff, this.GetDigestSize() );

        public virtual int DoFinal( byte[] output, int outOff, int outLen )
        {
            this.Absorb( new byte[1] { 15 }, 0, 4L );
            this.Squeeze( output, outOff, outLen * 8L );
            this.Reset();
            return outLen;
        }

        protected override int DoFinal( byte[] output, int outOff, byte partialByte, int partialBits ) => this.DoFinal( output, outOff, this.GetDigestSize(), partialByte, partialBits );

        protected virtual int DoFinal(
          byte[] output,
          int outOff,
          int outLen,
          byte partialByte,
          int partialBits )
        {
            if (partialBits < 0 || partialBits > 7)
                throw new ArgumentException( "must be in the range [0,7]", nameof( partialBits ) );
            int num = (partialByte & ((1 << partialBits) - 1)) | (15 << partialBits);
            int databitlen = partialBits + 4;
            if (databitlen >= 8)
            {
                this.oneByte[0] = (byte)num;
                this.Absorb( this.oneByte, 0, 8L );
                databitlen -= 8;
                num >>= 8;
            }
            if (databitlen > 0)
            {
                this.oneByte[0] = (byte)num;
                this.Absorb( this.oneByte, 0, databitlen );
            }
            this.Squeeze( output, outOff, outLen * 8L );
            this.Reset();
            return outLen;
        }

        public override IMemoable Copy() => new ShakeDigest( this );
    }
}
