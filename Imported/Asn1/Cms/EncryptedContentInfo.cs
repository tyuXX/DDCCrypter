// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.EncryptedContentInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class EncryptedContentInfo : Asn1Encodable
    {
        private DerObjectIdentifier contentType;
        private AlgorithmIdentifier contentEncryptionAlgorithm;
        private Asn1OctetString encryptedContent;

        public EncryptedContentInfo(
          DerObjectIdentifier contentType,
          AlgorithmIdentifier contentEncryptionAlgorithm,
          Asn1OctetString encryptedContent )
        {
            this.contentType = contentType;
            this.contentEncryptionAlgorithm = contentEncryptionAlgorithm;
            this.encryptedContent = encryptedContent;
        }

        public EncryptedContentInfo( Asn1Sequence seq )
        {
            this.contentType = (DerObjectIdentifier)seq[0];
            this.contentEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( seq[1] );
            if (seq.Count <= 2)
                return;
            this.encryptedContent = Asn1OctetString.GetInstance( (Asn1TaggedObject)seq[2], false );
        }

        public static EncryptedContentInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case EncryptedContentInfo _:
                    return (EncryptedContentInfo)obj;
                case Asn1Sequence _:
                    return new EncryptedContentInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid EncryptedContentInfo: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerObjectIdentifier ContentType => this.contentType;

        public AlgorithmIdentifier ContentEncryptionAlgorithm => this.contentEncryptionAlgorithm;

        public Asn1OctetString EncryptedContent => this.encryptedContent;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         contentType,
         contentEncryptionAlgorithm
            } );
            if (this.encryptedContent != null)
                v.Add( new BerTaggedObject( false, 0, encryptedContent ) );
            return new BerSequence( v );
        }
    }
}
