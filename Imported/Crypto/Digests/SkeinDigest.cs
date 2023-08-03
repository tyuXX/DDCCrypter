// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.SkeinDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class SkeinDigest : IDigest, IMemoable
    {
        public const int SKEIN_256 = 256;
        public const int SKEIN_512 = 512;
        public const int SKEIN_1024 = 1024;
        private readonly SkeinEngine engine;

        public SkeinDigest( int stateSizeBits, int digestSizeBits )
        {
            this.engine = new SkeinEngine( stateSizeBits, digestSizeBits );
            this.Init( null );
        }

        public SkeinDigest( SkeinDigest digest ) => this.engine = new SkeinEngine( digest.engine );

        public void Reset( IMemoable other ) => this.engine.Reset( ((SkeinDigest)other).engine );

        public IMemoable Copy() => new SkeinDigest( this );

        public string AlgorithmName => "Skein-" + (this.engine.BlockSize * 8) + "-" + (this.engine.OutputSize * 8);

        public int GetDigestSize() => this.engine.OutputSize;

        public int GetByteLength() => this.engine.BlockSize;

        public void Init( SkeinParameters parameters ) => this.engine.Init( parameters );

        public void Reset() => this.engine.Reset();

        public void Update( byte inByte ) => this.engine.Update( inByte );

        public void BlockUpdate( byte[] inBytes, int inOff, int len ) => this.engine.Update( inBytes, inOff, len );

        public int DoFinal( byte[] outBytes, int outOff ) => this.engine.DoFinal( outBytes, outOff );
    }
}
