// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.Attributes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class Attributes : Asn1Encodable
    {
        private readonly Asn1Set attributes;

        private Attributes( Asn1Set attributes ) => this.attributes = attributes;

        public Attributes( Asn1EncodableVector v ) => this.attributes = new BerSet( v );

        public static Attributes GetInstance( object obj )
        {
            if (obj is Attributes)
                return (Attributes)obj;
            return obj != null ? new Attributes( Asn1Set.GetInstance( obj ) ) : null;
        }

        public virtual Attribute[] GetAttributes()
        {
            Attribute[] attributes = new Attribute[this.attributes.Count];
            for (int index = 0; index != attributes.Length; ++index)
                attributes[index] = Attribute.GetInstance( this.attributes[index] );
            return attributes;
        }

        public override Asn1Object ToAsn1Object() => attributes;
    }
}
