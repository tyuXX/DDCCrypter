// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.CertChainType
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class CertChainType
    {
        public const byte individual_certs = 0;
        public const byte pkipath = 1;

        public static bool IsValid( byte certChainType ) => certChainType >= 0 && certChainType <= 1;
    }
}
