// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsEnvelopedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsEnvelopedData
    {
        internal RecipientInformationStore recipientInfoStore;
        internal ContentInfo contentInfo;
        private AlgorithmIdentifier encAlg;
        private Asn1Set unprotectedAttributes;

        public CmsEnvelopedData( byte[] envelopedData )
          : this( CmsUtilities.ReadContentInfo( envelopedData ) )
        {
        }

        public CmsEnvelopedData( Stream envelopedData )
          : this( CmsUtilities.ReadContentInfo( envelopedData ) )
        {
        }

        public CmsEnvelopedData( ContentInfo contentInfo )
        {
            this.contentInfo = contentInfo;
            EnvelopedData instance = EnvelopedData.GetInstance( contentInfo.Content );
            Asn1Set recipientInfos = instance.RecipientInfos;
            EncryptedContentInfo encryptedContentInfo = instance.EncryptedContentInfo;
            this.encAlg = encryptedContentInfo.ContentEncryptionAlgorithm;
            CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsEnvelopedSecureReadable( this.encAlg, new CmsProcessableByteArray( encryptedContentInfo.EncryptedContent.GetOctets() ) );
            this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore( recipientInfos, secureReadable );
            this.unprotectedAttributes = instance.UnprotectedAttrs;
        }

        public AlgorithmIdentifier EncryptionAlgorithmID => this.encAlg;

        public string EncryptionAlgOid => this.encAlg.Algorithm.Id;

        public RecipientInformationStore GetRecipientInfos() => this.recipientInfoStore;

        public ContentInfo ContentInfo => this.contentInfo;

        public Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnprotectedAttributes() => this.unprotectedAttributes == null ? null : new Org.BouncyCastle.Asn1.Cms.AttributeTable( this.unprotectedAttributes );

        public byte[] GetEncoded() => this.contentInfo.GetEncoded();
    }
}
