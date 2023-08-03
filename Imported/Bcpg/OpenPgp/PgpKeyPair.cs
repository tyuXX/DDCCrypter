// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpKeyPair
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpKeyPair
    {
        private readonly PgpPublicKey pub;
        private readonly PgpPrivateKey priv;

        public PgpKeyPair(
          PublicKeyAlgorithmTag algorithm,
          AsymmetricCipherKeyPair keyPair,
          DateTime time )
          : this( algorithm, keyPair.Public, keyPair.Private, time )
        {
        }

        public PgpKeyPair(
          PublicKeyAlgorithmTag algorithm,
          AsymmetricKeyParameter pubKey,
          AsymmetricKeyParameter privKey,
          DateTime time )
        {
            this.pub = new PgpPublicKey( algorithm, pubKey, time );
            this.priv = new PgpPrivateKey( this.pub.KeyId, this.pub.PublicKeyPacket, privKey );
        }

        public PgpKeyPair( PgpPublicKey pub, PgpPrivateKey priv )
        {
            this.pub = pub;
            this.priv = priv;
        }

        public long KeyId => this.pub.KeyId;

        public PgpPublicKey PublicKey => this.pub;

        public PgpPrivateKey PrivateKey => this.priv;
    }
}
