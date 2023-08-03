// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.WNafL2RMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class WNafL2RMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            int width = System.Math.Max( 2, System.Math.Min( 16, this.GetWindowSize( k.BitLength ) ) );
            WNafPreCompInfo wnafPreCompInfo = WNafUtilities.Precompute( p, width, true );
            ECPoint[] preComp = wnafPreCompInfo.PreComp;
            ECPoint[] preCompNeg = wnafPreCompInfo.PreCompNeg;
            int[] compactWindowNaf = WNafUtilities.GenerateCompactWindowNaf( width, k );
            ECPoint ecPoint1 = p.Curve.Infinity;
            int length = compactWindowNaf.Length;
            if (length > 1)
            {
                int num1 = compactWindowNaf[--length];
                int num2 = num1 >> 16;
                int e = num1 & ushort.MaxValue;
                int index = System.Math.Abs( num2 );
                ECPoint[] ecPointArray = num2 < 0 ? preCompNeg : preComp;
                ECPoint ecPoint2;
                if (index << 2 < 1 << width)
                {
                    int bitLength = LongArray.BitLengths[index];
                    int num3 = width - bitLength;
                    int num4 = index ^ (1 << (bitLength - 1));
                    int num5 = (1 << (width - 1)) - 1;
                    int num6 = (num4 << num3) + 1;
                    ecPoint2 = ecPointArray[num5 >> 1].Add( ecPointArray[num6 >> 1] );
                    e -= num3;
                }
                else
                    ecPoint2 = ecPointArray[index >> 1];
                ecPoint1 = ecPoint2.TimesPow2( e );
            }
            while (length > 0)
            {
                int num7 = compactWindowNaf[--length];
                int num8 = num7 >> 16;
                int e = num7 & ushort.MaxValue;
                int num9 = System.Math.Abs( num8 );
                ECPoint b = (num8 < 0 ? preCompNeg : preComp)[num9 >> 1];
                ecPoint1 = ecPoint1.TwicePlus( b ).TimesPow2( e );
            }
            return ecPoint1;
        }

        protected virtual int GetWindowSize( int bits ) => WNafUtilities.GetWindowSize( bits );
    }
}
