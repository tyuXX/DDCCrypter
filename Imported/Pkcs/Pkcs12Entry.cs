// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.Pkcs12Entry
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Pkcs
{
    public abstract class Pkcs12Entry
    {
        private readonly IDictionary attributes;

        protected internal Pkcs12Entry( IDictionary attributes )
        {
            this.attributes = attributes;
            foreach (DictionaryEntry attribute in attributes)
            {
                if (!(attribute.Key is string))
                    throw new ArgumentException( "Attribute keys must be of type: " + typeof( string ).FullName, nameof( attributes ) );
                if (!(attribute.Value is Asn1Encodable))
                    throw new ArgumentException( "Attribute values must be of type: " + typeof( Asn1Encodable ).FullName, nameof( attributes ) );
            }
        }

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public Asn1Encodable GetBagAttribute( DerObjectIdentifier oid ) => (Asn1Encodable)this.attributes[oid.Id];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public Asn1Encodable GetBagAttribute( string oid ) => (Asn1Encodable)this.attributes[oid];

        [Obsolete( "Use 'BagAttributeKeys' property" )]
        public IEnumerator GetBagAttributeKeys() => this.attributes.Keys.GetEnumerator();

        public Asn1Encodable this[DerObjectIdentifier oid] => (Asn1Encodable)this.attributes[oid.Id];

        public Asn1Encodable this[string oid] => (Asn1Encodable)this.attributes[oid];

        public IEnumerable BagAttributeKeys => new EnumerableProxy( attributes.Keys );
    }
}
