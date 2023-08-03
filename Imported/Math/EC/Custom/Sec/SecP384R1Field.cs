// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP384R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP384R1Field
    {
        private const uint P11 = 4294967295;
        private const uint PExt23 = 4294967295;
        internal static readonly uint[] P = new uint[12]
        {
      uint.MaxValue,
      0U,
      0U,
      uint.MaxValue,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[24]
        {
      1U,
      4294967294U,
      0U,
      2U,
      0U,
      4294967294U,
      0U,
      2U,
      1U,
      0U,
      0U,
      0U,
      4294967294U,
      1U,
      0U,
      4294967294U,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[17]
        {
      uint.MaxValue,
      1U,
      uint.MaxValue,
      4294967293U,
      uint.MaxValue,
      1U,
      uint.MaxValue,
      4294967293U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      1U,
      4294967294U,
      uint.MaxValue,
      1U,
      2U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat.Add( 12, x, y, z ) == 0U && (z[11] != uint.MaxValue || !Nat.Gte( 12, z, P )))
                return;
            AddPInvTo( z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if ((Nat.Add( 24, xx, yy, zz ) == 0U && (zz[23] != uint.MaxValue || !Nat.Gte( 24, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 24, zz, PExtInv.Length );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 12, x, z ) == 0U && (z[11] != uint.MaxValue || !Nat.Gte( 12, z, P )))
                return;
            AddPInvTo( z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat.FromBigInteger( 384, x );
            if (numArray[11] == uint.MaxValue && Nat.Gte( 12, numArray, P ))
                Nat.SubFrom( 12, P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 12, x, 0U, z );
            }
            else
            {
                uint c = Nat.Add( 12, x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 12, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] numArray = Nat.Create( 24 );
            Nat384.Mul( x, y, numArray );
            Reduce( numArray, z );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat.IsZero( 12, x ))
                Nat.Zero( 12, z );
            else
                Nat.Sub( 12, P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            long num1 = xx[16];
            long num2 = xx[17];
            long num3 = xx[18];
            long num4 = xx[19];
            long num5 = xx[20];
            long num6 = xx[21];
            long num7 = xx[22];
            long num8 = xx[23];
            long num9 = xx[12] + num5 - 1L;
            long num10 = xx[13] + num7;
            long num11 = xx[14] + num7 + num8;
            long num12 = xx[15] + num8;
            long num13 = num2 + num6;
            long num14 = num6 - num8;
            long num15 = num7 - num8;
            long num16 = 0L + xx[0] + num9 + num14;
            z[0] = (uint)num16;
            long num17 = (num16 >> 32) + xx[1] + num8 - num9 + num10;
            z[1] = (uint)num17;
            long num18 = (num17 >> 32) + xx[2] - num6 - num10 + num11;
            z[2] = (uint)num18;
            long num19 = (num18 >> 32) + xx[3] + num9 - num11 + num12 + num14;
            z[3] = (uint)num19;
            long num20 = (num19 >> 32) + xx[4] + num1 + num6 + num9 + num10 - num12 + num14;
            z[4] = (uint)num20;
            long num21 = (num20 >> 32) + xx[5] - num1 + num10 + num11 + num13;
            z[5] = (uint)num21;
            long num22 = (num21 >> 32) + xx[6] + num3 - num2 + num11 + num12;
            z[6] = (uint)num22;
            long num23 = (num22 >> 32) + xx[7] + num1 + num4 - num3 + num12;
            z[7] = (uint)num23;
            long num24 = (num23 >> 32) + (xx[8] + num1 + num2 + num5 - num4);
            z[8] = (uint)num24;
            long num25 = (num24 >> 32) + xx[9] + num3 - num5 + num13;
            z[9] = (uint)num25;
            long num26 = (num25 >> 32) + xx[10] + num3 + num4 - num14 + num15;
            z[10] = (uint)num26;
            long num27 = (num26 >> 32) + (xx[11] + num4 + num5 - num15);
            z[11] = (uint)num27;
            Reduce32( (uint)((ulong)(num27 >> 32) + 1UL), z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            long num1 = 0;
            if (x != 0U)
            {
                long num2 = x;
                long num3 = num1 + z[0] + num2;
                z[0] = (uint)num3;
                long num4 = (num3 >> 32) + (z[1] - num2);
                z[1] = (uint)num4;
                long num5 = num4 >> 32;
                if (num5 != 0L)
                {
                    long num6 = num5 + z[2];
                    z[2] = (uint)num6;
                    num5 = num6 >> 32;
                }
                long num7 = num5 + z[3] + num2;
                z[3] = (uint)num7;
                long num8 = (num7 >> 32) + z[4] + num2;
                z[4] = (uint)num8;
                num1 = num8 >> 32;
            }
            if ((num1 == 0L || Nat.IncAt( 12, z, 5 ) == 0U) && (z[11] != uint.MaxValue || !Nat.Gte( 12, z, P )))
                return;
            AddPInvTo( z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] numArray = Nat.Create( 24 );
            Nat384.Square( x, numArray );
            Reduce( numArray, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] numArray = Nat.Create( 24 );
            Nat384.Square( x, numArray );
            Reduce( numArray, z );
            while (--n > 0)
            {
                Nat384.Square( z, numArray );
                Reduce( numArray, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat.Sub( 12, x, y, z ) == 0)
                return;
            SubPInvFrom( z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 24, xx, yy, zz ) == 0 || Nat.SubFrom( PExtInv.Length, PExtInv, zz ) == 0)
                return;
            Nat.DecAt( 24, zz, PExtInv.Length );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 12, x, 0U, z ) == 0U && (z[11] != uint.MaxValue || !Nat.Gte( 12, z, P )))
                return;
            AddPInvTo( z );
        }

        private static void AddPInvTo( uint[] z )
        {
            long num1 = z[0] + 1L;
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + (z[1] - 1L);
            z[1] = (uint)num2;
            long num3 = num2 >> 32;
            if (num3 != 0L)
            {
                long num4 = num3 + z[2];
                z[2] = (uint)num4;
                num3 = num4 >> 32;
            }
            long num5 = num3 + z[3] + 1L;
            z[3] = (uint)num5;
            long num6 = (num5 >> 32) + z[4] + 1L;
            z[4] = (uint)num6;
            if (num6 >> 32 == 0L)
                return;
            int num7 = (int)Nat.IncAt( 12, z, 5 );
        }

        private static void SubPInvFrom( uint[] z )
        {
            long num1 = z[0] - 1L;
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + z[1] + 1L;
            z[1] = (uint)num2;
            long num3 = num2 >> 32;
            if (num3 != 0L)
            {
                long num4 = num3 + z[2];
                z[2] = (uint)num4;
                num3 = num4 >> 32;
            }
            long num5 = num3 + (z[3] - 1L);
            z[3] = (uint)num5;
            long num6 = (num5 >> 32) + (z[4] - 1L);
            z[4] = (uint)num6;
            if (num6 >> 32 == 0L)
                return;
            Nat.DecAt( 12, z, 5 );
        }
    }
}
