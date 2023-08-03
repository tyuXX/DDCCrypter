// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.Gcm.GcmUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    internal abstract class GcmUtilities
    {
        private const uint E1 = 3774873600;
        private const ulong E1L = 16212958658533785600;
        private static readonly uint[] LOOKUP = GenerateLookup();

        private static uint[] GenerateLookup()
        {
            uint[] lookup = new uint[256];
            for (int index1 = 0; index1 < 256; ++index1)
            {
                uint num = 0;
                for (int index2 = 7; index2 >= 0; --index2)
                {
                    if ((index1 & (1 << index2)) != 0)
                        num ^= 3774873600U >> (7 - index2);
                }
                lookup[index1] = num;
            }
            return lookup;
        }

        internal static byte[] OneAsBytes()
        {
            byte[] numArray = new byte[16];
            numArray[0] = 128;
            return numArray;
        }

        internal static uint[] OneAsUints() => new uint[4]
        {
      2147483648U,
      0U,
      0U,
      0U
        };

        internal static ulong[] OneAsUlongs() => new ulong[2]
        {
      9223372036854775808UL,
      0UL
        };

        internal static byte[] AsBytes( uint[] x ) => Pack.UInt32_To_BE( x );

        internal static void AsBytes( uint[] x, byte[] z ) => Pack.UInt32_To_BE( x, z, 0 );

        internal static byte[] AsBytes( ulong[] x )
        {
            byte[] bs = new byte[16];
            Pack.UInt64_To_BE( x, bs, 0 );
            return bs;
        }

        internal static void AsBytes( ulong[] x, byte[] z ) => Pack.UInt64_To_BE( x, z, 0 );

        internal static uint[] AsUints( byte[] bs )
        {
            uint[] ns = new uint[4];
            Pack.BE_To_UInt32( bs, 0, ns );
            return ns;
        }

        internal static void AsUints( byte[] bs, uint[] output ) => Pack.BE_To_UInt32( bs, 0, output );

        internal static ulong[] AsUlongs( byte[] x )
        {
            ulong[] ns = new ulong[2];
            Pack.BE_To_UInt64( x, 0, ns );
            return ns;
        }

        public static void AsUlongs( byte[] x, ulong[] z ) => Pack.BE_To_UInt64( x, 0, z );

        internal static void Multiply( byte[] x, byte[] y )
        {
            uint[] x1 = AsUints( x );
            uint[] y1 = AsUints( y );
            Multiply( x1, y1 );
            AsBytes( x1, x );
        }

        internal static void Multiply( uint[] x, uint[] y )
        {
            uint num1 = x[0];
            uint num2 = x[1];
            uint num3 = x[2];
            uint num4 = x[3];
            uint num5 = 0;
            uint num6 = 0;
            uint num7 = 0;
            uint num8 = 0;
            for (int index1 = 0; index1 < 4; ++index1)
            {
                int num9 = (int)y[index1];
                for (int index2 = 0; index2 < 32; ++index2)
                {
                    uint num10 = (uint)(num9 >> 31);
                    num9 <<= 1;
                    num5 ^= num1 & num10;
                    num6 ^= num2 & num10;
                    num7 ^= num3 & num10;
                    num8 ^= num4 & num10;
                    uint num11 = (uint)((int)num4 << 31 >> 8);
                    num4 = (num4 >> 1) | (num3 << 31);
                    num3 = (num3 >> 1) | (num2 << 31);
                    num2 = (num2 >> 1) | (num1 << 31);
                    num1 = (num1 >> 1) ^ (num11 & 3774873600U);
                }
            }
            x[0] = num5;
            x[1] = num6;
            x[2] = num7;
            x[3] = num8;
        }

        internal static void Multiply( ulong[] x, ulong[] y )
        {
            ulong num1 = x[0];
            ulong num2 = x[1];
            ulong num3 = 0;
            ulong num4 = 0;
            for (int index1 = 0; index1 < 2; ++index1)
            {
                long num5 = (long)y[index1];
                for (int index2 = 0; index2 < 64; ++index2)
                {
                    ulong num6 = (ulong)(num5 >> 63);
                    num5 <<= 1;
                    num3 ^= num1 & num6;
                    num4 ^= num2 & num6;
                    ulong num7 = (ulong)((long)num2 << 63 >> 8);
                    num2 = (num2 >> 1) | (num1 << 63);
                    num1 = (num1 >> 1) ^ (num7 & 16212958658533785600UL);
                }
            }
            x[0] = num3;
            x[1] = num4;
        }

        internal static void MultiplyP( uint[] x )
        {
            uint num = ShiftRight( x ) >> 8;
            uint[] numArray;
            (numArray = x)[0] = numArray[0] ^ (num & 3774873600U);
        }

        internal static void MultiplyP( uint[] x, uint[] z )
        {
            uint num = ShiftRight( x, z ) >> 8;
            uint[] numArray;
            (numArray = z)[0] = numArray[0] ^ (num & 3774873600U);
        }

        internal static void MultiplyP8( uint[] x )
        {
            uint num = ShiftRightN( x, 8 );
            uint[] numArray;
            (numArray = x)[0] = numArray[0] ^ LOOKUP[(int)(IntPtr)(num >> 24)];
        }

        internal static void MultiplyP8( uint[] x, uint[] y )
        {
            uint num = ShiftRightN( x, 8, y );
            uint[] numArray;
            (numArray = y)[0] = numArray[0] ^ LOOKUP[(int)(IntPtr)(num >> 24)];
        }

        internal static uint ShiftRight( uint[] x )
        {
            uint num1 = x[0];
            x[0] = num1 >> 1;
            uint num2 = num1 << 31;
            uint num3 = x[1];
            x[1] = (num3 >> 1) | num2;
            uint num4 = num3 << 31;
            uint num5 = x[2];
            x[2] = (num5 >> 1) | num4;
            uint num6 = num5 << 31;
            uint num7 = x[3];
            x[3] = (num7 >> 1) | num6;
            return num7 << 31;
        }

        internal static uint ShiftRight( uint[] x, uint[] z )
        {
            uint num1 = x[0];
            z[0] = num1 >> 1;
            uint num2 = num1 << 31;
            uint num3 = x[1];
            z[1] = (num3 >> 1) | num2;
            uint num4 = num3 << 31;
            uint num5 = x[2];
            z[2] = (num5 >> 1) | num4;
            uint num6 = num5 << 31;
            uint num7 = x[3];
            z[3] = (num7 >> 1) | num6;
            return num7 << 31;
        }

        internal static uint ShiftRightN( uint[] x, int n )
        {
            uint num1 = x[0];
            int num2 = 32 - n;
            x[0] = num1 >> n;
            uint num3 = num1 << num2;
            uint num4 = x[1];
            x[1] = (num4 >> n) | num3;
            uint num5 = num4 << num2;
            uint num6 = x[2];
            x[2] = (num6 >> n) | num5;
            uint num7 = num6 << num2;
            uint num8 = x[3];
            x[3] = (num8 >> n) | num7;
            return num8 << num2;
        }

        internal static uint ShiftRightN( uint[] x, int n, uint[] z )
        {
            uint num1 = x[0];
            int num2 = 32 - n;
            z[0] = num1 >> n;
            uint num3 = num1 << num2;
            uint num4 = x[1];
            z[1] = (num4 >> n) | num3;
            uint num5 = num4 << num2;
            uint num6 = x[2];
            z[2] = (num6 >> n) | num5;
            uint num7 = num6 << num2;
            uint num8 = x[3];
            z[3] = (num8 >> n) | num7;
            return num8 << num2;
        }

        internal static void Xor( byte[] x, byte[] y )
        {
            int index1 = 0;
            do
            {
                byte[] numArray1;
                IntPtr index2;
                (numArray1 = x)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray1[(int)index2] ^ (uint)y[index1]);
                int index3 = index1 + 1;
                byte[] numArray2;
                IntPtr index4;
                (numArray2 = x)[(int)(index4 = (IntPtr)index3)] = (byte)(numArray2[(int)index4] ^ (uint)y[index3]);
                int index5 = index3 + 1;
                byte[] numArray3;
                IntPtr index6;
                (numArray3 = x)[(int)(index6 = (IntPtr)index5)] = (byte)(numArray3[(int)index6] ^ (uint)y[index5]);
                int index7 = index5 + 1;
                byte[] numArray4;
                IntPtr index8;
                (numArray4 = x)[(int)(index8 = (IntPtr)index7)] = (byte)(numArray4[(int)index8] ^ (uint)y[index7]);
                index1 = index7 + 1;
            }
            while (index1 < 16);
        }

        internal static void Xor( byte[] x, byte[] y, int yOff, int yLen )
        {
            while (--yLen >= 0)
            {
                byte[] numArray;
                IntPtr index;
                (numArray = x)[(int)(index = (IntPtr)yLen)] = (byte)(numArray[(int)index] ^ (uint)y[yOff + yLen]);
            }
        }

        internal static void Xor( byte[] x, byte[] y, byte[] z )
        {
            int index1 = 0;
            do
            {
                z[index1] = (byte)(x[index1] ^ (uint)y[index1]);
                int index2 = index1 + 1;
                z[index2] = (byte)(x[index2] ^ (uint)y[index2]);
                int index3 = index2 + 1;
                z[index3] = (byte)(x[index3] ^ (uint)y[index3]);
                int index4 = index3 + 1;
                z[index4] = (byte)(x[index4] ^ (uint)y[index4]);
                index1 = index4 + 1;
            }
            while (index1 < 16);
        }

        internal static void Xor( uint[] x, uint[] y )
        {
            uint[] numArray1;
            (numArray1 = x)[0] = numArray1[0] ^ y[0];
            uint[] numArray2;
            (numArray2 = x)[1] = numArray2[1] ^ y[1];
            uint[] numArray3;
            (numArray3 = x)[2] = numArray3[2] ^ y[2];
            uint[] numArray4;
            (numArray4 = x)[3] = numArray4[3] ^ y[3];
        }

        internal static void Xor( uint[] x, uint[] y, uint[] z )
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
        }

        internal static void Xor( ulong[] x, ulong[] y )
        {
            ulong[] numArray1;
            (numArray1 = x)[0] = numArray1[0] ^ y[0];
            ulong[] numArray2;
            (numArray2 = x)[1] = numArray2[1] ^ y[1];
        }

        internal static void Xor( ulong[] x, ulong[] y, ulong[] z )
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
        }
    }
}
