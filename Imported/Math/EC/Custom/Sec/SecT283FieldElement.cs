// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT283FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT283FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT283FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 283)
                throw new ArgumentException( "value invalid for SecT283FieldElement", nameof( x ) );
            this.x = SecT283Field.FromBigInteger( x );
        }

        public SecT283FieldElement() => this.x = Nat320.Create64();

        protected internal SecT283FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat320.IsOne64( this.x );

        public override bool IsZero => Nat320.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat320.ToBigInteger64( this.x );

        public override string FieldName => "SecT283Field";

        public override int FieldSize => 283;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat320.Create64();
            SecT283Field.Add( this.x, ((SecT283FieldElement)b).x, numArray );
            return new SecT283FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat320.Create64();
            SecT283Field.AddOne( this.x, numArray );
            return new SecT283FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat320.Create64();
            SecT283Field.Multiply( this.x, ((SecT283FieldElement)b).x, numArray );
            return new SecT283FieldElement( numArray );
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
            ulong[] x2 = ((SecT283FieldElement)b).x;
            ulong[] x3 = ((SecT283FieldElement)x).x;
            ulong[] x4 = ((SecT283FieldElement)y).x;
            ulong[] numArray1 = Nat.Create64( 9 );
            SecT283Field.MultiplyAddToExt( x1, x2, numArray1 );
            SecT283Field.MultiplyAddToExt( x3, x4, numArray1 );
            ulong[] numArray2 = Nat320.Create64();
            SecT283Field.Reduce( numArray1, numArray2 );
            return new SecT283FieldElement( numArray2 );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat320.Create64();
            SecT283Field.Square( this.x, numArray );
            return new SecT283FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT283FieldElement)x).x;
            ulong[] x3 = ((SecT283FieldElement)y).x;
            ulong[] numArray1 = Nat.Create64( 9 );
            SecT283Field.SquareAddToExt( x1, numArray1 );
            SecT283Field.MultiplyAddToExt( x2, x3, numArray1 );
            ulong[] numArray2 = Nat320.Create64();
            SecT283Field.Reduce( numArray1, numArray2 );
            return new SecT283FieldElement( numArray2 );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat320.Create64();
            SecT283Field.SquareN( this.x, pow, numArray );
            return new SecT283FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat320.Create64();
            SecT283Field.Invert( this.x, numArray );
            return new SecT283FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat320.Create64();
            SecT283Field.Sqrt( this.x, numArray );
            return new SecT283FieldElement( numArray );
        }

        public virtual int Representation => 3;

        public virtual int M => 283;

        public virtual int K1 => 5;

        public virtual int K2 => 7;

        public virtual int K3 => 12;

        public override bool Equals( object obj ) => this.Equals( obj as SecT283FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT283FieldElement );

        public virtual bool Equals( SecT283FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat320.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 2831275 ^ Arrays.GetHashCode( this.x, 0, 5 );
    }
}
