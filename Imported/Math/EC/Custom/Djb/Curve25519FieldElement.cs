// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Djb.Curve25519FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    internal class Curve25519FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = Curve25519.q;
        private static readonly uint[] PRECOMP_POW2 = new uint[8]
        {
      1242472624U,
      3303938855U,
      2905597048U,
      792926214U,
      1039914919U,
      726466713U,
      1338105611U,
      730014848U
        };
        protected internal readonly uint[] x;

        public Curve25519FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for Curve25519FieldElement", nameof( x ) );
            this.x = Curve25519Field.FromBigInteger( x );
        }

        public Curve25519FieldElement() => this.x = Nat256.Create();

        protected internal Curve25519FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat256.IsZero( this.x );

        public override bool IsOne => Nat256.IsOne( this.x );

        public override bool TestBitZero() => Nat256.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat256.ToBigInteger( this.x );

        public override string FieldName => "Curve25519Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            Curve25519Field.Add( this.x, ((Curve25519FieldElement)b).x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat256.Create();
            Curve25519Field.AddOne( this.x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            Curve25519Field.Subtract( this.x, ((Curve25519FieldElement)b).x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            Curve25519Field.Multiply( this.x, ((Curve25519FieldElement)b).x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat256.Create();
            Mod.Invert( Curve25519Field.P, ((Curve25519FieldElement)b).x, numArray );
            Curve25519Field.Multiply( numArray, this.x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat256.Create();
            Curve25519Field.Negate( this.x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat256.Create();
            Curve25519Field.Square( this.x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat256.Create();
            Mod.Invert( Curve25519Field.P, this.x, numArray );
            return new Curve25519FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat256.IsZero( x ) || Nat256.IsOne( x ))
                return this;
            uint[] numArray1 = Nat256.Create();
            Curve25519Field.Square( x, numArray1 );
            Curve25519Field.Multiply( numArray1, x, numArray1 );
            uint[] numArray2 = numArray1;
            Curve25519Field.Square( numArray1, numArray2 );
            Curve25519Field.Multiply( numArray2, x, numArray2 );
            uint[] numArray3 = Nat256.Create();
            Curve25519Field.Square( numArray2, numArray3 );
            Curve25519Field.Multiply( numArray3, x, numArray3 );
            uint[] numArray4 = Nat256.Create();
            Curve25519Field.SquareN( numArray3, 3, numArray4 );
            Curve25519Field.Multiply( numArray4, numArray2, numArray4 );
            uint[] numArray5 = numArray2;
            Curve25519Field.SquareN( numArray4, 4, numArray5 );
            Curve25519Field.Multiply( numArray5, numArray3, numArray5 );
            uint[] numArray6 = numArray4;
            Curve25519Field.SquareN( numArray5, 4, numArray6 );
            Curve25519Field.Multiply( numArray6, numArray3, numArray6 );
            uint[] numArray7 = numArray3;
            Curve25519Field.SquareN( numArray6, 15, numArray7 );
            Curve25519Field.Multiply( numArray7, numArray6, numArray7 );
            uint[] numArray8 = numArray6;
            Curve25519Field.SquareN( numArray7, 30, numArray8 );
            Curve25519Field.Multiply( numArray8, numArray7, numArray8 );
            uint[] numArray9 = numArray7;
            Curve25519Field.SquareN( numArray8, 60, numArray9 );
            Curve25519Field.Multiply( numArray9, numArray8, numArray9 );
            uint[] numArray10 = numArray8;
            Curve25519Field.SquareN( numArray9, 11, numArray10 );
            Curve25519Field.Multiply( numArray10, numArray5, numArray10 );
            uint[] numArray11 = numArray5;
            Curve25519Field.SquareN( numArray10, 120, numArray11 );
            Curve25519Field.Multiply( numArray11, numArray9, numArray11 );
            uint[] numArray12 = numArray11;
            Curve25519Field.Square( numArray12, numArray12 );
            uint[] numArray13 = numArray9;
            Curve25519Field.Square( numArray12, numArray13 );
            if (Nat256.Eq( x, numArray13 ))
                return new Curve25519FieldElement( numArray12 );
            Curve25519Field.Multiply( numArray12, PRECOMP_POW2, numArray12 );
            Curve25519Field.Square( numArray12, numArray13 );
            return Nat256.Eq( x, numArray13 ) ? new Curve25519FieldElement( numArray12 ) : (ECFieldElement)null;
        }

        public override bool Equals( object obj ) => this.Equals( obj as Curve25519FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as Curve25519FieldElement );

        public virtual bool Equals( Curve25519FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat256.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 8 );
    }
}
