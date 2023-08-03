// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.PushbackStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
    public class PushbackStream : FilterStream
    {
        private int buf = -1;

        public PushbackStream( Stream s )
          : base( s )
        {
        }

        public override int ReadByte()
        {
            if (this.buf == -1)
                return base.ReadByte();
            int buf = this.buf;
            this.buf = -1;
            return buf;
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            if (this.buf == -1 || count <= 0)
                return base.Read( buffer, offset, count );
            buffer[offset] = (byte)this.buf;
            this.buf = -1;
            return 1;
        }

        public virtual void Unread( int b )
        {
            if (this.buf != -1)
                throw new InvalidOperationException( "Can only push back one byte" );
            this.buf = b & byte.MaxValue;
        }
    }
}
