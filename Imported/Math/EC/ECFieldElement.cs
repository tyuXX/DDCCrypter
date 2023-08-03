// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.ECFieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class ECFieldElement
    {
        public abstract BigInteger ToBigInteger();

        public abstract string FieldName { get; }

        public abstract int FieldSize { get; }

        public abstract ECFieldElement Add( ECFieldElement b );

        public abstract ECFieldElement AddOne();

        public abstract ECFieldElement Subtract( ECFieldElement b );

        public abstract ECFieldElement Multiply( ECFieldElement b );

        public abstract ECFieldElement Divide( ECFieldElement b );

        public abstract ECFieldElement Negate();

        public abstract ECFieldElement Square();

        public abstract ECFieldElement Invert();

        public abstract ECFieldElement Sqrt();

        public virtual int BitLength => this.ToBigInteger().BitLength;

        public virtual bool IsOne => this.BitLength == 1;

        public virtual bool IsZero => 0 == this.ToBigInteger().SignValue;

        public virtual ECFieldElement MultiplyMinusProduct(
          ECFieldElement b,
          ECFieldElement x,
          ECFieldElement y )
        {
            return this.Multiply( b ).Subtract( x.Multiply( y ) );
        }

        public virtual ECFieldElement MultiplyPlusProduct(
          ECFieldElement b,
          ECFieldElement x,
          ECFieldElement y )
        {
            return this.Multiply( b ).Add( x.Multiply( y ) );
        }

        public virtual ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.Square().Subtract( x.Multiply( y ) );

        public virtual ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y ) => this.Square().Add( x.Multiply( y ) );

        public virtual ECFieldElement SquarePow( int pow )
        {
            ECFieldElement ecFieldElement = this;
            for (int index = 0; index < pow; ++index)
                ecFieldElement = ecFieldElement.Square();
            return ecFieldElement;
        }

        public virtual bool TestBitZero() => this.ToBigInteger().TestBit( 0 );

        public override bool Equals( object obj ) => this.Equals( obj as ECFieldElement );

        public virtual bool Equals( ECFieldElement other )
        {
            if (this == other)
                return true;
            return other != null && this.ToBigInteger().Equals( other.ToBigInteger() );
        }

        public override int GetHashCode() => this.ToBigInteger().GetHashCode();

        public override string ToString() => this.ToBigInteger().ToString( 16 );

        public virtual byte[] GetEncoded() => BigIntegers.AsUnsignedByteArray( (this.FieldSize + 7) / 8, this.ToBigInteger() );
    }
}
