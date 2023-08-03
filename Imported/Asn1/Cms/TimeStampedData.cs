// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.TimeStampedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class TimeStampedData : Asn1Encodable
    {
        private DerInteger version;
        private DerIA5String dataUri;
        private MetaData metaData;
        private Asn1OctetString content;
        private Evidence temporalEvidence;

        public TimeStampedData(
          DerIA5String dataUri,
          MetaData metaData,
          Asn1OctetString content,
          Evidence temporalEvidence )
        {
            this.version = new DerInteger( 1 );
            this.dataUri = dataUri;
            this.metaData = metaData;
            this.content = content;
            this.temporalEvidence = temporalEvidence;
        }

        private TimeStampedData( Asn1Sequence seq )
        {
            this.version = DerInteger.GetInstance( seq[0] );
            int index = 1;
            if (seq[index] is DerIA5String)
                this.dataUri = DerIA5String.GetInstance( seq[index++] );
            if (seq[index] is MetaData || seq[index] is Asn1Sequence)
                this.metaData = MetaData.GetInstance( seq[index++] );
            if (seq[index] is Asn1OctetString)
                this.content = Asn1OctetString.GetInstance( seq[index++] );
            this.temporalEvidence = Evidence.GetInstance( seq[index] );
        }

        public static TimeStampedData GetInstance( object obj )
        {
            if (obj is TimeStampedData)
                return (TimeStampedData)obj;
            return obj != null ? new TimeStampedData( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public virtual DerIA5String DataUri => this.dataUri;

        public MetaData MetaData => this.metaData;

        public Asn1OctetString Content => this.content;

        public Evidence TemporalEvidence => this.temporalEvidence;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         version
            } );
            v.AddOptional( dataUri, metaData, content );
            v.Add( temporalEvidence );
            return new BerSequence( v );
        }
    }
}
