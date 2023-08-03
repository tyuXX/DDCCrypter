// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CertStatus
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CertStatus : Asn1Encodable
    {
        private readonly Asn1OctetString certHash;
        private readonly DerInteger certReqId;
        private readonly PkiStatusInfo statusInfo;

        private CertStatus( Asn1Sequence seq )
        {
            this.certHash = Asn1OctetString.GetInstance( seq[0] );
            this.certReqId = DerInteger.GetInstance( seq[1] );
            if (seq.Count <= 2)
                return;
            this.statusInfo = PkiStatusInfo.GetInstance( seq[2] );
        }

        public CertStatus( byte[] certHash, BigInteger certReqId )
        {
            this.certHash = new DerOctetString( certHash );
            this.certReqId = new DerInteger( certReqId );
        }

        public CertStatus( byte[] certHash, BigInteger certReqId, PkiStatusInfo statusInfo )
        {
            this.certHash = new DerOctetString( certHash );
            this.certReqId = new DerInteger( certReqId );
            this.statusInfo = statusInfo;
        }

        public static CertStatus GetInstance( object obj )
        {
            switch (obj)
            {
                case CertStatus _:
                    return (CertStatus)obj;
                case Asn1Sequence _:
                    return new CertStatus( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual Asn1OctetString CertHash => this.certHash;

        public virtual DerInteger CertReqID => this.certReqId;

        public virtual PkiStatusInfo StatusInfo => this.statusInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         certHash,
         certReqId
            } );
            v.AddOptional( statusInfo );
            return new DerSequence( v );
        }
    }
}
