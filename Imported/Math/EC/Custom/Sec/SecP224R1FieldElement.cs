// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP224R1FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP224R1Curve.q;
        protected internal readonly uint[] x;

        public SecP224R1FieldElement( BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.CompareTo( Q ) >= 0)
                throw new ArgumentException( "value invalid for SecP224R1FieldElement", nameof( x ) );
            this.x = SecP224R1Field.FromBigInteger( x );
        }

        public SecP224R1FieldElement() => this.x = Nat224.Create();

        protected internal SecP224R1FieldElement( uint[] x ) => this.x = x;

        public override bool IsZero => Nat224.IsZero( this.x );

        public override bool IsOne => Nat224.IsOne( this.x );

        public override bool TestBitZero() => Nat224.GetBit( this.x, 0 ) == 1U;

        public override BigInteger ToBigInteger() => Nat224.ToBigInteger( this.x );

        public override string FieldName => "SecP224R1Field";

        public override int FieldSize => Q.BitLength;

        public override ECFieldElement Add( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            SecP224R1Field.Add( this.x, ((SecP224R1FieldElement)b).x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement AddOne()
        {
            uint[] numArray = Nat224.Create();
            SecP224R1Field.AddOne( this.x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Subtract( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            SecP224R1Field.Subtract( this.x, ((SecP224R1FieldElement)b).x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Multiply( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            SecP224R1Field.Multiply( this.x, ((SecP224R1FieldElement)b).x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Divide( ECFieldElement b )
        {
            uint[] numArray = Nat224.Create();
            Mod.Invert( SecP224R1Field.P, ((SecP224R1FieldElement)b).x, numArray );
            SecP224R1Field.Multiply( numArray, this.x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Negate()
        {
            uint[] numArray = Nat224.Create();
            SecP224R1Field.Negate( this.x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Square()
        {
            uint[] numArray = Nat224.Create();
            SecP224R1Field.Square( this.x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Invert()
        {
            uint[] numArray = Nat224.Create();
            Mod.Invert( SecP224R1Field.P, this.x, numArray );
            return new SecP224R1FieldElement( numArray );
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat224.IsZero( x ) || Nat224.IsOne( x ))
                return this;
            uint[] numArray1 = Nat224.Create();
            SecP224R1Field.Negate( x, numArray1 );
            uint[] numArray2 = Mod.Random( SecP224R1Field.P );
            uint[] numArray3 = Nat224.Create();
            if (!IsSquare( x ))
                return null;
            while (!TrySqrt( numArray1, numArray2, numArray3 ))
                SecP224R1Field.AddOne( numArray2, numArray2 );
            SecP224R1Field.Square( numArray3, numArray2 );
            return !Nat224.Eq( x, numArray2 ) ? null : (ECFieldElement)new SecP224R1FieldElement( numArray3 );
        }

        public override bool Equals( object obj ) => this.Equals( obj as SecP224R1FieldElement );

        public override bool Equals( ECFieldElement other ) => this.Equals( other as SecP224R1FieldElement );

        public virtual bool Equals( SecP224R1FieldElement other )
        {
            if (this == other)
                return true;
            return other != null && Nat224.Eq( this.x, other.x );
        }

        public override int GetHashCode() => Q.GetHashCode() ^ Arrays.GetHashCode( this.x, 0, 7 );

        private static bool IsSquare( uint[] x )
        {
            uint[] numArray1 = Nat224.Create();
            uint[] numArray2 = Nat224.Create();
            Nat224.Copy( x, numArray1 );
            for (int index = 0; index < 7; ++index)
            {
                Nat224.Copy( numArray1, numArray2 );
                SecP224R1Field.SquareN( numArray1, 1 << index, numArray1 );
                SecP224R1Field.Multiply( numArray1, numArray2, numArray1 );
            }
            SecP224R1Field.SquareN( numArray1, 95, numArray1 );
            return Nat224.IsOne( numArray1 );
        }

        private static void RM(
          uint[] nc,
          uint[] d0,
          uint[] e0,
          uint[] d1,
          uint[] e1,
          uint[] f1,
          uint[] t )
        {
            SecP224R1Field.Multiply( e1, e0, t );
            SecP224R1Field.Multiply( t, nc, t );
            SecP224R1Field.Multiply( d1, d0, f1 );
            SecP224R1Field.Add( f1, t, f1 );
            SecP224R1Field.Multiply( d1, e0, t );
            Nat224.Copy( f1, d1 );
            SecP224R1Field.Multiply( e1, d0, e1 );
            SecP224R1Field.Add( e1, t, e1 );
            SecP224R1Field.Square( e1, f1 );
            SecP224R1Field.Multiply( f1, nc, f1 );
        }

        private static void RP( uint[] nc, uint[] d1, uint[] e1, uint[] f1, uint[] t )
        {
            Nat224.Copy( nc, f1 );
            uint[] numArray1 = Nat224.Create();
            uint[] numArray2 = Nat224.Create();
            for (int index = 0; index < 7; ++index)
            {
                Nat224.Copy( d1, numArray1 );
                Nat224.Copy( e1, numArray2 );
                int num = 1 << index;
                while (--num >= 0)
                    RS( d1, e1, f1, t );
                RM( nc, numArray1, numArray2, d1, e1, f1, t );
            }
        }

        private static void RS( uint[] d, uint[] e, uint[] f, uint[] t )
        {
            SecP224R1Field.Multiply( e, d, e );
            SecP224R1Field.Twice( e, e );
            SecP224R1Field.Square( d, t );
            SecP224R1Field.Add( f, t, d );
            SecP224R1Field.Multiply( f, t, f );
            SecP224R1Field.Reduce32( Nat.ShiftUpBits( 7, f, 2, 0U ), f );
        }

        private static bool TrySqrt( uint[] nc, uint[] r, uint[] t )
        {
            uint[] numArray1 = Nat224.Create();
            Nat224.Copy( r, numArray1 );
            uint[] numArray2 = Nat224.Create();
            numArray2[0] = 1U;
            uint[] numArray3 = Nat224.Create();
            RP( nc, numArray1, numArray2, numArray3, t );
            uint[] numArray4 = Nat224.Create();
            uint[] numArray5 = Nat224.Create();
            for (int index = 1; index < 96; ++index)
            {
                Nat224.Copy( numArray1, numArray4 );
                Nat224.Copy( numArray2, numArray5 );
                RS( numArray1, numArray2, numArray3, t );
                if (Nat224.IsZero( numArray1 ))
                {
                    Mod.Invert( SecP224R1Field.P, numArray5, t );
                    SecP224R1Field.Multiply( t, numArray4, t );
                    return true;
                }
            }
            return false;
        }
    }
}
