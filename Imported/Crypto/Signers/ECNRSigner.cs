// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.ECNRSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class ECNRSigner : IDsa
    {
        private bool forSigning;
        private ECKeyParameters key;
        private SecureRandom random;

        public virtual string AlgorithmName => "ECNR";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            this.forSigning = forSigning;
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
            if (!this.forSigning)
                throw new InvalidOperationException( "not initialised for signing" );
            BigInteger n = this.key.Parameters.N;
            int bitLength1 = n.BitLength;
            BigInteger bigInteger1 = new BigInteger( 1, message );
            int bitLength2 = bigInteger1.BitLength;
            ECPrivateKeyParameters key = (ECPrivateKeyParameters)this.key;
            if (bitLength2 > bitLength1)
                throw new DataLengthException( "input too large for ECNR key." );
            AsymmetricCipherKeyPair keyPair;
            BigInteger bigInteger2;
            do
            {
                ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator();
                keyPairGenerator.Init( new ECKeyGenerationParameters( key.Parameters, this.random ) );
                keyPair = keyPairGenerator.GenerateKeyPair();
                bigInteger2 = ((ECPublicKeyParameters)keyPair.Public).Q.AffineXCoord.ToBigInteger().Add( bigInteger1 ).Mod( n );
            }
            while (bigInteger2.SignValue == 0);
            BigInteger d = key.D;
            BigInteger bigInteger3 = ((ECPrivateKeyParameters)keyPair.Private).D.Subtract( bigInteger2.Multiply( d ) ).Mod( n );
            return new BigInteger[2] { bigInteger2, bigInteger3 };
        }

        public virtual bool VerifySignature( byte[] message, BigInteger r, BigInteger s )
        {
            if (this.forSigning)
                throw new InvalidOperationException( "not initialised for verifying" );
            ECPublicKeyParameters key = (ECPublicKeyParameters)this.key;
            BigInteger n = key.Parameters.N;
            int bitLength = n.BitLength;
            BigInteger bigInteger1 = new BigInteger( 1, message );
            if (bigInteger1.BitLength > bitLength)
                throw new DataLengthException( "input too large for ECNR key." );
            if (r.CompareTo( BigInteger.One ) < 0 || r.CompareTo( n ) >= 0 || s.CompareTo( BigInteger.Zero ) < 0 || s.CompareTo( n ) >= 0)
                return false;
            ECPoint g = key.Parameters.G;
            ECPoint q = key.Q;
            ECPoint ecPoint = ECAlgorithms.SumOfTwoMultiplies( g, s, q, r ).Normalize();
            if (ecPoint.IsInfinity)
                return false;
            BigInteger bigInteger2 = ecPoint.AffineXCoord.ToBigInteger();
            return r.Subtract( bigInteger2 ).Mod( n ).Equals( bigInteger1 );
        }
    }
}
