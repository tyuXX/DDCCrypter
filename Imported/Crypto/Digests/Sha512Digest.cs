// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Sha512Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Sha512Digest : LongDigest
    {
        private const int DigestLength = 64;

        public Sha512Digest()
        {
        }

        public Sha512Digest( Sha512Digest t )
          : base( t )
        {
        }

        public override string AlgorithmName => "SHA-512";

        public override int GetDigestSize() => 64;

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            Pack.UInt64_To_BE( this.H1, output, outOff );
            Pack.UInt64_To_BE( this.H2, output, outOff + 8 );
            Pack.UInt64_To_BE( this.H3, output, outOff + 16 );
            Pack.UInt64_To_BE( this.H4, output, outOff + 24 );
            Pack.UInt64_To_BE( this.H5, output, outOff + 32 );
            Pack.UInt64_To_BE( this.H6, output, outOff + 40 );
            Pack.UInt64_To_BE( this.H7, output, outOff + 48 );
            Pack.UInt64_To_BE( this.H8, output, outOff + 56 );
            this.Reset();
            return 64;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 7640891576956012808UL;
            this.H2 = 13503953896175478587UL;
            this.H3 = 4354685564936845355UL;
            this.H4 = 11912009170470909681UL;
            this.H5 = 5840696475078001361UL;
            this.H6 = 11170449401992604703UL;
            this.H7 = 2270897969802886507UL;
            this.H8 = 6620516959819538809UL;
        }

        public override IMemoable Copy() => new Sha512Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (LongDigest)other );
    }
}
