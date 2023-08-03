// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ElGamalKeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ElGamalKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly ElGamalParameters parameters;

        public ElGamalKeyGenerationParameters( SecureRandom random, ElGamalParameters parameters )
          : base( random, GetStrength( parameters ) )
        {
            this.parameters = parameters;
        }

        public ElGamalParameters Parameters => this.parameters;

        internal static int GetStrength( ElGamalParameters parameters ) => parameters.L == 0 ? parameters.P.BitLength : parameters.L;
    }
}
