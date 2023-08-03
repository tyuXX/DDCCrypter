// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Gost3410Parameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class Gost3410Parameters : ICipherParameters
    {
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger a;
        private readonly Gost3410ValidationParameters validation;

        public Gost3410Parameters( BigInteger p, BigInteger q, BigInteger a )
          : this( p, q, a, null )
        {
        }

        public Gost3410Parameters(
          BigInteger p,
          BigInteger q,
          BigInteger a,
          Gost3410ValidationParameters validation )
        {
            if (p == null)
                throw new ArgumentNullException( nameof( p ) );
            if (q == null)
                throw new ArgumentNullException( nameof( q ) );
            if (a == null)
                throw new ArgumentNullException( nameof( a ) );
            this.p = p;
            this.q = q;
            this.a = a;
            this.validation = validation;
        }

        public BigInteger P => this.p;

        public BigInteger Q => this.q;

        public BigInteger A => this.a;

        public Gost3410ValidationParameters ValidationParameters => this.validation;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is Gost3410Parameters other && this.Equals( other );
        }

        protected bool Equals( Gost3410Parameters other ) => this.p.Equals( other.p ) && this.q.Equals( other.q ) && this.a.Equals( other.a );

        public override int GetHashCode() => this.p.GetHashCode() ^ this.q.GetHashCode() ^ this.a.GetHashCode();
    }
}
