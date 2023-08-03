// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP160R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP160R1Field
    {
        private const uint P4 = 4294967295;
        private const uint PExt9 = 4294967295;
        private const uint PInv = 2147483649;
        internal static readonly uint[] P = new uint[5]
        {
       int.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[10]
        {
      1U,
      1073741825U,
      0U,
      0U,
      0U,
      4294967294U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[7]
        {
      uint.MaxValue,
      3221225470U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      1U,
      1U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat160.Add( x, y, z ) == 0U && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.AddWordTo( 5, 2147483649U, z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if ((Nat.Add( 10, xx, yy, zz ) == 0U && (zz[9] != uint.MaxValue || !Nat.Gte( 10, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 10, zz, PExtInv.Length );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 5, x, z ) == 0U && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.AddWordTo( 5, 2147483649U, z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat160.FromBigInteger( x );
            if (numArray[4] == uint.MaxValue && Nat160.Gte( numArray, P ))
                Nat160.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 5, x, 0U, z );
            }
            else
            {
                uint c = Nat160.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 5, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat160.CreateExt();
            Nat160.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            if ((Nat160.MulAddTo( x, y, zz ) == 0U && (zz[9] != uint.MaxValue || !Nat.Gte( 10, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 10, zz, PExtInv.Length );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat160.IsZero( x ))
                Nat160.Zero( z );
            else
                Nat160.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            ulong num1 = xx[5];
            ulong num2 = xx[6];
            ulong num3 = xx[7];
            ulong num4 = xx[8];
            ulong num5 = xx[9];
            ulong num6 = 0UL + (ulong)(xx[0] + (long)num1 + ((long)num1 << 31));
            z[0] = (uint)num6;
            ulong num7 = (num6 >> 32) + (ulong)(xx[1] + (long)num2 + ((long)num2 << 31));
            z[1] = (uint)num7;
            ulong num8 = (num7 >> 32) + (ulong)(xx[2] + (long)num3 + ((long)num3 << 31));
            z[2] = (uint)num8;
            ulong num9 = (num8 >> 32) + (ulong)(xx[3] + (long)num4 + ((long)num4 << 31));
            z[3] = (uint)num9;
            ulong num10 = (num9 >> 32) + (ulong)(xx[4] + (long)num5 + ((long)num5 << 31));
            z[4] = (uint)num10;
            Reduce32( (uint)(num10 >> 32), z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            if ((x == 0U || Nat160.MulWordsAdd( 2147483649U, x, z, 0 ) == 0U) && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.AddWordTo( 5, 2147483649U, z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat160.CreateExt();
            Nat160.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat160.CreateExt();
            Nat160.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat160.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat160.Sub( x, y, z ) == 0)
                return;
            Nat.SubWordFrom( 5, 2147483649U, z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 10, xx, yy, zz ) == 0 || Nat.SubFrom( PExtInv.Length, PExtInv, zz ) == 0)
                return;
            Nat.DecAt( 10, zz, PExtInv.Length );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 5, x, 0U, z ) == 0U && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.AddWordTo( 5, 2147483649U, z );
        }
    }
}
