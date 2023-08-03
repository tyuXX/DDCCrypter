// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.ContentInfoParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class ContentInfoParser
    {
        private DerObjectIdentifier contentType;
        private Asn1TaggedObjectParser content;

        public ContentInfoParser( Asn1SequenceParser seq )
        {
            this.contentType = (DerObjectIdentifier)seq.ReadObject();
            this.content = (Asn1TaggedObjectParser)seq.ReadObject();
        }

        public DerObjectIdentifier ContentType => this.contentType;

        public IAsn1Convertible GetContent( int tag ) => this.content == null ? null : this.content.GetObjectParser( tag, true );
    }
}
