// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ECSecretBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Bcpg
{
    public class ECSecretBcpgKey : BcpgObject, IBcpgKey
    {
        internal MPInteger x;

        public ECSecretBcpgKey( BcpgInputStream bcpgIn ) => this.x = new MPInteger( bcpgIn );

        public ECSecretBcpgKey( BigInteger x ) => this.x = new MPInteger( x );

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

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WriteObject( x );

        public virtual BigInteger X => this.x.Value;
    }
}
