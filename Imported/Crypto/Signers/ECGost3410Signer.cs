// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.ECGost3410Signer
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
    public class ECGost3410Signer : IDsa
    {
        private ECKeyParameters key;
        private SecureRandom random;

        public virtual string AlgorithmName => "ECGOST3410";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            if (forSigning)
            {
                if (parameters is ParametersWithRandom)
                {
                    ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                    this.random = parametersWithRandom.Random;
                    parameters = parametersWithRandom.Parameters;
                }
                else
                    this.random = new SecureRandom();
                this.key = parameters is ECPrivateKeyParameters ? (ECKeyParameters)parameters : throw new InvalidKeyException( "EC private key required for signing" );
            }
            else
                this.key = parameters is ECPublicKeyParameters ? (ECKeyParameters)parameters : throw new InvalidKeyException( "EC public key required for verification" );
        }

        public virtual BigInteger[] GenerateSignature( byte[] message )
        {
            byte[] bytes = new byte[message.Length];
            for (int index = 0; index != bytes.Length; ++index)
                bytes[index] = message[bytes.Length - 1 - index];
            BigInteger val1 = new( 1, bytes );
            ECDomainParameters parameters = this.key.Parameters;
            BigInteger n = parameters.N;
            BigInteger d = ((ECPrivateKeyParameters)this.key).D;
            ECMultiplier basePointMultiplier = this.CreateBasePointMultiplier();
            BigInteger val2;
            BigInteger bigInteger;
            do
            {
                BigInteger k;
                do
                {
                    do
                    {
                        k = new BigInteger( n.BitLength, random );
                    }
                    while (k.SignValue == 0);
                    val2 = basePointMultiplier.Multiply( parameters.G, k ).Normalize().AffineXCoord.ToBigInteger().Mod( n );
                }
                while (val2.SignValue == 0);
                bigInteger = k.Multiply( val1 ).Add( d.Multiply( val2 ) ).Mod( n );
            }
            while (bigInteger.SignValue == 0);
            return new BigInteger[2] { val2, bigInteger };
        }

        public virtual bool VerifySignature( byte[] message, BigInteger r, BigInteger s )
        {
            byte[] bytes = new byte[message.Length];
            for (int index = 0; index != bytes.Length; ++index)
                bytes[index] = message[bytes.Length - 1 - index];
            BigInteger bigInteger = new( 1, bytes );
            BigInteger n = this.key.Parameters.N;
            if (r.CompareTo( BigInteger.One ) < 0 || r.CompareTo( n ) >= 0 || s.CompareTo( BigInteger.One ) < 0 || s.CompareTo( n ) >= 0)
                return false;
            BigInteger val = bigInteger.ModInverse( n );
            BigInteger a = s.Multiply( val ).Mod( n );
            BigInteger b = n.Subtract( r ).Multiply( val ).Mod( n );
            ECPoint g = this.key.Parameters.G;
            ECPoint q = ((ECPublicKeyParameters)this.key).Q;
            ECPoint ecPoint = ECAlgorithms.SumOfTwoMultiplies( g, a, q, b ).Normalize();
            return !ecPoint.IsInfinity && ecPoint.AffineXCoord.ToBigInteger().Mod( n ).Equals( r );
        }

        protected virtual ECMultiplier CreateBasePointMultiplier() => new FixedPointCombMultiplier();
    }
}
