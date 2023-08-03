// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.X931SecureRandom
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class X931SecureRandom : SecureRandom
    {
        private readonly bool mPredictionResistant;
        private readonly SecureRandom mRandomSource;
        private readonly X931Rng mDrbg;

        internal X931SecureRandom( SecureRandom randomSource, X931Rng drbg, bool predictionResistant )
          : base( (IRandomGenerator)null )
        {
            this.mRandomSource = randomSource;
            this.mDrbg = drbg;
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
                if (this.mDrbg.Generate( bytes, this.mPredictionResistant ) >= 0)
                    return;
                this.mDrbg.Reseed();
                this.mDrbg.Generate( bytes, this.mPredictionResistant );
            }
        }

        public override void NextBytes( byte[] buf, int off, int len )
        {
            byte[] numArray = new byte[len];
            this.NextBytes( numArray );
            Array.Copy( numArray, 0, buf, off, len );
        }

        public override byte[] GenerateSeed( int numBytes ) => EntropyUtilities.GenerateSeed( this.mDrbg.EntropySource, numBytes );
    }
}
