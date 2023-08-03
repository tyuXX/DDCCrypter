// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.KeyAgreeRecipientInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class KeyAgreeRecipientInfo : Asn1Encodable
    {
        private DerInteger version;
        private OriginatorIdentifierOrKey originator;
        private Asn1OctetString ukm;
        private AlgorithmIdentifier keyEncryptionAlgorithm;
        private Asn1Sequence recipientEncryptedKeys;

        public KeyAgreeRecipientInfo(
          OriginatorIdentifierOrKey originator,
          Asn1OctetString ukm,
          AlgorithmIdentifier keyEncryptionAlgorithm,
          Asn1Sequence recipientEncryptedKeys )
        {
            this.version = new DerInteger( 3 );
            this.originator = originator;
            this.ukm = ukm;
            this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
            this.recipientEncryptedKeys = recipientEncryptedKeys;
        }

        public KeyAgreeRecipientInfo( Asn1Sequence seq )
        {
            int num1 = 0;
            Asn1Sequence asn1Sequence1 = seq;
            int index1 = num1;
            int num2 = index1 + 1;
            this.version = (DerInteger)asn1Sequence1[index1];
            Asn1Sequence asn1Sequence2 = seq;
            int index2 = num2;
            int index3 = index2 + 1;
            this.originator = OriginatorIdentifierOrKey.GetInstance( (Asn1TaggedObject)asn1Sequence2[index2], true );
            if (seq[index3] is Asn1TaggedObject)
                this.ukm = Asn1OctetString.GetInstance( (Asn1TaggedObject)seq[index3++], true );
            Asn1Sequence asn1Sequence3 = seq;
            int index4 = index3;
            int num3 = index4 + 1;
            this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( asn1Sequence3[index4] );
            Asn1Sequence asn1Sequence4 = seq;
            int index5 = num3;
            int num4 = index5 + 1;
            this.recipientEncryptedKeys = (Asn1Sequence)asn1Sequence4[index5];
        }

        public static KeyAgreeRecipientInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static KeyAgreeRecipientInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case KeyAgreeRecipientInfo _:
                    return (KeyAgreeRecipientInfo)obj;
                case Asn1Sequence _:
                    return new KeyAgreeRecipientInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Illegal object in KeyAgreeRecipientInfo: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public OriginatorIdentifierOrKey Originator => this.originator;

        public Asn1OctetString UserKeyingMaterial => this.ukm;

        public AlgorithmIdentifier KeyEncryptionAlgorithm => this.keyEncryptionAlgorithm;

        public Asn1Sequence RecipientEncryptedKeys => this.recipientEncryptedKeys;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         version,
         new DerTaggedObject(true, 0,  originator)
            } );
            if (this.ukm != null)
                v.Add( new DerTaggedObject( true, 1, ukm ) );
            v.Add( keyEncryptionAlgorithm, recipientEncryptedKeys );
            return new DerSequence( v );
        }
    }
}
