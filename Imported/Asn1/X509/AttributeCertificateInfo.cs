// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AttributeCertificateInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AttributeCertificateInfo : Asn1Encodable
    {
        internal readonly DerInteger version;
        internal readonly Holder holder;
        internal readonly AttCertIssuer issuer;
        internal readonly AlgorithmIdentifier signature;
        internal readonly DerInteger serialNumber;
        internal readonly AttCertValidityPeriod attrCertValidityPeriod;
        internal readonly Asn1Sequence attributes;
        internal readonly DerBitString issuerUniqueID;
        internal readonly X509Extensions extensions;

        public static AttributeCertificateInfo GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static AttributeCertificateInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case AttributeCertificateInfo _:
                    return (AttributeCertificateInfo)obj;
                case Asn1Sequence _:
                    return new AttributeCertificateInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private AttributeCertificateInfo( Asn1Sequence seq )
        {
            this.version = seq.Count >= 7 && seq.Count <= 9 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.holder = Holder.GetInstance( seq[1] );
            this.issuer = AttCertIssuer.GetInstance( seq[2] );
            this.signature = AlgorithmIdentifier.GetInstance( seq[3] );
            this.serialNumber = DerInteger.GetInstance( seq[4] );
            this.attrCertValidityPeriod = AttCertValidityPeriod.GetInstance( seq[5] );
            this.attributes = Asn1Sequence.GetInstance( seq[6] );
            for (int index = 7; index < seq.Count; ++index)
            {
                switch (seq[index])
                {
                    case DerBitString _:
                        this.issuerUniqueID = DerBitString.GetInstance( seq[index] );
                        break;
                    case Asn1Sequence _:
                    case X509Extensions _:
                        this.extensions = X509Extensions.GetInstance( seq[index] );
                        break;
                }
            }
        }

        public DerInteger Version => this.version;

        public Holder Holder => this.holder;

        public AttCertIssuer Issuer => this.issuer;

        public AlgorithmIdentifier Signature => this.signature;

        public DerInteger SerialNumber => this.serialNumber;

        public AttCertValidityPeriod AttrCertValidityPeriod => this.attrCertValidityPeriod;

        public Asn1Sequence Attributes => this.attributes;

        public DerBitString IssuerUniqueID => this.issuerUniqueID;

        public X509Extensions Extensions => this.extensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[7]
            {
         version,
         holder,
         issuer,
         signature,
         serialNumber,
         attrCertValidityPeriod,
         attributes
            } );
            if (this.issuerUniqueID != null)
                v.Add( issuerUniqueID );
            if (this.extensions != null)
                v.Add( extensions );
            return new DerSequence( v );
        }
    }
}
