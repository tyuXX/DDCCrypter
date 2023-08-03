// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.HandshakeType
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class HandshakeType
    {
        public const byte hello_request = 0;
        public const byte client_hello = 1;
        public const byte server_hello = 2;
        public const byte certificate = 11;
        public const byte server_key_exchange = 12;
        public const byte certificate_request = 13;
        public const byte server_hello_done = 14;
        public const byte certificate_verify = 15;
        public const byte client_key_exchange = 16;
        public const byte finished = 20;
        public const byte certificate_url = 21;
        public const byte certificate_status = 22;
        public const byte hello_verify_request = 3;
        public const byte supplemental_data = 23;
        public const byte session_ticket = 4;
    }
}
