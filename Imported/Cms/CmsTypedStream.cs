// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsTypedStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsTypedStream
    {
        private const int BufferSize = 32768;
        private readonly string _oid;
        private readonly Stream _in;

        public CmsTypedStream( Stream inStream )
          : this( PkcsObjectIdentifiers.Data.Id, inStream, 32768 )
        {
        }

        public CmsTypedStream( string oid, Stream inStream )
          : this( oid, inStream, 32768 )
        {
        }

        public CmsTypedStream( string oid, Stream inStream, int bufSize )
        {
            this._oid = oid;
            this._in = new CmsTypedStream.FullReaderStream( new BufferedStream( inStream, bufSize ) );
        }

        public string ContentType => this._oid;

        public Stream ContentStream => this._in;

        public void Drain()
        {
            Streams.Drain( this._in );
            Platform.Dispose( this._in );
        }

        private class FullReaderStream : FilterStream
        {
            internal FullReaderStream( Stream input )
              : base( input )
            {
            }

            public override int Read( byte[] buf, int off, int len ) => Streams.ReadFully( this.s, buf, off, len );
        }
    }
}
