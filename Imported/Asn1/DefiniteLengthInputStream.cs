// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DefiniteLengthInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    internal class DefiniteLengthInputStream : LimitedInputStream
    {
        private static readonly byte[] EmptyBytes = new byte[0];
        private readonly int _originalLength;
        private int _remaining;

        internal DefiniteLengthInputStream( Stream inStream, int length )
          : base( inStream, length )
        {
            this._originalLength = length >= 0 ? length : throw new ArgumentException( "negative lengths not allowed", nameof( length ) );
            this._remaining = length;
            if (length != 0)
                return;
            this.SetParentEofDetect( true );
        }

        internal int Remaining => this._remaining;

        public override int ReadByte()
        {
            if (this._remaining == 0)
                return -1;
            int num = this._in.ReadByte();
            if (num < 0)
                throw new EndOfStreamException( "DEF length " + _originalLength + " object truncated by " + _remaining );
            if (--this._remaining == 0)
                this.SetParentEofDetect( true );
            return num;
        }

        public override int Read( byte[] buf, int off, int len )
        {
            if (this._remaining == 0)
                return 0;
            int count = System.Math.Min( len, this._remaining );
            int num = this._in.Read( buf, off, count );
            if (num < 1)
                throw new EndOfStreamException( "DEF length " + _originalLength + " object truncated by " + _remaining );
            if ((this._remaining -= num) == 0)
                this.SetParentEofDetect( true );
            return num;
        }

        internal void ReadAllIntoByteArray( byte[] buf )
        {
            if (this._remaining != buf.Length)
                throw new ArgumentException( "buffer length not right for data" );
            if ((this._remaining -= Streams.ReadFully( this._in, buf )) != 0)
                throw new EndOfStreamException( "DEF length " + _originalLength + " object truncated by " + _remaining );
            this.SetParentEofDetect( true );
        }

        internal byte[] ToArray()
        {
            if (this._remaining == 0)
                return EmptyBytes;
            byte[] buf = new byte[this._remaining];
            if ((this._remaining -= Streams.ReadFully( this._in, buf )) != 0)
                throw new EndOfStreamException( "DEF length " + _originalLength + " object truncated by " + _remaining );
            this.SetParentEofDetect( true );
            return buf;
        }
    }
}
