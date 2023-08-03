// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.BigInteger
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.Globalization;

namespace Org.BouncyCastle.Math
{
    [Serializable]
    public class BigInteger
    {
        private const long IMASK = 4294967295;
        private const ulong UIMASK = 4294967295;
        private const int chunk2 = 1;
        private const int chunk8 = 1;
        private const int chunk10 = 19;
        private const int chunk16 = 16;
        private const int BitsPerByte = 8;
        private const int BitsPerInt = 32;
        private const int BytesPerInt = 4;
        internal static readonly int[][] primeLists = new int[64][]
        {
      new int[8]{ 3, 5, 7, 11, 13, 17, 19, 23 },
      new int[5]{ 29, 31, 37, 41, 43 },
      new int[5]{ 47, 53, 59, 61, 67 },
      new int[4]{ 71, 73, 79, 83 },
      new int[4]{ 89, 97, 101, 103 },
      new int[4]{ 107, 109, 113,  sbyte.MaxValue },
      new int[4]{ 131, 137, 139, 149 },
      new int[4]{ 151, 157, 163, 167 },
      new int[4]{ 173, 179, 181, 191 },
      new int[4]{ 193, 197, 199, 211 },
      new int[3]{ 223, 227, 229 },
      new int[3]{ 233, 239, 241 },
      new int[3]{ 251, 257, 263 },
      new int[3]{ 269, 271, 277 },
      new int[3]{ 281, 283, 293 },
      new int[3]{ 307, 311, 313 },
      new int[3]{ 317, 331, 337 },
      new int[3]{ 347, 349, 353 },
      new int[3]{ 359, 367, 373 },
      new int[3]{ 379, 383, 389 },
      new int[3]{ 397, 401, 409 },
      new int[3]{ 419, 421, 431 },
      new int[3]{ 433, 439, 443 },
      new int[3]{ 449, 457, 461 },
      new int[3]{ 463, 467, 479 },
      new int[3]{ 487, 491, 499 },
      new int[3]{ 503, 509, 521 },
      new int[3]{ 523, 541, 547 },
      new int[3]{ 557, 563, 569 },
      new int[3]{ 571, 577, 587 },
      new int[3]{ 593, 599, 601 },
      new int[3]{ 607, 613, 617 },
      new int[3]{ 619, 631, 641 },
      new int[3]{ 643, 647, 653 },
      new int[3]{ 659, 661, 673 },
      new int[3]{ 677, 683, 691 },
      new int[3]{ 701, 709, 719 },
      new int[3]{ 727, 733, 739 },
      new int[3]{ 743, 751, 757 },
      new int[3]{ 761, 769, 773 },
      new int[3]{ 787, 797, 809 },
      new int[3]{ 811, 821, 823 },
      new int[3]{ 827, 829, 839 },
      new int[3]{ 853, 857, 859 },
      new int[3]{ 863, 877, 881 },
      new int[3]{ 883, 887, 907 },
      new int[3]{ 911, 919, 929 },
      new int[3]{ 937, 941, 947 },
      new int[3]{ 953, 967, 971 },
      new int[3]{ 977, 983, 991 },
      new int[3]{ 997, 1009, 1013 },
      new int[3]{ 1019, 1021, 1031 },
      new int[3]{ 1033, 1039, 1049 },
      new int[3]{ 1051, 1061, 1063 },
      new int[3]{ 1069, 1087, 1091 },
      new int[3]{ 1093, 1097, 1103 },
      new int[3]{ 1109, 1117, 1123 },
      new int[3]{ 1129, 1151, 1153 },
      new int[3]{ 1163, 1171, 1181 },
      new int[3]{ 1187, 1193, 1201 },
      new int[3]{ 1213, 1217, 1223 },
      new int[3]{ 1229, 1231, 1237 },
      new int[3]{ 1249, 1259, 1277 },
      new int[3]{ 1279, 1283, 1289 }
        };
        internal static readonly int[] primeProducts;
        private static readonly int[] ZeroMagnitude = new int[0];
        private static readonly byte[] ZeroEncoding = new byte[0];
        private static readonly BigInteger[] SMALL_CONSTANTS = new BigInteger[17];
        public static readonly BigInteger Zero;
        public static readonly BigInteger One;
        public static readonly BigInteger Two;
        public static readonly BigInteger Three;
        public static readonly BigInteger Ten;
        private static readonly byte[] BitLengthTable = new byte[256]
        {
       0,
       1,
       2,
       2,
       3,
       3,
       3,
       3,
       4,
       4,
       4,
       4,
       4,
       4,
       4,
       4,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8
        };
        private static readonly BigInteger radix2;
        private static readonly BigInteger radix2E;
        private static readonly BigInteger radix8;
        private static readonly BigInteger radix8E;
        private static readonly BigInteger radix10;
        private static readonly BigInteger radix10E;
        private static readonly BigInteger radix16;
        private static readonly BigInteger radix16E;
        private static readonly SecureRandom RandomSource = new();
        private static readonly int[] ExpWindowThresholds = new int[8]
        {
      7,
      25,
      81,
      241,
      673,
      1793,
      4609,
      int.MaxValue
        };
        private int[] magnitude;
        private int sign;
        private int nBits = -1;
        private int nBitLength = -1;
        private int mQuote = 0;

        static BigInteger()
        {
            Zero = new BigInteger( 0, ZeroMagnitude, false )
            {
                nBits = 0,
                nBitLength = 0
            };
            SMALL_CONSTANTS[0] = Zero;
            for (uint index = 1; index < SMALL_CONSTANTS.Length; ++index)
                SMALL_CONSTANTS[(int)(IntPtr)index] = CreateUValueOf( index );
            One = SMALL_CONSTANTS[1];
            Two = SMALL_CONSTANTS[2];
            Three = SMALL_CONSTANTS[3];
            Ten = SMALL_CONSTANTS[10];
            radix2 = ValueOf( 2L );
            radix2E = radix2.Pow( 1 );
            radix8 = ValueOf( 8L );
            radix8E = radix8.Pow( 1 );
            radix10 = ValueOf( 10L );
            radix10E = radix10.Pow( 19 );
            radix16 = ValueOf( 16L );
            radix16E = radix16.Pow( 16 );
            primeProducts = new int[primeLists.Length];
            for (int index1 = 0; index1 < primeLists.Length; ++index1)
            {
                int[] primeList = primeLists[index1];
                int num = primeList[0];
                for (int index2 = 1; index2 < primeList.Length; ++index2)
                    num *= primeList[index2];
                primeProducts[index1] = num;
            }
        }

        private static int GetByteLength( int nBits ) => (nBits + 8 - 1) / 8;

        internal static BigInteger Arbitrary( int sizeInBits ) => new( sizeInBits, RandomSource );

        private BigInteger( int signum, int[] mag, bool checkMag )
        {
            if (checkMag)
            {
                int sourceIndex = 0;
                while (sourceIndex < mag.Length && mag[sourceIndex] == 0)
                    ++sourceIndex;
                if (sourceIndex == mag.Length)
                {
                    this.sign = 0;
                    this.magnitude = ZeroMagnitude;
                }
                else
                {
                    this.sign = signum;
                    if (sourceIndex == 0)
                    {
                        this.magnitude = mag;
                    }
                    else
                    {
                        this.magnitude = new int[mag.Length - sourceIndex];
                        Array.Copy( mag, sourceIndex, magnitude, 0, this.magnitude.Length );
                    }
                }
            }
            else
            {
                this.sign = signum;
                this.magnitude = mag;
            }
        }

        public BigInteger( string value )
          : this( value, 10 )
        {
        }

        public BigInteger( string str, int radix )
        {
            if (str.Length == 0)
                throw new FormatException( "Zero length BigInteger" );
            NumberStyles style;
            int length;
            BigInteger bigInteger1;
            BigInteger val;
            switch (radix)
            {
                case 2:
                    style = NumberStyles.Integer;
                    length = 1;
                    bigInteger1 = radix2;
                    val = radix2E;
                    break;
                case 8:
                    style = NumberStyles.Integer;
                    length = 1;
                    bigInteger1 = radix8;
                    val = radix8E;
                    break;
                case 10:
                    style = NumberStyles.Integer;
                    length = 19;
                    bigInteger1 = radix10;
                    val = radix10E;
                    break;
                case 16:
                    style = NumberStyles.AllowHexSpecifier;
                    length = 16;
                    bigInteger1 = radix16;
                    val = radix16E;
                    break;
                default:
                    throw new FormatException( "Only bases 2, 8, 10, or 16 allowed" );
            }
            int num1 = 0;
            this.sign = 1;
            if (str[0] == '-')
            {
                if (str.Length == 1)
                    throw new FormatException( "Zero length BigInteger" );
                this.sign = -1;
                num1 = 1;
            }
            while (num1 < str.Length && int.Parse( str[num1].ToString(), style ) == 0)
                ++num1;
            if (num1 >= str.Length)
            {
                this.sign = 0;
                this.magnitude = ZeroMagnitude;
            }
            else
            {
                BigInteger bigInteger2 = Zero;
                int num2 = num1 + length;
                if (num2 <= str.Length)
                {
                    do
                    {
                        string s = str.Substring( num1, length );
                        ulong num3 = ulong.Parse( s, style );
                        BigInteger uvalueOf = CreateUValueOf( num3 );
                        BigInteger bigInteger3;
                        switch (radix)
                        {
                            case 2:
                                if (num3 >= 2UL)
                                    throw new FormatException( "Bad character in radix 2 string: " + s );
                                bigInteger3 = bigInteger2.ShiftLeft( 1 );
                                break;
                            case 8:
                                if (num3 >= 8UL)
                                    throw new FormatException( "Bad character in radix 8 string: " + s );
                                bigInteger3 = bigInteger2.ShiftLeft( 3 );
                                break;
                            case 16:
                                bigInteger3 = bigInteger2.ShiftLeft( 64 );
                                break;
                            default:
                                bigInteger3 = bigInteger2.Multiply( val );
                                break;
                        }
                        bigInteger2 = bigInteger3.Add( uvalueOf );
                        num1 = num2;
                        num2 += length;
                    }
                    while (num2 <= str.Length);
                }
                if (num1 < str.Length)
                {
                    string s = str.Substring( num1 );
                    BigInteger uvalueOf = CreateUValueOf( ulong.Parse( s, style ) );
                    if (bigInteger2.sign > 0)
                    {
                        switch (radix)
                        {
                            case 2:
                            case 8:
                                bigInteger2 = bigInteger2.Add( uvalueOf );
                                break;
                            case 16:
                                bigInteger2 = bigInteger2.ShiftLeft( s.Length << 2 );
                                goto case 2;
                            default:
                                bigInteger2 = bigInteger2.Multiply( bigInteger1.Pow( s.Length ) );
                                goto case 2;
                        }
                    }
                    else
                        bigInteger2 = uvalueOf;
                }
                this.magnitude = bigInteger2.magnitude;
            }
        }

