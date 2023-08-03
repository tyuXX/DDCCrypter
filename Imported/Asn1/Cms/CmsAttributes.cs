// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.CmsAttributes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;

namespace Org.BouncyCastle.Asn1.Cms
{
    public abstract class CmsAttributes
    {
        public static readonly DerObjectIdentifier ContentType = PkcsObjectIdentifiers.Pkcs9AtContentType;
        public static readonly DerObjectIdentifier MessageDigest = PkcsObjectIdentifiers.Pkcs9AtMessageDigest;
        public static readonly DerObjectIdentifier SigningTime = PkcsObjectIdentifiers.Pkcs9AtSigningTime;
        public static readonly DerObjectIdentifier CounterSignature = PkcsObjectIdentifiers.Pkcs9AtCounterSignature;
        public static readonly DerObjectIdentifier ContentHint = PkcsObjectIdentifiers.IdAAContentHint;
    }
}
