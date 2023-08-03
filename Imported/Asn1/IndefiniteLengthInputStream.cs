// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IndefiniteLengthInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    internal class IndefiniteLengthInputStream : LimitedInputStream
    {
        private int _lookAhead;
        private bool _eofOn00 = true;

        internal IndefiniteLengthInputStream( Stream inStream, int limit )
          : base( inStream, limit )
        {
            this._lookAhead = this.RequireByte();
            this.CheckForEof();
        }

        internal void SetEofOn00( bool eofOn00 )
        {
            this._eofOn00 = eofOn00;
            if (!this._eofOn00)
                return;
            this.CheckForEof();
        }

        private bool CheckForEof()
        {
            if (this._lookAhead != 0)
                return this._lookAhead < 0;
            if (this.RequireByte() != 0)
                throw new IOException( "malformed end-of-contents marker" );
            this._lookAhead = -1;
            this.SetParentEofDetect( true );
            return true;
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            if (this._eofOn00 || count <= 1)
                return base.Read( buffer, offset, count );
            if (this._lookAhead < 0)
                return 0;
            int num = this._in.Read( buffer, offset + 1, count - 1 );
            if (num <= 0)
                throw new EndOfStreamException();
            buffer[offset] = (byte)this._lookAhead;
            this._lookAhead = this.RequireByte();
            return num + 1;
        }

        public override int ReadByte()
        {
            if (this._eofOn00 && this.CheckForEof())
                return -1;
            int lookAhead = this._lookAhead;
            this._lookAhead = this.RequireByte();
            return lookAhead;
        }

        private int RequireByte()
        {
            int num = this._in.ReadByte();
            return num >= 0 ? num : throw new EndOfStreamException();
        }
    }
}