        public BigInteger( byte[] bytes )
          : this( bytes, 0, bytes.Length )
        {
        }

        public BigInteger( byte[] bytes, int offset, int length )
        {
            if (length == 0)
                throw new FormatException( "Zero length BigInteger" );
            if ((sbyte)bytes[offset] < 0)
            {
                this.sign = -1;
                int num = offset + length;
                int index1 = offset;
                while (index1 < num && bytes[index1] == byte.MaxValue)
                    ++index1;
                if (index1 >= num)
                {
                    this.magnitude = One.magnitude;
                }
                else
                {
                    int length1 = num - index1;
                    byte[] bytes1 = new byte[length1];
                    int index2 = 0;
                    while (index2 < length1)
                        bytes1[index2++] = (byte)~bytes[index1++];
                    while (bytes1[--index2] == byte.MaxValue)
                        bytes1[index2] = 0;
                    byte[] numArray;
                    IntPtr index3;
                    (numArray = bytes1)[(int)(index3 = (IntPtr)index2)] = (byte)(numArray[(int)index3] + 1U);
                    this.magnitude = MakeMagnitude( bytes1, 0, bytes1.Length );
                }
            }
            else
            {
                this.magnitude = MakeMagnitude( bytes, offset, length );
                this.sign = this.magnitude.Length > 0 ? 1 : 0;
            }
        }

        private static int[] MakeMagnitude( byte[] bytes, int offset, int length )
        {
            int num1 = offset + length;
            int index1 = offset;
            while (index1 < num1 && bytes[index1] == 0)
                ++index1;
            if (index1 >= num1)
                return ZeroMagnitude;
            int length1 = (num1 - index1 + 3) / 4;
            int num2 = (num1 - index1) % 4;
            if (num2 == 0)
                num2 = 4;
            if (length1 < 1)
                return ZeroMagnitude;
            int[] numArray = new int[length1];
            int num3 = 0;
            int index2 = 0;
            for (int index3 = index1; index3 < num1; ++index3)
            {
                num3 = (num3 << 8) | (bytes[index3] & byte.MaxValue);
                --num2;
                if (num2 <= 0)
                {
                    numArray[index2] = num3;
                    ++index2;
                    num2 = 4;
                    num3 = 0;
                }
            }
            if (index2 < numArray.Length)
                numArray[index2] = num3;
            return numArray;
        }

        public BigInteger( int sign, byte[] bytes )
          : this( sign, bytes, 0, bytes.Length )
        {
        }

        public BigInteger( int sign, byte[] bytes, int offset, int length )
        {
            if (sign < -1 || sign > 1)
                throw new FormatException( "Invalid sign value" );
            if (sign == 0)
            {
                this.sign = 0;
                this.magnitude = ZeroMagnitude;
            }
            else
            {
                this.magnitude = MakeMagnitude( bytes, offset, length );
                this.sign = this.magnitude.Length < 1 ? 0 : sign;
            }
        }

        public BigInteger( int sizeInBits, Random random )
        {
            if (sizeInBits < 0)
                throw new ArgumentException( "sizeInBits must be non-negative" );
            this.nBits = -1;
            this.nBitLength = -1;
            if (sizeInBits == 0)
            {
                this.sign = 0;
                this.magnitude = ZeroMagnitude;
            }
            else
            {
                int byteLength = GetByteLength( sizeInBits );
                byte[] numArray1 = new byte[byteLength];
                random.NextBytes( numArray1 );
                int num = (8 * byteLength) - sizeInBits;
                byte[] numArray2;
                (numArray2 = numArray1)[0] = (byte)(numArray2[0] & (uint)(byte)((uint)byte.MaxValue >> num));
                this.magnitude = MakeMagnitude( numArray1, 0, numArray1.Length );
                this.sign = this.magnitude.Length < 1 ? 0 : 1;
            }
        }

        public BigInteger( int bitLength, int certainty, Random random )
        {
            if (bitLength < 2)
                throw new ArithmeticException( "bitLength < 2" );
            this.sign = 1;
            this.nBitLength = bitLength;
            if (bitLength == 2)
            {
                this.magnitude = random.Next( 2 ) == 0 ? Two.magnitude : Three.magnitude;
            }
            else
            {
                int byteLength = GetByteLength( bitLength );
                byte[] numArray1 = new byte[byteLength];
                int num1 = (8 * byteLength) - bitLength;
                byte num2 = (byte)((uint)byte.MaxValue >> num1);
                byte num3 = (byte)(1 << (7 - num1));
            label_5:
                random.NextBytes( numArray1 );
                byte[] numArray2;
                (numArray2 = numArray1)[0] = (byte)(numArray2[0] & (uint)num2);
                byte[] numArray3;
                (numArray3 = numArray1)[0] = (byte)(numArray3[0] | (uint)num3);
                byte[] numArray4;
                IntPtr index1;
                (numArray4 = numArray1)[(int)(index1 = (IntPtr)(byteLength - 1))] = (byte)(numArray4[(int)index1] | 1U);
                this.magnitude = MakeMagnitude( numArray1, 0, numArray1.Length );
                this.nBits = -1;
                this.mQuote = 0;
                if (certainty < 1 || this.CheckProbablePrime( certainty, random, true ))
                    return;
                for (int index2 = 1; index2 < this.magnitude.Length - 1; ++index2)
                {
                    this.magnitude[index2] ^= random.Next();
                    if (this.CheckProbablePrime( certainty, random, true ))
                        return;
                }
                goto label_5;
            }
        }

        public BigInteger Abs() => this.sign < 0 ? this.Negate() : this;

        private static int[] AddMagnitudes( int[] a, int[] b )
        {
            int index1 = a.Length - 1;
            int num1 = b.Length - 1;
            long num2 = 0;
            while (num1 >= 0)
            {
                long num3 = num2 + (uint)a[index1] + (uint)b[num1--];
                a[index1--] = (int)num3;
                num2 = num3 >>> 32;
            }
            if (num2 != 0L)
            {
                int[] numArray;
                int index2;
                while (index1 >= 0 && ((numArray = a)[index2 = index1--] = numArray[(int)(IntPtr)index2] + 1) == 0)
                    ;
            }
            return a;
        }

        public BigInteger Add( BigInteger value )
        {
            if (this.sign == 0)
                return value;
            if (this.sign == value.sign)
                return this.AddToMagnitude( value.magnitude );
            if (value.sign == 0)
                return this;
            return value.sign < 0 ? this.Subtract( value.Negate() ) : value.Subtract( this.Negate() );
        }

        private BigInteger AddToMagnitude( int[] magToAdd )
        {
            int[] numArray;
            int[] b;
            if (this.magnitude.Length < magToAdd.Length)
            {
                numArray = magToAdd;
                b = this.magnitude;
            }
            else
            {
                numArray = this.magnitude;
                b = magToAdd;
            }
            uint maxValue = uint.MaxValue;
            if (numArray.Length == b.Length)
                maxValue -= (uint)b[0];
            bool checkMag = (uint)numArray[0] >= maxValue;
            int[] a;
            if (checkMag)
            {
                a = new int[numArray.Length + 1];
                numArray.CopyTo( a, 1 );
            }
            else
                a = (int[])numArray.Clone();
            return new BigInteger( this.sign, AddMagnitudes( a, b ), checkMag );
        }

        public BigInteger And( BigInteger value )
        {
            if (this.sign == 0 || value.sign == 0)
                return Zero;
            int[] numArray1 = this.sign > 0 ? this.magnitude : this.Add( One ).magnitude;
            int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add( One ).magnitude;
            bool flag = this.sign < 0 && value.sign < 0;
            int[] mag = new int[System.Math.Max( numArray1.Length, numArray2.Length )];
            int num1 = mag.Length - numArray1.Length;
            int num2 = mag.Length - numArray2.Length;
            for (int index = 0; index < mag.Length; ++index)
            {
                int num3 = index >= num1 ? numArray1[index - num1] : 0;
                int num4 = index >= num2 ? numArray2[index - num2] : 0;
                if (this.sign < 0)
                    num3 = ~num3;
                if (value.sign < 0)
                    num4 = ~num4;
                mag[index] = num3 & num4;
                if (flag)
                    mag[index] = ~mag[index];
            }
            BigInteger bigInteger = new( 1, mag, true );
            if (flag)
                bigInteger = bigInteger.Not();
            return bigInteger;
        }

