// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.NafR2LMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class NafR2LMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            int[] compactNaf = WNafUtilities.GenerateCompactNaf( k );
            ECPoint ecPoint1 = p.Curve.Infinity;
            ECPoint ecPoint2 = p;
            int num1 = 0;
            for (int index = 0; index < compactNaf.Length; ++index)
            {
                int num2 = compactNaf[index];
                int num3 = num2 >> 16;
                int e = num1 + (num2 & ushort.MaxValue);
                ecPoint2 = ecPoint2.TimesPow2( e );
                ecPoint1 = ecPoint1.Add( num3 < 0 ? ecPoint2.Negate() : ecPoint2 );
                num1 = 1;
            }
            return ecPoint1;
        }
    }
}
