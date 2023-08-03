// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ElGamalParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ElGamalParameters : ICipherParameters
    {
        private readonly BigInteger p;
        private readonly BigInteger g;
        private readonly int l;

        public ElGamalParameters( BigInteger p, BigInteger g )
          : this( p, g, 0 )
        {
        }

        public ElGamalParameters( BigInteger p, BigInteger g, int l )
        {
            if (p == null)
                throw new ArgumentNullException( nameof( p ) );
            if (g == null)
                throw new ArgumentNullException( nameof( g ) );
            this.p = p;
            this.g = g;
            this.l = l;
        }

        public BigInteger P => this.p;

        public BigInteger G => this.g;

        public int L => this.l;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ElGamalParameters other && this.Equals( other );
        }

        protected bool Equals( ElGamalParameters other ) => this.p.Equals( other.p ) && this.g.Equals( other.g ) && this.l == other.l;

        public override int GetHashCode() => this.p.GetHashCode() ^ this.g.GetHashCode() ^ this.l;
    }
}
