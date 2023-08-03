// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.ContentHints
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ess
{
    public class ContentHints : Asn1Encodable
    {
        private readonly DerUtf8String contentDescription;
        private readonly DerObjectIdentifier contentType;

        public static ContentHints GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case ContentHints _:
                    return (ContentHints)o;
                case Asn1Sequence _:
                    return new ContentHints( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in 'ContentHints' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        private ContentHints( Asn1Sequence seq )
        {
            IAsn1Convertible asn1Convertible = seq[0];
            if (asn1Convertible.ToAsn1Object() is DerUtf8String)
            {
                this.contentDescription = DerUtf8String.GetInstance( asn1Convertible );
                this.contentType = DerObjectIdentifier.GetInstance( seq[1] );
            }
            else
                this.contentType = DerObjectIdentifier.GetInstance( seq[0] );
        }

        public ContentHints( DerObjectIdentifier contentType )
        {
            this.contentType = contentType;
            this.contentDescription = null;
        }

        public ContentHints( DerObjectIdentifier contentType, DerUtf8String contentDescription )
        {
            this.contentType = contentType;
            this.contentDescription = contentDescription;
        }

        public DerObjectIdentifier ContentType => this.contentType;

        public DerUtf8String ContentDescription => this.contentDescription;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.contentDescription != null)
                v.Add( contentDescription );
            v.Add( contentType );
            return new DerSequence( v );
        }
    }
}
