// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.F2mFieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math.EC
{
    public class F2mFieldElement : ECFieldElement
    {
        public const int Gnb = 1;
        public const int Tpb = 2;
        public const int Ppb = 3;
        private int representation;
        private int m;
        private int[] ks;
        private LongArray x;

        public F2mFieldElement( int m, int k1, int k2, int k3, BigInteger x )
        {
            if (x == null || x.SignValue < 0 || x.BitLength > m)
                throw new ArgumentException( "value invalid in F2m field element", nameof( x ) );
            if (k2 == 0 && k3 == 0)
            {
                this.representation = 2;
                this.ks = new int[1] { k1 };
            }
            else
            {
                if (k2 >= k3)
                    throw new ArgumentException( "k2 must be smaller than k3" );
                if (k2 <= 0)
                    throw new ArgumentException( "k2 must be larger than 0" );
                this.representation = 3;
                this.ks = new int[3] { k1, k2, k3 };
            }
            this.m = m;
            this.x = new LongArray( x );
        }

        public F2mFieldElement( int m, int k, BigInteger x )
          : this( m, k, 0, 0, x )
        {
        }

        private F2mFieldElement( int m, int[] ks, LongArray x )
        {
            this.m = m;
            this.representation = ks.Length == 1 ? 2 : 3;
            this.ks = ks;
            this.x = x;
        }

        public override int BitLength => this.x.Degree();

        public override bool IsOne => this.x.IsOne();

        public override bool IsZero => this.x.IsZero();

        public override bool TestBitZero() => this.x.TestBitZero();

        public override BigInteger ToBigInteger() => this.x.ToBigInteger();

        public override string FieldName => "F2m";

        public override int FieldSize => this.m;

        public static void CheckFieldElements( ECFieldElement a, ECFieldElement b )
        {
            F2mFieldElement f2mFieldElement1 = a is F2mFieldElement && b is F2mFieldElement ? (F2mFieldElement)a : throw new ArgumentException( "Field elements are not both instances of F2mFieldElement" );
            F2mFieldElement f2mFieldElement2 = (F2mFieldElement)b;
            if (f2mFieldElement1.representation != f2mFieldElement2.representation)
                throw new ArgumentException( "One of the F2m field elements has incorrect representation" );
            if (f2mFieldElement1.m != f2mFieldElement2.m || !Arrays.AreEqual( f2mFieldElement1.ks, f2mFieldElement2.ks ))
                throw new ArgumentException( "Field elements are not elements of the same field F2m" );
        }

        public override ECFieldElement Add( ECFieldElement b )
        {
            LongArray x = this.x.Copy();
            F2mFieldElement f2mFieldElement = (F2mFieldElement)b;
            x.AddShiftedByWords( f2mFieldElement.x, 0 );
            return new F2mFieldElement( this.m, this.ks, x );
        }

        public override ECFieldElement AddOne() => new F2mFieldElement( this.m, this.ks, this.x.AddOne() );

        public override ECFieldElement Subtract( ECFieldElement b ) => this.Add( b );

        public override ECFieldElement Multiply( ECFieldElement b ) => new F2mFieldElement( this.m, this.ks, this.x.ModMultiply( ((F2mFieldElement)b).x, this.m, this.ks ) );

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
            LongArray x1 = this.x;
            LongArray x2 = ((F2mFieldElement)b).x;
            LongArray x3 = ((F2mFieldElement)x).x;
            LongArray x4 = ((F2mFieldElement)y).x;
            LongArray x5 = x1.Multiply( x2, this.m, this.ks );
            LongArray other = x3.Multiply( x4, this.m, this.ks );
            if (x5 == x1 || x5 == x2)
                x5 = x5.Copy();
            x5.AddShiftedByWords( other, 0 );
            x5.Reduce( this.m, this.ks );
            return new F2mFieldElement( this.m, this.ks, x5 );
        }

        public override ECFieldElement Divide( ECFieldElement b ) => this.Multiply( b.Invert() );

        public override ECFieldElement Negate() => this;

        public override ECFieldElement Square() => new F2mFieldElement( this.m, this.ks, this.x.ModSquare( this.m, this.ks ) );

        public override ECFieldElement SquareMinusProduct( ECFieldElement x, ECFieldElement y ) => this.SquarePlusProduct( x, y );

        public override ECFieldElement SquarePlusProduct( ECFieldElement x, ECFieldElement y )
        {
            LongArray x1 = this.x;
            LongArray x2 = ((F2mFieldElement)x).x;
            LongArray x3 = ((F2mFieldElement)y).x;
            LongArray x4 = x1.Square( this.m, this.ks );
            LongArray other = x2.Multiply( x3, this.m, this.ks );
            if (x4 == x1)
                x4 = x4.Copy();
            x4.AddShiftedByWords( other, 0 );
            x4.Reduce( this.m, this.ks );
            return new F2mFieldElement( this.m, this.ks, x4 );
        }

        public override ECFieldElement SquarePow( int pow ) => pow >= 1 ? new F2mFieldElement( this.m, this.ks, this.x.ModSquareN( pow, this.m, this.ks ) ) : (ECFieldElement)this;

        public override ECFieldElement Invert() => new F2mFieldElement( this.m, this.ks, this.x.ModInverse( this.m, this.ks ) );

        public override ECFieldElement Sqrt() => !this.x.IsZero() && !this.x.IsOne() ? this.SquarePow( this.m - 1 ) : this;

        public int Representation => this.representation;

        public int M => this.m;

        public int K1 => this.ks[0];

        public int K2 => this.ks.Length < 2 ? 0 : this.ks[1];

        public int K3 => this.ks.Length < 3 ? 0 : this.ks[2];

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is F2mFieldElement other && this.Equals( other );
        }

        public virtual bool Equals( F2mFieldElement other ) => this.m == other.m && this.representation == other.representation && Arrays.AreEqual( this.ks, other.ks ) && this.x.Equals( other.x );

        public override int GetHashCode() => this.x.GetHashCode() ^ this.m ^ Arrays.GetHashCode( this.ks );
    }
}
