// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SignerInputBuffer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class SignerInputBuffer : MemoryStream
    {
        internal void UpdateSigner( ISigner s ) => this.WriteTo( new SignerInputBuffer.SigStream( s ) );

        private class SigStream : BaseOutputStream
        {
            private readonly ISigner s;

            internal SigStream( ISigner s ) => this.s = s;

            public override void WriteByte( byte b ) => this.s.Update( b );

            public override void Write( byte[] buf, int off, int len ) => this.s.BlockUpdate( buf, off, len );
        }
    }
}
