// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsAuthEnvelopedGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Nist;

namespace Org.BouncyCastle.Cms
{
    internal class CmsAuthEnvelopedGenerator
    {
        public static readonly string Aes128Ccm = NistObjectIdentifiers.IdAes128Ccm.Id;
        public static readonly string Aes192Ccm = NistObjectIdentifiers.IdAes192Ccm.Id;
        public static readonly string Aes256Ccm = NistObjectIdentifiers.IdAes256Ccm.Id;
        public static readonly string Aes128Gcm = NistObjectIdentifiers.IdAes128Gcm.Id;
        public static readonly string Aes192Gcm = NistObjectIdentifiers.IdAes192Gcm.Id;
        public static readonly string Aes256Gcm = NistObjectIdentifiers.IdAes256Gcm.Id;
    }
}
