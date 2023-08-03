// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP160R2Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP160R2Field
    {
        private const uint P4 = 4294967295;
        private const uint PExt9 = 4294967295;
        private const uint PInv33 = 21389;
        internal static readonly uint[] P = new uint[5]
        {
      4294945907U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[10]
        {
      457489321U,
      42778U,
      1U,
      0U,
      0U,
      4294924518U,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[7]
        {
      3837477975U,
      4294924517U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      42777U,
      2U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat160.Add( x, y, z ) == 0U && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 5, 21389U, z );
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
            int num = (int)Nat.Add33To( 5, 21389U, z );
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
            if (Nat160.Mul33DWordAdd( 21389U, Nat160.Mul33Add( 21389U, xx, 5, xx, 0, z, 0 ), z, 0 ) == 0U && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 5, 21389U, z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            if ((x == 0U || Nat160.Mul33WordAdd( 21389U, x, z, 0 ) == 0U) && (z[4] != uint.MaxValue || !Nat160.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 5, 21389U, z );
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
            Nat.Sub33From( 5, 21389U, z );
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
            int num = (int)Nat.Add33To( 5, 21389U, z );
        }
    }
}
