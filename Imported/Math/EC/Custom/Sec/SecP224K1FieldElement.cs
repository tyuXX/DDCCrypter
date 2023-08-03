// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP224K1FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224K1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP224K1Curve.q;
        private static readonly uint[] PRECOMP_POW2 = new uint[7]
        {
      868209154U,
      3707425075U,
      579297866U,
      3280018344U,
      2824165628U,
      514782679U,
      2396984652U
        };
        protected internal readonly uint[] x;

        public SecP224K1FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for SecP224K1FieldElement", nameof( x ) );
            this.x = SecP224K1Field.FromBigInteger( x );
        }

        public SecP224K1FieldElement() => this.x = Nat224.Create();

        protected internal SecP224K1FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat224.IsZero( this.x );

        public override bool IsOne => Nat224.IsOne( this.x );

        public override bool TestBitZero() => Nat224.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat224.ToBigInteger( this.x );

        public override string FieldName => "SecP224K1Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            SecP224K1Field.Add( this.x, ((SecP224K1FieldElement)b).x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat224.Create();
            SecP224K1Field.AddOne( this.x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            SecP224K1Field.Subtract( this.x, ((SecP224K1FieldElement)b).x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            SecP224K1Field.Multiply( this.x, ((SecP224K1FieldElement)b).x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            Mod.Invert( SecP224K1Field.P, ((SecP224K1FieldElement)b).x, numArray );
            SecP224K1Field.Multiply( numArray, this.x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat224.Create();
            SecP224K1Field.Negate( this.x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat224.Create();
            SecP224K1Field.Square( this.x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat224.Create();
            Mod.Invert( SecP224K1Field.P, this.x, numArray );
            return new SecP224K1FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat224.IsZero( x ) || Nat224.IsOne( x ))
                return this;
            uint[] numArray1 = Nat224.Create();
            SecP224K1Field.Square( x, numArray1 );
            SecP224K1Field.Multiply( numArray1, x, numArray1 );
            uint[] numArray2 = numArray1;
            SecP224K1Field.Square( numArray1, numArray2 );
            SecP224K1Field.Multiply( numArray2, x, numArray2 );
            uint[] numArray3 = Nat224.Create();
            SecP224K1Field.Square( numArray2, numArray3 );
            SecP224K1Field.Multiply( numArray3, x, numArray3 );
            uint[] numArray4 = Nat224.Create();
            SecP224K1Field.SquareN( numArray3, 4, numArray4 );
            SecP224K1Field.Multiply( numArray4, numArray3, numArray4 );
            uint[] numArray5 = Nat224.Create();
            SecP224K1Field.SquareN( numArray4, 3, numArray5 );
            SecP224K1Field.Multiply( numArray5, numArray2, numArray5 );
            uint[] numArray6 = numArray5;
            SecP224K1Field.SquareN( numArray5, 8, numArray6 );
            SecP224K1Field.Multiply( numArray6, numArray4, numArray6 );
            uint[] numArray7 = numArray4;
            SecP224K1Field.SquareN( numArray6, 4, numArray7 );
            SecP224K1Field.Multiply( numArray7, numArray3, numArray7 );
            uint[] numArray8 = numArray3;
            SecP224K1Field.SquareN( numArray7, 19, numArray8 );
            SecP224K1Field.Multiply( numArray8, numArray6, numArray8 );
            uint[] numArray9 = Nat224.Create();
            SecP224K1Field.SquareN( numArray8, 42, numArray9 );
            SecP224K1Field.Multiply( numArray9, numArray8, numArray9 );
            uint[] numArray10 = numArray8;
            SecP224K1Field.SquareN( numArray9, 23, numArray10 );
            SecP224K1Field.Multiply( numArray10, numArray7, numArray10 );
            uint[] numArray11 = numArray7;
            SecP224K1Field.SquareN( numArray10, 84, numArray11 );
            SecP224K1Field.Multiply( numArray11, numArray9, numArray11 );
            uint[] numArray12 = numArray11;
            SecP224K1Field.SquareN( numArray12, 20, numArray12 );
            SecP224K1Field.Multiply( numArray12, numArray6, numArray12 );
            SecP224K1Field.SquareN( numArray12, 3, numArray12 );
            SecP224K1Field.Multiply( numArray12, x, numArray12 );
            SecP224K1Field.SquareN( numArray12, 2, numArray12 );
            SecP224K1Field.Multiply( numArray12, x, numArray12 );
            SecP224K1Field.SquareN( numArray12, 4, numArray12 );
            SecP224K1Field.Multiply( numArray12, numArray2, numArray12 );
            SecP224K1Field.Square( numArray12, numArray12 );
            uint[] numArray13 = numArray9;
            SecP224K1Field.Square( numArray12, numArray13 );
            if (Nat224.Eq( x, numArray13 ))
                return new SecP224K1FieldElement( numArray12 );
            SecP224K1Field.Multiply( numArray12, PRECOMP_POW2, numArray12 );
            SecP224K1Field.Square( numArray12, numArray13 );
            return Nat224.Eq( x, numArray13 ) ? new SecP224K1FieldElement( numArray12 ) : (ECFieldElement)null;
        }

        public override bool Equals( object obj ) => this.Equals( obj as SecP224K1FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecP224K1FieldElement );

        public virtual bool Equals( SecP224K1FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat224.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 7 );
    }
}
