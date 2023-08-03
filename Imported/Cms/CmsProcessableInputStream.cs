// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsProcessableInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsProcessableInputStream : CmsProcessable, CmsReadable
    {
        private readonly Stream input;
        private bool used = false;

        public CmsProcessableInputStream( Stream input ) => this.input = input;

        public virtual Stream GetInputStream()
        {
            this.CheckSingleUsage();
            return this.input;
        }

        public virtual void Write( Stream output )
        {
            this.CheckSingleUsage();
            Streams.PipeAll( this.input, output );
            Platform.Dispose( this.input );
        }

        [Obsolete]
        public virtual object GetContent() => this.GetInputStream();

        protected virtual void CheckSingleUsage()
        {
            lock (this)
                this.used = !this.used ? true : throw new InvalidOperationException( "CmsProcessableInputStream can only be used once" );
        }
    }
}
