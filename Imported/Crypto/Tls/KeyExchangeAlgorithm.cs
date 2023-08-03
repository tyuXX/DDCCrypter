﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.KeyExchangeAlgorithm
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class KeyExchangeAlgorithm
    {
        public const int NULL = 0;
        public const int RSA = 1;
        public const int RSA_EXPORT = 2;
        public const int DHE_DSS = 3;
        public const int DHE_DSS_EXPORT = 4;
        public const int DHE_RSA = 5;
        public const int DHE_RSA_EXPORT = 6;
        public const int DH_DSS = 7;
        public const int DH_DSS_EXPORT = 8;
        public const int DH_RSA = 9;
        public const int DH_RSA_EXPORT = 10;
        public const int DH_anon = 11;
        public const int DH_anon_EXPORT = 12;
        public const int PSK = 13;
        public const int DHE_PSK = 14;
        public const int RSA_PSK = 15;
        public const int ECDH_ECDSA = 16;
        public const int ECDHE_ECDSA = 17;
        public const int ECDH_RSA = 18;
        public const int ECDHE_RSA = 19;
        public const int ECDH_anon = 20;
        public const int SRP = 21;
        public const int SRP_DSS = 22;
        public const int SRP_RSA = 23;
        public const int ECDHE_PSK = 24;
    }
}
