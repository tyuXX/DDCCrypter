// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DsaPublicKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DsaPublicKeyParameters : DsaKeyParameters
    {
        private readonly BigInteger y;

        public DsaPublicKeyParameters( BigInteger y, DsaParameters parameters )
          : base( false, parameters )
        {
            this.y = y != null ? y : throw new ArgumentNullException( nameof( y ) );
        }

        public BigInteger Y => this.y;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DsaPublicKeyParameters other && this.Equals( other );
        }

        protected bool Equals( DsaPublicKeyParameters other ) => this.y.Equals( other.y ) && this.Equals( (DsaKeyParameters)other );

        public override int GetHashCode() => this.y.GetHashCode() ^ base.GetHashCode();
    }
}
