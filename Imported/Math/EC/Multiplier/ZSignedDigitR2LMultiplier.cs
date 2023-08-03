// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.ZSignedDigitR2LMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class ZSignedDigitR2LMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            ECPoint ecPoint1 = p.Curve.Infinity;
            ECPoint ecPoint2 = p;
            int bitLength = k.BitLength;
            int lowestSetBit = k.GetLowestSetBit();
            ECPoint b = ecPoint2.TimesPow2( lowestSetBit );
            int n = lowestSetBit;
            while (++n < bitLength)
            {
                ecPoint1 = ecPoint1.Add( k.TestBit( n ) ? b : b.Negate() );
                b = b.Twice();
            }
            return ecPoint1.Add( b );
        }
    }
}
