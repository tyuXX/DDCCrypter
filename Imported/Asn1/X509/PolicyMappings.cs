// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.PolicyMappings
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class PolicyMappings : Asn1Encodable
    {
        private readonly Asn1Sequence seq;

        public PolicyMappings( Asn1Sequence seq ) => this.seq = seq;

        public PolicyMappings( Hashtable mappings )
          : this( (IDictionary)mappings )
        {
        }

        public PolicyMappings( IDictionary mappings )
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (string key in (IEnumerable)mappings.Keys)
            {
                string mapping = (string)mappings[key];
                v.Add( new DerSequence( new Asn1Encodable[2]
                {
           new DerObjectIdentifier(key),
           new DerObjectIdentifier(mapping)
                } ) );
            }
            this.seq = new DerSequence( v );
        }

        public override Asn1Object ToAsn1Object() => seq;
    }
}
