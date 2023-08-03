// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509CertificateStructure
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class X509CertificateStructure : Asn1Encodable
    {
        private readonly TbsCertificateStructure tbsCert;
        private readonly AlgorithmIdentifier sigAlgID;
        private readonly DerBitString sig;

        public static X509CertificateStructure GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static X509CertificateStructure GetInstance( object obj )
        {
            if (obj is X509CertificateStructure)
                return (X509CertificateStructure)obj;
            return obj == null ? null : new X509CertificateStructure( Asn1Sequence.GetInstance( obj ) );
        }

        public X509CertificateStructure(
          TbsCertificateStructure tbsCert,
          AlgorithmIdentifier sigAlgID,
          DerBitString sig )
        {
            if (tbsCert == null)
                throw new ArgumentNullException( nameof( tbsCert ) );
            if (sigAlgID == null)
                throw new ArgumentNullException( nameof( sigAlgID ) );
            if (sig == null)
                throw new ArgumentNullException( nameof( sig ) );
            this.tbsCert = tbsCert;
            this.sigAlgID = sigAlgID;
            this.sig = sig;
        }

        private X509CertificateStructure( Asn1Sequence seq )
        {
            this.tbsCert = seq.Count == 3 ? TbsCertificateStructure.GetInstance( seq[0] ) : throw new ArgumentException( "sequence wrong size for a certificate", nameof( seq ) );
            this.sigAlgID = AlgorithmIdentifier.GetInstance( seq[1] );
            this.sig = DerBitString.GetInstance( seq[2] );
        }

        public TbsCertificateStructure TbsCertificate => this.tbsCert;

        public int Version => this.tbsCert.Version;

        public DerInteger SerialNumber => this.tbsCert.SerialNumber;

        public X509Name Issuer => this.tbsCert.Issuer;

        public Time StartDate => this.tbsCert.StartDate;

        public Time EndDate => this.tbsCert.EndDate;

        public X509Name Subject => this.tbsCert.Subject;

        public SubjectPublicKeyInfo SubjectPublicKeyInfo => this.tbsCert.SubjectPublicKeyInfo;

        public AlgorithmIdentifier SignatureAlgorithm => this.sigAlgID;

        public DerBitString Signature => this.sig;

        public byte[] GetSignatureOctets() => this.sig.GetOctets();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       tbsCert,
       sigAlgID,
       sig
        } );
    }
}
