// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.FixedPointUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class FixedPointUtilities
    {
        public static readonly string PRECOMP_NAME = "bc_fixed_point";

        public static int GetCombSize( ECCurve c )
        {
            BigInteger order = c.Order;
            return order != null ? order.BitLength : c.FieldSize + 1;
        }

        public static FixedPointPreCompInfo GetFixedPointPreCompInfo( PreCompInfo preCompInfo ) => preCompInfo != null && preCompInfo is FixedPointPreCompInfo ? (FixedPointPreCompInfo)preCompInfo : new FixedPointPreCompInfo();

        public static FixedPointPreCompInfo Precompute( ECPoint p, int minWidth )
        {
            ECCurve curve = p.Curve;
            int length = 1 << minWidth;
            FixedPointPreCompInfo pointPreCompInfo = GetFixedPointPreCompInfo( curve.GetPreCompInfo( p, PRECOMP_NAME ) );
            ECPoint[] preComp = pointPreCompInfo.PreComp;
            if (preComp == null || preComp.Length < length)
            {
                int e = (GetCombSize( curve ) + minWidth - 1) / minWidth;
                ECPoint[] points1 = new ECPoint[minWidth];
                points1[0] = p;
                for (int index = 1; index < minWidth; ++index)
                    points1[index] = points1[index - 1].TimesPow2( e );
                curve.NormalizeAll( points1 );
                ECPoint[] points2 = new ECPoint[length];
                points2[0] = curve.Infinity;
                for (int index1 = minWidth - 1; index1 >= 0; --index1)
                {
                    ECPoint b = points1[index1];
                    int num = 1 << index1;
                    for (int index2 = num; index2 < length; index2 += num << 1)
                        points2[index2] = points2[index2 - num].Add( b );
                }
                curve.NormalizeAll( points2 );
                pointPreCompInfo.PreComp = points2;
                pointPreCompInfo.Width = minWidth;
                curve.SetPreCompInfo( p, PRECOMP_NAME, pointPreCompInfo );
            }
            return pointPreCompInfo;
        }
    }
}
