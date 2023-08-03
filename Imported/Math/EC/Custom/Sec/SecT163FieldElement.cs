// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT163FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT163FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT163FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 163)
                throw new ArgumentException( "value invalid for SecT163FieldElement", nameof( x ) );
            this.x = SecT163Field.FromBigInteger( x );
        }

        public SecT163FieldElement() => this.x = Nat192.Create64();

        protected internal SecT163FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat192.IsOne64( this.x );

        public override bool IsZero => Nat192.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat192.ToBigInteger64( this.x );

        public override string FieldName => "SecT163Field";

        public override int FieldSize => 163;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Add( this.x, ((SecT163FieldElement)b).x, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat192.Create64();
            SecT163Field.AddOne( this.x, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Multiply( this.x, ((SecT163FieldElement)b).x, numArray );
            return new SecT163FieldElement( numArray );
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
            ulong[] x2 = ((SecT163FieldElement)b).x;
            ulong[] x3 = ((SecT163FieldElement)x).x;
            ulong[] x4 = ((SecT163FieldElement)y).x;
            ulong[] ext64 = Nat192.CreateExt64();
            SecT163Field.MultiplyAddToExt( x1, x2, ext64 );
            SecT163Field.MultiplyAddToExt( x3, x4, ext64 );
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Reduce( ext64, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Square( this.x, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT163FieldElement)x).x;
            ulong[] x3 = ((SecT163FieldElement)y).x;
            ulong[] ext64 = Nat192.CreateExt64();
            SecT163Field.SquareAddToExt( x1, ext64 );
            SecT163Field.MultiplyAddToExt( x2, x3, ext64 );
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Reduce( ext64, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat192.Create64();
            SecT163Field.SquareN( this.x, pow, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Invert( this.x, numArray );
            return new SecT163FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat192.Create64();
            SecT163Field.Sqrt( this.x, numArray );
            return new SecT163FieldElement( numArray );
        }

        public virtual int Representation => 3;

        public virtual int M => 163;

        public virtual int K1 => 3;

        public virtual int K2 => 6;

        public virtual int K3 => 7;

        public override bool Equals( object obj ) => this.Equals( obj as SecT163FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT163FieldElement );

        public virtual bool Equals( SecT163FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat192.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 163763 ^ Arrays.GetHashCode( this.x, 0, 3 );
    }
}
