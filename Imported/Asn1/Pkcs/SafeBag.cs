// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.SafeBag
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class SafeBag : Asn1Encodable
    {
        private readonly DerObjectIdentifier bagID;
        private readonly Asn1Object bagValue;
        private readonly Asn1Set bagAttributes;

        public SafeBag( DerObjectIdentifier oid, Asn1Object obj )
        {
            this.bagID = oid;
            this.bagValue = obj;
            this.bagAttributes = null;
        }

        public SafeBag( DerObjectIdentifier oid, Asn1Object obj, Asn1Set bagAttributes )
        {
            this.bagID = oid;
            this.bagValue = obj;
            this.bagAttributes = bagAttributes;
        }

        public SafeBag( Asn1Sequence seq )
        {
            this.bagID = (DerObjectIdentifier)seq[0];
            this.bagValue = ((Asn1TaggedObject)seq[1]).GetObject();
            if (seq.Count != 3)
                return;
            this.bagAttributes = (Asn1Set)seq[2];
        }

        public DerObjectIdentifier BagID => this.bagID;

        public Asn1Object BagValue => this.bagValue;

        public Asn1Set BagAttributes => this.bagAttributes;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         bagID,
         new DerTaggedObject(0,  bagValue)
            } );
            if (this.bagAttributes != null)
                v.Add( bagAttributes );
            return new DerSequence( v );
        }
    }
}
