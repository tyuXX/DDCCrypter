// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.AbstractF2mPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC
{
    public abstract class AbstractF2mPoint : ECPointBase
    {
        protected AbstractF2mPoint(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
          : base( curve, x, y, withCompression )
        {
        }

        protected AbstractF2mPoint(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        protected override bool SatisfiesCurveEquation()
        {
            ECCurve curve = this.Curve;
            ECFieldElement rawXcoord = this.RawXCoord;
            ECFieldElement rawYcoord = this.RawYCoord;
            ECFieldElement ecFieldElement1 = curve.A;
            ECFieldElement ecFieldElement2 = curve.B;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement ecFieldElement3;
            ECFieldElement other;
            if (coordinateSystem == 6)
            {
                ECFieldElement rawZcoord = this.RawZCoords[0];
                bool isOne = rawZcoord.IsOne;
                if (rawXcoord.IsZero)
                {
                    ecFieldElement3 = rawYcoord.Square();
                    other = ecFieldElement2;
                    if (!isOne)
                    {
                        ECFieldElement b = rawZcoord.Square();
                        other = other.Multiply( b );
                    }
                }
                else
                {
                    ECFieldElement b1 = rawYcoord;
                    ECFieldElement b2 = rawXcoord.Square();
                    ECFieldElement ecFieldElement4;
                    if (isOne)
                    {
                        ecFieldElement4 = b1.Square().Add( b1 ).Add( ecFieldElement1 );
                        other = b2.Square().Add( ecFieldElement2 );
                    }
                    else
                    {
                        ECFieldElement y1 = rawZcoord.Square();
                        ECFieldElement y2 = y1.Square();
                        ecFieldElement4 = b1.Add( rawZcoord ).MultiplyPlusProduct( b1, ecFieldElement1, y1 );
                        other = b2.SquarePlusProduct( ecFieldElement2, y2 );
                    }
                    ecFieldElement3 = ecFieldElement4.Multiply( b2 );
                }
            }
            else
            {
                ecFieldElement3 = rawYcoord.Add( rawXcoord ).Multiply( rawYcoord );
                switch (coordinateSystem)
                {
                    case 0:
                        other = rawXcoord.Add( ecFieldElement1 ).Multiply( rawXcoord.Square() ).Add( ecFieldElement2 );
                        break;
                    case 1:
                        ECFieldElement rawZcoord = this.RawZCoords[0];
                        if (!rawZcoord.IsOne)
                        {
                            ECFieldElement b3 = rawZcoord.Square();
                            ECFieldElement b4 = rawZcoord.Multiply( b3 );
                            ecFieldElement3 = ecFieldElement3.Multiply( rawZcoord );
                            ecFieldElement1 = ecFieldElement1.Multiply( rawZcoord );
                            ecFieldElement2 = ecFieldElement2.Multiply( b4 );
                            goto case 0;
                        }
                        else
                            goto case 0;
                    default:
                        throw new InvalidOperationException( "unsupported coordinate system" );
                }
            }
            return ecFieldElement3.Equals( other );
        }

        public override ECPoint ScaleX( ECFieldElement scale )
        {
            if (this.IsInfinity)
                return this;
            switch (this.CurveCoordinateSystem)
            {
                case 5:
                    ECFieldElement rawXcoord1 = this.RawXCoord;
                    ECFieldElement rawYcoord1 = this.RawYCoord;
                    ECFieldElement b1 = rawXcoord1.Multiply( scale );
                    ECFieldElement y1 = rawYcoord1.Add( rawXcoord1 ).Divide( scale ).Add( b1 );
                    return this.Curve.CreateRawPoint( rawXcoord1, y1, this.RawZCoords, this.IsCompressed );
                case 6:
                    ECFieldElement rawXcoord2 = this.RawXCoord;
                    ECFieldElement rawYcoord2 = this.RawYCoord;
                    ECFieldElement rawZcoord = this.RawZCoords[0];
                    ECFieldElement b2 = rawXcoord2.Multiply( scale.Square() );
                    ECFieldElement y2 = rawYcoord2.Add( rawXcoord2 ).Add( b2 );
                    ECFieldElement ecFieldElement = rawZcoord.Multiply( scale );
                    return this.Curve.CreateRawPoint( rawXcoord2, y2, new ECFieldElement[1]
                    {
            ecFieldElement
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                default:
                    return base.ScaleX( scale );
            }
        }

        public override ECPoint ScaleY( ECFieldElement scale )
        {
            if (this.IsInfinity)
                return this;
            switch (this.CurveCoordinateSystem)
            {
                case 5:
                case 6:
                    ECFieldElement rawXcoord = this.RawXCoord;
                    ECFieldElement y = this.RawYCoord.Add( rawXcoord ).Multiply( scale ).Add( rawXcoord );
                    return this.Curve.CreateRawPoint( rawXcoord, y, this.RawZCoords, this.IsCompressed );
                default:
                    return base.ScaleY( scale );
            }
        }

        public override ECPoint Subtract( ECPoint b ) => b.IsInfinity ? this : this.Add( b.Negate() );

        public virtual AbstractF2mPoint Tau()
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXcoord = this.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                case 5:
                    ECFieldElement rawYcoord1 = this.RawYCoord;
                    return (AbstractF2mPoint)curve.CreateRawPoint( rawXcoord.Square(), rawYcoord1.Square(), this.IsCompressed );
                case 1:
                case 6:
                    ECFieldElement rawYcoord2 = this.RawYCoord;
                    ECFieldElement rawZcoord = this.RawZCoords[0];
                    return (AbstractF2mPoint)curve.CreateRawPoint( rawXcoord.Square(), rawYcoord2.Square(), new ECFieldElement[1]
                    {
            rawZcoord.Square()
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                default:
                    throw new InvalidOperationException( "unsupported coordinate system" );
            }
        }

        public virtual AbstractF2mPoint TauPow( int pow )
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXcoord = this.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                case 5:
                    ECFieldElement rawYcoord1 = this.RawYCoord;
                    return (AbstractF2mPoint)curve.CreateRawPoint( rawXcoord.SquarePow( pow ), rawYcoord1.SquarePow( pow ), this.IsCompressed );
                case 1:
                case 6:
                    ECFieldElement rawYcoord2 = this.RawYCoord;
                    ECFieldElement rawZcoord = this.RawZCoords[0];
                    return (AbstractF2mPoint)curve.CreateRawPoint( rawXcoord.SquarePow( pow ), rawYcoord2.SquarePow( pow ), new ECFieldElement[1]
                    {
            rawZcoord.SquarePow(pow)
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                default:
                    throw new InvalidOperationException( "unsupported coordinate system" );
            }
        }
    }
}
