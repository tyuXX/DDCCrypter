// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsDeflateCompression
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Zlib;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsDeflateCompression : TlsCompression
    {
        public const int LEVEL_NONE = 0;
        public const int LEVEL_FASTEST = 1;
        public const int LEVEL_SMALLEST = 9;
        public const int LEVEL_DEFAULT = -1;
        protected readonly ZStream zIn;
        protected readonly ZStream zOut;

        public TlsDeflateCompression()
          : this( -1 )
        {
        }

        public TlsDeflateCompression( int level )
        {
            this.zIn = new ZStream();
            this.zIn.inflateInit();
            this.zOut = new ZStream();
            this.zOut.deflateInit( level );
        }

        public virtual Stream Compress( Stream output ) => new TlsDeflateCompression.DeflateOutputStream( output, this.zOut, true );

        public virtual Stream Decompress( Stream output ) => new TlsDeflateCompression.DeflateOutputStream( output, this.zIn, false );

        protected class DeflateOutputStream : ZOutputStream
        {
            public DeflateOutputStream( Stream output, ZStream z, bool compress )
              : base( output, z )
            {
                this.compress = compress;
                this.FlushMode = 2;
            }

            public override void Flush()
            {
            }
        }
    }
}