        public BigInteger AndNot( BigInteger val ) => this.And( val.Not() );

        public int BitCount
        {
            get
            {
                if (this.nBits == -1)
                {
                    if (this.sign < 0)
                    {
                        this.nBits = this.Not().BitCount;
                    }
                    else
                    {
                        int num = 0;
                        for (int index = 0; index < this.magnitude.Length; ++index)
                            num += BitCnt( this.magnitude[index] );
                        this.nBits = num;
                    }
                }
                return this.nBits;
            }
        }

        public static int BitCnt( int i )
        {
            uint num1 = (uint)i;
            uint num2 = num1 - ((num1 >> 1) & 1431655765U);
            uint num3 = (uint)(((int)num2 & 858993459) + ((int)(num2 >> 2) & 858993459));
            uint num4 = (uint)(((int)num3 + (int)(num3 >> 4)) & 252645135);
            uint num5 = num4 + (num4 >> 8);
            return (int)((num5 + (num5 >> 16)) & 63U);
        }

        private static int CalcBitLength( int sign, int indx, int[] mag )
        {
            for (; indx < mag.Length; ++indx)
            {
                if (mag[indx] != 0)
                {
                    int num1 = 32 * (mag.Length - indx - 1);
                    int w = mag[indx];
                    int num2 = num1 + BitLen( w );
                    if (sign < 0 && (w & -w) == w)
                    {
                        while (++indx < mag.Length)
                        {
                            if (mag[indx] != 0)
                                goto label_8;
                        }
                        --num2;
                    }
                label_8:
                    return num2;
                }
            }
            return 0;
        }

        public int BitLength
        {
            get
            {
                if (this.nBitLength == -1)
                    this.nBitLength = this.sign == 0 ? 0 : CalcBitLength( this.sign, 0, this.magnitude );
                return this.nBitLength;
            }
        }

        internal static int BitLen( int w )
        {
            uint index1 = (uint)w;
            uint index2 = index1 >> 24;
            if (index2 != 0U)
                return 24 + BitLengthTable[(int)(IntPtr)index2];
            uint index3 = index1 >> 16;
            if (index3 != 0U)
                return 16 + BitLengthTable[(int)(IntPtr)index3];
            uint index4 = index1 >> 8;
            return index4 != 0U ? 8 + BitLengthTable[(int)(IntPtr)index4] : BitLengthTable[(int)(IntPtr)index1];
        }

        private bool QuickPow2Check() => this.sign > 0 && this.nBits == 1;

        public int CompareTo( object obj ) => this.CompareTo( (BigInteger)obj );

        private static int CompareTo( int xIndx, int[] x, int yIndx, int[] y )
        {
            while (xIndx != x.Length && x[xIndx] == 0)
                ++xIndx;
            while (yIndx != y.Length && y[yIndx] == 0)
                ++yIndx;
            return CompareNoLeadingZeroes( xIndx, x, yIndx, y );
        }

        private static int CompareNoLeadingZeroes( int xIndx, int[] x, int yIndx, int[] y )
        {
            int num1 = x.Length - y.Length - (xIndx - yIndx);
            if (num1 != 0)
                return num1 >= 0 ? 1 : -1;
            while (xIndx < x.Length)
            {
                uint num2 = (uint)x[xIndx++];
                uint num3 = (uint)y[yIndx++];
                if ((int)num2 != (int)num3)
                    return num2 >= num3 ? 1 : -1;
            }
            return 0;
        }

        public int CompareTo( BigInteger value )
        {
            if (this.sign < value.sign)
                return -1;
            if (this.sign > value.sign)
                return 1;
            return this.sign != 0 ? this.sign * CompareNoLeadingZeroes( 0, this.magnitude, 0, value.magnitude ) : 0;
        }

        private int[] Divide( int[] x, int[] y )
        {
            int index1 = 0;
            while (index1 < x.Length && x[index1] == 0)
                ++index1;
            int index2 = 0;
            while (index2 < y.Length && y[index2] == 0)
                ++index2;
            int num1 = CompareNoLeadingZeroes( index1, x, index2, y );
            int[] a;
            if (num1 > 0)
            {
                int num2 = CalcBitLength( 1, index2, y );
                int num3 = CalcBitLength( 1, index1, x );
                int n1 = num3 - num2;
                int start = 0;
                int index3 = 0;
                int num4 = num2;
                int[] numArray1;
                int[] numArray2;
                if (n1 > 0)
                {
                    numArray1 = new int[(n1 >> 5) + 1];
                    numArray1[0] = 1 << (n1 % 32);
                    numArray2 = ShiftLeft( y, n1 );
                    num4 += n1;
                }
                else
                {
                    numArray1 = new int[1] { 1 };
                    int length = y.Length - index2;
                    numArray2 = new int[length];
                    Array.Copy( y, index2, numArray2, 0, length );
                }
                a = new int[numArray1.Length];
            label_11:
                if (num4 < num3 || CompareNoLeadingZeroes( index1, x, index3, numArray2 ) >= 0)
                {
                    Subtract( index1, x, index3, numArray2 );
                    AddMagnitudes( a, numArray1 );
                    while (x[index1] == 0)
                    {
                        if (++index1 == x.Length)
                            return a;
                    }
                    num3 = (32 * (x.Length - index1 - 1)) + BitLen( x[index1] );
                    if (num3 <= num2)
                    {
                        if (num3 < num2)
                            return a;
                        num1 = CompareNoLeadingZeroes( index1, x, index2, y );
                        if (num1 <= 0)
                            goto label_30;
                    }
                }
                int n2 = num4 - num3;
                if (n2 == 1 && (uint)(numArray2[index3] >>> 1) > (uint)x[index1])
                    ++n2;
                if (n2 < 2)
                {
                    ShiftRightOneInPlace( index3, numArray2 );
                    --num4;
                    ShiftRightOneInPlace( start, numArray1 );
                }
                else
                {
                    ShiftRightInPlace( index3, numArray2, n2 );
                    num4 -= n2;
                    ShiftRightInPlace( start, numArray1, n2 );
                }
                while (numArray2[index3] == 0)
                    ++index3;
                while (numArray1[start] == 0)
                    ++start;
                goto label_11;
            }
            else
                a = new int[1];
            label_30:
            if (num1 == 0)
            {
                AddMagnitudes( a, One.magnitude );
                Array.Clear( x, index1, x.Length - index1 );
            }
            return a;
        }

        public BigInteger Divide( BigInteger val )
        {
            if (val.sign == 0)
                throw new ArithmeticException( "Division by zero error" );
            if (this.sign == 0)
                return Zero;
            if (val.QuickPow2Check())
            {
                BigInteger bigInteger = this.Abs().ShiftRight( val.Abs().BitLength - 1 );
                return val.sign != this.sign ? bigInteger.Negate() : bigInteger;
            }
            int[] x = (int[])this.magnitude.Clone();
            return new BigInteger( this.sign * val.sign, this.Divide( x, val.magnitude ), true );
        }

        public BigInteger[] DivideAndRemainder( BigInteger val )
        {
            if (val.sign == 0)
                throw new ArithmeticException( "Division by zero error" );
            BigInteger[] bigIntegerArray = new BigInteger[2];
            if (this.sign == 0)
            {
                bigIntegerArray[0] = Zero;
                bigIntegerArray[1] = Zero;
            }
            else if (val.QuickPow2Check())
            {
                int n = val.Abs().BitLength - 1;
                BigInteger bigInteger = this.Abs().ShiftRight( n );
                int[] mag = this.LastNBits( n );
                bigIntegerArray[0] = val.sign == this.sign ? bigInteger : bigInteger.Negate();
                bigIntegerArray[1] = new BigInteger( this.sign, mag, true );
            }
            else
            {
                int[] numArray = (int[])this.magnitude.Clone();
                int[] mag = this.Divide( numArray, val.magnitude );
                bigIntegerArray[0] = new BigInteger( this.sign * val.sign, mag, true );
                bigIntegerArray[1] = new BigInteger( this.sign, numArray, true );
            }
            return bigIntegerArray;
        }

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is BigInteger x && this.sign == x.sign && this.IsEqualMagnitude( x );
        }

        private bool IsEqualMagnitude( BigInteger x )
        {
            int[] magnitude = x.magnitude;
            if (this.magnitude.Length != x.magnitude.Length)
                return false;
            for (int index = 0; index < this.magnitude.Length; ++index)
            {
                if (this.magnitude[index] != x.magnitude[index])
                    return false;
            }
            return true;
        }

        public BigInteger Gcd( BigInteger value )
        {
            if (value.sign == 0)
                return this.Abs();
            if (this.sign == 0)
                return value.Abs();
            BigInteger bigInteger1 = this;
            BigInteger bigInteger2;
            for (BigInteger m = value; m.sign != 0; m = bigInteger2)
            {
                bigInteger2 = bigInteger1.Mod( m );
                bigInteger1 = m;
            }
            return bigInteger1;
        }

        public override int GetHashCode()
        {
            int length = this.magnitude.Length;
            if (this.magnitude.Length > 0)
            {
                length ^= this.magnitude[0];
                if (this.magnitude.Length > 1)
                    length ^= this.magnitude[this.magnitude.Length - 1];
            }
            return this.sign >= 0 ? length : ~length;
        }

        private BigInteger Inc()
        {
            if (this.sign == 0)
                return One;
            return this.sign < 0 ? new BigInteger( -1, doSubBigLil( this.magnitude, One.magnitude ), true ) : this.AddToMagnitude( One.magnitude );
        }

