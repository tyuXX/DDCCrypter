// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Endo.GlvTypeBEndomorphism
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Endo
{
    public class GlvTypeBEndomorphism : GlvEndomorphism, ECEndomorphism
    {
        protected readonly ECCurve m_curve;
        protected readonly GlvTypeBParameters m_parameters;
        protected readonly ECPointMap m_pointMap;

        public GlvTypeBEndomorphism( ECCurve curve, GlvTypeBParameters parameters )
        {
            this.m_curve = curve;
            this.m_parameters = parameters;
            this.m_pointMap = new ScaleXPointMap( curve.FromBigInteger( parameters.Beta ) );
        }

        public virtual BigInteger[] DecomposeScalar( BigInteger k )
        {
            int bits = this.m_parameters.Bits;
            BigInteger b1 = this.CalculateB( k, this.m_parameters.G1, bits );
            BigInteger b2 = this.CalculateB( k, this.m_parameters.G2, bits );
            BigInteger[] v1 = this.m_parameters.V1;
            BigInteger[] v2 = this.m_parameters.V2;
            return new BigInteger[2]
            {
        k.Subtract(b1.Multiply(v1[0]).Add(b2.Multiply(v2[0]))),
        b1.Multiply(v1[1]).Add(b2.Multiply(v2[1])).Negate()
            };
        }

        public virtual ECPointMap PointMap => this.m_pointMap;

        public virtual bool HasEfficientPointMap => true;

        protected virtual BigInteger CalculateB( BigInteger k, BigInteger g, int t )
        {
            bool flag1 = g.SignValue < 0;
            BigInteger bigInteger1 = k.Multiply( g.Abs() );
            bool flag2 = bigInteger1.TestBit( t - 1 );
            BigInteger bigInteger2 = bigInteger1.ShiftRight( t );
            if (flag2)
                bigInteger2 = bigInteger2.Add( BigInteger.One );
            return !flag1 ? bigInteger2 : bigInteger2.Negate();
        }
    }
}
