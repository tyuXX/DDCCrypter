// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ExporterLabel
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class ExporterLabel
    {
        public const string client_finished = "client finished";
        public const string server_finished = "server finished";
        public const string master_secret = "master secret";
        public const string key_expansion = "key expansion";
        public const string client_EAP_encryption = "client EAP encryption";
        public const string ttls_keying_material = "ttls keying material";
        public const string ttls_challenge = "ttls challenge";
        public const string dtls_srtp = "EXTRACTOR-dtls_srtp";
        public static readonly string extended_master_secret = "extended master secret";
    }
}
