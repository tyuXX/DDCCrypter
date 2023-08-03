// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsReplayWindow
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DtlsReplayWindow
    {
        private const long VALID_SEQ_MASK = 281474976710655;
        private const long WINDOW_SIZE = 64;
        private long mLatestConfirmedSeq = -1;
        private long mBitmap = 0;

        internal bool ShouldDiscard( long seq )
        {
            if ((seq & 281474976710655L) != seq)
                return true;
            if (seq <= this.mLatestConfirmedSeq)
            {
                long num = this.mLatestConfirmedSeq - seq;
                if (num >= 64L || (this.mBitmap & (1L << (int)num)) != 0L)
                    return true;
            }
            return false;
        }

        internal void ReportAuthenticated( long seq )
        {
            if ((seq & 281474976710655L) != seq)
                throw new ArgumentException( "out of range", nameof( seq ) );
            if (seq <= this.mLatestConfirmedSeq)
            {
                long num = this.mLatestConfirmedSeq - seq;
                if (num >= 64L)
                    return;
                this.mBitmap |= 1L << (int)num;
            }
            else
            {
                long num = seq - this.mLatestConfirmedSeq;
                if (num >= 64L)
                {
                    this.mBitmap = 1L;
                }
                else
                {
                    this.mBitmap <<= (int)num;
                    this.mBitmap |= 1L;
                }
                this.mLatestConfirmedSeq = seq;
            }
        }

        internal void Reset()
        {
            this.mLatestConfirmedSeq = -1L;
            this.mBitmap = 0L;
        }
    }
}
