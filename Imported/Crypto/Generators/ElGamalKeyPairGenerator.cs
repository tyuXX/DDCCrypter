// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.ElGamalKeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class ElGamalKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private ElGamalKeyGenerationParameters param;

        public void Init( KeyGenerationParameters parameters ) => this.param = (ElGamalKeyGenerationParameters)parameters;

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
            ElGamalParameters parameters = this.param.Parameters;
            DHParameters dhParams = new DHParameters( parameters.P, parameters.G, null, 0, parameters.L );
            BigInteger x = instance.CalculatePrivate( dhParams, this.param.Random );
            return new AsymmetricCipherKeyPair( new ElGamalPublicKeyParameters( instance.CalculatePublic( dhParams, x ), parameters ), new ElGamalPrivateKeyParameters( x, parameters ) );
        }
    }
}
