// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.ContentInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class ContentInfo : Asn1Encodable
    {
        private readonly DerObjectIdentifier contentType;
        private readonly Asn1Encodable content;

        public static ContentInfo GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is ContentInfo contentInfo ? contentInfo : new ContentInfo( Asn1Sequence.GetInstance( obj ) );
        }

        private ContentInfo( Asn1Sequence seq )
        {
            this.contentType = (DerObjectIdentifier)seq[0];
            if (seq.Count <= 1)
                return;
            this.content = ((Asn1TaggedObject)seq[1]).GetObject();
        }

        public ContentInfo( DerObjectIdentifier contentType, Asn1Encodable content )
        {
            this.contentType = contentType;
            this.content = content;
        }

        public DerObjectIdentifier ContentType => this.contentType;

        public Asn1Encodable Content => this.content;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         contentType
            } );
            if (this.content != null)
                v.Add( new BerTaggedObject( 0, this.content ) );
            return new BerSequence( v );
        }
    }
}
