// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP160R1FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP160R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP160R1Curve.q;
        protected internal readonly uint[] x;

        public SecP160R1FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for SecP160R1FieldElement", nameof( x ) );
            this.x = SecP160R1Field.FromBigInteger( x );
        }

        public SecP160R1FieldElement() => this.x = Nat160.Create();

        protected internal SecP160R1FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat160.IsZero( this.x );

        public override bool IsOne => Nat160.IsOne( this.x );

        public override bool TestBitZero() => Nat160.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat160.ToBigInteger( this.x );

        public override string FieldName => "SecP160R1Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat160.Create();
            SecP160R1Field.Add( this.x, ((SecP160R1FieldElement)b).x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat160.Create();
            SecP160R1Field.AddOne( this.x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat160.Create();
            SecP160R1Field.Subtract( this.x, ((SecP160R1FieldElement)b).x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat160.Create();
            SecP160R1Field.Multiply( this.x, ((SecP160R1FieldElement)b).x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat160.Create();
            Mod.Invert( SecP160R1Field.P, ((SecP160R1FieldElement)b).x, numArray );
            SecP160R1Field.Multiply( numArray, this.x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat160.Create();
            SecP160R1Field.Negate( this.x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat160.Create();
            SecP160R1Field.Square( this.x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat160.Create();
            Mod.Invert( SecP160R1Field.P, this.x, numArray );
            return new SecP160R1FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat160.IsZero( x ) || Nat160.IsOne( x ))
                return this;
            uint[] numArray1 = Nat160.Create();
            SecP160R1Field.Square( x, numArray1 );
            SecP160R1Field.Multiply( numArray1, x, numArray1 );
            uint[] numArray2 = Nat160.Create();
            SecP160R1Field.SquareN( numArray1, 2, numArray2 );
            SecP160R1Field.Multiply( numArray2, numArray1, numArray2 );
            uint[] numArray3 = numArray1;
            SecP160R1Field.SquareN( numArray2, 4, numArray3 );
            SecP160R1Field.Multiply( numArray3, numArray2, numArray3 );
            uint[] numArray4 = numArray2;
            SecP160R1Field.SquareN( numArray3, 8, numArray4 );
            SecP160R1Field.Multiply( numArray4, numArray3, numArray4 );
            uint[] numArray5 = numArray3;
            SecP160R1Field.SquareN( numArray4, 16, numArray5 );
            SecP160R1Field.Multiply( numArray5, numArray4, numArray5 );
            uint[] numArray6 = numArray4;
            SecP160R1Field.SquareN( numArray5, 32, numArray6 );
            SecP160R1Field.Multiply( numArray6, numArray5, numArray6 );
            uint[] numArray7 = numArray5;
            SecP160R1Field.SquareN( numArray6, 64, numArray7 );
            SecP160R1Field.Multiply( numArray7, numArray6, numArray7 );
            uint[] numArray8 = numArray6;
            SecP160R1Field.Square( numArray7, numArray8 );
            SecP160R1Field.Multiply( numArray8, x, numArray8 );
            uint[] numArray9 = numArray8;
            SecP160R1Field.SquareN( numArray9, 29, numArray9 );
            uint[] numArray10 = numArray7;
            SecP160R1Field.Square( numArray9, numArray10 );
            return !Nat160.Eq( x, numArray10 ) ? null : (ECFieldElement)new SecP160R1FieldElement( numArray9 );
        }

        public override bool Equals( object obj ) => this.Equals( obj as SecP160R1FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecP160R1FieldElement );

        public virtual bool Equals( SecP160R1FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat160.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 5 );
    }
}
