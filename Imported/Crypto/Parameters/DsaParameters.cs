// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DsaParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DsaParameters : ICipherParameters
    {
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger g;
        private readonly DsaValidationParameters validation;

        public DsaParameters( BigInteger p, BigInteger q, BigInteger g )
          : this( p, q, g, null )
        {
        }

        public DsaParameters(
          BigInteger p,
          BigInteger q,
          BigInteger g,
          DsaValidationParameters parameters )
        {
            if (p == null)
                throw new ArgumentNullException( nameof( p ) );
            if (q == null)
                throw new ArgumentNullException( nameof( q ) );
            if (g == null)
                throw new ArgumentNullException( nameof( g ) );
            this.p = p;
            this.q = q;
            this.g = g;
            this.validation = parameters;
        }

        public BigInteger P => this.p;

        public BigInteger Q => this.q;

        public BigInteger G => this.g;

        public DsaValidationParameters ValidationParameters => this.validation;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DsaParameters other && this.Equals( other );
        }

        protected bool Equals( DsaParameters other ) => this.p.Equals( other.p ) && this.q.Equals( other.q ) && this.g.Equals( other.g );

        public override int GetHashCode() => this.p.GetHashCode() ^ this.q.GetHashCode() ^ this.g.GetHashCode();
    }
}
