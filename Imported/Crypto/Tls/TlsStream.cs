// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class TlsStream : Stream
    {
        private readonly TlsProtocol handler;

        internal TlsStream( TlsProtocol handler ) => this.handler = handler;

        public override bool CanRead => !this.handler.IsClosed;

        public override bool CanSeek => false;

        public override bool CanWrite => !this.handler.IsClosed;

        public override void Close()
        {
            this.handler.Close();
            base.Close();
        }

        public override void Flush() => this.handler.Flush();

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read( byte[] buf, int off, int len ) => this.handler.ReadApplicationData( buf, off, len );

        public override int ReadByte()
        {
            byte[] buffer = new byte[1];
            return this.Read( buffer, 0, 1 ) <= 0 ? -1 : buffer[0];
        }

        public override long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

        public override void SetLength( long value ) => throw new NotSupportedException();

        public override void Write( byte[] buf, int off, int len ) => this.handler.WriteData( buf, off, len );

        public override void WriteByte( byte b ) => this.handler.WriteData( new byte[1]
        {
      b
        }, 0, 1 );
    }
}
