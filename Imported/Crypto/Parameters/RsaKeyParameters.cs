// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class RsaKeyParameters : AsymmetricKeyParameter
    {
        private readonly BigInteger modulus;
        private readonly BigInteger exponent;

        public RsaKeyParameters( bool isPrivate, BigInteger modulus, BigInteger exponent )
          : base( isPrivate )
        {
            if (modulus == null)
                throw new ArgumentNullException( nameof( modulus ) );
            if (exponent == null)
                throw new ArgumentNullException( nameof( exponent ) );
            if (modulus.SignValue <= 0)
                throw new ArgumentException( "Not a valid RSA modulus", nameof( modulus ) );
            if (exponent.SignValue <= 0)
                throw new ArgumentException( "Not a valid RSA exponent", nameof( exponent ) );
            this.modulus = modulus;
            this.exponent = exponent;
        }

        public BigInteger Modulus => this.modulus;

        public BigInteger Exponent => this.exponent;

        public override bool Equals( object obj ) => obj is RsaKeyParameters rsaKeyParameters && rsaKeyParameters.IsPrivate == this.IsPrivate && rsaKeyParameters.Modulus.Equals( modulus ) && rsaKeyParameters.Exponent.Equals( exponent );

        public override int GetHashCode() => this.modulus.GetHashCode() ^ this.exponent.GetHashCode() ^ this.IsPrivate.GetHashCode();
    }
}
