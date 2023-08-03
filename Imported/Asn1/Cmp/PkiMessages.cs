// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiMessages
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiMessages : Asn1Encodable
    {
        private Asn1Sequence content;

        private PkiMessages( Asn1Sequence seq ) => this.content = seq;

        public static PkiMessages GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiMessages _:
                    return (PkiMessages)obj;
                case Asn1Sequence _:
                    return new PkiMessages( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public PkiMessages( params PkiMessage[] msgs ) => this.content = new DerSequence( msgs );

        public virtual PkiMessage[] ToPkiMessageArray()
        {
            PkiMessage[] pkiMessageArray = new PkiMessage[this.content.Count];
            for (int index = 0; index != pkiMessageArray.Length; ++index)
                pkiMessageArray[index] = PkiMessage.GetInstance( this.content[index] );
            return pkiMessageArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
