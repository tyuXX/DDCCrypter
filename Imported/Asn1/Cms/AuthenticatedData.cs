// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.AuthenticatedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class AuthenticatedData : Asn1Encodable
    {
        private DerInteger version;
        private OriginatorInfo originatorInfo;
        private Asn1Set recipientInfos;
        private AlgorithmIdentifier macAlgorithm;
        private AlgorithmIdentifier digestAlgorithm;
        private ContentInfo encapsulatedContentInfo;
        private Asn1Set authAttrs;
        private Asn1OctetString mac;
        private Asn1Set unauthAttrs;

        public AuthenticatedData(
          OriginatorInfo originatorInfo,
          Asn1Set recipientInfos,
          AlgorithmIdentifier macAlgorithm,
          AlgorithmIdentifier digestAlgorithm,
          ContentInfo encapsulatedContent,
          Asn1Set authAttrs,
          Asn1OctetString mac,
          Asn1Set unauthAttrs )
        {
            if ((digestAlgorithm != null || authAttrs != null) && (digestAlgorithm == null || authAttrs == null))
                throw new ArgumentException( "digestAlgorithm and authAttrs must be set together" );
            this.version = new DerInteger( CalculateVersion( originatorInfo ) );
            this.originatorInfo = originatorInfo;
            this.macAlgorithm = macAlgorithm;
            this.digestAlgorithm = digestAlgorithm;
            this.recipientInfos = recipientInfos;
            this.encapsulatedContentInfo = encapsulatedContent;
            this.authAttrs = authAttrs;
            this.mac = mac;
            this.unauthAttrs = unauthAttrs;
        }

        private AuthenticatedData( Asn1Sequence seq )
        {
            int num1 = 0;
            Asn1Sequence asn1Sequence1 = seq;
            int index1 = num1;
            int num2 = index1 + 1;
            this.version = (DerInteger)asn1Sequence1[index1];
            Asn1Sequence asn1Sequence2 = seq;
            int index2 = num2;
            int num3 = index2 + 1;
            Asn1Encodable asn1Encodable1 = asn1Sequence2[index2];
            if (asn1Encodable1 is Asn1TaggedObject)
            {
                this.originatorInfo = OriginatorInfo.GetInstance( (Asn1TaggedObject)asn1Encodable1, false );
                asn1Encodable1 = seq[num3++];
            }
            this.recipientInfos = Asn1Set.GetInstance( asn1Encodable1 );
            Asn1Sequence asn1Sequence3 = seq;
            int index3 = num3;
            int num4 = index3 + 1;
            this.macAlgorithm = AlgorithmIdentifier.GetInstance( asn1Sequence3[index3] );
            Asn1Sequence asn1Sequence4 = seq;
            int index4 = num4;
            int num5 = index4 + 1;
            Asn1Encodable asn1Encodable2 = asn1Sequence4[index4];
            if (asn1Encodable2 is Asn1TaggedObject)
            {
                this.digestAlgorithm = AlgorithmIdentifier.GetInstance( (Asn1TaggedObject)asn1Encodable2, false );
                asn1Encodable2 = seq[num5++];
            }
            this.encapsulatedContentInfo = ContentInfo.GetInstance( asn1Encodable2 );
            Asn1Sequence asn1Sequence5 = seq;
            int index5 = num5;
            int index6 = index5 + 1;
            Asn1Encodable asn1Encodable3 = asn1Sequence5[index5];
            if (asn1Encodable3 is Asn1TaggedObject)
            {
                this.authAttrs = Asn1Set.GetInstance( (Asn1TaggedObject)asn1Encodable3, false );
                asn1Encodable3 = seq[index6++];
            }
            this.mac = Asn1OctetString.GetInstance( asn1Encodable3 );
            if (seq.Count <= index6)
                return;
            this.unauthAttrs = Asn1Set.GetInstance( (Asn1TaggedObject)seq[index6], false );
        }

        public static AuthenticatedData GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static AuthenticatedData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case AuthenticatedData _:
                    return (AuthenticatedData)obj;
                case Asn1Sequence _:
                    return new AuthenticatedData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid AuthenticatedData: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public OriginatorInfo OriginatorInfo => this.originatorInfo;

        public Asn1Set RecipientInfos => this.recipientInfos;

        public AlgorithmIdentifier MacAlgorithm => this.macAlgorithm;

        public AlgorithmIdentifier DigestAlgorithm => this.digestAlgorithm;

        public ContentInfo EncapsulatedContentInfo => this.encapsulatedContentInfo;

        public Asn1Set AuthAttrs => this.authAttrs;

        public Asn1OctetString Mac => this.mac;

        public Asn1Set UnauthAttrs => this.unauthAttrs;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         version
            } );
            if (this.originatorInfo != null)
                v.Add( new DerTaggedObject( false, 0, originatorInfo ) );
            v.Add( recipientInfos, macAlgorithm );
            if (this.digestAlgorithm != null)
                v.Add( new DerTaggedObject( false, 1, digestAlgorithm ) );
            v.Add( encapsulatedContentInfo );
            if (this.authAttrs != null)
                v.Add( new DerTaggedObject( false, 2, authAttrs ) );
            v.Add( mac );
            if (this.unauthAttrs != null)
                v.Add( new DerTaggedObject( false, 3, unauthAttrs ) );
            return new BerSequence( v );
        }

        public static int CalculateVersion( OriginatorInfo origInfo )
        {
            if (origInfo == null)
                return 0;
            int version = 0;
            foreach (object certificate in origInfo.Certificates)
            {
                if (certificate is Asn1TaggedObject)
                {
                    Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)certificate;
                    if (asn1TaggedObject.TagNo == 2)
                        version = 1;
                    else if (asn1TaggedObject.TagNo == 3)
                    {
                        version = 3;
                        break;
                    }
                }
            }
            foreach (object crl in origInfo.Crls)
            {
                if (crl is Asn1TaggedObject && ((Asn1TaggedObject)crl).TagNo == 1)
                {
                    version = 3;
                    break;
                }
            }
            return version;
        }
    }
}