        public int IntValue
        {
            get
            {
                if (this.sign == 0)
                    return 0;
                int num = this.magnitude[this.magnitude.Length - 1];
                return this.sign >= 0 ? num : -num;
            }
        }

        public bool IsProbablePrime( int certainty ) => this.IsProbablePrime( certainty, false );

        internal bool IsProbablePrime( int certainty, bool randomlySelected )
        {
            if (certainty <= 0)
                return true;
            BigInteger bigInteger = this.Abs();
            if (!bigInteger.TestBit( 0 ))
                return bigInteger.Equals( Two );
            return !bigInteger.Equals( One ) && bigInteger.CheckProbablePrime( certainty, RandomSource, randomlySelected );
        }

        private bool CheckProbablePrime( int certainty, Random random, bool randomlySelected )
        {
            int num1 = System.Math.Min( this.BitLength - 1, primeLists.Length );
            for (int index = 0; index < num1; ++index)
            {
                int num2 = this.Remainder( primeProducts[index] );
                foreach (int num3 in primeLists[index])
                {
                    if (num2 % num3 == 0)
                        return this.BitLength < 16 && this.IntValue == num3;
                }
            }
            return this.RabinMillerTest( certainty, random, randomlySelected );
        }

        public bool RabinMillerTest( int certainty, Random random ) => this.RabinMillerTest( certainty, random, false );

        internal bool RabinMillerTest( int certainty, Random random, bool randomlySelected )
        {
            int bitLength = this.BitLength;
            int val2 = ((certainty - 1) / 2) + 1;
            if (randomlySelected)
            {
                int val1 = bitLength >= 1024 ? 4 : (bitLength >= 512 ? 8 : (bitLength >= 256 ? 16 : 50));
                val2 = certainty >= 100 ? val2 - 50 + val1 : System.Math.Min( val1, val2 );
            }
            BigInteger bigInteger1 = this;
            int lowestSetBitMaskFirst = bigInteger1.GetLowestSetBitMaskFirst( -2 );
            BigInteger e = bigInteger1.ShiftRight( lowestSetBitMaskFirst );
            BigInteger bigInteger2 = One.ShiftLeft( 32 * bigInteger1.magnitude.Length ).Remainder( bigInteger1 );
            BigInteger x = bigInteger1.Subtract( bigInteger2 );
            do
            {
                BigInteger b1;
                do
                {
                    b1 = new BigInteger( bigInteger1.BitLength, random );
                }
                while (b1.sign == 0 || b1.CompareTo( bigInteger1 ) >= 0 || b1.IsEqualMagnitude( bigInteger2 ) || b1.IsEqualMagnitude( x ));
                BigInteger b2 = ModPowMonty( b1, e, bigInteger1, false );
                if (!b2.Equals( bigInteger2 ))
                {
                    int num = 0;
                    while (!b2.Equals( x ))
                    {
                        if (++num == lowestSetBitMaskFirst)
                            return false;
                        b2 = ModPowMonty( b2, Two, bigInteger1, false );
                        if (b2.Equals( bigInteger2 ))
                            return false;
                    }
                }
            }
            while (--val2 > 0);
            return true;
        }

        public long LongValue
        {
            get
            {
                if (this.sign == 0)
                    return 0;
                int length = this.magnitude.Length;
                long num = this.magnitude[length - 1] & uint.MaxValue;
                if (length > 1)
                    num |= (this.magnitude[length - 2] & uint.MaxValue) << 32;
                return this.sign >= 0 ? num : -num;
            }
        }

        public BigInteger Max( BigInteger value ) => this.CompareTo( value ) <= 0 ? value : this;

        public BigInteger Min( BigInteger value ) => this.CompareTo( value ) >= 0 ? value : this;

        public BigInteger Mod( BigInteger m )
        {
            BigInteger bigInteger = m.sign >= 1 ? this.Remainder( m ) : throw new ArithmeticException( "Modulus must be positive" );
            return bigInteger.sign < 0 ? bigInteger.Add( m ) : bigInteger;
        }

        public BigInteger ModInverse( BigInteger m )
        {
            if (m.sign < 1)
                throw new ArithmeticException( "Modulus must be positive" );
            if (m.QuickPow2Check())
                return this.ModInversePow2( m );
            BigInteger u1Out;
            if (!ExtEuclid( this.Remainder( m ), m, out u1Out ).Equals( One ))
                throw new ArithmeticException( "Numbers not relatively prime." );
            if (u1Out.sign < 0)
                u1Out = u1Out.Add( m );
            return u1Out;
        }

        private BigInteger ModInversePow2( BigInteger m )
        {
            if (!this.TestBit( 0 ))
                throw new ArithmeticException( "Numbers not relatively prime." );
            int num1 = m.BitLength - 1;
            long num2 = ModInverse64( this.LongValue );
            if (num1 < 64)
                num2 &= (1L << num1) - 1L;
            BigInteger bigInteger = ValueOf( num2 );
            if (num1 > 64)
            {
                BigInteger val = this.Remainder( m );
                int num3 = 64;
                do
                {
                    BigInteger n = bigInteger.Multiply( val ).Remainder( m );
                    bigInteger = bigInteger.Multiply( Two.Subtract( n ) ).Remainder( m );
                    num3 <<= 1;
                }
                while (num3 < num1);
            }
            if (bigInteger.sign < 0)
                bigInteger = bigInteger.Add( m );
            return bigInteger;
        }

        private static int ModInverse32( int d )
        {
            int num1 = d + (((d + 1) & 4) << 1);
            int num2 = num1 * (2 - (d * num1));
            int num3 = num2 * (2 - (d * num2));
            return num3 * (2 - (d * num3));
        }

        private static long ModInverse64( long d )
        {
            long num1 = d + (((d + 1L) & 4L) << 1);
            long num2 = num1 * (2L - (d * num1));
            long num3 = num2 * (2L - (d * num2));
            long num4 = num3 * (2L - (d * num3));
            return num4 * (2L - (d * num4));
        }

        private static BigInteger ExtEuclid( BigInteger a, BigInteger b, out BigInteger u1Out )
        {
            BigInteger bigInteger1 = One;
            BigInteger bigInteger2 = Zero;
            BigInteger bigInteger3 = a;
            BigInteger val = b;
            if (val.sign > 0)
            {
                while (true)
                {
                    BigInteger[] bigIntegerArray = bigInteger3.DivideAndRemainder( val );
                    bigInteger3 = val;
                    val = bigIntegerArray[1];
                    BigInteger bigInteger4 = bigInteger1;
                    bigInteger1 = bigInteger2;
                    if (val.sign > 0)
                        bigInteger2 = bigInteger4.Subtract( bigInteger2.Multiply( bigIntegerArray[0] ) );
                    else
                        break;
                }
            }
            u1Out = bigInteger1;
            return bigInteger3;
        }

        private static void ZeroOut( int[] x ) => Array.Clear( x, 0, x.Length );

        public BigInteger ModPow( BigInteger e, BigInteger m )
        {
            if (m.sign < 1)
                throw new ArithmeticException( "Modulus must be positive" );
            if (m.Equals( One ))
                return Zero;
            if (e.sign == 0)
                return One;
            if (this.sign == 0)
                return Zero;
            bool flag = e.sign < 0;
            if (flag)
                e = e.Negate();
            BigInteger b = this.Mod( m );
            if (!e.Equals( One ))
                b = (m.magnitude[m.magnitude.Length - 1] & 1) != 0 ? ModPowMonty( b, e, m, true ) : ModPowBarrett( b, e, m );
            if (flag)
                b = b.ModInverse( m );
            return b;
        }

        private static BigInteger ModPowBarrett( BigInteger b, BigInteger e, BigInteger m )
        {
            int length1 = m.magnitude.Length;
            BigInteger mr = One.ShiftLeft( (length1 + 1) << 5 );
            BigInteger yu = One.ShiftLeft( length1 << 6 ).Divide( m );
            int extraBits = 0;
            int bitLength = e.BitLength;
            while (bitLength > ExpWindowThresholds[extraBits])
                ++extraBits;
            int length2 = 1 << extraBits;
            BigInteger[] bigIntegerArray = new BigInteger[length2];
            bigIntegerArray[0] = b;
            BigInteger val = ReduceBarrett( b.Square(), m, mr, yu );
            for (int index = 1; index < length2; ++index)
                bigIntegerArray[index] = ReduceBarrett( bigIntegerArray[index - 1].Multiply( val ), m, mr, yu );
            int[] windowList = GetWindowList( e.magnitude, extraBits );
            int num1 = windowList[0];
            int num2 = num1 & byte.MaxValue;
            int num3 = num1 >> 8;
            BigInteger bigInteger;
            if (num2 == 1)
            {
                bigInteger = val;
                --num3;
            }
            else
                bigInteger = bigIntegerArray[num2 >> 1];
            int num4 = 1;
            while (true)
            {
                int[] numArray = windowList;
                int index1 = num4++;
                int num5;
                if ((num5 = numArray[index1]) != -1)
                {
                    int index2 = num5 & byte.MaxValue;
                    int num6 = num3 + BitLengthTable[index2];
                    for (int index3 = 0; index3 < num6; ++index3)
                        bigInteger = ReduceBarrett( bigInteger.Square(), m, mr, yu );
                    bigInteger = ReduceBarrett( bigInteger.Multiply( bigIntegerArray[index2 >> 1] ), m, mr, yu );
                    num3 = num5 >> 8;
                }
                else
                    break;
            }
            for (int index = 0; index < num3; ++index)
                bigInteger = ReduceBarrett( bigInteger.Square(), m, mr, yu );
            return bigInteger;
        }

