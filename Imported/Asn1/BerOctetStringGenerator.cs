// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerOctetStringGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerOctetStringGenerator : BerGenerator
    {
        public BerOctetStringGenerator( Stream outStream )
          : base( outStream )
        {
            this.WriteBerHeader( 36 );
        }

        public BerOctetStringGenerator( Stream outStream, int tagNo, bool isExplicit )
          : base( outStream, tagNo, isExplicit )
        {
            this.WriteBerHeader( 36 );
        }

        public Stream GetOctetOutputStream() => this.GetOctetOutputStream( new byte[1000] );

        public Stream GetOctetOutputStream( int bufSize ) => bufSize >= 1 ? this.GetOctetOutputStream( new byte[bufSize] ) : this.GetOctetOutputStream();

        public Stream GetOctetOutputStream( byte[] buf ) => new BerOctetStringGenerator.BufferedBerOctetStream( this, buf );

        private class BufferedBerOctetStream : BaseOutputStream
        {
            private byte[] _buf;
            private int _off;
            private readonly BerOctetStringGenerator _gen;
            private readonly DerOutputStream _derOut;

            internal BufferedBerOctetStream( BerOctetStringGenerator gen, byte[] buf )
            {
                this._gen = gen;
                this._buf = buf;
                this._off = 0;
                this._derOut = new DerOutputStream( this._gen.Out );
            }

            public override void WriteByte( byte b )
            {
                this._buf[this._off++] = b;
                if (this._off != this._buf.Length)
                    return;
                DerOctetString.Encode( this._derOut, this._buf, 0, this._off );
                this._off = 0;
            }

            public override void Write( byte[] buf, int offset, int len )
            {
                int length;
                for (; len > 0; len -= length)
                {
                    length = System.Math.Min( len, this._buf.Length - this._off );
                    if (length == this._buf.Length)
                    {
                        DerOctetString.Encode( this._derOut, buf, offset, length );
                    }
                    else
                    {
                        Array.Copy( buf, offset, _buf, this._off, length );
                        this._off += length;
                        if (this._off < this._buf.Length)
                            break;
                        DerOctetString.Encode( this._derOut, this._buf, 0, this._off );
                        this._off = 0;
                    }
                    offset += length;
                }
            }

            public override void Close()
            {
                if (this._off != 0)
                    DerOctetString.Encode( this._derOut, this._buf, 0, this._off );
                this._gen.WriteBerEnd();
                base.Close();
            }
        }
    }
}
