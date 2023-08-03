// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.Attribute
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class Attribute : Asn1Encodable
    {
        private DerObjectIdentifier attrType;
        private Asn1Set attrValues;

        public static Attribute GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Attribute _:
                    return (Attribute)obj;
                case Asn1Sequence _:
                    return new Attribute( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Attribute( Asn1Sequence seq )
        {
            this.attrType = (DerObjectIdentifier)seq[0];
            this.attrValues = (Asn1Set)seq[1];
        }

        public Attribute( DerObjectIdentifier attrType, Asn1Set attrValues )
        {
            this.attrType = attrType;
            this.attrValues = attrValues;
        }

        public DerObjectIdentifier AttrType => this.attrType;

        public Asn1Set AttrValues => this.attrValues;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       attrType,
       attrValues
        } );
    }
}
