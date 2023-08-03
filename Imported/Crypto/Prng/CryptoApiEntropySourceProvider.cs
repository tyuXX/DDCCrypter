// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.CryptoApiEntropySourceProvider
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Security.Cryptography;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class CryptoApiEntropySourceProvider : IEntropySourceProvider
    {
        private readonly RandomNumberGenerator mRng;
        private readonly bool mPredictionResistant;

        public CryptoApiEntropySourceProvider()
          : this( new RNGCryptoServiceProvider(), true )
        {
        }

        public CryptoApiEntropySourceProvider( RandomNumberGenerator rng, bool isPredictionResistant )
        {
            this.mRng = rng != null ? rng : throw new ArgumentNullException( nameof( rng ) );
            this.mPredictionResistant = isPredictionResistant;
        }

        public IEntropySource Get( int bitsRequired ) => new CryptoApiEntropySourceProvider.CryptoApiEntropySource( this.mRng, this.mPredictionResistant, bitsRequired );

        private class CryptoApiEntropySource : IEntropySource
        {
            private readonly RandomNumberGenerator mRng;
            private readonly bool mPredictionResistant;
            private readonly int mEntropySize;

            internal CryptoApiEntropySource(
              RandomNumberGenerator rng,
              bool predictionResistant,
              int entropySize )
            {
                this.mRng = rng;
                this.mPredictionResistant = predictionResistant;
                this.mEntropySize = entropySize;
            }

            bool IEntropySource.IsPredictionResistant => this.mPredictionResistant;

            byte[] IEntropySource.GetEntropy()
            {
                byte[] data = new byte[(this.mEntropySize + 7) / 8];
                this.mRng.GetBytes( data );
                return data;
            }

            int IEntropySource.EntropySize => this.mEntropySize;
        }
    }
}
