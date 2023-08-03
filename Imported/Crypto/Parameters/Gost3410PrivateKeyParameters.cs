// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Gost3410PrivateKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class Gost3410PrivateKeyParameters : Gost3410KeyParameters
    {
        private readonly BigInteger x;

        public Gost3410PrivateKeyParameters( BigInteger x, Gost3410Parameters parameters )
          : base( true, parameters )
        {
            if (x.SignValue < 1 || x.BitLength > 256 || x.CompareTo( this.Parameters.Q ) >= 0)
                throw new ArgumentException( "Invalid x for GOST3410 private key", nameof( x ) );
            this.x = x;
        }

        public Gost3410PrivateKeyParameters( BigInteger x, DerObjectIdentifier publicKeyParamSet )
          : base( true, publicKeyParamSet )
        {
            if (x.SignValue < 1 || x.BitLength > 256 || x.CompareTo( this.Parameters.Q ) >= 0)
                throw new ArgumentException( "Invalid x for GOST3410 private key", nameof( x ) );
            this.x = x;
        }

        public BigInteger X => this.x;
    }
}
