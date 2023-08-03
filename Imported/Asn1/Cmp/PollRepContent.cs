// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PollRepContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PollRepContent : Asn1Encodable
    {
        private readonly DerInteger certReqId;
        private readonly DerInteger checkAfter;
        private readonly PkiFreeText reason;

        private PollRepContent( Asn1Sequence seq )
        {
            this.certReqId = DerInteger.GetInstance( seq[0] );
            this.checkAfter = DerInteger.GetInstance( seq[1] );
            if (seq.Count <= 2)
                return;
            this.reason = PkiFreeText.GetInstance( seq[2] );
        }

        public static PollRepContent GetInstance( object obj )
        {
            switch (obj)
            {
                case PollRepContent _:
                    return (PollRepContent)obj;
                case Asn1Sequence _:
                    return new PollRepContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual DerInteger CertReqID => this.certReqId;

        public virtual DerInteger CheckAfter => this.checkAfter;

        public virtual PkiFreeText Reason => this.reason;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         certReqId,
         checkAfter
            } );
            v.AddOptional( reason );
            return new DerSequence( v );
        }
    }
}
