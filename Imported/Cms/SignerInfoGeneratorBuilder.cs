// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SignerInfoGeneratorBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;

namespace Org.BouncyCastle.Cms
{
    public class SignerInfoGeneratorBuilder
    {
        private bool directSignature;
        private CmsAttributeTableGenerator signedGen;
        private CmsAttributeTableGenerator unsignedGen;

        public SignerInfoGeneratorBuilder SetDirectSignature( bool hasNoSignedAttributes )
        {
            this.directSignature = hasNoSignedAttributes;
            return this;
        }

        public SignerInfoGeneratorBuilder WithSignedAttributeGenerator(
          CmsAttributeTableGenerator signedGen )
        {
            this.signedGen = signedGen;
            return this;
        }

        public SignerInfoGeneratorBuilder WithUnsignedAttributeGenerator(
          CmsAttributeTableGenerator unsignedGen )
        {
            this.unsignedGen = unsignedGen;
            return this;
        }

        public SignerInfoGenerator Build( ISignatureFactory contentSigner, X509Certificate certificate )
        {
            SignerIdentifier sigId = new( new IssuerAndSerialNumber( certificate.IssuerDN, new DerInteger( certificate.SerialNumber ) ) );
            SignerInfoGenerator generator = this.CreateGenerator( contentSigner, sigId );
            generator.setAssociatedCertificate( certificate );
            return generator;
        }

        public SignerInfoGenerator Build( ISignatureFactory signerFactory, byte[] subjectKeyIdentifier )
        {
            SignerIdentifier sigId = new( new DerOctetString( subjectKeyIdentifier ) );
            return this.CreateGenerator( signerFactory, sigId );
        }

        private SignerInfoGenerator CreateGenerator(
          ISignatureFactory contentSigner,
          SignerIdentifier sigId )
        {
            if (this.directSignature)
                return new SignerInfoGenerator( sigId, contentSigner, true );
            if (this.signedGen == null && this.unsignedGen == null)
                return new SignerInfoGenerator( sigId, contentSigner );
            if (this.signedGen == null)
                this.signedGen = new DefaultSignedAttributeTableGenerator();
            return new SignerInfoGenerator( sigId, contentSigner, this.signedGen, this.unsignedGen );
        }
    }
}