        private static BigInteger ReduceBarrett(
          BigInteger x,
          BigInteger m,
          BigInteger mr,
          BigInteger yu )
        {
            int bitLength1 = x.BitLength;
            int bitLength2 = m.BitLength;
            if (bitLength1 < bitLength2)
                return x;
            if (bitLength1 - bitLength2 > 1)
            {
                int length = m.magnitude.Length;
                BigInteger bigInteger = x.DivideWords( length - 1 ).Multiply( yu ).DivideWords( length + 1 );
                x = x.RemainderWords( length + 1 ).Subtract( bigInteger.Multiply( m ).RemainderWords( length + 1 ) );
                if (x.sign < 0)
                    x = x.Add( mr );
            }
            while (x.CompareTo( m ) >= 0)
                x = x.Subtract( m );
            return x;
        }

        private static BigInteger ModPowMonty( BigInteger b, BigInteger e, BigInteger m, bool convert )
        {
            int length1 = m.magnitude.Length;
            int n = 32 * length1;
            bool smallMontyModulus = m.BitLength + 2 <= n;
            uint mquote = (uint)m.GetMQuote();
            if (convert)
                b = b.ShiftLeft( n ).Remainder( m );
            int[] a = new int[length1 + 1];
            int[] data = b.magnitude;
            if (data.Length < length1)
            {
                int[] numArray = new int[length1];
                data.CopyTo( numArray, length1 - data.Length );
                data = numArray;
            }
            int extraBits = 0;
            if (e.magnitude.Length > 1 || e.BitCount > 2)
            {
                int bitLength = e.BitLength;
                while (bitLength > ExpWindowThresholds[extraBits])
                    ++extraBits;
            }
            int length2 = 1 << extraBits;
            int[][] numArray1 = new int[length2][];
            numArray1[0] = data;
            int[] numArray2 = Arrays.Clone( data );
            SquareMonty( a, numArray2, m.magnitude, mquote, smallMontyModulus );
            for (int index = 1; index < length2; ++index)
            {
                numArray1[index] = Arrays.Clone( numArray1[index - 1] );
                MultiplyMonty( a, numArray1[index], numArray2, m.magnitude, mquote, smallMontyModulus );
            }
            int[] windowList = GetWindowList( e.magnitude, extraBits );
            int num1 = windowList[0];
            int num2 = num1 & byte.MaxValue;
            int num3 = num1 >> 8;
            int[] numArray3;
            if (num2 == 1)
            {
                numArray3 = numArray2;
                --num3;
            }
            else
                numArray3 = Arrays.Clone( numArray1[num2 >> 1] );
            int num4 = 1;
            while (true)
            {
                int[] numArray4 = windowList;
                int index1 = num4++;
                int num5;
                if ((num5 = numArray4[index1]) != -1)
                {
                    int index2 = num5 & byte.MaxValue;
                    int num6 = num3 + BitLengthTable[index2];
                    for (int index3 = 0; index3 < num6; ++index3)
                        SquareMonty( a, numArray3, m.magnitude, mquote, smallMontyModulus );
                    MultiplyMonty( a, numArray3, numArray1[index2 >> 1], m.magnitude, mquote, smallMontyModulus );
                    num3 = num5 >> 8;
                }
                else
                    break;
            }
            for (int index = 0; index < num3; ++index)
                SquareMonty( a, numArray3, m.magnitude, mquote, smallMontyModulus );
            if (convert)
                MontgomeryReduce( numArray3, m.magnitude, mquote );
            else if (smallMontyModulus && CompareTo( 0, numArray3, 0, m.magnitude ) >= 0)
                Subtract( 0, numArray3, 0, m.magnitude );
            return new BigInteger( 1, numArray3, true );
        }

        private static int[] GetWindowList( int[] mag, int extraBits )
        {
            int w = mag[0];
            int num1 = BitLen( w );
            int[] windowList = new int[((((mag.Length - 1) << 5) + num1) / (1 + extraBits)) + 2];
            int num2 = 0;
            int num3 = 33 - num1;
            int num4 = w << num3;
            int mult = 1;
            int num5 = 1 << extraBits;
            int zeroes = 0;
            int index1 = 0;
            while (true)
            {
                for (; num3 < 32; ++num3)
                {
                    if (mult < num5)
                        mult = (mult << 1) | num4 >>> 31;
                    else if (num4 < 0)
                    {
                        windowList[num2++] = CreateWindowEntry( mult, zeroes );
                        mult = 1;
                        zeroes = 0;
                    }
                    else
                        ++zeroes;
                    num4 <<= 1;
                }
                if (++index1 != mag.Length)
                {
                    num4 = mag[index1];
                    num3 = 0;
                }
                else
                    break;
            }
            int[] numArray = windowList;
            int index2 = num2;
            int index3 = index2 + 1;
            int windowEntry = CreateWindowEntry( mult, zeroes );
            numArray[index2] = windowEntry;
            windowList[index3] = -1;
            return windowList;
        }

        private static int CreateWindowEntry( int mult, int zeroes )
        {
            while ((mult & 1) == 0)
            {
                mult >>= 1;
                ++zeroes;
            }
            return mult | (zeroes << 8);
        }

        private static int[] Square( int[] w, int[] x )
        {
            int index1 = w.Length - 1;
            for (int index2 = x.Length - 1; index2 > 0; --index2)
            {
                ulong num1 = (uint)x[index2];
                ulong num2 = (num1 * num1) + (uint)w[index1];
                w[index1] = (int)num2;
                ulong num3 = num2 >> 32;
                for (int index3 = index2 - 1; index3 >= 0; --index3)
                {
                    ulong num4 = num1 * (uint)x[index3];
                    ulong num5 = num3 + ((uint)w[--index1] & (ulong)uint.MaxValue) + ((uint)num4 << 1);
                    w[index1] = (int)num5;
                    num3 = (num5 >> 32) + (num4 >> 31);
                }
                int index4;
                ulong num6 = num3 + (uint)w[index4 = index1 - 1];
                w[index4] = (int)num6;
                int index5;
                if ((index5 = index4 - 1) >= 0)
                    w[index5] = (int)(num6 >> 32);
                index1 = index5 + index2;
            }
            ulong num7 = (uint)x[0];
            ulong num8 = (num7 * num7) + (uint)w[index1];
            w[index1] = (int)num8;
            int index6;
            if ((index6 = index1 - 1) >= 0)
                w[index6] += (int)(num8 >> 32);
            return w;
        }

        private static int[] Multiply( int[] x, int[] y, int[] z )
        {
            int length = z.Length;
            if (length < 1)
                return x;
            int index1 = x.Length - y.Length;
            do
            {
                long num1 = z[--length] & uint.MaxValue;
                long num2 = 0;
                if (num1 != 0L)
                {
                    for (int index2 = y.Length - 1; index2 >= 0; --index2)
                    {
                        long num3 = num2 + (num1 * (y[index2] & uint.MaxValue)) + (x[index1 + index2] & uint.MaxValue);
                        x[index1 + index2] = (int)num3;
                        num2 = num3 >>> 32;
                    }
                }
                --index1;
                if (index1 >= 0)
                    x[index1] = (int)num2;
            }
            while (length > 0);
            return x;
        }

        private int GetMQuote() => this.mQuote != 0 ? this.mQuote : (this.mQuote = ModInverse32( -this.magnitude[this.magnitude.Length - 1] ));

        private static void MontgomeryReduce( int[] x, int[] m, uint mDash )
        {
            int length = m.Length;
            for (int index1 = length - 1; index1 >= 0; --index1)
            {
                uint num1 = (uint)x[length - 1];
                ulong num2 = num1 * mDash;
                ulong num3 = ((num2 * (uint)m[length - 1]) + num1) >> 32;
                for (int index2 = length - 2; index2 >= 0; --index2)
                {
                    ulong num4 = num3 + (num2 * (uint)m[index2]) + (uint)x[index2];
                    x[index2 + 1] = (int)num4;
                    num3 = num4 >> 32;
                }
                x[0] = (int)num3;
            }
            if (CompareTo( 0, x, 0, m ) < 0)
                return;
            Subtract( 0, x, 0, m );
        }

        private static void MultiplyMonty(
          int[] a,
          int[] x,
          int[] y,
          int[] m,
          uint mDash,
          bool smallMontyModulus )
        {
            int length = m.Length;
            if (length == 1)
            {
                x[0] = (int)MultiplyMontyNIsOne( (uint)x[0], (uint)y[0], (uint)m[0], mDash );
            }
            else
            {
                uint num1 = (uint)y[length - 1];
                ulong num2 = (uint)x[length - 1];
                ulong num3 = num2 * num1;
                ulong num4 = (uint)num3 * mDash;
                ulong num5 = num4 * (uint)m[length - 1];
                ulong num6 = ((num3 + (uint)num5) >> 32) + (num5 >> 32);
                for (int index = length - 2; index >= 0; --index)
                {
                    ulong num7 = num2 * (uint)y[index];
                    ulong num8 = num4 * (uint)m[index];
                    ulong num9 = num6 + (num7 & uint.MaxValue) + (uint)num8;
                    a[index + 2] = (int)num9;
                    num6 = (num9 >> 32) + (num7 >> 32) + (num8 >> 32);
                }
                a[1] = (int)num6;
                int num10 = (int)(num6 >> 32);
                for (int index1 = length - 2; index1 >= 0; --index1)
                {
                    uint num11 = (uint)a[length];
                    ulong num12 = (uint)x[index1];
                    ulong num13 = num12 * num1;
                    ulong num14 = (num13 & uint.MaxValue) + num11;
                    ulong num15 = (uint)num14 * mDash;
                    ulong num16 = num15 * (uint)m[length - 1];
                    ulong num17 = ((num14 + (uint)num16) >> 32) + (num13 >> 32) + (num16 >> 32);
                    for (int index2 = length - 2; index2 >= 0; --index2)
                    {
                        ulong num18 = num12 * (uint)y[index2];
                        ulong num19 = num15 * (uint)m[index2];
                        ulong num20 = num17 + (num18 & uint.MaxValue) + (uint)num19 + (uint)a[index2 + 1];
                        a[index2 + 2] = (int)num20;
                        num17 = (num20 >> 32) + (num18 >> 32) + (num19 >> 32);
                    }
                    ulong num21 = num17 + (uint)num10;
                    a[1] = (int)num21;
                    num10 = (int)(num21 >> 32);
                }
                a[0] = num10;
                if (!smallMontyModulus && CompareTo( 0, a, 0, m ) >= 0)
                    Subtract( 0, a, 0, m );
                Array.Copy( a, 1, x, 0, length );
            }
        }

