// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DHKeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly DHParameters parameters;

        public DHKeyGenerationParameters( SecureRandom random, DHParameters parameters )
          : base( random, GetStrength( parameters ) )
        {
            this.parameters = parameters;
        }

        public DHParameters Parameters => this.parameters;

        internal static int GetStrength( DHParameters parameters ) => parameters.L == 0 ? parameters.P.BitLength : parameters.L;
    }
}
