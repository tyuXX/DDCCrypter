// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CertTemplateBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class CertTemplateBuilder
    {
        private DerInteger version;
        private DerInteger serialNumber;
        private AlgorithmIdentifier signingAlg;
        private X509Name issuer;
        private OptionalValidity validity;
        private X509Name subject;
        private SubjectPublicKeyInfo publicKey;
        private DerBitString issuerUID;
        private DerBitString subjectUID;
        private X509Extensions extensions;

        public virtual CertTemplateBuilder SetVersion( int ver )
        {
            this.version = new DerInteger( ver );
            return this;
        }

        public virtual CertTemplateBuilder SetSerialNumber( DerInteger ser )
        {
            this.serialNumber = ser;
            return this;
        }

        public virtual CertTemplateBuilder SetSigningAlg( AlgorithmIdentifier aid )
        {
            this.signingAlg = aid;
            return this;
        }

        public virtual CertTemplateBuilder SetIssuer( X509Name name )
        {
            this.issuer = name;
            return this;
        }

        public virtual CertTemplateBuilder SetValidity( OptionalValidity v )
        {
            this.validity = v;
            return this;
        }

        public virtual CertTemplateBuilder SetSubject( X509Name name )
        {
            this.subject = name;
            return this;
        }

        public virtual CertTemplateBuilder SetPublicKey( SubjectPublicKeyInfo spki )
        {
            this.publicKey = spki;
            return this;
        }

        public virtual CertTemplateBuilder SetIssuerUID( DerBitString uid )
        {
            this.issuerUID = uid;
            return this;
        }

        public virtual CertTemplateBuilder SetSubjectUID( DerBitString uid )
        {
            this.subjectUID = uid;
            return this;
        }

        public virtual CertTemplateBuilder SetExtensions( X509Extensions extens )
        {
            this.extensions = extens;
            return this;
        }

        public virtual CertTemplate Build()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            this.AddOptional( v, 0, false, version );
            this.AddOptional( v, 1, false, serialNumber );
            this.AddOptional( v, 2, false, signingAlg );
            this.AddOptional( v, 3, true, issuer );
            this.AddOptional( v, 4, false, validity );
            this.AddOptional( v, 5, true, subject );
            this.AddOptional( v, 6, false, publicKey );
            this.AddOptional( v, 7, false, issuerUID );
            this.AddOptional( v, 8, false, subjectUID );
            this.AddOptional( v, 9, false, extensions );
            return CertTemplate.GetInstance( new DerSequence( v ) );
        }

        private void AddOptional( Asn1EncodableVector v, int tagNo, bool isExplicit, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( isExplicit, tagNo, obj ) );
        }
    }
}
