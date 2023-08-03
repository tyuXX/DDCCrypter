// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Djb.Curve25519Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    internal class Curve25519Field
    {
        private const uint P7 = 2147483647;
        private const uint PInv = 19;
        internal static readonly uint[] P = new uint[8]
        {
      4294967277U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
       int.MaxValue
        };
        private static readonly uint[] PExt = new uint[16]
        {
      361U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      4294967277U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      1073741823U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            int num = (int)Nat256.Add( x, y, z );
            if (!Nat256.Gte( z, P ))
                return;
            SubPFrom( z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            int num = (int)Nat.Add( 16, xx, yy, zz );
            if (!Nat.Gte( 16, zz, PExt ))
                return;
            SubPExtFrom( zz );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            int num = (int)Nat.Inc( 8, x, z );
            if (!Nat256.Gte( z, P ))
                return;
            SubPFrom( z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat256.FromBigInteger( x );
            while (Nat256.Gte( numArray, P ))
                Nat256.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 8, x, 0U, z );
            }
            else
            {
                int num2 = (int)Nat256.Add( x, P, z );
                int num3 = (int)Nat.ShiftDownBit( 8, z, 0U );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat256.CreateExt();
            Nat256.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            int num = (int)Nat256.MulAddTo( x, y, zz );
            if (!Nat.Gte( 16, zz, PExt ))
                return;
            SubPExtFrom( zz );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat256.IsZero( x ))
                Nat256.Zero( z );
            else
                Nat256.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            uint c = xx[7];
            int num1 = (int)Nat.ShiftUpBit( 8, xx, 8, c, z, 0 );
            uint num2 = Nat256.MulByWordAddTo( 19U, xx, z ) << 1;
            uint num3 = z[7];
            uint num4 = num2 + ((num3 >> 31) - (c >> 31));
            uint num5 = (num3 & int.MaxValue) + Nat.AddWordTo( 7, num4 * 19U, z );
            z[7] = num5;
            if (num5 < int.MaxValue || !Nat256.Gte( z, P ))
                return;
            SubPFrom( z );
        }

        public static void Reduce27( uint x, uint[] z )
        {
            uint num1 = z[7];
            uint num2 = (x << 1) | (num1 >> 31);
            uint num3 = (num1 & int.MaxValue) + Nat.AddWordTo( 7, num2 * 19U, z );
            z[7] = num3;
            if (num3 < int.MaxValue || !Nat256.Gte( z, P ))
                return;
            SubPFrom( z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat256.CreateExt();
            Nat256.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat256.CreateExt();
            Nat256.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat256.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat256.Sub( x, y, z ) == 0)
                return;
            int num = (int)AddPTo( z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 16, xx, yy, zz ) == 0)
                return;
            int num = (int)AddPExtTo( zz );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            int num = (int)Nat.ShiftUpBit( 8, x, 0U, z );
            if (!Nat256.Gte( z, P ))
                return;
            SubPFrom( z );
        }

        private static uint AddPTo( uint[] z )
        {
            long num1 = z[0] - 19L;
            z[0] = (uint)num1;
            long num2 = num1 >> 32;
            if (num2 != 0L)
                num2 = Nat.DecAt( 7, z, 1 );
            long num3 = num2 + z[7] + 2147483648L;
            z[7] = (uint)num3;
            return (uint)(num3 >> 32);
        }

        private static uint AddPExtTo( uint[] zz )
        {
            long num1 = zz[0] + (long)PExt[0];
            zz[0] = (uint)num1;
            long num2 = num1 >> 32;
            if (num2 != 0L)
                num2 = Nat.IncAt( 8, zz, 1 );
            long num3 = num2 + (zz[8] - 19L);
            zz[8] = (uint)num3;
            long num4 = num3 >> 32;
            if (num4 != 0L)
                num4 = Nat.DecAt( 15, zz, 9 );
            long num5 = num4 + zz[15] + (PExt[15] + 1U);
            zz[15] = (uint)num5;
            return (uint)(num5 >> 32);
        }

        private static int SubPFrom( uint[] z )
        {
            long num1 = z[0] + 19L;
            z[0] = (uint)num1;
            long num2 = num1 >> 32;
            if (num2 != 0L)
                num2 = Nat.IncAt( 7, z, 1 );
            long num3 = num2 + (z[7] - 2147483648L);
            z[7] = (uint)num3;
            return (int)(num3 >> 32);
        }

        private static int SubPExtFrom( uint[] zz )
        {
            long num1 = zz[0] - (long)PExt[0];
            zz[0] = (uint)num1;
            long num2 = num1 >> 32;
            if (num2 != 0L)
                num2 = Nat.DecAt( 8, zz, 1 );
            long num3 = num2 + zz[8] + 19L;
            zz[8] = (uint)num3;
            long num4 = num3 >> 32;
            if (num4 != 0L)
                num4 = Nat.IncAt( 15, zz, 9 );
            long num5 = num4 + (zz[15] - (long)(PExt[15] + 1U));
            zz[15] = (uint)num5;
            return (int)(num5 >> 32);
        }
    }
}
