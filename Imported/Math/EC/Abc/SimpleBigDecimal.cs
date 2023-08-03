// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Abc.SimpleBigDecimal
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Text;

namespace Org.BouncyCastle.Math.EC.Abc
{
    internal class SimpleBigDecimal
    {
        private readonly BigInteger bigInt;
        private readonly int scale;

        public static SimpleBigDecimal GetInstance( BigInteger val, int scale ) => new SimpleBigDecimal( val.ShiftLeft( scale ), scale );

        public SimpleBigDecimal( BigInteger bigInt, int scale )
        {
            if (scale < 0)
                throw new ArgumentException( "scale may not be negative" );
            this.bigInt = bigInt;
            this.scale = scale;
        }

        private SimpleBigDecimal( SimpleBigDecimal limBigDec )
        {
            this.bigInt = limBigDec.bigInt;
            this.scale = limBigDec.scale;
        }

        private void CheckScale( SimpleBigDecimal b )
        {
            if (this.scale != b.scale)
                throw new ArgumentException( "Only SimpleBigDecimal of same scale allowed in arithmetic operations" );
        }

        public SimpleBigDecimal AdjustScale( int newScale )
        {
            if (newScale < 0)
                throw new ArgumentException( "scale may not be negative" );
            return newScale == this.scale ? this : new SimpleBigDecimal( this.bigInt.ShiftLeft( newScale - this.scale ), newScale );
        }

        public SimpleBigDecimal Add( SimpleBigDecimal b )
        {
            this.CheckScale( b );
            return new SimpleBigDecimal( this.bigInt.Add( b.bigInt ), this.scale );
        }

        public SimpleBigDecimal Add( BigInteger b ) => new SimpleBigDecimal( this.bigInt.Add( b.ShiftLeft( this.scale ) ), this.scale );

        public SimpleBigDecimal Negate() => new SimpleBigDecimal( this.bigInt.Negate(), this.scale );

        public SimpleBigDecimal Subtract( SimpleBigDecimal b ) => this.Add( b.Negate() );

        public SimpleBigDecimal Subtract( BigInteger b ) => new SimpleBigDecimal( this.bigInt.Subtract( b.ShiftLeft( this.scale ) ), this.scale );

        public SimpleBigDecimal Multiply( SimpleBigDecimal b )
        {
            this.CheckScale( b );
            return new SimpleBigDecimal( this.bigInt.Multiply( b.bigInt ), this.scale + this.scale );
        }

        public SimpleBigDecimal Multiply( BigInteger b ) => new SimpleBigDecimal( this.bigInt.Multiply( b ), this.scale );

        public SimpleBigDecimal Divide( SimpleBigDecimal b )
        {
            this.CheckScale( b );
            return new SimpleBigDecimal( this.bigInt.ShiftLeft( this.scale ).Divide( b.bigInt ), this.scale );
        }

        public SimpleBigDecimal Divide( BigInteger b ) => new SimpleBigDecimal( this.bigInt.Divide( b ), this.scale );

        public SimpleBigDecimal ShiftLeft( int n ) => new SimpleBigDecimal( this.bigInt.ShiftLeft( n ), this.scale );

        public int CompareTo( SimpleBigDecimal val )
        {
            this.CheckScale( val );
            return this.bigInt.CompareTo( val.bigInt );
        }

        public int CompareTo( BigInteger val ) => this.bigInt.CompareTo( val.ShiftLeft( this.scale ) );

        public BigInteger Floor() => this.bigInt.ShiftRight( this.scale );

        public BigInteger Round() => this.Add( new SimpleBigDecimal( BigInteger.One, 1 ).AdjustScale( this.scale ) ).Floor();

        public int IntValue => this.Floor().IntValue;

        public long LongValue => this.Floor().LongValue;

        public int Scale => this.scale;

        public override string ToString()
        {
            if (this.scale == 0)
                return this.bigInt.ToString();
            BigInteger bigInteger = this.Floor();
            BigInteger n = this.bigInt.Subtract( bigInteger.ShiftLeft( this.scale ) );
            if (this.bigInt.SignValue < 0)
                n = BigInteger.One.ShiftLeft( this.scale ).Subtract( n );
            if (bigInteger.SignValue == -1 && !n.Equals( BigInteger.Zero ))
                bigInteger = bigInteger.Add( BigInteger.One );
            string str1 = bigInteger.ToString();
            char[] chArray = new char[this.scale];
            string str2 = n.ToString( 2 );
            int length = str2.Length;
            int num = this.scale - length;
            for (int index = 0; index < num; ++index)
                chArray[index] = '0';
            for (int index = 0; index < length; ++index)
                chArray[num + index] = str2[index];
            string str3 = new string( chArray );
            StringBuilder stringBuilder = new StringBuilder( str1 );
            stringBuilder.Append( "." );
            stringBuilder.Append( str3 );
            return stringBuilder.ToString();
        }

        public override bool Equals( object obj )
        {
            if (this == obj)
                return true;
            return obj is SimpleBigDecimal simpleBigDecimal && this.bigInt.Equals( simpleBigDecimal.bigInt ) && this.scale == simpleBigDecimal.scale;
        }

        public override int GetHashCode() => this.bigInt.GetHashCode() ^ this.scale;
    }
}
