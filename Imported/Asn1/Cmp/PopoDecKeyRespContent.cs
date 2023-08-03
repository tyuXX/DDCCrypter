// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PopoDecKeyRespContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PopoDecKeyRespContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private PopoDecKeyRespContent( Asn1Sequence seq ) => this.content = seq;

        public static PopoDecKeyRespContent GetInstance( object obj )
        {
            switch (obj)
            {
                case PopoDecKeyRespContent _:
                    return (PopoDecKeyRespContent)obj;
                case Asn1Sequence _:
                    return new PopoDecKeyRespContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual DerInteger[] ToDerIntegerArray()
        {
            DerInteger[] derIntegerArray = new DerInteger[this.content.Count];
            for (int index = 0; index != derIntegerArray.Length; ++index)
                derIntegerArray[index] = DerInteger.GetInstance( this.content[index] );
            return derIntegerArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
