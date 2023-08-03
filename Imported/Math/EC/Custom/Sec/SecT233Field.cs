// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT233Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using System;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT233Field
    {
        private const ulong M41 = 2199023255551;
        private const ulong M59 = 576460752303423487;

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
            zz[7] = xx[7] ^ yy[7];
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
            Reduce23( z, 0 );
            return z;
        }

        public static void Invert( ulong[] x, ulong[] z )
        {
            if (Nat256.IsZero64( x ))
                throw new InvalidOperationException();
            ulong[] numArray1 = Nat256.Create64();
            ulong[] numArray2 = Nat256.Create64();
            Square( x, numArray1 );
            Multiply( numArray1, x, numArray1 );
            Square( numArray1, numArray1 );
            Multiply( numArray1, x, numArray1 );
            SquareN( numArray1, 3, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            Square( numArray2, numArray2 );
            Multiply( numArray2, x, numArray2 );
            SquareN( numArray2, 7, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 14, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            Square( numArray2, numArray2 );
            Multiply( numArray2, x, numArray2 );
            SquareN( numArray2, 29, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 58, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            SquareN( numArray2, 116, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            Square( numArray1, z );
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
            ulong num8 = xx[7];
            ulong num9 = num4 ^ (num8 << 23);
            ulong num10 = num5 ^ (num8 >> 41) ^ (num8 << 33);
            ulong num11 = num6 ^ (num8 >> 31);
            ulong num12 = num3 ^ (num7 << 23);
            ulong num13 = num9 ^ (num7 >> 41) ^ (num7 << 33);
            ulong num14 = num10 ^ (num7 >> 31);
            ulong num15 = num2 ^ (num11 << 23);
            ulong num16 = num12 ^ (num11 >> 41) ^ (num11 << 33);
            ulong num17 = num13 ^ (num11 >> 31);
            ulong num18 = num1 ^ (num14 << 23);
            ulong num19 = num15 ^ (num14 >> 41) ^ (num14 << 33);
            ulong num20 = num16 ^ (num14 >> 31);
            ulong num21 = num17 >> 41;
            z[0] = num18 ^ num21;
            z[1] = num19 ^ (num21 << 10);
            z[2] = num20;
            z[3] = num17 & 2199023255551UL;
        }

        public static void Reduce23( ulong[] z, int zOff )
        {
            ulong num1 = z[zOff + 3];
            ulong num2 = num1 >> 41;
            z[zOff] ^= num2;
            z[zOff + 1] ^= num2 << 10;
            z[zOff + 3] = num1 & 2199023255551UL;
        }

        public static void Sqrt( ulong[] x, ulong[] z )
        {
            ulong num1 = Interleave.Unshuffle( x[0] );
            ulong num2 = Interleave.Unshuffle( x[1] );
            ulong num3 = (ulong)(((long)num1 & uint.MaxValue) | ((long)num2 << 32));
            ulong num4 = (num1 >> 32) | (num2 & 18446744069414584320UL);
            ulong num5 = Interleave.Unshuffle( x[2] );
            ulong num6 = Interleave.Unshuffle( x[3] );
            ulong num7 = (ulong)(((long)num5 & uint.MaxValue) | ((long)num6 << 32));
            ulong num8 = (num5 >> 32) | (num6 & 18446744069414584320UL);
            ulong num9 = num8 >> 27;
            ulong num10 = num8 ^ ((num4 >> 27) | (num8 << 37));
            ulong num11 = num4 ^ (num4 << 37);
            ulong[] ext64 = Nat256.CreateExt64();
            int[] numArray1 = new int[3] { 32, 117, 191 };
            for (int index1 = 0; index1 < numArray1.Length; ++index1)
            {
                int index2 = numArray1[index1] >> 6;
                int num12 = numArray1[index1] & 63;
                ext64[index2] ^= num11 << num12;
                ext64[index2 + 1] ^= (num10 << num12) | (num11 >> -num12);
                ext64[index2 + 2] ^= (num9 << num12) | (num10 >> -num12);
                ext64[index2 + 3] ^= num9 >> -num12;
            }
            Reduce( ext64, z );
            ulong[] numArray2;
            (numArray2 = z)[0] = numArray2[0] ^ num3;
            ulong[] numArray3;
            (numArray3 = z)[1] = numArray3[1] ^ num7;
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

        public static uint Trace( ulong[] x ) => (uint)(x[0] ^ (x[2] >> 31)) & 1U;

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
            zz[0] = num1 ^ (num2 << 59);
            zz[1] = (num2 >> 5) ^ (num3 << 54);
            zz[2] = (num3 >> 10) ^ (num4 << 49);
            zz[3] = (num4 >> 15) ^ (num5 << 44);
            zz[4] = (num5 >> 20) ^ (num6 << 39);
            zz[5] = (num6 >> 25) ^ (num7 << 34);
            zz[6] = (num7 >> 30) ^ (num8 << 29);
            zz[7] = num8 >> 35;
        }

        protected static void ImplExpand( ulong[] x, ulong[] z )
        {
            ulong num1 = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong num4 = x[3];
            z[0] = num1 & 576460752303423487UL;
            z[1] = (ulong)(((long)(num1 >> 59) ^ ((long)num2 << 5)) & 576460752303423487L);
            z[2] = (ulong)(((long)(num2 >> 54) ^ ((long)num3 << 10)) & 576460752303423487L);
            z[3] = (num3 >> 49) ^ (num4 << 15);
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
            int num4 = 54;
            do
            {
                uint num5 = (uint)(x >> num4);
                ulong num6 = numArray[(int)(IntPtr)(num5 & 7U)] ^ (numArray[(int)(IntPtr)((num5 >> 3) & 7U)] << 3);
                num3 ^= num6 << num4;
                num2 ^= num6 >> -num4;
            }
            while ((num4 -= 6) > 0);
            z[zOff] ^= num3 & 576460752303423487UL;
            z[zOff + 1] ^= (num3 >> 59) ^ (num2 << 5);
        }

        protected static void ImplSquare( ulong[] x, ulong[] zz )
        {
            Interleave.Expand64To128( x[0], zz, 0 );
            Interleave.Expand64To128( x[1], zz, 2 );
            Interleave.Expand64To128( x[2], zz, 4 );
            ulong x1 = x[3];
            zz[6] = Interleave.Expand32to64( (uint)x1 );
            zz[7] = Interleave.Expand16to32( (uint)(x1 >> 32) );
        }
    }
}
