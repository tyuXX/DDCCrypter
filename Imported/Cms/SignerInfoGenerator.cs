// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SignerInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;

namespace Org.BouncyCastle.Cms
{
    public class SignerInfoGenerator
    {
        internal X509Certificate certificate;
        internal ISignatureFactory contentSigner;
        internal SignerIdentifier sigId;
        internal CmsAttributeTableGenerator signedGen;
        internal CmsAttributeTableGenerator unsignedGen;
        private bool isDirectSignature;

        internal SignerInfoGenerator( SignerIdentifier sigId, ISignatureFactory signerFactory )
          : this( sigId, signerFactory, false )
        {
        }

        internal SignerInfoGenerator(
          SignerIdentifier sigId,
          ISignatureFactory signerFactory,
          bool isDirectSignature )
        {
            this.sigId = sigId;
            this.contentSigner = signerFactory;
            this.isDirectSignature = isDirectSignature;
            if (this.isDirectSignature)
            {
                this.signedGen = null;
                this.unsignedGen = null;
            }
            else
            {
                this.signedGen = new DefaultSignedAttributeTableGenerator();
                this.unsignedGen = null;
            }
        }

        internal SignerInfoGenerator(
          SignerIdentifier sigId,
          ISignatureFactory contentSigner,
          CmsAttributeTableGenerator signedGen,
          CmsAttributeTableGenerator unsignedGen )
        {
            this.sigId = sigId;
            this.contentSigner = contentSigner;
            this.signedGen = signedGen;
            this.unsignedGen = unsignedGen;
            this.isDirectSignature = false;
        }

        internal void setAssociatedCertificate( X509Certificate certificate ) => this.certificate = certificate;
    }
}
