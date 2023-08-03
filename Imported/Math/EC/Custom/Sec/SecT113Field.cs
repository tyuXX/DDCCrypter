// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT113Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT113Field
    {
        private const ulong M49 = 562949953421311;
        private const ulong M57 = 144115188075855871;

        public static void Add( ulong[] x, ulong[] y, ulong[] z )
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
        }

        public static void AddExt( ulong[] xx, ulong[] yy, ulong[] zz )
        {
            zz[0] = xx[0] ^ yy[0];
            zz[1] = xx[1] ^ yy[1];
            zz[2] = xx[2] ^ yy[2];
            zz[3] = xx[3] ^ yy[3];
        }

        public static void AddOne( ulong[] x, ulong[] z )
        {
            z[0] = x[0] ^ 1UL;
            z[1] = x[1];
        }

        public static ulong[] FromBigInteger( BigInteger x )
        {
            ulong[] z = Nat128.FromBigInteger64( x );
            Reduce15( z, 0 );
            return z;
        }

        public static void Invert( ulong[] x, ulong[] z )
        {
            if (Nat128.IsZero64( x ))
                throw new InvalidOperationException();
            ulong[] numArray1 = Nat128.Create64();
            ulong[] numArray2 = Nat128.Create64();
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
            SquareN( numArray2, 28, numArray1 );
            Multiply( numArray1, numArray2, numArray1 );
            SquareN( numArray1, 56, numArray2 );
            Multiply( numArray2, numArray1, numArray2 );
            Square( numArray2, z );
        }

        public static void Multiply( ulong[] x, ulong[] y, ulong[] z )
        {
            ulong[] ext64 = Nat128.CreateExt64();
            ImplMultiply( x, y, ext64 );
            Reduce( ext64, z );
        }

        public static void MultiplyAddToExt( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong[] ext64 = Nat128.CreateExt64();
            ImplMultiply( x, y, ext64 );
            AddExt( zz, ext64, zz );
        }

        public static void Reduce( ulong[] xx, ulong[] z )
        {
            ulong num1 = xx[0];
            ulong num2 = xx[1];
            ulong num3 = xx[2];
            ulong num4 = xx[3];
            ulong num5 = num2 ^ (ulong)(((long)num4 << 15) ^ ((long)num4 << 24));
            ulong num6 = num3 ^ (num4 >> 49) ^ (num4 >> 40);
            ulong num7 = num1 ^ (ulong)(((long)num6 << 15) ^ ((long)num6 << 24));
            ulong num8 = num5 ^ (num6 >> 49) ^ (num6 >> 40);
            ulong num9 = num8 >> 49;
            z[0] = (ulong)((long)num7 ^ (long)num9 ^ ((long)num9 << 9));
            z[1] = num8 & 562949953421311UL;
        }

        public static void Reduce15( ulong[] z, int zOff )
        {
            ulong num1 = z[zOff + 1];
            ulong num2 = num1 >> 49;
            z[zOff] ^= num2 ^ (num2 << 9);
            z[zOff + 1] = num1 & 562949953421311UL;
        }

        public static void Sqrt( ulong[] x, ulong[] z )
        {
            ulong num1 = Interleave.Unshuffle( x[0] );
            ulong num2 = Interleave.Unshuffle( x[1] );
            ulong num3 = (ulong)(((long)num1 & uint.MaxValue) | ((long)num2 << 32));
            ulong num4 = (num1 >> 32) | (num2 & 18446744069414584320UL);
            z[0] = (ulong)((long)num3 ^ ((long)num4 << 57) ^ ((long)num4 << 5));
            z[1] = (num4 >> 7) ^ (num4 >> 59);
        }

        public static void Square( ulong[] x, ulong[] z )
        {
            ulong[] ext64 = Nat128.CreateExt64();
            ImplSquare( x, ext64 );
            Reduce( ext64, z );
        }

        public static void SquareAddToExt( ulong[] x, ulong[] zz )
        {
            ulong[] ext64 = Nat128.CreateExt64();
            ImplSquare( x, ext64 );
            AddExt( zz, ext64, zz );
        }

        public static void SquareN( ulong[] x, int n, ulong[] z )
        {
            ulong[] ext64 = Nat128.CreateExt64();
            ImplSquare( x, ext64 );
            Reduce( ext64, z );
            while (--n > 0)
            {
                ImplSquare( z, ext64 );
                Reduce( ext64, z );
            }
        }

        public static uint Trace( ulong[] x ) => (uint)x[0] & 1U;

        protected static void ImplMultiply( ulong[] x, ulong[] y, ulong[] zz )
        {
            ulong num1 = x[0];
            ulong num2 = x[1];
            ulong x1 = (ulong)(((long)(num1 >> 57) ^ ((long)num2 << 7)) & 144115188075855871L);
            ulong x2 = num1 & 144115188075855871UL;
            ulong num3 = y[0];
            ulong num4 = y[1];
            ulong y1 = (ulong)(((long)(num3 >> 57) ^ ((long)num4 << 7)) & 144115188075855871L);
            ulong y2 = num3 & 144115188075855871UL;
            ulong[] z = new ulong[6];
            ImplMulw( x2, y2, z, 0 );
            ImplMulw( x1, y1, z, 2 );
            ImplMulw( x2 ^ x1, y2 ^ y1, z, 4 );
            ulong num5 = z[1] ^ z[2];
            ulong num6 = z[0];
            ulong num7 = z[3];
            ulong num8 = z[4] ^ num6 ^ num5;
            ulong num9 = z[5] ^ num7 ^ num5;
            zz[0] = num6 ^ (num8 << 57);
            zz[1] = (num8 >> 7) ^ (num9 << 50);
            zz[2] = (num9 >> 14) ^ (num7 << 43);
            zz[3] = num7 >> 21;
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
            ulong num3 = numArray[(int)(IntPtr)(num1 & 7U)];
            int num4 = 48;
            do
            {
                uint num5 = (uint)(x >> num4);
                ulong num6 = (ulong)((long)numArray[(int)(IntPtr)(num5 & 7U)] ^ ((long)numArray[(int)(IntPtr)((num5 >> 3) & 7U)] << 3) ^ ((long)numArray[(int)(IntPtr)((num5 >> 6) & 7U)] << 6));
                num3 ^= num6 << num4;
                num2 ^= num6 >> -num4;
            }
            while ((num4 -= 9) > 0);
            ulong num7 = num2 ^ (ulong)(((long)x & 72198606942111744L & ((long)y << 7 >> 63)) >>> 8);
            z[zOff] = num3 & 144115188075855871UL;
            z[zOff + 1] = (num3 >> 57) ^ (num7 << 7);
        }

        protected static void ImplSquare( ulong[] x, ulong[] zz )
        {
            Interleave.Expand64To128( x[0], zz, 0 );
            Interleave.Expand64To128( x[1], zz, 2 );
        }
    }
}
