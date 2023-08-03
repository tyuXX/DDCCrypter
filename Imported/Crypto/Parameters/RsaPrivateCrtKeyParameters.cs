// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class RsaPrivateCrtKeyParameters : RsaKeyParameters
    {
        private readonly BigInteger e;
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger dP;
        private readonly BigInteger dQ;
        private readonly BigInteger qInv;

        public RsaPrivateCrtKeyParameters(
          BigInteger modulus,
          BigInteger publicExponent,
          BigInteger privateExponent,
          BigInteger p,
          BigInteger q,
          BigInteger dP,
          BigInteger dQ,
          BigInteger qInv )
          : base( true, modulus, privateExponent )
        {
            ValidateValue( publicExponent, nameof( publicExponent ), "exponent" );
            ValidateValue( p, nameof( p ), "P value" );
            ValidateValue( q, nameof( q ), "Q value" );
            ValidateValue( dP, nameof( dP ), "DP value" );
            ValidateValue( dQ, nameof( dQ ), "DQ value" );
            ValidateValue( qInv, nameof( qInv ), "InverseQ value" );
            this.e = publicExponent;
            this.p = p;
            this.q = q;
            this.dP = dP;
            this.dQ = dQ;
            this.qInv = qInv;
        }

        public BigInteger PublicExponent => this.e;

        public BigInteger P => this.p;

        public BigInteger Q => this.q;

        public BigInteger DP => this.dP;

        public BigInteger DQ => this.dQ;

        public BigInteger QInv => this.qInv;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is RsaPrivateCrtKeyParameters crtKeyParameters && crtKeyParameters.DP.Equals( dP ) && crtKeyParameters.DQ.Equals( dQ ) && crtKeyParameters.Exponent.Equals( Exponent ) && crtKeyParameters.Modulus.Equals( Modulus ) && crtKeyParameters.P.Equals( p ) && crtKeyParameters.Q.Equals( q ) && crtKeyParameters.PublicExponent.Equals( e ) && crtKeyParameters.QInv.Equals( qInv );
        }

        public override int GetHashCode() => this.DP.GetHashCode() ^ this.DQ.GetHashCode() ^ this.Exponent.GetHashCode() ^ this.Modulus.GetHashCode() ^ this.P.GetHashCode() ^ this.Q.GetHashCode() ^ this.PublicExponent.GetHashCode() ^ this.QInv.GetHashCode();

        private static void ValidateValue( BigInteger x, string name, string desc )
        {
            if (x == null)
                throw new ArgumentNullException( name );
            if (x.SignValue <= 0)
                throw new ArgumentException( "Not a valid RSA " + desc, name );
        }
    }
}
