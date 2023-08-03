// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat160
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat160
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
            ulong num5 = (num4 >> 32) + x[4] + y[4];
            z[4] = (uint)num5;
            return (uint)(num5 >> 32);
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
            ulong num5 = (num4 >> 32) + x[4] + y[4] + z[4];
            z[4] = (uint)num5;
            return (uint)(num5 >> 32);
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
            ulong num5 = (num4 >> 32) + x[4] + z[4];
            z[4] = (uint)num5;
            return (uint)(num5 >> 32);
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
            ulong num5 = (num4 >> 32) + x[xOff + 4] + z[zOff + 4];
            z[zOff + 4] = (uint)num5;
            return (uint)((num5 >> 32) + x[xOff + 5] + z[zOff + 5]);
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
            ulong num5 = (num4 >> 32) + u[uOff + 4] + v[vOff + 4];
            u[uOff + 4] = (uint)num5;
            v[vOff + 4] = (uint)num5;
            return (uint)(num5 >> 32);
        }

        public static void Copy( uint[] x, uint[] z )
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
        }

        public static uint[] Create() => new uint[5];

        public static uint[] CreateExt() => new uint[10];

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
            for (int index = 4; index >= 0; --index)
            {
                if ((int)x[index] != (int)y[index])
                    return false;
            }
            return true;
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > 160)
                throw new ArgumentException();
            uint[] numArray = Create();
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
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    int num = bit & 31;
                    return (x[index] >> num) & 1U;
                default:
                    return 0;
            }
        }

        public static bool Gte( uint[] x, uint[] y )
        {
            for (int index = 4; index >= 0; --index)
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
            for (int index = 4; index >= 0; --index)
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
            for (int index = 1; index < 5; ++index)
            {
                if (x[index] != 0U)
                    return false;
            }
            return true;
        }

        public static bool IsZero( uint[] x )
        {
            for (int index = 0; index < 5; ++index)
            {
                if (x[index] != 0U)
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
            ulong num5 = y[4];
            ulong num6 = 0;
            ulong num7 = x[0];
            ulong num8 = num6 + (num7 * num1);
            zz[0] = (uint)num8;
            ulong num9 = (num8 >> 32) + (num7 * num2);
            zz[1] = (uint)num9;
            ulong num10 = (num9 >> 32) + (num7 * num3);
            zz[2] = (uint)num10;
            ulong num11 = (num10 >> 32) + (num7 * num4);
            zz[3] = (uint)num11;
            ulong num12 = (num11 >> 32) + (num7 * num5);
            zz[4] = (uint)num12;
            ulong num13 = num12 >> 32;
            zz[5] = (uint)num13;
            for (int index = 1; index < 5; ++index)
            {
                ulong num14 = 0;
                ulong num15 = x[index];
                ulong num16 = num14 + (num15 * num1) + zz[index];
                zz[index] = (uint)num16;
                ulong num17 = (num16 >> 32) + (num15 * num2) + zz[index + 1];
                zz[index + 1] = (uint)num17;
                ulong num18 = (num17 >> 32) + (num15 * num3) + zz[index + 2];
                zz[index + 2] = (uint)num18;
                ulong num19 = (num18 >> 32) + (num15 * num4) + zz[index + 3];
                zz[index + 3] = (uint)num19;
                ulong num20 = (num19 >> 32) + (num15 * num5) + zz[index + 4];
                zz[index + 4] = (uint)num20;
                ulong num21 = num20 >> 32;
                zz[index + 5] = (uint)num21;
            }
        }

        public static void Mul( uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff )
        {
            ulong num1 = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = y[yOff + 4];
            ulong num6 = 0;
            ulong num7 = x[xOff];
            ulong num8 = num6 + (num7 * num1);
            zz[zzOff] = (uint)num8;
            ulong num9 = (num8 >> 32) + (num7 * num2);
            zz[zzOff + 1] = (uint)num9;
            ulong num10 = (num9 >> 32) + (num7 * num3);
            zz[zzOff + 2] = (uint)num10;
            ulong num11 = (num10 >> 32) + (num7 * num4);
            zz[zzOff + 3] = (uint)num11;
            ulong num12 = (num11 >> 32) + (num7 * num5);
            zz[zzOff + 4] = (uint)num12;
            ulong num13 = num12 >> 32;
            zz[zzOff + 5] = (uint)num13;
            for (int index = 1; index < 5; ++index)
            {
                ++zzOff;
                ulong num14 = 0;
                ulong num15 = x[xOff + index];
                ulong num16 = num14 + (num15 * num1) + zz[zzOff];
                zz[zzOff] = (uint)num16;
                ulong num17 = (num16 >> 32) + (num15 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint)num17;
                ulong num18 = (num17 >> 32) + (num15 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint)num18;
                ulong num19 = (num18 >> 32) + (num15 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint)num19;
                ulong num20 = (num19 >> 32) + (num15 * num5) + zz[zzOff + 4];
                zz[zzOff + 4] = (uint)num20;
                ulong num21 = num20 >> 32;
                zz[zzOff + 5] = (uint)num21;
            }
        }

        public static uint MulAddTo( uint[] x, uint[] y, uint[] zz )
        {
            ulong num1 = y[0];
            ulong num2 = y[1];
            ulong num3 = y[2];
            ulong num4 = y[3];
            ulong num5 = y[4];
            ulong num6 = 0;
            for (int index = 0; index < 5; ++index)
            {
                ulong num7 = 0;
                ulong num8 = x[index];
                ulong num9 = num7 + (num8 * num1) + zz[index];
                zz[index] = (uint)num9;
                ulong num10 = (num9 >> 32) + (num8 * num2) + zz[index + 1];
                zz[index + 1] = (uint)num10;
                ulong num11 = (num10 >> 32) + (num8 * num3) + zz[index + 2];
                zz[index + 2] = (uint)num11;
                ulong num12 = (num11 >> 32) + (num8 * num4) + zz[index + 3];
                zz[index + 3] = (uint)num12;
                ulong num13 = (num12 >> 32) + (num8 * num5) + zz[index + 4];
                zz[index + 4] = (uint)num13;
                ulong num14 = (num13 >> 32) + num6 + zz[index + 5];
                zz[index + 5] = (uint)num14;
                num6 = num14 >> 32;
            }
            return (uint)num6;
        }

        public static uint MulAddTo( uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff )
        {
            ulong num1 = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = y[yOff + 4];
            ulong num6 = 0;
            for (int index = 0; index < 5; ++index)
            {
                ulong num7 = 0;
                ulong num8 = x[xOff + index];
                ulong num9 = num7 + (num8 * num1) + zz[zzOff];
                zz[zzOff] = (uint)num9;
                ulong num10 = (num9 >> 32) + (num8 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint)num10;
                ulong num11 = (num10 >> 32) + (num8 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint)num11;
                ulong num12 = (num11 >> 32) + (num8 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint)num12;
                ulong num13 = (num12 >> 32) + (num8 * num5) + zz[zzOff + 4];
                zz[zzOff + 4] = (uint)num13;
                ulong num14 = (num13 >> 32) + num6 + zz[zzOff + 5];
                zz[zzOff + 5] = (uint)num14;
                num6 = num14 >> 32;
                ++zzOff;
            }
            return (uint)num6;
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
            ulong num14 = num13 >> 32;
            ulong num15 = x[xOff + 4];
            ulong num16 = num14 + (num2 * num15) + num12 + y[yOff + 4];
            z[zOff + 4] = (uint)num16;
            return (num16 >> 32) + num15;
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
            ulong num7 = (num6 >> 32) + (num2 * yy[yyOff + 4]) + zz[zzOff + 4];
            zz[zzOff + 4] = (uint)num7;
            return (uint)(num7 >> 32);
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
            return num9 >> 32 != 0UL ? Nat.IncAt( 5, z, zOff, 4 ) : 0U;
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
            return num5 >> 32 != 0UL ? Nat.IncAt( 5, z, zOff, 3 ) : 0U;
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
            return num5 >> 32 != 0UL ? Nat.IncAt( 5, z, zOff, 3 ) : 0U;
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
            return num5 >> 32 != 0UL ? Nat.IncAt( 5, z, zOff, 2 ) : 0U;
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
            while (++index < 5);
            return (uint)num1;
        }

        public static void Square( uint[] x, uint[] zz )
        {
            ulong num1 = x[0];
            uint num2 = 0;
            int num3 = 4;
            int num4 = 10;
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
            ulong num34 = num32 & uint.MaxValue;
            ulong num35 = num28 + (num33 >> 32);
            ulong num36 = num33 & uint.MaxValue;
            ulong num37 = x[4];
            ulong num38 = zz[7];
            ulong num39 = zz[8];
            ulong num40 = num34 + (num37 * num1);
            uint num41 = (uint)num40;
            zz[4] = (num41 << 1) | num31;
            uint num42 = num41 >> 31;
            ulong num43 = num36 + (num40 >> 32) + (num37 * num11);
            ulong num44 = num35 + (num43 >> 32) + (num37 * num17);
            ulong num45 = num38 + (num44 >> 32) + (num37 * num26);
            ulong num46 = num39 + (num45 >> 32);
            uint num47 = (uint)num43;
            zz[5] = (num47 << 1) | num42;
            uint num48 = num47 >> 31;
            uint num49 = (uint)num44;
            zz[6] = (num49 << 1) | num48;
            uint num50 = num49 >> 31;
            uint num51 = (uint)num45;
            zz[7] = (num51 << 1) | num50;
            uint num52 = num51 >> 31;
            uint num53 = (uint)num46;
            zz[8] = (num53 << 1) | num52;
            uint num54 = num53 >> 31;
            uint num55 = zz[9] + (uint)(num46 >> 32);
            zz[9] = (num55 << 1) | num54;
        }

        public static void Square( uint[] x, int xOff, uint[] zz, int zzOff )
        {
            ulong num1 = x[xOff];
            uint num2 = 0;
            int num3 = 4;
            int num4 = 10;
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
            ulong num34 = num32 & uint.MaxValue;
            ulong num35 = num28 + (num33 >> 32);
            ulong num36 = num33 & uint.MaxValue;
            ulong num37 = x[xOff + 4];
            ulong num38 = zz[zzOff + 7];
            ulong num39 = zz[zzOff + 8];
            ulong num40 = num34 + (num37 * num1);
            uint num41 = (uint)num40;
            zz[zzOff + 4] = (num41 << 1) | num31;
            uint num42 = num41 >> 31;
            ulong num43 = num36 + (num40 >> 32) + (num37 * num11);
            ulong num44 = num35 + (num43 >> 32) + (num37 * num17);
            ulong num45 = num38 + (num44 >> 32) + (num37 * num26);
            ulong num46 = num39 + (num45 >> 32);
            uint num47 = (uint)num43;
            zz[zzOff + 5] = (num47 << 1) | num42;
            uint num48 = num47 >> 31;
            uint num49 = (uint)num44;
            zz[zzOff + 6] = (num49 << 1) | num48;
            uint num50 = num49 >> 31;
            uint num51 = (uint)num45;
            zz[zzOff + 7] = (num51 << 1) | num50;
            uint num52 = num51 >> 31;
            uint num53 = (uint)num46;
            zz[zzOff + 8] = (num53 << 1) | num52;
            uint num54 = num53 >> 31;
            uint num55 = zz[zzOff + 9] + (uint)(num46 >> 32);
            zz[zzOff + 9] = (num55 << 1) | num54;
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
            long num5 = (num4 >> 32) + (x[4] - (long)y[4]);
            z[4] = (uint)num5;
            return (int)(num5 >> 32);
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
            long num5 = (num4 >> 32) + (x[xOff + 4] - (long)y[yOff + 4]);
            z[zOff + 4] = (uint)num5;
            return (int)(num5 >> 32);
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
            long num5 = (num4 >> 32) + (z[4] - (long)x[4] - y[4]);
            z[4] = (uint)num5;
            return (int)(num5 >> 32);
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
            long num5 = (num4 >> 32) + (z[4] - (long)x[4]);
            z[4] = (uint)num5;
            return (int)(num5 >> 32);
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
            long num5 = (num4 >> 32) + (z[zOff + 4] - (long)x[xOff + 4]);
            z[zOff + 4] = (uint)num5;
            return (int)(num5 >> 32);
        }

        public static BigInteger ToBigInteger( uint[] x )
        {
            byte[] numArray = new byte[20];
            for (int index = 0; index < 5; ++index)
            {
                uint n = x[index];
                if (n != 0U)
                    Pack.UInt32_To_BE( n, numArray, (4 - index) << 2 );
            }
            return new BigInteger( 1, numArray );
        }

        public static void Zero( uint[] z )
        {
            z[0] = 0U;
            z[1] = 0U;
            z[2] = 0U;
            z[3] = 0U;
            z[4] = 0U;
        }
    }
}
