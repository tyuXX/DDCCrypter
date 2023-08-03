// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.TimeStampTokenEvidence
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class TimeStampTokenEvidence : Asn1Encodable
    {
        private TimeStampAndCrl[] timeStampAndCrls;

        public TimeStampTokenEvidence( TimeStampAndCrl[] timeStampAndCrls ) => this.timeStampAndCrls = timeStampAndCrls;

        public TimeStampTokenEvidence( TimeStampAndCrl timeStampAndCrl ) => this.timeStampAndCrls = new TimeStampAndCrl[1]
        {
      timeStampAndCrl
        };

        private TimeStampTokenEvidence( Asn1Sequence seq )
        {
            this.timeStampAndCrls = new TimeStampAndCrl[seq.Count];
            int num = 0;
            foreach (Asn1Encodable asn1Encodable in seq)
                this.timeStampAndCrls[num++] = TimeStampAndCrl.GetInstance( asn1Encodable.ToAsn1Object() );
        }

        public static TimeStampTokenEvidence GetInstance( Asn1TaggedObject tagged, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( tagged, isExplicit ) );

        public static TimeStampTokenEvidence GetInstance( object obj )
        {
            if (obj is TimeStampTokenEvidence)
                return (TimeStampTokenEvidence)obj;
            return obj != null ? new TimeStampTokenEvidence( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public virtual TimeStampAndCrl[] ToTimeStampAndCrlArray() => (TimeStampAndCrl[])this.timeStampAndCrls.Clone();

        public override Asn1Object ToAsn1Object() => new DerSequence( timeStampAndCrls );
    }
}
