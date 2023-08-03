// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT239FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT239FieldElement : ECFieldElement
    {
        protected ulong[] x;

        public SecT239FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > 239)
                throw new ArgumentException( "value invalid for SecT239FieldElement", nameof( x ) );
            this.x = SecT239Field.FromBigInteger( x );
        }

        public SecT239FieldElement() => this.x = Nat256.Create64();

        protected internal SecT239FieldElement( ulong[] x ) => this.x = x;

        public override bool IsOne => Nat256.IsOne64( this.x );

        public override bool IsZero => Nat256.IsZero64( this.x );

        public override bool TestBitZero() => ((long)this.x[0] & 1L) != 0L;

        public override BigInteger ToBigInteger() => Nat256.ToBigInteger64( this.x );

        public override string FieldName => "SecT239Field";

        public override int FieldSize => 239;

        public override ECFieldElement Add( ECFieldElement b )
        {
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Add( this.x, ((SecT239FieldElement)b).x, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            ulong[] numArray = Nat256.Create64();
            SecT239Field.AddOne( this.x, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Multiply( this.x, ((SecT239FieldElement)b).x, numArray );
            return new SecT239FieldElement( numArray );
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
            ulong[] x2 = ((SecT239FieldElement)b).x;
            ulong[] x3 = ((SecT239FieldElement)x).x;
            ulong[] x4 = ((SecT239FieldElement)y).x;
            ulong[] ext64 = Nat256.CreateExt64();
            SecT239Field.MultiplyAddToExt( x1, x2, ext64 );
            SecT239Field.MultiplyAddToExt( x3, x4, ext64 );
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Reduce( ext64, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square()
        {
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Square( this.x, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            ulong[] x1 = this.x;
            ulong[] x2 = ((SecT239FieldElement)x).x;
            ulong[] x3 = ((SecT239FieldElement)y).x;
            ulong[] ext64 = Nat256.CreateExt64();
            SecT239Field.SquareAddToExt( x1, ext64 );
            SecT239Field.MultiplyAddToExt( x2, x3, ext64 );
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Reduce( ext64, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement SquarePow( int pow )
        {
            if (pow < 1)
                return this;
            ulong[] numArray = Nat256.Create64();
            SecT239Field.SquareN( this.x, pow, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Invert( this.x, numArray );
            return new SecT239FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            ulong[] numArray = Nat256.Create64();
            SecT239Field.Sqrt( this.x, numArray );
            return new SecT239FieldElement( numArray );
        }

        public virtual int Representation => 2;

        public virtual int M => 239;

        public virtual int K1 => 158;

        public virtual int K2 => 0;

        public virtual int K3 => 0;

        public override bool Equals( object obj ) => this.Equals( obj as SecT239FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecT239FieldElement );

        public virtual bool Equals( SecT239FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat256.Eq64( this.x, other.x );
        }

        public override int GetHashCode() => 23900158 ^ Arrays.GetHashCode( this.x, 0, 4 );
    }
}
