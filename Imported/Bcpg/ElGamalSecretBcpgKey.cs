// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ElGamalSecretBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Bcpg
{
    public class ElGamalSecretBcpgKey : BcpgObject, IBcpgKey
    {
        internal MPInteger x;

        public ElGamalSecretBcpgKey( BcpgInputStream bcpgIn ) => this.x = new MPInteger( bcpgIn );

        public ElGamalSecretBcpgKey( BigInteger x ) => this.x = new MPInteger( x );

        public string Format => "PGP";

        public BigInteger X => this.x.Value;

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

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WriteObject( x );
    }
}
