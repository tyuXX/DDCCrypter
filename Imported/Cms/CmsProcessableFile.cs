// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsProcessableFile
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsProcessableFile : CmsProcessable, CmsReadable
    {
        private const int DefaultBufSize = 32768;
        private readonly FileInfo _file;
        private readonly int _bufSize;

        public CmsProcessableFile( FileInfo file )
          : this( file, 32768 )
        {
        }

        public CmsProcessableFile( FileInfo file, int bufSize )
        {
            this._file = file;
            this._bufSize = bufSize;
        }

        public virtual Stream GetInputStream() => new FileStream( this._file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, this._bufSize );

        public virtual void Write( Stream zOut )
        {
            Stream inputStream = this.GetInputStream();
            Streams.PipeAll( inputStream, zOut );
            Platform.Dispose( inputStream );
        }

        [Obsolete]
        public virtual object GetContent() => _file;
    }
}
