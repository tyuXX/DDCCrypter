// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ElGamalPrivateKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ElGamalPrivateKeyParameters : ElGamalKeyParameters
    {
        private readonly BigInteger x;

        public ElGamalPrivateKeyParameters( BigInteger x, ElGamalParameters parameters )
          : base( true, parameters )
        {
            this.x = x != null ? x : throw new ArgumentNullException( nameof( x ) );
        }

        public BigInteger X => this.x;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ElGamalPrivateKeyParameters other && this.Equals( other );
        }

        protected bool Equals( ElGamalPrivateKeyParameters other ) => other.x.Equals( x ) && this.Equals( (ElGamalKeyParameters)other );

        public override int GetHashCode() => this.x.GetHashCode() ^ base.GetHashCode();
    }
}
