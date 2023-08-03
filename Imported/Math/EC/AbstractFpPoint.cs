// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.AbstractFpPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class AbstractFpPoint : ECPointBase
    {
        protected AbstractFpPoint(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
          : base( curve, x, y, withCompression )
        {
        }

        protected AbstractFpPoint(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        protected internal override bool CompressionYTilde => this.AffineYCoord.TestBitZero();

        protected override bool SatisfiesCurveEquation()
        {
            ECFieldElement rawXcoord = this.RawXCoord;
            ECFieldElement rawYcoord = this.RawYCoord;
            ECFieldElement b1 = this.Curve.A;
            ECFieldElement b2 = this.Curve.B;
            ECFieldElement ecFieldElement1 = rawYcoord.Square();
            switch (this.CurveCoordinateSystem)
            {
                case 0:
                    ECFieldElement other = rawXcoord.Square().Add( b1 ).Multiply( rawXcoord ).Add( b2 );
                    return ecFieldElement1.Equals( other );
                case 1:
                    ECFieldElement rawZcoord1 = this.RawZCoords[0];
                    if (!rawZcoord1.IsOne)
                    {
                        ECFieldElement b3 = rawZcoord1.Square();
                        ECFieldElement b4 = rawZcoord1.Multiply( b3 );
                        ecFieldElement1 = ecFieldElement1.Multiply( rawZcoord1 );
                        b1 = b1.Multiply( b3 );
                        b2 = b2.Multiply( b4 );
                        goto case 0;
                    }
                    else
                        goto case 0;
                case 2:
                case 3:
                case 4:
                    ECFieldElement rawZcoord2 = this.RawZCoords[0];
                    if (!rawZcoord2.IsOne)
                    {
                        ECFieldElement ecFieldElement2 = rawZcoord2.Square();
                        ECFieldElement b5 = ecFieldElement2.Square();
                        ECFieldElement b6 = ecFieldElement2.Multiply( b5 );
                        b1 = b1.Multiply( b5 );
                        b2 = b2.Multiply( b6 );
                        goto case 0;
                    }
                    else
                        goto case 0;
                default:
                    throw new InvalidOperationException( "unsupported coordinate system" );
            }
        }

        public override ECPoint Subtract( ECPoint b ) => b.IsInfinity ? this : this.Add( b.Negate() );
    }
}
