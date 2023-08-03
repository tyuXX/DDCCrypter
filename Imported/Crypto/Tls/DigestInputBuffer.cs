// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DigestInputBuffer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DigestInputBuffer : MemoryStream
    {
        internal void UpdateDigest( IDigest d ) => this.WriteTo( new DigestInputBuffer.DigStream( d ) );

        private class DigStream : BaseOutputStream
        {
            private readonly IDigest d;

            internal DigStream( IDigest d ) => this.d = d;

            public override void WriteByte( byte b ) => this.d.Update( b );

            public override void Write( byte[] buf, int off, int len ) => this.d.BlockUpdate( buf, off, len );
        }
    }
}
