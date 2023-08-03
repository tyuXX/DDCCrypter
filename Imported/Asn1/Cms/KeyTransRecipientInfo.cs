// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.KeyTransRecipientInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class KeyTransRecipientInfo : Asn1Encodable
    {
        private DerInteger version;
        private RecipientIdentifier rid;
        private AlgorithmIdentifier keyEncryptionAlgorithm;
        private Asn1OctetString encryptedKey;

        public KeyTransRecipientInfo(
          RecipientIdentifier rid,
          AlgorithmIdentifier keyEncryptionAlgorithm,
          Asn1OctetString encryptedKey )
        {
            this.version = !(rid.ToAsn1Object() is Asn1TaggedObject) ? new DerInteger( 0 ) : new DerInteger( 2 );
            this.rid = rid;
            this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
            this.encryptedKey = encryptedKey;
        }

        public KeyTransRecipientInfo( Asn1Sequence seq )
        {
            this.version = (DerInteger)seq[0];
            this.rid = RecipientIdentifier.GetInstance( seq[1] );
            this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( seq[2] );
            this.encryptedKey = (Asn1OctetString)seq[3];
        }

        public static KeyTransRecipientInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case KeyTransRecipientInfo _:
                    return (KeyTransRecipientInfo)obj;
                case Asn1Sequence _:
                    return new KeyTransRecipientInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Illegal object in KeyTransRecipientInfo: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public RecipientIdentifier RecipientIdentifier => this.rid;

        public AlgorithmIdentifier KeyEncryptionAlgorithm => this.keyEncryptionAlgorithm;

        public Asn1OctetString EncryptedKey => this.encryptedKey;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[4]
        {
       version,
       rid,
       keyEncryptionAlgorithm,
       encryptedKey
        } );
    }
}
