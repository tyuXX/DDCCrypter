// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.DefaultAuthenticatedAttributeTableGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System.Collections;
using Attribute = Org.BouncyCastle.Asn1.Cms.Attribute;

namespace Org.BouncyCastle.Cms
{
    public class DefaultAuthenticatedAttributeTableGenerator : CmsAttributeTableGenerator
    {
        private readonly IDictionary table;

        public DefaultAuthenticatedAttributeTableGenerator() => this.table = Platform.CreateHashtable();

        public DefaultAuthenticatedAttributeTableGenerator( AttributeTable attributeTable )
        {
            if (attributeTable != null)
                this.table = attributeTable.ToDictionary();
            else
                this.table = Platform.CreateHashtable();
        }

        protected virtual IDictionary CreateStandardAttributeTable( IDictionary parameters )
        {
            IDictionary hashtable = Platform.CreateHashtable( this.table );
            if (!hashtable.Contains( CmsAttributes.ContentType ))
            {
                DerObjectIdentifier parameter = (DerObjectIdentifier)parameters[CmsAttributeTableParameter.ContentType];
                Attribute attribute = new Attribute( CmsAttributes.ContentType, new DerSet( parameter ) );
                hashtable[attribute.AttrType] = attribute;
            }
            if (!hashtable.Contains( CmsAttributes.MessageDigest ))
            {
                byte[] parameter = (byte[])parameters[CmsAttributeTableParameter.Digest];
                Attribute attribute = new Attribute( CmsAttributes.MessageDigest, new DerSet( new DerOctetString( parameter ) ) );
                hashtable[attribute.AttrType] = attribute;
            }
            return hashtable;
        }

        public virtual AttributeTable GetAttributes( IDictionary parameters ) => new AttributeTable( this.CreateStandardAttributeTable( parameters ) );
    }
}
