// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPrivateKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using System;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpPrivateKey
    {
        private readonly long keyID;
        private readonly PublicKeyPacket publicKeyPacket;
        private readonly AsymmetricKeyParameter privateKey;

        public PgpPrivateKey(
          long keyID,
          PublicKeyPacket publicKeyPacket,
          AsymmetricKeyParameter privateKey )
        {
            if (!privateKey.IsPrivate)
                throw new ArgumentException( "Expected a private key", nameof( privateKey ) );
            this.keyID = keyID;
            this.publicKeyPacket = publicKeyPacket;
            this.privateKey = privateKey;
        }

        public long KeyId => this.keyID;

        public PublicKeyPacket PublicKeyPacket => this.publicKeyPacket;

        public AsymmetricKeyParameter Key => this.privateKey;
    }
}
