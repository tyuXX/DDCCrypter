// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.WrappedGeneratorStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class WrappedGeneratorStream : FilterStream
    {
        private readonly IStreamGenerator gen;

        public WrappedGeneratorStream( IStreamGenerator gen, Stream str )
          : base( str )
        {
            this.gen = gen;
        }

        public override void Close() => this.gen.Close();
    }
}
