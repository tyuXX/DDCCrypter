// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.ECMqvBasicAgreement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;

namespace Org.BouncyCastle.Crypto.Agreement
{
    public class ECMqvBasicAgreement : IBasicAgreement
    {
        protected internal MqvPrivateParameters privParams;

        public virtual void Init( ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.privParams = (MqvPrivateParameters)parameters;
        }

        public virtual int GetFieldSize() => (this.privParams.StaticPrivateKey.Parameters.Curve.FieldSize + 7) / 8;

        public virtual BigInteger CalculateAgreement( ICipherParameters pubKey )
        {
            MqvPublicParameters publicParameters = (MqvPublicParameters)pubKey;
            ECPrivateKeyParameters staticPrivateKey = this.privParams.StaticPrivateKey;
            ECPoint ecPoint = CalculateMqvAgreement( staticPrivateKey.Parameters, staticPrivateKey, this.privParams.EphemeralPrivateKey, this.privParams.EphemeralPublicKey, publicParameters.StaticPublicKey, publicParameters.EphemeralPublicKey ).Normalize();
            return !ecPoint.IsInfinity ? ecPoint.AffineXCoord.ToBigInteger() : throw new InvalidOperationException( "Infinity is not a valid agreement value for MQV" );
        }

        private static ECPoint CalculateMqvAgreement(
          ECDomainParameters parameters,
          ECPrivateKeyParameters d1U,
          ECPrivateKeyParameters d2U,
          ECPublicKeyParameters Q2U,
          ECPublicKeyParameters Q1V,
          ECPublicKeyParameters Q2V )
        {
            BigInteger n1 = parameters.N;
            int n2 = (n1.BitLength + 1) / 2;
            BigInteger m = BigInteger.One.ShiftLeft( n2 );
            ECCurve curve = parameters.Curve;
            ECPoint[] points = new ECPoint[3]
            {
        ECAlgorithms.ImportPoint(curve, Q2U == null ? parameters.G.Multiply(d2U.D) : Q2U.Q),
        ECAlgorithms.ImportPoint(curve, Q1V.Q),
        ECAlgorithms.ImportPoint(curve, Q2V.Q)
            };
            curve.NormalizeAll( points );
            ECPoint ecPoint = points[0];
            ECPoint P = points[1];
            ECPoint Q = points[2];
            BigInteger val1 = ecPoint.AffineXCoord.ToBigInteger().Mod( m ).SetBit( n2 );
            BigInteger val2 = d1U.D.Multiply( val1 ).Add( d2U.D ).Mod( n1 );
            BigInteger bigInteger1 = Q.AffineXCoord.ToBigInteger().Mod( m ).SetBit( n2 );
            BigInteger bigInteger2 = parameters.H.Multiply( val2 ).Mod( n1 );
            return ECAlgorithms.SumOfTwoMultiplies( P, bigInteger1.Multiply( bigInteger2 ).Mod( n1 ), Q, bigInteger2 );
        }
    }
}
