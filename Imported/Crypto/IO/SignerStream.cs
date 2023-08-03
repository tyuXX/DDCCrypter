// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.IO.SignerStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.IO
{
    public class SignerStream : Stream
    {
        protected readonly Stream stream;
        protected readonly ISigner inSigner;
        protected readonly ISigner outSigner;

        public SignerStream( Stream stream, ISigner readSigner, ISigner writeSigner )
        {
            this.stream = stream;
            this.inSigner = readSigner;
            this.outSigner = writeSigner;
        }

        public virtual ISigner ReadSigner() => this.inSigner;

        public virtual ISigner WriteSigner() => this.outSigner;

        public override int Read( byte[] buffer, int offset, int count )
        {
            int length = this.stream.Read( buffer, offset, count );
            if (this.inSigner != null && length > 0)
                this.inSigner.BlockUpdate( buffer, offset, length );
            return length;
        }

        public override int ReadByte()
        {
            int input = this.stream.ReadByte();
            if (this.inSigner != null && input >= 0)
                this.inSigner.Update( (byte)input );
            return input;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            if (this.outSigner != null && count > 0)
                this.outSigner.BlockUpdate( buffer, offset, count );
            this.stream.Write( buffer, offset, count );
        }

        public override void WriteByte( byte b )
        {
            if (this.outSigner != null)
                this.outSigner.Update( b );
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
