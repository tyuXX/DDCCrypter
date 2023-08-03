// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.MixedNafR2LMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class MixedNafR2LMultiplier : AbstractECMultiplier
    {
        protected readonly int additionCoord;
        protected readonly int doublingCoord;

        public MixedNafR2LMultiplier()
          : this( 2, 4 )
        {
        }

        public MixedNafR2LMultiplier( int additionCoord, int doublingCoord )
        {
            this.additionCoord = additionCoord;
            this.doublingCoord = doublingCoord;
        }

        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            ECCurve curve = p.Curve;
            ECCurve ecCurve1 = this.ConfigureCurve( curve, this.additionCoord );
            ECCurve ecCurve2 = this.ConfigureCurve( curve, this.doublingCoord );
            int[] compactNaf = WNafUtilities.GenerateCompactNaf( k );
            ECPoint p1 = ecCurve1.Infinity;
            ECPoint p2 = ecCurve2.ImportPoint( p );
            int num1 = 0;
            for (int index = 0; index < compactNaf.Length; ++index)
            {
                int num2 = compactNaf[index];
                int num3 = num2 >> 16;
                int e = num1 + (num2 & ushort.MaxValue);
                p2 = p2.TimesPow2( e );
                ECPoint b = ecCurve1.ImportPoint( p2 );
                if (num3 < 0)
                    b = b.Negate();
                p1 = p1.Add( b );
                num1 = 1;
            }
            return curve.ImportPoint( p1 );
        }

        protected virtual ECCurve ConfigureCurve( ECCurve c, int coord )
        {
            if (c.CoordinateSystem == coord)
                return c;
            return c.SupportsCoordinateSystem( coord ) ? c.Configure().SetCoordinateSystem( coord ).Create() : throw new ArgumentException( "Coordinate system " + coord + " not supported by this curve", nameof( coord ) );
        }
    }
}
