// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.RsaKeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class RsaKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly BigInteger publicExponent;
        private readonly int certainty;

        public RsaKeyGenerationParameters(
          BigInteger publicExponent,
          SecureRandom random,
          int strength,
          int certainty )
          : base( random, strength )
        {
            this.publicExponent = publicExponent;
            this.certainty = certainty;
        }

        public BigInteger PublicExponent => this.publicExponent;

        public int Certainty => this.certainty;

        public override bool Equals( object obj ) => obj is RsaKeyGenerationParameters generationParameters && this.certainty == generationParameters.certainty && this.publicExponent.Equals( generationParameters.publicExponent );

        public override int GetHashCode() => this.certainty.GetHashCode() ^ this.publicExponent.GetHashCode();
    }
}
