// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.TimeStampedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class TimeStampedDataParser
    {
        private DerInteger version;
        private DerIA5String dataUri;
        private MetaData metaData;
        private Asn1OctetStringParser content;
        private Evidence temporalEvidence;
        private Asn1SequenceParser parser;

        private TimeStampedDataParser( Asn1SequenceParser parser )
        {
            this.parser = parser;
            this.version = DerInteger.GetInstance( parser.ReadObject() );
            Asn1Object asn1Object = parser.ReadObject().ToAsn1Object();
            if (asn1Object is DerIA5String)
            {
                this.dataUri = DerIA5String.GetInstance( asn1Object );
                asn1Object = parser.ReadObject().ToAsn1Object();
            }
            if (asn1Object is Asn1SequenceParser)
            {
                this.metaData = MetaData.GetInstance( asn1Object.ToAsn1Object() );
                asn1Object = parser.ReadObject().ToAsn1Object();
            }
            if (!(asn1Object is Asn1OctetStringParser))
                return;
            this.content = (Asn1OctetStringParser)asn1Object;
        }

        public static TimeStampedDataParser GetInstance( object obj )
        {
            switch (obj)
            {
                case Asn1Sequence _:
                    return new TimeStampedDataParser( ((Asn1Sequence)obj).Parser );
                case Asn1SequenceParser _:
                    return new TimeStampedDataParser( (Asn1SequenceParser)obj );
                default:
                    return null;
            }
        }

        public virtual DerIA5String DataUri => this.dataUri;

        public virtual MetaData MetaData => this.metaData;

        public virtual Asn1OctetStringParser Content => this.content;

        public virtual Evidence GetTemporalEvidence()
        {
            if (this.temporalEvidence == null)
                this.temporalEvidence = Evidence.GetInstance( this.parser.ReadObject().ToAsn1Object() );
            return this.temporalEvidence;
        }
    }
}
