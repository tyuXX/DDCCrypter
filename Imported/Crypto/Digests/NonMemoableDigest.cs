// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.NonMemoableDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class NonMemoableDigest : IDigest
    {
        protected readonly IDigest mBaseDigest;

        public NonMemoableDigest( IDigest baseDigest ) => this.mBaseDigest = baseDigest != null ? baseDigest : throw new ArgumentNullException( nameof( baseDigest ) );

        public virtual string AlgorithmName => this.mBaseDigest.AlgorithmName;

        public virtual int GetDigestSize() => this.mBaseDigest.GetDigestSize();

        public virtual void Update( byte input ) => this.mBaseDigest.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int len ) => this.mBaseDigest.BlockUpdate( input, inOff, len );

        public virtual int DoFinal( byte[] output, int outOff ) => this.mBaseDigest.DoFinal( output, outOff );

        public virtual void Reset() => this.mBaseDigest.Reset();

        public virtual int GetByteLength() => this.mBaseDigest.GetByteLength();
    }
}
