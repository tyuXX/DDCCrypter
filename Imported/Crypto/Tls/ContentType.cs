// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ContentType
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class ContentType
    {
        public const byte change_cipher_spec = 20;
        public const byte alert = 21;
        public const byte handshake = 22;
        public const byte application_data = 23;
        public const byte heartbeat = 24;
    }
}
