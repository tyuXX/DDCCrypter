// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SimpleAttributeTableGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cms;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class SimpleAttributeTableGenerator : CmsAttributeTableGenerator
    {
        private readonly AttributeTable attributes;

        public SimpleAttributeTableGenerator( AttributeTable attributes ) => this.attributes = attributes;

        public virtual AttributeTable GetAttributes( IDictionary parameters ) => this.attributes;
    }
}
