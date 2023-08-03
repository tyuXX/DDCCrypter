// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Utilities.FilterStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Asn1.Utilities
{
    [Obsolete( "Use Org.BouncyCastle.Utilities.IO.FilterStream" )]
    public class FilterStream : Stream
    {
        protected readonly Stream s;

        [Obsolete( "Use Org.BouncyCastle.Utilities.IO.FilterStream" )]
        public FilterStream( Stream s ) => this.s = s;

        public override bool CanRead => this.s.CanRead;

        public override bool CanSeek => this.s.CanSeek;

        public override bool CanWrite => this.s.CanWrite;

        public override long Length => this.s.Length;

        public override long Position
        {
            get => this.s.Position;
            set => this.s.Position = value;
        }

        public override void Close()
        {
            Platform.Dispose( this.s );
            base.Close();
        }

        public override void Flush() => this.s.Flush();

        public override long Seek( long offset, SeekOrigin origin ) => this.s.Seek( offset, origin );

        public override void SetLength( long value ) => this.s.SetLength( value );

        public override int Read( byte[] buffer, int offset, int count ) => this.s.Read( buffer, offset, count );

        public override int ReadByte() => this.s.ReadByte();

        public override void Write( byte[] buffer, int offset, int count ) => this.s.Write( buffer, offset, count );

        public override void WriteByte( byte value ) => this.s.WriteByte( value );
    }
}
