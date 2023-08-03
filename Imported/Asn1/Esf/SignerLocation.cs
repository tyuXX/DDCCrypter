// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.SignerLocation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class SignerLocation : Asn1Encodable
    {
        private DerUtf8String countryName;
        private DerUtf8String localityName;
        private Asn1Sequence postalAddress;

        public SignerLocation( Asn1Sequence seq )
        {
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.countryName = DerUtf8String.GetInstance( asn1TaggedObject, true );
                        continue;
                    case 1:
                        this.localityName = DerUtf8String.GetInstance( asn1TaggedObject, true );
                        continue;
                    case 2:
                        bool explicitly = asn1TaggedObject.IsExplicit();
                        this.postalAddress = Asn1Sequence.GetInstance( asn1TaggedObject, explicitly );
                        if (this.postalAddress != null && this.postalAddress.Count > 6)
                            throw new ArgumentException( "postal address must contain less than 6 strings" );
                        continue;
                    default:
                        throw new ArgumentException( "illegal tag" );
                }
            }
        }

        public SignerLocation(
          DerUtf8String countryName,
          DerUtf8String localityName,
          Asn1Sequence postalAddress )
        {
            if (postalAddress != null && postalAddress.Count > 6)
                throw new ArgumentException( "postal address must contain less than 6 strings" );
            if (countryName != null)
                this.countryName = DerUtf8String.GetInstance( countryName.ToAsn1Object() );
            if (localityName != null)
                this.localityName = DerUtf8String.GetInstance( localityName.ToAsn1Object() );
            if (postalAddress == null)
                return;
            this.postalAddress = (Asn1Sequence)postalAddress.ToAsn1Object();
        }

        public static SignerLocation GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SignerLocation _:
                    return (SignerLocation)obj;
                default:
                    return new SignerLocation( Asn1Sequence.GetInstance( obj ) );
            }
        }

        public DerUtf8String CountryName => this.countryName;

        public DerUtf8String LocalityName => this.localityName;

        public Asn1Sequence PostalAddress => this.postalAddress;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.countryName != null)
                v.Add( new DerTaggedObject( true, 0, countryName ) );
            if (this.localityName != null)
                v.Add( new DerTaggedObject( true, 1, localityName ) );
            if (this.postalAddress != null)
                v.Add( new DerTaggedObject( true, 2, postalAddress ) );
            return new DerSequence( v );
        }
    }
}
