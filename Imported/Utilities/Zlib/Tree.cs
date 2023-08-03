// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.Tree
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Utilities.Zlib
{
    internal sealed class Tree
    {
        private const int MAX_BITS = 15;
        private const int BL_CODES = 19;
        private const int D_CODES = 30;
        private const int LITERALS = 256;
        private const int LENGTH_CODES = 29;
        private const int L_CODES = 286;
        private const int HEAP_SIZE = 573;
        internal const int MAX_BL_BITS = 7;
        internal const int END_BLOCK = 256;
        internal const int REP_3_6 = 16;
        internal const int REPZ_3_10 = 17;
        internal const int REPZ_11_138 = 18;
        internal const int Buf_size = 16;
        internal const int DIST_CODE_LEN = 512;
        internal static readonly int[] extra_lbits = new int[29]
        {
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      1,
      1,
      1,
      1,
      2,
      2,
      2,
      2,
      3,
      3,
      3,
      3,
      4,
      4,
      4,
      4,
      5,
      5,
      5,
      5,
      0
        };
        internal static readonly int[] extra_dbits = new int[30]
        {
      0,
      0,
      0,
      0,
      1,
      1,
      2,
      2,
      3,
      3,
      4,
      4,
      5,
      5,
      6,
      6,
      7,
      7,
      8,
      8,
      9,
      9,
      10,
      10,
      11,
      11,
      12,
      12,
      13,
      13
        };
        internal static readonly int[] extra_blbits = new int[19]
        {
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      2,
      3,
      7
        };
        internal static readonly byte[] bl_order = new byte[19]
        {
       16,
       17,
       18,
       0,
       8,
       7,
       9,
       6,
       10,
       5,
       11,
       4,
       12,
       3,
       13,
       2,
       14,
       1,
       15
        };
        internal static readonly byte[] _dist_code = new byte[512]
        {
       0,
       1,
       2,
       3,
       4,
       4,
       5,
       5,
       6,
       6,
       6,
       6,
       7,
       7,
       7,
       7,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       9,
       9,
       9,
       9,
       9,
       9,
       9,
       9,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       10,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       11,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       12,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       13,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       14,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       15,
       0,
       0,
       16,
       17,
       18,
       18,
       19,
       19,
       20,
       20,
       20,
       20,
       21,
       21,
       21,
       21,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       28,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29,
       29
        };
        internal static readonly byte[] _length_code = new byte[256]
        {
       0,
       1,
       2,
       3,
       4,
       5,
       6,
       7,
       8,
       8,
       9,
       9,
       10,
       10,
       11,
       11,
       12,
       12,
       12,
       12,
       13,
       13,
       13,
       13,
       14,
       14,
       14,
       14,
       15,
       15,
       15,
       15,
       16,
       16,
       16,
       16,
       16,
       16,
       16,
       16,
       17,
       17,
       17,
       17,
       17,
       17,
       17,
       17,
       18,
       18,
       18,
       18,
       18,
       18,
       18,
       18,
       19,
       19,
       19,
       19,
       19,
       19,
       19,
       19,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       20,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       21,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       22,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       23,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       24,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       25,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       26,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       27,
       28
        };
        internal static readonly int[] base_length = new int[29]
        {
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      10,
      12,
      14,
      16,
      20,
      24,
      28,
      32,
      40,
      48,
      56,
      64,
      80,
      96,
      112,
      128,
      160,
      192,
      224,
      0
        };
        internal static readonly int[] base_dist = new int[30]
        {
      0,
      1,
      2,
      3,
      4,
      6,
      8,
      12,
      16,
      24,
      32,
      48,
      64,
      96,
      128,
      192,
      256,
      384,
      512,
      768,
      1024,
      1536,
      2048,
      3072,
      4096,
      6144,
      8192,
      12288,
      16384,
      24576
        };
        internal short[] dyn_tree;
        internal int max_code;
        internal StaticTree stat_desc;

        internal static int d_code( int dist ) => dist >= 256 ? _dist_code[256 + (dist >> 7)] : _dist_code[dist];

        internal void gen_bitlen( Deflate s )
        {
            short[] dynTree = this.dyn_tree;
            short[] staticTree = this.stat_desc.static_tree;
            int[] extraBits = this.stat_desc.extra_bits;
            int extraBase = this.stat_desc.extra_base;
            int maxLength = this.stat_desc.max_length;
            int num1 = 0;
            for (int index = 0; index <= 15; ++index)
                s.bl_count[index] = 0;
            dynTree[(s.heap[s.heap_max] * 2) + 1] = 0;
            int index1;
            for (index1 = s.heap_max + 1; index1 < 573; ++index1)
            {
                int num2 = s.heap[index1];
                int num3 = dynTree[(dynTree[(num2 * 2) + 1] * 2) + 1] + 1;
                if (num3 > maxLength)
                {
                    num3 = maxLength;
                    ++num1;
                }
                dynTree[(num2 * 2) + 1] = (short)num3;
                if (num2 <= this.max_code)
                {
                    short[] blCount;
                    IntPtr index2;
                    (blCount = s.bl_count)[(int)(index2 = (IntPtr)num3)] = (short)(blCount[(int)index2] + 1);
                    int num4 = 0;
                    if (num2 >= extraBase)
                        num4 = extraBits[num2 - extraBase];
                    short num5 = dynTree[num2 * 2];
                    s.opt_len += num5 * (num3 + num4);
                    if (staticTree != null)
                        s.static_len += num5 * (staticTree[(num2 * 2) + 1] + num4);
                }
            }
            if (num1 == 0)
                return;
            do
            {
                int index3 = maxLength - 1;
                while (s.bl_count[index3] == 0)
                    --index3;
                short[] blCount1;
                IntPtr index4;
                (blCount1 = s.bl_count)[(int)(index4 = (IntPtr)index3)] = (short)(blCount1[(int)index4] - 1);
                short[] blCount2;
                IntPtr index5;
                (blCount2 = s.bl_count)[(int)(index5 = (IntPtr)(index3 + 1))] = (short)(blCount2[(int)index5] + 2);
                short[] blCount3;
                IntPtr index6;
                (blCount3 = s.bl_count)[(int)(index6 = (IntPtr)maxLength)] = (short)(blCount3[(int)index6] - 1);
                num1 -= 2;
            }
            while (num1 > 0);
            for (int index7 = maxLength; index7 != 0; --index7)
            {
                int num6 = s.bl_count[index7];
                while (num6 != 0)
                {
                    int num7 = s.heap[--index1];
                    if (num7 <= this.max_code)
                    {
                        if (dynTree[(num7 * 2) + 1] != index7)
                        {
                            s.opt_len += (int)((index7 - (long)dynTree[(num7 * 2) + 1]) * dynTree[num7 * 2]);
                            dynTree[(num7 * 2) + 1] = (short)index7;
                        }
                        --num6;
                    }
                }
            }
        }

        internal void build_tree( Deflate s )
        {
            short[] dynTree = this.dyn_tree;
            short[] staticTree = this.stat_desc.static_tree;
            int elems = this.stat_desc.elems;
            int max_code = -1;
            s.heap_len = 0;
            s.heap_max = 573;
            for (int index = 0; index < elems; ++index)
            {
                if (dynTree[index * 2] != 0)
                {
                    s.heap[++s.heap_len] = max_code = index;
                    s.depth[index] = 0;
                }
                else
                    dynTree[(index * 2) + 1] = 0;
            }
            while (s.heap_len < 2)
            {
                int[] heap = s.heap;
                int index1 = ++s.heap_len;
                int num1;
                if (max_code >= 2)
                    num1 = 0;
                else
                    max_code = num1 = max_code + 1;
                int num2 = num1;
                heap[index1] = num1;
                int index2 = num2;
                dynTree[index2 * 2] = 1;
                s.depth[index2] = 0;
                --s.opt_len;
                if (staticTree != null)
                    s.static_len -= staticTree[(index2 * 2) + 1];
            }
            this.max_code = max_code;
            for (int k = s.heap_len / 2; k >= 1; --k)
                s.pqdownheap( dynTree, k );
            int index3 = elems;
            do
            {
                int index4 = s.heap[1];
                s.heap[1] = s.heap[s.heap_len--];
                s.pqdownheap( dynTree, 1 );
                int index5 = s.heap[1];
                s.heap[--s.heap_max] = index4;
                s.heap[--s.heap_max] = index5;
                dynTree[index3 * 2] = (short)(dynTree[index4 * 2] + dynTree[index5 * 2]);
                s.depth[index3] = (byte)((uint)System.Math.Max( s.depth[index4], s.depth[index5] ) + 1U);
                dynTree[(index4 * 2) + 1] = dynTree[(index5 * 2) + 1] = (short)index3;
                s.heap[1] = index3++;
                s.pqdownheap( dynTree, 1 );
            }
            while (s.heap_len >= 2);
            s.heap[--s.heap_max] = s.heap[1];
            this.gen_bitlen( s );
            gen_codes( dynTree, max_code, s.bl_count );
        }

        internal static void gen_codes( short[] tree, int max_code, short[] bl_count )
        {
            short[] numArray1 = new short[16];
            short num1 = 0;
            for (int index = 1; index <= 15; ++index)
                numArray1[index] = num1 = (short)((num1 + bl_count[index - 1]) << 1);
            for (int index1 = 0; index1 <= max_code; ++index1)
            {
                int len = tree[(index1 * 2) + 1];
                if (len != 0)
                {
                    short[] numArray2 = tree;
                    int index2 = index1 * 2;
                    short[] numArray3;
                    IntPtr index3;
                    short code;
                    (numArray3 = numArray1)[(int)(index3 = (IntPtr)len)] = (short)((code = numArray3[(int)index3]) + 1);
                    int num2 = (short)bi_reverse( code, len );
                    numArray2[index2] = (short)num2;
                }
            }
        }

        internal static int bi_reverse( int code, int len )
        {
            int num1 = 0;
            do
            {
                int num2 = num1 | (code & 1);
                code >>= 1;
                num1 = num2 << 1;
            }
            while (--len > 0);
            return num1 >> 1;
        }
    }
}
