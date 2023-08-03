// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.X931SecureRandomBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Date;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class X931SecureRandomBuilder
    {
        private readonly SecureRandom mRandom;
        private IEntropySourceProvider mEntropySourceProvider;
        private byte[] mDateTimeVector;

        public X931SecureRandomBuilder()
          : this( new SecureRandom(), false )
        {
        }

        public X931SecureRandomBuilder( SecureRandom entropySource, bool predictionResistant )
        {
            this.mRandom = entropySource;
            this.mEntropySourceProvider = new BasicEntropySourceProvider( this.mRandom, predictionResistant );
        }

        public X931SecureRandomBuilder( IEntropySourceProvider entropySourceProvider )
        {
            this.mRandom = null;
            this.mEntropySourceProvider = entropySourceProvider;
        }

        public X931SecureRandomBuilder SetDateTimeVector( byte[] dateTimeVector )
        {
            this.mDateTimeVector = dateTimeVector;
            return this;
        }

        public X931SecureRandom Build( IBlockCipher engine, KeyParameter key, bool predictionResistant )
        {
            if (this.mDateTimeVector == null)
            {
                this.mDateTimeVector = new byte[engine.GetBlockSize()];
                Pack.UInt64_To_BE( (ulong)DateTimeUtilities.CurrentUnixMs(), this.mDateTimeVector, 0 );
            }
            engine.Init( true, key );
            return new X931SecureRandom( this.mRandom, new X931Rng( engine, this.mDateTimeVector, this.mEntropySourceProvider.Get( engine.GetBlockSize() * 8 ) ), predictionResistant );
        }
    }
}
