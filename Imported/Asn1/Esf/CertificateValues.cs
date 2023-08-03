// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CertificateValues
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CertificateValues : Asn1Encodable
    {
        private readonly Asn1Sequence certificates;

        public static CertificateValues GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CertificateValues _:
                    return (CertificateValues)obj;
                case Asn1Sequence _:
                    return new CertificateValues( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CertificateValues' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CertificateValues( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            foreach (Asn1Encodable asn1Encodable in seq)
                X509CertificateStructure.GetInstance( asn1Encodable.ToAsn1Object() );
            this.certificates = seq;
        }

        public CertificateValues( params X509CertificateStructure[] certificates ) => this.certificates = certificates != null ? (Asn1Sequence)new DerSequence( certificates ) : throw new ArgumentNullException( nameof( certificates ) );

        public CertificateValues( IEnumerable certificates )
        {
            if (certificates == null)
                throw new ArgumentNullException( nameof( certificates ) );
            this.certificates = CollectionUtilities.CheckElementsAreOfType( certificates, typeof( X509CertificateStructure ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( certificates ) ) : throw new ArgumentException( "Must contain only 'X509CertificateStructure' objects", nameof( certificates ) );
        }

        public X509CertificateStructure[] GetCertificates()
        {
            X509CertificateStructure[] certificates = new X509CertificateStructure[this.certificates.Count];
            for (int index = 0; index < this.certificates.Count; ++index)
                certificates[index] = X509CertificateStructure.GetInstance( this.certificates[index] );
            return certificates;
        }

        public override Asn1Object ToAsn1Object() => certificates;
    }
}
