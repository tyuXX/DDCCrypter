﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsEnvelopedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsEnvelopedDataParser : CmsContentInfoParser
    {
        internal RecipientInformationStore recipientInfoStore;
        internal EnvelopedDataParser envelopedData;
        private AlgorithmIdentifier _encAlg;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable _unprotectedAttributes;
        private bool _attrNotRead;

        public CmsEnvelopedDataParser( byte[] envelopedData )
          : this( new MemoryStream( envelopedData, false ) )
        {
        }

        public CmsEnvelopedDataParser( Stream envelopedData )
          : base( envelopedData )
        {
            this._attrNotRead = true;
            this.envelopedData = new EnvelopedDataParser( (Asn1SequenceParser)this.contentInfo.GetContent( 16 ) );
            Asn1Set instance = Asn1Set.GetInstance( this.envelopedData.GetRecipientInfos().ToAsn1Object() );
            EncryptedContentInfoParser encryptedContentInfo = this.envelopedData.GetEncryptedContentInfo();
            this._encAlg = encryptedContentInfo.ContentEncryptionAlgorithm;
            CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsEnvelopedSecureReadable( this._encAlg, new CmsProcessableInputStream( ((Asn1OctetStringParser)encryptedContentInfo.GetEncryptedContent( 4 )).GetOctetStream() ) );
            this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore( instance, secureReadable );
        }

        public AlgorithmIdentifier EncryptionAlgorithmID => this._encAlg;

        public string EncryptionAlgOid => this._encAlg.Algorithm.Id;

        public Asn1Object EncryptionAlgParams => this._encAlg.Parameters?.ToAsn1Object();

        public RecipientInformationStore GetRecipientInfos() => this.recipientInfoStore;

        public Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnprotectedAttributes()
        {
            if (this._unprotectedAttributes == null && this._attrNotRead)
            {
                Asn1SetParser unprotectedAttrs = this.envelopedData.GetUnprotectedAttrs();
                this._attrNotRead = false;
                if (unprotectedAttrs != null)
                {
                    Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
                    IAsn1Convertible asn1Convertible;
                    while ((asn1Convertible = unprotectedAttrs.ReadObject()) != null)
                    {
                        Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
                        v.Add( asn1SequenceParser.ToAsn1Object() );
                    }
                    this._unprotectedAttributes = new Org.BouncyCastle.Asn1.Cms.AttributeTable( new DerSet( v ) );
                }
            }
            return this._unprotectedAttributes;
        }
    }
}
