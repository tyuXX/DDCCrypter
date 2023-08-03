// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.GenMsgContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class GenMsgContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private GenMsgContent( Asn1Sequence seq ) => this.content = seq;

        public static GenMsgContent GetInstance( object obj )
        {
            switch (obj)
            {
                case GenMsgContent _:
                    return (GenMsgContent)obj;
                case Asn1Sequence _:
                    return new GenMsgContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public GenMsgContent( params InfoTypeAndValue[] itv ) => this.content = new DerSequence( itv );

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
