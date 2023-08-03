// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.DsaPublicBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Bcpg
{
    public class DsaPublicBcpgKey : BcpgObject, IBcpgKey
    {
        private readonly MPInteger p;
        private readonly MPInteger q;
        private readonly MPInteger g;
        private readonly MPInteger y;

        public DsaPublicBcpgKey( BcpgInputStream bcpgIn )
        {
            this.p = new MPInteger( bcpgIn );
            this.q = new MPInteger( bcpgIn );
            this.g = new MPInteger( bcpgIn );
            this.y = new MPInteger( bcpgIn );
        }

        public DsaPublicBcpgKey( BigInteger p, BigInteger q, BigInteger g, BigInteger y )
        {
            this.p = new MPInteger( p );
            this.q = new MPInteger( q );
            this.g = new MPInteger( g );
            this.y = new MPInteger( y );
        }

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

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WriteObjects( p, q, g, y );

        public BigInteger G => this.g.Value;

        public BigInteger P => this.p.Value;

        public BigInteger Q => this.q.Value;

        public BigInteger Y => this.y.Value;
    }
}
