// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Tsp.TimeStampResp
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Tsp
{
    public class TimeStampResp : Asn1Encodable
    {
        private readonly PkiStatusInfo pkiStatusInfo;
        private readonly ContentInfo timeStampToken;

        public static TimeStampResp GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case TimeStampResp _:
                    return (TimeStampResp)o;
                case Asn1Sequence _:
                    return new TimeStampResp( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "Unknown object in 'TimeStampResp' factory: " + Platform.GetTypeName( o ) );
            }
        }

        private TimeStampResp( Asn1Sequence seq )
        {
            this.pkiStatusInfo = PkiStatusInfo.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            this.timeStampToken = ContentInfo.GetInstance( seq[1] );
        }

        public TimeStampResp( PkiStatusInfo pkiStatusInfo, ContentInfo timeStampToken )
        {
            this.pkiStatusInfo = pkiStatusInfo;
            this.timeStampToken = timeStampToken;
        }

        public PkiStatusInfo Status => this.pkiStatusInfo;

        public ContentInfo TimeStampToken => this.timeStampToken;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         pkiStatusInfo
            } );
            if (this.timeStampToken != null)
                v.Add( timeStampToken );
            return new DerSequence( v );
        }
    }
}
