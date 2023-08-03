// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using System;

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat
    {
        private const ulong M = 4294967295;

        public static uint Add( int len, uint[] x, uint[] y, uint[] z )
        {
            ulong num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                ulong num2 = num1 + x[index] + y[index];
                z[index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (uint)num1;
        }

        public static uint Add33At( int len, uint x, uint[] z, int zPos )
        {
            ulong num1 = z[zPos] + (ulong)x;
            z[zPos] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[zPos + 1] + 1UL;
            z[zPos + 1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, zPos + 2 ) : 0U;
        }

        public static uint Add33At( int len, uint x, uint[] z, int zOff, int zPos )
        {
            ulong num1 = z[zOff + zPos] + (ulong)x;
            z[zOff + zPos] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[zOff + zPos + 1] + 1UL;
            z[zOff + zPos + 1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, zOff, zPos + 2 ) : 0U;
        }

        public static uint Add33To( int len, uint x, uint[] z )
        {
            ulong num1 = z[0] + (ulong)x;
            z[0] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[1] + 1UL;
            z[1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, 2 ) : 0U;
        }

        public static uint Add33To( int len, uint x, uint[] z, int zOff )
        {
            ulong num1 = z[zOff] + (ulong)x;
            z[zOff] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[zOff + 1] + 1UL;
            z[zOff + 1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, zOff, 2 ) : 0U;
        }

        public static uint AddBothTo( int len, uint[] x, uint[] y, uint[] z )
        {
            ulong num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                ulong num2 = num1 + x[index] + y[index] + z[index];
                z[index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (uint)num1;
        }

        public static uint AddBothTo(
          int len,
          uint[] x,
          int xOff,
          uint[] y,
          int yOff,
          uint[] z,
          int zOff )
        {
            ulong num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                ulong num2 = num1 + x[xOff + index] + y[yOff + index] + z[zOff + index];
                z[zOff + index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (uint)num1;
        }

        public static uint AddDWordAt( int len, ulong x, uint[] z, int zPos )
        {
            ulong num1 = z[zPos] + (x & uint.MaxValue);
            z[zPos] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[zPos + 1] + (x >> 32);
            z[zPos + 1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, zPos + 2 ) : 0U;
        }

        public static uint AddDWordAt( int len, ulong x, uint[] z, int zOff, int zPos )
        {
            ulong num1 = z[zOff + zPos] + (x & uint.MaxValue);
            z[zOff + zPos] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[zOff + zPos + 1] + (x >> 32);
            z[zOff + zPos + 1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, zOff, zPos + 2 ) : 0U;
        }

        public static uint AddDWordTo( int len, ulong x, uint[] z )
        {
            ulong num1 = z[0] + (x & uint.MaxValue);
            z[0] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[1] + (x >> 32);
            z[1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, 2 ) : 0U;
        }

        public static uint AddDWordTo( int len, ulong x, uint[] z, int zOff )
        {
            ulong num1 = z[zOff] + (x & uint.MaxValue);
            z[zOff] = (uint)num1;
            ulong num2 = (num1 >> 32) + z[zOff + 1] + (x >> 32);
            z[zOff + 1] = (uint)num2;
            return num2 >> 32 != 0UL ? IncAt( len, z, zOff, 2 ) : 0U;
        }

        public static uint AddTo( int len, uint[] x, uint[] z )
        {
            ulong num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                ulong num2 = num1 + x[index] + z[index];
                z[index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (uint)num1;
        }

        public static uint AddTo( int len, uint[] x, int xOff, uint[] z, int zOff )
        {
            ulong num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                ulong num2 = num1 + x[xOff + index] + z[zOff + index];
                z[zOff + index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (uint)num1;
        }

        public static uint AddWordAt( int len, uint x, uint[] z, int zPos )
        {
            ulong num = x + (ulong)z[zPos];
            z[zPos] = (uint)num;
            return num >> 32 != 0UL ? IncAt( len, z, zPos + 1 ) : 0U;
        }

        public static uint AddWordAt( int len, uint x, uint[] z, int zOff, int zPos )
        {
            ulong num = x + (ulong)z[zOff + zPos];
            z[zOff + zPos] = (uint)num;
            return num >> 32 != 0UL ? IncAt( len, z, zOff, zPos + 1 ) : 0U;
        }

        public static uint AddWordTo( int len, uint x, uint[] z )
        {
            ulong num = x + (ulong)z[0];
            z[0] = (uint)num;
            return num >> 32 != 0UL ? IncAt( len, z, 1 ) : 0U;
        }

        public static uint AddWordTo( int len, uint x, uint[] z, int zOff )
        {
            ulong num = x + (ulong)z[zOff];
            z[zOff] = (uint)num;
            return num >> 32 != 0UL ? IncAt( len, z, zOff, 1 ) : 0U;
        }

        public static void Copy( int len, uint[] x, uint[] z ) => Array.Copy( x, 0, z, 0, len );

        public static uint[] Copy( int len, uint[] x )
        {
            uint[] destinationArray = new uint[len];
            Array.Copy( x, 0, destinationArray, 0, len );
            return destinationArray;
        }

        public static uint[] Create( int len ) => new uint[len];

        public static ulong[] Create64( int len ) => new ulong[len];

        public static int Dec( int len, uint[] z )
        {
            for (int index = 0; index < len; ++index)
            {
                if (--z[index] != uint.MaxValue)
                    return 0;
            }
            return -1;
        }

        public static int Dec( int len, uint[] x, uint[] z )
        {
            int index = 0;
            while (index < len)
            {
                uint num = x[index] - 1U;
                z[index] = num;
                ++index;
                if (num != uint.MaxValue)
                {
                    for (; index < len; ++index)
                        z[index] = x[index];
                    return 0;
                }
            }
            return -1;
        }

        public static int DecAt( int len, uint[] z, int zPos )
        {
            for (int index = zPos; index < len; ++index)
            {
                if (--z[index] != uint.MaxValue)
                    return 0;
            }
            return -1;
        }

        public static int DecAt( int len, uint[] z, int zOff, int zPos )
        {
            for (int index = zPos; index < len; ++index)
            {
                if (--z[zOff + index] != uint.MaxValue)
                    return 0;
            }
            return -1;
        }

        public static bool Eq( int len, uint[] x, uint[] y )
        {
            for (int index = len - 1; index >= 0; --index)
            {
                if ((int)x[index] != (int)y[index])
                    return false;
            }
            return true;
        }

        public static uint[] FromBigInteger( int bits, BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > bits)
                throw new ArgumentException();
            uint[] numArray = Create( (bits + 31) >> 5 );
            int num = 0;
            for (; x.SignValue != 0; x = x.ShiftRight( 32 ))
                numArray[num++] = (uint)x.IntValue;
            return numArray;
        }

        public static uint GetBit( uint[] x, int bit )
        {
            if (bit == 0)
                return x[0] & 1U;
            int index = bit >> 5;
            if (index < 0 || index >= x.Length)
                return 0;
            int num = bit & 31;
            return (x[index] >> num) & 1U;
        }

        public static bool Gte( int len, uint[] x, uint[] y )
        {
            for (int index = len - 1; index >= 0; --index)
            {
                uint num1 = x[index];
                uint num2 = y[index];
                if (num1 < num2)
                    return false;
                if (num1 > num2)
                    return true;
            }
            return true;
        }

        public static uint Inc( int len, uint[] z )
        {
            for (int index = 0; index < len; ++index)
            {
                if (++z[index] != 0U)
                    return 0;
            }
            return 1;
        }

        public static uint Inc( int len, uint[] x, uint[] z )
        {
            int index = 0;
            while (index < len)
            {
                uint num = x[index] + 1U;
                z[index] = num;
                ++index;
                if (num != 0U)
                {
                    for (; index < len; ++index)
                        z[index] = x[index];
                    return 0;
                }
            }
            return 1;
        }

        public static uint IncAt( int len, uint[] z, int zPos )
        {
            for (int index = zPos; index < len; ++index)
            {
                if (++z[index] != 0U)
                    return 0;
            }
            return 1;
        }

        public static uint IncAt( int len, uint[] z, int zOff, int zPos )
        {
            for (int index = zPos; index < len; ++index)
            {
                if (++z[zOff + index] != 0U)
                    return 0;
            }
            return 1;
        }

        public static bool IsOne( int len, uint[] x )
        {
            if (x[0] != 1U)
                return false;
            for (int index = 1; index < len; ++index)
            {
                if (x[index] != 0U)
                    return false;
            }
            return true;
        }

        public static bool IsZero( int len, uint[] x )
        {
            if (x[0] != 0U)
                return false;
            for (int index = 1; index < len; ++index)
            {
                if (x[index] != 0U)
                    return false;
            }
            return true;
        }

        public static void Mul( int len, uint[] x, uint[] y, uint[] zz )
        {
            zz[len] = MulWord( len, x[0], y, zz );
            for (int zOff = 1; zOff < len; ++zOff)
                zz[zOff + len] = MulWordAddTo( len, x[zOff], y, 0, zz, zOff );
        }

        public static void Mul(
          int len,
          uint[] x,
          int xOff,
          uint[] y,
          int yOff,
          uint[] zz,
          int zzOff )
        {
            zz[zzOff + len] = MulWord( len, x[xOff], y, yOff, zz, zzOff );
            for (int index = 1; index < len; ++index)
                zz[zzOff + index + len] = MulWordAddTo( len, x[xOff + index], y, yOff, zz, zzOff + index );
        }

        public static uint Mul31BothAdd(
          int len,
          uint a,
          uint[] x,
          uint b,
          uint[] y,
          uint[] z,
          int zOff )
        {
            ulong num1 = 0;
            ulong num2 = a;
            ulong num3 = b;
            int index = 0;
            do
            {
                ulong num4 = num1 + (ulong)(((long)num2 * x[index]) + ((long)num3 * y[index])) + z[zOff + index];
                z[zOff + index] = (uint)num4;
                num1 = num4 >> 32;
            }
            while (++index < len);
            return (uint)num1;
        }

        public static uint MulWord( int len, uint x, uint[] y, uint[] z )
        {
            ulong num1 = 0;
            ulong num2 = x;
            int index = 0;
            do
            {
                ulong num3 = num1 + (num2 * y[index]);
                z[index] = (uint)num3;
                num1 = num3 >> 32;
            }
            while (++index < len);
            return (uint)num1;
        }

        public static uint MulWord( int len, uint x, uint[] y, int yOff, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            int num3 = 0;
            do
            {
                ulong num4 = num1 + (num2 * y[yOff + num3]);
                z[zOff + num3] = (uint)num4;
                num1 = num4 >> 32;
            }
            while (++num3 < len);
            return (uint)num1;
        }

        public static uint MulWordAddTo( int len, uint x, uint[] y, int yOff, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            int num3 = 0;
            do
            {
                ulong num4 = num1 + (num2 * y[yOff + num3]) + z[zOff + num3];
                z[zOff + num3] = (uint)num4;
                num1 = num4 >> 32;
            }
            while (++num3 < len);
            return (uint)num1;
        }

        public static uint MulWordDwordAddAt( int len, uint x, ulong y, uint[] z, int zPos )
        {
            ulong num1 = 0;
            ulong num2 = x;
            ulong num3 = num1 + (num2 * (uint)y) + z[zPos];
            z[zPos] = (uint)num3;
            ulong num4 = (num3 >> 32) + (num2 * (y >> 32)) + z[zPos + 1];
            z[zPos + 1] = (uint)num4;
            ulong num5 = (num4 >> 32) + z[zPos + 2];
            z[zPos + 2] = (uint)num5;
            return num5 >> 32 != 0UL ? IncAt( len, z, zPos + 3 ) : 0U;
        }

        public static uint ShiftDownBit( int len, uint[] z, uint c )
        {
            int index = len;
            while (--index >= 0)
            {
                uint num = z[index];
                z[index] = (num >> 1) | (c << 31);
                c = num;
            }
            return c << 31;
        }

        public static uint ShiftDownBit( int len, uint[] z, int zOff, uint c )
        {
            int num1 = len;
            while (--num1 >= 0)
            {
                uint num2 = z[zOff + num1];
                z[zOff + num1] = (num2 >> 1) | (c << 31);
                c = num2;
            }
            return c << 31;
        }

        public static uint ShiftDownBit( int len, uint[] x, uint c, uint[] z )
        {
            int index = len;
            while (--index >= 0)
            {
                uint num = x[index];
                z[index] = (num >> 1) | (c << 31);
                c = num;
            }
            return c << 31;
        }

        public static uint ShiftDownBit( int len, uint[] x, int xOff, uint c, uint[] z, int zOff )
        {
            int num1 = len;
            while (--num1 >= 0)
            {
                uint num2 = x[xOff + num1];
                z[zOff + num1] = (num2 >> 1) | (c << 31);
                c = num2;
            }
            return c << 31;
        }

        public static uint ShiftDownBits( int len, uint[] z, int bits, uint c )
        {
            int index = len;
            while (--index >= 0)
            {
                uint num = z[index];
                z[index] = (num >> bits) | (c << -bits);
                c = num;
            }
            return c << -bits;
        }

        public static uint ShiftDownBits( int len, uint[] z, int zOff, int bits, uint c )
        {
            int num1 = len;
            while (--num1 >= 0)
            {
                uint num2 = z[zOff + num1];
                z[zOff + num1] = (num2 >> bits) | (c << -bits);
                c = num2;
            }
            return c << -bits;
        }

        public static uint ShiftDownBits( int len, uint[] x, int bits, uint c, uint[] z )
        {
            int index = len;
            while (--index >= 0)
            {
                uint num = x[index];
                z[index] = (num >> bits) | (c << -bits);
                c = num;
            }
            return c << -bits;
        }

        public static uint ShiftDownBits(
          int len,
          uint[] x,
          int xOff,
          int bits,
          uint c,
          uint[] z,
          int zOff )
        {
            int num1 = len;
            while (--num1 >= 0)
            {
                uint num2 = x[xOff + num1];
                z[zOff + num1] = (num2 >> bits) | (c << -bits);
                c = num2;
            }
            return c << -bits;
        }

        public static uint ShiftDownWord( int len, uint[] z, uint c )
        {
            int index = len;
            while (--index >= 0)
            {
                uint num = z[index];
                z[index] = c;
                c = num;
            }
            return c;
        }

        public static uint ShiftUpBit( int len, uint[] z, uint c )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = z[index];
                z[index] = (num << 1) | (c >> 31);
                c = num;
            }
            return c >> 31;
        }

        public static uint ShiftUpBit( int len, uint[] z, int zOff, uint c )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = z[zOff + index];
                z[zOff + index] = (num << 1) | (c >> 31);
                c = num;
            }
            return c >> 31;
        }

        public static uint ShiftUpBit( int len, uint[] x, uint c, uint[] z )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = x[index];
                z[index] = (num << 1) | (c >> 31);
                c = num;
            }
            return c >> 31;
        }

        public static uint ShiftUpBit( int len, uint[] x, int xOff, uint c, uint[] z, int zOff )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = x[xOff + index];
                z[zOff + index] = (num << 1) | (c >> 31);
                c = num;
            }
            return c >> 31;
        }

        public static ulong ShiftUpBit64( int len, ulong[] x, int xOff, ulong c, ulong[] z, int zOff )
        {
            for (int index = 0; index < len; ++index)
            {
                ulong num = x[xOff + index];
                z[zOff + index] = (num << 1) | (c >> 63);
                c = num;
            }
            return c >> 63;
        }

        public static uint ShiftUpBits( int len, uint[] z, int bits, uint c )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = z[index];
                z[index] = (num << bits) | (c >> -bits);
                c = num;
            }
            return c >> -bits;
        }

        public static uint ShiftUpBits( int len, uint[] z, int zOff, int bits, uint c )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = z[zOff + index];
                z[zOff + index] = (num << bits) | (c >> -bits);
                c = num;
            }
            return c >> -bits;
        }

        public static ulong ShiftUpBits64( int len, ulong[] z, int zOff, int bits, ulong c )
        {
            for (int index = 0; index < len; ++index)
            {
                ulong num = z[zOff + index];
                z[zOff + index] = (num << bits) | (c >> -bits);
                c = num;
            }
            return c >> -bits;
        }

        public static uint ShiftUpBits( int len, uint[] x, int bits, uint c, uint[] z )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = x[index];
                z[index] = (num << bits) | (c >> -bits);
                c = num;
            }
            return c >> -bits;
        }

        public static uint ShiftUpBits(
          int len,
          uint[] x,
          int xOff,
          int bits,
          uint c,
          uint[] z,
          int zOff )
        {
            for (int index = 0; index < len; ++index)
            {
                uint num = x[xOff + index];
                z[zOff + index] = (num << bits) | (c >> -bits);
                c = num;
            }
            return c >> -bits;
        }

        public static ulong ShiftUpBits64(
          int len,
          ulong[] x,
          int xOff,
          int bits,
          ulong c,
          ulong[] z,
          int zOff )
        {
            for (int index = 0; index < len; ++index)
            {
                ulong num = x[xOff + index];
                z[zOff + index] = (num << bits) | (c >> -bits);
                c = num;
            }
            return c >> -bits;
        }

        public static void Square( int len, uint[] x, uint[] zz )
        {
            int len1 = len << 1;
            uint num1 = 0;
            int num2 = len;
            int num3 = len1;
            do
            {
                ulong num4 = x[--num2];
                ulong num5 = num4 * num4;
                int num6;
                zz[num6 = num3 - 1] = (num1 << 31) | (uint)(num5 >> 33);
                zz[num3 = num6 - 1] = (uint)(num5 >> 1);
                num1 = (uint)num5;
            }
            while (num2 > 0);
            for (int xPos = 1; xPos < len; ++xPos)
            {
                uint x1 = SquareWordAdd( x, xPos, zz );
                int num7 = (int)AddWordAt( len1, x1, zz, xPos << 1 );
            }
            int num8 = (int)ShiftUpBit( len1, zz, x[0] << 31 );
        }

        public static void Square( int len, uint[] x, int xOff, uint[] zz, int zzOff )
        {
            int len1 = len << 1;
            uint num1 = 0;
            int num2 = len;
            int num3 = len1;
            do
            {
                ulong num4 = x[xOff + --num2];
                ulong num5 = num4 * num4;
                int num6;
                zz[zzOff + (num6 = num3 - 1)] = (num1 << 31) | (uint)(num5 >> 33);
                zz[zzOff + (num3 = num6 - 1)] = (uint)(num5 >> 1);
                num1 = (uint)num5;
            }
            while (num2 > 0);
            for (int xPos = 1; xPos < len; ++xPos)
            {
                uint x1 = SquareWordAdd( x, xOff, xPos, zz, zzOff );
                int num7 = (int)AddWordAt( len1, x1, zz, zzOff, xPos << 1 );
            }
            int num8 = (int)ShiftUpBit( len1, zz, zzOff, x[xOff] << 31 );
        }

        public static uint SquareWordAdd( uint[] x, int xPos, uint[] z )
        {
            ulong num1 = 0;
            ulong num2 = x[xPos];
            int index = 0;
            do
            {
                ulong num3 = num1 + (num2 * x[index]) + z[xPos + index];
                z[xPos + index] = (uint)num3;
                num1 = num3 >> 32;
            }
            while (++index < xPos);
            return (uint)num1;
        }

        public static uint SquareWordAdd( uint[] x, int xOff, int xPos, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x[xOff + xPos];
            int num3 = 0;
            do
            {
                ulong num4 = num1 + (ulong)(((long)num2 * (x[xOff + num3] & (long)uint.MaxValue)) + (z[xPos + zOff] & (long)uint.MaxValue));
                z[xPos + zOff] = (uint)num4;
                num1 = num4 >> 32;
                ++zOff;
            }
            while (++num3 < xPos);
            return (uint)num1;
        }

        public static int Sub( int len, uint[] x, uint[] y, uint[] z )
        {
            long num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                long num2 = num1 + (x[index] - (long)y[index]);
                z[index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (int)num1;
        }

        public static int Sub( int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff )
        {
            long num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                long num2 = num1 + (x[xOff + index] - (long)y[yOff + index]);
                z[zOff + index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (int)num1;
        }

        public static int Sub33At( int len, uint x, uint[] z, int zPos )
        {
            long num1 = z[zPos] - (long)x;
            z[zPos] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zPos + 1] - 1L);
            z[zPos + 1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, zPos + 2 ) : 0;
        }

        public static int Sub33At( int len, uint x, uint[] z, int zOff, int zPos )
        {
            long num1 = z[zOff + zPos] - (long)x;
            z[zOff + zPos] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zOff + zPos + 1] - 1L);
            z[zOff + zPos + 1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, zOff, zPos + 2 ) : 0;
        }

        public static int Sub33From( int len, uint x, uint[] z )
        {
            long num1 = z[0] - (long)x;
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + (z[1] - 1L);
            z[1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, 2 ) : 0;
        }

        public static int Sub33From( int len, uint x, uint[] z, int zOff )
        {
            long num1 = z[zOff] - (long)x;
            z[zOff] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zOff + 1] - 1L);
            z[zOff + 1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, zOff, 2 ) : 0;
        }

        public static int SubBothFrom( int len, uint[] x, uint[] y, uint[] z )
        {
            long num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                long num2 = num1 + (z[index] - (long)x[index] - y[index]);
                z[index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (int)num1;
        }

        public static int SubBothFrom(
          int len,
          uint[] x,
          int xOff,
          uint[] y,
          int yOff,
          uint[] z,
          int zOff )
        {
            long num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                long num2 = num1 + (z[zOff + index] - (long)x[xOff + index] - y[yOff + index]);
                z[zOff + index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (int)num1;
        }

        public static int SubDWordAt( int len, ulong x, uint[] z, int zPos )
        {
            long num1 = z[zPos] - ((long)x & uint.MaxValue);
            z[zPos] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zPos + 1] - (long)(x >> 32));
            z[zPos + 1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, zPos + 2 ) : 0;
        }

        public static int SubDWordAt( int len, ulong x, uint[] z, int zOff, int zPos )
        {
            long num1 = z[zOff + zPos] - ((long)x & uint.MaxValue);
            z[zOff + zPos] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zOff + zPos + 1] - (long)(x >> 32));
            z[zOff + zPos + 1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, zOff, zPos + 2 ) : 0;
        }

        public static int SubDWordFrom( int len, ulong x, uint[] z )
        {
            long num1 = z[0] - ((long)x & uint.MaxValue);
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + (z[1] - (long)(x >> 32));
            z[1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, 2 ) : 0;
        }

        public static int SubDWordFrom( int len, ulong x, uint[] z, int zOff )
        {
            long num1 = z[zOff] - ((long)x & uint.MaxValue);
            z[zOff] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zOff + 1] - (long)(x >> 32));
            z[zOff + 1] = (uint)num2;
            return num2 >> 32 != 0L ? DecAt( len, z, zOff, 2 ) : 0;
        }

        public static int SubFrom( int len, uint[] x, uint[] z )
        {
            long num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                long num2 = num1 + (z[index] - (long)x[index]);
                z[index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (int)num1;
        }

        public static int SubFrom( int len, uint[] x, int xOff, uint[] z, int zOff )
        {
            long num1 = 0;
            for (int index = 0; index < len; ++index)
            {
                long num2 = num1 + (z[zOff + index] - (long)x[xOff + index]);
                z[zOff + index] = (uint)num2;
                num1 = num2 >> 32;
            }
            return (int)num1;
        }

        public static int SubWordAt( int len, uint x, uint[] z, int zPos )
        {
            long num = z[zPos] - (long)x;
            z[zPos] = (uint)num;
            return num >> 32 != 0L ? DecAt( len, z, zPos + 1 ) : 0;
        }

        public static int SubWordAt( int len, uint x, uint[] z, int zOff, int zPos )
        {
            long num = z[zOff + zPos] - (long)x;
            z[zOff + zPos] = (uint)num;
            return num >> 32 != 0L ? DecAt( len, z, zOff, zPos + 1 ) : 0;
        }

        public static int SubWordFrom( int len, uint x, uint[] z )
        {
            long num = z[0] - (long)x;
            z[0] = (uint)num;
            return num >> 32 != 0L ? DecAt( len, z, 1 ) : 0;
        }

        public static int SubWordFrom( int len, uint x, uint[] z, int zOff )
        {
            long num = z[zOff] - (long)x;
            z[zOff] = (uint)num;
            return num >> 32 != 0L ? DecAt( len, z, zOff, 1 ) : 0;
        }

        public static BigInteger ToBigInteger( int len, uint[] x )
        {
            byte[] numArray = new byte[len << 2];
            for (int index = 0; index < len; ++index)
            {
                uint n = x[index];
                if (n != 0U)
                    Pack.UInt32_To_BE( n, numArray, (len - 1 - index) << 2 );
            }
            return new BigInteger( 1, numArray );
        }

        public static void Zero( int len, uint[] z )
        {
            for (int index = 0; index < len; ++index)
                z[index] = 0U;
        }
    }
}
