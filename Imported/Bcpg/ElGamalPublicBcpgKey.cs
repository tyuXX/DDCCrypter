// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ElGamalPublicBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Bcpg
{
    public class ElGamalPublicBcpgKey : BcpgObject, IBcpgKey
    {
        internal MPInteger p;
        internal MPInteger g;
        internal MPInteger y;

        public ElGamalPublicBcpgKey( BcpgInputStream bcpgIn )
        {
            this.p = new MPInteger( bcpgIn );
            this.g = new MPInteger( bcpgIn );
            this.y = new MPInteger( bcpgIn );
        }

        public ElGamalPublicBcpgKey( BigInteger p, BigInteger g, BigInteger y )
        {
            this.p = new MPInteger( p );
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

        public BigInteger P => this.p.Value;

        public BigInteger G => this.g.Value;

        public BigInteger Y => this.y.Value;

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WriteObjects( p, g, y );
    }
}
