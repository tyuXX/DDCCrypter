// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP256R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP256R1Field
    {
        internal const uint P7 = 4294967295;
        internal const uint PExt15 = 4294967294;
        internal static readonly uint[] P = new uint[8]
        {
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      0U,
      0U,
      0U,
      1U,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[16]
        {
      1U,
      0U,
      0U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      4294967294U,
      1U,
      4294967294U,
      1U,
      4294967294U,
      1U,
      1U,
      4294967294U,
      2U,
      4294967294U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat256.Add( x, y, z ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Add( 16, xx, yy, zz ) == 0U && (zz[15] < 4294967294U || !Nat.Gte( 16, zz, PExt )))
                return;
            Nat.SubFrom( 16, PExt, zz );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 8, x, z ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat256.FromBigInteger( x );
            if (numArray[7] == uint.MaxValue && Nat256.Gte( numArray, P ))
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
                uint c = Nat256.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 8, z, c );
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
            if (Nat256.MulAddTo( x, y, zz ) == 0U && (zz[15] < 4294967294U || !Nat.Gte( 16, zz, PExt )))
                return;
            Nat.SubFrom( 16, PExt, zz );
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
            long num1 = xx[8];
            long num2 = xx[9];
            long num3 = xx[10];
            long num4 = xx[11];
            long num5 = xx[12];
            long num6 = xx[13];
            long num7 = xx[14];
            long num8 = xx[15];
            long num9 = num1 - 6L;
            long num10 = num9 + num2;
            long num11 = num2 + num3;
            long num12 = num3 + num4 - num8;
            long num13 = num4 + num5;
            long num14 = num5 + num6;
            long num15 = num6 + num7;
            long num16 = num7 + num8;
            long num17 = 0L + (xx[0] + num10 - num13 - num15);
            z[0] = (uint)num17;
            long num18 = (num17 >> 32) + (xx[1] + num11 - num14 - num16);
            z[1] = (uint)num18;
            long num19 = (num18 >> 32) + (xx[2] + num12 - num15);
            z[2] = (uint)num19;
            long num20 = (num19 >> 32) + (xx[3] + (num13 << 1) + num6 - num8 - num10);
            z[3] = (uint)num20;
            long num21 = (num20 >> 32) + (xx[4] + (num14 << 1) + num7 - num11);
            z[4] = (uint)num21;
            long num22 = (num21 >> 32) + (xx[5] + (num15 << 1) - num12);
            z[5] = (uint)num22;
            long num23 = (num22 >> 32) + (xx[6] + (num16 << 1) + num15 - num10);
            z[6] = (uint)num23;
            long num24 = (num23 >> 32) + (xx[7] + (num8 << 1) + num9 - num12 - num14);
            z[7] = (uint)num24;
            Reduce32( (uint)((ulong)(num24 >> 32) + 6UL), z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            long num1 = 0;
            if (x != 0U)
            {
                long num2 = x;
                long num3 = num1 + z[0] + num2;
                z[0] = (uint)num3;
                long num4 = num3 >> 32;
                if (num4 != 0L)
                {
                    long num5 = num4 + z[1];
                    z[1] = (uint)num5;
                    long num6 = (num5 >> 32) + z[2];
                    z[2] = (uint)num6;
                    num4 = num6 >> 32;
                }
                long num7 = num4 + (z[3] - num2);
                z[3] = (uint)num7;
                long num8 = num7 >> 32;
                if (num8 != 0L)
                {
                    long num9 = num8 + z[4];
                    z[4] = (uint)num9;
                    long num10 = (num9 >> 32) + z[5];
                    z[5] = (uint)num10;
                    num8 = num10 >> 32;
                }
                long num11 = num8 + (z[6] - num2);
                z[6] = (uint)num11;
                long num12 = (num11 >> 32) + z[7] + num2;
                z[7] = (uint)num12;
                num1 = num12 >> 32;
            }
            if (num1 == 0L && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            AddPInvTo( z );
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
            SubPInvFrom( z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 16, xx, yy, zz ) == 0)
                return;
            int num = (int)Nat.AddTo( 16, PExt, zz );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 8, x, 0U, z ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
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
            long num5 = num2 + (z[3] - 1L);
            z[3] = (uint)num5;
            long num6 = num5 >> 32;
            if (num6 != 0L)
            {
                long num7 = num6 + z[4];
                z[4] = (uint)num7;
                long num8 = (num7 >> 32) + z[5];
                z[5] = (uint)num8;
                num6 = num8 >> 32;
            }
            long num9 = num6 + (z[6] - 1L);
            z[6] = (uint)num9;
            long num10 = (num9 >> 32) + z[7] + 1L;
            z[7] = (uint)num10;
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
            long num5 = num2 + z[3] + 1L;
            z[3] = (uint)num5;
            long num6 = num5 >> 32;
            if (num6 != 0L)
            {
                long num7 = num6 + z[4];
                z[4] = (uint)num7;
                long num8 = (num7 >> 32) + z[5];
                z[5] = (uint)num8;
                num6 = num8 >> 32;
            }
            long num9 = num6 + z[6] + 1L;
            z[6] = (uint)num9;
            long num10 = (num9 >> 32) + (z[7] - 1L);
            z[7] = (uint)num10;
        }
    }
}
