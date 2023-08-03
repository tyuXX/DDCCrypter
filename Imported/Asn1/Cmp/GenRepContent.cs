// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.GenRepContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class GenRepContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private GenRepContent( Asn1Sequence seq ) => this.content = seq;

        public static GenRepContent GetInstance( object obj )
        {
            switch (obj)
            {
                case GenRepContent _:
                    return (GenRepContent)obj;
                case Asn1Sequence _:
                    return new GenRepContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public GenRepContent( params InfoTypeAndValue[] itv ) => this.content = new DerSequence( itv );

        public virtual InfoTypeAndValue[] ToInfoTypeAndValueArray()
        {
            InfoTypeAndValue[] typeAndValueArray = new InfoTypeAndValue[this.content.Count];
            for (int index = 0; index != typeAndValueArray.Length; ++index)
                typeAndValueArray[index] = InfoTypeAndValue.GetInstance( this.content[index] );
            return typeAndValueArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
