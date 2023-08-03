// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AttributeX509
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AttributeX509 : Asn1Encodable
    {
        private readonly DerObjectIdentifier attrType;
        private readonly Asn1Set attrValues;

        public static AttributeX509 GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case AttributeX509 _:
                    return (AttributeX509)obj;
                case Asn1Sequence _:
                    return new AttributeX509( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private AttributeX509( Asn1Sequence seq )
        {
            this.attrType = seq.Count == 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.attrValues = Asn1Set.GetInstance( seq[1] );
        }

        public AttributeX509( DerObjectIdentifier attrType, Asn1Set attrValues )
        {
            this.attrType = attrType;
            this.attrValues = attrValues;
        }

        public DerObjectIdentifier AttrType => this.attrType;

        public Asn1Encodable[] GetAttributeValues() => this.attrValues.ToArray();

        public Asn1Set AttrValues => this.attrValues;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       attrType,
       attrValues
        } );
    }
}
