// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.Controls
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class Controls : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private Controls( Asn1Sequence seq ) => this.content = seq;

        public static Controls GetInstance( object obj )
        {
            switch (obj)
            {
                case Controls _:
                    return (Controls)obj;
                case Asn1Sequence _:
                    return new Controls( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Controls( params AttributeTypeAndValue[] atvs ) => this.content = new DerSequence( atvs );

        public virtual AttributeTypeAndValue[] ToAttributeTypeAndValueArray()
        {
            AttributeTypeAndValue[] typeAndValueArray = new AttributeTypeAndValue[this.content.Count];
            for (int index = 0; index != typeAndValueArray.Length; ++index)
                typeAndValueArray[index] = AttributeTypeAndValue.GetInstance( this.content[index] );
            return typeAndValueArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
