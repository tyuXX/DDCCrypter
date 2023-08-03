// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ByteQueueStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ByteQueueStream : Stream
    {
        private readonly ByteQueue buffer;

        public ByteQueueStream() => this.buffer = new ByteQueue();

        public virtual int Available => this.buffer.Available;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override void Flush()
        {
        }

        public override long Length => throw new NotSupportedException();

        public virtual int Peek( byte[] buf )
        {
            int len = System.Math.Min( this.buffer.Available, buf.Length );
            this.buffer.Read( buf, 0, len, 0 );
            return len;
        }

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public virtual int Read( byte[] buf ) => this.Read( buf, 0, buf.Length );

        public override int Read( byte[] buf, int off, int len )
        {
            int len1 = System.Math.Min( this.buffer.Available, len );
            this.buffer.RemoveData( buf, off, len1, 0 );
            return len1;
        }

        public override int ReadByte() => this.buffer.Available == 0 ? -1 : this.buffer.RemoveData( 1, 0 )[0] & byte.MaxValue;

        public override long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

        public override void SetLength( long value ) => throw new NotSupportedException();

        public virtual int Skip( int n )
        {
            int i = System.Math.Min( this.buffer.Available, n );
            this.buffer.RemoveData( i );
            return i;
        }

        public virtual void Write( byte[] buf ) => this.buffer.AddData( buf, 0, buf.Length );

        public override void Write( byte[] buf, int off, int len ) => this.buffer.AddData( buf, off, len );

        public override void WriteByte( byte b ) => this.buffer.AddData( new byte[1]
        {
      b
        }, 0, 1 );
    }
}
