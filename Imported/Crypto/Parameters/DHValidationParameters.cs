// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DHValidationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHValidationParameters
    {
        private readonly byte[] seed;
        private readonly int counter;

        public DHValidationParameters( byte[] seed, int counter )
        {
            this.seed = seed != null ? (byte[])seed.Clone() : throw new ArgumentNullException( nameof( seed ) );
            this.counter = counter;
        }

        public byte[] GetSeed() => (byte[])this.seed.Clone();

        public int Counter => this.counter;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DHValidationParameters other && this.Equals( other );
        }

        protected bool Equals( DHValidationParameters other ) => this.counter == other.counter && Arrays.AreEqual( this.seed, other.seed );

        public override int GetHashCode() => this.counter.GetHashCode() ^ Arrays.GetHashCode( this.seed );
    }
}
