// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DsaValidationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DsaValidationParameters
    {
        private readonly byte[] seed;
        private readonly int counter;
        private readonly int usageIndex;

        public DsaValidationParameters( byte[] seed, int counter )
          : this( seed, counter, -1 )
        {
        }

        public DsaValidationParameters( byte[] seed, int counter, int usageIndex )
        {
            this.seed = seed != null ? (byte[])seed.Clone() : throw new ArgumentNullException( nameof( seed ) );
            this.counter = counter;
            this.usageIndex = usageIndex;
        }

        public virtual byte[] GetSeed() => (byte[])this.seed.Clone();

        public virtual int Counter => this.counter;

        public virtual int UsageIndex => this.usageIndex;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DsaValidationParameters other && this.Equals( other );
        }

        protected virtual bool Equals( DsaValidationParameters other ) => this.counter == other.counter && Arrays.AreEqual( this.seed, other.seed );

        public override int GetHashCode() => this.counter.GetHashCode() ^ Arrays.GetHashCode( this.seed );
    }
}
