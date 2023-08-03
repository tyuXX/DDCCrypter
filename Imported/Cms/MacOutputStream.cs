// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.MacOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Cms
{
    internal class MacOutputStream : BaseOutputStream
    {
        private readonly IMac mac;

        internal MacOutputStream( IMac mac ) => this.mac = mac;

        public override void Write( byte[] b, int off, int len ) => this.mac.BlockUpdate( b, off, len );

        public override void WriteByte( byte b ) => this.mac.Update( b );
    }
}