        private static void SquareMonty(
          int[] a,
          int[] x,
          int[] m,
          uint mDash,
          bool smallMontyModulus )
        {
            int length = m.Length;
            if (length == 1)
            {
                uint num = (uint)x[0];
                x[0] = (int)MultiplyMontyNIsOne( num, num, (uint)m[0], mDash );
            }
            else
            {
                ulong num1 = (uint)x[length - 1];
                ulong num2 = num1 * num1;
                ulong num3 = (uint)num2 * mDash;
                ulong num4 = num3 * (uint)m[length - 1];
                ulong num5 = ((num2 + (uint)num4) >> 32) + (num4 >> 32);
                for (int index = length - 2; index >= 0; --index)
                {
                    ulong num6 = num1 * (uint)x[index];
                    ulong num7 = num3 * (uint)m[index];
                    ulong num8 = num5 + (num7 & uint.MaxValue) + ((uint)num6 << 1);
                    a[index + 2] = (int)num8;
                    num5 = (num8 >> 32) + (num6 >> 31) + (num7 >> 32);
                }
                a[1] = (int)num5;
                int num9 = (int)(num5 >> 32);
                for (int index1 = length - 2; index1 >= 0; --index1)
                {
                    uint num10 = (uint)a[length];
                    ulong num11 = num10 * mDash;
                    ulong num12 = ((num11 * (uint)m[length - 1]) + num10) >> 32;
                    for (int index2 = length - 2; index2 > index1; --index2)
                    {
                        ulong num13 = num12 + (num11 * (uint)m[index2]) + (uint)a[index2 + 1];
                        a[index2 + 2] = (int)num13;
                        num12 = num13 >> 32;
                    }
                    ulong num14 = (uint)x[index1];
                    ulong num15 = num14 * num14;
                    ulong num16 = num11 * (uint)m[index1];
                    ulong num17 = num12 + (num15 & uint.MaxValue) + (uint)num16 + (uint)a[index1 + 1];
                    a[index1 + 2] = (int)num17;
                    ulong num18 = (num17 >> 32) + (num15 >> 32) + (num16 >> 32);
                    for (int index3 = index1 - 1; index3 >= 0; --index3)
                    {
                        ulong num19 = num14 * (uint)x[index3];
                        ulong num20 = num11 * (uint)m[index3];
                        ulong num21 = num18 + (num20 & uint.MaxValue) + ((uint)num19 << 1) + (uint)a[index3 + 1];
                        a[index3 + 2] = (int)num21;
                        num18 = (num21 >> 32) + (num19 >> 31) + (num20 >> 32);
                    }
                    ulong num22 = num18 + (uint)num9;
                    a[1] = (int)num22;
                    num9 = (int)(num22 >> 32);
                }
                a[0] = num9;
                if (!smallMontyModulus && CompareTo( 0, a, 0, m ) >= 0)
                    Subtract( 0, a, 0, m );
                Array.Copy( a, 1, x, 0, length );
            }
        }

        private static uint MultiplyMontyNIsOne( uint x, uint y, uint m, uint mDash )
        {
            ulong num1 = x * (ulong)y;
            uint num2 = (uint)num1 * mDash;
            ulong num3 = m;
            ulong num4 = num3 * num2;
            ulong num5 = ((num1 + (uint)num4) >> 32) + (num4 >> 32);
            if (num5 > num3)
                num5 -= num3;
            return (uint)num5;
        }

        public BigInteger Multiply( BigInteger val )
        {
            if (val == this)
                return this.Square();
            if ((this.sign & val.sign) == 0)
                return Zero;
            if (val.QuickPow2Check())
            {
                BigInteger bigInteger = this.ShiftLeft( val.Abs().BitLength - 1 );
                return val.sign <= 0 ? bigInteger.Negate() : bigInteger;
            }
            if (this.QuickPow2Check())
            {
                BigInteger bigInteger = val.ShiftLeft( this.Abs().BitLength - 1 );
                return this.sign <= 0 ? bigInteger.Negate() : bigInteger;
            }
            int[] numArray = new int[this.magnitude.Length + val.magnitude.Length];
            Multiply( numArray, this.magnitude, val.magnitude );
            return new BigInteger( this.sign ^ val.sign ^ 1, numArray, true );
        }

        public BigInteger Square()
        {
            if (this.sign == 0)
                return Zero;
            if (this.QuickPow2Check())
                return this.ShiftLeft( this.Abs().BitLength - 1 );
            int length = this.magnitude.Length << 1;
            if (this.magnitude[0] >>> 16 == 0)
                --length;
            int[] numArray = new int[length];
            Square( numArray, this.magnitude );
            return new BigInteger( 1, numArray, false );
        }

        public BigInteger Negate() => this.sign == 0 ? this : new BigInteger( -this.sign, this.magnitude, false );

        public BigInteger NextProbablePrime()
        {
            if (this.sign < 0)
                throw new ArithmeticException( "Cannot be called on value < 0" );
            if (this.CompareTo( Two ) < 0)
                return Two;
            BigInteger bigInteger = this.Inc().SetBit( 0 );
            while (!bigInteger.CheckProbablePrime( 100, RandomSource, false ))
                bigInteger = bigInteger.Add( Two );
            return bigInteger;
        }

        public BigInteger Not() => this.Inc().Negate();

        public BigInteger Pow( int exp )
        {
            if (exp <= 0)
            {
                if (exp < 0)
                    throw new ArithmeticException( "Negative exponent" );
                return One;
            }
            if (this.sign == 0)
                return this;
            if (this.QuickPow2Check())
            {
                long n = exp * (long)(this.BitLength - 1);
                return n <= int.MaxValue ? One.ShiftLeft( (int)n ) : throw new ArithmeticException( "Result too large" );
            }
            BigInteger bigInteger = One;
            BigInteger val = this;
            while (true)
            {
                if ((exp & 1) == 1)
                    bigInteger = bigInteger.Multiply( val );
                exp >>= 1;
                if (exp != 0)
                    val = val.Multiply( val );
                else
                    break;
            }
            return bigInteger;
        }

        public static BigInteger ProbablePrime( int bitLength, Random random ) => new( bitLength, 100, random );

        private int Remainder( int m )
        {
            long num1 = 0;
            for (int index = 0; index < this.magnitude.Length; ++index)
            {
                long num2 = (uint)this.magnitude[index];
                num1 = ((num1 << 32) | num2) % m;
            }
            return (int)num1;
        }

        private static int[] Remainder( int[] x, int[] y )
        {
            int index1 = 0;
            while (index1 < x.Length && x[index1] == 0)
                ++index1;
            int index2 = 0;
            while (index2 < y.Length && y[index2] == 0)
                ++index2;
            int num1 = CompareNoLeadingZeroes( index1, x, index2, y );
            if (num1 > 0)
            {
                int num2 = CalcBitLength( 1, index2, y );
                int num3 = CalcBitLength( 1, index1, x );
                int n1 = num3 - num2;
                int index3 = 0;
                int num4 = num2;
                int[] numArray;
                if (n1 > 0)
                {
                    numArray = ShiftLeft( y, n1 );
                    num4 += n1;
                }
                else
                {
                    int length = y.Length - index2;
                    numArray = new int[length];
                    Array.Copy( y, index2, numArray, 0, length );
                }
            label_10:
                if (num4 < num3 || CompareNoLeadingZeroes( index1, x, index3, numArray ) >= 0)
                {
                    Subtract( index1, x, index3, numArray );
                    while (x[index1] == 0)
                    {
                        if (++index1 == x.Length)
                            return x;
                    }
                    num3 = (32 * (x.Length - index1 - 1)) + BitLen( x[index1] );
                    if (num3 <= num2)
                    {
                        if (num3 < num2)
                            return x;
                        num1 = CompareNoLeadingZeroes( index1, x, index2, y );
                        if (num1 <= 0)
                            goto label_26;
                    }
                }
                int n2 = num4 - num3;
                if (n2 == 1 && (uint)(numArray[index3] >>> 1) > (uint)x[index1])
                    ++n2;
                if (n2 < 2)
                {
                    ShiftRightOneInPlace( index3, numArray );
                    --num4;
                }
                else
                {
                    ShiftRightInPlace( index3, numArray, n2 );
                    num4 -= n2;
                }
                while (numArray[index3] == 0)
                    ++index3;
                goto label_10;
            }
        label_26:
            if (num1 == 0)
                Array.Clear( x, index1, x.Length - index1 );
            return x;
        }

