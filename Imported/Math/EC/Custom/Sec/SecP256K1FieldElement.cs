// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP256K1FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP256K1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP256K1Curve.q;
        protected internal readonly uint[] x;

        public SecP256K1FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for SecP256K1FieldElement", nameof( x ) );
            this.x = SecP256K1Field.FromBigInteger( x );
        }

        public SecP256K1FieldElement() => this.x = Nat256.Create();

        protected internal SecP256K1FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat256.IsZero( this.x );

        public override bool IsOne => Nat256.IsOne( this.x );

        public override bool TestBitZero() => Nat256.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat256.ToBigInteger( this.x );

        public override string FieldName => "SecP256K1Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            SecP256K1Field.Add( this.x, ((SecP256K1FieldElement)b).x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat256.Create();
            SecP256K1Field.AddOne( this.x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            SecP256K1Field.Subtract( this.x, ((SecP256K1FieldElement)b).x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            SecP256K1Field.Multiply( this.x, ((SecP256K1FieldElement)b).x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            Mod.Invert( SecP256K1Field.P, ((SecP256K1FieldElement)b).x, numArray );
            SecP256K1Field.Multiply( numArray, this.x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat256.Create();
            SecP256K1Field.Negate( this.x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat256.Create();
            SecP256K1Field.Square( this.x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat256.Create();
            Mod.Invert( SecP256K1Field.P, this.x, numArray );
            return new SecP256K1FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat256.IsZero( x ) || Nat256.IsOne( x ))
                return this;
            uint[] numArray1 = Nat256.Create();
            SecP256K1Field.Square( x, numArray1 );
            SecP256K1Field.Multiply( numArray1, x, numArray1 );
            uint[] numArray2 = Nat256.Create();
            SecP256K1Field.Square( numArray1, numArray2 );
            SecP256K1Field.Multiply( numArray2, x, numArray2 );
            uint[] numArray3 = Nat256.Create();
            SecP256K1Field.SquareN( numArray2, 3, numArray3 );
            SecP256K1Field.Multiply( numArray3, numArray2, numArray3 );
            uint[] numArray4 = numArray3;
            SecP256K1Field.SquareN( numArray3, 3, numArray4 );
            SecP256K1Field.Multiply( numArray4, numArray2, numArray4 );
            uint[] numArray5 = numArray4;
            SecP256K1Field.SquareN( numArray4, 2, numArray5 );
            SecP256K1Field.Multiply( numArray5, numArray1, numArray5 );
            uint[] numArray6 = Nat256.Create();
            SecP256K1Field.SquareN( numArray5, 11, numArray6 );
            SecP256K1Field.Multiply( numArray6, numArray5, numArray6 );
            uint[] numArray7 = numArray5;
            SecP256K1Field.SquareN( numArray6, 22, numArray7 );
            SecP256K1Field.Multiply( numArray7, numArray6, numArray7 );
            uint[] numArray8 = Nat256.Create();
            SecP256K1Field.SquareN( numArray7, 44, numArray8 );
            SecP256K1Field.Multiply( numArray8, numArray7, numArray8 );
            uint[] numArray9 = Nat256.Create();
            SecP256K1Field.SquareN( numArray8, 88, numArray9 );
            SecP256K1Field.Multiply( numArray9, numArray8, numArray9 );
            uint[] numArray10 = numArray8;
            SecP256K1Field.SquareN( numArray9, 44, numArray10 );
            SecP256K1Field.Multiply( numArray10, numArray7, numArray10 );
            uint[] numArray11 = numArray7;
            SecP256K1Field.SquareN( numArray10, 3, numArray11 );
            SecP256K1Field.Multiply( numArray11, numArray2, numArray11 );
            uint[] numArray12 = numArray11;
            SecP256K1Field.SquareN( numArray12, 23, numArray12 );
            SecP256K1Field.Multiply( numArray12, numArray6, numArray12 );
            SecP256K1Field.SquareN( numArray12, 6, numArray12 );
            SecP256K1Field.Multiply( numArray12, numArray1, numArray12 );
            SecP256K1Field.SquareN( numArray12, 2, numArray12 );
            uint[] numArray13 = numArray1;
            SecP256K1Field.Square( numArray12, numArray13 );
            return !Nat256.Eq( x, numArray13 ) ? null : (ECFieldElement)new SecP256K1FieldElement( numArray12 );
        }

        public override bool Equals( object obj ) => this.Equals( obj as SecP256K1FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecP256K1FieldElement );

        public virtual bool Equals( SecP256K1FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat256.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 8 );
    }
}
