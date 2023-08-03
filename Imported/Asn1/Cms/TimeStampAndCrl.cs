// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.TimeStampAndCrl
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class TimeStampAndCrl : Asn1Encodable
    {
        private ContentInfo timeStamp;
        private CertificateList crl;

        public TimeStampAndCrl( ContentInfo timeStamp ) => this.timeStamp = timeStamp;

        private TimeStampAndCrl( Asn1Sequence seq )
        {
            this.timeStamp = ContentInfo.GetInstance( seq[0] );
            if (seq.Count != 2)
                return;
            this.crl = CertificateList.GetInstance( seq[1] );
        }

        public static TimeStampAndCrl GetInstance( object obj )
        {
            if (obj is TimeStampAndCrl)
                return (TimeStampAndCrl)obj;
            return obj != null ? new TimeStampAndCrl( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public virtual ContentInfo TimeStampToken => this.timeStamp;

        public virtual CertificateList Crl => this.crl;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         timeStamp
            } );
            v.AddOptional( crl );
            return new DerSequence( v );
        }
    }
}
