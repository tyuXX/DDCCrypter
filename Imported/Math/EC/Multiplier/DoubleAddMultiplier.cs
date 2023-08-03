﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.DoubleAddMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class DoubleAddMultiplier : AbstractECMultiplier
    {
        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            ECPoint[] ecPointArray = new ECPoint[2]
            {
        p.Curve.Infinity,
        p
            };
            int bitLength = k.BitLength;
            for (int n = 0; n < bitLength; ++n)
            {
                int index1 = k.TestBit( n ) ? 1 : 0;
                int index2 = 1 - index1;
                ecPointArray[index2] = ecPointArray[index2].TwicePlus( ecPointArray[index1] );
            }
            return ecPointArray[0];
        }
    }
}
