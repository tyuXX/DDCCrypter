// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Gost3410PublicKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class Gost3410PublicKeyParameters : Gost3410KeyParameters
    {
        private readonly BigInteger y;

        public Gost3410PublicKeyParameters( BigInteger y, Gost3410Parameters parameters )
          : base( false, parameters )
        {
            if (y.SignValue < 1 || y.CompareTo( this.Parameters.P ) >= 0)
                throw new ArgumentException( "Invalid y for GOST3410 public key", nameof( y ) );
            this.y = y;
        }

        public Gost3410PublicKeyParameters( BigInteger y, DerObjectIdentifier publicKeyParamSet )
          : base( false, publicKeyParamSet )
        {
            if (y.SignValue < 1 || y.CompareTo( this.Parameters.P ) >= 0)
                throw new ArgumentException( "Invalid y for GOST3410 public key", nameof( y ) );
            this.y = y;
        }

        public BigInteger Y => this.y;
    }
}
