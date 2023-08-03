// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat192
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat192
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
            ulong num6 = (num5 >> 32) + x[5] + y[5];
            z[5] = (uint)num6;
            return (uint)(num6 >> 32);
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
            ulong num6 = (num5 >> 32) + x[5] + y[5] + z[5];
            z[5] = (uint)num6;
            return (uint)(num6 >> 32);
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
            ulong num6 = (num5 >> 32) + x[5] + z[5];
            z[5] = (uint)num6;
            return (uint)(num6 >> 32);
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
            ulong num6 = (num5 >> 32) + x[xOff + 5] + z[zOff + 5];
            z[zOff + 5] = (uint)num6;
            return (uint)(num6 >> 32);
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
            ulong num6 = (num5 >> 32) + u[uOff + 5] + v[vOff + 5];
            u[uOff + 5] = (uint)num6;
            v[vOff + 5] = (uint)num6;
            return (uint)(num6 >> 32);
        }

        public static void Copy( uint[] x, uint[] z )
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
            z[5] = x[5];
        }

        public static void Copy64( ulong[] x, ulong[] z )
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
        }

        public static uint[] Create() => new uint[6];

        public static ulong[] Create64() => new ulong[3];

        public static uint[] CreateExt() => new uint[12];

        public static ulong[] CreateExt64() => new ulong[6];

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
            for (int index = 5; index >= 0; --index)
            {
                if ((int)x[index] != (int)y[index])
                    return false;
            }
            return true;
        }

        public static bool Eq64( ulong[] x, ulong[] y )
        {
            for (int index = 2; index >= 0; --index)
            {
                if ((long)x[index] != (long)y[index])
                    return false;
            }
            return true;
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > 192)
                throw new ArgumentException();
            uint[] numArray = Create();
            int num = 0;
            for (; x.SignValue != 0; x = x.ShiftRight( 32 ))
                numArray[num++] = (uint)x.IntValue;
            return numArray;
        }

        public static ulong[] FromBigInteger64( BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > 192)
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
            int index = bit >> 5;
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    int num = bit & 31;
                    return (x[index] >> num) & 1U;
                default:
                    return 0;
            }
        }

        public static bool Gte( uint[] x, uint[] y )
        {
            for (int index = 5; index >= 0; --index)
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
            for (int index = 5; index >= 0; --index)
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
            for (int index = 1; index < 6; ++index)
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
            for (int index = 1; index < 3; ++index)
            {
                if (x[index] != 0UL)
                    return false;
            }
            return true;
        }

        public static bool IsZero( uint[] x )
        {
            for (int index = 0; index < 6; ++index)
            {
                if (x[index] != 0U)
                    return false;
            }
            return true;
        }

        public static bool IsZero64( ulong[] x )
        {
            for (int index = 0; index < 3; ++index)
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
            ulong num5 = y[4];
            ulong num6 = y[5];
            ulong num7 = 0;
            ulong num8 = x[0];
            ulong num9 = num7 + (num8 * num1);
            zz[0] = (uint)num9;
            ulong num10 = (num9 >> 32) + (num8 * num2);
            zz[1] = (uint)num10;
            ulong num11 = (num10 >> 32) + (num8 * num3);
            zz[2] = (uint)num11;
            ulong num12 = (num11 >> 32) + (num8 * num4);
            zz[3] = (uint)num12;
            ulong num13 = (num12 >> 32) + (num8 * num5);
            zz[4] = (uint)num13;
            ulong num14 = (num13 >> 32) + (num8 * num6);
            zz[5] = (uint)num14;
            ulong num15 = num14 >> 32;
            zz[6] = (uint)num15;
            for (int index = 1; index < 6; ++index)
            {
                ulong num16 = 0;
                ulong num17 = x[index];
                ulong num18 = num16 + (num17 * num1) + zz[index];
                zz[index] = (uint)num18;
                ulong num19 = (num18 >> 32) + (num17 * num2) + zz[index + 1];
                zz[index + 1] = (uint)num19;
                ulong num20 = (num19 >> 32) + (num17 * num3) + zz[index + 2];
                zz[index + 2] = (uint)num20;
                ulong num21 = (num20 >> 32) + (num17 * num4) + zz[index + 3];
                zz[index + 3] = (uint)num21;
                ulong num22 = (num21 >> 32) + (num17 * num5) + zz[index + 4];
                zz[index + 4] = (uint)num22;
                ulong num23 = (num22 >> 32) + (num17 * num6) + zz[index + 5];
                zz[index + 5] = (uint)num23;
                ulong num24 = num23 >> 32;
                zz[index + 6] = (uint)num24;
            }
        }

        public static void Mul( uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff )
        {
            ulong num1 = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = y[yOff + 4];
            ulong num6 = y[yOff + 5];
            ulong num7 = 0;
            ulong num8 = x[xOff];
            ulong num9 = num7 + (num8 * num1);
            zz[zzOff] = (uint)num9;
            ulong num10 = (num9 >> 32) + (num8 * num2);
            zz[zzOff + 1] = (uint)num10;
            ulong num11 = (num10 >> 32) + (num8 * num3);
            zz[zzOff + 2] = (uint)num11;
            ulong num12 = (num11 >> 32) + (num8 * num4);
            zz[zzOff + 3] = (uint)num12;
            ulong num13 = (num12 >> 32) + (num8 * num5);
            zz[zzOff + 4] = (uint)num13;
            ulong num14 = (num13 >> 32) + (num8 * num6);
            zz[zzOff + 5] = (uint)num14;
            ulong num15 = num14 >> 32;
            zz[zzOff + 6] = (uint)num15;
            for (int index = 1; index < 6; ++index)
            {
                ++zzOff;
                ulong num16 = 0;
                ulong num17 = x[xOff + index];
                ulong num18 = num16 + (num17 * num1) + zz[zzOff];
                zz[zzOff] = (uint)num18;
                ulong num19 = (num18 >> 32) + (num17 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint)num19;
                ulong num20 = (num19 >> 32) + (num17 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint)num20;
                ulong num21 = (num20 >> 32) + (num17 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint)num21;
                ulong num22 = (num21 >> 32) + (num17 * num5) + zz[zzOff + 4];
                zz[zzOff + 4] = (uint)num22;
                ulong num23 = (num22 >> 32) + (num17 * num6) + zz[zzOff + 5];
                zz[zzOff + 5] = (uint)num23;
                ulong num24 = num23 >> 32;
                zz[zzOff + 6] = (uint)num24;
            }
        }

        public static uint MulAddTo( uint[] x, uint[] y, uint[] zz )
        {
            ulong num1 = y[0];
            ulong num2 = y[1];
            ulong num3 = y[2];
            ulong num4 = y[3];
            ulong num5 = y[4];
            ulong num6 = y[5];
            ulong num7 = 0;
            for (int index = 0; index < 6; ++index)
            {
                ulong num8 = 0;
                ulong num9 = x[index];
                ulong num10 = num8 + (num9 * num1) + zz[index];
                zz[index] = (uint)num10;
                ulong num11 = (num10 >> 32) + (num9 * num2) + zz[index + 1];
                zz[index + 1] = (uint)num11;
                ulong num12 = (num11 >> 32) + (num9 * num3) + zz[index + 2];
                zz[index + 2] = (uint)num12;
                ulong num13 = (num12 >> 32) + (num9 * num4) + zz[index + 3];
                zz[index + 3] = (uint)num13;
                ulong num14 = (num13 >> 32) + (num9 * num5) + zz[index + 4];
                zz[index + 4] = (uint)num14;
                ulong num15 = (num14 >> 32) + (num9 * num6) + zz[index + 5];
                zz[index + 5] = (uint)num15;
                ulong num16 = (num15 >> 32) + num7 + zz[index + 6];
                zz[index + 6] = (uint)num16;
                num7 = num16 >> 32;
            }
            return (uint)num7;
        }

        public static uint MulAddTo( uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff )
        {
            ulong num1 = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = y[yOff + 4];
            ulong num6 = y[yOff + 5];
            ulong num7 = 0;
            for (int index = 0; index < 6; ++index)
            {
                ulong num8 = 0;
                ulong num9 = x[xOff + index];
                ulong num10 = num8 + (num9 * num1) + zz[zzOff];
                zz[zzOff] = (uint)num10;
                ulong num11 = (num10 >> 32) + (num9 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint)num11;
                ulong num12 = (num11 >> 32) + (num9 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint)num12;
                ulong num13 = (num12 >> 32) + (num9 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint)num13;
                ulong num14 = (num13 >> 32) + (num9 * num5) + zz[zzOff + 4];
                zz[zzOff + 4] = (uint)num14;
                ulong num15 = (num14 >> 32) + (num9 * num6) + zz[zzOff + 5];
                zz[zzOff + 5] = (uint)num15;
                ulong num16 = (num15 >> 32) + num7 + zz[zzOff + 6];
                zz[zzOff + 6] = (uint)num16;
                num7 = num16 >> 32;
                ++zzOff;
            }
            return (uint)num7;
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
            ulong num17 = num16 >> 32;
            ulong num18 = x[xOff + 5];
            ulong num19 = num17 + (num2 * num18) + num15 + y[yOff + 5];
            z[zOff + 5] = (uint)num19;
            return (num19 >> 32) + num18;
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
            ulong num8 = (num7 >> 32) + (num2 * yy[yyOff + 5]) + zz[zzOff + 5];
            zz[zzOff + 5] = (uint)num8;
            return (uint)(num8 >> 32);
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
            return num9 >> 32 != 0UL ? Nat.IncAt( 6, z, zOff, 4 ) : 0U;
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
            return num5 >> 32 != 0UL ? Nat.IncAt( 6, z, zOff, 3 ) : 0U;
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
            return num5 >> 32 != 0UL ? Nat.IncAt( 6, z, zOff, 3 ) : 0U;
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
            while (++index < 6);
            return (uint)num1;
        }

        public static void Square( uint[] x, uint[] zz )
        {
            ulong num1 = x[0];
            uint num2 = 0;
            int num3 = 5;
            int num4 = 12;
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
            ulong num45 = num43 & uint.MaxValue;
            ulong num46 = num38 + (num44 >> 32) + (num37 * num26);
            ulong num47 = num44 & uint.MaxValue;
            ulong num48 = num39 + (num46 >> 32);
            ulong num49 = num46 & uint.MaxValue;
            ulong num50 = x[5];
            ulong num51 = zz[9];
            ulong num52 = zz[10];
            ulong num53 = num45 + (num50 * num1);
            uint num54 = (uint)num53;
            zz[5] = (num54 << 1) | num42;
            uint num55 = num54 >> 31;
            ulong num56 = num47 + (num53 >> 32) + (num50 * num11);
            ulong num57 = num49 + (num56 >> 32) + (num50 * num17);
            ulong num58 = num48 + (num57 >> 32) + (num50 * num26);
            ulong num59 = num51 + (num58 >> 32) + (num50 * num37);
            ulong num60 = num52 + (num59 >> 32);
            uint num61 = (uint)num56;
            zz[6] = (num61 << 1) | num55;
            uint num62 = num61 >> 31;
            uint num63 = (uint)num57;
            zz[7] = (num63 << 1) | num62;
            uint num64 = num63 >> 31;
            uint num65 = (uint)num58;
            zz[8] = (num65 << 1) | num64;
            uint num66 = num65 >> 31;
            uint num67 = (uint)num59;
            zz[9] = (num67 << 1) | num66;
            uint num68 = num67 >> 31;
            uint num69 = (uint)num60;
            zz[10] = (num69 << 1) | num68;
            uint num70 = num69 >> 31;
            uint num71 = zz[11] + (uint)(num60 >> 32);
            zz[11] = (num71 << 1) | num70;
        }

        public static void Square( uint[] x, int xOff, uint[] zz, int zzOff )
        {
            ulong num1 = x[xOff];
            uint num2 = 0;
            int num3 = 5;
            int num4 = 12;
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
            ulong num45 = num43 & uint.MaxValue;
            ulong num46 = num38 + (num44 >> 32) + (num37 * num26);
            ulong num47 = num44 & uint.MaxValue;
            ulong num48 = num39 + (num46 >> 32);
            ulong num49 = num46 & uint.MaxValue;
            ulong num50 = x[xOff + 5];
            ulong num51 = zz[zzOff + 9];
            ulong num52 = zz[zzOff + 10];
            ulong num53 = num45 + (num50 * num1);
            uint num54 = (uint)num53;
            zz[zzOff + 5] = (num54 << 1) | num42;
            uint num55 = num54 >> 31;
            ulong num56 = num47 + (num53 >> 32) + (num50 * num11);
            ulong num57 = num49 + (num56 >> 32) + (num50 * num17);
            ulong num58 = num48 + (num57 >> 32) + (num50 * num26);
            ulong num59 = num51 + (num58 >> 32) + (num50 * num37);
            ulong num60 = num52 + (num59 >> 32);
            uint num61 = (uint)num56;
            zz[zzOff + 6] = (num61 << 1) | num55;
            uint num62 = num61 >> 31;
            uint num63 = (uint)num57;
            zz[zzOff + 7] = (num63 << 1) | num62;
            uint num64 = num63 >> 31;
            uint num65 = (uint)num58;
            zz[zzOff + 8] = (num65 << 1) | num64;
            uint num66 = num65 >> 31;
            uint num67 = (uint)num59;
            zz[zzOff + 9] = (num67 << 1) | num66;
            uint num68 = num67 >> 31;
            uint num69 = (uint)num60;
            zz[zzOff + 10] = (num69 << 1) | num68;
            uint num70 = num69 >> 31;
            uint num71 = zz[zzOff + 11] + (uint)(num60 >> 32);
            zz[zzOff + 11] = (num71 << 1) | num70;
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
            long num6 = (num5 >> 32) + (x[5] - (long)y[5]);
            z[5] = (uint)num6;
            return (int)(num6 >> 32);
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
            long num6 = (num5 >> 32) + (x[xOff + 5] - (long)y[yOff + 5]);
            z[zOff + 5] = (uint)num6;
            return (int)(num6 >> 32);
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
            long num6 = (num5 >> 32) + (z[5] - (long)x[5] - y[5]);
            z[5] = (uint)num6;
            return (int)(num6 >> 32);
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
            long num6 = (num5 >> 32) + (z[5] - (long)x[5]);
            z[5] = (uint)num6;
            return (int)(num6 >> 32);
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
            long num6 = (num5 >> 32) + (z[zOff + 5] - (long)x[xOff + 5]);
            z[zOff + 5] = (uint)num6;
            return (int)(num6 >> 32);
        }

        public static BigInteger ToBigInteger( uint[] x )
        {
            byte[] numArray = new byte[24];
            for (int index = 0; index < 6; ++index)
            {
                uint n = x[index];
                if (n != 0U)
                    Pack.UInt32_To_BE( n, numArray, (5 - index) << 2 );
            }
            return new BigInteger( 1, numArray );
        }

        public static BigInteger ToBigInteger64( ulong[] x )
        {
            byte[] numArray = new byte[24];
            for (int index = 0; index < 3; ++index)
            {
                ulong n = x[index];
                if (n != 0UL)
                    Pack.UInt64_To_BE( n, numArray, (2 - index) << 3 );
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
            z[5] = 0U;
        }
    }
}
