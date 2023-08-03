// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP384R1Point
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP384R1Point : AbstractFpPoint
    {
        public SecP384R1Point( ECCurve curve, ECFieldElement x, ECFieldElement y )
          : this( curve, x, y, false )
        {
        }

        public SecP384R1Point( ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression )
          : base( curve, x, y, withCompression )
        {
            if (x == null != (y == null))
                throw new ArgumentException( "Exactly one of the field elements is null" );
        }

        internal SecP384R1Point(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        protected override ECPoint Detach() => new SecP384R1Point( null, this.AffineXCoord, this.AffineYCoord );

        public override ECPoint Add( ECPoint b )
        {
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this;
            if (this == b)
                return this.Twice();
            ECCurve curve = this.Curve;
            SecP384R1FieldElement rawXcoord1 = (SecP384R1FieldElement)this.RawXCoord;
            SecP384R1FieldElement rawYcoord1 = (SecP384R1FieldElement)this.RawYCoord;
            SecP384R1FieldElement rawXcoord2 = (SecP384R1FieldElement)b.RawXCoord;
            SecP384R1FieldElement rawYcoord2 = (SecP384R1FieldElement)b.RawYCoord;
            SecP384R1FieldElement rawZcoord1 = (SecP384R1FieldElement)this.RawZCoords[0];
            SecP384R1FieldElement rawZcoord2 = (SecP384R1FieldElement)b.RawZCoords[0];
            uint[] numArray1 = Nat.Create( 24 );
            uint[] numArray2 = Nat.Create( 24 );
            uint[] numArray3 = Nat.Create( 12 );
            uint[] x1 = Nat.Create( 12 );
            bool isOne1 = rawZcoord1.IsOne;
            uint[] numArray4;
            uint[] numArray5;
            if (isOne1)
            {
                numArray4 = rawXcoord2.x;
                numArray5 = rawYcoord2.x;
            }
            else
            {
                numArray5 = numArray3;
                SecP384R1Field.Square( rawZcoord1.x, numArray5 );
                numArray4 = numArray2;
                SecP384R1Field.Multiply( numArray5, rawXcoord2.x, numArray4 );
                SecP384R1Field.Multiply( numArray5, rawZcoord1.x, numArray5 );
                SecP384R1Field.Multiply( numArray5, rawYcoord2.x, numArray5 );
            }
            bool isOne2 = rawZcoord2.IsOne;
            uint[] numArray6;
            uint[] numArray7;
            if (isOne2)
            {
                numArray6 = rawXcoord1.x;
                numArray7 = rawYcoord1.x;
            }
            else
            {
                numArray7 = x1;
                SecP384R1Field.Square( rawZcoord2.x, numArray7 );
                numArray6 = numArray1;
                SecP384R1Field.Multiply( numArray7, rawXcoord1.x, numArray6 );
                SecP384R1Field.Multiply( numArray7, rawZcoord2.x, numArray7 );
                SecP384R1Field.Multiply( numArray7, rawYcoord1.x, numArray7 );
            }
            uint[] numArray8 = Nat.Create( 12 );
            SecP384R1Field.Subtract( numArray6, numArray4, numArray8 );
            uint[] numArray9 = Nat.Create( 12 );
            SecP384R1Field.Subtract( numArray7, numArray5, numArray9 );
            if (Nat.IsZero( 12, numArray8 ))
                return Nat.IsZero( 12, numArray9 ) ? this.Twice() : curve.Infinity;
            uint[] numArray10 = numArray3;
            SecP384R1Field.Square( numArray8, numArray10 );
            uint[] numArray11 = Nat.Create( 12 );
            SecP384R1Field.Multiply( numArray10, numArray8, numArray11 );
            uint[] numArray12 = numArray3;
            SecP384R1Field.Multiply( numArray10, numArray6, numArray12 );
            SecP384R1Field.Negate( numArray11, numArray11 );
            Nat384.Mul( numArray7, numArray11, numArray1 );
            SecP384R1Field.Reduce32( Nat.AddBothTo( 12, numArray12, numArray12, numArray11 ), numArray11 );
            SecP384R1FieldElement x2 = new SecP384R1FieldElement( x1 );
            SecP384R1Field.Square( numArray9, x2.x );
            SecP384R1Field.Subtract( x2.x, numArray11, x2.x );
            SecP384R1FieldElement y = new SecP384R1FieldElement( numArray11 );
            SecP384R1Field.Subtract( numArray12, x2.x, y.x );
            Nat384.Mul( y.x, numArray9, numArray2 );
            SecP384R1Field.AddExt( numArray1, numArray2, numArray1 );
            SecP384R1Field.Reduce( numArray1, y.x );
            SecP384R1FieldElement p384R1FieldElement = new SecP384R1FieldElement( numArray8 );
            if (!isOne1)
                SecP384R1Field.Multiply( p384R1FieldElement.x, rawZcoord1.x, p384R1FieldElement.x );
            if (!isOne2)
                SecP384R1Field.Multiply( p384R1FieldElement.x, rawZcoord2.x, p384R1FieldElement.x );
            ECFieldElement[] zs = new ECFieldElement[1]
            {
         p384R1FieldElement
            };
            return new SecP384R1Point( curve, x2, y, zs, this.IsCompressed );
        }

        public override ECPoint Twice()
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            SecP384R1FieldElement rawYcoord = (SecP384R1FieldElement)this.RawYCoord;
            if (rawYcoord.IsZero)
                return curve.Infinity;
            SecP384R1FieldElement rawXcoord = (SecP384R1FieldElement)this.RawXCoord;
            SecP384R1FieldElement rawZcoord = (SecP384R1FieldElement)this.RawZCoords[0];
            uint[] numArray1 = Nat.Create( 12 );
            uint[] numArray2 = Nat.Create( 12 );
            uint[] numArray3 = Nat.Create( 12 );
            SecP384R1Field.Square( rawYcoord.x, numArray3 );
            uint[] numArray4 = Nat.Create( 12 );
            SecP384R1Field.Square( numArray3, numArray4 );
            bool isOne = rawZcoord.IsOne;
            uint[] numArray5 = rawZcoord.x;
            if (!isOne)
            {
                numArray5 = numArray2;
                SecP384R1Field.Square( rawZcoord.x, numArray5 );
            }
            SecP384R1Field.Subtract( rawXcoord.x, numArray5, numArray1 );
            uint[] numArray6 = numArray2;
            SecP384R1Field.Add( rawXcoord.x, numArray5, numArray6 );
            SecP384R1Field.Multiply( numArray6, numArray1, numArray6 );
            SecP384R1Field.Reduce32( Nat.AddBothTo( 12, numArray6, numArray6, numArray6 ), numArray6 );
            uint[] numArray7 = numArray3;
            SecP384R1Field.Multiply( numArray3, rawXcoord.x, numArray7 );
            SecP384R1Field.Reduce32( Nat.ShiftUpBits( 12, numArray7, 2, 0U ), numArray7 );
            SecP384R1Field.Reduce32( Nat.ShiftUpBits( 12, numArray4, 3, 0U, numArray1 ), numArray1 );
            SecP384R1FieldElement x = new SecP384R1FieldElement( numArray4 );
            SecP384R1Field.Square( numArray6, x.x );
            SecP384R1Field.Subtract( x.x, numArray7, x.x );
            SecP384R1Field.Subtract( x.x, numArray7, x.x );
            SecP384R1FieldElement y = new SecP384R1FieldElement( numArray7 );
            SecP384R1Field.Subtract( numArray7, x.x, y.x );
            SecP384R1Field.Multiply( y.x, numArray6, y.x );
            SecP384R1Field.Subtract( y.x, numArray1, y.x );
            SecP384R1FieldElement p384R1FieldElement = new SecP384R1FieldElement( numArray6 );
            SecP384R1Field.Twice( rawYcoord.x, p384R1FieldElement.x );
            if (!isOne)
                SecP384R1Field.Multiply( p384R1FieldElement.x, rawZcoord.x, p384R1FieldElement.x );
            return new SecP384R1Point( curve, x, y, new ECFieldElement[1]
            {
         p384R1FieldElement
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

        public override ECPoint Negate() => this.IsInfinity ? this : (ECPoint)new SecP384R1Point( this.Curve, this.RawXCoord, this.RawYCoord.Negate(), this.RawZCoords, this.IsCompressed );
    }
}
