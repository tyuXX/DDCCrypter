// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.AttributeTable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class AttributeTable
    {
        private readonly IDictionary attributes;

        [Obsolete]
        public AttributeTable( Hashtable attrs ) => this.attributes = Platform.CreateHashtable( attrs );

        public AttributeTable( IDictionary attrs ) => this.attributes = Platform.CreateHashtable( attrs );

        public AttributeTable( Asn1EncodableVector v )
        {
            this.attributes = Platform.CreateHashtable( v.Count );
            foreach (Asn1Encodable asn1Encodable in v)
                this.AddAttribute( Attribute.GetInstance( asn1Encodable ) );
        }

        public AttributeTable( Asn1Set s )
        {
            this.attributes = Platform.CreateHashtable( s.Count );
            for (int index = 0; index != s.Count; ++index)
                this.AddAttribute( Attribute.GetInstance( s[index] ) );
        }

        public AttributeTable( Attributes attrs )
          : this( Asn1Set.GetInstance( attrs.ToAsn1Object() ) )
        {
        }

        private void AddAttribute( Attribute a )
        {
            DerObjectIdentifier attrType = a.AttrType;
            object attribute = this.attributes[attrType];
            if (attribute == null)
            {
                this.attributes[attrType] = a;
            }
            else
            {
                IList list;
                if (attribute is Attribute)
                {
                    list = Platform.CreateArrayList();
                    list.Add( attribute );
                    list.Add( a );
                }
                else
                {
                    list = (IList)attribute;
                    list.Add( a );
                }
                this.attributes[attrType] = list;
            }
        }

        public Attribute this[DerObjectIdentifier oid]
        {
            get
            {
                object attribute = this.attributes[oid];
                return attribute is IList ? (Attribute)((IList)attribute)[0] : (Attribute)attribute;
            }
        }

        [Obsolete( "Use 'object[oid]' syntax instead" )]
        public Attribute Get( DerObjectIdentifier oid ) => this[oid];

        public Asn1EncodableVector GetAll( DerObjectIdentifier oid )
        {
            Asn1EncodableVector all = new( new Asn1Encodable[0] );
            object attribute1 = this.attributes[oid];
            if (attribute1 is IList)
            {
                foreach (Attribute attribute2 in (IEnumerable)attribute1)
                    all.Add( attribute2 );
            }
            else if (attribute1 != null)
                all.Add( (Asn1Encodable)attribute1 );
            return all;
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (object obj in (IEnumerable)this.attributes.Values)
                {
                    if (obj is IList)
                        count += ((ICollection)obj).Count;
                    else
                        ++count;
                }
                return count;
            }
        }

        public IDictionary ToDictionary() => Platform.CreateHashtable( this.attributes );

        [Obsolete( "Use 'ToDictionary' instead" )]
        public Hashtable ToHashtable() => new( this.attributes );

        public Asn1EncodableVector ToAsn1EncodableVector()
        {
            Asn1EncodableVector asn1EncodableVector = new( new Asn1Encodable[0] );
            foreach (object obj1 in (IEnumerable)this.attributes.Values)
            {
                if (obj1 is IList)
                {
                    foreach (object obj2 in (IEnumerable)obj1)
                        asn1EncodableVector.Add( Attribute.GetInstance( obj2 ) );
                }
                else
                    asn1EncodableVector.Add( Attribute.GetInstance( obj1 ) );
            }
            return asn1EncodableVector;
        }

        public Attributes ToAttributes() => new( this.ToAsn1EncodableVector() );

        public AttributeTable Add( DerObjectIdentifier attrType, Asn1Encodable attrValue )
        {
            AttributeTable attributeTable = new( this.attributes );
            attributeTable.AddAttribute( new Attribute( attrType, new DerSet( attrValue ) ) );
            return attributeTable;
        }

        public AttributeTable Remove( DerObjectIdentifier attrType )
        {
            AttributeTable attributeTable = new( this.attributes );
            attributeTable.attributes.Remove( attrType );
            return attributeTable;
        }
    }
}
