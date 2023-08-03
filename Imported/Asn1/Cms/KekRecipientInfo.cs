// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.KekRecipientInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class KekRecipientInfo : Asn1Encodable
    {
        private DerInteger version;
        private KekIdentifier kekID;
        private AlgorithmIdentifier keyEncryptionAlgorithm;
        private Asn1OctetString encryptedKey;

        public KekRecipientInfo(
          KekIdentifier kekID,
          AlgorithmIdentifier keyEncryptionAlgorithm,
          Asn1OctetString encryptedKey )
        {
            this.version = new DerInteger( 4 );
            this.kekID = kekID;
            this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
            this.encryptedKey = encryptedKey;
        }

        public KekRecipientInfo( Asn1Sequence seq )
        {
            this.version = (DerInteger)seq[0];
            this.kekID = KekIdentifier.GetInstance( seq[1] );
            this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( seq[2] );
            this.encryptedKey = (Asn1OctetString)seq[3];
        }

        public static KekRecipientInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static KekRecipientInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case KekRecipientInfo _:
                    return (KekRecipientInfo)obj;
                case Asn1Sequence _:
                    return new KekRecipientInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid KekRecipientInfo: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public KekIdentifier KekID => this.kekID;

        public AlgorithmIdentifier KeyEncryptionAlgorithm => this.keyEncryptionAlgorithm;

        public Asn1OctetString EncryptedKey => this.encryptedKey;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[4]
        {
       version,
       kekID,
       keyEncryptionAlgorithm,
       encryptedKey
        } );
    }
}
