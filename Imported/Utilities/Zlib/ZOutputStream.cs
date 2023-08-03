// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.ZOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Zlib
{
    public class ZOutputStream : Stream
    {
        private const int BufferSize = 512;
        protected ZStream z;
        protected int flushLevel = 0;
        protected byte[] buf = new byte[512];
        protected byte[] buf1 = new byte[1];
        protected bool compress;
        protected Stream output;
        protected bool closed;

        public ZOutputStream( Stream output )
          : this( output, null )
        {
        }

        public ZOutputStream( Stream output, ZStream z )
        {
            if (z == null)
            {
                z = new ZStream();
                z.inflateInit();
            }
            this.output = output;
            this.z = z;
            this.compress = false;
        }

        public ZOutputStream( Stream output, int level )
          : this( output, level, false )
        {
        }

        public ZOutputStream( Stream output, int level, bool nowrap )
        {
            this.output = output;
            this.z = new ZStream();
            this.z.deflateInit( level, nowrap );
            this.compress = true;
        }

        public override sealed bool CanRead => false;

        public override sealed bool CanSeek => false;

        public override sealed bool CanWrite => !this.closed;

        public override void Close()
        {
            if (this.closed)
                return;
            this.DoClose();
            base.Close();
        }

        private void DoClose()
        {
            try
            {
                this.Finish();
            }
            catch (IOException ex)
            {
            }
            finally
            {
                this.closed = true;
                this.End();
                Platform.Dispose( this.output );
                this.output = null;
            }
        }

        public virtual void End()
        {
            if (this.z == null)
                return;
            if (this.compress)
                this.z.deflateEnd();
            else
                this.z.inflateEnd();
            this.z.free();
            this.z = null;
        }

        public virtual void Finish()
        {
            do
            {
                this.z.next_out = this.buf;
                this.z.next_out_index = 0;
                this.z.avail_out = this.buf.Length;
                switch (this.compress ? this.z.deflate( 4 ) : this.z.inflate( 4 ))
                {
                    case 0:
                    case 1:
                        int count = this.buf.Length - this.z.avail_out;
                        if (count > 0)
                            this.output.Write( this.buf, 0, count );
                        continue;
                    default:
                        throw new IOException( (this.compress ? "de" : "in") + "flating: " + this.z.msg );
                }
            }
            while (this.z.avail_in > 0 || this.z.avail_out == 0);
            this.Flush();
        }

        public override void Flush() => this.output.Flush();

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

        public override sealed int Read( byte[] buffer, int offset, int count ) => throw new NotSupportedException();

        public override sealed long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

        public override sealed void SetLength( long value ) => throw new NotSupportedException();

        public virtual long TotalIn => this.z.total_in;

        public virtual long TotalOut => this.z.total_out;

        public override void Write( byte[] b, int off, int len )
        {
            if (len == 0)
                return;
            this.z.next_in = b;
            this.z.next_in_index = off;
            this.z.avail_in = len;
            do
            {
                this.z.next_out = this.buf;
                this.z.next_out_index = 0;
                this.z.avail_out = this.buf.Length;
                if ((this.compress ? this.z.deflate( this.flushLevel ) : this.z.inflate( this.flushLevel )) != 0)
                    throw new IOException( (this.compress ? "de" : "in") + "flating: " + this.z.msg );
                this.output.Write( this.buf, 0, this.buf.Length - this.z.avail_out );
            }
            while (this.z.avail_in > 0 || this.z.avail_out == 0);
        }

        public override void WriteByte( byte b )
        {
            this.buf1[0] = b;
            this.Write( this.buf1, 0, 1 );
        }
    }
}
