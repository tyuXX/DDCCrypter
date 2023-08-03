// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.ZSignedDigitL2RMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class ZSignedDigitL2RMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            ECPoint ecPoint1 = p.Normalize();
            ECPoint ecPoint2 = ecPoint1.Negate();
            ECPoint ecPoint3 = ecPoint1;
            int bitLength = k.BitLength;
            int lowestSetBit = k.GetLowestSetBit();
            int n = bitLength;
            while (--n > lowestSetBit)
                ecPoint3 = ecPoint3.TwicePlus( k.TestBit( n ) ? ecPoint1 : ecPoint2 );
            return ecPoint3.TimesPow2( lowestSetBit );
        }
    }
}
