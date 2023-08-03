// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT193Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT193Field
    {
        private const ulong M01 = 1;
        private const ulong M49 = 562949953421311;

        public static void Add( ulong[] x, ulong[] y, ulong[] z )
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
        }

        public static void AddExt( ulong[] xx, ulong[] yy, ulong[] zz )
        {
            zz[0] = xx[0] ^ yy[0];
            zz[1] = xx[1] ^ yy[1];
            zz[2] = xx[2] ^ yy[2];
            zz[3] = xx[3] ^ yy[3];
            zz[4] = xx[4] ^ yy[4];
            zz[5] = xx[5] ^ yy[5];
            zz[6] = xx[6] ^ yy[6];
        }

        public static void AddOne( ulong[] x, ulong[] z )
        {
            z[0] = x[0] ^ 1UL;
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
        }

        public static ulong[] FromBigInteger( BigInteger x )
        {
            ulong[] z = Nat256.FromBigInteger64( x );
            Reduce63( z, 0 );
            return z;
        }

        public static void Invert( ulong[] x, ulong[] z )
        {
            if (Nat256.IsZero64( x ))
                throw new InvalidOperationException();
            ulong[] numArray1 = Nat256.Create64();
            ulong[] numArray2 = Nat256.Create64();
            Square( x, numArray1 );
            SquareN( numArray1, 1, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray2, 1, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 3, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 6, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 12, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 24, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 48, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 96, numArray2 );
            Multiply( numArray1, numArray2, z );
        }

        public static void Multiply( ulong[] x, ulong[] y, ulong[] z )
        {
            ulong[] ext64 = Nat256.CreateExt64();
            ImplMultiply( x, y, ext64 );
            Reduce( ext64, z );
        }

        public static void MultiplyAddToExt( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] ext64 = Nat256.CreateExt64();
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
            ulong num6 = xx[5];
            ulong num7 = xx[6];
            ulong num8 = num3 ^ (num7 << 63);
            ulong num9 = num4 ^ (num7 >> 1) ^ (num7 << 14);
            ulong num10 = num5 ^ (num7 >> 50);
            ulong num11 = num2 ^ (num6 << 63);
            ulong num12 = num8 ^ (num6 >> 1) ^ (num6 << 14);
            ulong num13 = num9 ^ (num6 >> 50);
            ulong num14 = num1 ^ (num10 << 63);
            ulong num15 = num11 ^ (num10 >> 1) ^ (num10 << 14);
            ulong num16 = num12 ^ (num10 >> 50);
            ulong num17 = num13 >> 1;
            z[0] = (ulong)((long)num14 ^ (long)num17 ^ ((long)num17 << 15));
            z[1] = num15 ^ (num17 >> 49);
            z[2] = num16;
            z[3] = num13 & 1UL;
        }

        public static void Reduce63( ulong[] z, int zOff )
        {
            ulong num1 = z[zOff + 3];
            ulong num2 = num1 >> 1;
            z[zOff] ^= num2 ^ (num2 << 15);
            z[zOff + 1] ^= num2 >> 49;
            z[zOff + 3] = num1 & 1UL;
        }

        public static void Sqrt( ulong[] x, ulong[] z )
        {
            ulong num1 = Interleave.Unshuffle( x[0] );
            ulong num2 = Interleave.Unshuffle( x[1] );
            ulong num3 = (ulong)(((long)num1 & uint.MaxValue) | ((long)num2 << 32));
            ulong num4 = (num1 >> 32) | (num2 & 18446744069414584320UL);
            ulong num5 = Interleave.Unshuffle( x[2] );
            ulong num6 = (ulong)(((long)num5 & uint.MaxValue) ^ ((long)x[3] << 32));
            ulong num7 = num5 >> 32;
            z[0] = num3 ^ (num4 << 8);
            z[1] = (ulong)((long)num6 ^ ((long)num7 << 8) ^ (long)(num4 >> 56) ^ ((long)num4 << 33));
            z[2] = (num7 >> 56) ^ (num7 << 33) ^ (num4 >> 31);
            z[3] = num7 >> 31;
        }

        public static void Square( ulong[] x, ulong[] z )
        {
            ulong[] ext64 = Nat256.CreateExt64();
            ImplSquare( x, ext64 );
            Reduce( ext64, z );
        }

        public static void SquareAddToExt( ulong[] x, ulong[] zz )
        {
            ulong[] ext64 = Nat256.CreateExt64();
            ImplSquare( x, ext64 );
            AddExt( zz, ext64, zz );
        }

        public static void SquareN( ulong[] x, int n, ulong[] z )
        {
            ulong[] ext64 = Nat256.CreateExt64();
            ImplSquare( x, ext64 );
            Reduce( ext64, z );
            while (--n > 0)
            {
                ImplSquare( z, ext64 );
                Reduce( ext64, z );
            }
        }

        public static uint Trace( ulong[] x ) => (uint)x[0] & 1U;

        protected static void ImplCompactExt( ulong[] zz )
        {
            ulong num1 = zz[0];
            ulong num2 = zz[1];
            ulong num3 = zz[2];
            ulong num4 = zz[3];
            ulong num5 = zz[4];
            ulong num6 = zz[5];
            ulong num7 = zz[6];
            ulong num8 = zz[7];
            zz[0] = num1 ^ (num2 << 49);
            zz[1] = (num2 >> 15) ^ (num3 << 34);
            zz[2] = (num3 >> 30) ^ (num4 << 19);
            zz[3] = (ulong)((long)(num4 >> 45) ^ ((long)num5 << 4) ^ ((long)num6 << 53));
            zz[4] = (num5 >> 60) ^ (num7 << 38) ^ (num6 >> 11);
            zz[5] = (num7 >> 26) ^ (num8 << 23);
            zz[6] = num8 >> 41;
            zz[7] = 0UL;
        }

        protected static void ImplExpand( ulong[] x, ulong[] z )
        {
            ulong num1 = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong num4 = x[3];
            z[0] = num1 & 562949953421311UL;
            z[1] = (ulong)(((long)(num1 >> 49) ^ ((long)num2 << 15)) & 562949953421311L);
            z[2] = (ulong)(((long)(num2 >> 34) ^ ((long)num3 << 30)) & 562949953421311L);
            z[3] = (num3 >> 19) ^ (num4 << 45);
        }

        protected static void ImplMultiply( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] z1 = new ulong[4];
            ulong[] z2 = new ulong[4];
            ImplExpand( x, z1 );
            ImplExpand( y, z2 );
            ImplMulwAcc( z1[0], z2[0], zz, 0 );
            ImplMulwAcc( z1[1], z2[1], zz, 1 );
            ImplMulwAcc( z1[2], z2[2], zz, 2 );
            ImplMulwAcc( z1[3], z2[3], zz, 3 );
            for (int index = 5; index > 0; --index)
                zz[index] ^= zz[index - 1];
            ImplMulwAcc( z1[0] ^ z1[1], z2[0] ^ z2[1], zz, 1 );
            ImplMulwAcc( z1[2] ^ z1[3], z2[2] ^ z2[3], zz, 3 );
            for (int index = 7; index > 1; --index)
                zz[index] ^= zz[index - 2];
            ulong x1 = z1[0] ^ z1[2];
            ulong x2 = z1[1] ^ z1[3];
            ulong y1 = z2[0] ^ z2[2];
            ulong y2 = z2[1] ^ z2[3];
            ImplMulwAcc( x1 ^ x2, y1 ^ y2, zz, 3 );
            ulong[] z3 = new ulong[3];
            ImplMulwAcc( x1, y1, z3, 0 );
            ImplMulwAcc( x2, y2, z3, 1 );
            ulong num1 = z3[0];
            ulong num2 = z3[1];
            ulong num3 = z3[2];
            ulong[] numArray1;
            (numArray1 = zz)[2] = numArray1[2] ^ num1;
            ulong[] numArray2;
            (numArray2 = zz)[3] = numArray2[3] ^ num1 ^ num2;
            ulong[] numArray3;
            (numArray3 = zz)[4] = numArray3[4] ^ num3 ^ num2;
            ulong[] numArray4;
            (numArray4 = zz)[5] = numArray4[5] ^ num3;
            ImplCompactExt( zz );
        }

        protected static void ImplMulwAcc( ulong x, ulong y, ulong[] z, int zOff )
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
            ulong num3 = numArray[(int)(IntPtr)(num1 & 7U)] ^ (numArray[(int)(IntPtr)((num1 >> 3) & 7U)] << 3);
            int num4 = 36;
            do
            {
                uint num5 = (uint)(x >> num4);
                ulong num6 = (ulong)((long)numArray[(int)(IntPtr)(num5 & 7U)] ^ ((long)numArray[(int)(IntPtr)((num5 >> 3) & 7U)] << 3) ^ ((long)numArray[(int)(IntPtr)((num5 >> 6) & 7U)] << 6) ^ ((long)numArray[(int)(IntPtr)((num5 >> 9) & 7U)] << 9) ^ ((long)numArray[(int)(IntPtr)((num5 >> 12) & 7U)] << 12));
                num3 ^= num6 << num4;
                num2 ^= num6 >> -num4;
            }
            while ((num4 -= 15) > 0);
            z[zOff] ^= num3 & 562949953421311UL;
            z[zOff + 1] ^= (num3 >> 49) ^ (num2 << 15);
        }

        protected static void ImplSquare( ulong[] x, ulong[] zz )
        {
            Interleave.Expand64To128( x[0], zz, 0 );
            Interleave.Expand64To128( x[1], zz, 2 );
            Interleave.Expand64To128( x[2], zz, 4 );
            zz[6] = x[3] & 1UL;
        }
    }
}
