// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ParametersWithRandom
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ParametersWithRandom : ICipherParameters
    {
        private readonly ICipherParameters parameters;
        private readonly SecureRandom random;

        public ParametersWithRandom( ICipherParameters parameters, SecureRandom random )
        {
            if (parameters == null)
                throw new ArgumentNullException( nameof( parameters ) );
            if (random == null)
                throw new ArgumentNullException( nameof( random ) );
            this.parameters = parameters;
            this.random = random;
        }

        public ParametersWithRandom( ICipherParameters parameters )
          : this( parameters, new SecureRandom() )
        {
        }

        [Obsolete( "Use Random property instead" )]
        public SecureRandom GetRandom() => this.Random;

        public SecureRandom Random => this.random;

        public ICipherParameters Parameters => this.parameters;
    }
}
