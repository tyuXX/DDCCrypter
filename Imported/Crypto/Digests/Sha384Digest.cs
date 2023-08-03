// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Sha384Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Sha384Digest : LongDigest
    {
        private const int DigestLength = 48;

        public Sha384Digest()
        {
        }

        public Sha384Digest( Sha384Digest t )
          : base( t )
        {
        }

        public override string AlgorithmName => "SHA-384";

        public override int GetDigestSize() => 48;

        public override int DoFinal( byte[] output, int outOff )
        {
            this.Finish();
            Pack.UInt64_To_BE( this.H1, output, outOff );
            Pack.UInt64_To_BE( this.H2, output, outOff + 8 );
            Pack.UInt64_To_BE( this.H3, output, outOff + 16 );
            Pack.UInt64_To_BE( this.H4, output, outOff + 24 );
            Pack.UInt64_To_BE( this.H5, output, outOff + 32 );
            Pack.UInt64_To_BE( this.H6, output, outOff + 40 );
            this.Reset();
            return 48;
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 14680500436340154072UL;
            this.H2 = 7105036623409894663UL;
            this.H3 = 10473403895298186519UL;
            this.H4 = 1526699215303891257UL;
            this.H5 = 7436329637833083697UL;
            this.H6 = 10282925794625328401UL;
            this.H7 = 15784041429090275239UL;
            this.H8 = 5167115440072839076UL;
        }

        public override IMemoable Copy() => new Sha384Digest( this );

        public override void Reset( IMemoable other ) => this.CopyIn( (LongDigest)other );
    }
}
