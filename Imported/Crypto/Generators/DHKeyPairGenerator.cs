// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DHKeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class DHKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private DHKeyGenerationParameters param;

        public virtual void Init( KeyGenerationParameters parameters ) => this.param = (DHKeyGenerationParameters)parameters;

        public virtual AsymmetricCipherKeyPair GenerateKeyPair()
        {
            DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
            DHParameters parameters = this.param.Parameters;
            BigInteger x = instance.CalculatePrivate( parameters, this.param.Random );
            return new AsymmetricCipherKeyPair( new DHPublicKeyParameters( instance.CalculatePublic( parameters, x ), parameters ), new DHPrivateKeyParameters( x, parameters ) );
        }
    }
}
