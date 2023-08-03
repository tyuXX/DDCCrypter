// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.SingleResponse
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class SingleResponse : Asn1Encodable
    {
        private readonly CertID certID;
        private readonly CertStatus certStatus;
        private readonly DerGeneralizedTime thisUpdate;
        private readonly DerGeneralizedTime nextUpdate;
        private readonly X509Extensions singleExtensions;

        public SingleResponse(
          CertID certID,
          CertStatus certStatus,
          DerGeneralizedTime thisUpdate,
          DerGeneralizedTime nextUpdate,
          X509Extensions singleExtensions )
        {
            this.certID = certID;
            this.certStatus = certStatus;
            this.thisUpdate = thisUpdate;
            this.nextUpdate = nextUpdate;
            this.singleExtensions = singleExtensions;
        }

        public SingleResponse( Asn1Sequence seq )
        {
            this.certID = CertID.GetInstance( seq[0] );
            this.certStatus = CertStatus.GetInstance( seq[1] );
            this.thisUpdate = (DerGeneralizedTime)seq[2];
            if (seq.Count > 4)
            {
                this.nextUpdate = DerGeneralizedTime.GetInstance( (Asn1TaggedObject)seq[3], true );
                this.singleExtensions = X509Extensions.GetInstance( (Asn1TaggedObject)seq[4], true );
            }
            else
            {
                if (seq.Count <= 3)
                    return;
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[3];
                if (asn1TaggedObject.TagNo == 0)
                    this.nextUpdate = DerGeneralizedTime.GetInstance( asn1TaggedObject, true );
                else
                    this.singleExtensions = X509Extensions.GetInstance( asn1TaggedObject, true );
            }
        }

        public static SingleResponse GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static SingleResponse GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SingleResponse _:
                    return (SingleResponse)obj;
                case Asn1Sequence _:
                    return new SingleResponse( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertID CertId => this.certID;

        public CertStatus CertStatus => this.certStatus;

        public DerGeneralizedTime ThisUpdate => this.thisUpdate;

        public DerGeneralizedTime NextUpdate => this.nextUpdate;

        public X509Extensions SingleExtensions => this.singleExtensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[3]
            {
         certID,
         certStatus,
         thisUpdate
            } );
            if (this.nextUpdate != null)
                v.Add( new DerTaggedObject( true, 0, nextUpdate ) );
            if (this.singleExtensions != null)
                v.Add( new DerTaggedObject( true, 1, singleExtensions ) );
            return new DerSequence( v );
        }
    }
}
