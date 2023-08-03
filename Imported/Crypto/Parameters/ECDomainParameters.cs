// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ECDomainParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ECDomainParameters
    {
        internal ECCurve curve;
        internal byte[] seed;
        internal ECPoint g;
        internal BigInteger n;
        internal BigInteger h;

        public ECDomainParameters( ECCurve curve, ECPoint g, BigInteger n )
          : this( curve, g, n, BigInteger.One )
        {
        }

        public ECDomainParameters( ECCurve curve, ECPoint g, BigInteger n, BigInteger h )
          : this( curve, g, n, h, null )
        {
        }

        public ECDomainParameters( ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed )
        {
            if (curve == null)
                throw new ArgumentNullException( nameof( curve ) );
            if (g == null)
                throw new ArgumentNullException( nameof( g ) );
            if (n == null)
                throw new ArgumentNullException( nameof( n ) );
            if (h == null)
                throw new ArgumentNullException( nameof( h ) );
            this.curve = curve;
            this.g = g.Normalize();
            this.n = n;
            this.h = h;
            this.seed = Arrays.Clone( seed );
        }

        public ECCurve Curve => this.curve;

        public ECPoint G => this.g;

        public BigInteger N => this.n;

        public BigInteger H => this.h;

        public byte[] GetSeed() => Arrays.Clone( this.seed );

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ECDomainParameters other && this.Equals( other );
        }

        protected bool Equals( ECDomainParameters other ) => this.curve.Equals( other.curve ) && this.g.Equals( other.g ) && this.n.Equals( other.n ) && this.h.Equals( other.h ) && Arrays.AreEqual( this.seed, other.seed );

        public override int GetHashCode() => this.curve.GetHashCode() ^ this.g.GetHashCode() ^ this.n.GetHashCode() ^ this.h.GetHashCode() ^ Arrays.GetHashCode( this.seed );
    }
}
