// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.EncryptedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class EncryptedData : Asn1Encodable
    {
        private readonly DerInteger version;
        private readonly EncryptedContentInfo encryptedContentInfo;
        private readonly Asn1Set unprotectedAttrs;

        public static EncryptedData GetInstance( object obj )
        {
            switch (obj)
            {
                case EncryptedData _:
                    return (EncryptedData)obj;
                case Asn1Sequence _:
                    return new EncryptedData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid EncryptedData: " + Platform.GetTypeName( obj ) );
            }
        }

        public EncryptedData( EncryptedContentInfo encInfo )
          : this( encInfo, null )
        {
        }

        public EncryptedData( EncryptedContentInfo encInfo, Asn1Set unprotectedAttrs )
        {
            if (encInfo == null)
                throw new ArgumentNullException( nameof( encInfo ) );
            this.version = new DerInteger( unprotectedAttrs == null ? 0 : 2 );
            this.encryptedContentInfo = encInfo;
            this.unprotectedAttrs = unprotectedAttrs;
        }

        private EncryptedData( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.version = seq.Count >= 2 && seq.Count <= 3 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.encryptedContentInfo = EncryptedContentInfo.GetInstance( seq[1] );
            if (seq.Count <= 2)
                return;
            this.unprotectedAttrs = Asn1Set.GetInstance( (Asn1TaggedObject)seq[2], false );
        }

        public virtual DerInteger Version => this.version;

        public virtual EncryptedContentInfo EncryptedContentInfo => this.encryptedContentInfo;

        public virtual Asn1Set UnprotectedAttrs => this.unprotectedAttrs;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         version,
         encryptedContentInfo
            } );
            if (this.unprotectedAttrs != null)
                v.Add( new BerTaggedObject( false, 1, unprotectedAttrs ) );
            return new BerSequence( v );
        }
    }
}
