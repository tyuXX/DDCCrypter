// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Zlib.ZInflaterInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.Zlib
{
    [Obsolete( "Use 'ZInputStream' instead" )]
    public class ZInflaterInputStream : Stream
    {
        private const int BUFSIZE = 4192;
        protected ZStream z = new();
        protected int flushLevel = 0;
        protected byte[] buf = new byte[4192];
        private byte[] buf1 = new byte[1];
        protected Stream inp = null;
        private bool nomoreinput = false;

        public ZInflaterInputStream( Stream inp )
          : this( inp, false )
        {
        }

        public ZInflaterInputStream( Stream inp, bool nowrap )
        {
            this.inp = inp;
            this.z.inflateInit( nowrap );
            this.z.next_in = this.buf;
            this.z.next_in_index = 0;
            this.z.avail_in = 0;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

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
        }

        public override long Seek( long offset, SeekOrigin origin ) => 0;

        public override void SetLength( long value )
        {
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
                    this.z.avail_in = this.inp.Read( this.buf, 0, 4192 );
                    if (this.z.avail_in <= 0)
                    {
                        this.z.avail_in = 0;
                        this.nomoreinput = true;
                    }
                }
                num = this.z.inflate( this.flushLevel );
                if (this.nomoreinput && num == -5)
                    return 0;
                if (num != 0 && num != 1)
                    throw new IOException( "inflating: " + this.z.msg );
                if ((this.nomoreinput || num == 1) && this.z.avail_out == len)
                    return 0;
            }
            while (this.z.avail_out == len && num == 0);
            return len - this.z.avail_out;
        }

        public override void Flush() => this.inp.Flush();

        public override void WriteByte( byte b )
        {
        }

        public override void Close()
        {
            Platform.Dispose( this.inp );
            base.Close();
        }

        public override int ReadByte() => this.Read( this.buf1, 0, 1 ) <= 0 ? -1 : this.buf1[0] & byte.MaxValue;
    }
}
