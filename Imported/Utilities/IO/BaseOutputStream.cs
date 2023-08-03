// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.BaseOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
    public abstract class BaseOutputStream : Stream
    {
        private bool closed;

        public override sealed bool CanRead => false;

        public override sealed bool CanSeek => false;

        public override sealed bool CanWrite => !this.closed;

        public override void Close()
        {
            this.closed = true;
            base.Close();
        }

        public override void Flush()
        {
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

        public override void Write( byte[] buffer, int offset, int count )
        {
            int num = offset + count;
            for (int index = offset; index < num; ++index)
                this.WriteByte( buffer[index] );
        }

        public virtual void Write( params byte[] buffer ) => this.Write( buffer, 0, buffer.Length );
    }
}
