// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.EnvelopedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class EnvelopedData : Asn1Encodable
    {
        private DerInteger version;
        private OriginatorInfo originatorInfo;
        private Asn1Set recipientInfos;
        private EncryptedContentInfo encryptedContentInfo;
        private Asn1Set unprotectedAttrs;

        public EnvelopedData(
          OriginatorInfo originatorInfo,
          Asn1Set recipientInfos,
          EncryptedContentInfo encryptedContentInfo,
          Asn1Set unprotectedAttrs )
        {
            this.version = new DerInteger( CalculateVersion( originatorInfo, recipientInfos, unprotectedAttrs ) );
            this.originatorInfo = originatorInfo;
            this.recipientInfos = recipientInfos;
            this.encryptedContentInfo = encryptedContentInfo;
            this.unprotectedAttrs = unprotectedAttrs;
        }

        public EnvelopedData(
          OriginatorInfo originatorInfo,
          Asn1Set recipientInfos,
          EncryptedContentInfo encryptedContentInfo,
          Attributes unprotectedAttrs )
        {
            this.version = new DerInteger( CalculateVersion( originatorInfo, recipientInfos, Asn1Set.GetInstance( unprotectedAttrs ) ) );
            this.originatorInfo = originatorInfo;
            this.recipientInfos = recipientInfos;
            this.encryptedContentInfo = encryptedContentInfo;
            this.unprotectedAttrs = Asn1Set.GetInstance( unprotectedAttrs );
        }

        [Obsolete( "Use 'GetInstance' instead" )]
        public EnvelopedData( Asn1Sequence seq )
        {
            int num1 = 0;
            Asn1Sequence asn1Sequence1 = seq;
            int index1 = num1;
            int num2 = index1 + 1;
            this.version = (DerInteger)asn1Sequence1[index1];
            Asn1Sequence asn1Sequence2 = seq;
            int index2 = num2;
            int num3 = index2 + 1;
            object obj = asn1Sequence2[index2];
            if (obj is Asn1TaggedObject)
            {
                this.originatorInfo = OriginatorInfo.GetInstance( (Asn1TaggedObject)obj, false );
                obj = seq[num3++];
            }
            this.recipientInfos = Asn1Set.GetInstance( obj );
            Asn1Sequence asn1Sequence3 = seq;
            int index3 = num3;
            int index4 = index3 + 1;
            this.encryptedContentInfo = EncryptedContentInfo.GetInstance( asn1Sequence3[index3] );
            if (seq.Count <= index4)
                return;
            this.unprotectedAttrs = Asn1Set.GetInstance( (Asn1TaggedObject)seq[index4], false );
        }

        public static EnvelopedData GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static EnvelopedData GetInstance( object obj )
        {
            if (obj is EnvelopedData)
                return (EnvelopedData)obj;
            return obj == null ? null : new EnvelopedData( Asn1Sequence.GetInstance( obj ) );
        }

        public DerInteger Version => this.version;

        public OriginatorInfo OriginatorInfo => this.originatorInfo;

        public Asn1Set RecipientInfos => this.recipientInfos;

        public EncryptedContentInfo EncryptedContentInfo => this.encryptedContentInfo;

        public Asn1Set UnprotectedAttrs => this.unprotectedAttrs;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         version
            } );
            if (this.originatorInfo != null)
                v.Add( new DerTaggedObject( false, 0, originatorInfo ) );
            v.Add( recipientInfos, encryptedContentInfo );
            if (this.unprotectedAttrs != null)
                v.Add( new DerTaggedObject( false, 1, unprotectedAttrs ) );
            return new BerSequence( v );
        }

        public static int CalculateVersion(
          OriginatorInfo originatorInfo,
          Asn1Set recipientInfos,
          Asn1Set unprotectedAttrs )
        {
            if (originatorInfo != null || unprotectedAttrs != null)
                return 2;
            foreach (object recipientInfo in recipientInfos)
            {
                if (RecipientInfo.GetInstance( recipientInfo ).Version.Value.IntValue != 0)
                    return 2;
            }
            return 0;
        }
    }
}
