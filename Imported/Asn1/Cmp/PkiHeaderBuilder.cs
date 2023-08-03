// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiHeaderBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiHeaderBuilder
    {
        private DerInteger pvno;
        private GeneralName sender;
        private GeneralName recipient;
        private DerGeneralizedTime messageTime;
        private AlgorithmIdentifier protectionAlg;
        private Asn1OctetString senderKID;
        private Asn1OctetString recipKID;
        private Asn1OctetString transactionID;
        private Asn1OctetString senderNonce;
        private Asn1OctetString recipNonce;
        private PkiFreeText freeText;
        private Asn1Sequence generalInfo;

        public PkiHeaderBuilder( int pvno, GeneralName sender, GeneralName recipient )
          : this( new DerInteger( pvno ), sender, recipient )
        {
        }

        private PkiHeaderBuilder( DerInteger pvno, GeneralName sender, GeneralName recipient )
        {
            this.pvno = pvno;
            this.sender = sender;
            this.recipient = recipient;
        }

        public virtual PkiHeaderBuilder SetMessageTime( DerGeneralizedTime time )
        {
            this.messageTime = time;
            return this;
        }

        public virtual PkiHeaderBuilder SetProtectionAlg( AlgorithmIdentifier aid )
        {
            this.protectionAlg = aid;
            return this;
        }

        public virtual PkiHeaderBuilder SetSenderKID( byte[] kid ) => this.SetSenderKID( kid == null ? null : (Asn1OctetString)new DerOctetString( kid ) );

        public virtual PkiHeaderBuilder SetSenderKID( Asn1OctetString kid )
        {
            this.senderKID = kid;
            return this;
        }

        public virtual PkiHeaderBuilder SetRecipKID( byte[] kid ) => this.SetRecipKID( kid == null ? null : new DerOctetString( kid ) );

        public virtual PkiHeaderBuilder SetRecipKID( DerOctetString kid )
        {
            this.recipKID = kid;
            return this;
        }

        public virtual PkiHeaderBuilder SetTransactionID( byte[] tid ) => this.SetTransactionID( tid == null ? null : (Asn1OctetString)new DerOctetString( tid ) );

        public virtual PkiHeaderBuilder SetTransactionID( Asn1OctetString tid )
        {
            this.transactionID = tid;
            return this;
        }

        public virtual PkiHeaderBuilder SetSenderNonce( byte[] nonce ) => this.SetSenderNonce( nonce == null ? null : (Asn1OctetString)new DerOctetString( nonce ) );

        public virtual PkiHeaderBuilder SetSenderNonce( Asn1OctetString nonce )
        {
            this.senderNonce = nonce;
            return this;
        }

        public virtual PkiHeaderBuilder SetRecipNonce( byte[] nonce ) => this.SetRecipNonce( nonce == null ? null : (Asn1OctetString)new DerOctetString( nonce ) );

        public virtual PkiHeaderBuilder SetRecipNonce( Asn1OctetString nonce )
        {
            this.recipNonce = nonce;
            return this;
        }

        public virtual PkiHeaderBuilder SetFreeText( PkiFreeText text )
        {
            this.freeText = text;
            return this;
        }

        public virtual PkiHeaderBuilder SetGeneralInfo( InfoTypeAndValue genInfo ) => this.SetGeneralInfo( MakeGeneralInfoSeq( genInfo ) );

        public virtual PkiHeaderBuilder SetGeneralInfo( InfoTypeAndValue[] genInfos ) => this.SetGeneralInfo( MakeGeneralInfoSeq( genInfos ) );

        public virtual PkiHeaderBuilder SetGeneralInfo( Asn1Sequence seqOfInfoTypeAndValue )
        {
            this.generalInfo = seqOfInfoTypeAndValue;
            return this;
        }

        private static Asn1Sequence MakeGeneralInfoSeq( InfoTypeAndValue generalInfo ) => new DerSequence( generalInfo );

        private static Asn1Sequence MakeGeneralInfoSeq( InfoTypeAndValue[] generalInfos )
        {
            Asn1Sequence asn1Sequence = null;
            if (generalInfos != null)
            {
                Asn1EncodableVector v = new( new Asn1Encodable[0] );
                for (int index = 0; index < generalInfos.Length; ++index)
                    v.Add( generalInfos[index] );
                asn1Sequence = new DerSequence( v );
            }
            return asn1Sequence;
        }

        public virtual PkiHeader Build()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         pvno,
         sender,
         recipient
            } );
            this.AddOptional( v, 0, messageTime );
            this.AddOptional( v, 1, protectionAlg );
            this.AddOptional( v, 2, senderKID );
            this.AddOptional( v, 3, recipKID );
            this.AddOptional( v, 4, transactionID );
            this.AddOptional( v, 5, senderNonce );
            this.AddOptional( v, 6, recipNonce );
            this.AddOptional( v, 7, freeText );
            this.AddOptional( v, 8, generalInfo );
            this.messageTime = null;
            this.protectionAlg = null;
            this.senderKID = null;
            this.recipKID = null;
            this.transactionID = null;
            this.senderNonce = null;
            this.recipNonce = null;
            this.freeText = null;
            this.generalInfo = null;
            return PkiHeader.GetInstance( new DerSequence( v ) );
        }

        private void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( true, tagNo, obj ) );
        }
    }
}
