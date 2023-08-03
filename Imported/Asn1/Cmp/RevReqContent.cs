// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.RevReqContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class RevReqContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private RevReqContent( Asn1Sequence seq ) => this.content = seq;

        public static RevReqContent GetInstance( object obj )
        {
            switch (obj)
            {
                case RevReqContent _:
                    return (RevReqContent)obj;
                case Asn1Sequence _:
                    return new RevReqContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public RevReqContent( params RevDetails[] revDetails ) => this.content = new DerSequence( revDetails );

        public virtual RevDetails[] ToRevDetailsArray()
        {
            RevDetails[] revDetailsArray = new RevDetails[this.content.Count];
            for (int index = 0; index != revDetailsArray.Length; ++index)
                revDetailsArray[index] = RevDetails.GetInstance( this.content[index] );
            return revDetailsArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
