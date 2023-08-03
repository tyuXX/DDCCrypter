// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiHeader
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiHeader : Asn1Encodable
    {
        public static readonly GeneralName NULL_NAME = new GeneralName( X509Name.GetInstance( new DerSequence() ) );
        public static readonly int CMP_1999 = 1;
        public static readonly int CMP_2000 = 2;
        private readonly DerInteger pvno;
        private readonly GeneralName sender;
        private readonly GeneralName recipient;
        private readonly DerGeneralizedTime messageTime;
        private readonly AlgorithmIdentifier protectionAlg;
        private readonly Asn1OctetString senderKID;
        private readonly Asn1OctetString recipKID;
        private readonly Asn1OctetString transactionID;
        private readonly Asn1OctetString senderNonce;
        private readonly Asn1OctetString recipNonce;
        private readonly PkiFreeText freeText;
        private readonly Asn1Sequence generalInfo;

        private PkiHeader( Asn1Sequence seq )
        {
            this.pvno = DerInteger.GetInstance( seq[0] );
            this.sender = GeneralName.GetInstance( seq[1] );
            this.recipient = GeneralName.GetInstance( seq[2] );
            for (int index = 3; index < seq.Count; ++index)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index];
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.messageTime = DerGeneralizedTime.GetInstance( asn1TaggedObject, true );
                        break;
                    case 1:
                        this.protectionAlg = AlgorithmIdentifier.GetInstance( asn1TaggedObject, true );
                        break;
                    case 2:
                        this.senderKID = Asn1OctetString.GetInstance( asn1TaggedObject, true );
                        break;
                    case 3:
                        this.recipKID = Asn1OctetString.GetInstance( asn1TaggedObject, true );
                        break;
                    case 4:
                        this.transactionID = Asn1OctetString.GetInstance( asn1TaggedObject, true );
                        break;
                    case 5:
                        this.senderNonce = Asn1OctetString.GetInstance( asn1TaggedObject, true );
                        break;
                    case 6:
                        this.recipNonce = Asn1OctetString.GetInstance( asn1TaggedObject, true );
                        break;
                    case 7:
                        this.freeText = PkiFreeText.GetInstance( asn1TaggedObject, true );
                        break;
                    case 8:
                        this.generalInfo = Asn1Sequence.GetInstance( asn1TaggedObject, true );
                        break;
                    default:
                        throw new ArgumentException( "unknown tag number: " + asn1TaggedObject.TagNo, nameof( seq ) );
                }
            }
        }

        public static PkiHeader GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiHeader _:
                    return (PkiHeader)obj;
                case Asn1Sequence _:
                    return new PkiHeader( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public PkiHeader( int pvno, GeneralName sender, GeneralName recipient )
          : this( new DerInteger( pvno ), sender, recipient )
        {
        }

        private PkiHeader( DerInteger pvno, GeneralName sender, GeneralName recipient )
        {
            this.pvno = pvno;
            this.sender = sender;
            this.recipient = recipient;
        }

        public virtual DerInteger Pvno => this.pvno;

        public virtual GeneralName Sender => this.sender;

        public virtual GeneralName Recipient => this.recipient;

        public virtual DerGeneralizedTime MessageTime => this.messageTime;

        public virtual AlgorithmIdentifier ProtectionAlg => this.protectionAlg;

        public virtual Asn1OctetString SenderKID => this.senderKID;

        public virtual Asn1OctetString RecipKID => this.recipKID;

        public virtual Asn1OctetString TransactionID => this.transactionID;

        public virtual Asn1OctetString SenderNonce => this.senderNonce;

        public virtual Asn1OctetString RecipNonce => this.recipNonce;

        public virtual PkiFreeText FreeText => this.freeText;

        public virtual InfoTypeAndValue[] GetGeneralInfo()
        {
            if (this.generalInfo == null)
                return null;
            InfoTypeAndValue[] generalInfo = new InfoTypeAndValue[this.generalInfo.Count];
            for (int index = 0; index < generalInfo.Length; ++index)
                generalInfo[index] = InfoTypeAndValue.GetInstance( this.generalInfo[index] );
            return generalInfo;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[3]
            {
         pvno,
         sender,
         recipient
            } );
            AddOptional( v, 0, messageTime );
            AddOptional( v, 1, protectionAlg );
            AddOptional( v, 2, senderKID );
            AddOptional( v, 3, recipKID );
            AddOptional( v, 4, transactionID );
            AddOptional( v, 5, senderNonce );
            AddOptional( v, 6, recipNonce );
            AddOptional( v, 7, freeText );
            AddOptional( v, 8, generalInfo );
            return new DerSequence( v );
        }

        private static void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( true, tagNo, obj ) );
        }
    }
}
