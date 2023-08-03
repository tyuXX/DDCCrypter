// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.WNafUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public abstract class WNafUtilities
    {
        public static readonly string PRECOMP_NAME = "bc_wnaf";
        private static readonly int[] DEFAULT_WINDOW_SIZE_CUTOFFS = new int[6]
        {
      13,
      41,
      121,
      337,
      897,
      2305
        };
        private static readonly byte[] EMPTY_BYTES = new byte[0];
        private static readonly int[] EMPTY_INTS = new int[0];
        private static readonly ECPoint[] EMPTY_POINTS = new ECPoint[0];

        public static int[] GenerateCompactNaf( BigInteger k )
        {
            if (k.BitLength >> 16 != 0)
                throw new ArgumentException( "must have bitlength < 2^16", nameof( k ) );
            if (k.SignValue == 0)
                return EMPTY_INTS;
            BigInteger bigInteger1 = k.ShiftLeft( 1 ).Add( k );
            int bitLength = bigInteger1.BitLength;
            int[] a = new int[bitLength >> 1];
            BigInteger bigInteger2 = bigInteger1.Xor( k );
            int num1 = bitLength - 1;
            int num2 = 0;
            int num3 = 0;
            for (int n = 1; n < num1; ++n)
            {
                if (!bigInteger2.TestBit( n ))
                {
                    ++num3;
                }
                else
                {
                    int num4 = k.TestBit( n ) ? -1 : 1;
                    a[num2++] = (num4 << 16) | num3;
                    num3 = 1;
                    ++n;
                }
            }
            int[] numArray = a;
            int index = num2;
            int length = index + 1;
            int num5 = 65536 | num3;
            numArray[index] = num5;
            if (a.Length > length)
                a = Trim( a, length );
            return a;
        }

        public static int[] GenerateCompactWindowNaf( int width, BigInteger k )
        {
            if (width == 2)
                return GenerateCompactNaf( k );
            if (width < 2 || width > 16)
                throw new ArgumentException( "must be in the range [2, 16]", nameof( width ) );
            if (k.BitLength >> 16 != 0)
                throw new ArgumentException( "must have bitlength < 2^16", nameof( k ) );
            if (k.SignValue == 0)
                return EMPTY_INTS;
            int[] a = new int[(k.BitLength / width) + 1];
            int num1 = 1 << width;
            int num2 = num1 - 1;
            int num3 = num1 >> 1;
            bool flag = false;
            int length = 0;
            int n = 0;
            while (n <= k.BitLength)
            {
                if (k.TestBit( n ) == flag)
                {
                    ++n;
                }
                else
                {
                    k = k.ShiftRight( n );
                    int num4 = k.IntValue & num2;
                    if (flag)
                        ++num4;
                    flag = (num4 & num3) != 0;
                    if (flag)
                        num4 -= num1;
                    int num5 = length > 0 ? n - 1 : n;
                    a[length++] = (num4 << 16) | num5;
                    n = width;
                }
            }
            if (a.Length > length)
                a = Trim( a, length );
            return a;
        }

        public static byte[] GenerateJsf( BigInteger g, BigInteger h )
        {
            byte[] a = new byte[System.Math.Max( g.BitLength, h.BitLength ) + 1];
            BigInteger bigInteger1 = g;
            BigInteger bigInteger2 = h;
            int length = 0;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            while ((num1 | num2) != 0 || bigInteger1.BitLength > num3 || bigInteger2.BitLength > num3)
            {
                int num4 = ((bigInteger1.IntValue >>> num3) + num1) & 7;
                int num5 = ((bigInteger2.IntValue >>> num3) + num2) & 7;
                int num6 = num4 & 1;
                if (num6 != 0)
                {
                    num6 -= num4 & 2;
                    if (num4 + num6 == 4 && (num5 & 3) == 2)
                        num6 = -num6;
                }
                int num7 = num5 & 1;
                if (num7 != 0)
                {
                    num7 -= num5 & 2;
                    if (num5 + num7 == 4 && (num4 & 3) == 2)
                        num7 = -num7;
                }
                if (num1 << 1 == 1 + num6)
                    num1 ^= 1;
                if (num2 << 1 == 1 + num7)
                    num2 ^= 1;
                if (++num3 == 30)
                {
                    num3 = 0;
                    bigInteger1 = bigInteger1.ShiftRight( 30 );
                    bigInteger2 = bigInteger2.ShiftRight( 30 );
                }
                a[length++] = (byte)((num6 << 4) | (num7 & 15));
            }
            if (a.Length > length)
                a = Trim( a, length );
            return a;
        }

        public static byte[] GenerateNaf( BigInteger k )
        {
            if (k.SignValue == 0)
                return EMPTY_BYTES;
            BigInteger bigInteger1 = k.ShiftLeft( 1 ).Add( k );
            int length = bigInteger1.BitLength - 1;
            byte[] naf = new byte[length];
            BigInteger bigInteger2 = bigInteger1.Xor( k );
            for (int n = 1; n < length; ++n)
            {
                if (bigInteger2.TestBit( n ))
                {
                    naf[n - 1] = k.TestBit( n ) ? byte.MaxValue : (byte)1;
                    ++n;
                }
            }
            naf[length - 1] = 1;
            return naf;
        }

        public static byte[] GenerateWindowNaf( int width, BigInteger k )
        {
            if (width == 2)
                return GenerateNaf( k );
            if (width < 2 || width > 8)
                throw new ArgumentException( "must be in the range [2, 8]", nameof( width ) );
            if (k.SignValue == 0)
                return EMPTY_BYTES;
            byte[] a = new byte[k.BitLength + 1];
            int num1 = 1 << width;
            int num2 = num1 - 1;
            int num3 = num1 >> 1;
            bool flag = false;
            int length = 0;
            int n = 0;
            while (n <= k.BitLength)
            {
                if (k.TestBit( n ) == flag)
                {
                    ++n;
                }
                else
                {
                    k = k.ShiftRight( n );
                    int num4 = k.IntValue & num2;
                    if (flag)
                        ++num4;
                    flag = (num4 & num3) != 0;
                    if (flag)
                        num4 -= num1;
                    int num5 = length + (length > 0 ? n - 1 : n);
                    byte[] numArray = a;
                    int index = num5;
                    length = index + 1;
                    int num6 = (byte)num4;
                    numArray[index] = (byte)num6;
                    n = width;
                }
            }
            if (a.Length > length)
                a = Trim( a, length );
            return a;
        }

        public static int GetNafWeight( BigInteger k ) => k.SignValue == 0 ? 0 : k.ShiftLeft( 1 ).Add( k ).Xor( k ).BitCount;

        public static WNafPreCompInfo GetWNafPreCompInfo( ECPoint p ) => GetWNafPreCompInfo( p.Curve.GetPreCompInfo( p, PRECOMP_NAME ) );

        public static WNafPreCompInfo GetWNafPreCompInfo( PreCompInfo preCompInfo ) => preCompInfo != null && preCompInfo is WNafPreCompInfo ? (WNafPreCompInfo)preCompInfo : new WNafPreCompInfo();

        public static int GetWindowSize( int bits ) => GetWindowSize( bits, DEFAULT_WINDOW_SIZE_CUTOFFS );

        public static int GetWindowSize( int bits, int[] windowSizeCutoffs )
        {
            int index = 0;
            while (index < windowSizeCutoffs.Length && bits >= windowSizeCutoffs[index])
                ++index;
            return index + 2;
        }

        public static ECPoint MapPointWithPrecomp(
          ECPoint p,
          int width,
          bool includeNegated,
          ECPointMap pointMap )
        {
            ECCurve curve = p.Curve;
            WNafPreCompInfo wnafPreCompInfo1 = Precompute( p, width, includeNegated );
            ECPoint point = pointMap.Map( p );
            WNafPreCompInfo wnafPreCompInfo2 = GetWNafPreCompInfo( curve.GetPreCompInfo( point, PRECOMP_NAME ) );
            ECPoint twice = wnafPreCompInfo1.Twice;
            if (twice != null)
            {
                ECPoint ecPoint = pointMap.Map( twice );
                wnafPreCompInfo2.Twice = ecPoint;
            }
            ECPoint[] preComp = wnafPreCompInfo1.PreComp;
            ECPoint[] ecPointArray1 = new ECPoint[preComp.Length];
            for (int index = 0; index < preComp.Length; ++index)
                ecPointArray1[index] = pointMap.Map( preComp[index] );
            wnafPreCompInfo2.PreComp = ecPointArray1;
            if (includeNegated)
            {
                ECPoint[] ecPointArray2 = new ECPoint[ecPointArray1.Length];
                for (int index = 0; index < ecPointArray2.Length; ++index)
                    ecPointArray2[index] = ecPointArray1[index].Negate();
                wnafPreCompInfo2.PreCompNeg = ecPointArray2;
            }
            curve.SetPreCompInfo( point, PRECOMP_NAME, wnafPreCompInfo2 );
            return point;
        }

        public static WNafPreCompInfo Precompute( ECPoint p, int width, bool includeNegated )
        {
            ECCurve curve = p.Curve;
            WNafPreCompInfo wnafPreCompInfo = GetWNafPreCompInfo( curve.GetPreCompInfo( p, PRECOMP_NAME ) );
            int off = 0;
            int length = 1 << System.Math.Max( 0, width - 2 );
            ECPoint[] ecPointArray = wnafPreCompInfo.PreComp;
            if (ecPointArray == null)
                ecPointArray = EMPTY_POINTS;
            else
                off = ecPointArray.Length;
            if (off < length)
            {
                ecPointArray = ResizeTable( ecPointArray, length );
                if (length == 1)
                {
                    ecPointArray[0] = p.Normalize();
                }
                else
                {
                    int num = off;
                    if (num == 0)
                    {
                        ecPointArray[0] = p;
                        num = 1;
                    }
                    ECFieldElement ecFieldElement = null;
                    if (length == 2)
                    {
                        ecPointArray[1] = p.ThreeTimes();
                    }
                    else
                    {
                        ECPoint b = wnafPreCompInfo.Twice;
                        ECPoint ecPoint = ecPointArray[num - 1];
                        if (b == null)
                        {
                            b = ecPointArray[0].Twice();
                            wnafPreCompInfo.Twice = b;
                            if (ECAlgorithms.IsFpCurve( curve ) && curve.FieldSize >= 64)
                            {
                                switch (curve.CoordinateSystem)
                                {
                                    case 2:
                                    case 3:
                                    case 4:
                                        ecFieldElement = b.GetZCoord( 0 );
                                        b = curve.CreatePoint( b.XCoord.ToBigInteger(), b.YCoord.ToBigInteger() );
                                        ECFieldElement scale1 = ecFieldElement.Square();
                                        ECFieldElement scale2 = scale1.Multiply( ecFieldElement );
                                        ecPoint = ecPoint.ScaleX( scale1 ).ScaleY( scale2 );
                                        if (off == 0)
                                        {
                                            ecPointArray[0] = ecPoint;
                                            break;
                                        }
                                        break;
                                }
                            }
                        }
                        while (num < length)
                            ecPointArray[num++] = ecPoint = ecPoint.Add( b );
                    }
                    curve.NormalizeAll( ecPointArray, off, length - off, ecFieldElement );
                }
            }
            wnafPreCompInfo.PreComp = ecPointArray;
            if (includeNegated)
            {
                ECPoint[] a = wnafPreCompInfo.PreCompNeg;
                int index;
                if (a == null)
                {
                    index = 0;
                    a = new ECPoint[length];
                }
                else
                {
                    index = a.Length;
                    if (index < length)
                        a = ResizeTable( a, length );
                }
                for (; index < length; ++index)
                    a[index] = ecPointArray[index].Negate();
                wnafPreCompInfo.PreCompNeg = a;
            }
            curve.SetPreCompInfo( p, PRECOMP_NAME, wnafPreCompInfo );
            return wnafPreCompInfo;
        }

        private static byte[] Trim( byte[] a, int length )
        {
            byte[] destinationArray = new byte[length];
            Array.Copy( a, 0, destinationArray, 0, destinationArray.Length );
            return destinationArray;
        }

        private static int[] Trim( int[] a, int length )
        {
            int[] destinationArray = new int[length];
            Array.Copy( a, 0, destinationArray, 0, destinationArray.Length );
            return destinationArray;
        }

        private static ECPoint[] ResizeTable( ECPoint[] a, int length )
        {
            ECPoint[] destinationArray = new ECPoint[length];
            Array.Copy( a, 0, destinationArray, 0, a.Length );
            return destinationArray;
        }
    }
}
