// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP192R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP192R1Field
    {
        private const uint P5 = 4294967295;
        private const uint PExt11 = 4294967295;
        internal static readonly uint[] P = new uint[6]
        {
      uint.MaxValue,
      uint.MaxValue,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[12]
        {
      1U,
      0U,
      2U,
      0U,
      1U,
      0U,
      4294967294U,
      uint.MaxValue,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[9]
        {
      uint.MaxValue,
      uint.MaxValue,
      4294967293U,
      uint.MaxValue,
      4294967294U,
      uint.MaxValue,
      1U,
      0U,
      2U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat192.Add( x, y, z ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if ((Nat.Add( 12, xx, yy, zz ) == 0U && (zz[11] != uint.MaxValue || !Nat.Gte( 12, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 12, zz, PExtInv.Length );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 6, x, z ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat192.FromBigInteger( x );
            if (numArray[5] == uint.MaxValue && Nat192.Gte( numArray, P ))
                Nat192.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 6, x, 0U, z );
            }
            else
            {
                uint c = Nat192.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 6, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat192.CreateExt();
            Nat192.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            if ((Nat192.MulAddTo( x, y, zz ) == 0U && (zz[11] != uint.MaxValue || !Nat.Gte( 12, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 12, zz, PExtInv.Length );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat192.IsZero( x ))
                Nat192.Zero( z );
            else
                Nat192.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            ulong num1 = xx[6];
            ulong num2 = xx[7];
            ulong num3 = xx[8];
            ulong num4 = xx[9];
            ulong num5 = xx[10];
            ulong num6 = xx[11];
            ulong num7 = num1 + num5;
            ulong num8 = num2 + num6;
            ulong num9 = 0UL + xx[0] + num7;
            uint num10 = (uint)num9;
            ulong num11 = (num9 >> 32) + xx[1] + num8;
            z[1] = (uint)num11;
            ulong num12 = num11 >> 32;
            ulong num13 = num7 + num3;
            ulong num14 = num8 + num4;
            ulong num15 = num12 + xx[2] + num13;
            ulong num16 = (uint)num15;
            ulong num17 = (num15 >> 32) + xx[3] + num14;
            z[3] = (uint)num17;
            ulong num18 = num17 >> 32;
            ulong num19 = num13 - num1;
            ulong num20 = num14 - num2;
            ulong num21 = num18 + xx[4] + num19;
            z[4] = (uint)num21;
            ulong num22 = (num21 >> 32) + xx[5] + num20;
            z[5] = (uint)num22;
            ulong num23 = num22 >> 32;
            ulong num24 = num16 + num23;
            ulong num25 = num23 + num10;
            z[0] = (uint)num25;
            ulong num26 = num25 >> 32;
            if (num26 != 0UL)
            {
                ulong num27 = num26 + z[1];
                z[1] = (uint)num27;
                num24 += num27 >> 32;
            }
            z[2] = (uint)num24;
            if ((num24 >> 32 == 0UL || Nat.IncAt( 6, z, 3 ) == 0U) && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            ulong num1 = 0;
            if (x != 0U)
            {
                ulong num2 = num1 + z[0] + x;
                z[0] = (uint)num2;
                ulong num3 = num2 >> 32;
                if (num3 != 0UL)
                {
                    ulong num4 = num3 + z[1];
                    z[1] = (uint)num4;
                    num3 = num4 >> 32;
                }
                ulong num5 = num3 + z[2] + x;
                z[2] = (uint)num5;
                num1 = num5 >> 32;
            }
            if ((num1 == 0UL || Nat.IncAt( 6, z, 3 ) == 0U) && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat192.CreateExt();
            Nat192.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat192.CreateExt();
            Nat192.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat192.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat192.Sub( x, y, z ) == 0)
                return;
            SubPInvFrom( z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 12, xx, yy, zz ) == 0 || Nat.SubFrom( PExtInv.Length, PExtInv, zz ) == 0)
                return;
            Nat.DecAt( 12, zz, PExtInv.Length );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 6, x, 0U, z ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
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
                num2 = num3 >> 32;
            }
            long num4 = num2 + z[2] + 1L;
            z[2] = (uint)num4;
            if (num4 >> 32 == 0L)
                return;
            int num5 = (int)Nat.IncAt( 6, z, 3 );
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
                num2 = num3 >> 32;
            }
            long num4 = num2 + (z[2] - 1L);
            z[2] = (uint)num4;
            if (num4 >> 32 == 0L)
                return;
            Nat.DecAt( 6, z, 3 );
        }
    }
}
