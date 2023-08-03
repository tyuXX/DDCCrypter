// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OcspIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OcspIdentifier : Asn1Encodable
    {
        private readonly ResponderID ocspResponderID;
        private readonly DerGeneralizedTime producedAt;

        public static OcspIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OcspIdentifier _:
                    return (OcspIdentifier)obj;
                case Asn1Sequence _:
                    return new OcspIdentifier( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'OcspIdentifier' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private OcspIdentifier( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.ocspResponderID = seq.Count == 2 ? ResponderID.GetInstance( seq[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.producedAt = (DerGeneralizedTime)seq[1].ToAsn1Object();
        }

        public OcspIdentifier( ResponderID ocspResponderID, DateTime producedAt )
        {
            this.ocspResponderID = ocspResponderID != null ? ocspResponderID : throw new ArgumentNullException();
            this.producedAt = new DerGeneralizedTime( producedAt );
        }

        public ResponderID OcspResponderID => this.ocspResponderID;

        public DateTime ProducedAt => this.producedAt.ToDateTime();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       ocspResponderID,
       producedAt
        } );
    }
}
