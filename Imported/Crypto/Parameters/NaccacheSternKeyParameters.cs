// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.NaccacheSternKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class NaccacheSternKeyParameters : AsymmetricKeyParameter
    {
        private readonly BigInteger g;
        private readonly BigInteger n;
        private readonly int lowerSigmaBound;

        public NaccacheSternKeyParameters(
          bool privateKey,
          BigInteger g,
          BigInteger n,
          int lowerSigmaBound )
          : base( privateKey )
        {
            this.g = g;
            this.n = n;
            this.lowerSigmaBound = lowerSigmaBound;
        }

        public BigInteger G => this.g;

        public int LowerSigmaBound => this.lowerSigmaBound;

        public BigInteger Modulus => this.n;
    }
}
