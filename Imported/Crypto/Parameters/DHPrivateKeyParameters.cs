// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DHPrivateKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHPrivateKeyParameters : DHKeyParameters
    {
        private readonly BigInteger x;

        public DHPrivateKeyParameters( BigInteger x, DHParameters parameters )
          : base( true, parameters )
        {
            this.x = x;
        }

        public DHPrivateKeyParameters(
          BigInteger x,
          DHParameters parameters,
          DerObjectIdentifier algorithmOid )
          : base( true, parameters, algorithmOid )
        {
            this.x = x;
        }

        public BigInteger X => this.x;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DHPrivateKeyParameters other && this.Equals( other );
        }

        protected bool Equals( DHPrivateKeyParameters other ) => this.x.Equals( other.x ) && this.Equals( (DHKeyParameters)other );

        public override int GetHashCode() => this.x.GetHashCode() ^ base.GetHashCode();
    }
}