        public BigInteger Remainder( BigInteger n )
        {
            if (n.sign == 0)
                throw new ArithmeticException( "Division by zero error" );
            if (this.sign == 0)
                return Zero;
            if (n.magnitude.Length == 1)
            {
                int m = n.magnitude[0];
                if (m > 0)
                {
                    if (m == 1)
                        return Zero;
                    int num = this.Remainder( m );
                    if (num == 0)
                        return Zero;
                    return new BigInteger( this.sign, new int[1] { num }, false );
                }
            }
            return CompareNoLeadingZeroes( 0, this.magnitude, 0, n.magnitude ) < 0 ? this : new BigInteger( this.sign, !n.QuickPow2Check() ? Remainder( (int[])this.magnitude.Clone(), n.magnitude ) : this.LastNBits( n.Abs().BitLength - 1 ), true );
        }

        private int[] LastNBits( int n )
        {
            if (n < 1)
                return ZeroMagnitude;
            int length = System.Math.Min( (n + 32 - 1) / 32, this.magnitude.Length );
            int[] destinationArray = new int[length];
            Array.Copy( magnitude, this.magnitude.Length - length, destinationArray, 0, length );
            int num = (length << 5) - n;
            if (num > 0)
            {
                int[] numArray;
                (numArray = destinationArray)[0] = numArray[0] & (int)(uint.MaxValue >> num);
            }
            return destinationArray;
        }

        private BigInteger DivideWords( int w )
        {
            int length = this.magnitude.Length;
            if (w >= length)
                return Zero;
            int[] numArray = new int[length - w];
            Array.Copy( magnitude, 0, numArray, 0, length - w );
            return new BigInteger( this.sign, numArray, false );
        }

        private BigInteger RemainderWords( int w )
        {
            int length = this.magnitude.Length;
            if (w >= length)
                return this;
            int[] numArray = new int[w];
            Array.Copy( magnitude, length - w, numArray, 0, w );
            return new BigInteger( this.sign, numArray, false );
        }

        private static int[] ShiftLeft( int[] mag, int n )
        {
            int num1 = n >>> 5;
            int num2 = n & 31;
            int length = mag.Length;
            int[] numArray;
            if (num2 == 0)
            {
                numArray = new int[length + num1];
                mag.CopyTo( numArray, 0 );
            }
            else
            {
                int index1 = 0;
                int num3 = 32 - num2;
                int num4 = mag[0] >>> num3;
                if (num4 != 0)
                {
                    numArray = new int[length + num1 + 1];
                    numArray[index1++] = num4;
                }
                else
                    numArray = new int[length + num1];
                int num5 = mag[0];
                for (int index2 = 0; index2 < length - 1; ++index2)
                {
                    int num6 = mag[index2 + 1];
                    numArray[index1++] = (num5 << num2) | num6 >>> num3;
                    num5 = num6;
                }
                numArray[index1] = mag[length - 1] << num2;
            }
            return numArray;
        }

        private static int ShiftLeftOneInPlace( int[] x, int carry )
        {
            int length = x.Length;
            while (--length >= 0)
            {
                uint num = (uint)x[length];
                x[length] = ((int)num << 1) | carry;
                carry = (int)(num >> 31);
            }
            return carry;
        }

        public BigInteger ShiftLeft( int n )
        {
            if (this.sign == 0 || this.magnitude.Length == 0)
                return Zero;
            if (n == 0)
                return this;
            if (n < 0)
                return this.ShiftRight( -n );
            BigInteger bigInteger = new( this.sign, ShiftLeft( this.magnitude, n ), true );
            if (this.nBits != -1)
                bigInteger.nBits = this.sign > 0 ? this.nBits : this.nBits + n;
            if (this.nBitLength != -1)
                bigInteger.nBitLength = this.nBitLength + n;
            return bigInteger;
        }

        private static void ShiftRightInPlace( int start, int[] mag, int n )
        {
            int index1 = (n >>> 5) + start;
            int num1 = n & 31;
            int index2 = mag.Length - 1;
            if (index1 != start)
            {
                int num2 = index1 - start;
                for (int index3 = index2; index3 >= index1; --index3)
                    mag[index3] = mag[index3 - num2];
                for (int index4 = index1 - 1; index4 >= start; --index4)
                    mag[index4] = 0;
            }
            if (num1 == 0)
                return;
            int num3 = 32 - num1;
            int num4 = mag[index2];
            for (int index5 = index2; index5 > index1; --index5)
            {
                int num5 = mag[index5 - 1];
                mag[index5] = num4 >>> num1 | (num5 << num3);
                num4 = num5;
            }
            mag[index1] = mag[index1] >>> num1;
        }

        private static void ShiftRightOneInPlace( int start, int[] mag )
        {
            int length = mag.Length;
            int num1 = mag[length - 1];
            while (--length > start)
            {
                int num2 = mag[length - 1];
                mag[length] = num1 >>> 1 | (num2 << 31);
                num1 = num2;
            }
            mag[start] = mag[start] >>> 1;
        }

        public BigInteger ShiftRight( int n )
        {
            if (n == 0)
                return this;
            if (n < 0)
                return this.ShiftLeft( -n );
            if (n >= this.BitLength)
                return this.sign >= 0 ? Zero : One.Negate();
            int length = (this.BitLength - n + 31) >> 5;
            int[] numArray = new int[length];
            int num1 = n >> 5;
            int num2 = n & 31;
            if (num2 == 0)
            {
                Array.Copy( magnitude, 0, numArray, 0, numArray.Length );
            }
            else
            {
                int num3 = 32 - num2;
                int index1 = this.magnitude.Length - 1 - num1;
                for (int index2 = length - 1; index2 >= 0; --index2)
                {
                    numArray[index2] = this.magnitude[index1--] >>> num2;
                    if (index1 >= 0)
                        numArray[index2] |= this.magnitude[index1] << num3;
                }
            }
            return new BigInteger( this.sign, numArray, false );
        }

        public int SignValue => this.sign;

        private static int[] Subtract( int xStart, int[] x, int yStart, int[] y )
        {
            int index1 = x.Length;
            int length = y.Length;
            int num1 = 0;
            do
            {
                long num2 = (x[--index1] & uint.MaxValue) - (y[--length] & uint.MaxValue) + num1;
                x[index1] = (int)num2;
                num1 = (int)(num2 >> 63);
            }
            while (length > yStart);
            if (num1 != 0)
            {
                int num3;
                do
                {
                    int[] numArray1;
                    int[] numArray2 = numArray1 = x;
                    int index2;
                    index1 = index2 = index1 - 1;
                    int index3 = index2;
                    int num4;
                    num3 = num4 = numArray2[(int)(IntPtr)index3] - 1;
                    numArray1[index2] = num4;
                }
                while (num3 == -1);
            }
            return x;
        }

        public BigInteger Subtract( BigInteger n )
        {
            if (n.sign == 0)
                return this;
            if (this.sign == 0)
                return n.Negate();
            if (this.sign != n.sign)
                return this.Add( n.Negate() );
            int num = CompareNoLeadingZeroes( 0, this.magnitude, 0, n.magnitude );
            if (num == 0)
                return Zero;
            BigInteger bigInteger1;
            BigInteger bigInteger2;
            if (num < 0)
            {
                bigInteger1 = n;
                bigInteger2 = this;
            }
            else
            {
                bigInteger1 = this;
                bigInteger2 = n;
            }
            return new BigInteger( this.sign * num, doSubBigLil( bigInteger1.magnitude, bigInteger2.magnitude ), true );
        }

        private static int[] doSubBigLil( int[] bigMag, int[] lilMag ) => Subtract( 0, (int[])bigMag.Clone(), 0, lilMag );

        public byte[] ToByteArray() => this.ToByteArray( false );

        public byte[] ToByteArrayUnsigned() => this.ToByteArray( true );

        private byte[] ToByteArray( bool unsigned )
        {
            if (this.sign == 0)
                return !unsigned ? new byte[1] : ZeroEncoding;
            byte[] byteArray = new byte[GetByteLength( !unsigned || this.sign <= 0 ? this.BitLength + 1 : this.BitLength )];
            int length = this.magnitude.Length;
            int num1 = byteArray.Length;
            int num2;
            if (this.sign > 0)
            {
                while (length > 1)
                {
                    uint num3 = (uint)this.magnitude[--length];
                    int num4;
                    byteArray[num4 = num1 - 1] = (byte)num3;
                    int num5;
                    byteArray[num5 = num4 - 1] = (byte)(num3 >> 8);
                    int num6;
                    byteArray[num6 = num5 - 1] = (byte)(num3 >> 16);
                    byteArray[num1 = num6 - 1] = (byte)(num3 >> 24);
                }
                uint num7;
                for (num7 = (uint)this.magnitude[0]; num7 > byte.MaxValue; num7 >>= 8)
                    byteArray[--num1] = (byte)num7;
                byteArray[num2 = num1 - 1] = (byte)num7;
            }
            else
            {
                bool flag = true;
                while (length > 1)
                {
                    uint num8 = (uint)~this.magnitude[--length];
                    if (flag)
                        flag = ++num8 == 0U;
                    int num9;
                    byteArray[num9 = num1 - 1] = (byte)num8;
                    int num10;
                    byteArray[num10 = num9 - 1] = (byte)(num8 >> 8);
                    int num11;
                    byteArray[num11 = num10 - 1] = (byte)(num8 >> 16);
                    byteArray[num1 = num11 - 1] = (byte)(num8 >> 24);
                }
                uint num12 = (uint)this.magnitude[0];
                if (flag)
                    --num12;
                for (; num12 > byte.MaxValue; num12 >>= 8)
                    byteArray[--num1] = (byte)~num12;
                int num13;
                byteArray[num13 = num1 - 1] = (byte)~num12;
                if (num13 > 0)
                    byteArray[num2 = num13 - 1] = byte.MaxValue;
            }
            return byteArray;
        }

