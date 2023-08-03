// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ECPublicKeyParameters : ECKeyParameters
    {
        private readonly ECPoint q;

        public ECPublicKeyParameters( ECPoint q, ECDomainParameters parameters )
          : this( "EC", q, parameters )
        {
        }

        [Obsolete( "Use version with explicit 'algorithm' parameter" )]
        public ECPublicKeyParameters( ECPoint q, DerObjectIdentifier publicKeyParamSet )
          : base( "ECGOST3410", false, publicKeyParamSet )
        {
            this.q = q != null ? q.Normalize() : throw new ArgumentNullException( nameof( q ) );
        }

        public ECPublicKeyParameters( string algorithm, ECPoint q, ECDomainParameters parameters )
          : base( algorithm, false, parameters )
        {
            this.q = q != null ? q.Normalize() : throw new ArgumentNullException( nameof( q ) );
        }

        public ECPublicKeyParameters(
          string algorithm,
          ECPoint q,
          DerObjectIdentifier publicKeyParamSet )
          : base( algorithm, false, publicKeyParamSet )
        {
            this.q = q != null ? q.Normalize() : throw new ArgumentNullException( nameof( q ) );
        }

        public ECPoint Q => this.q;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ECPublicKeyParameters other && this.Equals( other );
        }

        protected bool Equals( ECPublicKeyParameters other ) => this.q.Equals( other.q ) && this.Equals( (ECKeyParameters)other );

        public override int GetHashCode() => this.q.GetHashCode() ^ base.GetHashCode();
    }
}
