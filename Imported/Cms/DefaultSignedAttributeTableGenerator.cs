// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.DefaultSignedAttributeTableGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class DefaultSignedAttributeTableGenerator : CmsAttributeTableGenerator
    {
        private readonly IDictionary table;

        public DefaultSignedAttributeTableGenerator() => this.table = Platform.CreateHashtable();

        public DefaultSignedAttributeTableGenerator( AttributeTable attributeTable )
        {
            if (attributeTable != null)
                this.table = attributeTable.ToDictionary();
            else
                this.table = Platform.CreateHashtable();
        }

        protected virtual Hashtable createStandardAttributeTable( IDictionary parameters )
        {
            Hashtable std = new Hashtable( this.table );
            this.DoCreateStandardAttributeTable( parameters, std );
            return std;
        }

        private void DoCreateStandardAttributeTable( IDictionary parameters, IDictionary std )
        {
            if (parameters.Contains( CmsAttributeTableParameter.ContentType ) && !std.Contains( CmsAttributes.ContentType ))
            {
                DerObjectIdentifier parameter = (DerObjectIdentifier)parameters[CmsAttributeTableParameter.ContentType];
                Org.BouncyCastle.Asn1.Cms.Attribute attribute = new Org.BouncyCastle.Asn1.Cms.Attribute( CmsAttributes.ContentType, new DerSet( parameter ) );
                std[attribute.AttrType] = attribute;
            }
            if (!std.Contains( CmsAttributes.SigningTime ))
            {
                Org.BouncyCastle.Asn1.Cms.Attribute attribute = new Org.BouncyCastle.Asn1.Cms.Attribute( CmsAttributes.SigningTime, new DerSet( new Time( DateTime.UtcNow ) ) );
                std[attribute.AttrType] = attribute;
            }
            if (std.Contains( CmsAttributes.MessageDigest ))
                return;
            byte[] parameter1 = (byte[])parameters[CmsAttributeTableParameter.Digest];
            Org.BouncyCastle.Asn1.Cms.Attribute attribute1 = new Org.BouncyCastle.Asn1.Cms.Attribute( CmsAttributes.MessageDigest, new DerSet( new DerOctetString( parameter1 ) ) );
            std[attribute1.AttrType] = attribute1;
        }

        public virtual AttributeTable GetAttributes( IDictionary parameters ) => new AttributeTable( (IDictionary)this.createStandardAttributeTable( parameters ) );
    }
}
