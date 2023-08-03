// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.BasicEntropySourceProvider
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class BasicEntropySourceProvider : IEntropySourceProvider
    {
        private readonly SecureRandom mSecureRandom;
        private readonly bool mPredictionResistant;

        public BasicEntropySourceProvider( SecureRandom secureRandom, bool isPredictionResistant )
        {
            this.mSecureRandom = secureRandom;
            this.mPredictionResistant = isPredictionResistant;
        }

        public IEntropySource Get( int bitsRequired ) => new BasicEntropySourceProvider.BasicEntropySource( this.mSecureRandom, this.mPredictionResistant, bitsRequired );

        private class BasicEntropySource : IEntropySource
        {
            private readonly SecureRandom mSecureRandom;
            private readonly bool mPredictionResistant;
            private readonly int mEntropySize;

            internal BasicEntropySource(
              SecureRandom secureRandom,
              bool predictionResistant,
              int entropySize )
            {
                this.mSecureRandom = secureRandom;
                this.mPredictionResistant = predictionResistant;
                this.mEntropySize = entropySize;
            }

            bool IEntropySource.IsPredictionResistant => this.mPredictionResistant;

            byte[] IEntropySource.GetEntropy() => SecureRandom.GetNextBytes( this.mSecureRandom, (this.mEntropySize + 7) / 8 );

            int IEntropySource.EntropySize => this.mEntropySize;
        }
    }
}
