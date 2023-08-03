// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT571R1Point
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT571R1Point : AbstractF2mPoint
    {
        public SecT571R1Point( ECCurve curve, ECFieldElement x, ECFieldElement y )
          : this( curve, x, y, false )
        {
        }

        public SecT571R1Point( ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression )
          : base( curve, x, y, withCompression )
        {
            if (x == null != (y == null))
                throw new ArgumentException( "Exactly one of the field elements is null" );
        }

        internal SecT571R1Point(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        protected override ECPoint Detach() => new SecT571R1Point( null, this.AffineXCoord, this.AffineYCoord );

        public override ECFieldElement YCoord
        {
            get
            {
                ECFieldElement rawXcoord = this.RawXCoord;
                ECFieldElement rawYcoord = this.RawYCoord;
                if (this.IsInfinity || rawXcoord.IsZero)
                    return rawYcoord;
                ECFieldElement ycoord = rawYcoord.Add( rawXcoord ).Multiply( rawXcoord );
                ECFieldElement rawZcoord = this.RawZCoords[0];
                if (!rawZcoord.IsOne)
                    ycoord = ycoord.Divide( rawZcoord );
                return ycoord;
            }
        }

        protected internal override bool CompressionYTilde
        {
            get
            {
                ECFieldElement rawXcoord = this.RawXCoord;
                return !rawXcoord.IsZero && this.RawYCoord.TestBitZero() != rawXcoord.TestBitZero();
            }
        }

        public override ECPoint Add( ECPoint b )
        {
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            ECFieldElement rawXcoord1 = this.RawXCoord;
            ECFieldElement rawXcoord2 = b.RawXCoord;
            if (rawXcoord1.IsZero)
                return rawXcoord2.IsZero ? curve.Infinity : b.Add( this );
            ECFieldElement rawYcoord1 = this.RawYCoord;
            ECFieldElement rawZcoord1 = this.RawZCoords[0];
            ECFieldElement rawYcoord2 = b.RawYCoord;
            ECFieldElement rawZcoord2 = b.RawZCoords[0];
            bool isOne1 = rawZcoord1.IsOne;
            ECFieldElement b1 = rawXcoord2;
            ECFieldElement b2 = rawYcoord2;
            if (!isOne1)
            {
                b1 = b1.Multiply( rawZcoord1 );
                b2 = b2.Multiply( rawZcoord1 );
            }
            bool isOne2 = rawZcoord2.IsOne;
            ECFieldElement b3 = rawXcoord1;
            ECFieldElement ecFieldElement1 = rawYcoord1;
            if (!isOne2)
            {
                b3 = b3.Multiply( rawZcoord2 );
                ecFieldElement1 = ecFieldElement1.Multiply( rawZcoord2 );
            }
            ECFieldElement ecFieldElement2 = ecFieldElement1.Add( b2 );
            ECFieldElement ecFieldElement3 = b3.Add( b1 );
            if (ecFieldElement3.IsZero)
                return ecFieldElement2.IsZero ? this.Twice() : curve.Infinity;
            ECFieldElement ecFieldElement4;
            ECFieldElement y;
            ECFieldElement ecFieldElement5;
            if (rawXcoord2.IsZero)
            {
                ECPoint ecPoint = this.Normalize();
                ECFieldElement xcoord = ecPoint.XCoord;
                ECFieldElement ycoord = ecPoint.YCoord;
                ECFieldElement b4 = rawYcoord2;
                ECFieldElement b5 = ycoord.Add( b4 ).Divide( xcoord );
                ecFieldElement4 = b5.Square().Add( b5 ).Add( xcoord ).AddOne();
                if (ecFieldElement4.IsZero)
                    return new SecT571R1Point( curve, ecFieldElement4, SecT571R1Curve.SecT571R1_B_SQRT, this.IsCompressed );
                y = b5.Multiply( xcoord.Add( ecFieldElement4 ) ).Add( ecFieldElement4 ).Add( ycoord ).Divide( ecFieldElement4 ).Add( ecFieldElement4 );
                ecFieldElement5 = curve.FromBigInteger( BigInteger.One );
            }
            else
            {
                ECFieldElement b6 = ecFieldElement3.Square();
                ECFieldElement ecFieldElement6 = ecFieldElement2.Multiply( b3 );
                ECFieldElement b7 = ecFieldElement2.Multiply( b1 );
                ecFieldElement4 = ecFieldElement6.Multiply( b7 );
                if (ecFieldElement4.IsZero)
                    return new SecT571R1Point( curve, ecFieldElement4, SecT571R1Curve.SecT571R1_B_SQRT, this.IsCompressed );
                ECFieldElement x = ecFieldElement2.Multiply( b6 );
                if (!isOne2)
                    x = x.Multiply( rawZcoord2 );
                y = b7.Add( b6 ).SquarePlusProduct( x, rawYcoord1.Add( rawZcoord1 ) );
                ecFieldElement5 = x;
                if (!isOne1)
                    ecFieldElement5 = ecFieldElement5.Multiply( rawZcoord1 );
            }
            return new SecT571R1Point( curve, ecFieldElement4, y, new ECFieldElement[1]
            {
        ecFieldElement5
            }, (this.IsCompressed ? 1 : 0) != 0 );
        }

        public override ECPoint Twice()
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            ECFieldElement rawXcoord = this.RawXCoord;
            if (rawXcoord.IsZero)
                return curve.Infinity;
            ECFieldElement rawYcoord = this.RawYCoord;
            ECFieldElement rawZcoord = this.RawZCoords[0];
            bool isOne = rawZcoord.IsOne;
            ECFieldElement ecFieldElement1 = isOne ? rawYcoord : rawYcoord.Multiply( rawZcoord );
            ECFieldElement b1 = isOne ? rawZcoord : rawZcoord.Square();
            ECFieldElement x = rawYcoord.Square().Add( ecFieldElement1 ).Add( b1 );
            if (x.IsZero)
                return new SecT571R1Point( curve, x, SecT571R1Curve.SecT571R1_B_SQRT, this.IsCompressed );
            ECFieldElement ecFieldElement2 = x.Square();
            ECFieldElement b2 = isOne ? x : x.Multiply( b1 );
            ECFieldElement y = (isOne ? rawXcoord : rawXcoord.Multiply( rawZcoord )).SquarePlusProduct( x, ecFieldElement1 ).Add( ecFieldElement2 ).Add( b2 );
            return new SecT571R1Point( curve, ecFieldElement2, y, new ECFieldElement[1]
            {
        b2
            }, (this.IsCompressed ? 1 : 0) != 0 );
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
            ECFieldElement rawXcoord2 = b.RawXCoord;
            ECFieldElement rawZcoord1 = b.RawZCoords[0];
            if (rawXcoord2.IsZero || !rawZcoord1.IsOne)
                return this.Twice().Add( b );
            ECFieldElement rawYcoord1 = this.RawYCoord;
            ECFieldElement rawZcoord2 = this.RawZCoords[0];
            ECFieldElement rawYcoord2 = b.RawYCoord;
            ECFieldElement x1 = rawXcoord1.Square();
            ECFieldElement b1 = rawYcoord1.Square();
            ECFieldElement ecFieldElement = rawZcoord2.Square();
            ECFieldElement b2 = rawYcoord1.Multiply( rawZcoord2 );
            ECFieldElement b3 = ecFieldElement.Add( b1 ).Add( b2 );
            ECFieldElement x2 = rawYcoord2.AddOne();
            ECFieldElement x3 = rawYcoord2.Multiply( ecFieldElement ).Add( b1 ).MultiplyPlusProduct( b3, x1, ecFieldElement );
            ECFieldElement b4 = rawXcoord2.Multiply( ecFieldElement );
            ECFieldElement b5 = b4.Add( b3 ).Square();
            if (b5.IsZero)
                return x3.IsZero ? b.Twice() : curve.Infinity;
            if (x3.IsZero)
                return new SecT571R1Point( curve, x3, SecT571R1Curve.SecT571R1_B_SQRT, this.IsCompressed );
            ECFieldElement x4 = x3.Square().Multiply( b4 );
            ECFieldElement y1 = x3.Multiply( b5 ).Multiply( ecFieldElement );
            ECFieldElement y2 = x3.Add( b5 ).Square().MultiplyPlusProduct( b3, x2, y1 );
            return new SecT571R1Point( curve, x4, y2, new ECFieldElement[1]
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
            ECFieldElement rawYcoord = this.RawYCoord;
            ECFieldElement rawZcoord = this.RawZCoords[0];
            return new SecT571R1Point( this.Curve, rawXcoord, rawYcoord.Add( rawZcoord ), new ECFieldElement[1]
            {
        rawZcoord
            }, (this.IsCompressed ? 1 : 0) != 0 );
        }
    }
}
