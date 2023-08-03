// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.SP800SecureRandom
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Prng.Drbg;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class SP800SecureRandom : SecureRandom
    {
        private readonly IDrbgProvider mDrbgProvider;
        private readonly bool mPredictionResistant;
        private readonly SecureRandom mRandomSource;
        private readonly IEntropySource mEntropySource;
        private ISP80090Drbg mDrbg;

        internal SP800SecureRandom(
          SecureRandom randomSource,
          IEntropySource entropySource,
          IDrbgProvider drbgProvider,
          bool predictionResistant )
          : base( (IRandomGenerator)null )
        {
            this.mRandomSource = randomSource;
            this.mEntropySource = entropySource;
            this.mDrbgProvider = drbgProvider;
            this.mPredictionResistant = predictionResistant;
        }

        public override void SetSeed( byte[] seed )
        {
            lock (this)
            {
                if (this.mRandomSource == null)
                    return;
                this.mRandomSource.SetSeed( seed );
            }
        }

        public override void SetSeed( long seed )
        {
            lock (this)
            {
                if (this.mRandomSource == null)
                    return;
                this.mRandomSource.SetSeed( seed );
            }
        }

        public override void NextBytes( byte[] bytes )
        {
            lock (this)
            {
                if (this.mDrbg == null)
                    this.mDrbg = this.mDrbgProvider.Get( this.mEntropySource );
                if (this.mDrbg.Generate( bytes, null, this.mPredictionResistant ) >= 0)
                    return;
                this.mDrbg.Reseed( null );
                this.mDrbg.Generate( bytes, null, this.mPredictionResistant );
            }
        }

        public override void NextBytes( byte[] buf, int off, int len )
        {
            byte[] numArray = new byte[len];
            this.NextBytes( numArray );
            Array.Copy( numArray, 0, buf, off, len );
        }

        public override byte[] GenerateSeed( int numBytes ) => EntropyUtilities.GenerateSeed( this.mEntropySource, numBytes );
    }
}
