// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ClientCertificateType
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class ClientCertificateType
    {
        public const byte rsa_sign = 1;
        public const byte dss_sign = 2;
        public const byte rsa_fixed_dh = 3;
        public const byte dss_fixed_dh = 4;
        public const byte rsa_ephemeral_dh_RESERVED = 5;
        public const byte dss_ephemeral_dh_RESERVED = 6;
        public const byte fortezza_dms_RESERVED = 20;
        public const byte ecdsa_sign = 64;
        public const byte rsa_fixed_ecdh = 65;
        public const byte ecdsa_fixed_ecdh = 66;
    }
}
