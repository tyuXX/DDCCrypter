﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.Adler32
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.Zlib
{
    internal sealed class Adler32
    {
        private const int BASE = 65521;
        private const int NMAX = 5552;

        internal long adler32( long adler, byte[] buf, int index, int len )
        {
            if (buf == null)
                return 1;
            long num1 = adler & ushort.MaxValue;
            long num2 = (adler >> 16) & ushort.MaxValue;
            while (len > 0)
            {
                int num3 = len < 5552 ? len : 5552;
                len -= num3;
                for (; num3 >= 16; num3 -= 16)
                {
                    long num4 = num1 + (buf[index++] & byte.MaxValue);
                    long num5 = num2 + num4;
                    long num6 = num4 + (buf[index++] & byte.MaxValue);
                    long num7 = num5 + num6;
                    long num8 = num6 + (buf[index++] & byte.MaxValue);
                    long num9 = num7 + num8;
                    long num10 = num8 + (buf[index++] & byte.MaxValue);
                    long num11 = num9 + num10;
                    long num12 = num10 + (buf[index++] & byte.MaxValue);
                    long num13 = num11 + num12;
                    long num14 = num12 + (buf[index++] & byte.MaxValue);
                    long num15 = num13 + num14;
                    long num16 = num14 + (buf[index++] & byte.MaxValue);
                    long num17 = num15 + num16;
                    long num18 = num16 + (buf[index++] & byte.MaxValue);
                    long num19 = num17 + num18;
                    long num20 = num18 + (buf[index++] & byte.MaxValue);
                    long num21 = num19 + num20;
                    long num22 = num20 + (buf[index++] & byte.MaxValue);
                    long num23 = num21 + num22;
                    long num24 = num22 + (buf[index++] & byte.MaxValue);
                    long num25 = num23 + num24;
                    long num26 = num24 + (buf[index++] & byte.MaxValue);
                    long num27 = num25 + num26;
                    long num28 = num26 + (buf[index++] & byte.MaxValue);
                    long num29 = num27 + num28;
                    long num30 = num28 + (buf[index++] & byte.MaxValue);
                    long num31 = num29 + num30;
                    long num32 = num30 + (buf[index++] & byte.MaxValue);
                    long num33 = num31 + num32;
                    num1 = num32 + (buf[index++] & byte.MaxValue);
                    num2 = num33 + num1;
                }
                if (num3 != 0)
                {
                    do
                    {
                        num1 += buf[index++] & byte.MaxValue;
                        num2 += num1;
                    }
                    while (--num3 != 0);
                }
                num1 %= 65521L;
                num2 %= 65521L;
            }
            return (num2 << 16) | num1;
        }
    }
}
