// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DHParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class DHParametersGenerator
    {
        private int size;
        private int certainty;
        private SecureRandom random;

        public virtual void Init( int size, int certainty, SecureRandom random )
        {
            this.size = size;
            this.certainty = certainty;
            this.random = random;
        }

        public virtual DHParameters GenerateParameters()
        {
            BigInteger[] safePrimes = DHParametersHelper.GenerateSafePrimes( this.size, this.certainty, this.random );
            BigInteger p = safePrimes[0];
            BigInteger q = safePrimes[1];
            BigInteger g = DHParametersHelper.SelectGenerator( p, q, this.random );
            return new DHParameters( p, g, q, BigInteger.Two, null );
        }
    }
}
