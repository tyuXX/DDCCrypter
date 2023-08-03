// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Gost3410KeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public abstract class Gost3410KeyParameters : AsymmetricKeyParameter
    {
        private readonly Gost3410Parameters parameters;
        private readonly DerObjectIdentifier publicKeyParamSet;

        protected Gost3410KeyParameters( bool isPrivate, Gost3410Parameters parameters )
          : base( isPrivate )
        {
            this.parameters = parameters;
        }

        protected Gost3410KeyParameters( bool isPrivate, DerObjectIdentifier publicKeyParamSet )
          : base( isPrivate )
        {
            this.parameters = LookupParameters( publicKeyParamSet );
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
