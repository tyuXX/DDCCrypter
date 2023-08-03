// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP192K1FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP192K1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP192K1Curve.q;
        protected internal readonly uint[] x;

        public SecP192K1FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for SecP192K1FieldElement", nameof( x ) );
            this.x = SecP192K1Field.FromBigInteger( x );
        }

        public SecP192K1FieldElement() => this.x = Nat192.Create();

        protected internal SecP192K1FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat192.IsZero( this.x );

        public override bool IsOne => Nat192.IsOne( this.x );

        public override bool TestBitZero() => Nat192.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat192.ToBigInteger( this.x );

        public override string FieldName => "SecP192K1Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat192.Create();
            SecP192K1Field.Add( this.x, ((SecP192K1FieldElement)b).x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat192.Create();
            SecP192K1Field.AddOne( this.x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat192.Create();
            SecP192K1Field.Subtract( this.x, ((SecP192K1FieldElement)b).x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat192.Create();
            SecP192K1Field.Multiply( this.x, ((SecP192K1FieldElement)b).x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat192.Create();
            Mod.Invert( SecP192K1Field.P, ((SecP192K1FieldElement)b).x, numArray );
            SecP192K1Field.Multiply( numArray, this.x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat192.Create();
            SecP192K1Field.Negate( this.x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat192.Create();
            SecP192K1Field.Square( this.x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat192.Create();
            Mod.Invert( SecP192K1Field.P, this.x, numArray );
            return new SecP192K1FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat192.IsZero( x ) || Nat192.IsOne( x ))
                return this;
            uint[] numArray1 = Nat192.Create();
            SecP192K1Field.Square( x, numArray1 );
            SecP192K1Field.Multiply( numArray1, x, numArray1 );
            uint[] numArray2 = Nat192.Create();
            SecP192K1Field.Square( numArray1, numArray2 );
            SecP192K1Field.Multiply( numArray2, x, numArray2 );
            uint[] numArray3 = Nat192.Create();
            SecP192K1Field.SquareN( numArray2, 3, numArray3 );
            SecP192K1Field.Multiply( numArray3, numArray2, numArray3 );
            uint[] numArray4 = numArray3;
            SecP192K1Field.SquareN( numArray3, 2, numArray4 );
            SecP192K1Field.Multiply( numArray4, numArray1, numArray4 );
            uint[] numArray5 = numArray1;
            SecP192K1Field.SquareN( numArray4, 8, numArray5 );
            SecP192K1Field.Multiply( numArray5, numArray4, numArray5 );
            uint[] numArray6 = numArray4;
            SecP192K1Field.SquareN( numArray5, 3, numArray6 );
            SecP192K1Field.Multiply( numArray6, numArray2, numArray6 );
            uint[] numArray7 = Nat192.Create();
            SecP192K1Field.SquareN( numArray6, 16, numArray7 );
            SecP192K1Field.Multiply( numArray7, numArray5, numArray7 );
            uint[] numArray8 = numArray5;
            SecP192K1Field.SquareN( numArray7, 35, numArray8 );
            SecP192K1Field.Multiply( numArray8, numArray7, numArray8 );
            uint[] numArray9 = numArray7;
            SecP192K1Field.SquareN( numArray8, 70, numArray9 );
            SecP192K1Field.Multiply( numArray9, numArray8, numArray9 );
            uint[] numArray10 = numArray8;
            SecP192K1Field.SquareN( numArray9, 19, numArray10 );
            SecP192K1Field.Multiply( numArray10, numArray6, numArray10 );
            uint[] numArray11 = numArray10;
            SecP192K1Field.SquareN( numArray11, 20, numArray11 );
            SecP192K1Field.Multiply( numArray11, numArray6, numArray11 );
            SecP192K1Field.SquareN( numArray11, 4, numArray11 );
            SecP192K1Field.Multiply( numArray11, numArray2, numArray11 );
            SecP192K1Field.SquareN( numArray11, 6, numArray11 );
            SecP192K1Field.Multiply( numArray11, numArray2, numArray11 );
            SecP192K1Field.Square( numArray11, numArray11 );
            uint[] numArray12 = numArray2;
            SecP192K1Field.Square( numArray11, numArray12 );
            return !Nat192.Eq( x, numArray12 ) ? null : (ECFieldElement)new SecP192K1FieldElement( numArray11 );
        }

        public override bool Equals( object obj ) => this.Equals( obj as SecP192K1FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecP192K1FieldElement );

        public virtual bool Equals( SecP192K1FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat192.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 6 );
    }
}
