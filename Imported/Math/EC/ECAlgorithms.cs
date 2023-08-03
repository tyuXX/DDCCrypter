// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.ECAlgorithms
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Endo;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Math.EC
{
    public class ECAlgorithms
    {
        public static bool IsF2mCurve( ECCurve c ) => IsF2mField( c.Field );

        public static bool IsF2mField( IFiniteField field ) => field.Dimension > 1 && field.Characteristic.Equals( BigInteger.Two ) && field is IPolynomialExtensionField;

        public static bool IsFpCurve( ECCurve c ) => IsFpField( c.Field );

        public static bool IsFpField( IFiniteField field ) => field.Dimension == 1;

        public static ECPoint SumOfMultiplies( ECPoint[] ps, BigInteger[] ks )
        {
            if (ps == null || ks == null || ps.Length != ks.Length || ps.Length < 1)
                throw new ArgumentException( "point and scalar arrays should be non-null, and of equal, non-zero, length" );
            int length = ps.Length;
            switch (length)
            {
                case 1:
                    return ps[0].Multiply( ks[0] );
                case 2:
                    return SumOfTwoMultiplies( ps[0], ks[0], ps[1], ks[1] );
                default:
                    ECPoint p = ps[0];
                    ECCurve curve = p.Curve;
                    ECPoint[] ps1 = new ECPoint[length];
                    ps1[0] = p;
                    for (int index = 1; index < length; ++index)
                        ps1[index] = ImportPoint( curve, ps[index] );
                    return curve.GetEndomorphism() is GlvEndomorphism endomorphism ? ValidatePoint( ImplSumOfMultipliesGlv( ps1, ks, endomorphism ) ) : ValidatePoint( ImplSumOfMultiplies( ps1, ks ) );
            }
        }

        public static ECPoint SumOfTwoMultiplies( ECPoint P, BigInteger a, ECPoint Q, BigInteger b )
        {
            ECCurve curve = P.Curve;
            Q = ImportPoint( curve, Q );
            if (curve is AbstractF2mCurve abstractF2mCurve && abstractF2mCurve.IsKoblitz)
                return ValidatePoint( P.Multiply( a ).Add( Q.Multiply( b ) ) );
            if (!(curve.GetEndomorphism() is GlvEndomorphism endomorphism))
                return ValidatePoint( ImplShamirsTrickWNaf( P, a, Q, b ) );
            return ValidatePoint( ImplSumOfMultipliesGlv( new ECPoint[2]
            {
        P,
        Q
            }, new BigInteger[2] { a, b }, endomorphism ) );
        }

        public static ECPoint ShamirsTrick( ECPoint P, BigInteger k, ECPoint Q, BigInteger l )
        {
            Q = ImportPoint( P.Curve, Q );
            return ValidatePoint( ImplShamirsTrickJsf( P, k, Q, l ) );
        }

        public static ECPoint ImportPoint( ECCurve c, ECPoint p )
        {
            ECCurve curve = p.Curve;
            return c.Equals( curve ) ? c.ImportPoint( p ) : throw new ArgumentException( "Point must be on the same curve" );
        }

        public static void MontgomeryTrick( ECFieldElement[] zs, int off, int len ) => MontgomeryTrick( zs, off, len, null );

        public static void MontgomeryTrick(
          ECFieldElement[] zs,
          int off,
          int len,
          ECFieldElement scale )
        {
            ECFieldElement[] ecFieldElementArray = new ECFieldElement[len];
            ecFieldElementArray[0] = zs[off];
            int index1 = 0;
            while (++index1 < len)
                ecFieldElementArray[index1] = ecFieldElementArray[index1 - 1].Multiply( zs[off + index1] );
            int index2 = index1 - 1;
            if (scale != null)
                ecFieldElementArray[index2] = ecFieldElementArray[index2].Multiply( scale );
            ECFieldElement b = ecFieldElementArray[index2].Invert();
            while (index2 > 0)
            {
                int index3 = off + index2--;
                ECFieldElement z = zs[index3];
                zs[index3] = ecFieldElementArray[index2].Multiply( b );
                b = b.Multiply( z );
            }
            zs[off] = b;
        }

        public static ECPoint ReferenceMultiply( ECPoint p, BigInteger k )
        {
            BigInteger bigInteger = k.Abs();
            ECPoint ecPoint = p.Curve.Infinity;
            int bitLength = bigInteger.BitLength;
            if (bitLength > 0)
            {
                if (bigInteger.TestBit( 0 ))
                    ecPoint = p;
                for (int n = 1; n < bitLength; ++n)
                {
                    p = p.Twice();
                    if (bigInteger.TestBit( n ))
                        ecPoint = ecPoint.Add( p );
                }
            }
            return k.SignValue >= 0 ? ecPoint : ecPoint.Negate();
        }

        public static ECPoint ValidatePoint( ECPoint p ) => p.IsValid() ? p : throw new ArgumentException( "Invalid point", nameof( p ) );

        internal static ECPoint ImplShamirsTrickJsf( ECPoint P, BigInteger k, ECPoint Q, BigInteger l )
        {
            ECCurve curve = P.Curve;
            ECPoint infinity = curve.Infinity;
            ECPoint ecPoint1 = P.Add( Q );
            ECPoint ecPoint2 = P.Subtract( Q );
            ECPoint[] points = new ECPoint[4]
            {
        Q,
        ecPoint2,
        P,
        ecPoint1
            };
            curve.NormalizeAll( points );
            ECPoint[] ecPointArray = new ECPoint[9]
            {
        points[3].Negate(),
        points[2].Negate(),
        points[1].Negate(),
        points[0].Negate(),
        infinity,
        points[0],
        points[1],
        points[2],
        points[3]
            };
            byte[] jsf = WNafUtilities.GenerateJsf( k, l );
            ECPoint ecPoint3 = infinity;
            int length = jsf.Length;
            while (--length >= 0)
            {
                int num = jsf[length];
                int index = 4 + ((num << 24 >> 28) * 3) + (num << 28 >> 28);
                ecPoint3 = ecPoint3.TwicePlus( ecPointArray[index] );
            }
            return ecPoint3;
        }

        internal static ECPoint ImplShamirsTrickWNaf( ECPoint P, BigInteger k, ECPoint Q, BigInteger l )
        {
            bool flag1 = k.SignValue < 0;
            bool flag2 = l.SignValue < 0;
            k = k.Abs();
            l = l.Abs();
            int width1 = System.Math.Max( 2, System.Math.Min( 16, WNafUtilities.GetWindowSize( k.BitLength ) ) );
            int width2 = System.Math.Max( 2, System.Math.Min( 16, WNafUtilities.GetWindowSize( l.BitLength ) ) );
            WNafPreCompInfo wnafPreCompInfo1 = WNafUtilities.Precompute( P, width1, true );
            WNafPreCompInfo wnafPreCompInfo2 = WNafUtilities.Precompute( Q, width2, true );
            ECPoint[] preCompP = flag1 ? wnafPreCompInfo1.PreCompNeg : wnafPreCompInfo1.PreComp;
            ECPoint[] preCompQ = flag2 ? wnafPreCompInfo2.PreCompNeg : wnafPreCompInfo2.PreComp;
            ECPoint[] preCompNegP = flag1 ? wnafPreCompInfo1.PreComp : wnafPreCompInfo1.PreCompNeg;
            ECPoint[] preCompNegQ = flag2 ? wnafPreCompInfo2.PreComp : wnafPreCompInfo2.PreCompNeg;
            byte[] windowNaf1 = WNafUtilities.GenerateWindowNaf( width1, k );
            byte[] windowNaf2 = WNafUtilities.GenerateWindowNaf( width2, l );
            return ImplShamirsTrickWNaf( preCompP, preCompNegP, windowNaf1, preCompQ, preCompNegQ, windowNaf2 );
        }

        internal static ECPoint ImplShamirsTrickWNaf(
          ECPoint P,
          BigInteger k,
          ECPointMap pointMapQ,
          BigInteger l )
        {
            bool flag1 = k.SignValue < 0;
            bool flag2 = l.SignValue < 0;
            k = k.Abs();
            l = l.Abs();
            int width = System.Math.Max( 2, System.Math.Min( 16, WNafUtilities.GetWindowSize( System.Math.Max( k.BitLength, l.BitLength ) ) ) );
            ECPoint p = WNafUtilities.MapPointWithPrecomp( P, width, true, pointMapQ );
            WNafPreCompInfo wnafPreCompInfo1 = WNafUtilities.GetWNafPreCompInfo( P );
            WNafPreCompInfo wnafPreCompInfo2 = WNafUtilities.GetWNafPreCompInfo( p );
            ECPoint[] preCompP = flag1 ? wnafPreCompInfo1.PreCompNeg : wnafPreCompInfo1.PreComp;
            ECPoint[] preCompQ = flag2 ? wnafPreCompInfo2.PreCompNeg : wnafPreCompInfo2.PreComp;
            ECPoint[] preCompNegP = flag1 ? wnafPreCompInfo1.PreComp : wnafPreCompInfo1.PreCompNeg;
            ECPoint[] preCompNegQ = flag2 ? wnafPreCompInfo2.PreComp : wnafPreCompInfo2.PreCompNeg;
            byte[] windowNaf1 = WNafUtilities.GenerateWindowNaf( width, k );
            byte[] windowNaf2 = WNafUtilities.GenerateWindowNaf( width, l );
            return ImplShamirsTrickWNaf( preCompP, preCompNegP, windowNaf1, preCompQ, preCompNegQ, windowNaf2 );
        }

        private static ECPoint ImplShamirsTrickWNaf(
          ECPoint[] preCompP,
          ECPoint[] preCompNegP,
          byte[] wnafP,
          ECPoint[] preCompQ,
          ECPoint[] preCompNegQ,
          byte[] wnafQ )
        {
            int num1 = System.Math.Max( wnafP.Length, wnafQ.Length );
            ECPoint infinity = preCompP[0].Curve.Infinity;
            ECPoint ecPoint = infinity;
            int e = 0;
            for (int index = num1 - 1; index >= 0; --index)
            {
                int num2 = index < wnafP.Length ? (sbyte)wnafP[index] : 0;
                int num3 = index < wnafQ.Length ? (sbyte)wnafQ[index] : 0;
                if ((num2 | num3) == 0)
                {
                    ++e;
                }
                else
                {
                    ECPoint b = infinity;
                    if (num2 != 0)
                    {
                        int num4 = System.Math.Abs( num2 );
                        ECPoint[] ecPointArray = num2 < 0 ? preCompNegP : preCompP;
                        b = b.Add( ecPointArray[num4 >> 1] );
                    }
                    if (num3 != 0)
                    {
                        int num5 = System.Math.Abs( num3 );
                        ECPoint[] ecPointArray = num3 < 0 ? preCompNegQ : preCompQ;
                        b = b.Add( ecPointArray[num5 >> 1] );
                    }
                    if (e > 0)
                    {
                        ecPoint = ecPoint.TimesPow2( e );
                        e = 0;
                    }
                    ecPoint = ecPoint.TwicePlus( b );
                }
            }
            if (e > 0)
                ecPoint = ecPoint.TimesPow2( e );
            return ecPoint;
        }

        internal static ECPoint ImplSumOfMultiplies( ECPoint[] ps, BigInteger[] ks )
        {
            int length = ps.Length;
            bool[] negs = new bool[length];
            WNafPreCompInfo[] infos = new WNafPreCompInfo[length];
            byte[][] wnafs = new byte[length][];
            for (int index = 0; index < length; ++index)
            {
                BigInteger k1 = ks[index];
                negs[index] = k1.SignValue < 0;
                BigInteger k2 = k1.Abs();
                int width = System.Math.Max( 2, System.Math.Min( 16, WNafUtilities.GetWindowSize( k2.BitLength ) ) );
                infos[index] = WNafUtilities.Precompute( ps[index], width, true );
                wnafs[index] = WNafUtilities.GenerateWindowNaf( width, k2 );
            }
            return ImplSumOfMultiplies( negs, infos, wnafs );
        }

        internal static ECPoint ImplSumOfMultipliesGlv(
          ECPoint[] ps,
          BigInteger[] ks,
          GlvEndomorphism glvEndomorphism )
        {
            BigInteger order = ps[0].Curve.Order;
            int length = ps.Length;
            BigInteger[] ks1 = new BigInteger[length << 1];
            int index1 = 0;
            int num1 = 0;
            for (; index1 < length; ++index1)
            {
                BigInteger[] bigIntegerArray1 = glvEndomorphism.DecomposeScalar( ks[index1].Mod( order ) );
                BigInteger[] bigIntegerArray2 = ks1;
                int index2 = num1;
                int num2 = index2 + 1;
                BigInteger bigInteger1 = bigIntegerArray1[0];
                bigIntegerArray2[index2] = bigInteger1;
                BigInteger[] bigIntegerArray3 = ks1;
                int index3 = num2;
                num1 = index3 + 1;
                BigInteger bigInteger2 = bigIntegerArray1[1];
                bigIntegerArray3[index3] = bigInteger2;
            }
            ECPointMap pointMap = glvEndomorphism.PointMap;
            if (glvEndomorphism.HasEfficientPointMap)
                return ImplSumOfMultiplies( ps, pointMap, ks1 );
            ECPoint[] ps1 = new ECPoint[length << 1];
            int index4 = 0;
            int num3 = 0;
            for (; index4 < length; ++index4)
            {
                ECPoint p = ps[index4];
                ECPoint ecPoint1 = pointMap.Map( p );
                ECPoint[] ecPointArray1 = ps1;
                int index5 = num3;
                int num4 = index5 + 1;
                ECPoint ecPoint2 = p;
                ecPointArray1[index5] = ecPoint2;
                ECPoint[] ecPointArray2 = ps1;
                int index6 = num4;
                num3 = index6 + 1;
                ECPoint ecPoint3 = ecPoint1;
                ecPointArray2[index6] = ecPoint3;
            }
            return ImplSumOfMultiplies( ps1, ks1 );
        }

        internal static ECPoint ImplSumOfMultiplies( ECPoint[] ps, ECPointMap pointMap, BigInteger[] ks )
        {
            int length1 = ps.Length;
            int length2 = length1 << 1;
            bool[] negs = new bool[length2];
            WNafPreCompInfo[] infos = new WNafPreCompInfo[length2];
            byte[][] wnafs = new byte[length2][];
            for (int index1 = 0; index1 < length1; ++index1)
            {
                int index2 = index1 << 1;
                int index3 = index2 + 1;
                BigInteger k1 = ks[index2];
                negs[index2] = k1.SignValue < 0;
                BigInteger k2 = k1.Abs();
                BigInteger k3 = ks[index3];
                negs[index3] = k3.SignValue < 0;
                BigInteger k4 = k3.Abs();
                int width = System.Math.Max( 2, System.Math.Min( 16, WNafUtilities.GetWindowSize( System.Math.Max( k2.BitLength, k4.BitLength ) ) ) );
                ECPoint p1 = ps[index1];
                ECPoint p2 = WNafUtilities.MapPointWithPrecomp( p1, width, true, pointMap );
                infos[index2] = WNafUtilities.GetWNafPreCompInfo( p1 );
                infos[index3] = WNafUtilities.GetWNafPreCompInfo( p2 );
                wnafs[index2] = WNafUtilities.GenerateWindowNaf( width, k2 );
                wnafs[index3] = WNafUtilities.GenerateWindowNaf( width, k4 );
            }
            return ImplSumOfMultiplies( negs, infos, wnafs );
        }

        private static ECPoint ImplSumOfMultiplies(
          bool[] negs,
          WNafPreCompInfo[] infos,
          byte[][] wnafs )
        {
            int val1 = 0;
            int length = wnafs.Length;
            for (int index = 0; index < length; ++index)
                val1 = System.Math.Max( val1, wnafs[index].Length );
            ECPoint infinity = infos[0].PreComp[0].Curve.Infinity;
            ECPoint ecPoint = infinity;
            int e = 0;
            for (int index1 = val1 - 1; index1 >= 0; --index1)
            {
                ECPoint b = infinity;
                for (int index2 = 0; index2 < length; ++index2)
                {
                    byte[] wnaf = wnafs[index2];
                    int num1 = index1 < wnaf.Length ? (sbyte)wnaf[index1] : 0;
                    if (num1 != 0)
                    {
                        int num2 = System.Math.Abs( num1 );
                        WNafPreCompInfo info = infos[index2];
                        ECPoint[] ecPointArray = (num1 < 0) == negs[index2] ? info.PreComp : info.PreCompNeg;
                        b = b.Add( ecPointArray[num2 >> 1] );
                    }
                }
                if (b == infinity)
                {
                    ++e;
                }
                else
                {
                    if (e > 0)
                    {
                        ecPoint = ecPoint.TimesPow2( e );
                        e = 0;
                    }
                    ecPoint = ecPoint.TwicePlus( b );
                }
            }
            if (e > 0)
                ecPoint = ecPoint.TimesPow2( e );
            return ecPoint;
        }
    }
}
