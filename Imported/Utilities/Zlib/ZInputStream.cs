// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.ZInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Zlib
{
    public class ZInputStream : Stream
    {
        private const int BufferSize = 512;
        protected ZStream z = new ZStream();
        protected int flushLevel = 0;
        protected byte[] buf = new byte[512];
        protected byte[] buf1 = new byte[1];
        protected bool compress;
        protected Stream input;
        protected bool closed;
        private bool nomoreinput = false;

        public ZInputStream( Stream input )
          : this( input, false )
        {
        }

        public ZInputStream( Stream input, bool nowrap )
        {
            this.input = input;
            this.z.inflateInit( nowrap );
            this.compress = false;
            this.z.next_in = this.buf;
            this.z.next_in_index = 0;
            this.z.avail_in = 0;
        }

        public ZInputStream( Stream input, int level )
        {
            this.input = input;
            this.z.deflateInit( level );
            this.compress = true;
            this.z.next_in = this.buf;
            this.z.next_in_index = 0;
            this.z.avail_in = 0;
        }

        public override sealed bool CanRead => !this.closed;

        public override sealed bool CanSeek => false;

        public override sealed bool CanWrite => false;

        public override void Close()
        {
            if (this.closed)
                return;
            this.closed = true;
            Platform.Dispose( this.input );
            base.Close();
        }

        public override sealed void Flush()
        {
        }

        public virtual int FlushMode
        {
            get => this.flushLevel;
            set => this.flushLevel = value;
        }

        public override sealed long Length => throw new NotSupportedException();

        public override sealed long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read( byte[] b, int off, int len )
        {
            if (len == 0)
                return 0;
            this.z.next_out = b;
            this.z.next_out_index = off;
            this.z.avail_out = len;
            int num;
            do
            {
                if (this.z.avail_in == 0 && !this.nomoreinput)
                {
                    this.z.next_in_index = 0;
                    this.z.avail_in = this.input.Read( this.buf, 0, this.buf.Length );
                    if (this.z.avail_in <= 0)
                    {
                        this.z.avail_in = 0;
                        this.nomoreinput = true;
                    }
                }
                num = this.compress ? this.z.deflate( this.flushLevel ) : this.z.inflate( this.flushLevel );
                if (this.nomoreinput && num == -5)
                    return 0;
                if (num != 0 && num != 1)
                    throw new IOException( (this.compress ? "de" : "in") + "flating: " + this.z.msg );
                if ((this.nomoreinput || num == 1) && this.z.avail_out == len)
                    return 0;
            }
            while (this.z.avail_out == len && num == 0);
            return len - this.z.avail_out;
        }

        public override int ReadByte() => this.Read( this.buf1, 0, 1 ) <= 0 ? -1 : this.buf1[0];

        public override sealed long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

        public override sealed void SetLength( long value ) => throw new NotSupportedException();

        public virtual long TotalIn => this.z.total_in;

        public virtual long TotalOut => this.z.total_out;

        public override sealed void Write( byte[] buffer, int offset, int count ) => throw new NotSupportedException();
    }
}
