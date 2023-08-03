// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ECPrivateKeyParameters : ECKeyParameters
    {
        private readonly BigInteger d;

        public ECPrivateKeyParameters( BigInteger d, ECDomainParameters parameters )
          : this( "EC", d, parameters )
        {
        }

        [Obsolete( "Use version with explicit 'algorithm' parameter" )]
        public ECPrivateKeyParameters( BigInteger d, DerObjectIdentifier publicKeyParamSet )
          : base( "ECGOST3410", true, publicKeyParamSet )
        {
            this.d = d != null ? d : throw new ArgumentNullException( nameof( d ) );
        }

        public ECPrivateKeyParameters( string algorithm, BigInteger d, ECDomainParameters parameters )
          : base( algorithm, true, parameters )
        {
            this.d = d != null ? d : throw new ArgumentNullException( nameof( d ) );
        }

        public ECPrivateKeyParameters(
          string algorithm,
          BigInteger d,
          DerObjectIdentifier publicKeyParamSet )
          : base( algorithm, true, publicKeyParamSet )
        {
            this.d = d != null ? d : throw new ArgumentNullException( nameof( d ) );
        }

        public BigInteger D => this.d;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ECPrivateKeyParameters other && this.Equals( other );
        }

        protected bool Equals( ECPrivateKeyParameters other ) => this.d.Equals( other.d ) && this.Equals( (ECKeyParameters)other );

        public override int GetHashCode() => this.d.GetHashCode() ^ base.GetHashCode();
    }
}
