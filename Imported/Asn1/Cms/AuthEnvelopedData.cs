// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.AuthEnvelopedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class AuthEnvelopedData : Asn1Encodable
    {
        private DerInteger version;
        private OriginatorInfo originatorInfo;
        private Asn1Set recipientInfos;
        private EncryptedContentInfo authEncryptedContentInfo;
        private Asn1Set authAttrs;
        private Asn1OctetString mac;
        private Asn1Set unauthAttrs;

        public AuthEnvelopedData(
          OriginatorInfo originatorInfo,
          Asn1Set recipientInfos,
          EncryptedContentInfo authEncryptedContentInfo,
          Asn1Set authAttrs,
          Asn1OctetString mac,
          Asn1Set unauthAttrs )
        {
            this.version = new DerInteger( 0 );
            this.originatorInfo = originatorInfo;
            this.recipientInfos = recipientInfos;
            this.authEncryptedContentInfo = authEncryptedContentInfo;
            this.authAttrs = authAttrs;
            this.mac = mac;
            this.unauthAttrs = unauthAttrs;
        }

        private AuthEnvelopedData( Asn1Sequence seq )
        {
            int num1 = 0;
            Asn1Sequence asn1Sequence1 = seq;
            int index1 = num1;
            int num2 = index1 + 1;
            this.version = (DerInteger)asn1Sequence1[index1].ToAsn1Object();
            Asn1Sequence asn1Sequence2 = seq;
            int index2 = num2;
            int num3 = index2 + 1;
            Asn1Object asn1Object1 = asn1Sequence2[index2].ToAsn1Object();
            if (asn1Object1 is Asn1TaggedObject)
            {
                this.originatorInfo = OriginatorInfo.GetInstance( (Asn1TaggedObject)asn1Object1, false );
                asn1Object1 = seq[num3++].ToAsn1Object();
            }
            this.recipientInfos = Asn1Set.GetInstance( asn1Object1 );
            Asn1Sequence asn1Sequence3 = seq;
            int index3 = num3;
            int num4 = index3 + 1;
            this.authEncryptedContentInfo = EncryptedContentInfo.GetInstance( asn1Sequence3[index3].ToAsn1Object() );
            Asn1Sequence asn1Sequence4 = seq;
            int index4 = num4;
            int num5 = index4 + 1;
            Asn1Object asn1Object2 = asn1Sequence4[index4].ToAsn1Object();
            if (asn1Object2 is Asn1TaggedObject)
            {
                this.authAttrs = Asn1Set.GetInstance( (Asn1TaggedObject)asn1Object2, false );
                asn1Object2 = seq[num5++].ToAsn1Object();
            }
            this.mac = Asn1OctetString.GetInstance( asn1Object2 );
            if (seq.Count <= num5)
                return;
            Asn1Sequence asn1Sequence5 = seq;
            int index5 = num5;
            int num6 = index5 + 1;
            this.unauthAttrs = Asn1Set.GetInstance( (Asn1TaggedObject)asn1Sequence5[index5].ToAsn1Object(), false );
        }

        public static AuthEnvelopedData GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static AuthEnvelopedData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case AuthEnvelopedData _:
                    return (AuthEnvelopedData)obj;
                case Asn1Sequence _:
                    return new AuthEnvelopedData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid AuthEnvelopedData: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public OriginatorInfo OriginatorInfo => this.originatorInfo;

        public Asn1Set RecipientInfos => this.recipientInfos;

        public EncryptedContentInfo AuthEncryptedContentInfo => this.authEncryptedContentInfo;

        public Asn1Set AuthAttrs => this.authAttrs;

        public Asn1OctetString Mac => this.mac;

        public Asn1Set UnauthAttrs => this.unauthAttrs;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         version
            } );
            if (this.originatorInfo != null)
                v.Add( new DerTaggedObject( false, 0, originatorInfo ) );
            v.Add( recipientInfos, authEncryptedContentInfo );
            if (this.authAttrs != null)
                v.Add( new DerTaggedObject( false, 1, authAttrs ) );
            v.Add( mac );
            if (this.unauthAttrs != null)
                v.Add( new DerTaggedObject( false, 2, unauthAttrs ) );
            return new BerSequence( v );
        }
    }
}
