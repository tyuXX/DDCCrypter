// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT409Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT409Field
    {
        private const ulong M25 = 33554431;
        private const ulong M59 = 576460752303423487;

        public static void Add( ulong[] x, ulong[] y, ulong[] z )
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
            z[4] = x[4] ^ y[4];
            z[5] = x[5] ^ y[5];
            z[6] = x[6] ^ y[6];
        }

        public static void AddExt( ulong[] xx, ulong[] yy, ulong[] zz )
        {
            for (int index = 0; index < 13; ++index)
                zz[index] = xx[index] ^ yy[index];
        }

        public static void AddOne( ulong[] x, ulong[] z )
        {
            z[0] = x[0] ^ 1UL;
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
            z[5] = x[5];
            z[6] = x[6];
        }

        public static ulong[] FromBigInteger( BigInteger x )
        {
            ulong[] z = Nat448.FromBigInteger64( x );
            Reduce39( z, 0 );
            return z;
        }

        public static void Invert( ulong[] x, ulong[] z )
        {
            if (Nat448.IsZero64( x ))
                throw new InvalidOperationException();
            ulong[] numArray1 = Nat448.Create64();
            ulong[] numArray2 = Nat448.Create64();
            ulong[] numArray3 = Nat448.Create64();
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
            Multiply( numArray1, numArray2, numArray3 );
            SquareN( numArray3, 24, numArray1 );
            SquareN( numArray1, 24, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 48, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 96, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 192, numArray2 );
            Multiply( numArray1, numArray2, numArray1 );
            Multiply( numArray1, numArray3, z );
        }

        public static void Multiply( ulong[] x, ulong[] y, ulong[] z )
        {
            ulong[] ext64 = Nat448.CreateExt64();
            ImplMultiply( x, y, ext64 );
            Reduce( ext64, z );
        }

        public static void MultiplyAddToExt( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] ext64 = Nat448.CreateExt64();
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
            ulong num9 = xx[12];
            ulong num10 = num6 ^ (num9 << 39);
            ulong num11 = num7 ^ (num9 >> 25) ^ (num9 << 62);
            ulong num12 = num8 ^ (num9 >> 2);
            ulong num13 = xx[11];
            ulong num14 = num5 ^ (num13 << 39);
            ulong num15 = num10 ^ (num13 >> 25) ^ (num13 << 62);
            ulong num16 = num11 ^ (num13 >> 2);
            ulong num17 = xx[10];
            ulong num18 = num4 ^ (num17 << 39);
            ulong num19 = num14 ^ (num17 >> 25) ^ (num17 << 62);
            ulong num20 = num15 ^ (num17 >> 2);
            ulong num21 = xx[9];
            ulong num22 = num3 ^ (num21 << 39);
            ulong num23 = num18 ^ (num21 >> 25) ^ (num21 << 62);
            ulong num24 = num19 ^ (num21 >> 2);
            ulong num25 = xx[8];
            ulong num26 = num2 ^ (num25 << 39);
            ulong num27 = num22 ^ (num25 >> 25) ^ (num25 << 62);
            ulong num28 = num23 ^ (num25 >> 2);
            ulong num29 = num12;
            ulong num30 = num1 ^ (num29 << 39);
            ulong num31 = num26 ^ (num29 >> 25) ^ (num29 << 62);
            ulong num32 = num27 ^ (num29 >> 2);
            ulong num33 = num16 >> 25;
            z[0] = num30 ^ num33;
            z[1] = num31 ^ (num33 << 23);
            z[2] = num32;
            z[3] = num28;
            z[4] = num24;
            z[5] = num20;
            z[6] = num16 & 33554431UL;
        }

        public static void Reduce39( ulong[] z, int zOff )
        {
            ulong num1 = z[zOff + 6];
            ulong num2 = num1 >> 25;
            z[zOff] ^= num2;
            z[zOff + 1] ^= num2 << 23;
            z[zOff + 6] = num1 & 33554431UL;
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
            ulong num9 = Interleave.Unshuffle( x[4] );
            ulong num10 = Interleave.Unshuffle( x[5] );
            ulong num11 = (ulong)(((long)num9 & uint.MaxValue) | ((long)num10 << 32));
            ulong num12 = (num9 >> 32) | (num10 & 18446744069414584320UL);
            ulong num13 = Interleave.Unshuffle( x[6] );
            ulong num14 = num13 & uint.MaxValue;
            ulong num15 = num13 >> 32;
            z[0] = num3 ^ (num4 << 44);
            z[1] = num7 ^ (num8 << 44) ^ (num4 >> 20);
            z[2] = num11 ^ (num12 << 44) ^ (num8 >> 20);
            z[3] = (ulong)((long)num14 ^ ((long)num15 << 44) ^ (long)(num12 >> 20) ^ ((long)num4 << 13));
            z[4] = (num15 >> 20) ^ (num8 << 13) ^ (num4 >> 51);
            z[5] = (num12 << 13) ^ (num8 >> 51);
            z[6] = (num15 << 13) ^ (num12 >> 51);
        }

        public static void Square( ulong[] x, ulong[] z )
        {
            ulong[] numArray = Nat.Create64( 13 );
            ImplSquare( x, numArray );
            Reduce( numArray, z );
        }

        public static void SquareAddToExt( ulong[] x, ulong[] zz )
        {
            ulong[] numArray = Nat.Create64( 13 );
            ImplSquare( x, numArray );
            AddExt( zz, numArray, zz );
        }

        public static void SquareN( ulong[] x, int n, ulong[] z )
        {
            ulong[] numArray = Nat.Create64( 13 );
            ImplSquare( x, numArray );
            Reduce( numArray, z );
            while (--n > 0)
            {
                ImplSquare( z, numArray );
                Reduce( numArray, z );
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
            ulong num9 = zz[8];
            ulong num10 = zz[9];
            ulong num11 = zz[10];
            ulong num12 = zz[11];
            ulong num13 = zz[12];
            ulong num14 = zz[13];
            zz[0] = num1 ^ (num2 << 59);
            zz[1] = (num2 >> 5) ^ (num3 << 54);
            zz[2] = (num3 >> 10) ^ (num4 << 49);
            zz[3] = (num4 >> 15) ^ (num5 << 44);
            zz[4] = (num5 >> 20) ^ (num6 << 39);
            zz[5] = (num6 >> 25) ^ (num7 << 34);
            zz[6] = (num7 >> 30) ^ (num8 << 29);
            zz[7] = (num8 >> 35) ^ (num9 << 24);
            zz[8] = (num9 >> 40) ^ (num10 << 19);
            zz[9] = (num10 >> 45) ^ (num11 << 14);
            zz[10] = (num11 >> 50) ^ (num12 << 9);
            zz[11] = (ulong)((long)(num12 >> 55) ^ ((long)num13 << 4) ^ ((long)num14 << 63));
            zz[12] = (num13 >> 60) ^ (num14 >> 1);
            zz[13] = 0UL;
        }

        protected static void ImplExpand( ulong[] x, ulong[] z )
        {
            ulong num1 = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong num4 = x[3];
            ulong num5 = x[4];
            ulong num6 = x[5];
            ulong num7 = x[6];
            z[0] = num1 & 576460752303423487UL;
            z[1] = (ulong)(((long)(num1 >> 59) ^ ((long)num2 << 5)) & 576460752303423487L);
            z[2] = (ulong)(((long)(num2 >> 54) ^ ((long)num3 << 10)) & 576460752303423487L);
            z[3] = (ulong)(((long)(num3 >> 49) ^ ((long)num4 << 15)) & 576460752303423487L);
            z[4] = (ulong)(((long)(num4 >> 44) ^ ((long)num5 << 20)) & 576460752303423487L);
            z[5] = (ulong)(((long)(num5 >> 39) ^ ((long)num6 << 25)) & 576460752303423487L);
            z[6] = (num6 >> 34) ^ (num7 << 30);
        }

        protected static void ImplMultiply( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] numArray = new ulong[7];
            ulong[] z = new ulong[7];
            ImplExpand( x, numArray );
            ImplExpand( y, z );
            for (int zOff = 0; zOff < 7; ++zOff)
                ImplMulwAcc( numArray, z[zOff], zz, zOff );
            ImplCompactExt( zz );
        }

        protected static void ImplMulwAcc( ulong[] xs, ulong y, ulong[] z, int zOff )
        {
            ulong[] numArray = new ulong[8];
            numArray[1] = y;
            numArray[2] = numArray[1] << 1;
            numArray[3] = numArray[2] ^ y;
            numArray[4] = numArray[2] << 1;
            numArray[5] = numArray[4] ^ y;
            numArray[6] = numArray[3] << 1;
            numArray[7] = numArray[6] ^ y;
            for (int index = 0; index < 7; ++index)
            {
                ulong num1 = xs[index];
                uint num2 = (uint)num1;
                ulong num3 = 0;
                ulong num4 = numArray[(int)(IntPtr)(num2 & 7U)] ^ (numArray[(int)(IntPtr)((num2 >> 3) & 7U)] << 3);
                int num5 = 54;
                do
                {
                    uint num6 = (uint)(num1 >> num5);
                    ulong num7 = numArray[(int)(IntPtr)(num6 & 7U)] ^ (numArray[(int)(IntPtr)((num6 >> 3) & 7U)] << 3);
                    num4 ^= num7 << num5;
                    num3 ^= num7 >> -num5;
                }
                while ((num5 -= 6) > 0);
                z[zOff + index] ^= num4 & 576460752303423487UL;
                z[zOff + index + 1] ^= (num4 >> 59) ^ (num3 << 5);
            }
        }

        protected static void ImplSquare( ulong[] x, ulong[] zz )
        {
            for (int index = 0; index < 6; ++index)
                Interleave.Expand64To128( x[index], zz, index << 1 );
            zz[12] = Interleave.Expand32to64( (uint)x[6] );
        }
    }
}
