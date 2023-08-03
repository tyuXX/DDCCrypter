// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AttributeCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AttributeCertificate : Asn1Encodable
    {
        private readonly AttributeCertificateInfo acinfo;
        private readonly AlgorithmIdentifier signatureAlgorithm;
        private readonly DerBitString signatureValue;

        public static AttributeCertificate GetInstance( object obj )
        {
            if (obj is AttributeCertificate)
                return (AttributeCertificate)obj;
            return obj != null ? new AttributeCertificate( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public AttributeCertificate(
          AttributeCertificateInfo acinfo,
          AlgorithmIdentifier signatureAlgorithm,
          DerBitString signatureValue )
        {
            this.acinfo = acinfo;
            this.signatureAlgorithm = signatureAlgorithm;
            this.signatureValue = signatureValue;
        }

        private AttributeCertificate( Asn1Sequence seq )
        {
            this.acinfo = seq.Count == 3 ? AttributeCertificateInfo.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.signatureAlgorithm = AlgorithmIdentifier.GetInstance( seq[1] );
            this.signatureValue = DerBitString.GetInstance( seq[2] );
        }

        public AttributeCertificateInfo ACInfo => this.acinfo;

        public AlgorithmIdentifier SignatureAlgorithm => this.signatureAlgorithm;

        public DerBitString SignatureValue => this.signatureValue;

        public byte[] GetSignatureOctets() => this.signatureValue.GetOctets();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       acinfo,
       signatureAlgorithm,
       signatureValue
        } );
    }
}
