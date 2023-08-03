// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.OtherInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Asn1.X9
{
    public class OtherInfo : Asn1Encodable
    {
        private KeySpecificInfo keyInfo;
        private Asn1OctetString partyAInfo;
        private Asn1OctetString suppPubInfo;

        public OtherInfo(
          KeySpecificInfo keyInfo,
          Asn1OctetString partyAInfo,
          Asn1OctetString suppPubInfo )
        {
            this.keyInfo = keyInfo;
            this.partyAInfo = partyAInfo;
            this.suppPubInfo = suppPubInfo;
        }

        public OtherInfo( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.keyInfo = new KeySpecificInfo( (Asn1Sequence)enumerator.Current );
            while (enumerator.MoveNext())
            {
                DerTaggedObject current = (DerTaggedObject)enumerator.Current;
                if (current.TagNo == 0)
                    this.partyAInfo = (Asn1OctetString)current.GetObject();
                else if (current.TagNo == 2)
                    this.suppPubInfo = (Asn1OctetString)current.GetObject();
            }
        }

        public KeySpecificInfo KeyInfo => this.keyInfo;

        public Asn1OctetString PartyAInfo => this.partyAInfo;

        public Asn1OctetString SuppPubInfo => this.suppPubInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         keyInfo
            } );
            if (this.partyAInfo != null)
                v.Add( new DerTaggedObject( 0, partyAInfo ) );
            v.Add( new DerTaggedObject( 2, suppPubInfo ) );
            return new DerSequence( v );
        }
    }
}
