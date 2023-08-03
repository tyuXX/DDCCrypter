// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Srp6GroupParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public sealed class Srp6GroupParameters
    {
        private readonly BigInteger n;
        private readonly BigInteger g;

        public Srp6GroupParameters( BigInteger N, BigInteger g )
        {
            this.n = N;
            this.g = g;
        }

        public BigInteger G => this.g;

        public BigInteger N => this.n;
    }
}
