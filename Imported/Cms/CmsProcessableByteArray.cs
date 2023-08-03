// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsProcessableByteArray
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsProcessableByteArray : CmsProcessable, CmsReadable
    {
        private readonly byte[] bytes;

        public CmsProcessableByteArray( byte[] bytes ) => this.bytes = bytes;

        public virtual Stream GetInputStream() => new MemoryStream( this.bytes, false );

        public virtual void Write( Stream zOut ) => zOut.Write( this.bytes, 0, this.bytes.Length );

        [Obsolete]
        public virtual object GetContent() => this.bytes.Clone();
    }
}
