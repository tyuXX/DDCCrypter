// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.SignerAttribute
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class SignerAttribute : Asn1Encodable
    {
        private Asn1Sequence claimedAttributes;
        private AttributeCertificate certifiedAttributes;

        public static SignerAttribute GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SignerAttribute _:
                    return (SignerAttribute)obj;
                case Asn1Sequence _:
                    return new SignerAttribute( obj );
                default:
                    throw new ArgumentException( "Unknown object in 'SignerAttribute' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private SignerAttribute( object obj )
        {
            DerTaggedObject derTaggedObject = (DerTaggedObject)((Asn1Sequence)obj)[0];
            if (derTaggedObject.TagNo == 0)
                this.claimedAttributes = Asn1Sequence.GetInstance( derTaggedObject, true );
            else
                this.certifiedAttributes = derTaggedObject.TagNo == 1 ? AttributeCertificate.GetInstance( derTaggedObject ) : throw new ArgumentException( "illegal tag.", nameof( obj ) );
        }

        public SignerAttribute( Asn1Sequence claimedAttributes ) => this.claimedAttributes = claimedAttributes;

        public SignerAttribute( AttributeCertificate certifiedAttributes ) => this.certifiedAttributes = certifiedAttributes;

        public virtual Asn1Sequence ClaimedAttributes => this.claimedAttributes;

        public virtual AttributeCertificate CertifiedAttributes => this.certifiedAttributes;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.claimedAttributes != null)
                v.Add( new DerTaggedObject( 0, claimedAttributes ) );
            else
                v.Add( new DerTaggedObject( 1, certifiedAttributes ) );
            return new DerSequence( v );
        }
    }
}
