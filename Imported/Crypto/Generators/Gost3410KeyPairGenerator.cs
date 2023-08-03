// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Gost3410KeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Gost3410KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private Gost3410KeyGenerationParameters param;

        public void Init( KeyGenerationParameters parameters )
        {
            if (parameters is Gost3410KeyGenerationParameters)
            {
                this.param = (Gost3410KeyGenerationParameters)parameters;
            }
            else
            {
                Gost3410KeyGenerationParameters generationParameters = new Gost3410KeyGenerationParameters( parameters.Random, CryptoProObjectIdentifiers.GostR3410x94CryptoProA );
                int strength = parameters.Strength;
                int num = generationParameters.Parameters.P.BitLength - 1;
                this.param = generationParameters;
            }
        }

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            SecureRandom random = this.param.Random;
            Gost3410Parameters parameters = this.param.Parameters;
            BigInteger q = parameters.Q;
            int num = 64;
            BigInteger bigInteger;
            do
            {
                bigInteger = new BigInteger( 256, random );
            }
            while (bigInteger.SignValue < 1 || bigInteger.CompareTo( q ) >= 0 || WNafUtilities.GetNafWeight( bigInteger ) < num);
            BigInteger p = parameters.P;
            BigInteger y = parameters.A.ModPow( bigInteger, p );
            return this.param.PublicKeyParamSet != null ? new AsymmetricCipherKeyPair( new Gost3410PublicKeyParameters( y, this.param.PublicKeyParamSet ), new Gost3410PrivateKeyParameters( bigInteger, this.param.PublicKeyParamSet ) ) : new AsymmetricCipherKeyPair( new Gost3410PublicKeyParameters( y, parameters ), new Gost3410PrivateKeyParameters( bigInteger, parameters ) );
        }
    }
}
