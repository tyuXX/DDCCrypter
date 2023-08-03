// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.NaccacheSternPrivateKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class NaccacheSternPrivateKeyParameters : NaccacheSternKeyParameters
    {
        private readonly BigInteger phiN;
        private readonly IList smallPrimes;

        [Obsolete]
        public NaccacheSternPrivateKeyParameters(
          BigInteger g,
          BigInteger n,
          int lowerSigmaBound,
          ArrayList smallPrimes,
          BigInteger phiN )
          : base( true, g, n, lowerSigmaBound )
        {
            this.smallPrimes = smallPrimes;
            this.phiN = phiN;
        }

        public NaccacheSternPrivateKeyParameters(
          BigInteger g,
          BigInteger n,
          int lowerSigmaBound,
          IList smallPrimes,
          BigInteger phiN )
          : base( true, g, n, lowerSigmaBound )
        {
            this.smallPrimes = smallPrimes;
            this.phiN = phiN;
        }

        public BigInteger PhiN => this.phiN;

        [Obsolete( "Use 'SmallPrimesList' instead" )]
        public ArrayList SmallPrimes => new( smallPrimes );

        public IList SmallPrimesList => this.smallPrimes;
    }
}
