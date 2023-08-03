// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CertificateList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class CertificateList : Asn1Encodable
    {
        private readonly TbsCertificateList tbsCertList;
        private readonly AlgorithmIdentifier sigAlgID;
        private readonly DerBitString sig;

        public static CertificateList GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static CertificateList GetInstance( object obj )
        {
            if (obj is CertificateList)
                return (CertificateList)obj;
            return obj != null ? new CertificateList( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        private CertificateList( Asn1Sequence seq )
        {
            this.tbsCertList = seq.Count == 3 ? TbsCertificateList.GetInstance( seq[0] ) : throw new ArgumentException( "sequence wrong size for CertificateList", nameof( seq ) );
            this.sigAlgID = AlgorithmIdentifier.GetInstance( seq[1] );
            this.sig = DerBitString.GetInstance( seq[2] );
        }

        public TbsCertificateList TbsCertList => this.tbsCertList;

        public CrlEntry[] GetRevokedCertificates() => this.tbsCertList.GetRevokedCertificates();

        public IEnumerable GetRevokedCertificateEnumeration() => this.tbsCertList.GetRevokedCertificateEnumeration();

        public AlgorithmIdentifier SignatureAlgorithm => this.sigAlgID;

        public DerBitString Signature => this.sig;

        public byte[] GetSignatureOctets() => this.sig.GetOctets();

        public int Version => this.tbsCertList.Version;

        public X509Name Issuer => this.tbsCertList.Issuer;

        public Time ThisUpdate => this.tbsCertList.ThisUpdate;

        public Time NextUpdate => this.tbsCertList.NextUpdate;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       tbsCertList,
       sigAlgID,
       sig
        } );
    }
}
