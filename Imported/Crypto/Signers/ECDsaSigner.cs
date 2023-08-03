// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.ECDsaSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class ECDsaSigner : IDsa
    {
        private static readonly BigInteger Eight = BigInteger.ValueOf( 8L );
        protected readonly IDsaKCalculator kCalculator;
        protected ECKeyParameters key = null;
        protected SecureRandom random = null;

        public ECDsaSigner() => this.kCalculator = new RandomDsaKCalculator();

        public ECDsaSigner( IDsaKCalculator kCalculator ) => this.kCalculator = kCalculator;

        public virtual string AlgorithmName => "ECDSA";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            SecureRandom provided = null;
            if (forSigning)
            {
                if (parameters is ParametersWithRandom)
                {
                    ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                    provided = parametersWithRandom.Random;
                    parameters = parametersWithRandom.Parameters;
                }
                this.key = parameters is ECPrivateKeyParameters ? (ECKeyParameters)parameters : throw new InvalidKeyException( "EC private key required for signing" );
            }
            else
                this.key = parameters is ECPublicKeyParameters ? (ECKeyParameters)parameters : throw new InvalidKeyException( "EC public key required for verification" );
            this.random = this.InitSecureRandom( forSigning && !this.kCalculator.IsDeterministic, provided );
        }

        public virtual BigInteger[] GenerateSignature( byte[] message )
        {
            ECDomainParameters parameters = this.key.Parameters;
            BigInteger n = parameters.N;
            BigInteger e = this.CalculateE( n, message );
            BigInteger d = ((ECPrivateKeyParameters)this.key).D;
            if (this.kCalculator.IsDeterministic)
                this.kCalculator.Init( n, d, message );
            else
                this.kCalculator.Init( n, this.random );
            ECMultiplier basePointMultiplier = this.CreateBasePointMultiplier();
            BigInteger val;
            BigInteger bigInteger;
            do
            {
                BigInteger k;
                do
                {
                    k = this.kCalculator.NextK();
                    val = basePointMultiplier.Multiply( parameters.G, k ).Normalize().AffineXCoord.ToBigInteger().Mod( n );
                }
                while (val.SignValue == 0);
                bigInteger = k.ModInverse( n ).Multiply( e.Add( d.Multiply( val ) ) ).Mod( n );
            }
            while (bigInteger.SignValue == 0);
            return new BigInteger[2] { val, bigInteger };
        }

        public virtual bool VerifySignature( byte[] message, BigInteger r, BigInteger s )
        {
            BigInteger n = this.key.Parameters.N;
            if (r.SignValue < 1 || s.SignValue < 1 || r.CompareTo( n ) >= 0 || s.CompareTo( n ) >= 0)
                return false;
            BigInteger e = this.CalculateE( n, message );
            BigInteger val = s.ModInverse( n );
            BigInteger a = e.Multiply( val ).Mod( n );
            BigInteger b = r.Multiply( val ).Mod( n );
            ECPoint g = this.key.Parameters.G;
            ECPoint q = ((ECPublicKeyParameters)this.key).Q;
            ECPoint p = ECAlgorithms.SumOfTwoMultiplies( g, a, q, b );
            if (p.IsInfinity)
                return false;
            ECCurve curve = p.Curve;
            if (curve != null)
            {
                BigInteger cofactor = curve.Cofactor;
                if (cofactor != null && cofactor.CompareTo( Eight ) <= 0)
                {
                    ECFieldElement denominator = this.GetDenominator( curve.CoordinateSystem, p );
                    if (denominator != null && !denominator.IsZero)
                    {
                        ECFieldElement xcoord = p.XCoord;
                        for (; curve.IsValidFieldElement( r ); r = r.Add( n ))
                        {
                            if (curve.FromBigInteger( r ).Multiply( denominator ).Equals( xcoord ))
                                return true;
                        }
                        return false;
                    }
                }
            }
            return p.Normalize().AffineXCoord.ToBigInteger().Mod( n ).Equals( r );
        }

        protected virtual BigInteger CalculateE( BigInteger n, byte[] message )
        {
            int num = message.Length * 8;
            BigInteger e = new BigInteger( 1, message );
            if (n.BitLength < num)
                e = e.ShiftRight( num - n.BitLength );
            return e;
        }

        protected virtual ECMultiplier CreateBasePointMultiplier() => new FixedPointCombMultiplier();

        protected virtual ECFieldElement GetDenominator( int coordinateSystem, ECPoint p )
        {
            switch (coordinateSystem)
            {
                case 1:
                case 6:
                case 7:
                    return p.GetZCoord( 0 );
                case 2:
                case 3:
                case 4:
                    return p.GetZCoord( 0 ).Square();
                default:
                    return null;
            }
        }

        protected virtual SecureRandom InitSecureRandom( bool needed, SecureRandom provided )
        {
            if (!needed)
                return null;
            return provided == null ? new SecureRandom() : provided;
        }
    }
}
