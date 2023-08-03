// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.CertificationRequestInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class CertificationRequestInfo : Asn1Encodable
    {
        internal DerInteger version = new( 0 );
        internal X509Name subject;
        internal SubjectPublicKeyInfo subjectPKInfo;
        internal Asn1Set attributes;

        public static CertificationRequestInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case CertificationRequestInfo _:
                    return (CertificationRequestInfo)obj;
                case Asn1Sequence _:
                    return new CertificationRequestInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertificationRequestInfo(
          X509Name subject,
          SubjectPublicKeyInfo pkInfo,
          Asn1Set attributes )
        {
            this.subject = subject;
            this.subjectPKInfo = pkInfo;
            this.attributes = attributes;
            if (subject == null || this.version == null || this.subjectPKInfo == null)
                throw new ArgumentException( "Not all mandatory fields set in CertificationRequestInfo generator." );
        }

        private CertificationRequestInfo( Asn1Sequence seq )
        {
            this.version = (DerInteger)seq[0];
            this.subject = X509Name.GetInstance( seq[1] );
            this.subjectPKInfo = SubjectPublicKeyInfo.GetInstance( seq[2] );
            if (seq.Count > 3)
                this.attributes = Asn1Set.GetInstance( (Asn1TaggedObject)seq[3], false );
            if (this.subject == null || this.version == null || this.subjectPKInfo == null)
                throw new ArgumentException( "Not all mandatory fields set in CertificationRequestInfo generator." );
        }

        public DerInteger Version => this.version;

        public X509Name Subject => this.subject;

        public SubjectPublicKeyInfo SubjectPublicKeyInfo => this.subjectPKInfo;

        public Asn1Set Attributes => this.attributes;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         version,
         subject,
         subjectPKInfo
            } );
            if (this.attributes != null)
                v.Add( new DerTaggedObject( false, 0, attributes ) );
            return new DerSequence( v );
        }
    }
}
