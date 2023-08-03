// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT131Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT131Field
    {
        private const ulong M03 = 7;
        private const ulong M44 = 17592186044415;
        private static readonly ulong[] ROOT_Z = new ulong[3]
        {
      2791191049453778211UL,
      2791191049453778402UL,
      6UL
        };

        public static void Add( ulong[] x, ulong[] y, ulong[] z )
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
        }

        public static void AddExt( ulong[] xx, ulong[] yy, ulong[] zz )
        {
            zz[0] = xx[0] ^ yy[0];
            zz[1] = xx[1] ^ yy[1];
            zz[2] = xx[2] ^ yy[2];
            zz[3] = xx[3] ^ yy[3];
            zz[4] = xx[4] ^ yy[4];
        }

        public static void AddOne( ulong[] x, ulong[] z )
        {
            z[0] = x[0] ^ 1UL;
            z[1] = x[1];
            z[2] = x[2];
        }

        public static ulong[] FromBigInteger( BigInteger x )
        {
            ulong[] z = Nat192.FromBigInteger64( x );
            Reduce61( z, 0 );
            return z;
        }

        public static void Invert( ulong[] x, ulong[] z )
        {
            if (Nat192.IsZero64( x ))
                throw new InvalidOperationException();
            ulong[] numArray1 = Nat192.Create64();
            ulong[] numArray2 = Nat192.Create64();
            Square( x, numArray1 );
            Multiply( numArray1, x, numArray1 );
            SquareN( numArray1, 2, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            SquareN( numArray2, 4, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 8, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            SquareN( numArray2, 16, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 32, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            Square( numArray2, numArray2 );
            Multiply( numArray2, x, numArray2 );
            SquareN( numArray2, 65, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            Square( numArray1, z );
        }

        public static void Multiply( ulong[] x, ulong[] y, ulong[] z )
        {
            ulong[] ext64 = Nat192.CreateExt64();
            ImplMultiply( x, y, ext64 );
            Reduce( ext64, z );
        }

        public static void MultiplyAddToExt( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] ext64 = Nat192.CreateExt64();
            ImplMultiply( x, y, ext64 );
            AddExt( zz, ext64, zz );
        }

        public static void Reduce( ulong[] xx, ulong[] z )
        {
            ulong num1 = xx[0];
            ulong num2 = xx[1];
            ulong num3 = xx[2];
            ulong num4 = xx[3];
            ulong num5 = xx[4];
            ulong num6 = num2 ^ (ulong)(((long)num5 << 61) ^ ((long)num5 << 63));
            ulong num7 = num3 ^ (ulong)((long)(num5 >> 3) ^ (long)(num5 >> 1) ^ (long)num5 ^ ((long)num5 << 5));
            ulong num8 = num4 ^ (num5 >> 59);
            ulong num9 = num1 ^ (ulong)(((long)num8 << 61) ^ ((long)num8 << 63));
            ulong num10 = num6 ^ (ulong)((long)(num8 >> 3) ^ (long)(num8 >> 1) ^ (long)num8 ^ ((long)num8 << 5));
            ulong num11 = num7 ^ (num8 >> 59);
            ulong num12 = num11 >> 3;
            z[0] = (ulong)((long)num9 ^ (long)num12 ^ ((long)num12 << 2) ^ ((long)num12 << 3) ^ ((long)num12 << 8));
            z[1] = num10 ^ (num12 >> 56);
            z[2] = num11 & 7UL;
        }

        public static void Reduce61( ulong[] z, int zOff )
        {
            ulong num1 = z[zOff + 2];
            ulong num2 = num1 >> 3;
            z[zOff] ^= (ulong)((long)num2 ^ ((long)num2 << 2) ^ ((long)num2 << 3) ^ ((long)num2 << 8));
            z[zOff + 1] ^= num2 >> 56;
            z[zOff + 2] = num1 & 7UL;
        }

        public static void Sqrt( ulong[] x, ulong[] z )
        {
            ulong[] x1 = Nat192.Create64();
            ulong num1 = Interleave.Unshuffle( x[0] );
            ulong num2 = Interleave.Unshuffle( x[1] );
            ulong num3 = (ulong)(((long)num1 & uint.MaxValue) | ((long)num2 << 32));
            x1[0] = (num1 >> 32) | (num2 & 18446744069414584320UL);
            ulong num4 = Interleave.Unshuffle( x[2] );
            ulong num5 = num4 & uint.MaxValue;
            x1[1] = num4 >> 32;
            Multiply( x1, ROOT_Z, z );
            ulong[] numArray1;
            (numArray1 = z)[0] = numArray1[0] ^ num3;
            ulong[] numArray2;
            (numArray2 = z)[1] = numArray2[1] ^ num5;
        }

        public static void Square( ulong[] x, ulong[] z )
        {
            ulong[] numArray = Nat.Create64( 5 );
            ImplSquare( x, numArray );
            Reduce( numArray, z );
        }

        public static void SquareAddToExt( ulong[] x, ulong[] zz )
        {
            ulong[] numArray = Nat.Create64( 5 );
            ImplSquare( x, numArray );
            AddExt( zz, numArray, zz );
        }

        public static void SquareN( ulong[] x, int n, ulong[] z )
        {
            ulong[] numArray = Nat.Create64( 5 );
            ImplSquare( x, numArray );
            Reduce( numArray, z );
            while (--n > 0)
            {
                ImplSquare( z, numArray );
                Reduce( numArray, z );
            }
        }

        public static uint Trace( ulong[] x ) => (uint)(x[0] ^ (x[1] >> 59) ^ (x[2] >> 1)) & 1U;

        protected static void ImplCompactExt( ulong[] zz )
        {
            ulong num1 = zz[0];
            ulong num2 = zz[1];
            ulong num3 = zz[2];
            ulong num4 = zz[3];
            ulong num5 = zz[4];
            ulong num6 = zz[5];
            zz[0] = num1 ^ (num2 << 44);
            zz[1] = (num2 >> 20) ^ (num3 << 24);
            zz[2] = (ulong)((long)(num3 >> 40) ^ ((long)num4 << 4) ^ ((long)num5 << 48));
            zz[3] = (num4 >> 60) ^ (num6 << 28) ^ (num5 >> 16);
            zz[4] = num6 >> 36;
            zz[5] = 0UL;
        }

        protected static void ImplMultiply( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong num1 = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong x1 = (ulong)(((long)(num2 >> 24) ^ ((long)num3 << 40)) & 17592186044415L);
            ulong num4 = (ulong)(((long)(num1 >> 44) ^ ((long)num2 << 20)) & 17592186044415L);
            ulong x2 = num1 & 17592186044415UL;
            ulong num5 = y[0];
            ulong num6 = y[1];
            ulong num7 = y[2];
            ulong y1 = (ulong)(((long)(num6 >> 24) ^ ((long)num7 << 40)) & 17592186044415L);
            ulong num8 = (ulong)(((long)(num5 >> 44) ^ ((long)num6 << 20)) & 17592186044415L);
            ulong y2 = num5 & 17592186044415UL;
            ulong[] z = new ulong[10];
            ImplMulw( x2, y2, z, 0 );
            ImplMulw( x1, y1, z, 2 );
            ulong x3 = x2 ^ num4 ^ x1;
            ulong y3 = y2 ^ num8 ^ y1;
            ImplMulw( x3, y3, z, 4 );
            ulong num9 = (ulong)(((long)num4 << 1) ^ ((long)x1 << 2));
            ulong num10 = (ulong)(((long)num8 << 1) ^ ((long)y1 << 2));
            ImplMulw( x2 ^ num9, y2 ^ num10, z, 6 );
            ImplMulw( x3 ^ num9, y3 ^ num10, z, 8 );
            ulong num11 = z[6] ^ z[8];
            ulong num12 = z[7] ^ z[9];
            ulong num13 = (num11 << 1) ^ z[6];
            ulong num14 = num11 ^ (num12 << 1) ^ z[7];
            ulong num15 = num12;
            ulong num16 = z[0];
            ulong num17 = z[1] ^ z[0] ^ z[4];
            ulong num18 = z[1] ^ z[5];
            ulong num19 = (ulong)((long)num16 ^ (long)num13 ^ ((long)z[2] << 4) ^ ((long)z[2] << 1));
            ulong num20 = (ulong)((long)num17 ^ (long)num14 ^ ((long)z[3] << 4) ^ ((long)z[3] << 1));
            ulong num21 = num18 ^ num15;
            ulong num22 = num20 ^ (num19 >> 44);
            ulong num23 = num19 & 17592186044415UL;
            ulong num24 = num21 ^ (num22 >> 44);
            ulong num25 = num22 & 17592186044415UL;
            ulong num26 = (num23 >> 1) ^ (ulong)(((long)num25 & 1L) << 43);
            ulong num27 = (num25 >> 1) ^ (ulong)(((long)num24 & 1L) << 43);
            ulong num28 = num24 >> 1;
            ulong num29 = num26 ^ (num26 << 1);
            ulong num30 = num29 ^ (num29 << 2);
            ulong num31 = num30 ^ (num30 << 4);
            ulong num32 = num31 ^ (num31 << 8);
            ulong num33 = num32 ^ (num32 << 16);
            ulong num34 = (num33 ^ (num33 << 32)) & 17592186044415UL;
            ulong num35 = num27 ^ (num34 >> 43);
            ulong num36 = num35 ^ (num35 << 1);
            ulong num37 = num36 ^ (num36 << 2);
            ulong num38 = num37 ^ (num37 << 4);
            ulong num39 = num38 ^ (num38 << 8);
            ulong num40 = num39 ^ (num39 << 16);
            ulong num41 = (num40 ^ (num40 << 32)) & 17592186044415UL;
            ulong num42 = num28 ^ (num41 >> 43);
            ulong num43 = num42 ^ (num42 << 1);
            ulong num44 = num43 ^ (num43 << 2);
            ulong num45 = num44 ^ (num44 << 4);
            ulong num46 = num45 ^ (num45 << 8);
            ulong num47 = num46 ^ (num46 << 16);
            ulong num48 = num47 ^ (num47 << 32);
            zz[0] = num16;
            zz[1] = num17 ^ num34 ^ z[2];
            zz[2] = num18 ^ num41 ^ num34 ^ z[3];
            zz[3] = num48 ^ num41;
            zz[4] = num48 ^ z[2];
            zz[5] = z[3];
            ImplCompactExt( zz );
        }

        protected static void ImplMulw( ulong x, ulong y, ulong[] z, int zOff )
        {
            ulong[] numArray = new ulong[8];
            numArray[1] = y;
            numArray[2] = numArray[1] << 1;
            numArray[3] = numArray[2] ^ y;
            numArray[4] = numArray[2] << 1;
            numArray[5] = numArray[4] ^ y;
            numArray[6] = numArray[3] << 1;
            numArray[7] = numArray[6] ^ y;
            uint num1 = (uint)x;
            ulong num2 = 0;
            ulong num3 = (ulong)((long)numArray[(int)(IntPtr)(num1 & 7U)] ^ ((long)numArray[(int)(IntPtr)((num1 >> 3) & 7U)] << 3) ^ ((long)numArray[(int)(IntPtr)((num1 >> 6) & 7U)] << 6));
            int num4 = 33;
            do
            {
                uint num5 = (uint)(x >> num4);
                ulong num6 = (ulong)((long)numArray[(int)(IntPtr)(num5 & 7U)] ^ ((long)numArray[(int)(IntPtr)((num5 >> 3) & 7U)] << 3) ^ ((long)numArray[(int)(IntPtr)((num5 >> 6) & 7U)] << 6) ^ ((long)numArray[(int)(IntPtr)((num5 >> 9) & 7U)] << 9));
                num3 ^= num6 << num4;
                num2 ^= num6 >> -num4;
            }
            while ((num4 -= 12) > 0);
            z[zOff] = num3 & 17592186044415UL;
            z[zOff + 1] = (num3 >> 44) ^ (num2 << 20);
        }

        protected static void ImplSquare( ulong[] x, ulong[] zz )
        {
            Interleave.Expand64To128( x[0], zz, 0 );
            Interleave.Expand64To128( x[1], zz, 2 );
            zz[4] = Interleave.Expand8to16( (uint)x[2] );
        }
    }
}
