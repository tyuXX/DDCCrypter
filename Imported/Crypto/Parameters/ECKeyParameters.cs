// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ECKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public abstract class ECKeyParameters : AsymmetricKeyParameter
    {
        private static readonly string[] algorithms = new string[6]
        {
      "EC",
      "ECDSA",
      "ECDH",
      "ECDHC",
      "ECGOST3410",
      "ECMQV"
        };
        private readonly string algorithm;
        private readonly ECDomainParameters parameters;
        private readonly DerObjectIdentifier publicKeyParamSet;

        protected ECKeyParameters( string algorithm, bool isPrivate, ECDomainParameters parameters )
          : base( isPrivate )
        {
            if (algorithm == null)
                throw new ArgumentNullException( nameof( algorithm ) );
            if (parameters == null)
                throw new ArgumentNullException( nameof( parameters ) );
            this.algorithm = VerifyAlgorithmName( algorithm );
            this.parameters = parameters;
        }

        protected ECKeyParameters(
          string algorithm,
          bool isPrivate,
          DerObjectIdentifier publicKeyParamSet )
          : base( isPrivate )
        {
            if (algorithm == null)
                throw new ArgumentNullException( nameof( algorithm ) );
            if (publicKeyParamSet == null)
                throw new ArgumentNullException( nameof( publicKeyParamSet ) );
            this.algorithm = VerifyAlgorithmName( algorithm );
            this.parameters = LookupParameters( publicKeyParamSet );
            this.publicKeyParamSet = publicKeyParamSet;
        }

        public string AlgorithmName => this.algorithm;

        public ECDomainParameters Parameters => this.parameters;

        public DerObjectIdentifier PublicKeyParamSet => this.publicKeyParamSet;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ECDomainParameters domainParameters && this.Equals( domainParameters );
        }

        protected bool Equals( ECKeyParameters other ) => this.parameters.Equals( other.parameters ) && this.Equals( (AsymmetricKeyParameter)other );

        public override int GetHashCode() => this.parameters.GetHashCode() ^ base.GetHashCode();

        internal ECKeyGenerationParameters CreateKeyGenerationParameters( SecureRandom random ) => this.publicKeyParamSet != null ? new ECKeyGenerationParameters( this.publicKeyParamSet, random ) : new ECKeyGenerationParameters( this.parameters, random );

        internal static string VerifyAlgorithmName( string algorithm )
        {
            string upperInvariant = Platform.ToUpperInvariant( algorithm );
            if (Array.IndexOf( (Array)algorithms, algorithm, 0, algorithms.Length ) < 0)
                throw new ArgumentException( "unrecognised algorithm: " + algorithm, nameof( algorithm ) );
            return upperInvariant;
        }

        internal static ECDomainParameters LookupParameters( DerObjectIdentifier publicKeyParamSet )
        {
            ECDomainParameters domainParameters = publicKeyParamSet != null ? ECGost3410NamedCurves.GetByOid( publicKeyParamSet ) : throw new ArgumentNullException( nameof( publicKeyParamSet ) );
            if (domainParameters == null)
            {
                X9ECParameters ecCurveByOid = ECKeyPairGenerator.FindECCurveByOid( publicKeyParamSet );
                if (ecCurveByOid == null)
                    throw new ArgumentException( "OID is not a valid public key parameter set", nameof( publicKeyParamSet ) );
                domainParameters = new ECDomainParameters( ecCurveByOid.Curve, ecCurveByOid.G, ecCurveByOid.N, ecCurveByOid.H, ecCurveByOid.GetSeed() );
            }
            return domainParameters;
        }
    }
}
