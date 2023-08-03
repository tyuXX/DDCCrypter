// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.ShortenedDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class ShortenedDigest : IDigest
    {
        private IDigest baseDigest;
        private int length;

        public ShortenedDigest( IDigest baseDigest, int length )
        {
            if (baseDigest == null)
                throw new ArgumentNullException( nameof( baseDigest ) );
            this.baseDigest = length <= baseDigest.GetDigestSize() ? baseDigest : throw new ArgumentException( "baseDigest output not large enough to support length" );
            this.length = length;
        }

        public string AlgorithmName => this.baseDigest.AlgorithmName + "(" + this.length * 8 + ")";

        public int GetDigestSize() => this.length;

        public void Update( byte input ) => this.baseDigest.Update( input );

        public void BlockUpdate( byte[] input, int inOff, int length ) => this.baseDigest.BlockUpdate( input, inOff, length );

        public int DoFinal( byte[] output, int outOff )
        {
            byte[] numArray = new byte[this.baseDigest.GetDigestSize()];
            this.baseDigest.DoFinal( numArray, 0 );
            Array.Copy( numArray, 0, output, outOff, this.length );
            return this.length;
        }

        public void Reset() => this.baseDigest.Reset();

        public int GetByteLength() => this.baseDigest.GetByteLength();
    }
}
