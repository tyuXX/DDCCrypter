// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.MqvPrivateParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class MqvPrivateParameters : ICipherParameters
    {
        private readonly ECPrivateKeyParameters staticPrivateKey;
        private readonly ECPrivateKeyParameters ephemeralPrivateKey;
        private readonly ECPublicKeyParameters ephemeralPublicKey;

        public MqvPrivateParameters(
          ECPrivateKeyParameters staticPrivateKey,
          ECPrivateKeyParameters ephemeralPrivateKey )
          : this( staticPrivateKey, ephemeralPrivateKey, null )
        {
        }

        public MqvPrivateParameters(
          ECPrivateKeyParameters staticPrivateKey,
          ECPrivateKeyParameters ephemeralPrivateKey,
          ECPublicKeyParameters ephemeralPublicKey )
        {
            this.staticPrivateKey = staticPrivateKey;
            this.ephemeralPrivateKey = ephemeralPrivateKey;
            this.ephemeralPublicKey = ephemeralPublicKey;
        }

        public ECPrivateKeyParameters StaticPrivateKey => this.staticPrivateKey;

        public ECPrivateKeyParameters EphemeralPrivateKey => this.ephemeralPrivateKey;

        public ECPublicKeyParameters EphemeralPublicKey => this.ephemeralPublicKey;
    }
}
