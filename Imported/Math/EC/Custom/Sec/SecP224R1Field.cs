// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP224R1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224R1Field
    {
        private const uint P6 = 4294967295;
        private const uint PExt13 = 4294967295;
        internal static readonly uint[] P = new uint[7]
        {
      1U,
      0U,
      0U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[14]
        {
      1U,
      0U,
      0U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      0U,
      2U,
      0U,
      0U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[11]
        {
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      1U,
      0U,
      0U,
      uint.MaxValue,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      1U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat224.Add( x, y, z ) == 0U && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            AddPInvTo( z );
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
            AddPInvTo( z );
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
            long num1 = xx[10];
            long num2 = xx[11];
            long num3 = xx[12];
            long num4 = xx[13];
            long num5 = xx[7] + num2 - 1L;
            long num6 = xx[8] + num3;
            long num7 = xx[9] + num4;
            long num8 = 0L + (xx[0] - num5);
            long num9 = (uint)num8;
            long num10 = (num8 >> 32) + (xx[1] - num6);
            z[1] = (uint)num10;
            long num11 = (num10 >> 32) + (xx[2] - num7);
            z[2] = (uint)num11;
            long num12 = (num11 >> 32) + (xx[3] + num5 - num1);
            long num13 = (uint)num12;
            long num14 = (num12 >> 32) + (xx[4] + num6 - num2);
            z[4] = (uint)num14;
            long num15 = (num14 >> 32) + (xx[5] + num7 - num3);
            z[5] = (uint)num15;
            long num16 = (num15 >> 32) + (xx[6] + num1 - num4);
            z[6] = (uint)num16;
            long num17 = (num16 >> 32) + 1L;
            long num18 = num13 + num17;
            long num19 = num9 - num17;
            z[0] = (uint)num19;
            long num20 = num19 >> 32;
            if (num20 != 0L)
            {
                long num21 = num20 + z[1];
                z[1] = (uint)num21;
                long num22 = (num21 >> 32) + z[2];
                z[2] = (uint)num22;
                num18 += num22 >> 32;
            }
            z[3] = (uint)num18;
            if ((num18 >> 32 == 0L || Nat.IncAt( 7, z, 4 ) == 0U) && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            AddPInvTo( z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            long num1 = 0;
            if (x != 0U)
            {
                long num2 = x;
                long num3 = num1 + (z[0] - num2);
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
                long num7 = num4 + z[3] + num2;
                z[3] = (uint)num7;
                num1 = num7 >> 32;
            }
            if ((num1 == 0L || Nat.IncAt( 7, z, 4 ) == 0U) && (z[6] != uint.MaxValue || !Nat224.Gte( z, P )))
                return;
            AddPInvTo( z );
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
            SubPInvFrom( z );
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
            AddPInvTo( z );
        }

        private static void AddPInvTo( uint[] z )
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
            if (num5 >> 32 == 0L)
                return;
            int num6 = (int)Nat.IncAt( 7, z, 4 );
        }

        private static void SubPInvFrom( uint[] z )
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
            if (num5 >> 32 == 0L)
                return;
            Nat.DecAt( 7, z, 4 );
        }
    }
}
