﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT233FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT233FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT233FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 233)
                throw new ArgumentException( "value invalid for SecT233FieldElement", nameof( x ) );
            this.x = SecT233Field.FromBigInteger( x );
        }

        public SecT233FieldElement() => this.x = Nat256.Create64();

        protected internal SecT233FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat256.IsOne64( this.x );

        public override bool IsZero => Nat256.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat256.ToBigInteger64( this.x );

        public override string FieldName => "SecT233Field";

        public override int FieldSize => 233;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Add( this.x, ((SecT233FieldElement)b).x, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat256.Create64();
            SecT233Field.AddOne( this.x, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Multiply( this.x, ((SecT233FieldElement)b).x, numArray );
            return new SecT233FieldElement( numArray );
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
            ulong[] x2 = ((SecT233FieldElement)b).x;
            ulong[] x3 = ((SecT233FieldElement)x).x;
            ulong[] x4 = ((SecT233FieldElement)y).x;
            ulong[] ext64 = Nat256.CreateExt64();
            SecT233Field.MultiplyAddToExt( x1, x2, ext64 );
            SecT233Field.MultiplyAddToExt( x3, x4, ext64 );
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Reduce( ext64, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Square( this.x, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT233FieldElement)x).x;
            ulong[] x3 = ((SecT233FieldElement)y).x;
            ulong[] ext64 = Nat256.CreateExt64();
            SecT233Field.SquareAddToExt( x1, ext64 );
            SecT233Field.MultiplyAddToExt( x2, x3, ext64 );
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Reduce( ext64, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat256.Create64();
            SecT233Field.SquareN( this.x, pow, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Invert( this.x, numArray );
            return new SecT233FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat256.Create64();
            SecT233Field.Sqrt( this.x, numArray );
            return new SecT233FieldElement( numArray );
        }

        public virtual int Representation => 2;

        public virtual int M => 233;

        public virtual int K1 => 74;

        public virtual int K2 => 0;

        public virtual int K3 => 0;

        public override bool Equals( object obj ) => this.Equals( obj as SecT233FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT233FieldElement );

        public virtual bool Equals( SecT233FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat256.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 2330074 ^ Arrays.GetHashCode( this.x, 0, 4 );
    }
}
