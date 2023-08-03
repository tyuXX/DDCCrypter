// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.ZDeflaterOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Zlib
{
    [Obsolete( "Use 'ZOutputStream' instead" )]
    public class ZDeflaterOutputStream : Stream
    {
        private const int BUFSIZE = 4192;
        protected ZStream z = new ZStream();
        protected int flushLevel = 0;
        protected byte[] buf = new byte[4192];
        private byte[] buf1 = new byte[1];
        protected Stream outp;

        public ZDeflaterOutputStream( Stream outp )
          : this( outp, 6, false )
        {
        }

        public ZDeflaterOutputStream( Stream outp, int level )
          : this( outp, level, false )
        {
        }

        public ZDeflaterOutputStream( Stream outp, int level, bool nowrap )
        {
            this.outp = outp;
            this.z.deflateInit( level, nowrap );
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position
        {
            get => 0;
            set
            {
            }
        }

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
                this.z.avail_out = 4192;
                if (this.z.deflate( this.flushLevel ) != 0)
                    throw new IOException( "deflating: " + this.z.msg );
                if (this.z.avail_out < 4192)
                    this.outp.Write( this.buf, 0, 4192 - this.z.avail_out );
            }
            while (this.z.avail_in > 0 || this.z.avail_out == 0);
        }

        public override long Seek( long offset, SeekOrigin origin ) => 0;

        public override void SetLength( long value )
        {
        }

        public override int Read( byte[] buffer, int offset, int count ) => 0;

        public override void Flush() => this.outp.Flush();

        public override void WriteByte( byte b )
        {
            this.buf1[0] = b;
            this.Write( this.buf1, 0, 1 );
        }

        public void Finish()
        {
            do
            {
                this.z.next_out = this.buf;
                this.z.next_out_index = 0;
                this.z.avail_out = 4192;
                switch (this.z.deflate( 4 ))
                {
                    case 0:
                    case 1:
                        if (4192 - this.z.avail_out > 0)
                            this.outp.Write( this.buf, 0, 4192 - this.z.avail_out );
                        continue;
                    default:
                        throw new IOException( "deflating: " + this.z.msg );
                }
            }
            while (this.z.avail_in > 0 || this.z.avail_out == 0);
            this.Flush();
        }

        public void End()
        {
            if (this.z == null)
                return;
            this.z.deflateEnd();
            this.z.free();
            this.z = null;
        }

        public override void Close()
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
                this.End();
                Platform.Dispose( this.outp );
                this.outp = null;
            }
            base.Close();
        }
    }
}
