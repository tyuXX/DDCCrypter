// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.RsaSecretBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Bcpg
{
    public class RsaSecretBcpgKey : BcpgObject, IBcpgKey
    {
        private readonly MPInteger d;
        private readonly MPInteger p;
        private readonly MPInteger q;
        private readonly MPInteger u;
        private readonly BigInteger expP;
        private readonly BigInteger expQ;
        private readonly BigInteger crt;

        public RsaSecretBcpgKey( BcpgInputStream bcpgIn )
        {
            this.d = new MPInteger( bcpgIn );
            this.p = new MPInteger( bcpgIn );
            this.q = new MPInteger( bcpgIn );
            this.u = new MPInteger( bcpgIn );
            this.expP = this.d.Value.Remainder( this.p.Value.Subtract( BigInteger.One ) );
            this.expQ = this.d.Value.Remainder( this.q.Value.Subtract( BigInteger.One ) );
            this.crt = this.q.Value.ModInverse( this.p.Value );
        }

        public RsaSecretBcpgKey( BigInteger d, BigInteger p, BigInteger q )
        {
            int num = p.CompareTo( q );
            if (num >= 0)
            {
                if (num == 0)
                    throw new ArgumentException( "p and q cannot be equal" );
                BigInteger bigInteger = p;
                p = q;
                q = bigInteger;
            }
            this.d = new MPInteger( d );
            this.p = new MPInteger( p );
            this.q = new MPInteger( q );
            this.u = new MPInteger( p.ModInverse( q ) );
            this.expP = d.Remainder( p.Subtract( BigInteger.One ) );
            this.expQ = d.Remainder( q.Subtract( BigInteger.One ) );
            this.crt = q.ModInverse( p );
        }

        public BigInteger Modulus => this.p.Value.Multiply( this.q.Value );

        public BigInteger PrivateExponent => this.d.Value;

        public BigInteger PrimeP => this.p.Value;

        public BigInteger PrimeQ => this.q.Value;

        public BigInteger PrimeExponentP => this.expP;

        public BigInteger PrimeExponentQ => this.expQ;

        public BigInteger CrtCoefficient => this.crt;

        public string Format => "PGP";

        public override byte[] GetEncoded()
        {
            try
            {
                return base.GetEncoded();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WriteObjects( d, p, q, u );
    }
}
