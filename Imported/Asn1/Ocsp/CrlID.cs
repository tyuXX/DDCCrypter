// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.CrlID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class CrlID : Asn1Encodable
    {
        private readonly DerIA5String crlUrl;
        private readonly DerInteger crlNum;
        private readonly DerGeneralizedTime crlTime;

        public CrlID( Asn1Sequence seq )
        {
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.crlUrl = DerIA5String.GetInstance( asn1TaggedObject, true );
                        continue;
                    case 1:
                        this.crlNum = DerInteger.GetInstance( asn1TaggedObject, true );
                        continue;
                    case 2:
                        this.crlTime = DerGeneralizedTime.GetInstance( asn1TaggedObject, true );
                        continue;
                    default:
                        throw new ArgumentException( "unknown tag number: " + asn1TaggedObject.TagNo );
                }
            }
        }

        public DerIA5String CrlUrl => this.crlUrl;

        public DerInteger CrlNum => this.crlNum;

        public DerGeneralizedTime CrlTime => this.crlTime;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.crlUrl != null)
                v.Add( new DerTaggedObject( true, 0, crlUrl ) );
            if (this.crlNum != null)
                v.Add( new DerTaggedObject( true, 1, crlNum ) );
            if (this.crlTime != null)
                v.Add( new DerTaggedObject( true, 2, crlTime ) );
            return new DerSequence( v );
        }
    }
}
