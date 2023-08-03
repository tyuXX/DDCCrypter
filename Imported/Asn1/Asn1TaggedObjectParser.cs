// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1TaggedObjectParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public interface Asn1TaggedObjectParser : IAsn1Convertible
    {
        int TagNo { get; }

        IAsn1Convertible GetObjectParser( int tag, bool isExplicit );
    }
}
