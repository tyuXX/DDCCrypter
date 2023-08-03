// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.NaccacheSternKeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class NaccacheSternKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly int certainty;
        private readonly int countSmallPrimes;

        public NaccacheSternKeyGenerationParameters(
          SecureRandom random,
          int strength,
          int certainty,
          int countSmallPrimes )
          : base( random, strength )
        {
            if (countSmallPrimes % 2 == 1)
                throw new ArgumentException( "countSmallPrimes must be a multiple of 2" );
            if (countSmallPrimes < 30)
                throw new ArgumentException( "countSmallPrimes must be >= 30 for security reasons" );
            this.certainty = certainty;
            this.countSmallPrimes = countSmallPrimes;
        }

        [Obsolete( "Use version without 'debug' parameter" )]
        public NaccacheSternKeyGenerationParameters(
          SecureRandom random,
          int strength,
          int certainty,
          int countSmallPrimes,
          bool debug )
          : this( random, strength, certainty, countSmallPrimes )
        {
        }

        public int Certainty => this.certainty;

        public int CountSmallPrimes => this.countSmallPrimes;

        [Obsolete( "Remove: always false" )]
        public bool IsDebug => false;
    }
}
