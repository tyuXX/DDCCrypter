// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT131FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT131FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT131FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 131)
                throw new ArgumentException( "value invalid for SecT131FieldElement", nameof( x ) );
            this.x = SecT131Field.FromBigInteger( x );
        }

        public SecT131FieldElement() => this.x = Nat192.Create64();

        protected internal SecT131FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat192.IsOne64( this.x );

        public override bool IsZero => Nat192.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat192.ToBigInteger64( this.x );

        public override string FieldName => "SecT131Field";

        public override int FieldSize => 131;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat192.Create64();
            SecT131Field.Add( this.x, ((SecT131FieldElement)b).x, numArray );
            return new SecT131FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat192.Create64();
            SecT131Field.AddOne( this.x, numArray );
            return new SecT131FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat192.Create64();
            SecT131Field.Multiply( this.x, ((SecT131FieldElement)b).x, numArray );
            return new SecT131FieldElement( numArray );
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
            ulong[] x2 = ((SecT131FieldElement)b).x;
            ulong[] x3 = ((SecT131FieldElement)x).x;
            ulong[] x4 = ((SecT131FieldElement)y).x;
            ulong[] numArray1 = Nat.Create64( 5 );
            SecT131Field.MultiplyAddToExt( x1, x2, numArray1 );
            SecT131Field.MultiplyAddToExt( x3, x4, numArray1 );
            ulong[] numArray2 = Nat192.Create64();
            SecT131Field.Reduce( numArray1, numArray2 );
            return new SecT131FieldElement( numArray2 );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat192.Create64();
            SecT131Field.Square( this.x, numArray );
            return new SecT131FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT131FieldElement)x).x;
            ulong[] x3 = ((SecT131FieldElement)y).x;
            ulong[] numArray1 = Nat.Create64( 5 );
            SecT131Field.SquareAddToExt( x1, numArray1 );
            SecT131Field.MultiplyAddToExt( x2, x3, numArray1 );
            ulong[] numArray2 = Nat192.Create64();
            SecT131Field.Reduce( numArray1, numArray2 );
            return new SecT131FieldElement( numArray2 );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat192.Create64();
            SecT131Field.SquareN( this.x, pow, numArray );
            return new SecT131FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat192.Create64();
            SecT131Field.Invert( this.x, numArray );
            return new SecT131FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat192.Create64();
            SecT131Field.Sqrt( this.x, numArray );
            return new SecT131FieldElement( numArray );
        }

        public virtual int Representation => 3;

        public virtual int M => 131;

        public virtual int K1 => 2;

        public virtual int K2 => 3;

        public virtual int K3 => 8;

        public override bool Equals( object obj ) => this.Equals( obj as SecT131FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT131FieldElement );

        public virtual bool Equals( SecT131FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat192.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 131832 ^ Arrays.GetHashCode( this.x, 0, 3 );
    }
}
