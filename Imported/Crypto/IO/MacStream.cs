// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.IO.MacStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.IO
{
    public class MacStream : Stream
    {
        protected readonly Stream stream;
        protected readonly IMac inMac;
        protected readonly IMac outMac;

        public MacStream( Stream stream, IMac readMac, IMac writeMac )
        {
            this.stream = stream;
            this.inMac = readMac;
            this.outMac = writeMac;
        }

        public virtual IMac ReadMac() => this.inMac;

        public virtual IMac WriteMac() => this.outMac;

        public override int Read( byte[] buffer, int offset, int count )
        {
            int len = this.stream.Read( buffer, offset, count );
            if (this.inMac != null && len > 0)
                this.inMac.BlockUpdate( buffer, offset, len );
            return len;
        }

        public override int ReadByte()
        {
            int input = this.stream.ReadByte();
            if (this.inMac != null && input >= 0)
                this.inMac.Update( (byte)input );
            return input;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            if (this.outMac != null && count > 0)
                this.outMac.BlockUpdate( buffer, offset, count );
            this.stream.Write( buffer, offset, count );
        }

        public override void WriteByte( byte b )
        {
            if (this.outMac != null)
                this.outMac.Update( b );
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
