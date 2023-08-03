// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.FixedPointCombMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class FixedPointCombMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            ECCurve curve = p.Curve;
            int combSize = FixedPointUtilities.GetCombSize( curve );
            if (k.BitLength > combSize)
                throw new InvalidOperationException( "fixed-point comb doesn't support scalars larger than the curve order" );
            int widthForCombSize = this.GetWidthForCombSize( combSize );
            FixedPointPreCompInfo pointPreCompInfo = FixedPointUtilities.Precompute( p, widthForCombSize );
            ECPoint[] preComp = pointPreCompInfo.PreComp;
            int width = pointPreCompInfo.Width;
            int num1 = (combSize + width - 1) / width;
            ECPoint ecPoint = curve.Infinity;
            int num2 = (num1 * width) - 1;
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = 0;
                for (int n = num2 - index1; n >= 0; n -= num1)
                {
                    index2 <<= 1;
                    if (k.TestBit( n ))
                        index2 |= 1;
                }
                ecPoint = ecPoint.TwicePlus( preComp[index2] );
            }
            return ecPoint;
        }

        protected virtual int GetWidthForCombSize( int combSize ) => combSize <= 257 ? 5 : 6;
    }
}
