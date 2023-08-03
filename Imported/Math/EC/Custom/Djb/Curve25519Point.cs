// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Djb.Curve25519Point
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    internal class Curve25519Point : AbstractFpPoint
    {
        public Curve25519Point( ECCurve curve, ECFieldElement x, ECFieldElement y )
          : this( curve, x, y, false )
        {
        }

        public Curve25519Point(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
          : base( curve, x, y, withCompression )
        {
            if (x == null != (y == null))
                throw new ArgumentException( "Exactly one of the field elements is null" );
        }

        internal Curve25519Point(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        protected override ECPoint Detach() => new Curve25519Point( null, this.AffineXCoord, this.AffineYCoord );

        public override ECFieldElement GetZCoord( int index ) => index == 1 ? this.GetJacobianModifiedW() : base.GetZCoord( index );

        public override ECPoint Add( ECPoint b )
        {
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this;
            if (this == b)
                return this.Twice();
            ECCurve curve = this.Curve;
            Curve25519FieldElement rawXcoord1 = (Curve25519FieldElement)this.RawXCoord;
            Curve25519FieldElement rawYcoord1 = (Curve25519FieldElement)this.RawYCoord;
            Curve25519FieldElement rawZcoord1 = (Curve25519FieldElement)this.RawZCoords[0];
            Curve25519FieldElement rawXcoord2 = (Curve25519FieldElement)b.RawXCoord;
            Curve25519FieldElement rawYcoord2 = (Curve25519FieldElement)b.RawYCoord;
            Curve25519FieldElement rawZcoord2 = (Curve25519FieldElement)b.RawZCoords[0];
            uint[] ext = Nat256.CreateExt();
            uint[] numArray1 = Nat256.Create();
            uint[] numArray2 = Nat256.Create();
            uint[] x1 = Nat256.Create();
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
                Curve25519Field.Square( rawZcoord1.x, numArray4 );
                numArray3 = numArray1;
                Curve25519Field.Multiply( numArray4, rawXcoord2.x, numArray3 );
                Curve25519Field.Multiply( numArray4, rawZcoord1.x, numArray4 );
                Curve25519Field.Multiply( numArray4, rawYcoord2.x, numArray4 );
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
                Curve25519Field.Square( rawZcoord2.x, numArray6 );
                numArray5 = ext;
                Curve25519Field.Multiply( numArray6, rawXcoord1.x, numArray5 );
                Curve25519Field.Multiply( numArray6, rawZcoord2.x, numArray6 );
                Curve25519Field.Multiply( numArray6, rawYcoord1.x, numArray6 );
            }
            uint[] numArray7 = Nat256.Create();
            Curve25519Field.Subtract( numArray5, numArray3, numArray7 );
            uint[] numArray8 = numArray1;
            Curve25519Field.Subtract( numArray6, numArray4, numArray8 );
            if (Nat256.IsZero( numArray7 ))
                return Nat256.IsZero( numArray8 ) ? this.Twice() : curve.Infinity;
            uint[] numArray9 = Nat256.Create();
            Curve25519Field.Square( numArray7, numArray9 );
            uint[] numArray10 = Nat256.Create();
            Curve25519Field.Multiply( numArray9, numArray7, numArray10 );
            uint[] numArray11 = numArray2;
            Curve25519Field.Multiply( numArray9, numArray5, numArray11 );
            Curve25519Field.Negate( numArray10, numArray10 );
            Nat256.Mul( numArray6, numArray10, ext );
            Curve25519Field.Reduce27( Nat256.AddBothTo( numArray11, numArray11, numArray10 ), numArray10 );
            Curve25519FieldElement x2 = new( x1 );
            Curve25519Field.Square( numArray8, x2.x );
            Curve25519Field.Subtract( x2.x, numArray10, x2.x );
            Curve25519FieldElement y = new( numArray10 );
            Curve25519Field.Subtract( numArray11, x2.x, y.x );
            Curve25519Field.MultiplyAddToExt( y.x, numArray8, ext );
            Curve25519Field.Reduce( ext, y.x );
            Curve25519FieldElement Z = new( numArray7 );
            if (!isOne1)
                Curve25519Field.Multiply( Z.x, rawZcoord1.x, Z.x );
            if (!isOne2)
                Curve25519Field.Multiply( Z.x, rawZcoord2.x, Z.x );
            uint[] ZSquared = !isOne1 || !isOne2 ? null : numArray9;
            Curve25519FieldElement jacobianModifiedW = this.CalculateJacobianModifiedW( Z, ZSquared );
            ECFieldElement[] zs = new ECFieldElement[2]
            {
         Z,
         jacobianModifiedW
            };
            return new Curve25519Point( curve, x2, y, zs, this.IsCompressed );
        }

        public override ECPoint Twice()
        {
            if (this.IsInfinity)
                return this;
            ECCurve curve = this.Curve;
            return this.RawYCoord.IsZero ? curve.Infinity : this.TwiceJacobianModified( true );
        }

        public override ECPoint TwicePlus( ECPoint b )
        {
            if (this == b)
                return this.ThreeTimes();
            if (this.IsInfinity)
                return b;
            if (b.IsInfinity)
                return this.Twice();
            return this.RawYCoord.IsZero ? b : this.TwiceJacobianModified( false ).Add( b );
        }

        public override ECPoint ThreeTimes() => this.IsInfinity || this.RawYCoord.IsZero ? this : this.TwiceJacobianModified( false ).Add( this );

        public override ECPoint Negate() => this.IsInfinity ? this : (ECPoint)new Curve25519Point( this.Curve, this.RawXCoord, this.RawYCoord.Negate(), this.RawZCoords, this.IsCompressed );

        protected virtual Curve25519FieldElement CalculateJacobianModifiedW(
          Curve25519FieldElement Z,
          uint[] ZSquared )
        {
            Curve25519FieldElement a = (Curve25519FieldElement)this.Curve.A;
            if (Z.IsOne)
                return a;
            Curve25519FieldElement jacobianModifiedW = new();
            if (ZSquared == null)
            {
                ZSquared = jacobianModifiedW.x;
                Curve25519Field.Square( Z.x, ZSquared );
            }
            Curve25519Field.Square( ZSquared, jacobianModifiedW.x );
            Curve25519Field.Multiply( jacobianModifiedW.x, a.x, jacobianModifiedW.x );
            return jacobianModifiedW;
        }

        protected virtual Curve25519FieldElement GetJacobianModifiedW()
        {
            ECFieldElement[] rawZcoords = this.RawZCoords;
            Curve25519FieldElement jacobianModifiedW = (Curve25519FieldElement)rawZcoords[1];
            if (jacobianModifiedW == null)
                rawZcoords[1] = jacobianModifiedW = this.CalculateJacobianModifiedW( (Curve25519FieldElement)rawZcoords[0], null );
            return jacobianModifiedW;
        }

        protected virtual Curve25519Point TwiceJacobianModified( bool calculateW )
        {
            Curve25519FieldElement rawXcoord = (Curve25519FieldElement)this.RawXCoord;
            Curve25519FieldElement rawYcoord = (Curve25519FieldElement)this.RawYCoord;
            Curve25519FieldElement rawZcoord = (Curve25519FieldElement)this.RawZCoords[0];
            Curve25519FieldElement jacobianModifiedW = this.GetJacobianModifiedW();
            uint[] numArray1 = Nat256.Create();
            Curve25519Field.Square( rawXcoord.x, numArray1 );
            Curve25519Field.Reduce27( Nat256.AddBothTo( numArray1, numArray1, numArray1 ) + Nat256.AddTo( jacobianModifiedW.x, numArray1 ), numArray1 );
            uint[] numArray2 = Nat256.Create();
            Curve25519Field.Twice( rawYcoord.x, numArray2 );
            uint[] numArray3 = Nat256.Create();
            Curve25519Field.Multiply( numArray2, rawYcoord.x, numArray3 );
            uint[] numArray4 = Nat256.Create();
            Curve25519Field.Multiply( numArray3, rawXcoord.x, numArray4 );
            Curve25519Field.Twice( numArray4, numArray4 );
            uint[] numArray5 = Nat256.Create();
            Curve25519Field.Square( numArray3, numArray5 );
            Curve25519Field.Twice( numArray5, numArray5 );
            Curve25519FieldElement x = new( numArray3 );
            Curve25519Field.Square( numArray1, x.x );
            Curve25519Field.Subtract( x.x, numArray4, x.x );
            Curve25519Field.Subtract( x.x, numArray4, x.x );
            Curve25519FieldElement y = new( numArray4 );
            Curve25519Field.Subtract( numArray4, x.x, y.x );
            Curve25519Field.Multiply( y.x, numArray1, y.x );
            Curve25519Field.Subtract( y.x, numArray5, y.x );
            Curve25519FieldElement curve25519FieldElement1 = new( numArray2 );
            if (!Nat256.IsOne( rawZcoord.x ))
                Curve25519Field.Multiply( curve25519FieldElement1.x, rawZcoord.x, curve25519FieldElement1.x );
            Curve25519FieldElement curve25519FieldElement2 = null;
            if (calculateW)
            {
                curve25519FieldElement2 = new Curve25519FieldElement( numArray5 );
                Curve25519Field.Multiply( curve25519FieldElement2.x, jacobianModifiedW.x, curve25519FieldElement2.x );
                Curve25519Field.Twice( curve25519FieldElement2.x, curve25519FieldElement2.x );
            }
            return new Curve25519Point( this.Curve, x, y, new ECFieldElement[2]
            {
         curve25519FieldElement1,
         curve25519FieldElement2
            }, (this.IsCompressed ? 1 : 0) != 0 );
        }
    }
}
