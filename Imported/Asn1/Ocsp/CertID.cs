// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.CertID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class CertID : Asn1Encodable
    {
        private readonly AlgorithmIdentifier hashAlgorithm;
        private readonly Asn1OctetString issuerNameHash;
        private readonly Asn1OctetString issuerKeyHash;
        private readonly DerInteger serialNumber;

        public static CertID GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static CertID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CertID _:
                    return (CertID)obj;
                case Asn1Sequence _:
                    return new CertID( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertID(
          AlgorithmIdentifier hashAlgorithm,
          Asn1OctetString issuerNameHash,
          Asn1OctetString issuerKeyHash,
          DerInteger serialNumber )
        {
            this.hashAlgorithm = hashAlgorithm;
            this.issuerNameHash = issuerNameHash;
            this.issuerKeyHash = issuerKeyHash;
            this.serialNumber = serialNumber;
        }

        private CertID( Asn1Sequence seq )
        {
            this.hashAlgorithm = seq.Count == 4 ? AlgorithmIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.issuerNameHash = Asn1OctetString.GetInstance( seq[1] );
            this.issuerKeyHash = Asn1OctetString.GetInstance( seq[2] );
            this.serialNumber = DerInteger.GetInstance( seq[3] );
        }

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public Asn1OctetString IssuerNameHash => this.issuerNameHash;

        public Asn1OctetString IssuerKeyHash => this.issuerKeyHash;

        public DerInteger SerialNumber => this.serialNumber;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[4]
        {
       hashAlgorithm,
       issuerNameHash,
       issuerKeyHash,
       serialNumber
        } );
    }
}
