// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.InfCodes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.Zlib
{
    internal sealed class InfCodes
    {
        private const int Z_OK = 0;
        private const int Z_STREAM_END = 1;
        private const int Z_NEED_DICT = 2;
        private const int Z_ERRNO = -1;
        private const int Z_STREAM_ERROR = -2;
        private const int Z_DATA_ERROR = -3;
        private const int Z_MEM_ERROR = -4;
        private const int Z_BUF_ERROR = -5;
        private const int Z_VERSION_ERROR = -6;
        private const int START = 0;
        private const int LEN = 1;
        private const int LENEXT = 2;
        private const int DIST = 3;
        private const int DISTEXT = 4;
        private const int COPY = 5;
        private const int LIT = 6;
        private const int WASH = 7;
        private const int END = 8;
        private const int BADCODE = 9;
        private static readonly int[] inflate_mask = new int[17]
        {
      0,
      1,
      3,
      7,
      15,
      31,
      63,
       sbyte.MaxValue,
       byte.MaxValue,
      511,
      1023,
      2047,
      4095,
      8191,
      16383,
       short.MaxValue,
       ushort.MaxValue
        };
        private int mode;
        private int len;
        private int[] tree;
        private int tree_index = 0;
        private int need;
        private int lit;
        private int get;
        private int dist;
        private byte lbits;
        private byte dbits;
        private int[] ltree;
        private int ltree_index;
        private int[] dtree;
        private int dtree_index;

        internal InfCodes()
        {
        }

        internal void init(
          int bl,
          int bd,
          int[] tl,
          int tl_index,
          int[] td,
          int td_index,
          ZStream z )
        {
            this.mode = 0;
            this.lbits = (byte)bl;
            this.dbits = (byte)bd;
            this.ltree = tl;
            this.ltree_index = tl_index;
            this.dtree = td;
            this.dtree_index = td_index;
            this.tree = null;
        }

        internal int proc( InfBlocks s, ZStream z, int r )
        {
            int nextInIndex = z.next_in_index;
            int availIn = z.avail_in;
            int bitb = s.bitb;
            int bitk = s.bitk;
            int num1 = s.write;
            int num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
            while (true)
            {
                switch (this.mode)
                {
                    case 0:
                        if (num2 >= 258 && availIn >= 10)
                        {
                            s.bitb = bitb;
                            s.bitk = bitk;
                            z.avail_in = availIn;
                            z.total_in += nextInIndex - z.next_in_index;
                            z.next_in_index = nextInIndex;
                            s.write = num1;
                            r = this.inflate_fast( lbits, dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, s, z );
                            nextInIndex = z.next_in_index;
                            availIn = z.avail_in;
                            bitb = s.bitb;
                            bitk = s.bitk;
                            num1 = s.write;
                            num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                            int num3;
                            switch (r)
                            {
                                case 0:
                                    goto label_7;
                                case 1:
                                    num3 = 7;
                                    break;
                                default:
                                    num3 = 9;
                                    break;
                            }
                            this.mode = num3;
                            continue;
                        }
                    label_7:
                        this.need = lbits;
                        this.tree = this.ltree;
                        this.tree_index = this.ltree_index;
                        this.mode = 1;
                        goto case 1;
                    case 1:
                        int need1;
                        for (need1 = this.need; bitk < need1; bitk += 8)
                        {
                            if (availIn != 0)
                            {
                                r = 0;
                                --availIn;
                                bitb |= (z.next_in[nextInIndex++] & byte.MaxValue) << bitk;
                            }
                            else
                            {
                                s.bitb = bitb;
                                s.bitk = bitk;
                                z.avail_in = availIn;
                                z.total_in += nextInIndex - z.next_in_index;
                                z.next_in_index = nextInIndex;
                                s.write = num1;
                                return s.inflate_flush( z, r );
                            }
                        }
                        int index1 = (this.tree_index + (bitb & inflate_mask[need1])) * 3;
                        bitb >>= this.tree[index1 + 1];
                        bitk -= this.tree[index1 + 1];
                        int num4 = this.tree[index1];
                        if (num4 == 0)
                        {
                            this.lit = this.tree[index1 + 2];
                            this.mode = 6;
                            continue;
                        }
                        if ((num4 & 16) != 0)
                        {
                            this.get = num4 & 15;
                            this.len = this.tree[index1 + 2];
                            this.mode = 2;
                            continue;
                        }
                        if ((num4 & 64) == 0)
                        {
                            this.need = num4;
                            this.tree_index = (index1 / 3) + this.tree[index1 + 2];
                            continue;
                        }
                        if ((num4 & 32) != 0)
                        {
                            this.mode = 7;
                            continue;
                        }
                        goto label_21;
                    case 2:
                        int get1;
                        for (get1 = this.get; bitk < get1; bitk += 8)
                        {
                            if (availIn != 0)
                            {
                                r = 0;
                                --availIn;
                                bitb |= (z.next_in[nextInIndex++] & byte.MaxValue) << bitk;
                            }
                            else
                            {
                                s.bitb = bitb;
                                s.bitk = bitk;
                                z.avail_in = availIn;
                                z.total_in += nextInIndex - z.next_in_index;
                                z.next_in_index = nextInIndex;
                                s.write = num1;
                                return s.inflate_flush( z, r );
                            }
                        }
                        this.len += bitb & inflate_mask[get1];
                        bitb >>= get1;
                        bitk -= get1;
                        this.need = dbits;
                        this.tree = this.dtree;
                        this.tree_index = this.dtree_index;
                        this.mode = 3;
                        goto case 3;
                    case 3:
                        int need2;
                        for (need2 = this.need; bitk < need2; bitk += 8)
                        {
                            if (availIn != 0)
                            {
                                r = 0;
                                --availIn;
                                bitb |= (z.next_in[nextInIndex++] & byte.MaxValue) << bitk;
                            }
                            else
                            {
                                s.bitb = bitb;
                                s.bitk = bitk;
                                z.avail_in = availIn;
                                z.total_in += nextInIndex - z.next_in_index;
                                z.next_in_index = nextInIndex;
                                s.write = num1;
                                return s.inflate_flush( z, r );
                            }
                        }
                        int index2 = (this.tree_index + (bitb & inflate_mask[need2])) * 3;
                        bitb >>= this.tree[index2 + 1];
                        bitk -= this.tree[index2 + 1];
                        int num5 = this.tree[index2];
                        if ((num5 & 16) != 0)
                        {
                            this.get = num5 & 15;
                            this.dist = this.tree[index2 + 2];
                            this.mode = 4;
                            continue;
                        }
                        if ((num5 & 64) == 0)
                        {
                            this.need = num5;
                            this.tree_index = (index2 / 3) + this.tree[index2 + 2];
                            continue;
                        }
                        goto label_37;
                    case 4:
                        int get2;
                        for (get2 = this.get; bitk < get2; bitk += 8)
                        {
                            if (availIn != 0)
                            {
                                r = 0;
                                --availIn;
                                bitb |= (z.next_in[nextInIndex++] & byte.MaxValue) << bitk;
                            }
                            else
                            {
                                s.bitb = bitb;
                                s.bitk = bitk;
                                z.avail_in = availIn;
                                z.total_in += nextInIndex - z.next_in_index;
                                z.next_in_index = nextInIndex;
                                s.write = num1;
                                return s.inflate_flush( z, r );
                            }
                        }
                        this.dist += bitb & inflate_mask[get2];
                        bitb >>= get2;
                        bitk -= get2;
                        this.mode = 5;
                        goto case 5;
                    case 5:
                        int num6 = num1 - this.dist;
                        while (num6 < 0)
                            num6 += s.end;
                        for (; this.len != 0; --this.len)
                        {
                            if (num2 == 0)
                            {
                                if (num1 == s.end && s.read != 0)
                                {
                                    num1 = 0;
                                    num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                                }
                                if (num2 == 0)
                                {
                                    s.write = num1;
                                    r = s.inflate_flush( z, r );
                                    num1 = s.write;
                                    num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                                    if (num1 == s.end && s.read != 0)
                                    {
                                        num1 = 0;
                                        num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                                    }
                                    if (num2 == 0)
                                    {
                                        s.bitb = bitb;
                                        s.bitk = bitk;
                                        z.avail_in = availIn;
                                        z.total_in += nextInIndex - z.next_in_index;
                                        z.next_in_index = nextInIndex;
                                        s.write = num1;
                                        return s.inflate_flush( z, r );
                                    }
                                }
                            }
                            s.window[num1++] = s.window[num6++];
                            --num2;
                            if (num6 == s.end)
                                num6 = 0;
                        }
                        this.mode = 0;
                        continue;
                    case 6:
                        if (num2 == 0)
                        {
                            if (num1 == s.end && s.read != 0)
                            {
                                num1 = 0;
                                num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                            }
                            if (num2 == 0)
                            {
                                s.write = num1;
                                r = s.inflate_flush( z, r );
                                num1 = s.write;
                                num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                                if (num1 == s.end && s.read != 0)
                                {
                                    num1 = 0;
                                    num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                                }
                                if (num2 == 0)
                                    goto label_67;
                            }
                        }
                        r = 0;
                        s.window[num1++] = (byte)this.lit;
                        --num2;
                        this.mode = 0;
                        continue;
                    case 7:
                        goto label_69;
                    case 8:
                        goto label_74;
                    case 9:
                        goto label_75;
                    default:
                        goto label_76;
                }
            }
        label_21:
            this.mode = 9;
            z.msg = "invalid literal/length code";
            r = -3;
            s.bitb = bitb;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += nextInIndex - z.next_in_index;
            z.next_in_index = nextInIndex;
            s.write = num1;
            return s.inflate_flush( z, r );
        label_37:
            this.mode = 9;
            z.msg = "invalid distance code";
            r = -3;
            s.bitb = bitb;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += nextInIndex - z.next_in_index;
            z.next_in_index = nextInIndex;
            s.write = num1;
            return s.inflate_flush( z, r );
        label_67:
            s.bitb = bitb;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += nextInIndex - z.next_in_index;
            z.next_in_index = nextInIndex;
            s.write = num1;
            return s.inflate_flush( z, r );
        label_69:
            if (bitk > 7)
            {
                bitk -= 8;
                ++availIn;
                --nextInIndex;
            }
            s.write = num1;
            r = s.inflate_flush( z, r );
            num1 = s.write;
            int num7 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
            if (s.read != s.write)
            {
                s.bitb = bitb;
                s.bitk = bitk;
                z.avail_in = availIn;
                z.total_in += nextInIndex - z.next_in_index;
                z.next_in_index = nextInIndex;
                s.write = num1;
                return s.inflate_flush( z, r );
            }
            this.mode = 8;
        label_74:
            r = 1;
            s.bitb = bitb;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += nextInIndex - z.next_in_index;
            z.next_in_index = nextInIndex;
            s.write = num1;
            return s.inflate_flush( z, r );
        label_75:
            r = -3;
            s.bitb = bitb;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += nextInIndex - z.next_in_index;
            z.next_in_index = nextInIndex;
            s.write = num1;
            return s.inflate_flush( z, r );
        label_76:
            r = -2;
            s.bitb = bitb;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += nextInIndex - z.next_in_index;
            z.next_in_index = nextInIndex;
            s.write = num1;
            return s.inflate_flush( z, r );
        }

        internal void free( ZStream z )
        {
        }

        internal int inflate_fast(
          int bl,
          int bd,
          int[] tl,
          int tl_index,
          int[] td,
          int td_index,
          InfBlocks s,
          ZStream z )
        {
            int nextInIndex = z.next_in_index;
            int availIn = z.avail_in;
            int num1 = s.bitb;
            int num2 = s.bitk;
            int destinationIndex = s.write;
            int num3 = destinationIndex < s.read ? s.read - destinationIndex - 1 : s.end - destinationIndex;
            int num4 = inflate_mask[bl];
            int num5 = inflate_mask[bd];
            do
            {
                for (; num2 < 20; num2 += 8)
                {
                    --availIn;
                    num1 |= (z.next_in[nextInIndex++] & byte.MaxValue) << num2;
                }
                int num6 = num1 & num4;
                int[] numArray1 = tl;
                int num7 = tl_index;
                int index1 = (num7 + num6) * 3;
                int index2;
                if ((index2 = numArray1[index1]) == 0)
                {
                    num1 >>= numArray1[index1 + 1];
                    num2 -= numArray1[index1 + 1];
                    s.window[destinationIndex++] = (byte)numArray1[index1 + 2];
                    --num3;
                }
                else
                {
                    do
                    {
                        num1 >>= numArray1[index1 + 1];
                        num2 -= numArray1[index1 + 1];
                        if ((index2 & 16) != 0)
                        {
                            int index3 = index2 & 15;
                            int length1 = numArray1[index1 + 2] + (num1 & inflate_mask[index3]);
                            int num8 = num1 >> index3;
                            int num9;
                            for (num9 = num2 - index3; num9 < 15; num9 += 8)
                            {
                                --availIn;
                                num8 |= (z.next_in[nextInIndex++] & byte.MaxValue) << num9;
                            }
                            int num10 = num8 & num5;
                            int[] numArray2 = td;
                            int num11 = td_index;
                            int index4 = (num11 + num10) * 3;
                            int index5 = numArray2[index4];
                            while (true)
                            {
                                num8 >>= numArray2[index4 + 1];
                                num9 -= numArray2[index4 + 1];
                                if ((index5 & 16) == 0)
                                {
                                    if ((index5 & 64) == 0)
                                    {
                                        num10 = num10 + numArray2[index4 + 2] + (num8 & inflate_mask[index5]);
                                        index4 = (num11 + num10) * 3;
                                        index5 = numArray2[index4];
                                    }
                                    else
                                        goto label_30;
                                }
                                else
                                    break;
                            }
                            int index6;
                            for (index6 = index5 & 15; num9 < index6; num9 += 8)
                            {
                                --availIn;
                                num8 |= (z.next_in[nextInIndex++] & byte.MaxValue) << num9;
                            }
                            int num12 = numArray2[index4 + 2] + (num8 & inflate_mask[index6]);
                            num1 = num8 >> index6;
                            num2 = num9 - index6;
                            num3 -= length1;
                            int sourceIndex1;
                            int num13;
                            if (destinationIndex >= num12)
                            {
                                int sourceIndex2 = destinationIndex - num12;
                                if (destinationIndex - sourceIndex2 > 0 && 2 > destinationIndex - sourceIndex2)
                                {
                                    byte[] window1 = s.window;
                                    int index7 = destinationIndex;
                                    int num14 = index7 + 1;
                                    byte[] window2 = s.window;
                                    int index8 = sourceIndex2;
                                    int num15 = index8 + 1;
                                    int num16 = window2[index8];
                                    window1[index7] = (byte)num16;
                                    byte[] window3 = s.window;
                                    int index9 = num14;
                                    destinationIndex = index9 + 1;
                                    byte[] window4 = s.window;
                                    int index10 = num15;
                                    sourceIndex1 = index10 + 1;
                                    int num17 = window4[index10];
                                    window3[index9] = (byte)num17;
                                    length1 -= 2;
                                }
                                else
                                {
                                    Array.Copy( s.window, sourceIndex2, s.window, destinationIndex, 2 );
                                    destinationIndex += 2;
                                    sourceIndex1 = sourceIndex2 + 2;
                                    length1 -= 2;
                                }
                            }
                            else
                            {
                                sourceIndex1 = destinationIndex - num12;
                                do
                                {
                                    sourceIndex1 += s.end;
                                }
                                while (sourceIndex1 < 0);
                                int length2 = s.end - sourceIndex1;
                                if (length1 > length2)
                                {
                                    length1 -= length2;
                                    if (destinationIndex - sourceIndex1 > 0 && length2 > destinationIndex - sourceIndex1)
                                    {
                                        do
                                        {
                                            s.window[destinationIndex++] = s.window[sourceIndex1++];
                                        }
                                        while (--length2 != 0);
                                    }
                                    else
                                    {
                                        Array.Copy( s.window, sourceIndex1, s.window, destinationIndex, length2 );
                                        destinationIndex += length2;
                                        num13 = sourceIndex1 + length2;
                                    }
                                    sourceIndex1 = 0;
                                }
                            }
                            if (destinationIndex - sourceIndex1 > 0 && length1 > destinationIndex - sourceIndex1)
                            {
                                do
                                {
                                    s.window[destinationIndex++] = s.window[sourceIndex1++];
                                }
                                while (--length1 != 0);
                                goto label_37;
                            }
                            else
                            {
                                Array.Copy( s.window, sourceIndex1, s.window, destinationIndex, length1 );
                                destinationIndex += length1;
                                num13 = sourceIndex1 + length1;
                                goto label_37;
                            }
                        label_30:
                            z.msg = "invalid distance code";
                            int num18 = z.avail_in - availIn;
                            int num19 = num9 >> 3 < num18 ? num9 >> 3 : num18;
                            int num20 = availIn + num19;
                            int num21 = nextInIndex - num19;
                            int num22 = num9 - (num19 << 3);
                            s.bitb = num8;
                            s.bitk = num22;
                            z.avail_in = num20;
                            z.total_in += num21 - z.next_in_index;
                            z.next_in_index = num21;
                            s.write = destinationIndex;
                            return -3;
                        }
                        if ((index2 & 64) == 0)
                        {
                            num6 = num6 + numArray1[index1 + 2] + (num1 & inflate_mask[index2]);
                            index1 = (num7 + num6) * 3;
                        }
                        else
                            goto label_34;
                    }
                    while ((index2 = numArray1[index1]) != 0);
                    num1 >>= numArray1[index1 + 1];
                    num2 -= numArray1[index1 + 1];
                    s.window[destinationIndex++] = (byte)numArray1[index1 + 2];
                    --num3;
                    goto label_37;
                label_34:
                    if ((index2 & 32) != 0)
                    {
                        int num23 = z.avail_in - availIn;
                        int num24 = num2 >> 3 < num23 ? num2 >> 3 : num23;
                        int num25 = availIn + num24;
                        int num26 = nextInIndex - num24;
                        int num27 = num2 - (num24 << 3);
                        s.bitb = num1;
                        s.bitk = num27;
                        z.avail_in = num25;
                        z.total_in += num26 - z.next_in_index;
                        z.next_in_index = num26;
                        s.write = destinationIndex;
                        return 1;
                    }
                    z.msg = "invalid literal/length code";
                    int num28 = z.avail_in - availIn;
                    int num29 = num2 >> 3 < num28 ? num2 >> 3 : num28;
                    int num30 = availIn + num29;
                    int num31 = nextInIndex - num29;
                    int num32 = num2 - (num29 << 3);
                    s.bitb = num1;
                    s.bitk = num32;
                    z.avail_in = num30;
                    z.total_in += num31 - z.next_in_index;
                    z.next_in_index = num31;
                    s.write = destinationIndex;
                    return -3;
                }
            label_37:;
            }
            while (num3 >= 258 && availIn >= 10);
            int num33 = z.avail_in - availIn;
            int num34 = num2 >> 3 < num33 ? num2 >> 3 : num33;
            int num35 = availIn + num34;
            int num36 = nextInIndex - num34;
            int num37 = num2 - (num34 << 3);
            s.bitb = num1;
            s.bitk = num37;
            z.avail_in = num35;
            z.total_in += num36 - z.next_in_index;
            z.next_in_index = num36;
            s.write = destinationIndex;
            return 0;
        }
    }
}
