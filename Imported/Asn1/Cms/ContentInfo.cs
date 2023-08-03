// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.ContentInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class ContentInfo : Asn1Encodable
    {
        private readonly DerObjectIdentifier contentType;
        private readonly Asn1Encodable content;

        public static ContentInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ContentInfo _:
                    return (ContentInfo)obj;
                case Asn1Sequence _:
                    return new ContentInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ) );
            }
        }

        public static ContentInfo GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        private ContentInfo( Asn1Sequence seq )
        {
            this.contentType = seq.Count >= 1 && seq.Count <= 2 ? (DerObjectIdentifier)seq[0] : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[1];
            this.content = asn1TaggedObject.IsExplicit() && asn1TaggedObject.TagNo == 0 ? (Asn1Encodable)asn1TaggedObject.GetObject() : throw new ArgumentException( "Bad tag for 'content'", nameof( seq ) );
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
