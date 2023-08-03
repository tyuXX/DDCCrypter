// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.TbsCertificateStructure
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class TbsCertificateStructure : Asn1Encodable
    {
        internal Asn1Sequence seq;
        internal DerInteger version;
        internal DerInteger serialNumber;
        internal AlgorithmIdentifier signature;
        internal X509Name issuer;
        internal Time startDate;
        internal Time endDate;
        internal X509Name subject;
        internal SubjectPublicKeyInfo subjectPublicKeyInfo;
        internal DerBitString issuerUniqueID;
        internal DerBitString subjectUniqueID;
        internal X509Extensions extensions;

        public static TbsCertificateStructure GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static TbsCertificateStructure GetInstance( object obj )
        {
            if (obj is TbsCertificateStructure)
                return (TbsCertificateStructure)obj;
            return obj != null ? new TbsCertificateStructure( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        internal TbsCertificateStructure( Asn1Sequence seq )
        {
            int num = 0;
            this.seq = seq;
            if (seq[0] is DerTaggedObject)
            {
                this.version = DerInteger.GetInstance( (Asn1TaggedObject)seq[0], true );
            }
            else
            {
                num = -1;
                this.version = new DerInteger( 0 );
            }
            this.serialNumber = DerInteger.GetInstance( seq[num + 1] );
            this.signature = AlgorithmIdentifier.GetInstance( seq[num + 2] );
            this.issuer = X509Name.GetInstance( seq[num + 3] );
            Asn1Sequence asn1Sequence = (Asn1Sequence)seq[num + 4];
            this.startDate = Time.GetInstance( asn1Sequence[0] );
            this.endDate = Time.GetInstance( asn1Sequence[1] );
            this.subject = X509Name.GetInstance( seq[num + 5] );
            this.subjectPublicKeyInfo = SubjectPublicKeyInfo.GetInstance( seq[num + 6] );
            for (int index = seq.Count - (num + 6) - 1; index > 0; --index)
            {
                DerTaggedObject derTaggedObject = (DerTaggedObject)seq[num + 6 + index];
                switch (derTaggedObject.TagNo)
                {
                    case 1:
                        this.issuerUniqueID = DerBitString.GetInstance( derTaggedObject, false );
                        break;
                    case 2:
                        this.subjectUniqueID = DerBitString.GetInstance( derTaggedObject, false );
                        break;
                    case 3:
                        this.extensions = X509Extensions.GetInstance( derTaggedObject );
                        break;
                }
            }
        }

        public int Version => this.version.Value.IntValue + 1;

        public DerInteger VersionNumber => this.version;

        public DerInteger SerialNumber => this.serialNumber;

        public AlgorithmIdentifier Signature => this.signature;

        public X509Name Issuer => this.issuer;

        public Time StartDate => this.startDate;

        public Time EndDate => this.endDate;

        public X509Name Subject => this.subject;

        public SubjectPublicKeyInfo SubjectPublicKeyInfo => this.subjectPublicKeyInfo;

        public DerBitString IssuerUniqueID => this.issuerUniqueID;

        public DerBitString SubjectUniqueID => this.subjectUniqueID;

        public X509Extensions Extensions => this.extensions;

        public override Asn1Object ToAsn1Object() => seq;
    }
}
