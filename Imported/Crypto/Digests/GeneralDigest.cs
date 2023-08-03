// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.GeneralDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
    public abstract class GeneralDigest : IDigest, IMemoable
    {
        private const int BYTE_LENGTH = 64;
        private byte[] xBuf;
        private int xBufOff;
        private long byteCount;

        internal GeneralDigest() => this.xBuf = new byte[4];

        internal GeneralDigest( GeneralDigest t )
        {
            this.xBuf = new byte[t.xBuf.Length];
            this.CopyIn( t );
        }

        protected void CopyIn( GeneralDigest t )
        {
            Array.Copy( t.xBuf, 0, xBuf, 0, t.xBuf.Length );
            this.xBufOff = t.xBufOff;
            this.byteCount = t.byteCount;
        }

        public void Update( byte input )
        {
            this.xBuf[this.xBufOff++] = input;
            if (this.xBufOff == this.xBuf.Length)
            {
                this.ProcessWord( this.xBuf, 0 );
                this.xBufOff = 0;
            }
            ++this.byteCount;
        }

        public void BlockUpdate( byte[] input, int inOff, int length )
        {
            length = System.Math.Max( 0, length );
            int num = 0;
            if (this.xBufOff != 0)
            {
                while (num < length)
                {
                    this.xBuf[this.xBufOff++] = input[inOff + num++];
                    if (this.xBufOff == 4)
                    {
                        this.ProcessWord( this.xBuf, 0 );
                        this.xBufOff = 0;
                        break;
                    }
                }
            }
            for (int index = ((length - num) & -4) + num; num < index; num += 4)
                this.ProcessWord( input, inOff + num );
            while (num < length)
                this.xBuf[this.xBufOff++] = input[inOff + num++];
            this.byteCount += length;
        }

        public void Finish()
        {
            long bitLength = this.byteCount << 3;
            this.Update( 128 );
            while (this.xBufOff != 0)
                this.Update( 0 );
            this.ProcessLength( bitLength );
            this.ProcessBlock();
        }

        public virtual void Reset()
        {
            this.byteCount = 0L;
            this.xBufOff = 0;
            Array.Clear( xBuf, 0, this.xBuf.Length );
        }

        public int GetByteLength() => 64;

        internal abstract void ProcessWord( byte[] input, int inOff );

        internal abstract void ProcessLength( long bitLength );

        internal abstract void ProcessBlock();

        public abstract string AlgorithmName { get; }

        public abstract int GetDigestSize();

        public abstract int DoFinal( byte[] output, int outOff );

        public abstract IMemoable Copy();

        public abstract void Reset( IMemoable t );
    }
}
