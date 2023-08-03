// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.MqvPublicParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class MqvPublicParameters : ICipherParameters
    {
        private readonly ECPublicKeyParameters staticPublicKey;
        private readonly ECPublicKeyParameters ephemeralPublicKey;

        public MqvPublicParameters(
          ECPublicKeyParameters staticPublicKey,
          ECPublicKeyParameters ephemeralPublicKey )
        {
            this.staticPublicKey = staticPublicKey;
            this.ephemeralPublicKey = ephemeralPublicKey;
        }

        public ECPublicKeyParameters StaticPublicKey => this.staticPublicKey;

        public ECPublicKeyParameters EphemeralPublicKey => this.ephemeralPublicKey;
    }
}
