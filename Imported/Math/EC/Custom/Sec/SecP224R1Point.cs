// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP224R1Point
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224R1Point : AbstractFpPoint
    {
        public SecP224R1Point( ECCurve curve, ECFieldElement x, ECFieldElement y )
          : this( curve, x, y, false )
        {
        }

        public SecP224R1Point( ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression )
          : base( curve, x, y, withCompression )
        {
            if (x == null != (y == null))
                throw new ArgumentException( "Exactly one of the field elements is null" );
        }

        internal SecP224R1Point(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        protected override ECPoint Detach() => new SecP224R1Point( null, this.AffineXCoord, this.AffineYCoord );

        public override ECPoint Add( ECPoint b )
        {
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this;
            if (this == b)
                return this.Twice();
            ECCurve curve = this.Curve;
            SecP224R1FieldElement rawXcoord1 = (SecP224R1FieldElement)this.RawXCoord;
            SecP224R1FieldElement rawYcoord1 = (SecP224R1FieldElement)this.RawYCoord;
            SecP224R1FieldElement rawXcoord2 = (SecP224R1FieldElement)b.RawXCoord;
            SecP224R1FieldElement rawYcoord2 = (SecP224R1FieldElement)b.RawYCoord;
            SecP224R1FieldElement rawZcoord1 = (SecP224R1FieldElement)this.RawZCoords[0];
            SecP224R1FieldElement rawZcoord2 = (SecP224R1FieldElement)b.RawZCoords[0];
            uint[] ext = Nat224.CreateExt();
            uint[] numArray1 = Nat224.Create();
            uint[] numArray2 = Nat224.Create();
            uint[] x1 = Nat224.Create();
            bool isOne1 = rawZcoord1.IsOne;
            uint[] numArray3;
            uint[] numArray4;
            if (isOne1)
            {
                numArray3 = rawXcoord2.x;
                numArray4 = rawYcoord2.x;
            }
            else
            {
                numArray4 = numArray2;
                SecP224R1Field.Square( rawZcoord1.x, numArray4 );
                numArray3 = numArray1;
                SecP224R1Field.Multiply( numArray4, rawXcoord2.x, numArray3 );
                SecP224R1Field.Multiply( numArray4, rawZcoord1.x, numArray4 );
                SecP224R1Field.Multiply( numArray4, rawYcoord2.x, numArray4 );
            }
            bool isOne2 = rawZcoord2.IsOne;
            uint[] numArray5;
            uint[] numArray6;
            if (isOne2)
            {
                numArray5 = rawXcoord1.x;
                numArray6 = rawYcoord1.x;
            }
            else
            {
                numArray6 = x1;
                SecP224R1Field.Square( rawZcoord2.x, numArray6 );
                numArray5 = ext;
                SecP224R1Field.Multiply( numArray6, rawXcoord1.x, numArray5 );
                SecP224R1Field.Multiply( numArray6, rawZcoord2.x, numArray6 );
                SecP224R1Field.Multiply( numArray6, rawYcoord1.x, numArray6 );
            }
            uint[] numArray7 = Nat224.Create();
            SecP224R1Field.Subtract( numArray5, numArray3, numArray7 );
            uint[] numArray8 = numArray1;
            SecP224R1Field.Subtract( numArray6, numArray4, numArray8 );
            if (Nat224.IsZero( numArray7 ))
                return Nat224.IsZero( numArray8 ) ? this.Twice() : curve.Infinity;
            uint[] numArray9 = numArray2;
            SecP224R1Field.Square( numArray7, numArray9 );
            uint[] numArray10 = Nat224.Create();
            SecP224R1Field.Multiply( numArray9, numArray7, numArray10 );
            uint[] numArray11 = numArray2;
            SecP224R1Field.Multiply( numArray9, numArray5, numArray11 );
            SecP224R1Field.Negate( numArray10, numArray10 );
            Nat224.Mul( numArray6, numArray10, ext );
            SecP224R1Field.Reduce32( Nat224.AddBothTo( numArray11, numArray11, numArray10 ), numArray10 );
            SecP224R1FieldElement x2 = new( x1 );
            SecP224R1Field.Square( numArray8, x2.x );
            SecP224R1Field.Subtract( x2.x, numArray10, x2.x );
            SecP224R1FieldElement y = new( numArray10 );
            SecP224R1Field.Subtract( numArray11, x2.x, y.x );
            SecP224R1Field.MultiplyAddToExt( y.x, numArray8, ext );
            SecP224R1Field.Reduce( ext, y.x );
            SecP224R1FieldElement p224R1FieldElement = new( numArray7 );
            if (!isOne1)
                SecP224R1Field.Multiply( p224R1FieldElement.x, rawZcoord1.x, p224R1FieldElement.x );
            if (!isOne2)
                SecP224R1Field.Multiply( p224R1FieldElement.x, rawZcoord2.x, p224R1FieldElement.x );
            ECFieldElement[] zs = new ECFieldElement[1]
            {
         p224R1FieldElement
            };
            return new SecP224R1Point( curve, x2, y, zs, this.IsCompressed );
        }

        public override ECPoint Twice()
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            SecP224R1FieldElement rawYcoord = (SecP224R1FieldElement)this.RawYCoord;
            if (rawYcoord.IsZero)
                return curve.Infinity;
            SecP224R1FieldElement rawXcoord = (SecP224R1FieldElement)this.RawXCoord;
            SecP224R1FieldElement rawZcoord = (SecP224R1FieldElement)this.RawZCoords[0];
            uint[] numArray1 = Nat224.Create();
            uint[] numArray2 = Nat224.Create();
            uint[] numArray3 = Nat224.Create();
            SecP224R1Field.Square( rawYcoord.x, numArray3 );
            uint[] numArray4 = Nat224.Create();
            SecP224R1Field.Square( numArray3, numArray4 );
            bool isOne = rawZcoord.IsOne;
            uint[] numArray5 = rawZcoord.x;
            if (!isOne)
            {
                numArray5 = numArray2;
                SecP224R1Field.Square( rawZcoord.x, numArray5 );
            }
            SecP224R1Field.Subtract( rawXcoord.x, numArray5, numArray1 );
            uint[] numArray6 = numArray2;
            SecP224R1Field.Add( rawXcoord.x, numArray5, numArray6 );
            SecP224R1Field.Multiply( numArray6, numArray1, numArray6 );
            SecP224R1Field.Reduce32( Nat224.AddBothTo( numArray6, numArray6, numArray6 ), numArray6 );
            uint[] numArray7 = numArray3;
            SecP224R1Field.Multiply( numArray3, rawXcoord.x, numArray7 );
            SecP224R1Field.Reduce32( Nat.ShiftUpBits( 7, numArray7, 2, 0U ), numArray7 );
            SecP224R1Field.Reduce32( Nat.ShiftUpBits( 7, numArray4, 3, 0U, numArray1 ), numArray1 );
            SecP224R1FieldElement x = new( numArray4 );
            SecP224R1Field.Square( numArray6, x.x );
            SecP224R1Field.Subtract( x.x, numArray7, x.x );
            SecP224R1Field.Subtract( x.x, numArray7, x.x );
            SecP224R1FieldElement y = new( numArray7 );
            SecP224R1Field.Subtract( numArray7, x.x, y.x );
            SecP224R1Field.Multiply( y.x, numArray6, y.x );
            SecP224R1Field.Subtract( y.x, numArray1, y.x );
            SecP224R1FieldElement p224R1FieldElement = new( numArray6 );
            SecP224R1Field.Twice( rawYcoord.x, p224R1FieldElement.x );
            if (!isOne)
                SecP224R1Field.Multiply( p224R1FieldElement.x, rawZcoord.x, p224R1FieldElement.x );
            return new SecP224R1Point( curve, x, y, new ECFieldElement[1]
            {
         p224R1FieldElement
            }, (this.IsCompressed ? 1 : 0) != 0 );
        }

        public override ECPoint TwicePlus( ECPoint b )
        {
            if (this == b)
                return this.ThreeTimes();
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this.Twice();
            return this.RawYCoord.IsZero ? b : this.Twice().Add( b );
        }

        public override ECPoint ThreeTimes() => this.IsInfinity || this.RawYCoord.IsZero ? this : this.Twice().Add( this );

        public override ECPoint Negate() => this.IsInfinity ? this : (ECPoint)new SecP224R1Point( this.Curve, this.RawXCoord, this.RawYCoord.Negate(), this.RawZCoords, this.IsCompressed );
    }
}
