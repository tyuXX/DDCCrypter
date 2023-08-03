// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT571Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT571Field
    {
        private const ulong M59 = 576460752303423487;
        private const ulong RM = 17256631552825064414;
        private static readonly ulong[] ROOT_Z = new ulong[9]
        {
      3161836309350906777UL,
      10804290191530228771UL,
      14625517132619890193UL,
      7312758566309945096UL,
      17890083061325672324UL,
      8945041530681231562UL,
      13695892802195391589UL,
      6847946401097695794UL,
      541669439031730457UL
        };

        public static void Add( ulong[] x, ulong[] y, ulong[] z )
        {
            for (int index = 0; index < 9; ++index)
                z[index] = x[index] ^ y[index];
        }

        private static void Add( ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff )
        {
            for (int index = 0; index < 9; ++index)
                z[zOff + index] = x[xOff + index] ^ y[yOff + index];
        }

        private static void AddBothTo( ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff )
        {
            for (int index = 0; index < 9; ++index)
                z[zOff + index] ^= x[xOff + index] ^ y[yOff + index];
        }

        public static void AddExt( ulong[] xx, ulong[] yy, ulong[] zz )
        {
            for (int index = 0; index < 18; ++index)
                zz[index] = xx[index] ^ yy[index];
        }

        public static void AddOne( ulong[] x, ulong[] z )
        {
            z[0] = x[0] ^ 1UL;
            for (int index = 1; index < 9; ++index)
                z[index] = x[index];
        }

        public static ulong[] FromBigInteger( BigInteger x )
        {
            ulong[] z = Nat576.FromBigInteger64( x );
            Reduce5( z, 0 );
            return z;
        }

        public static void Invert( ulong[] x, ulong[] z )
        {
            if (Nat576.IsZero64( x ))
                throw new InvalidOperationException();
            ulong[] numArray1 = Nat576.Create64();
            ulong[] numArray2 = Nat576.Create64();
            ulong[] numArray3 = Nat576.Create64();
            Square( x, numArray3 );
            Square( numArray3, numArray1 );
            Square( numArray1, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 2, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            Multiply( numArray1, numArray3, numArray1 );
            SquareN( numArray1, 5, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray2, 5, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 15, numArray2 );
            Multiply( numArray1, numArray2, numArray3 );
            SquareN( numArray3, 30, numArray1 );
            SquareN( numArray1, 30, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 60, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray2, 60, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 180, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray2, 180, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            Multiply( numArray1, numArray3, z );
        }

        public static void Multiply( ulong[] x, ulong[] y, ulong[] z )
        {
            ulong[] ext64 = Nat576.CreateExt64();
            ImplMultiply( x, y, ext64 );
            Reduce( ext64, z );
        }

        public static void MultiplyAddToExt( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] ext64 = Nat576.CreateExt64();
            ImplMultiply( x, y, ext64 );
            AddExt( zz, ext64, zz );
        }

        public static void Reduce( ulong[] xx, ulong[] z )
        {
            ulong num1 = xx[9];
            ulong num2 = xx[17];
            ulong num3 = num1 ^ (num2 >> 59) ^ (num2 >> 57) ^ (num2 >> 54) ^ (num2 >> 49);
            ulong num4 = (ulong)((long)xx[8] ^ ((long)num2 << 5) ^ ((long)num2 << 7) ^ ((long)num2 << 10) ^ ((long)num2 << 15));
            for (int index = 16; index >= 10; --index)
            {
                ulong num5 = xx[index];
                z[index - 8] = num4 ^ (num5 >> 59) ^ (num5 >> 57) ^ (num5 >> 54) ^ (num5 >> 49);
                num4 = (ulong)((long)xx[index - 9] ^ ((long)num5 << 5) ^ ((long)num5 << 7) ^ ((long)num5 << 10) ^ ((long)num5 << 15));
            }
            ulong num6 = num3;
            z[1] = num4 ^ (num6 >> 59) ^ (num6 >> 57) ^ (num6 >> 54) ^ (num6 >> 49);
            ulong num7 = (ulong)((long)xx[0] ^ ((long)num6 << 5) ^ ((long)num6 << 7) ^ ((long)num6 << 10) ^ ((long)num6 << 15));
            ulong num8 = z[8];
            ulong num9 = num8 >> 59;
            z[0] = (ulong)((long)num7 ^ (long)num9 ^ ((long)num9 << 2) ^ ((long)num9 << 5) ^ ((long)num9 << 10));
            z[8] = num8 & 576460752303423487UL;
        }

        public static void Reduce5( ulong[] z, int zOff )
        {
            ulong num1 = z[zOff + 8];
            ulong num2 = num1 >> 59;
            z[zOff] ^= (ulong)((long)num2 ^ ((long)num2 << 2) ^ ((long)num2 << 5) ^ ((long)num2 << 10));
            z[zOff + 8] = num1 & 576460752303423487UL;
        }

        public static void Sqrt( ulong[] x, ulong[] z )
        {
            ulong[] y = Nat576.Create64();
            ulong[] x1 = Nat576.Create64();
            int index1 = 0;
            for (int index2 = 0; index2 < 4; ++index2)
            {
                ulong[] numArray1 = x;
                int index3 = index1;
                int num1 = index3 + 1;
                ulong num2 = Interleave.Unshuffle( numArray1[index3] );
                ulong[] numArray2 = x;
                int index4 = num1;
                index1 = index4 + 1;
                ulong num3 = Interleave.Unshuffle( numArray2[index4] );
                y[index2] = (ulong)(((long)num2 & uint.MaxValue) | ((long)num3 << 32));
                x1[index2] = (num2 >> 32) | (num3 & 18446744069414584320UL);
            }
            ulong num = Interleave.Unshuffle( x[index1] );
            y[4] = num & uint.MaxValue;
            x1[4] = num >> 32;
            Multiply( x1, ROOT_Z, z );
            Add( z, y, z );
        }

        public static void Square( ulong[] x, ulong[] z )
        {
            ulong[] ext64 = Nat576.CreateExt64();
            ImplSquare( x, ext64 );
            Reduce( ext64, z );
        }

        public static void SquareAddToExt( ulong[] x, ulong[] zz )
        {
            ulong[] ext64 = Nat576.CreateExt64();
            ImplSquare( x, ext64 );
            AddExt( zz, ext64, zz );
        }

        public static void SquareN( ulong[] x, int n, ulong[] z )
        {
            ulong[] ext64 = Nat576.CreateExt64();
            ImplSquare( x, ext64 );
            Reduce( ext64, z );
            while (--n > 0)
            {
                ImplSquare( z, ext64 );
                Reduce( ext64, z );
            }
        }

        public static uint Trace( ulong[] x ) => (uint)(x[0] ^ (x[8] >> 49) ^ (x[8] >> 57)) & 1U;

        protected static void ImplMultiply( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] numArray1 = new ulong[144];
            Array.Copy( y, 0, numArray1, 9, 9 );
            int num1 = 0;
            for (int index = 7; index > 0; --index)
            {
                num1 += 18;
                long num2 = (long)Nat.ShiftUpBit64( 9, numArray1, num1 >> 1, 0UL, numArray1, num1 );
                Reduce5( numArray1, num1 );
                Add( numArray1, 9, numArray1, num1, numArray1, num1 + 9 );
            }
            ulong[] numArray2 = new ulong[numArray1.Length];
            long num3 = (long)Nat.ShiftUpBits64( numArray1.Length, numArray1, 0, 4, 0UL, numArray2, 0 );
            uint num4 = 15;
            for (int index1 = 56; index1 >= 0; index1 -= 8)
            {
                for (int index2 = 1; index2 < 9; index2 += 2)
                {
                    uint num5 = (uint)(x[index2] >> index1);
                    uint num6 = num5 & num4;
                    uint num7 = (num5 >> 4) & num4;
                    AddBothTo( numArray1, 9 * (int)num6, numArray2, 9 * (int)num7, zz, index2 - 1 );
                }
                long num8 = (long)Nat.ShiftUpBits64( 16, zz, 0, 8, 0UL );
            }
            for (int index = 56; index >= 0; index -= 8)
            {
                for (int zOff = 0; zOff < 9; zOff += 2)
                {
                    uint num9 = (uint)(x[zOff] >> index);
                    uint num10 = num9 & num4;
                    uint num11 = (num9 >> 4) & num4;
                    AddBothTo( numArray1, 9 * (int)num10, numArray2, 9 * (int)num11, zz, zOff );
                }
                if (index > 0)
                {
                    long num12 = (long)Nat.ShiftUpBits64( 18, zz, 0, 8, 0UL );
                }
            }
        }

        protected static void ImplMulwAcc( ulong[] xs, ulong y, ulong[] z, int zOff )
        {
            ulong[] numArray = new ulong[32];
            numArray[1] = y;
            for (int index = 2; index < 32; index += 2)
            {
                numArray[index] = numArray[index >> 1] << 1;
                numArray[index + 1] = numArray[index] ^ y;
            }
            ulong num1 = 0;
            for (int index1 = 0; index1 < 9; ++index1)
            {
                ulong num2 = xs[index1];
                uint num3 = (uint)num2;
                ulong num4 = num1 ^ numArray[(int)(IntPtr)(num3 & 31U)];
                ulong num5 = 0;
                int num6 = 60;
                do
                {
                    uint num7 = (uint)(num2 >> num6);
                    ulong num8 = numArray[(int)(IntPtr)(num7 & 31U)];
                    num4 ^= num8 << num6;
                    num5 ^= num8 >> -num6;
                }
                while ((num6 -= 5) > 0);
                for (int index2 = 0; index2 < 4; ++index2)
                {
                    num2 = (num2 & 17256631552825064414UL) >> 1;
                    num5 ^= num2 & (ulong)((long)y << index2 >> 63);
                }
                z[zOff + index1] ^= num4;
                num1 = num5;
            }
            z[zOff + 9] ^= num1;
        }

        protected static void ImplSquare( ulong[] x, ulong[] zz )
        {
            for (int index = 0; index < 9; ++index)
                Interleave.Expand64To128( x[index], zz, index << 1 );
        }
    }
}
