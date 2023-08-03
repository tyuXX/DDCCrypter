// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.InfoTypeAndValue
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class InfoTypeAndValue : Asn1Encodable
    {
        private readonly DerObjectIdentifier infoType;
        private readonly Asn1Encodable infoValue;

        private InfoTypeAndValue( Asn1Sequence seq )
        {
            this.infoType = DerObjectIdentifier.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            this.infoValue = seq[1];
        }

        public static InfoTypeAndValue GetInstance( object obj )
        {
            switch (obj)
            {
                case InfoTypeAndValue _:
                    return (InfoTypeAndValue)obj;
                case Asn1Sequence _:
                    return new InfoTypeAndValue( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public InfoTypeAndValue( DerObjectIdentifier infoType )
        {
            this.infoType = infoType;
            this.infoValue = null;
        }

        public InfoTypeAndValue( DerObjectIdentifier infoType, Asn1Encodable optionalValue )
        {
            this.infoType = infoType;
            this.infoValue = optionalValue;
        }

        public virtual DerObjectIdentifier InfoType => this.infoType;

        public virtual Asn1Encodable InfoValue => this.infoValue;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         infoType
            } );
            if (this.infoValue != null)
                v.Add( this.infoValue );
            return new DerSequence( v );
        }
    }
}
