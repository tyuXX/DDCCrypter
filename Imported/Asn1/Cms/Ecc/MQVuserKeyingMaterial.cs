// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.Ecc.MQVuserKeyingMaterial
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms.Ecc
{
    public class MQVuserKeyingMaterial : Asn1Encodable
    {
        private OriginatorPublicKey ephemeralPublicKey;
        private Asn1OctetString addedukm;

        public MQVuserKeyingMaterial( OriginatorPublicKey ephemeralPublicKey, Asn1OctetString addedukm )
        {
            this.ephemeralPublicKey = ephemeralPublicKey;
            this.addedukm = addedukm;
        }

        private MQVuserKeyingMaterial( Asn1Sequence seq )
        {
            this.ephemeralPublicKey = OriginatorPublicKey.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            this.addedukm = Asn1OctetString.GetInstance( (Asn1TaggedObject)seq[1], true );
        }

        public static MQVuserKeyingMaterial GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static MQVuserKeyingMaterial GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case MQVuserKeyingMaterial _:
                    return (MQVuserKeyingMaterial)obj;
                case Asn1Sequence _:
                    return new MQVuserKeyingMaterial( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid MQVuserKeyingMaterial: " + Platform.GetTypeName( obj ) );
            }
        }

        public OriginatorPublicKey EphemeralPublicKey => this.ephemeralPublicKey;

        public Asn1OctetString AddedUkm => this.addedukm;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         ephemeralPublicKey
            } );
            if (this.addedukm != null)
                v.Add( new DerTaggedObject( true, 0, addedukm ) );
            return new DerSequence( v );
        }
    }
}
