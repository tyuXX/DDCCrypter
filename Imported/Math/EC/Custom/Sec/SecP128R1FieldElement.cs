// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP128R1FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP128R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP128R1Curve.q;
        protected internal readonly uint[] x;

        public SecP128R1FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for SecP128R1FieldElement", nameof( x ) );
            this.x = SecP128R1Field.FromBigInteger( x );
        }

        public SecP128R1FieldElement() => this.x = Nat128.Create();

        protected internal SecP128R1FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat128.IsZero( this.x );

        public override bool IsOne => Nat128.IsOne( this.x );

        public override bool TestBitZero() => Nat128.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat128.ToBigInteger( this.x );

        public override string FieldName => "SecP128R1Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat128.Create();
            SecP128R1Field.Add( this.x, ((SecP128R1FieldElement)b).x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat128.Create();
            SecP128R1Field.AddOne( this.x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat128.Create();
            SecP128R1Field.Subtract( this.x, ((SecP128R1FieldElement)b).x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat128.Create();
            SecP128R1Field.Multiply( this.x, ((SecP128R1FieldElement)b).x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat128.Create();
            Mod.Invert( SecP128R1Field.P, ((SecP128R1FieldElement)b).x, numArray );
            SecP128R1Field.Multiply( numArray, this.x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat128.Create();
            SecP128R1Field.Negate( this.x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat128.Create();
            SecP128R1Field.Square( this.x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat128.Create();
            Mod.Invert( SecP128R1Field.P, this.x, numArray );
            return new SecP128R1FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat128.IsZero( x ) || Nat128.IsOne( x ))
                return this;
            uint[] numArray1 = Nat128.Create();
            SecP128R1Field.Square( x, numArray1 );
            SecP128R1Field.Multiply( numArray1, x, numArray1 );
            uint[] numArray2 = Nat128.Create();
            SecP128R1Field.SquareN( numArray1, 2, numArray2 );
            SecP128R1Field.Multiply( numArray2, numArray1, numArray2 );
            uint[] numArray3 = Nat128.Create();
            SecP128R1Field.SquareN( numArray2, 4, numArray3 );
            SecP128R1Field.Multiply( numArray3, numArray2, numArray3 );
            uint[] numArray4 = numArray2;
            SecP128R1Field.SquareN( numArray3, 2, numArray4 );
            SecP128R1Field.Multiply( numArray4, numArray1, numArray4 );
            uint[] numArray5 = numArray1;
            SecP128R1Field.SquareN( numArray4, 10, numArray5 );
            SecP128R1Field.Multiply( numArray5, numArray4, numArray5 );
            uint[] numArray6 = numArray3;
            SecP128R1Field.SquareN( numArray5, 10, numArray6 );
            SecP128R1Field.Multiply( numArray6, numArray4, numArray6 );
            uint[] numArray7 = numArray4;
            SecP128R1Field.Square( numArray6, numArray7 );
            SecP128R1Field.Multiply( numArray7, x, numArray7 );
            uint[] numArray8 = numArray7;
            SecP128R1Field.SquareN( numArray8, 95, numArray8 );
            uint[] numArray9 = numArray6;
            SecP128R1Field.Square( numArray8, numArray9 );
            return !Nat128.Eq( x, numArray9 ) ? null : (ECFieldElement)new SecP128R1FieldElement( numArray8 );
        }

        public override bool Equals( object obj ) => this.Equals( obj as SecP128R1FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecP128R1FieldElement );

        public virtual bool Equals( SecP128R1FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat128.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 4 );
    }
}
