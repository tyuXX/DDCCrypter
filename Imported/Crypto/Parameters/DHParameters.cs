// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DHParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHParameters : ICipherParameters
    {
        private const int DefaultMinimumLength = 160;
        private readonly BigInteger p;
        private readonly BigInteger g;
        private readonly BigInteger q;
        private readonly BigInteger j;
        private readonly int m;
        private readonly int l;
        private readonly DHValidationParameters validation;

        private static int GetDefaultMParam( int lParam ) => lParam == 0 ? 160 : System.Math.Min( lParam, 160 );

        public DHParameters( BigInteger p, BigInteger g )
          : this( p, g, null, 0 )
        {
        }

        public DHParameters( BigInteger p, BigInteger g, BigInteger q )
          : this( p, g, q, 0 )
        {
        }

        public DHParameters( BigInteger p, BigInteger g, BigInteger q, int l )
          : this( p, g, q, GetDefaultMParam( l ), l, null, null )
        {
        }

        public DHParameters( BigInteger p, BigInteger g, BigInteger q, int m, int l )
          : this( p, g, q, m, l, null, null )
        {
        }

        public DHParameters(
          BigInteger p,
          BigInteger g,
          BigInteger q,
          BigInteger j,
          DHValidationParameters validation )
          : this( p, g, q, 160, 0, j, validation )
        {
        }

        public DHParameters(
          BigInteger p,
          BigInteger g,
          BigInteger q,
          int m,
          int l,
          BigInteger j,
          DHValidationParameters validation )
        {
            if (p == null)
                throw new ArgumentNullException( nameof( p ) );
            if (g == null)
                throw new ArgumentNullException( nameof( g ) );
            if (!p.TestBit( 0 ))
                throw new ArgumentException( "field must be an odd prime", nameof( p ) );
            if (g.CompareTo( BigInteger.Two ) < 0 || g.CompareTo( p.Subtract( BigInteger.Two ) ) > 0)
                throw new ArgumentException( "generator must in the range [2, p - 2]", nameof( g ) );
            if (q != null && q.BitLength >= p.BitLength)
                throw new ArgumentException( "q too big to be a factor of (p-1)", nameof( q ) );
            if (m >= p.BitLength)
                throw new ArgumentException( "m value must be < bitlength of p", nameof( m ) );
            if (l != 0)
            {
                if (l >= p.BitLength)
                    throw new ArgumentException( "when l value specified, it must be less than bitlength(p)", nameof( l ) );
                if (l < m)
                    throw new ArgumentException( "when l value specified, it may not be less than m value", nameof( l ) );
            }
            if (j != null && j.CompareTo( BigInteger.Two ) < 0)
                throw new ArgumentException( "subgroup factor must be >= 2", nameof( j ) );
            this.p = p;
            this.g = g;
            this.q = q;
            this.m = m;
            this.l = l;
            this.j = j;
            this.validation = validation;
        }

        public BigInteger P => this.p;

        public BigInteger G => this.g;

        public BigInteger Q => this.q;

        public BigInteger J => this.j;

        public int M => this.m;

        public int L => this.l;

        public DHValidationParameters ValidationParameters => this.validation;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DHParameters other && this.Equals( other );
        }

        protected bool Equals( DHParameters other ) => this.p.Equals( other.p ) && this.g.Equals( other.g ) && Equals( q, other.q );

        public override int GetHashCode()
        {
            int hashCode = this.p.GetHashCode() ^ this.g.GetHashCode();
            if (this.q != null)
                hashCode ^= this.q.GetHashCode();
            return hashCode;
        }
    }
}
