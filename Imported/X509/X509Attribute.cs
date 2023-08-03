// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509Attribute
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.X509
{
    public class X509Attribute : Asn1Encodable
    {
        private readonly AttributeX509 attr;

        internal X509Attribute( Asn1Encodable at ) => this.attr = AttributeX509.GetInstance( at );

        public X509Attribute( string oid, Asn1Encodable value ) => this.attr = new AttributeX509( new DerObjectIdentifier( oid ), new DerSet( value ) );

        public X509Attribute( string oid, Asn1EncodableVector value ) => this.attr = new AttributeX509( new DerObjectIdentifier( oid ), new DerSet( value ) );

        public string Oid => this.attr.AttrType.Id;

        public Asn1Encodable[] GetValues()
        {
            Asn1Set attrValues = this.attr.AttrValues;
            Asn1Encodable[] values = new Asn1Encodable[attrValues.Count];
            for (int index = 0; index != attrValues.Count; ++index)
                values[index] = attrValues[index];
            return values;
        }

        public override Asn1Object ToAsn1Object() => this.attr.ToAsn1Object();
    }
}
