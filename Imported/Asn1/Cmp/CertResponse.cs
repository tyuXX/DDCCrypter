// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CertResponse
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CertResponse : Asn1Encodable
    {
        private readonly DerInteger certReqId;
        private readonly PkiStatusInfo status;
        private readonly CertifiedKeyPair certifiedKeyPair;
        private readonly Asn1OctetString rspInfo;

        private CertResponse( Asn1Sequence seq )
        {
            this.certReqId = DerInteger.GetInstance( seq[0] );
            this.status = PkiStatusInfo.GetInstance( seq[1] );
            if (seq.Count < 3)
                return;
            if (seq.Count == 3)
            {
                Asn1Encodable asn1Encodable = seq[2];
                if (asn1Encodable is Asn1OctetString)
                    this.rspInfo = Asn1OctetString.GetInstance( asn1Encodable );
                else
                    this.certifiedKeyPair = CertifiedKeyPair.GetInstance( asn1Encodable );
            }
            else
            {
                this.certifiedKeyPair = CertifiedKeyPair.GetInstance( seq[2] );
                this.rspInfo = Asn1OctetString.GetInstance( seq[3] );
            }
        }

        public static CertResponse GetInstance( object obj )
        {
            switch (obj)
            {
                case CertResponse _:
                    return (CertResponse)obj;
                case Asn1Sequence _:
                    return new CertResponse( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertResponse( DerInteger certReqId, PkiStatusInfo status )
          : this( certReqId, status, null, null )
        {
        }

        public CertResponse(
          DerInteger certReqId,
          PkiStatusInfo status,
          CertifiedKeyPair certifiedKeyPair,
          Asn1OctetString rspInfo )
        {
            if (certReqId == null)
                throw new ArgumentNullException( nameof( certReqId ) );
            if (status == null)
                throw new ArgumentNullException( nameof( status ) );
            this.certReqId = certReqId;
            this.status = status;
            this.certifiedKeyPair = certifiedKeyPair;
            this.rspInfo = rspInfo;
        }

        public virtual DerInteger CertReqID => this.certReqId;

        public virtual PkiStatusInfo Status => this.status;

        public virtual CertifiedKeyPair CertifiedKeyPair => this.certifiedKeyPair;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         certReqId,
         status
            } );
            v.AddOptional( certifiedKeyPair, rspInfo );
            return new DerSequence( v );
        }
    }
}
