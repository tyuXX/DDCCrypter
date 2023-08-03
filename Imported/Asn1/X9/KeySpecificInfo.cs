// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.KeySpecificInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Asn1.X9
{
    public class KeySpecificInfo : Asn1Encodable
    {
        private DerObjectIdentifier algorithm;
        private Asn1OctetString counter;

        public KeySpecificInfo( DerObjectIdentifier algorithm, Asn1OctetString counter )
        {
            this.algorithm = algorithm;
            this.counter = counter;
        }

        public KeySpecificInfo( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.algorithm = (DerObjectIdentifier)enumerator.Current;
            enumerator.MoveNext();
            this.counter = (Asn1OctetString)enumerator.Current;
        }

        public DerObjectIdentifier Algorithm => this.algorithm;

        public Asn1OctetString Counter => this.counter;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       algorithm,
       counter
        } );
    }
}
