// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat128
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat128
    {
        private const ulong M = 4294967295;

        public static uint Add( uint[] x, uint[] y, uint[] z )
        {
            ulong num1 = 0UL + x[0] + y[0];
            z[0] = (uint)num1;
            ulong num2 = (num1 >> 32) + x[1] + y[1];
            z[1] = (uint)num2;
            ulong num3 = (num2 >> 32) + x[2] + y[2];
            z[2] = (uint)num3;
            ulong num4 = (num3 >> 32) + x[3] + y[3];
            z[3] = (uint)num4;
            return (uint)(num4 >> 32);
        }

        public static uint AddBothTo( uint[] x, uint[] y, uint[] z )
        {
            ulong num1 = 0UL + x[0] + y[0] + z[0];
            z[0] = (uint)num1;
            ulong num2 = (num1 >> 32) + x[1] + y[1] + z[1];
            z[1] = (uint)num2;
            ulong num3 = (num2 >> 32) + x[2] + y[2] + z[2];
            z[2] = (uint)num3;
            ulong num4 = (num3 >> 32) + x[3] + y[3] + z[3];
            z[3] = (uint)num4;
            return (uint)(num4 >> 32);
        }

        public static uint AddTo( uint[] x, uint[] z )
        {
            ulong num1 = 0UL + x[0] + z[0];
            z[0] = (uint)num1;
            ulong num2 = (num1 >> 32) + x[1] + z[1];
            z[1] = (uint)num2;
            ulong num3 = (num2 >> 32) + x[2] + z[2];
            z[2] = (uint)num3;
            ulong num4 = (num3 >> 32) + x[3] + z[3];
            z[3] = (uint)num4;
            return (uint)(num4 >> 32);
        }

        public static uint AddTo( uint[] x, int xOff, uint[] z, int zOff, uint cIn )
        {
            ulong num1 = cIn + (ulong)x[xOff] + z[zOff];
            z[zOff] = (uint)num1;
            ulong num2 = (num1 >> 32) + x[xOff + 1] + z[zOff + 1];
            z[zOff + 1] = (uint)num2;
            ulong num3 = (num2 >> 32) + x[xOff + 2] + z[zOff + 2];
            z[zOff + 2] = (uint)num3;
            ulong num4 = (num3 >> 32) + x[xOff + 3] + z[zOff + 3];
            z[zOff + 3] = (uint)num4;
            return (uint)(num4 >> 32);
        }

        public static uint AddToEachOther( uint[] u, int uOff, uint[] v, int vOff )
        {
            ulong num1 = 0UL + u[uOff] + v[vOff];
            u[uOff] = (uint)num1;
            v[vOff] = (uint)num1;
            ulong num2 = (num1 >> 32) + u[uOff + 1] + v[vOff + 1];
            u[uOff + 1] = (uint)num2;
            v[vOff + 1] = (uint)num2;
            ulong num3 = (num2 >> 32) + u[uOff + 2] + v[vOff + 2];
            u[uOff + 2] = (uint)num3;
            v[vOff + 2] = (uint)num3;
            ulong num4 = (num3 >> 32) + u[uOff + 3] + v[vOff + 3];
            u[uOff + 3] = (uint)num4;
            v[vOff + 3] = (uint)num4;
            return (uint)(num4 >> 32);
        }

        public static void Copy( uint[] x, uint[] z )
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
        }

        public static void Copy64( ulong[] x, ulong[] z )
        {
            z[0] = x[0];
            z[1] = x[1];
        }

        public static uint[] Create() => new uint[4];

        public static ulong[] Create64() => new ulong[2];

        public static uint[] CreateExt() => new uint[8];

        public static ulong[] CreateExt64() => new ulong[4];

        public static bool Diff( uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff )
        {
            bool flag = Gte( x, xOff, y, yOff );
            if (flag)
                Sub( x, xOff, y, yOff, z, zOff );
            else
                Sub( y, yOff, x, xOff, z, zOff );
            return flag;
        }

        public static bool Eq( uint[] x, uint[] y )
        {
            for (int index = 3; index >= 0; --index)
            {
                if ((int)x[index] != (int)y[index])
                    return false;
            }
            return true;
        }

        public static bool Eq64( ulong[] x, ulong[] y )
        {
            for (int index = 1; index >= 0; --index)
            {
                if ((long)x[index] != (long)y[index])
                    return false;
            }
            return true;
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > 128)
                throw new ArgumentException();
            uint[] numArray = Create();
            int num = 0;
            for (; x.SignValue != 0; x = x.ShiftRight( 32 ))
                numArray[num++] = (uint)x.IntValue;
            return numArray;
        }

        public static ulong[] FromBigInteger64( BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > 128)
                throw new ArgumentException();
            ulong[] numArray = Create64();
            int num = 0;
            for (; x.SignValue != 0; x = x.ShiftRight( 64 ))
                numArray[num++] = (ulong)x.LongValue;
            return numArray;
        }

        public static uint GetBit( uint[] x, int bit )
        {
            if (bit == 0)
                return x[0] & 1U;
            if ((bit & sbyte.MaxValue) != bit)
                return 0;
            int index = bit >> 5;
            int num = bit & 31;
            return (x[index] >> num) & 1U;
        }

        public static bool Gte( uint[] x, uint[] y )
        {
            for (int index = 3; index >= 0; --index)
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

        public static bool Gte( uint[] x, int xOff, uint[] y, int yOff )
        {
            for (int index = 3; index >= 0; --index)
            {
                uint num1 = x[xOff + index];
                uint num2 = y[yOff + index];
                if (num1 < num2)
                    return false;
                if (num1 > num2)
                    return true;
            }
            return true;
        }

        public static bool IsOne( uint[] x )
        {
            if (x[0] != 1U)
                return false;
            for (int index = 1; index < 4; ++index)
            {
                if (x[index] != 0U)
                    return false;
            }
            return true;
        }

        public static bool IsOne64( ulong[] x )
        {
            if (x[0] != 1UL)
                return false;
            for (int index = 1; index < 2; ++index)
            {
                if (x[index] != 0UL)
                    return false;
            }
            return true;
        }

        public static bool IsZero( uint[] x )
        {
            for (int index = 0; index < 4; ++index)
            {
                if (x[index] != 0U)
                    return false;
            }
            return true;
        }

        public static bool IsZero64( ulong[] x )
        {
            for (int index = 0; index < 2; ++index)
            {
                if (x[index] != 0UL)
                    return false;
            }
            return true;
        }

        public static void Mul( uint[] x, uint[] y, uint[] zz )
        {
            ulong num1 = y[0];
            ulong num2 = y[1];
            ulong num3 = y[2];
            ulong num4 = y[3];
            ulong num5 = 0;
            ulong num6 = x[0];
            ulong num7 = num5 + (num6 * num1);
            zz[0] = (uint)num7;
            ulong num8 = (num7 >> 32) + (num6 * num2);
            zz[1] = (uint)num8;
            ulong num9 = (num8 >> 32) + (num6 * num3);
            zz[2] = (uint)num9;
            ulong num10 = (num9 >> 32) + (num6 * num4);
            zz[3] = (uint)num10;
            ulong num11 = num10 >> 32;
            zz[4] = (uint)num11;
            for (int index = 1; index < 4; ++index)
            {
                ulong num12 = 0;
                ulong num13 = x[index];
                ulong num14 = num12 + (num13 * num1) + zz[index];
                zz[index] = (uint)num14;
                ulong num15 = (num14 >> 32) + (num13 * num2) + zz[index + 1];
                zz[index + 1] = (uint)num15;
                ulong num16 = (num15 >> 32) + (num13 * num3) + zz[index + 2];
                zz[index + 2] = (uint)num16;
                ulong num17 = (num16 >> 32) + (num13 * num4) + zz[index + 3];
                zz[index + 3] = (uint)num17;
                ulong num18 = num17 >> 32;
                zz[index + 4] = (uint)num18;
            }
        }

        public static void Mul( uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff )
        {
            ulong num1 = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = 0;
            ulong num6 = x[xOff];
            ulong num7 = num5 + (num6 * num1);
            zz[zzOff] = (uint)num7;
            ulong num8 = (num7 >> 32) + (num6 * num2);
            zz[zzOff + 1] = (uint)num8;
            ulong num9 = (num8 >> 32) + (num6 * num3);
            zz[zzOff + 2] = (uint)num9;
            ulong num10 = (num9 >> 32) + (num6 * num4);
            zz[zzOff + 3] = (uint)num10;
            ulong num11 = num10 >> 32;
            zz[zzOff + 4] = (uint)num11;
            for (int index = 1; index < 4; ++index)
            {
                ++zzOff;
                ulong num12 = 0;
                ulong num13 = x[xOff + index];
                ulong num14 = num12 + (num13 * num1) + zz[zzOff];
                zz[zzOff] = (uint)num14;
                ulong num15 = (num14 >> 32) + (num13 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint)num15;
                ulong num16 = (num15 >> 32) + (num13 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint)num16;
                ulong num17 = (num16 >> 32) + (num13 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint)num17;
                ulong num18 = num17 >> 32;
                zz[zzOff + 4] = (uint)num18;
            }
        }

        public static uint MulAddTo( uint[] x, uint[] y, uint[] zz )
        {
            ulong num1 = y[0];
            ulong num2 = y[1];
            ulong num3 = y[2];
            ulong num4 = y[3];
            ulong num5 = 0;
            for (int index = 0; index < 4; ++index)
            {
                ulong num6 = 0;
                ulong num7 = x[index];
                ulong num8 = num6 + (num7 * num1) + zz[index];
                zz[index] = (uint)num8;
                ulong num9 = (num8 >> 32) + (num7 * num2) + zz[index + 1];
                zz[index + 1] = (uint)num9;
                ulong num10 = (num9 >> 32) + (num7 * num3) + zz[index + 2];
                zz[index + 2] = (uint)num10;
                ulong num11 = (num10 >> 32) + (num7 * num4) + zz[index + 3];
                zz[index + 3] = (uint)num11;
                ulong num12 = (num11 >> 32) + num5 + zz[index + 4];
                zz[index + 4] = (uint)num12;
                num5 = num12 >> 32;
            }
            return (uint)num5;
        }

        public static uint MulAddTo( uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff )
        {
            ulong num1 = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = 0;
            for (int index = 0; index < 4; ++index)
            {
                ulong num6 = 0;
                ulong num7 = x[xOff + index];
                ulong num8 = num6 + (num7 * num1) + zz[zzOff];
                zz[zzOff] = (uint)num8;
                ulong num9 = (num8 >> 32) + (num7 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint)num9;
                ulong num10 = (num9 >> 32) + (num7 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint)num10;
                ulong num11 = (num10 >> 32) + (num7 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint)num11;
                ulong num12 = (num11 >> 32) + num5 + zz[zzOff + 4];
                zz[zzOff + 4] = (uint)num12;
                num5 = num12 >> 32;
                ++zzOff;
            }
            return (uint)num5;
        }

        public static ulong Mul33Add(
          uint w,
          uint[] x,
          int xOff,
          uint[] y,
          int yOff,
          uint[] z,
          int zOff )
        {
            ulong num1 = 0;
            ulong num2 = w;
            ulong num3 = x[xOff];
            ulong num4 = num1 + (num2 * num3) + y[yOff];
            z[zOff] = (uint)num4;
            ulong num5 = num4 >> 32;
            ulong num6 = x[xOff + 1];
            ulong num7 = num5 + (num2 * num6) + num3 + y[yOff + 1];
            z[zOff + 1] = (uint)num7;
            ulong num8 = num7 >> 32;
            ulong num9 = x[xOff + 2];
            ulong num10 = num8 + (num2 * num9) + num6 + y[yOff + 2];
            z[zOff + 2] = (uint)num10;
            ulong num11 = num10 >> 32;
            ulong num12 = x[xOff + 3];
            ulong num13 = num11 + (num2 * num12) + num9 + y[yOff + 3];
            z[zOff + 3] = (uint)num13;
            return (num13 >> 32) + num12;
        }

        public static uint MulWordAddExt( uint x, uint[] yy, int yyOff, uint[] zz, int zzOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            ulong num3 = num1 + (num2 * yy[yyOff]) + zz[zzOff];
            zz[zzOff] = (uint)num3;
            ulong num4 = (num3 >> 32) + (num2 * yy[yyOff + 1]) + zz[zzOff + 1];
            zz[zzOff + 1] = (uint)num4;
            ulong num5 = (num4 >> 32) + (num2 * yy[yyOff + 2]) + zz[zzOff + 2];
            zz[zzOff + 2] = (uint)num5;
            ulong num6 = (num5 >> 32) + (num2 * yy[yyOff + 3]) + zz[zzOff + 3];
            zz[zzOff + 3] = (uint)num6;
            return (uint)(num6 >> 32);
        }

        public static uint Mul33DWordAdd( uint x, ulong y, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            ulong num3 = y & uint.MaxValue;
            ulong num4 = num1 + (num2 * num3) + z[zOff];
            z[zOff] = (uint)num4;
            ulong num5 = num4 >> 32;
            ulong num6 = y >> 32;
            ulong num7 = num5 + (num2 * num6) + num3 + z[zOff + 1];
            z[zOff + 1] = (uint)num7;
            ulong num8 = (num7 >> 32) + num6 + z[zOff + 2];
            z[zOff + 2] = (uint)num8;
            ulong num9 = (num8 >> 32) + z[zOff + 3];
            z[zOff + 3] = (uint)num9;
            return (uint)(num9 >> 32);
        }

        public static uint Mul33WordAdd( uint x, uint y, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = y;
            ulong num3 = num1 + (num2 * x) + z[zOff];
            z[zOff] = (uint)num3;
            ulong num4 = (num3 >> 32) + num2 + z[zOff + 1];
            z[zOff + 1] = (uint)num4;
            ulong num5 = (num4 >> 32) + z[zOff + 2];
            z[zOff + 2] = (uint)num5;
            return num5 >> 32 != 0UL ? Nat.IncAt( 4, z, zOff, 3 ) : 0U;
        }

        public static uint MulWordDwordAdd( uint x, ulong y, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            ulong num3 = num1 + (num2 * y) + z[zOff];
            z[zOff] = (uint)num3;
            ulong num4 = (num3 >> 32) + (num2 * (y >> 32)) + z[zOff + 1];
            z[zOff + 1] = (uint)num4;
            ulong num5 = (num4 >> 32) + z[zOff + 2];
            z[zOff + 2] = (uint)num5;
            return num5 >> 32 != 0UL ? Nat.IncAt( 4, z, zOff, 3 ) : 0U;
        }

        public static uint MulWordsAdd( uint x, uint y, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            ulong num3 = y;
            ulong num4 = num1 + (num3 * num2) + z[zOff];
            z[zOff] = (uint)num4;
            ulong num5 = (num4 >> 32) + z[zOff + 1];
            z[zOff + 1] = (uint)num5;
            return num5 >> 32 != 0UL ? Nat.IncAt( 4, z, zOff, 2 ) : 0U;
        }

        public static uint MulWord( uint x, uint[] y, uint[] z, int zOff )
        {
            ulong num1 = 0;
            ulong num2 = x;
            int index = 0;
            do
            {
                ulong num3 = num1 + (num2 * y[index]);
                z[zOff + index] = (uint)num3;
                num1 = num3 >> 32;
            }
            while (++index < 4);
            return (uint)num1;
        }

        public static void Square( uint[] x, uint[] zz )
        {
            ulong num1 = x[0];
            uint num2 = 0;
            int num3 = 3;
            int num4 = 8;
            do
            {
                ulong num5 = x[num3--];
                ulong num6 = num5 * num5;
                int num7;
                zz[num7 = num4 - 1] = (num2 << 31) | (uint)(num6 >> 33);
                zz[num4 = num7 - 1] = (uint)(num6 >> 1);
                num2 = (uint)num6;
            }
            while (num3 > 0);
            ulong num8 = num1 * num1;
            ulong num9 = num2 << 31 | (num8 >> 33);
            zz[0] = (uint)num8;
            uint num10 = (uint)(num8 >> 32) & 1U;
            ulong num11 = x[1];
            ulong num12 = zz[2];
            ulong num13 = num9 + (num11 * num1);
            uint num14 = (uint)num13;
            zz[1] = (num14 << 1) | num10;
            uint num15 = num14 >> 31;
            ulong num16 = num12 + (num13 >> 32);
            ulong num17 = x[2];
            ulong num18 = zz[3];
            ulong num19 = zz[4];
            ulong num20 = num16 + (num17 * num1);
            uint num21 = (uint)num20;
            zz[2] = (num21 << 1) | num15;
            uint num22 = num21 >> 31;
            ulong num23 = num18 + (num20 >> 32) + (num17 * num11);
            ulong num24 = num19 + (num23 >> 32);
            ulong num25 = num23 & uint.MaxValue;
            ulong num26 = x[3];
            ulong num27 = zz[5];
            ulong num28 = zz[6];
            ulong num29 = num25 + (num26 * num1);
            uint num30 = (uint)num29;
            zz[3] = (num30 << 1) | num22;
            uint num31 = num30 >> 31;
            ulong num32 = num24 + (num29 >> 32) + (num26 * num11);
            ulong num33 = num27 + (num32 >> 32) + (num26 * num17);
            ulong num34 = num28 + (num33 >> 32);
            uint num35 = (uint)num32;
            zz[4] = (num35 << 1) | num31;
            uint num36 = num35 >> 31;
            uint num37 = (uint)num33;
            zz[5] = (num37 << 1) | num36;
            uint num38 = num37 >> 31;
            uint num39 = (uint)num34;
            zz[6] = (num39 << 1) | num38;
            uint num40 = num39 >> 31;
            uint num41 = zz[7] + (uint)(num34 >> 32);
            zz[7] = (num41 << 1) | num40;
        }

        public static void Square( uint[] x, int xOff, uint[] zz, int zzOff )
        {
            ulong num1 = x[xOff];
            uint num2 = 0;
            int num3 = 3;
            int num4 = 8;
            do
            {
                ulong num5 = x[xOff + num3--];
                ulong num6 = num5 * num5;
                int num7;
                zz[zzOff + (num7 = num4 - 1)] = (num2 << 31) | (uint)(num6 >> 33);
                zz[zzOff + (num4 = num7 - 1)] = (uint)(num6 >> 1);
                num2 = (uint)num6;
            }
            while (num3 > 0);
            ulong num8 = num1 * num1;
            ulong num9 = num2 << 31 | (num8 >> 33);
            zz[zzOff] = (uint)num8;
            uint num10 = (uint)(num8 >> 32) & 1U;
            ulong num11 = x[xOff + 1];
            ulong num12 = zz[zzOff + 2];
            ulong num13 = num9 + (num11 * num1);
            uint num14 = (uint)num13;
            zz[zzOff + 1] = (num14 << 1) | num10;
            uint num15 = num14 >> 31;
            ulong num16 = num12 + (num13 >> 32);
            ulong num17 = x[xOff + 2];
            ulong num18 = zz[zzOff + 3];
            ulong num19 = zz[zzOff + 4];
            ulong num20 = num16 + (num17 * num1);
            uint num21 = (uint)num20;
            zz[zzOff + 2] = (num21 << 1) | num15;
            uint num22 = num21 >> 31;
            ulong num23 = num18 + (num20 >> 32) + (num17 * num11);
            ulong num24 = num19 + (num23 >> 32);
            ulong num25 = num23 & uint.MaxValue;
            ulong num26 = x[xOff + 3];
            ulong num27 = zz[zzOff + 5];
            ulong num28 = zz[zzOff + 6];
            ulong num29 = num25 + (num26 * num1);
            uint num30 = (uint)num29;
            zz[zzOff + 3] = (num30 << 1) | num22;
            uint num31 = num30 >> 31;
            ulong num32 = num24 + (num29 >> 32) + (num26 * num11);
            ulong num33 = num27 + (num32 >> 32) + (num26 * num17);
            ulong num34 = num28 + (num33 >> 32);
            uint num35 = (uint)num32;
            zz[zzOff + 4] = (num35 << 1) | num31;
            uint num36 = num35 >> 31;
            uint num37 = (uint)num33;
            zz[zzOff + 5] = (num37 << 1) | num36;
            uint num38 = num37 >> 31;
            uint num39 = (uint)num34;
            zz[zzOff + 6] = (num39 << 1) | num38;
            uint num40 = num39 >> 31;
            uint num41 = zz[zzOff + 7] + (uint)(num34 >> 32);
            zz[zzOff + 7] = (num41 << 1) | num40;
        }

        public static int Sub( uint[] x, uint[] y, uint[] z )
        {
            long num1 = 0L + (x[0] - (long)y[0]);
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + (x[1] - (long)y[1]);
            z[1] = (uint)num2;
            long num3 = (num2 >> 32) + (x[2] - (long)y[2]);
            z[2] = (uint)num3;
            long num4 = (num3 >> 32) + (x[3] - (long)y[3]);
            z[3] = (uint)num4;
            return (int)(num4 >> 32);
        }

        public static int Sub( uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff )
        {
            long num1 = 0L + (x[xOff] - (long)y[yOff]);
            z[zOff] = (uint)num1;
            long num2 = (num1 >> 32) + (x[xOff + 1] - (long)y[yOff + 1]);
            z[zOff + 1] = (uint)num2;
            long num3 = (num2 >> 32) + (x[xOff + 2] - (long)y[yOff + 2]);
            z[zOff + 2] = (uint)num3;
            long num4 = (num3 >> 32) + (x[xOff + 3] - (long)y[yOff + 3]);
            z[zOff + 3] = (uint)num4;
            return (int)(num4 >> 32);
        }

        public static int SubBothFrom( uint[] x, uint[] y, uint[] z )
        {
            long num1 = 0L + (z[0] - (long)x[0] - y[0]);
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + (z[1] - (long)x[1] - y[1]);
            z[1] = (uint)num2;
            long num3 = (num2 >> 32) + (z[2] - (long)x[2] - y[2]);
            z[2] = (uint)num3;
            long num4 = (num3 >> 32) + (z[3] - (long)x[3] - y[3]);
            z[3] = (uint)num4;
            return (int)(num4 >> 32);
        }

        public static int SubFrom( uint[] x, uint[] z )
        {
            long num1 = 0L + (z[0] - (long)x[0]);
            z[0] = (uint)num1;
            long num2 = (num1 >> 32) + (z[1] - (long)x[1]);
            z[1] = (uint)num2;
            long num3 = (num2 >> 32) + (z[2] - (long)x[2]);
            z[2] = (uint)num3;
            long num4 = (num3 >> 32) + (z[3] - (long)x[3]);
            z[3] = (uint)num4;
            return (int)(num4 >> 32);
        }

        public static int SubFrom( uint[] x, int xOff, uint[] z, int zOff )
        {
            long num1 = 0L + (z[zOff] - (long)x[xOff]);
            z[zOff] = (uint)num1;
            long num2 = (num1 >> 32) + (z[zOff + 1] - (long)x[xOff + 1]);
            z[zOff + 1] = (uint)num2;
            long num3 = (num2 >> 32) + (z[zOff + 2] - (long)x[xOff + 2]);
            z[zOff + 2] = (uint)num3;
            long num4 = (num3 >> 32) + (z[zOff + 3] - (long)x[xOff + 3]);
            z[zOff + 3] = (uint)num4;
            return (int)(num4 >> 32);
        }

        public static BigInteger ToBigInteger( uint[] x )
        {
            byte[] numArray = new byte[16];
            for (int index = 0; index < 4; ++index)
            {
                uint n = x[index];
                if (n != 0U)
                    Pack.UInt32_To_BE( n, numArray, (3 - index) << 2 );
            }
            return new BigInteger( 1, numArray );
        }

        public static BigInteger ToBigInteger64( ulong[] x )
        {
            byte[] numArray = new byte[16];
            for (int index = 0; index < 2; ++index)
            {
                ulong n = x[index];
                if (n != 0UL)
                    Pack.UInt64_To_BE( n, numArray, (1 - index) << 3 );
            }
            return new BigInteger( 1, numArray );
        }

        public static void Zero( uint[] z )
        {
            z[0] = 0U;
            z[1] = 0U;
            z[2] = 0U;
            z[3] = 0U;
        }
    }
}
