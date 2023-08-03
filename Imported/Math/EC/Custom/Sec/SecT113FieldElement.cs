// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT113FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT113FieldElement : ECFieldElement
    {
        protected internal readonly ulong[] x;

        public SecT113FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 113)
                throw new ArgumentException( "value invalid for SecT113FieldElement", nameof( x ) );
            this.x = SecT113Field.FromBigInteger( x );
        }

        public SecT113FieldElement() => this.x = Nat128.Create64();

        protected internal SecT113FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat128.IsOne64( this.x );

        public override bool IsZero => Nat128.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat128.ToBigInteger64( this.x );

        public override string FieldName => "SecT113Field";

        public override int FieldSize => 113;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Add( this.x, ((SecT113FieldElement)b).x, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat128.Create64();
            SecT113Field.AddOne( this.x, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Multiply( this.x, ((SecT113FieldElement)b).x, numArray );
            return new SecT113FieldElement( numArray );
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
            ulong[] x2 = ((SecT113FieldElement)b).x;
            ulong[] x3 = ((SecT113FieldElement)x).x;
            ulong[] x4 = ((SecT113FieldElement)y).x;
            ulong[] ext64 = Nat128.CreateExt64();
            SecT113Field.MultiplyAddToExt( x1, x2, ext64 );
            SecT113Field.MultiplyAddToExt( x3, x4, ext64 );
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Reduce( ext64, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Square( this.x, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT113FieldElement)x).x;
            ulong[] x3 = ((SecT113FieldElement)y).x;
            ulong[] ext64 = Nat128.CreateExt64();
            SecT113Field.SquareAddToExt( x1, ext64 );
            SecT113Field.MultiplyAddToExt( x2, x3, ext64 );
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Reduce( ext64, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat128.Create64();
            SecT113Field.SquareN( this.x, pow, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Invert( this.x, numArray );
            return new SecT113FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat128.Create64();
            SecT113Field.Sqrt( this.x, numArray );
            return new SecT113FieldElement( numArray );
        }

        public virtual int Representation => 2;

        public virtual int M => 113;

        public virtual int K1 => 9;

        public virtual int K2 => 0;

        public virtual int K3 => 0;

        public override bool Equals( object obj ) => this.Equals( obj as SecT113FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT113FieldElement );

        public virtual bool Equals( SecT113FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat128.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 113009 ^ Arrays.GetHashCode( this.x, 0, 2 );
    }
}
