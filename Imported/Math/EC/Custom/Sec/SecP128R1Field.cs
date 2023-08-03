// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP128R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP128R1Field
    {
        private const uint P3 = 4294967293;
        private const uint PExt7 = 4294967292;
        internal static readonly uint[] P = new uint[4]
        {
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      4294967293U
        };
        internal static readonly uint[] PExt = new uint[8]
        {
      1U,
      0U,
      0U,
      4U,
      4294967294U,
      uint.MaxValue,
      3U,
      4294967292U
        };
        private static readonly uint[] PExtInv = new uint[8]
        {
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      4294967291U,
      1U,
      0U,
      4294967292U,
      3U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat128.Add( x, y, z ) == 0U && (z[3] != 4294967293U || !Nat128.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat256.Add( xx, yy, zz ) == 0U && (zz[7] != 4294967292U || !Nat256.Gte( zz, PExt )))
                return;
            int num = (int)Nat.AddTo( PExtInv.Length, PExtInv, zz );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 4, x, z ) == 0U && (z[3] != 4294967293U || !Nat128.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat128.FromBigInteger( x );
            if (numArray[3] == 4294967293U && Nat128.Gte( numArray, P ))
                Nat128.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 4, x, 0U, z );
            }
            else
            {
                uint c = Nat128.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 4, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat128.CreateExt();
            Nat128.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            if (Nat128.MulAddTo( x, y, zz ) == 0U && (zz[7] != 4294967292U || !Nat256.Gte( zz, PExt )))
                return;
            int num = (int)Nat.AddTo( PExtInv.Length, PExtInv, zz );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat128.IsZero( x ))
                Nat128.Zero( z );
            else
                Nat128.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            ulong num1 = xx[0];
            ulong num2 = xx[1];
            ulong num3 = xx[2];
            ulong num4 = xx[3];
            ulong num5 = xx[4];
            ulong num6 = xx[5];
            ulong num7 = xx[6];
            ulong num8 = xx[7];
            ulong num9 = num4 + num8;
            ulong num10 = num7 + (num8 << 1);
            ulong num11 = num3 + num10;
            ulong num12 = num6 + (num10 << 1);
            ulong num13 = num2 + num12;
            ulong num14 = num5 + (num12 << 1);
            ulong num15 = num1 + num14;
            ulong num16 = num9 + (num14 << 1);
            z[0] = (uint)num15;
            ulong num17 = num13 + (num15 >> 32);
            z[1] = (uint)num17;
            ulong num18 = num11 + (num17 >> 32);
            z[2] = (uint)num18;
            ulong num19 = num16 + (num18 >> 32);
            z[3] = (uint)num19;
            Reduce32( (uint)(num19 >> 32), z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            ulong num1;
            for (; x != 0U; x = (uint)(num1 >> 32))
            {
                ulong num2 = x;
                ulong num3 = z[0] + num2;
                z[0] = (uint)num3;
                ulong num4 = num3 >> 32;
                if (num4 != 0UL)
                {
                    ulong num5 = num4 + z[1];
                    z[1] = (uint)num5;
                    ulong num6 = (num5 >> 32) + z[2];
                    z[2] = (uint)num6;
                    num4 = num6 >> 32;
                }
                num1 = num4 + z[3] + (num2 << 1);
                z[3] = (uint)num1;
            }
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat128.CreateExt();
            Nat128.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat128.CreateExt();
            Nat128.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat128.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat128.Sub( x, y, z ) == 0)
                return;
            SubPInvFrom( z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 10, xx, yy, zz ) == 0)
                return;
            Nat.SubFrom( PExtInv.Length, PExtInv, zz );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 4, x, 0U, z ) == 0U && (z[3] != 4294967293U || !Nat128.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        private static void AddPInvTo( uint[] z )
        {
            long num1 = z[0] + 1L;
            z[0] = (uint)num1;
            long num2 = num1 >> 32;
            if (num2 != 0L)
            {
                long num3 = num2 + z[1];
                z[1] = (uint)num3;
                long num4 = (num3 >> 32) + z[2];
                z[2] = (uint)num4;
                num2 = num4 >> 32;
            }
            long num5 = num2 + z[3] + 2L;
            z[3] = (uint)num5;
        }

        private static void SubPInvFrom( uint[] z )
        {
            long num1 = z[0] - 1L;
            z[0] = (uint)num1;
            long num2 = num1 >> 32;
            if (num2 != 0L)
            {
                long num3 = num2 + z[1];
                z[1] = (uint)num3;
                long num4 = (num3 >> 32) + z[2];
                z[2] = (uint)num4;
                num2 = num4 >> 32;
            }
            long num5 = num2 + (z[3] - 2L);
            z[3] = (uint)num5;
        }
    }
}
