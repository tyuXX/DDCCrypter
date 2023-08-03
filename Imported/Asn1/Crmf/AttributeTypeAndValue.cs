// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.AttributeTypeAndValue
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class AttributeTypeAndValue : Asn1Encodable
    {
        private readonly DerObjectIdentifier type;
        private readonly Asn1Encodable value;

        private AttributeTypeAndValue( Asn1Sequence seq )
        {
            this.type = (DerObjectIdentifier)seq[0];
            this.value = seq[1];
        }

        public static AttributeTypeAndValue GetInstance( object obj )
        {
            switch (obj)
            {
                case AttributeTypeAndValue _:
                    return (AttributeTypeAndValue)obj;
                case Asn1Sequence _:
                    return new AttributeTypeAndValue( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public AttributeTypeAndValue( string oid, Asn1Encodable value )
          : this( new DerObjectIdentifier( oid ), value )
        {
        }

        public AttributeTypeAndValue( DerObjectIdentifier type, Asn1Encodable value )
        {
            this.type = type;
            this.value = value;
        }

        public virtual DerObjectIdentifier Type => this.type;

        public virtual Asn1Encodable Value => this.value;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       type,
      this.value
        } );
    }
}
