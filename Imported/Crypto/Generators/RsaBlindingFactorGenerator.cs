// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.RsaBlindingFactorGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class RsaBlindingFactorGenerator
    {
        private RsaKeyParameters key;
        private SecureRandom random;

        public void Init( ICipherParameters param )
        {
            if (param is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
                this.key = (RsaKeyParameters)parametersWithRandom.Parameters;
                this.random = parametersWithRandom.Random;
            }
            else
            {
                this.key = (RsaKeyParameters)param;
                this.random = new SecureRandom();
            }
            if (this.key.IsPrivate)
                throw new ArgumentException( "generator requires RSA public key" );
        }

        public BigInteger GenerateBlindingFactor()
        {
            BigInteger bigInteger1 = this.key != null ? this.key.Modulus : throw new InvalidOperationException( "generator not initialised" );
            int sizeInBits = bigInteger1.BitLength - 1;
            BigInteger blindingFactor;
            BigInteger bigInteger2;
            do
            {
                blindingFactor = new BigInteger( sizeInBits, random );
                bigInteger2 = blindingFactor.Gcd( bigInteger1 );
            }
            while (blindingFactor.SignValue == 0 || blindingFactor.Equals( BigInteger.One ) || !bigInteger2.Equals( BigInteger.One ));
            return blindingFactor;
        }
    }
}
