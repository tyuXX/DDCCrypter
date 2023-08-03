// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DsaParameterGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DsaParameterGenerationParameters
    {
        public const int DigitalSignatureUsage = 1;
        public const int KeyEstablishmentUsage = 2;
        private readonly int l;
        private readonly int n;
        private readonly int certainty;
        private readonly SecureRandom random;
        private readonly int usageIndex;

        public DsaParameterGenerationParameters( int L, int N, int certainty, SecureRandom random )
          : this( L, N, certainty, random, -1 )
        {
        }

        public DsaParameterGenerationParameters(
          int L,
          int N,
          int certainty,
          SecureRandom random,
          int usageIndex )
        {
            this.l = L;
            this.n = N;
            this.certainty = certainty;
            this.random = random;
            this.usageIndex = usageIndex;
        }

        public virtual int L => this.l;

        public virtual int N => this.n;

        public virtual int UsageIndex => this.usageIndex;

        public virtual int Certainty => this.certainty;

        public virtual SecureRandom Random => this.random;
    }
}
