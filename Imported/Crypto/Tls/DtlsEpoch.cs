// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsEpoch
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DtlsEpoch
    {
        private readonly DtlsReplayWindow mReplayWindow = new();
        private readonly int mEpoch;
        private readonly TlsCipher mCipher;
        private long mSequenceNumber = 0;

        internal DtlsEpoch( int epoch, TlsCipher cipher )
        {
            if (epoch < 0)
                throw new ArgumentException( "must be >= 0", nameof( epoch ) );
            if (cipher == null)
                throw new ArgumentNullException( nameof( cipher ) );
            this.mEpoch = epoch;
            this.mCipher = cipher;
        }

        internal long AllocateSequenceNumber() => this.mSequenceNumber++;

        internal TlsCipher Cipher => this.mCipher;

        internal int Epoch => this.mEpoch;

        internal DtlsReplayWindow ReplayWindow => this.mReplayWindow;

        internal long SequenceNumber => this.mSequenceNumber;
    }
}
