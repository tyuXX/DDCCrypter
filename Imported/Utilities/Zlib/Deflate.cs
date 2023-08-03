// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.Deflate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.Zlib
{
    public sealed class Deflate
    {
        private const int MAX_MEM_LEVEL = 9;
        private const int Z_DEFAULT_COMPRESSION = -1;
        private const int MAX_WBITS = 15;
        private const int DEF_MEM_LEVEL = 8;
        private const int STORED = 0;
        private const int FAST = 1;
        private const int SLOW = 2;
        private const int NeedMore = 0;
        private const int BlockDone = 1;
        private const int FinishStarted = 2;
        private const int FinishDone = 3;
        private const int PRESET_DICT = 32;
        private const int Z_FILTERED = 1;
        private const int Z_HUFFMAN_ONLY = 2;
        private const int Z_DEFAULT_STRATEGY = 0;
        private const int Z_NO_FLUSH = 0;
        private const int Z_PARTIAL_FLUSH = 1;
        private const int Z_SYNC_FLUSH = 2;
        private const int Z_FULL_FLUSH = 3;
        private const int Z_FINISH = 4;
        private const int Z_OK = 0;
        private const int Z_STREAM_END = 1;
        private const int Z_NEED_DICT = 2;
        private const int Z_ERRNO = -1;
        private const int Z_STREAM_ERROR = -2;
        private const int Z_DATA_ERROR = -3;
        private const int Z_MEM_ERROR = -4;
        private const int Z_BUF_ERROR = -5;
        private const int Z_VERSION_ERROR = -6;
        private const int INIT_STATE = 42;
        private const int BUSY_STATE = 113;
        private const int FINISH_STATE = 666;
        private const int Z_DEFLATED = 8;
        private const int STORED_BLOCK = 0;
        private const int STATIC_TREES = 1;
        private const int DYN_TREES = 2;
        private const int Z_BINARY = 0;
        private const int Z_ASCII = 1;
        private const int Z_UNKNOWN = 2;
        private const int Buf_size = 16;
        private const int REP_3_6 = 16;
        private const int REPZ_3_10 = 17;
        private const int REPZ_11_138 = 18;
        private const int MIN_MATCH = 3;
        private const int MAX_MATCH = 258;
        private const int MIN_LOOKAHEAD = 262;
        private const int MAX_BITS = 15;
        private const int D_CODES = 30;
        private const int BL_CODES = 19;
        private const int LENGTH_CODES = 29;
        private const int LITERALS = 256;
        private const int L_CODES = 286;
        private const int HEAP_SIZE = 573;
        private const int END_BLOCK = 256;
        private static readonly Deflate.Config[] config_table;
        private static readonly string[] z_errmsg = new string[10]
        {
      "need dictionary",
      "stream end",
      "",
      "file error",
      "stream error",
      "data error",
      "insufficient memory",
      "buffer error",
      "incompatible version",
      ""
        };
        internal ZStream strm;
        internal int status;
        internal byte[] pending_buf;
        internal int pending_buf_size;
        internal int pending_out;
        internal int pending;
        internal int noheader;
        internal byte data_type;
        internal byte method;
        internal int last_flush;
        internal int w_size;
        internal int w_bits;
        internal int w_mask;
        internal byte[] window;
        internal int window_size;
        internal short[] prev;
        internal short[] head;
        internal int ins_h;
        internal int hash_size;
        internal int hash_bits;
        internal int hash_mask;
        internal int hash_shift;
        internal int block_start;
        internal int match_length;
        internal int prev_match;
        internal int match_available;
        internal int strstart;
        internal int match_start;
        internal int lookahead;
        internal int prev_length;
        internal int max_chain_length;
        internal int max_lazy_match;
        internal int level;
        internal int strategy;
        internal int good_match;
        internal int nice_match;
        internal short[] dyn_ltree;
        internal short[] dyn_dtree;
        internal short[] bl_tree;
        internal Tree l_desc = new();
        internal Tree d_desc = new();
        internal Tree bl_desc = new();
        internal short[] bl_count = new short[16];
        internal int[] heap = new int[573];
        internal int heap_len;
        internal int heap_max;
        internal byte[] depth = new byte[573];
        internal int l_buf;
        internal int lit_bufsize;
        internal int last_lit;
        internal int d_buf;
        internal int opt_len;
        internal int static_len;
        internal int matches;
        internal int last_eob_len;
        internal uint bi_buf;
        internal int bi_valid;

        static Deflate()
        {
            config_table = new Deflate.Config[10];
            config_table[0] = new Deflate.Config( 0, 0, 0, 0, 0 );
            config_table[1] = new Deflate.Config( 4, 4, 8, 4, 1 );
            config_table[2] = new Deflate.Config( 4, 5, 16, 8, 1 );
            config_table[3] = new Deflate.Config( 4, 6, 32, 32, 1 );
            config_table[4] = new Deflate.Config( 4, 4, 16, 16, 2 );
            config_table[5] = new Deflate.Config( 8, 16, 32, 32, 2 );
            config_table[6] = new Deflate.Config( 8, 16, 128, 128, 2 );
            config_table[7] = new Deflate.Config( 8, 32, 128, 256, 2 );
            config_table[8] = new Deflate.Config( 32, 128, 258, 1024, 2 );
            config_table[9] = new Deflate.Config( 32, 258, 258, 4096, 2 );
        }

        internal Deflate()
        {
            this.dyn_ltree = new short[1146];
            this.dyn_dtree = new short[122];
            this.bl_tree = new short[78];
        }

        internal void lm_init()
        {
            this.window_size = 2 * this.w_size;
            this.head[this.hash_size - 1] = 0;
            for (int index = 0; index < this.hash_size - 1; ++index)
                this.head[index] = 0;
            this.max_lazy_match = config_table[this.level].max_lazy;
            this.good_match = config_table[this.level].good_length;
            this.nice_match = config_table[this.level].nice_length;
            this.max_chain_length = config_table[this.level].max_chain;
            this.strstart = 0;
            this.block_start = 0;
            this.lookahead = 0;
            this.match_length = this.prev_length = 2;
            this.match_available = 0;
            this.ins_h = 0;
        }

        internal void tr_init()
        {
            this.l_desc.dyn_tree = this.dyn_ltree;
            this.l_desc.stat_desc = StaticTree.static_l_desc;
            this.d_desc.dyn_tree = this.dyn_dtree;
            this.d_desc.stat_desc = StaticTree.static_d_desc;
            this.bl_desc.dyn_tree = this.bl_tree;
            this.bl_desc.stat_desc = StaticTree.static_bl_desc;
            this.bi_buf = 0U;
            this.bi_valid = 0;
            this.last_eob_len = 8;
            this.init_block();
        }

        internal void init_block()
        {
            for (int index = 0; index < 286; ++index)
                this.dyn_ltree[index * 2] = 0;
            for (int index = 0; index < 30; ++index)
                this.dyn_dtree[index * 2] = 0;
            for (int index = 0; index < 19; ++index)
                this.bl_tree[index * 2] = 0;
            this.dyn_ltree[512] = 1;
            this.opt_len = this.static_len = 0;
            this.last_lit = this.matches = 0;
        }

        internal void pqdownheap( short[] tree, int k )
        {
            int n = this.heap[k];
            for (int index = k << 1; index <= this.heap_len; index <<= 1)
            {
                if (index < this.heap_len && smaller( tree, this.heap[index + 1], this.heap[index], this.depth ))
                    ++index;
                if (!smaller( tree, n, this.heap[index], this.depth ))
                {
                    this.heap[k] = this.heap[index];
                    k = index;
                }
                else
                    break;
            }
            this.heap[k] = n;
        }

        internal static bool smaller( short[] tree, int n, int m, byte[] depth )
        {
            short num1 = tree[n * 2];
            short num2 = tree[m * 2];
            if (num1 < num2)
                return true;
            return num1 == num2 && depth[n] <= depth[m];
        }

        internal void scan_tree( short[] tree, int max_code )
        {
            int num1 = -1;
            int num2 = tree[1];
            int num3 = 0;
            int num4 = 7;
            int num5 = 4;
            if (num2 == 0)
            {
                num4 = 138;
                num5 = 3;
            }
            tree[((max_code + 1) * 2) + 1] = -1;
            for (int index1 = 0; index1 <= max_code; ++index1)
            {
                int num6 = num2;
                num2 = tree[((index1 + 1) * 2) + 1];
                if (++num3 >= num4 || num6 != num2)
                {
                    if (num3 < num5)
                    {
                        short[] blTree;
                        IntPtr index2;
                        (blTree = this.bl_tree)[(int)(index2 = (IntPtr)(num6 * 2))] = (short)(blTree[(int)index2] + (short)num3);
                    }
                    else if (num6 != 0)
                    {
                        if (num6 != num1)
                        {
                            short[] blTree;
                            IntPtr index3;
                            (blTree = this.bl_tree)[(int)(index3 = (IntPtr)(num6 * 2))] = (short)(blTree[(int)index3] + 1);
                        }
                        short[] blTree1;
                        (blTree1 = this.bl_tree)[32] = (short)(blTree1[32] + 1);
                    }
                    else if (num3 <= 10)
                    {
                        short[] blTree;
                        (blTree = this.bl_tree)[34] = (short)(blTree[34] + 1);
                    }
                    else
                    {
                        short[] blTree;
                        (blTree = this.bl_tree)[36] = (short)(blTree[36] + 1);
                    }
                    num3 = 0;
                    num1 = num6;
                    if (num2 == 0)
                    {
                        num4 = 138;
                        num5 = 3;
                    }
                    else if (num6 == num2)
                    {
                        num4 = 6;
                        num5 = 3;
                    }
                    else
                    {
                        num4 = 7;
                        num5 = 4;
                    }
                }
            }
        }

        internal int build_bl_tree()
        {
            this.scan_tree( this.dyn_ltree, this.l_desc.max_code );
            this.scan_tree( this.dyn_dtree, this.d_desc.max_code );
            this.bl_desc.build_tree( this );
            int index = 18;
            while (index >= 3 && this.bl_tree[(Tree.bl_order[index] * 2) + 1] == 0)
                --index;
            this.opt_len += (3 * (index + 1)) + 5 + 5 + 4;
            return index;
        }

        internal void send_all_trees( int lcodes, int dcodes, int blcodes )
        {
            this.send_bits( lcodes - 257, 5 );
            this.send_bits( dcodes - 1, 5 );
            this.send_bits( blcodes - 4, 4 );
            for (int index = 0; index < blcodes; ++index)
                this.send_bits( this.bl_tree[(Tree.bl_order[index] * 2) + 1], 3 );
            this.send_tree( this.dyn_ltree, lcodes - 1 );
            this.send_tree( this.dyn_dtree, dcodes - 1 );
        }

        internal void send_tree( short[] tree, int max_code )
        {
            int num1 = -1;
            int num2 = tree[1];
            int num3 = 0;
            int num4 = 7;
            int num5 = 4;
            if (num2 == 0)
            {
                num4 = 138;
                num5 = 3;
            }
            for (int index = 0; index <= max_code; ++index)
            {
                int c = num2;
                num2 = tree[((index + 1) * 2) + 1];
                if (++num3 >= num4 || c != num2)
                {
                    if (num3 < num5)
                    {
                        do
                        {
                            this.send_code( c, this.bl_tree );
                        }
                        while (--num3 != 0);
                    }
                    else if (c != 0)
                    {
                        if (c != num1)
                        {
                            this.send_code( c, this.bl_tree );
                            --num3;
                        }
                        this.send_code( 16, this.bl_tree );
                        this.send_bits( num3 - 3, 2 );
                    }
                    else if (num3 <= 10)
                    {
                        this.send_code( 17, this.bl_tree );
                        this.send_bits( num3 - 3, 3 );
                    }
                    else
                    {
                        this.send_code( 18, this.bl_tree );
                        this.send_bits( num3 - 11, 7 );
                    }
                    num3 = 0;
                    num1 = c;
                    if (num2 == 0)
                    {
                        num4 = 138;
                        num5 = 3;
                    }
                    else if (c == num2)
                    {
                        num4 = 6;
                        num5 = 3;
                    }
                    else
                    {
                        num4 = 7;
                        num5 = 4;
                    }
                }
            }
        }

        internal void put_byte( byte[] p, int start, int len )
        {
            Array.Copy( p, start, pending_buf, this.pending, len );
            this.pending += len;
        }

        internal void put_byte( byte c ) => this.pending_buf[this.pending++] = c;

        internal void put_short( int w )
        {
            this.pending_buf[this.pending++] = (byte)w;
            this.pending_buf[this.pending++] = (byte)(w >> 8);
        }

        internal void putShortMSB( int b )
        {
            this.pending_buf[this.pending++] = (byte)(b >> 8);
            this.pending_buf[this.pending++] = (byte)b;
        }

        internal void send_code( int c, short[] tree )
        {
            int index = c * 2;
            this.send_bits( tree[index] & ushort.MaxValue, tree[index + 1] & ushort.MaxValue );
        }

        internal void send_bits( int val, int length )
        {
            if (this.bi_valid > 16 - length)
            {
                this.bi_buf |= (uint)(val << this.bi_valid);
                this.pending_buf[this.pending++] = (byte)this.bi_buf;
                this.pending_buf[this.pending++] = (byte)(this.bi_buf >> 8);
                this.bi_buf = (uint)(val >>> 16 - this.bi_valid);
                this.bi_valid += length - 16;
            }
            else
            {
                this.bi_buf |= (uint)(val << this.bi_valid);
                this.bi_valid += length;
            }
        }

        internal void _tr_align()
        {
            this.send_bits( 2, 3 );
            this.send_code( 256, StaticTree.static_ltree );
            this.bi_flush();
            if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
            {
                this.send_bits( 2, 3 );
                this.send_code( 256, StaticTree.static_ltree );
                this.bi_flush();
            }
            this.last_eob_len = 7;
        }

        internal bool _tr_tally( int dist, int lc )
        {
            this.pending_buf[this.d_buf + (this.last_lit * 2)] = (byte)(dist >> 8);
            this.pending_buf[this.d_buf + (this.last_lit * 2) + 1] = (byte)dist;
            this.pending_buf[this.l_buf + this.last_lit] = (byte)lc;
            ++this.last_lit;
            if (dist == 0)
            {
                short[] dynLtree;
                IntPtr index;
                (dynLtree = this.dyn_ltree)[(int)(index = (IntPtr)(lc * 2))] = (short)(dynLtree[(int)index] + 1);
            }
            else
            {
                ++this.matches;
                --dist;
                short[] dynLtree;
                IntPtr index1;
                (dynLtree = this.dyn_ltree)[(int)(index1 = (IntPtr)((Tree._length_code[lc] + 256 + 1) * 2))] = (short)(dynLtree[(int)index1] + 1);
                short[] dynDtree;
                IntPtr index2;
                (dynDtree = this.dyn_dtree)[(int)(index2 = (IntPtr)(Tree.d_code( dist ) * 2))] = (short)(dynDtree[(int)index2] + 1);
            }
            if ((this.last_lit & 8191) == 0 && this.level > 2)
            {
                int num1 = this.last_lit * 8;
                int num2 = this.strstart - this.block_start;
                for (int index = 0; index < 30; ++index)
                    num1 += (int)(this.dyn_dtree[index * 2] * (5L + Tree.extra_dbits[index]));
                int num3 = num1 >> 3;
                if (this.matches < this.last_lit / 2 && num3 < num2 / 2)
                    return true;
            }
            return this.last_lit == this.lit_bufsize - 1;
        }

        internal void compress_block( short[] ltree, short[] dtree )
        {
            int num1 = 0;
            if (this.last_lit != 0)
            {
                do
                {
                    int num2 = ((this.pending_buf[this.d_buf + (num1 * 2)] << 8) & 65280) | (this.pending_buf[this.d_buf + (num1 * 2) + 1] & byte.MaxValue);
                    int c1 = this.pending_buf[this.l_buf + num1] & byte.MaxValue;
                    ++num1;
                    if (num2 == 0)
                    {
                        this.send_code( c1, ltree );
                    }
                    else
                    {
                        int index = Tree._length_code[c1];
                        this.send_code( index + 256 + 1, ltree );
                        int extraLbit = Tree.extra_lbits[index];
                        if (extraLbit != 0)
                            this.send_bits( c1 - Tree.base_length[index], extraLbit );
                        int dist = num2 - 1;
                        int c2 = Tree.d_code( dist );
                        this.send_code( c2, dtree );
                        int extraDbit = Tree.extra_dbits[c2];
                        if (extraDbit != 0)
                            this.send_bits( dist - Tree.base_dist[c2], extraDbit );
                    }
                }
                while (num1 < this.last_lit);
            }
            this.send_code( 256, ltree );
            this.last_eob_len = ltree[513];
        }

        internal void set_data_type()
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            for (; num1 < 7; ++num1)
                num3 += this.dyn_ltree[num1 * 2];
            for (; num1 < 128; ++num1)
                num2 += this.dyn_ltree[num1 * 2];
            for (; num1 < 256; ++num1)
                num3 += this.dyn_ltree[num1 * 2];
            this.data_type = num3 > num2 >> 2 ? (byte)0 : (byte)1;
        }

        internal void bi_flush()
        {
            if (this.bi_valid == 16)
            {
                this.pending_buf[this.pending++] = (byte)this.bi_buf;
                this.pending_buf[this.pending++] = (byte)(this.bi_buf >> 8);
                this.bi_buf = 0U;
                this.bi_valid = 0;
            }
            else
            {
                if (this.bi_valid < 8)
                    return;
                this.pending_buf[this.pending++] = (byte)this.bi_buf;
                this.bi_buf >>= 8;
                this.bi_buf &= byte.MaxValue;
                this.bi_valid -= 8;
            }
        }

        internal void bi_windup()
        {
            if (this.bi_valid > 8)
            {
                this.pending_buf[this.pending++] = (byte)this.bi_buf;
                this.pending_buf[this.pending++] = (byte)(this.bi_buf >> 8);
            }
            else if (this.bi_valid > 0)
                this.pending_buf[this.pending++] = (byte)this.bi_buf;
            this.bi_buf = 0U;
            this.bi_valid = 0;
        }

        internal void copy_block( int buf, int len, bool header )
        {
            this.bi_windup();
            this.last_eob_len = 8;
            if (header)
            {
                this.put_short( (short)len );
                this.put_short( (short)~len );
            }
            this.put_byte( this.window, buf, len );
        }

        internal void flush_block_only( bool eof )
        {
            this._tr_flush_block( this.block_start >= 0 ? this.block_start : -1, this.strstart - this.block_start, eof );
            this.block_start = this.strstart;
            this.strm.flush_pending();
        }

        internal int deflate_stored( int flush )
        {
            int num1 = ushort.MaxValue;
            if (num1 > this.pending_buf_size - 5)
                num1 = this.pending_buf_size - 5;
            do
            {
                do
                {
                    if (this.lookahead <= 1)
                    {
                        this.fill_window();
                        if (this.lookahead == 0 && flush == 0)
                            return 0;
                        if (this.lookahead == 0)
                            goto label_12;
                    }
                    this.strstart += this.lookahead;
                    this.lookahead = 0;
                    int num2 = this.block_start + num1;
                    if (this.strstart == 0 || this.strstart >= num2)
                    {
                        this.lookahead = this.strstart - num2;
                        this.strstart = num2;
                        this.flush_block_only( false );
                        if (this.strm.avail_out == 0)
                            return 0;
                    }
                }
                while (this.strstart - this.block_start < this.w_size - 262);
                this.flush_block_only( false );
            }
            while (this.strm.avail_out != 0);
            return 0;
        label_12:
            this.flush_block_only( flush == 4 );
            return this.strm.avail_out == 0 ? (flush != 4 ? 0 : 2) : (flush != 4 ? 1 : 3);
        }

        internal void _tr_stored_block( int buf, int stored_len, bool eof )
        {
            this.send_bits( eof ? 1 : 0, 3 );
            this.copy_block( buf, stored_len, true );
        }

        internal void _tr_flush_block( int buf, int stored_len, bool eof )
        {
            int num1 = 0;
            int num2;
            int num3;
            if (this.level > 0)
            {
                if (this.data_type == 2)
                    this.set_data_type();
                this.l_desc.build_tree( this );
                this.d_desc.build_tree( this );
                num1 = this.build_bl_tree();
                num2 = (this.opt_len + 3 + 7) >> 3;
                num3 = (this.static_len + 3 + 7) >> 3;
                if (num3 <= num2)
                    num2 = num3;
            }
            else
                num2 = num3 = stored_len + 5;
            if (stored_len + 4 <= num2 && buf != -1)
                this._tr_stored_block( buf, stored_len, eof );
            else if (num3 == num2)
            {
                this.send_bits( 2 + (eof ? 1 : 0), 3 );
                this.compress_block( StaticTree.static_ltree, StaticTree.static_dtree );
            }
            else
            {
                this.send_bits( 4 + (eof ? 1 : 0), 3 );
                this.send_all_trees( this.l_desc.max_code + 1, this.d_desc.max_code + 1, num1 + 1 );
                this.compress_block( this.dyn_ltree, this.dyn_dtree );
            }
            this.init_block();
            if (!eof)
                return;
            this.bi_windup();
        }

        internal void fill_window()
        {
            do
            {
                int size = this.window_size - this.lookahead - this.strstart;
                if (size == 0 && this.strstart == 0 && this.lookahead == 0)
                    size = this.w_size;
                else if (size == -1)
                    --size;
                else if (this.strstart >= this.w_size + this.w_size - 262)
                {
                    Array.Copy( window, this.w_size, window, 0, this.w_size );
                    this.match_start -= this.w_size;
                    this.strstart -= this.w_size;
                    this.block_start -= this.w_size;
                    int hashSize = this.hash_size;
                    int index1 = hashSize;
                    do
                    {
                        int num = this.head[--index1] & ushort.MaxValue;
                        this.head[index1] = num >= this.w_size ? (short)(num - this.w_size) : (short)0;
                    }
                    while (--hashSize != 0);
                    int wSize = this.w_size;
                    int index2 = wSize;
                    do
                    {
                        int num = this.prev[--index2] & ushort.MaxValue;
                        this.prev[index2] = num >= this.w_size ? (short)(num - this.w_size) : (short)0;
                    }
                    while (--wSize != 0);
                    size += this.w_size;
                }
                if (this.strm.avail_in == 0)
                    break;
                this.lookahead += this.strm.read_buf( this.window, this.strstart + this.lookahead, size );
                if (this.lookahead >= 3)
                {
                    this.ins_h = this.window[this.strstart] & byte.MaxValue;
                    this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask;
                }
            }
            while (this.lookahead < 262 && this.strm.avail_in != 0);
        }

        internal int deflate_fast( int flush )
        {
            int cur_match = 0;
            do
            {
                bool flag;
                do
                {
                    if (this.lookahead < 262)
                    {
                        this.fill_window();
                        if (this.lookahead < 262 && flush == 0)
                            return 0;
                        if (this.lookahead == 0)
                            goto label_19;
                    }
                    if (this.lookahead >= 3)
                    {
                        this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask;
                        cur_match = this.head[this.ins_h] & ushort.MaxValue;
                        this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                        this.head[this.ins_h] = (short)this.strstart;
                    }
                    if (cur_match != 0 && ((this.strstart - cur_match) & ushort.MaxValue) <= this.w_size - 262 && this.strategy != 2)
                        this.match_length = this.longest_match( cur_match );
                    if (this.match_length >= 3)
                    {
                        flag = this._tr_tally( this.strstart - this.match_start, this.match_length - 3 );
                        this.lookahead -= this.match_length;
                        if (this.match_length <= this.max_lazy_match && this.lookahead >= 3)
                        {
                            --this.match_length;
                            do
                            {
                                ++this.strstart;
                                this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask;
                                cur_match = this.head[this.ins_h] & ushort.MaxValue;
                                this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                                this.head[this.ins_h] = (short)this.strstart;
                            }
                            while (--this.match_length != 0);
                            ++this.strstart;
                        }
                        else
                        {
                            this.strstart += this.match_length;
                            this.match_length = 0;
                            this.ins_h = this.window[this.strstart] & byte.MaxValue;
                            this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask;
                        }
                    }
                    else
                    {
                        flag = this._tr_tally( 0, this.window[this.strstart] & byte.MaxValue );
                        --this.lookahead;
                        ++this.strstart;
                    }
                }
                while (!flag);
                this.flush_block_only( false );
            }
            while (this.strm.avail_out != 0);
            return 0;
        label_19:
            this.flush_block_only( flush == 4 );
            return this.strm.avail_out == 0 ? (flush == 4 ? 2 : 0) : (flush != 4 ? 1 : 3);
        }

        internal int deflate_slow( int flush )
        {
            int cur_match = 0;
            while (true)
            {
                do
                {
                    if (this.lookahead < 262)
                    {
                        this.fill_window();
                        if (this.lookahead < 262 && flush == 0)
                            return 0;
                        if (this.lookahead == 0)
                            goto label_26;
                    }
                    if (this.lookahead >= 3)
                    {
                        this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask;
                        cur_match = this.head[this.ins_h] & ushort.MaxValue;
                        this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                        this.head[this.ins_h] = (short)this.strstart;
                    }
                    this.prev_length = this.match_length;
                    this.prev_match = this.match_start;
                    this.match_length = 2;
                    if (cur_match != 0 && this.prev_length < this.max_lazy_match && ((this.strstart - cur_match) & ushort.MaxValue) <= this.w_size - 262)
                    {
                        if (this.strategy != 2)
                            this.match_length = this.longest_match( cur_match );
                        if (this.match_length <= 5 && (this.strategy == 1 || (this.match_length == 3 && this.strstart - this.match_start > 4096)))
                            this.match_length = 2;
                    }
                    if (this.prev_length >= 3 && this.match_length <= this.prev_length)
                    {
                        int num = this.strstart + this.lookahead - 3;
                        bool flag = this._tr_tally( this.strstart - 1 - this.prev_match, this.prev_length - 3 );
                        this.lookahead -= this.prev_length - 1;
                        this.prev_length -= 2;
                        do
                        {
                            if (++this.strstart <= num)
                            {
                                this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask;
                                cur_match = this.head[this.ins_h] & ushort.MaxValue;
                                this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                                this.head[this.ins_h] = (short)this.strstart;
                            }
                        }
                        while (--this.prev_length != 0);
                        this.match_available = 0;
                        this.match_length = 2;
                        ++this.strstart;
                        if (flag)
                        {
                            this.flush_block_only( false );
                            if (this.strm.avail_out == 0)
                                return 0;
                        }
                    }
                    else if (this.match_available != 0)
                    {
                        if (this._tr_tally( 0, this.window[this.strstart - 1] & byte.MaxValue ))
                            this.flush_block_only( false );
                        ++this.strstart;
                        --this.lookahead;
                    }
                    else
                        goto label_25;
                }
                while (this.strm.avail_out != 0);
                break;
            label_25:
                this.match_available = 1;
                ++this.strstart;
                --this.lookahead;
            }
            return 0;
        label_26:
            if (this.match_available != 0)
            {
                this._tr_tally( 0, this.window[this.strstart - 1] & byte.MaxValue );
                this.match_available = 0;
            }
            this.flush_block_only( flush == 4 );
            return this.strm.avail_out == 0 ? (flush == 4 ? 2 : 0) : (flush != 4 ? 1 : 3);
        }

        internal int longest_match( int cur_match )
        {
            int maxChainLength = this.max_chain_length;
            int index1 = this.strstart;
            int num1 = this.prev_length;
            int num2 = this.strstart > this.w_size - 262 ? this.strstart - (this.w_size - 262) : 0;
            int num3 = this.nice_match;
            int wMask = this.w_mask;
            int num4 = this.strstart + 258;
            byte num5 = this.window[index1 + num1 - 1];
            byte num6 = this.window[index1 + num1];
            if (this.prev_length >= this.good_match)
                maxChainLength >>= 2;
            if (num3 > this.lookahead)
                num3 = this.lookahead;
            do
            {
                int index2 = cur_match;
                int num7;
                if (this.window[index2 + num1] == num6 && this.window[index2 + num1 - 1] == num5 && this.window[index2] == this.window[index1] && this.window[num7 = index2 + 1] == this.window[index1 + 1])
                {
                    int num8 = index1 + 2;
                    int num9 = num7 + 1;
                    int num10;
                    int num11;
                    int num12;
                    int num13;
                    int num14;
                    int num15;
                    int num16;
                    do
                        ;
                    while (this.window[++num8] == this.window[num10 = num9 + 1] && this.window[++num8] == this.window[num11 = num10 + 1] && this.window[++num8] == this.window[num12 = num11 + 1] && this.window[++num8] == this.window[num13 = num12 + 1] && this.window[++num8] == this.window[num14 = num13 + 1] && this.window[++num8] == this.window[num15 = num14 + 1] && this.window[++num8] == this.window[num16 = num15 + 1] && this.window[++num8] == this.window[num9 = num16 + 1] && num8 < num4);
                    int num17 = 258 - (num4 - num8);
                    index1 = num4 - 258;
                    if (num17 > num1)
                    {
                        this.match_start = cur_match;
                        num1 = num17;
                        if (num17 < num3)
                        {
                            num5 = this.window[index1 + num1 - 1];
                            num6 = this.window[index1 + num1];
                        }
                        else
                            break;
                    }
                }
            }
            while ((cur_match = this.prev[cur_match & wMask] & ushort.MaxValue) > num2 && --maxChainLength != 0);
            return num1 <= this.lookahead ? num1 : this.lookahead;
        }

        internal int deflateInit( ZStream strm, int level, int bits ) => this.deflateInit2( strm, level, 8, bits, 8, 0 );

        internal int deflateInit( ZStream strm, int level ) => this.deflateInit( strm, level, 15 );

        internal int deflateInit2(
          ZStream strm,
          int level,
          int method,
          int windowBits,
          int memLevel,
          int strategy )
        {
            int num = 0;
            strm.msg = null;
            if (level == -1)
                level = 6;
            if (windowBits < 0)
            {
                num = 1;
                windowBits = -windowBits;
            }
            if (memLevel < 1 || memLevel > 9 || method != 8 || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < 0 || strategy > 2)
                return -2;
            strm.dstate = this;
            this.noheader = num;
            this.w_bits = windowBits;
            this.w_size = 1 << this.w_bits;
            this.w_mask = this.w_size - 1;
            this.hash_bits = memLevel + 7;
            this.hash_size = 1 << this.hash_bits;
            this.hash_mask = this.hash_size - 1;
            this.hash_shift = (this.hash_bits + 3 - 1) / 3;
            this.window = new byte[this.w_size * 2];
            this.prev = new short[this.w_size];
            this.head = new short[this.hash_size];
            this.lit_bufsize = 1 << (memLevel + 6);
            this.pending_buf = new byte[this.lit_bufsize * 4];
            this.pending_buf_size = this.lit_bufsize * 4;
            this.d_buf = this.lit_bufsize / 2;
            this.l_buf = 3 * this.lit_bufsize;
            this.level = level;
            this.strategy = strategy;
            this.method = (byte)method;
            return this.deflateReset( strm );
        }

        internal int deflateReset( ZStream strm )
        {
            strm.total_in = strm.total_out = 0L;
            strm.msg = null;
            strm.data_type = 2;
            this.pending = 0;
            this.pending_out = 0;
            if (this.noheader < 0)
                this.noheader = 0;
            this.status = this.noheader != 0 ? 113 : 42;
            strm.adler = strm._adler.adler32( 0L, null, 0, 0 );
            this.last_flush = 0;
            this.tr_init();
            this.lm_init();
            return 0;
        }

        internal int deflateEnd()
        {
            if (this.status != 42 && this.status != 113 && this.status != 666)
                return -2;
            this.pending_buf = null;
            this.head = null;
            this.prev = null;
            this.window = null;
            return this.status != 113 ? 0 : -3;
        }

        internal int deflateParams( ZStream strm, int _level, int _strategy )
        {
            int num = 0;
            if (_level == -1)
                _level = 6;
            if (_level < 0 || _level > 9 || _strategy < 0 || _strategy > 2)
                return -2;
            if (config_table[this.level].func != config_table[_level].func && strm.total_in != 0L)
                num = strm.deflate( 1 );
            if (this.level != _level)
            {
                this.level = _level;
                this.max_lazy_match = config_table[this.level].max_lazy;
                this.good_match = config_table[this.level].good_length;
                this.nice_match = config_table[this.level].nice_length;
                this.max_chain_length = config_table[this.level].max_chain;
            }
            this.strategy = _strategy;
            return num;
        }

        internal int deflateSetDictionary( ZStream strm, byte[] dictionary, int dictLength )
        {
            int length = dictLength;
            int sourceIndex = 0;
            if (dictionary == null || this.status != 42)
                return -2;
            strm.adler = strm._adler.adler32( strm.adler, dictionary, 0, dictLength );
            if (length < 3)
                return 0;
            if (length > this.w_size - 262)
            {
                length = this.w_size - 262;
                sourceIndex = dictLength - length;
            }
            Array.Copy( dictionary, sourceIndex, window, 0, length );
            this.strstart = length;
            this.block_start = length;
            this.ins_h = this.window[0] & byte.MaxValue;
            this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[1] & byte.MaxValue)) & this.hash_mask;
            for (int index = 0; index <= length - 3; ++index)
            {
                this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[index + 2] & byte.MaxValue)) & this.hash_mask;
                this.prev[index & this.w_mask] = this.head[this.ins_h];
                this.head[this.ins_h] = (short)index;
            }
            return 0;
        }

        internal int deflate( ZStream strm, int flush )
        {
            if (flush > 4 || flush < 0)
                return -2;
            if (strm.next_out == null || (strm.next_in == null && strm.avail_in != 0) || (this.status == 666 && flush != 4))
            {
                strm.msg = z_errmsg[4];
                return -2;
            }
            if (strm.avail_out == 0)
            {
                strm.msg = z_errmsg[7];
                return -5;
            }
            this.strm = strm;
            int lastFlush = this.last_flush;
            this.last_flush = flush;
            if (this.status == 42)
            {
                int num1 = (8 + ((this.w_bits - 8) << 4)) << 8;
                int num2 = ((this.level - 1) & byte.MaxValue) >> 1;
                if (num2 > 3)
                    num2 = 3;
                int num3 = num1 | (num2 << 6);
                if (this.strstart != 0)
                    num3 |= 32;
                int b = num3 + (31 - (num3 % 31));
                this.status = 113;
                this.putShortMSB( b );
                if (this.strstart != 0)
                {
                    this.putShortMSB( (int)(strm.adler >> 16) );
                    this.putShortMSB( (int)(strm.adler & ushort.MaxValue) );
                }
                strm.adler = strm._adler.adler32( 0L, null, 0, 0 );
            }
            if (this.pending != 0)
            {
                strm.flush_pending();
                if (strm.avail_out == 0)
                {
                    this.last_flush = -1;
                    return 0;
                }
            }
            else if (strm.avail_in == 0 && flush <= lastFlush && flush != 4)
            {
                strm.msg = z_errmsg[7];
                return -5;
            }
            if (this.status == 666 && strm.avail_in != 0)
            {
                strm.msg = z_errmsg[7];
                return -5;
            }
            if (strm.avail_in != 0 || this.lookahead != 0 || (flush != 0 && this.status != 666))
            {
                int num = -1;
                switch (config_table[this.level].func)
                {
                    case 0:
                        num = this.deflate_stored( flush );
                        break;
                    case 1:
                        num = this.deflate_fast( flush );
                        break;
                    case 2:
                        num = this.deflate_slow( flush );
                        break;
                }
                if (num == 2 || num == 3)
                    this.status = 666;
                if (num == 0 || num == 2)
                {
                    if (strm.avail_out == 0)
                        this.last_flush = -1;
                    return 0;
                }
                if (num == 1)
                {
                    if (flush == 1)
                    {
                        this._tr_align();
                    }
                    else
                    {
                        this._tr_stored_block( 0, 0, false );
                        if (flush == 3)
                        {
                            for (int index = 0; index < this.hash_size; ++index)
                                this.head[index] = 0;
                        }
                    }
                    strm.flush_pending();
                    if (strm.avail_out == 0)
                    {
                        this.last_flush = -1;
                        return 0;
                    }
                }
            }
            if (flush != 4)
                return 0;
            if (this.noheader != 0)
                return 1;
            this.putShortMSB( (int)(strm.adler >> 16) );
            this.putShortMSB( (int)(strm.adler & ushort.MaxValue) );
            strm.flush_pending();
            this.noheader = -1;
            return this.pending == 0 ? 1 : 0;
        }

        internal class Config
        {
            internal int good_length;
            internal int max_lazy;
            internal int nice_length;
            internal int max_chain;
            internal int func;

            internal Config( int good_length, int max_lazy, int nice_length, int max_chain, int func )
            {
                this.good_length = good_length;
                this.max_lazy = max_lazy;
                this.nice_length = nice_length;
                this.max_chain = max_chain;
                this.func = func;
            }
        }
    }
}
