// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CertRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class CertRequest : Asn1Encodable
    {
        private readonly DerInteger certReqId;
        private readonly CertTemplate certTemplate;
        private readonly Controls controls;

        private CertRequest( Asn1Sequence seq )
        {
            this.certReqId = DerInteger.GetInstance( seq[0] );
            this.certTemplate = CertTemplate.GetInstance( seq[1] );
            if (seq.Count <= 2)
                return;
            this.controls = Controls.GetInstance( seq[2] );
        }

        public static CertRequest GetInstance( object obj )
        {
            if (obj is CertRequest)
                return (CertRequest)obj;
            return obj != null ? new CertRequest( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public CertRequest( int certReqId, CertTemplate certTemplate, Controls controls )
          : this( new DerInteger( certReqId ), certTemplate, controls )
        {
        }

        public CertRequest( DerInteger certReqId, CertTemplate certTemplate, Controls controls )
        {
            this.certReqId = certReqId;
            this.certTemplate = certTemplate;
            this.controls = controls;
        }

        public virtual DerInteger CertReqID => this.certReqId;

        public virtual CertTemplate CertTemplate => this.certTemplate;

        public virtual Controls Controls => this.controls;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         certReqId,
         certTemplate
            } );
            v.AddOptional( controls );
            return new DerSequence( v );
        }
    }
}
