// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.ZStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.Zlib
{
    public sealed class ZStream
    {
        private const int MAX_WBITS = 15;
        private const int DEF_WBITS = 15;
        private const int Z_NO_FLUSH = 0;
        private const int Z_PARTIAL_FLUSH = 1;
        private const int Z_SYNC_FLUSH = 2;
        private const int Z_FULL_FLUSH = 3;
        private const int Z_FINISH = 4;
        private const int MAX_MEM_LEVEL = 9;
        private const int Z_OK = 0;
        private const int Z_STREAM_END = 1;
        private const int Z_NEED_DICT = 2;
        private const int Z_ERRNO = -1;
        private const int Z_STREAM_ERROR = -2;
        private const int Z_DATA_ERROR = -3;
        private const int Z_MEM_ERROR = -4;
        private const int Z_BUF_ERROR = -5;
        private const int Z_VERSION_ERROR = -6;
        public byte[] next_in;
        public int next_in_index;
        public int avail_in;
        public long total_in;
        public byte[] next_out;
        public int next_out_index;
        public int avail_out;
        public long total_out;
        public string msg;
        internal Deflate dstate;
        internal Inflate istate;
        internal int data_type;
        public long adler;
        internal Adler32 _adler = new();

        public int inflateInit() => this.inflateInit( 15 );

        public int inflateInit( bool nowrap ) => this.inflateInit( 15, nowrap );

        public int inflateInit( int w ) => this.inflateInit( w, false );

        public int inflateInit( int w, bool nowrap )
        {
            this.istate = new Inflate();
            return this.istate.inflateInit( this, nowrap ? -w : w );
        }

        public int inflate( int f ) => this.istate == null ? -2 : this.istate.inflate( this, f );

        public int inflateEnd()
        {
            if (this.istate == null)
                return -2;
            int num = this.istate.inflateEnd( this );
            this.istate = null;
            return num;
        }

        public int inflateSync() => this.istate == null ? -2 : this.istate.inflateSync( this );

        public int inflateSetDictionary( byte[] dictionary, int dictLength ) => this.istate == null ? -2 : this.istate.inflateSetDictionary( this, dictionary, dictLength );

        public int deflateInit( int level ) => this.deflateInit( level, 15 );

        public int deflateInit( int level, bool nowrap ) => this.deflateInit( level, 15, nowrap );

        public int deflateInit( int level, int bits ) => this.deflateInit( level, bits, false );

        public int deflateInit( int level, int bits, bool nowrap )
        {
            this.dstate = new Deflate();
            return this.dstate.deflateInit( this, level, nowrap ? -bits : bits );
        }

        public int deflate( int flush ) => this.dstate == null ? -2 : this.dstate.deflate( this, flush );

        public int deflateEnd()
        {
            if (this.dstate == null)
                return -2;
            int num = this.dstate.deflateEnd();
            this.dstate = null;
            return num;
        }

        public int deflateParams( int level, int strategy ) => this.dstate == null ? -2 : this.dstate.deflateParams( this, level, strategy );

        public int deflateSetDictionary( byte[] dictionary, int dictLength ) => this.dstate == null ? -2 : this.dstate.deflateSetDictionary( this, dictionary, dictLength );

        internal void flush_pending()
        {
            int length1 = this.dstate.pending;
            if (length1 > this.avail_out)
                length1 = this.avail_out;
            if (length1 == 0)
                return;
            if (this.dstate.pending_buf.Length > this.dstate.pending_out && this.next_out.Length > this.next_out_index && this.dstate.pending_buf.Length >= this.dstate.pending_out + length1)
            {
                int length2 = this.next_out.Length;
                int num = this.next_out_index + length1;
            }
            Array.Copy( dstate.pending_buf, this.dstate.pending_out, next_out, this.next_out_index, length1 );
            this.next_out_index += length1;
            this.dstate.pending_out += length1;
            this.total_out += length1;
            this.avail_out -= length1;
            this.dstate.pending -= length1;
            if (this.dstate.pending != 0)
                return;
            this.dstate.pending_out = 0;
        }

        internal int read_buf( byte[] buf, int start, int size )
        {
            int num = this.avail_in;
            if (num > size)
                num = size;
            if (num == 0)
                return 0;
            this.avail_in -= num;
            if (this.dstate.noheader == 0)
                this.adler = this._adler.adler32( this.adler, this.next_in, this.next_in_index, num );
            Array.Copy( next_in, this.next_in_index, buf, start, num );
            this.next_in_index += num;
            this.total_in += num;
            return num;
        }

        public void free()
        {
            this.next_in = null;
            this.next_out = null;
            this.msg = null;
            this._adler = null;
        }
    }
}
