// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.ECKeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class ECKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private readonly string algorithm;
        private ECDomainParameters parameters;
        private DerObjectIdentifier publicKeyParamSet;
        private SecureRandom random;

        public ECKeyPairGenerator()
          : this( "EC" )
        {
        }

        public ECKeyPairGenerator( string algorithm ) => this.algorithm = algorithm != null ? ECKeyParameters.VerifyAlgorithmName( algorithm ) : throw new ArgumentNullException( nameof( algorithm ) );

        public void Init( KeyGenerationParameters parameters )
        {
            if (parameters is ECKeyGenerationParameters)
            {
                ECKeyGenerationParameters generationParameters = (ECKeyGenerationParameters)parameters;
                this.publicKeyParamSet = generationParameters.PublicKeyParamSet;
                this.parameters = generationParameters.DomainParameters;
            }
            else
            {
                DerObjectIdentifier oid;
                switch (parameters.Strength)
                {
                    case 192:
                        oid = X9ObjectIdentifiers.Prime192v1;
                        break;
                    case 224:
                        oid = SecObjectIdentifiers.SecP224r1;
                        break;
                    case 239:
                        oid = X9ObjectIdentifiers.Prime239v1;
                        break;
                    case 256:
                        oid = X9ObjectIdentifiers.Prime256v1;
                        break;
                    case 384:
                        oid = SecObjectIdentifiers.SecP384r1;
                        break;
                    case 521:
                        oid = SecObjectIdentifiers.SecP521r1;
                        break;
                    default:
                        throw new InvalidParameterException( "unknown key size." );
                }
                X9ECParameters ecCurveByOid = FindECCurveByOid( oid );
                this.publicKeyParamSet = oid;
                this.parameters = new ECDomainParameters( ecCurveByOid.Curve, ecCurveByOid.G, ecCurveByOid.N, ecCurveByOid.H, ecCurveByOid.GetSeed() );
            }
            this.random = parameters.Random;
            if (this.random != null)
                return;
            this.random = new SecureRandom();
        }

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            BigInteger n = this.parameters.N;
            int num = n.BitLength >> 2;
            BigInteger bigInteger;
            do
            {
                bigInteger = new BigInteger( n.BitLength, random );
            }
            while (bigInteger.CompareTo( BigInteger.Two ) < 0 || bigInteger.CompareTo( n ) >= 0 || WNafUtilities.GetNafWeight( bigInteger ) < num);
            ECPoint q = this.CreateBasePointMultiplier().Multiply( this.parameters.G, bigInteger );
            return this.publicKeyParamSet != null ? new AsymmetricCipherKeyPair( new ECPublicKeyParameters( this.algorithm, q, this.publicKeyParamSet ), new ECPrivateKeyParameters( this.algorithm, bigInteger, this.publicKeyParamSet ) ) : new AsymmetricCipherKeyPair( new ECPublicKeyParameters( this.algorithm, q, this.parameters ), new ECPrivateKeyParameters( this.algorithm, bigInteger, this.parameters ) );
        }

        protected virtual ECMultiplier CreateBasePointMultiplier() => new FixedPointCombMultiplier();

        internal static X9ECParameters FindECCurveByOid( DerObjectIdentifier oid ) => CustomNamedCurves.GetByOid( oid ) ?? ECNamedCurveTable.GetByOid( oid );

        internal static ECPublicKeyParameters GetCorrespondingPublicKey( ECPrivateKeyParameters privKey )
        {
            ECDomainParameters parameters = privKey.Parameters;
            ECPoint q = new FixedPointCombMultiplier().Multiply( parameters.G, privKey.D );
            return privKey.PublicKeyParamSet != null ? new ECPublicKeyParameters( privKey.AlgorithmName, q, privKey.PublicKeyParamSet ) : new ECPublicKeyParameters( privKey.AlgorithmName, q, parameters );
        }
    }
}
