// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.OtherKeyAttribute
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class OtherKeyAttribute : Asn1Encodable
    {
        private DerObjectIdentifier keyAttrId;
        private Asn1Encodable keyAttr;

        public static OtherKeyAttribute GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OtherKeyAttribute _:
                    return (OtherKeyAttribute)obj;
                case Asn1Sequence _:
                    return new OtherKeyAttribute( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public OtherKeyAttribute( Asn1Sequence seq )
        {
            this.keyAttrId = (DerObjectIdentifier)seq[0];
            this.keyAttr = seq[1];
        }

        public OtherKeyAttribute( DerObjectIdentifier keyAttrId, Asn1Encodable keyAttr )
        {
            this.keyAttrId = keyAttrId;
            this.keyAttr = keyAttr;
        }

        public DerObjectIdentifier KeyAttrId => this.keyAttrId;

        public Asn1Encodable KeyAttr => this.keyAttr;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       keyAttrId,
      this.keyAttr
        } );
    }
}
