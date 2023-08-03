// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Gost3410KeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class Gost3410KeyGenerationParameters : KeyGenerationParameters
    {
        private readonly Gost3410Parameters parameters;
        private readonly DerObjectIdentifier publicKeyParamSet;

        public Gost3410KeyGenerationParameters( SecureRandom random, Gost3410Parameters parameters )
          : base( random, parameters.P.BitLength - 1 )
        {
            this.parameters = parameters;
        }

        public Gost3410KeyGenerationParameters(
          SecureRandom random,
          DerObjectIdentifier publicKeyParamSet )
          : this( random, LookupParameters( publicKeyParamSet ) )
        {
            this.publicKeyParamSet = publicKeyParamSet;
        }

        public Gost3410Parameters Parameters => this.parameters;

        public DerObjectIdentifier PublicKeyParamSet => this.publicKeyParamSet;

        private static Gost3410Parameters LookupParameters( DerObjectIdentifier publicKeyParamSet )
        {
            Gost3410ParamSetParameters paramSetParameters = publicKeyParamSet != null ? Gost3410NamedParameters.GetByOid( publicKeyParamSet ) : throw new ArgumentNullException( nameof( publicKeyParamSet ) );
            if (paramSetParameters == null)
                throw new ArgumentException( "OID is not a valid CryptoPro public key parameter set", nameof( publicKeyParamSet ) );
            return new Gost3410Parameters( paramSetParameters.P, paramSetParameters.Q, paramSetParameters.A );
        }
    }
}
