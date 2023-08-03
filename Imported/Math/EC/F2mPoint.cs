// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.F2mPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Math.EC
{
    public class F2mPoint : AbstractF2mPoint
    {
        public F2mPoint( ECCurve curve, ECFieldElement x, ECFieldElement y )
          : this( curve, x, y, false )
        {
        }

        public F2mPoint( ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression )
          : base( curve, x, y, withCompression )
        {
            if (x == null != (y == null))
                throw new ArgumentException( "Exactly one of the field elements is null" );
            if (x == null)
                return;
            F2mFieldElement.CheckFieldElements( x, y );
            if (curve == null)
                return;
            F2mFieldElement.CheckFieldElements( x, curve.A );
        }

        internal F2mPoint(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        [Obsolete( "Use ECCurve.Infinity property" )]
        public F2mPoint( ECCurve curve )
          : this( curve, null, null )
        {
        }

        protected override ECPoint Detach() => new F2mPoint( null, this.AffineXCoord, this.AffineYCoord );

        public override ECFieldElement YCoord
        {
            get
            {
                int coordinateSystem = this.CurveCoordinateSystem;
                switch (coordinateSystem)
                {
                    case 5:
                    case 6:
                        ECFieldElement rawXcoord = this.RawXCoord;
                        ECFieldElement rawYcoord = this.RawYCoord;
                        if (this.IsInfinity || rawXcoord.IsZero)
                            return rawYcoord;
                        ECFieldElement ycoord = rawYcoord.Add( rawXcoord ).Multiply( rawXcoord );
                        if (6 == coordinateSystem)
                        {
                            ECFieldElement rawZcoord = this.RawZCoords[0];
                            if (!rawZcoord.IsOne)
                                ycoord = ycoord.Divide( rawZcoord );
                        }
                        return ycoord;
                    default:
                        return this.RawYCoord;
                }
            }
        }

        protected internal override bool CompressionYTilde
        {
            get
            {
                ECFieldElement rawXcoord = this.RawXCoord;
                if (rawXcoord.IsZero)
                    return false;
                ECFieldElement rawYcoord = this.RawYCoord;
                switch (this.CurveCoordinateSystem)
                {
                    case 5:
                    case 6:
                        return rawYcoord.TestBitZero() != rawXcoord.TestBitZero();
                    default:
                        return rawYcoord.Divide( rawXcoord ).TestBitZero();
                }
            }
        }

        public override ECPoint Add( ECPoint b )
        {
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXcoord1 = this.RawXCoord;
            ECFieldElement rawXcoord2 = b.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                    ECFieldElement rawYcoord1 = this.RawYCoord;
                    ECFieldElement rawYcoord2 = b.RawYCoord;
                    ECFieldElement b1 = rawXcoord1.Add( rawXcoord2 );
                    ECFieldElement ecFieldElement1 = rawYcoord1.Add( rawYcoord2 );
                    if (b1.IsZero)
                        return ecFieldElement1.IsZero ? this.Twice() : curve.Infinity;
                    ECFieldElement b2 = ecFieldElement1.Divide( b1 );
                    ECFieldElement ecFieldElement2 = b2.Square().Add( b2 ).Add( b1 ).Add( curve.A );
                    ECFieldElement y1 = b2.Multiply( rawXcoord1.Add( ecFieldElement2 ) ).Add( ecFieldElement2 ).Add( rawYcoord1 );
                    return new F2mPoint( curve, ecFieldElement2, y1, this.IsCompressed );
                case 1:
                    ECFieldElement rawYcoord3 = this.RawYCoord;
                    ECFieldElement rawZcoord1 = this.RawZCoords[0];
                    ECFieldElement rawYcoord4 = b.RawYCoord;
                    ECFieldElement rawZcoord2 = b.RawZCoords[0];
                    bool isOne1 = rawZcoord1.IsOne;
                    ECFieldElement ecFieldElement3 = rawYcoord4;
                    ECFieldElement ecFieldElement4 = rawXcoord2;
                    if (!isOne1)
                    {
                        ecFieldElement3 = ecFieldElement3.Multiply( rawZcoord1 );
                        ecFieldElement4 = ecFieldElement4.Multiply( rawZcoord1 );
                    }
                    bool isOne2 = rawZcoord2.IsOne;
                    ECFieldElement b3 = rawYcoord3;
                    ECFieldElement b4 = rawXcoord1;
                    if (!isOne2)
                    {
                        b3 = b3.Multiply( rawZcoord2 );
                        b4 = b4.Multiply( rawZcoord2 );
                    }
                    ECFieldElement b5 = ecFieldElement3.Add( b3 );
                    ECFieldElement ecFieldElement5 = ecFieldElement4.Add( b4 );
                    if (ecFieldElement5.IsZero)
                        return b5.IsZero ? this.Twice() : curve.Infinity;
                    ECFieldElement x1 = ecFieldElement5.Square();
                    ECFieldElement b6 = x1.Multiply( ecFieldElement5 );
                    ECFieldElement b7 = isOne1 ? rawZcoord2 : (isOne2 ? rawZcoord1 : rawZcoord1.Multiply( rawZcoord2 ));
                    ECFieldElement x2 = b5.Add( ecFieldElement5 );
                    ECFieldElement ecFieldElement6 = x2.MultiplyPlusProduct( b5, x1, curve.A ).Multiply( b7 ).Add( b6 );
                    ECFieldElement x3 = ecFieldElement5.Multiply( ecFieldElement6 );
                    ECFieldElement b8 = isOne2 ? x1 : x1.Multiply( rawZcoord2 );
                    ECFieldElement y2 = b5.MultiplyPlusProduct( rawXcoord1, ecFieldElement5, rawYcoord3 ).MultiplyPlusProduct( b8, x2, ecFieldElement6 );
                    ECFieldElement ecFieldElement7 = b6.Multiply( b7 );
                    return new F2mPoint( curve, x3, y2, new ECFieldElement[1]
                    {
            ecFieldElement7
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                case 6:
                    if (rawXcoord1.IsZero)
                        return rawXcoord2.IsZero ? curve.Infinity : b.Add( this );
                    ECFieldElement rawYcoord5 = this.RawYCoord;
                    ECFieldElement rawZcoord3 = this.RawZCoords[0];
                    ECFieldElement rawYcoord6 = b.RawYCoord;
                    ECFieldElement rawZcoord4 = b.RawZCoords[0];
                    bool isOne3 = rawZcoord3.IsOne;
                    ECFieldElement b9 = rawXcoord2;
                    ECFieldElement b10 = rawYcoord6;
                    if (!isOne3)
                    {
                        b9 = b9.Multiply( rawZcoord3 );
                        b10 = b10.Multiply( rawZcoord3 );
                    }
                    bool isOne4 = rawZcoord4.IsOne;
                    ECFieldElement b11 = rawXcoord1;
                    ECFieldElement ecFieldElement8 = rawYcoord5;
                    if (!isOne4)
                    {
                        b11 = b11.Multiply( rawZcoord4 );
                        ecFieldElement8 = ecFieldElement8.Multiply( rawZcoord4 );
                    }
                    ECFieldElement ecFieldElement9 = ecFieldElement8.Add( b10 );
                    ECFieldElement ecFieldElement10 = b11.Add( b9 );
                    if (ecFieldElement10.IsZero)
                        return ecFieldElement9.IsZero ? this.Twice() : curve.Infinity;
                    ECFieldElement ecFieldElement11;
                    ECFieldElement y3;
                    ECFieldElement ecFieldElement12;
                    if (rawXcoord2.IsZero)
                    {
                        ECPoint ecPoint = this.Normalize();
                        ECFieldElement rawXcoord3 = ecPoint.RawXCoord;
                        ECFieldElement ycoord = ecPoint.YCoord;
                        ECFieldElement b12 = rawYcoord6;
                        ECFieldElement b13 = ycoord.Add( b12 ).Divide( rawXcoord3 );
                        ecFieldElement11 = b13.Square().Add( b13 ).Add( rawXcoord3 ).Add( curve.A );
                        if (ecFieldElement11.IsZero)
                            return new F2mPoint( curve, ecFieldElement11, curve.B.Sqrt(), this.IsCompressed );
                        y3 = b13.Multiply( rawXcoord3.Add( ecFieldElement11 ) ).Add( ecFieldElement11 ).Add( ycoord ).Divide( ecFieldElement11 ).Add( ecFieldElement11 );
                        ecFieldElement12 = curve.FromBigInteger( BigInteger.One );
                    }
                    else
                    {
                        ECFieldElement b14 = ecFieldElement10.Square();
                        ECFieldElement ecFieldElement13 = ecFieldElement9.Multiply( b11 );
                        ECFieldElement b15 = ecFieldElement9.Multiply( b9 );
                        ecFieldElement11 = ecFieldElement13.Multiply( b15 );
                        if (ecFieldElement11.IsZero)
                            return new F2mPoint( curve, ecFieldElement11, curve.B.Sqrt(), this.IsCompressed );
                        ECFieldElement x4 = ecFieldElement9.Multiply( b14 );
                        if (!isOne4)
                            x4 = x4.Multiply( rawZcoord4 );
                        y3 = b15.Add( b14 ).SquarePlusProduct( x4, rawYcoord5.Add( rawZcoord3 ) );
                        ecFieldElement12 = x4;
                        if (!isOne3)
                            ecFieldElement12 = ecFieldElement12.Multiply( rawZcoord3 );
                    }
                    return new F2mPoint( curve, ecFieldElement11, y3, new ECFieldElement[1]
                    {
            ecFieldElement12
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                default:
                    throw new InvalidOperationException( "unsupported coordinate system" );
            }
        }

        public override ECPoint Twice()
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            ECFieldElement rawXcoord = this.RawXCoord;
            if (rawXcoord.IsZero)
                return curve.Infinity;
            switch (curve.CoordinateSystem)
            {
                case 0:
                    ECFieldElement b1 = this.RawYCoord.Divide( rawXcoord ).Add( rawXcoord );
                    ECFieldElement x1 = b1.Square().Add( b1 ).Add( curve.A );
                    ECFieldElement y1 = rawXcoord.SquarePlusProduct( x1, b1.AddOne() );
                    return new F2mPoint( curve, x1, y1, this.IsCompressed );
                case 1:
                    ECFieldElement rawYcoord1 = this.RawYCoord;
                    ECFieldElement rawZcoord1 = this.RawZCoords[0];
                    bool isOne1 = rawZcoord1.IsOne;
                    ECFieldElement ecFieldElement1 = isOne1 ? rawXcoord : rawXcoord.Multiply( rawZcoord1 );
                    ECFieldElement b2 = isOne1 ? rawYcoord1 : rawYcoord1.Multiply( rawZcoord1 );
                    ECFieldElement ecFieldElement2 = rawXcoord.Square();
                    ECFieldElement b3 = ecFieldElement2.Add( b2 );
                    ECFieldElement b4 = ecFieldElement1;
                    ECFieldElement ecFieldElement3 = b4.Square();
                    ECFieldElement y2 = b3.Add( b4 );
                    ECFieldElement ecFieldElement4 = y2.MultiplyPlusProduct( b3, ecFieldElement3, curve.A );
                    ECFieldElement x2 = b4.Multiply( ecFieldElement4 );
                    ECFieldElement y3 = ecFieldElement2.Square().MultiplyPlusProduct( b4, ecFieldElement4, y2 );
                    ECFieldElement ecFieldElement5 = b4.Multiply( ecFieldElement3 );
                    return new F2mPoint( curve, x2, y3, new ECFieldElement[1]
                    {
            ecFieldElement5
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                case 6:
                    ECFieldElement rawYcoord2 = this.RawYCoord;
                    ECFieldElement rawZcoord2 = this.RawZCoords[0];
                    bool isOne2 = rawZcoord2.IsOne;
                    ECFieldElement ecFieldElement6 = isOne2 ? rawYcoord2 : rawYcoord2.Multiply( rawZcoord2 );
                    ECFieldElement b5 = isOne2 ? rawZcoord2 : rawZcoord2.Square();
                    ECFieldElement a = curve.A;
                    ECFieldElement b6 = isOne2 ? a : a.Multiply( b5 );
                    ECFieldElement ecFieldElement7 = rawYcoord2.Square().Add( ecFieldElement6 ).Add( b6 );
                    if (ecFieldElement7.IsZero)
                        return new F2mPoint( curve, ecFieldElement7, curve.B.Sqrt(), this.IsCompressed );
                    ECFieldElement ecFieldElement8 = ecFieldElement7.Square();
                    ECFieldElement b7 = isOne2 ? ecFieldElement7 : ecFieldElement7.Multiply( b5 );
                    ECFieldElement b8 = curve.B;
                    ECFieldElement y4;
                    if (b8.BitLength < curve.FieldSize >> 1)
                    {
                        ECFieldElement b9 = rawYcoord2.Add( rawXcoord ).Square();
                        ECFieldElement b10 = !b8.IsOne ? b6.SquarePlusProduct( b8, b5.Square() ) : b6.Add( b5 ).Square();
                        y4 = b9.Add( ecFieldElement7 ).Add( b5 ).Multiply( b9 ).Add( b10 ).Add( ecFieldElement8 );
                        if (a.IsZero)
                            y4 = y4.Add( b7 );
                        else if (!a.IsOne)
                            y4 = y4.Add( a.AddOne().Multiply( b7 ) );
                    }
                    else
                        y4 = (isOne2 ? rawXcoord : rawXcoord.Multiply( rawZcoord2 )).SquarePlusProduct( ecFieldElement7, ecFieldElement6 ).Add( ecFieldElement8 ).Add( b7 );
                    return new F2mPoint( curve, ecFieldElement8, y4, new ECFieldElement[1]
                    {
            b7
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                default:
                    throw new InvalidOperationException( "unsupported coordinate system" );
            }
        }

        public override ECPoint TwicePlus( ECPoint b )
        {
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this.Twice();
            ECCurve curve = this.Curve;
            ECFieldElement rawXcoord1 = this.RawXCoord;
            if (rawXcoord1.IsZero)
                return b;
            if (curve.CoordinateSystem != 6)
                return this.Twice().Add( b );
            ECFieldElement rawXcoord2 = b.RawXCoord;
            ECFieldElement rawZcoord1 = b.RawZCoords[0];
            if (rawXcoord2.IsZero || !rawZcoord1.IsOne)
                return this.Twice().Add( b );
            ECFieldElement rawYcoord1 = this.RawYCoord;
            ECFieldElement rawZcoord2 = this.RawZCoords[0];
            ECFieldElement rawYcoord2 = b.RawYCoord;
            ECFieldElement x1 = rawXcoord1.Square();
            ECFieldElement b1 = rawYcoord1.Square();
            ECFieldElement ecFieldElement1 = rawZcoord2.Square();
            ECFieldElement b2 = rawYcoord1.Multiply( rawZcoord2 );
            ECFieldElement b3 = curve.A.Multiply( ecFieldElement1 ).Add( b1 ).Add( b2 );
            ECFieldElement ecFieldElement2 = rawYcoord2.AddOne();
            ECFieldElement x2 = curve.A.Add( ecFieldElement2 ).Multiply( ecFieldElement1 ).Add( b1 ).MultiplyPlusProduct( b3, x1, ecFieldElement1 );
            ECFieldElement b4 = rawXcoord2.Multiply( ecFieldElement1 );
            ECFieldElement b5 = b4.Add( b3 ).Square();
            if (b5.IsZero)
                return x2.IsZero ? b.Twice() : curve.Infinity;
            if (x2.IsZero)
                return new F2mPoint( curve, x2, curve.B.Sqrt(), this.IsCompressed );
            ECFieldElement x3 = x2.Square().Multiply( b4 );
            ECFieldElement y1 = x2.Multiply( b5 ).Multiply( ecFieldElement1 );
            ECFieldElement y2 = x2.Add( b5 ).Square().MultiplyPlusProduct( b3, ecFieldElement2, y1 );
            return new F2mPoint( curve, x3, y2, new ECFieldElement[1]
            {
        y1
            }, (this.IsCompressed ? 1 : 0) != 0 );
        }

        public override ECPoint Negate()
        {
            if (this.IsInfinity)
                return this;
            ECFieldElement rawXcoord = this.RawXCoord;
            if (rawXcoord.IsZero)
                return this;
            ECCurve curve = this.Curve;
            switch (curve.CoordinateSystem)
            {
                case 0:
                    ECFieldElement rawYcoord1 = this.RawYCoord;
                    return new F2mPoint( curve, rawXcoord, rawYcoord1.Add( rawXcoord ), this.IsCompressed );
                case 1:
                    ECFieldElement rawYcoord2 = this.RawYCoord;
                    ECFieldElement rawZcoord1 = this.RawZCoords[0];
                    return new F2mPoint( curve, rawXcoord, rawYcoord2.Add( rawXcoord ), new ECFieldElement[1]
                    {
            rawZcoord1
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                case 5:
                    ECFieldElement rawYcoord3 = this.RawYCoord;
                    return new F2mPoint( curve, rawXcoord, rawYcoord3.AddOne(), this.IsCompressed );
                case 6:
                    ECFieldElement rawYcoord4 = this.RawYCoord;
                    ECFieldElement rawZcoord2 = this.RawZCoords[0];
                    return new F2mPoint( curve, rawXcoord, rawYcoord4.Add( rawZcoord2 ), new ECFieldElement[1]
                    {
            rawZcoord2
                    }, (this.IsCompressed ? 1 : 0) != 0 );
                default:
                    throw new InvalidOperationException( "unsupported coordinate system" );
            }
        }
    }
}
