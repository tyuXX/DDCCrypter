// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.PasswordRecipientInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class PasswordRecipientInfo : Asn1Encodable
    {
        private readonly DerInteger version;
        private readonly AlgorithmIdentifier keyDerivationAlgorithm;
        private readonly AlgorithmIdentifier keyEncryptionAlgorithm;
        private readonly Asn1OctetString encryptedKey;

        public PasswordRecipientInfo(
          AlgorithmIdentifier keyEncryptionAlgorithm,
          Asn1OctetString encryptedKey )
        {
            this.version = new DerInteger( 0 );
            this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
            this.encryptedKey = encryptedKey;
        }

        public PasswordRecipientInfo(
          AlgorithmIdentifier keyDerivationAlgorithm,
          AlgorithmIdentifier keyEncryptionAlgorithm,
          Asn1OctetString encryptedKey )
        {
            this.version = new DerInteger( 0 );
            this.keyDerivationAlgorithm = keyDerivationAlgorithm;
            this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
            this.encryptedKey = encryptedKey;
        }

        public PasswordRecipientInfo( Asn1Sequence seq )
        {
            this.version = (DerInteger)seq[0];
            if (seq[1] is Asn1TaggedObject)
            {
                this.keyDerivationAlgorithm = AlgorithmIdentifier.GetInstance( (Asn1TaggedObject)seq[1], false );
                this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( seq[2] );
                this.encryptedKey = (Asn1OctetString)seq[3];
            }
            else
            {
                this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( seq[1] );
                this.encryptedKey = (Asn1OctetString)seq[2];
            }
        }

        public static PasswordRecipientInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static PasswordRecipientInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case PasswordRecipientInfo _:
                    return (PasswordRecipientInfo)obj;
                case Asn1Sequence _:
                    return new PasswordRecipientInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid PasswordRecipientInfo: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public AlgorithmIdentifier KeyDerivationAlgorithm => this.keyDerivationAlgorithm;

        public AlgorithmIdentifier KeyEncryptionAlgorithm => this.keyEncryptionAlgorithm;

        public Asn1OctetString EncryptedKey => this.encryptedKey;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         version
            } );
            if (this.keyDerivationAlgorithm != null)
                v.Add( new DerTaggedObject( false, 0, keyDerivationAlgorithm ) );
            v.Add( keyEncryptionAlgorithm, encryptedKey );
            return new DerSequence( v );
        }
    }
}
