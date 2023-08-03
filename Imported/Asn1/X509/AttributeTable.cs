// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AttributeTable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AttributeTable
    {
        private readonly IDictionary attributes;

        public AttributeTable( IDictionary attrs ) => this.attributes = Platform.CreateHashtable( attrs );

        [Obsolete]
        public AttributeTable( Hashtable attrs ) => this.attributes = Platform.CreateHashtable( attrs );

        public AttributeTable( Asn1EncodableVector v )
        {
            this.attributes = Platform.CreateHashtable( v.Count );
            for (int index = 0; index != v.Count; ++index)
            {
                AttributeX509 instance = AttributeX509.GetInstance( v[index] );
                this.attributes.Add( instance.AttrType, instance );
            }
        }

        public AttributeTable( Asn1Set s )
        {
            this.attributes = Platform.CreateHashtable( s.Count );
            for (int index = 0; index != s.Count; ++index)
            {
                AttributeX509 instance = AttributeX509.GetInstance( s[index] );
                this.attributes.Add( instance.AttrType, instance );
            }
        }

        public AttributeX509 Get( DerObjectIdentifier oid ) => (AttributeX509)this.attributes[oid];

        [Obsolete( "Use 'ToDictionary' instead" )]
        public Hashtable ToHashtable() => new Hashtable( this.attributes );

        public IDictionary ToDictionary() => Platform.CreateHashtable( this.attributes );
    }
}