        public override string ToString() => this.ToString( 10 );

        public string ToString( int radix )
        {
            switch (radix)
            {
                case 2:
                case 8:
                case 10:
                case 16:
                    if (this.magnitude == null)
                        return "null";
                    if (this.sign == 0)
                        return "0";
                    int index1 = 0;
                    while (index1 < this.magnitude.Length && this.magnitude[index1] == 0)
                        ++index1;
                    if (index1 == this.magnitude.Length)
                        return "0";
                    StringBuilder sb = new();
                    if (this.sign == -1)
                        sb.Append( '-' );
                    switch (radix)
                    {
                        case 2:
                            int index2 = index1;
                            sb.Append( Convert.ToString( this.magnitude[index2], 2 ) );
                            while (++index2 < this.magnitude.Length)
                                AppendZeroExtendedString( sb, Convert.ToString( this.magnitude[index2], 2 ), 32 );
                            break;
                        case 8:
                            int num1 = 1073741823;
                            BigInteger bigInteger1 = this.Abs();
                            int bitLength = bigInteger1.BitLength;
                            IList arrayList1 = Platform.CreateArrayList();
                            for (; bitLength > 30; bitLength -= 30)
                            {
                                arrayList1.Add( Convert.ToString( bigInteger1.IntValue & num1, 8 ) );
                                bigInteger1 = bigInteger1.ShiftRight( 30 );
                            }
                            sb.Append( Convert.ToString( bigInteger1.IntValue, 8 ) );
                            for (int index3 = arrayList1.Count - 1; index3 >= 0; --index3)
                                AppendZeroExtendedString( sb, (string)arrayList1[index3], 10 );
                            break;
                        case 10:
                            BigInteger bigInteger2 = this.Abs();
                            if (bigInteger2.BitLength < 64)
                            {
                                sb.Append( Convert.ToString( bigInteger2.LongValue, radix ) );
                                break;
                            }
                            long num2 = long.MaxValue / radix;
                            long num3 = radix;
                            int minLength = 1;
                            while (num3 <= num2)
                            {
                                num3 *= radix;
                                ++minLength;
                            }
                            BigInteger val = ValueOf( num3 );
                            IList arrayList2 = Platform.CreateArrayList();
                            BigInteger[] bigIntegerArray;
                            for (; bigInteger2.CompareTo( val ) >= 0; bigInteger2 = bigIntegerArray[0])
                            {
                                bigIntegerArray = bigInteger2.DivideAndRemainder( val );
                                arrayList2.Add( Convert.ToString( bigIntegerArray[1].LongValue, radix ) );
                            }
                            sb.Append( Convert.ToString( bigInteger2.LongValue, radix ) );
                            for (int index4 = arrayList2.Count - 1; index4 >= 0; --index4)
                                AppendZeroExtendedString( sb, (string)arrayList2[index4], minLength );
                            break;
                        case 16:
                            int index5 = index1;
                            sb.Append( Convert.ToString( this.magnitude[index5], 16 ) );
                            while (++index5 < this.magnitude.Length)
                                AppendZeroExtendedString( sb, Convert.ToString( this.magnitude[index5], 16 ), 8 );
                            break;
                    }
                    return sb.ToString();
                default:
                    throw new FormatException( "Only bases 2, 8, 10, 16 are allowed" );
            }
        }

        private static void AppendZeroExtendedString( StringBuilder sb, string s, int minLength )
        {
            for (int length = s.Length; length < minLength; ++length)
                sb.Append( '0' );
            sb.Append( s );
        }

        private static BigInteger CreateUValueOf( ulong value )
        {
            int num1 = (int)(value >> 32);
            int num2 = (int)value;
            if (num1 != 0)
                return new BigInteger( 1, new int[2] { num1, num2 }, false );
            if (num2 == 0)
                return Zero;
            BigInteger uvalueOf = new( 1, new int[1]
            {
        num2
            }, false );
            if ((num2 & -num2) == num2)
                uvalueOf.nBits = 1;
            return uvalueOf;
        }

        private static BigInteger CreateValueOf( long value )
        {
            if (value >= 0L)
                return CreateUValueOf( (ulong)value );
            return value == long.MinValue ? CreateValueOf( ~value ).Not() : CreateValueOf( -value ).Negate();
        }

        public static BigInteger ValueOf( long value ) => value >= 0L && value < SMALL_CONSTANTS.Length ? SMALL_CONSTANTS[value] : CreateValueOf( value );

        public int GetLowestSetBit() => this.sign == 0 ? -1 : this.GetLowestSetBitMaskFirst( -1 );

        private int GetLowestSetBitMaskFirst( int firstWordMask )
        {
            int length = this.magnitude.Length;
            int lowestSetBitMaskFirst = 0;
            int num1;
            uint num2 = (uint)(this.magnitude[num1 = length - 1] & firstWordMask);
            while (num2 == 0U)
            {
                num2 = (uint)this.magnitude[--num1];
                lowestSetBitMaskFirst += 32;
            }
            while (((int)num2 & byte.MaxValue) == 0)
            {
                num2 >>= 8;
                lowestSetBitMaskFirst += 8;
            }
            while (((int)num2 & 1) == 0)
            {
                num2 >>= 1;
                ++lowestSetBitMaskFirst;
            }
            return lowestSetBitMaskFirst;
        }

        public bool TestBit( int n )
        {
            if (n < 0)
                throw new ArithmeticException( "Bit position must not be negative" );
            if (this.sign < 0)
                return !this.Not().TestBit( n );
            int num = n / 32;
            return num < this.magnitude.Length && ((this.magnitude[this.magnitude.Length - 1 - num] >> (n % 32)) & 1) > 0;
        }

        public BigInteger Or( BigInteger value )
        {
            if (this.sign == 0)
                return value;
            if (value.sign == 0)
                return this;
            int[] numArray1 = this.sign > 0 ? this.magnitude : this.Add( One ).magnitude;
            int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add( One ).magnitude;
            bool flag = this.sign < 0 || value.sign < 0;
            int[] mag = new int[System.Math.Max( numArray1.Length, numArray2.Length )];
            int num1 = mag.Length - numArray1.Length;
            int num2 = mag.Length - numArray2.Length;
            for (int index = 0; index < mag.Length; ++index)
            {
                int num3 = index >= num1 ? numArray1[index - num1] : 0;
                int num4 = index >= num2 ? numArray2[index - num2] : 0;
                if (this.sign < 0)
                    num3 = ~num3;
                if (value.sign < 0)
                    num4 = ~num4;
                mag[index] = num3 | num4;
                if (flag)
                    mag[index] = ~mag[index];
            }
            BigInteger bigInteger = new( 1, mag, true );
            if (flag)
                bigInteger = bigInteger.Not();
            return bigInteger;
        }

        public BigInteger Xor( BigInteger value )
        {
            if (this.sign == 0)
                return value;
            if (value.sign == 0)
                return this;
            int[] numArray1 = this.sign > 0 ? this.magnitude : this.Add( One ).magnitude;
            int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add( One ).magnitude;
            bool flag = (this.sign < 0 && value.sign >= 0) || (this.sign >= 0 && value.sign < 0);
            int[] mag = new int[System.Math.Max( numArray1.Length, numArray2.Length )];
            int num1 = mag.Length - numArray1.Length;
            int num2 = mag.Length - numArray2.Length;
            for (int index = 0; index < mag.Length; ++index)
            {
                int num3 = index >= num1 ? numArray1[index - num1] : 0;
                int num4 = index >= num2 ? numArray2[index - num2] : 0;
                if (this.sign < 0)
                    num3 = ~num3;
                if (value.sign < 0)
                    num4 = ~num4;
                mag[index] = num3 ^ num4;
                if (flag)
                    mag[index] = ~mag[index];
            }
            BigInteger bigInteger = new( 1, mag, true );
            if (flag)
                bigInteger = bigInteger.Not();
            return bigInteger;
        }

        public BigInteger SetBit( int n )
        {
            if (n < 0)
                throw new ArithmeticException( "Bit address less than zero" );
            if (this.TestBit( n ))
                return this;
            return this.sign > 0 && n < this.BitLength - 1 ? this.FlipExistingBit( n ) : this.Or( One.ShiftLeft( n ) );
        }

        public BigInteger ClearBit( int n )
        {
            if (n < 0)
                throw new ArithmeticException( "Bit address less than zero" );
            if (!this.TestBit( n ))
                return this;
            return this.sign > 0 && n < this.BitLength - 1 ? this.FlipExistingBit( n ) : this.AndNot( One.ShiftLeft( n ) );
        }

        public BigInteger FlipBit( int n )
        {
            if (n < 0)
                throw new ArithmeticException( "Bit address less than zero" );
            return this.sign > 0 && n < this.BitLength - 1 ? this.FlipExistingBit( n ) : this.Xor( One.ShiftLeft( n ) );
        }

        private BigInteger FlipExistingBit( int n )
        {
            int[] mag = (int[])this.magnitude.Clone();
            mag[mag.Length - 1 - (n >> 5)] ^= 1 << n;
            return new BigInteger( this.sign, mag, false );
        }
    }
}
