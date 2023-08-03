// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT409FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT409FieldElement : ECFieldElement
    {
        protected ulong[] x;

        public SecT409FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 409)
                throw new ArgumentException( "value invalid for SecT409FieldElement", nameof( x ) );
            this.x = SecT409Field.FromBigInteger( x );
        }

        public SecT409FieldElement() => this.x = Nat448.Create64();

        protected internal SecT409FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat448.IsOne64( this.x );

        public override bool IsZero => Nat448.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat448.ToBigInteger64( this.x );

        public override string FieldName => "SecT409Field";

        public override int FieldSize => 409;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat448.Create64();
            SecT409Field.Add( this.x, ((SecT409FieldElement)b).x, numArray );
            return new SecT409FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat448.Create64();
            SecT409Field.AddOne( this.x, numArray );
            return new SecT409FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat448.Create64();
            SecT409Field.Multiply( this.x, ((SecT409FieldElement)b).x, numArray );
            return new SecT409FieldElement( numArray );
        }

        public override ECFieldElement MultiplyMinusProduct(
          ECFieldElement b,
          ECFieldElement x,
          ECFieldElement y )
        {
            return this.MultiplyPlusProduct( b, x, y );
        }

        public override ECFieldElement MultiplyPlusProduct(
          ECFieldElement b,
          ECFieldElement x,
          ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT409FieldElement)b).x;
            ulong[] x3 = ((SecT409FieldElement)x).x;
            ulong[] x4 = ((SecT409FieldElement)y).x;
            ulong[] numArray1 = Nat.Create64( 13 );
            SecT409Field.MultiplyAddToExt( x1, x2, numArray1 );
            SecT409Field.MultiplyAddToExt( x3, x4, numArray1 );
            ulong[] numArray2 = Nat448.Create64();
            SecT409Field.Reduce( numArray1, numArray2 );
            return new SecT409FieldElement( numArray2 );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat448.Create64();
            SecT409Field.Square( this.x, numArray );
            return new SecT409FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT409FieldElement)x).x;
            ulong[] x3 = ((SecT409FieldElement)y).x;
            ulong[] numArray1 = Nat.Create64( 13 );
            SecT409Field.SquareAddToExt( x1, numArray1 );
            SecT409Field.MultiplyAddToExt( x2, x3, numArray1 );
            ulong[] numArray2 = Nat448.Create64();
            SecT409Field.Reduce( numArray1, numArray2 );
            return new SecT409FieldElement( numArray2 );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat448.Create64();
            SecT409Field.SquareN( this.x, pow, numArray );
            return new SecT409FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat448.Create64();
            SecT409Field.Invert( this.x, numArray );
            return new SecT409FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat448.Create64();
            SecT409Field.Sqrt( this.x, numArray );
            return new SecT409FieldElement( numArray );
        }

        public virtual int Representation => 2;

        public virtual int M => 409;

        public virtual int K1 => 87;

        public virtual int K2 => 0;

        public virtual int K3 => 0;

        public override bool Equals( object obj ) => this.Equals( obj as SecT409FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT409FieldElement );

        public virtual bool Equals( SecT409FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat448.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 4090087 ^ Arrays.GetHashCode( this.x, 0, 7 );
    }
}
