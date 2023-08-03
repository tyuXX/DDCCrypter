// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.IO.DigestStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.IO
{
    public class DigestStream : Stream
    {
        protected readonly Stream stream;
        protected readonly IDigest inDigest;
        protected readonly IDigest outDigest;

        public DigestStream( Stream stream, IDigest readDigest, IDigest writeDigest )
        {
            this.stream = stream;
            this.inDigest = readDigest;
            this.outDigest = writeDigest;
        }

        public virtual IDigest ReadDigest() => this.inDigest;

        public virtual IDigest WriteDigest() => this.outDigest;

        public override int Read( byte[] buffer, int offset, int count )
        {
            int length = this.stream.Read( buffer, offset, count );
            if (this.inDigest != null && length > 0)
                this.inDigest.BlockUpdate( buffer, offset, length );
            return length;
        }

        public override int ReadByte()
        {
            int input = this.stream.ReadByte();
            if (this.inDigest != null && input >= 0)
                this.inDigest.Update( (byte)input );
            return input;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            if (this.outDigest != null && count > 0)
                this.outDigest.BlockUpdate( buffer, offset, count );
            this.stream.Write( buffer, offset, count );
        }

        public override void WriteByte( byte b )
        {
            if (this.outDigest != null)
                this.outDigest.Update( b );
            this.stream.WriteByte( b );
        }

        public override bool CanRead => this.stream.CanRead;

        public override bool CanWrite => this.stream.CanWrite;

        public override bool CanSeek => this.stream.CanSeek;

        public override long Length => this.stream.Length;

        public override long Position
        {
            get => this.stream.Position;
            set => this.stream.Position = value;
        }

        public override void Close()
        {
            Platform.Dispose( this.stream );
            base.Close();
        }

        public override void Flush() => this.stream.Flush();

        public override long Seek( long offset, SeekOrigin origin ) => this.stream.Seek( offset, origin );

        public override void SetLength( long length ) => this.stream.SetLength( length );
    }
}
