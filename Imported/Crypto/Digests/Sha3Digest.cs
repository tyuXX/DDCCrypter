// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Sha3Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Sha3Digest : KeccakDigest
    {
        private static int CheckBitLength( int bitLength )
        {
            switch (bitLength)
            {
                case 224:
                case 256:
                case 384:
                case 512:
                    return bitLength;
                default:
                    throw new ArgumentException( bitLength.ToString() + " not supported for SHA-3", nameof( bitLength ) );
            }
        }

        public Sha3Digest()
          : this( 256 )
        {
        }

        public Sha3Digest( int bitLength )
          : base( CheckBitLength( bitLength ) )
        {
        }

        public Sha3Digest( Sha3Digest source )
          : base( source )
        {
        }

        public override string AlgorithmName => "SHA3-" + fixedOutputLength;

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Absorb( new byte[1] { 2 }, 0, 2L );
            return base.DoFinal( output, outOff );
        }

        protected override int DoFinal( byte[] output, int outOff, byte partialByte, int partialBits )
        {
            if (partialBits < 0 || partialBits > 7)
                throw new ArgumentException( "must be in the range [0,7]", nameof( partialBits ) );
            int partialByte1 = (partialByte & ((1 << partialBits) - 1)) | (2 << partialBits);
            int partialBits1 = partialBits + 2;
            if (partialBits1 >= 8)
            {
                this.oneByte[0] = (byte)partialByte1;
                this.Absorb( this.oneByte, 0, 8L );
                partialBits1 -= 8;
                partialByte1 >>= 8;
            }
            return base.DoFinal( output, outOff, (byte)partialByte1, partialBits1 );
        }

        public override IMemoable Copy() => new Sha3Digest( this );
    }
}
