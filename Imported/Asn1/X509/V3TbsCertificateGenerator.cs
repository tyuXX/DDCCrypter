// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.V3TbsCertificateGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class V3TbsCertificateGenerator
    {
        internal DerTaggedObject version = new DerTaggedObject( 0, new DerInteger( 2 ) );
        internal DerInteger serialNumber;
        internal AlgorithmIdentifier signature;
        internal X509Name issuer;
        internal Time startDate;
        internal Time endDate;
        internal X509Name subject;
        internal SubjectPublicKeyInfo subjectPublicKeyInfo;
        internal X509Extensions extensions;
        private bool altNamePresentAndCritical;
        private DerBitString issuerUniqueID;
        private DerBitString subjectUniqueID;

        public void SetSerialNumber( DerInteger serialNumber ) => this.serialNumber = serialNumber;

        public void SetSignature( AlgorithmIdentifier signature ) => this.signature = signature;

        public void SetIssuer( X509Name issuer ) => this.issuer = issuer;

        public void SetStartDate( DerUtcTime startDate ) => this.startDate = new Time( startDate );

        public void SetStartDate( Time startDate ) => this.startDate = startDate;

        public void SetEndDate( DerUtcTime endDate ) => this.endDate = new Time( endDate );

        public void SetEndDate( Time endDate ) => this.endDate = endDate;

        public void SetSubject( X509Name subject ) => this.subject = subject;

        public void SetIssuerUniqueID( DerBitString uniqueID ) => this.issuerUniqueID = uniqueID;

        public void SetSubjectUniqueID( DerBitString uniqueID ) => this.subjectUniqueID = uniqueID;

        public void SetSubjectPublicKeyInfo( SubjectPublicKeyInfo pubKeyInfo ) => this.subjectPublicKeyInfo = pubKeyInfo;

        public void SetExtensions( X509Extensions extensions )
        {
            this.extensions = extensions;
            if (extensions == null)
                return;
            X509Extension extension = extensions.GetExtension( X509Extensions.SubjectAlternativeName );
            if (extension == null || !extension.IsCritical)
                return;
            this.altNamePresentAndCritical = true;
        }

        public TbsCertificateStructure GenerateTbsCertificate()
        {
            if (this.serialNumber == null || this.signature == null || this.issuer == null || this.startDate == null || this.endDate == null || (this.subject == null && !this.altNamePresentAndCritical) || this.subjectPublicKeyInfo == null)
                throw new InvalidOperationException( "not all mandatory fields set in V3 TBScertificate generator" );
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[5]
            {
         version,
         serialNumber,
         signature,
         issuer,
         new DerSequence(new Asn1Encodable[2]
        {
           startDate,
           endDate
        })
            } );
            if (this.subject != null)
                v.Add( subject );
            else
                v.Add( DerSequence.Empty );
            v.Add( subjectPublicKeyInfo );
            if (this.issuerUniqueID != null)
                v.Add( new DerTaggedObject( false, 1, issuerUniqueID ) );
            if (this.subjectUniqueID != null)
                v.Add( new DerTaggedObject( false, 2, subjectUniqueID ) );
            if (this.extensions != null)
                v.Add( new DerTaggedObject( 3, extensions ) );
            return new TbsCertificateStructure( new DerSequence( v ) );
        }
    }
}
