// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ECKeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ECKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly ECDomainParameters domainParams;
        private readonly DerObjectIdentifier publicKeyParamSet;

        public ECKeyGenerationParameters( ECDomainParameters domainParameters, SecureRandom random )
          : base( random, domainParameters.N.BitLength )
        {
            this.domainParams = domainParameters;
        }

        public ECKeyGenerationParameters( DerObjectIdentifier publicKeyParamSet, SecureRandom random )
          : this( ECKeyParameters.LookupParameters( publicKeyParamSet ), random )
        {
            this.publicKeyParamSet = publicKeyParamSet;
        }

        public ECDomainParameters DomainParameters => this.domainParams;

        public DerObjectIdentifier PublicKeyParamSet => this.publicKeyParamSet;
    }
}
