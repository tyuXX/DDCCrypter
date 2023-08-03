// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP521R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP521R1Field
    {
        private const int P16 = 511;
        internal static readonly uint[] P = new uint[17]
        {
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      511U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            uint num = Nat.Add( 16, x, y, z ) + x[16] + y[16];
            if (num > 511U || (num == 511U && Nat.Eq( 16, z, P )))
                num = (num + Nat.Inc( 16, z )) & 511U;
            z[16] = num;
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            uint num = Nat.Inc( 16, x, z ) + x[16];
            if (num > 511U || (num == 511U && Nat.Eq( 16, z, P )))
                num = (num + Nat.Inc( 16, z )) & 511U;
            z[16] = num;
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat.FromBigInteger( 521, x );
            if (Nat.Eq( 17, numArray, P ))
                Nat.Zero( 17, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            uint c = x[16];
            uint num = Nat.ShiftDownBit( 16, x, c, z );
            z[16] = (c >> 1) | (num >> 23);
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] numArray = Nat.Create( 33 );
            ImplMultiply( x, y, numArray );
            Reduce( numArray, z );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat.IsZero( 17, x ))
                Nat.Zero( 17, z );
            else
                Nat.Sub( 17, P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            uint c = xx[32];
            uint num = (Nat.ShiftDownBits( 16, xx, 16, 9, c, z, 0 ) >> 23) + (c >> 9) + Nat.AddTo( 16, xx, z );
            if (num > 511U || (num == 511U && Nat.Eq( 16, z, P )))
                num = (num + Nat.Inc( 16, z )) & 511U;
            z[16] = num;
        }

        public static void Reduce23( uint[] z )
        {
            uint num1 = z[16];
            uint num2 = Nat.AddWordTo( 16, num1 >> 9, z ) + (num1 & 511U);
            if (num2 > 511U || (num2 == 511U && Nat.Eq( 16, z, P )))
                num2 = (num2 + Nat.Inc( 16, z )) & 511U;
            z[16] = num2;
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] numArray = Nat.Create( 33 );
            ImplSquare( x, numArray );
            Reduce( numArray, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] numArray = Nat.Create( 33 );
            ImplSquare( x, numArray );
            Reduce( numArray, z );
            while (--n > 0)
            {
                ImplSquare( z, numArray );
                Reduce( numArray, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            int num = Nat.Sub( 16, x, y, z ) + ((int)x[16] - (int)y[16]);
            if (num < 0)
                num = (num + Nat.Dec( 16, z )) & 511;
            z[16] = (uint)num;
        }

        public static void Twice( uint[] x, uint[] z )
        {
            uint num1 = x[16];
            uint num2 = Nat.ShiftUpBit( 16, x, num1 << 23, z ) | (num1 << 1);
            z[16] = num2 & 511U;
        }

        protected static void ImplMultiply( uint[] x, uint[] y, uint[] zz )
        {
            Nat512.Mul( x, y, zz );
            uint a = x[16];
            uint b = y[16];
            zz[32] = Nat.Mul31BothAdd( 16, a, y, b, x, zz, 16 ) + (a * b);
        }

        protected static void ImplSquare( uint[] x, uint[] zz )
        {
            Nat512.Square( x, zz );
            uint num = x[16];
            zz[32] = Nat.MulWordAddTo( 16, num << 1, x, 0, zz, 16 ) + (num * num);
        }
    }
}
