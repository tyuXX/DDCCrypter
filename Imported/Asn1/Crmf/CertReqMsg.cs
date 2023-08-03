// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CertReqMsg
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class CertReqMsg : Asn1Encodable
    {
        private readonly CertRequest certReq;
        private readonly ProofOfPossession popo;
        private readonly Asn1Sequence regInfo;

        private CertReqMsg( Asn1Sequence seq )
        {
            this.certReq = CertRequest.GetInstance( seq[0] );
            for (int index = 1; index < seq.Count; ++index)
            {
                object obj = seq[index];
                switch (obj)
                {
                    case Asn1TaggedObject _:
                    case ProofOfPossession _:
                        this.popo = ProofOfPossession.GetInstance( obj );
                        break;
                    default:
                        this.regInfo = Asn1Sequence.GetInstance( obj );
                        break;
                }
            }
        }

        public static CertReqMsg GetInstance( object obj )
        {
            if (obj is CertReqMsg)
                return (CertReqMsg)obj;
            return obj != null ? new CertReqMsg( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public CertReqMsg( CertRequest certReq, ProofOfPossession popo, AttributeTypeAndValue[] regInfo )
        {
            this.certReq = certReq != null ? certReq : throw new ArgumentNullException( nameof( certReq ) );
            this.popo = popo;
            if (regInfo == null)
                return;
            this.regInfo = new DerSequence( regInfo );
        }

        public virtual CertRequest CertReq => this.certReq;

        public virtual ProofOfPossession Popo => this.popo;

        public virtual AttributeTypeAndValue[] GetRegInfo()
        {
            if (this.regInfo == null)
                return null;
            AttributeTypeAndValue[] regInfo = new AttributeTypeAndValue[this.regInfo.Count];
            for (int index = 0; index != regInfo.Length; ++index)
                regInfo[index] = AttributeTypeAndValue.GetInstance( this.regInfo[index] );
            return regInfo;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         certReq
            } );
            v.AddOptional( popo, regInfo );
            return new DerSequence( v );
        }
    }
}
