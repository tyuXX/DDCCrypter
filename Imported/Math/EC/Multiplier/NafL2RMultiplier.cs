// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.NafL2RMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class NafL2RMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            int[] compactNaf = WNafUtilities.GenerateCompactNaf( k );
            ECPoint ecPoint1 = p.Normalize();
            ECPoint ecPoint2 = ecPoint1.Negate();
            ECPoint ecPoint3 = p.Curve.Infinity;
            int length = compactNaf.Length;
            while (--length >= 0)
            {
                int num1 = compactNaf[length];
                int num2 = num1 >> 16;
                int e = num1 & ushort.MaxValue;
                ecPoint3 = ecPoint3.TwicePlus( num2 < 0 ? ecPoint2 : ecPoint1 ).TimesPow2( e );
            }
            return ecPoint3;
        }
    }
}
