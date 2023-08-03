// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OtherRevVals
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OtherRevVals : Asn1Encodable
    {
        private readonly DerObjectIdentifier otherRevValType;
        private readonly Asn1Object otherRevVals;

        public static OtherRevVals GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OtherRevVals _:
                    return (OtherRevVals)obj;
                case Asn1Sequence _:
                    return new OtherRevVals( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'OtherRevVals' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private OtherRevVals( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.otherRevValType = seq.Count == 2 ? (DerObjectIdentifier)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.otherRevVals = seq[1].ToAsn1Object();
        }

        public OtherRevVals( DerObjectIdentifier otherRevValType, Asn1Encodable otherRevVals )
        {
            if (otherRevValType == null)
                throw new ArgumentNullException( nameof( otherRevValType ) );
            if (otherRevVals == null)
                throw new ArgumentNullException( nameof( otherRevVals ) );
            this.otherRevValType = otherRevValType;
            this.otherRevVals = otherRevVals.ToAsn1Object();
        }

        public DerObjectIdentifier OtherRevValType => this.otherRevValType;

        public Asn1Object OtherRevValsObject => this.otherRevVals;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       otherRevValType,
       otherRevVals
        } );
    }
}
