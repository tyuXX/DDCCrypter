// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsNullCompression
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsNullCompression : TlsCompression
    {
        public virtual Stream Compress( Stream output ) => output;

        public virtual Stream Decompress( Stream output ) => output;
    }
}
