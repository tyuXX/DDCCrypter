// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DsaKeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class DsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private static readonly BigInteger One = BigInteger.One;
        private DsaKeyGenerationParameters param;

        public void Init( KeyGenerationParameters parameters ) => this.param = parameters != null ? (DsaKeyGenerationParameters)parameters : throw new ArgumentNullException( nameof( parameters ) );

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            DsaParameters parameters = this.param.Parameters;
            BigInteger privateKey = GeneratePrivateKey( parameters.Q, this.param.Random );
            return new AsymmetricCipherKeyPair( new DsaPublicKeyParameters( CalculatePublicKey( parameters.P, parameters.G, privateKey ), parameters ), new DsaPrivateKeyParameters( privateKey, parameters ) );
        }

        private static BigInteger GeneratePrivateKey( BigInteger q, SecureRandom random )
        {
            int num = q.BitLength >> 2;
            BigInteger randomInRange;
            do
            {
                randomInRange = BigIntegers.CreateRandomInRange( One, q.Subtract( One ), random );
            }
            while (WNafUtilities.GetNafWeight( randomInRange ) < num);
            return randomInRange;
        }

        private static BigInteger CalculatePublicKey( BigInteger p, BigInteger g, BigInteger x ) => g.ModPow( x, p );
    }
}
