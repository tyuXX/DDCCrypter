// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP224K1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224K1Field
    {
        private const uint P6 = 4294967295;
        private const uint PExt13 = 4294967295;
        private const uint PInv33 = 6803;
        internal static readonly uint[] P = new uint[7]
        {
      4294960493U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[14]
        {
      46280809U,
      13606U,
      1U,
      0U,
      0U,
      0U,
      0U,
      4294953690U,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[9]
        {
      4248686487U,
      4294953689U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      13605U,
      2U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat224.Add( x, y, z ) == 0U && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 7, 6803U, z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if ((Nat.Add( 14, xx, yy, zz ) == 0U && (zz[13] != uint.MaxValue || !Nat.Gte( 14, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 14, zz, PExtInv.Length );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 7, x, z ) == 0U && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 7, 6803U, z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat224.FromBigInteger( x );
            if (numArray[6] == uint.MaxValue && Nat224.Gte( numArray, P ))
                Nat224.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 7, x, 0U, z );
            }
            else
            {
                uint c = Nat224.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 7, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat224.CreateExt();
            Nat224.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            if ((Nat224.MulAddTo( x, y, zz ) == 0U && (zz[13] != uint.MaxValue || !Nat.Gte( 14, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 14, zz, PExtInv.Length );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat224.IsZero( x ))
                Nat224.Zero( z );
            else
                Nat224.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            if (Nat224.Mul33DWordAdd( 6803U, Nat224.Mul33Add( 6803U, xx, 7, xx, 0, z, 0 ), z, 0 ) == 0U && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 7, 6803U, z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            if ((x == 0U || Nat224.Mul33WordAdd( 6803U, x, z, 0 ) == 0U) && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 7, 6803U, z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat224.CreateExt();
            Nat224.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat224.CreateExt();
            Nat224.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat224.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat224.Sub( x, y, z ) == 0)
                return;
            Nat.Sub33From( 7, 6803U, z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 14, xx, yy, zz ) == 0 || Nat.SubFrom( PExtInv.Length, PExtInv, zz ) == 0)
                return;
            Nat.DecAt( 14, zz, PExtInv.Length );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 7, x, 0U, z ) == 0U && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 7, 6803U, z );
        }
    }
}
