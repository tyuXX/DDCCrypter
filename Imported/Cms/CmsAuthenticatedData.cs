// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsAuthenticatedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsAuthenticatedData
    {
        internal RecipientInformationStore recipientInfoStore;
        internal ContentInfo contentInfo;
        private AlgorithmIdentifier macAlg;
        private Asn1Set authAttrs;
        private Asn1Set unauthAttrs;
        private byte[] mac;

        public CmsAuthenticatedData( byte[] authData )
          : this( CmsUtilities.ReadContentInfo( authData ) )
        {
        }

        public CmsAuthenticatedData( Stream authData )
          : this( CmsUtilities.ReadContentInfo( authData ) )
        {
        }

        public CmsAuthenticatedData( ContentInfo contentInfo )
        {
            this.contentInfo = contentInfo;
            AuthenticatedData instance = AuthenticatedData.GetInstance( contentInfo.Content );
            Asn1Set recipientInfos = instance.RecipientInfos;
            this.macAlg = instance.MacAlgorithm;
            CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable( this.macAlg, new CmsProcessableByteArray( Asn1OctetString.GetInstance( instance.EncapsulatedContentInfo.Content ).GetOctets() ) );
            this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore( recipientInfos, secureReadable );
            this.authAttrs = instance.AuthAttrs;
            this.mac = instance.Mac.GetOctets();
            this.unauthAttrs = instance.UnauthAttrs;
        }

        public byte[] GetMac() => Arrays.Clone( this.mac );

        public AlgorithmIdentifier MacAlgorithmID => this.macAlg;

        public string MacAlgOid => this.macAlg.Algorithm.Id;

        public RecipientInformationStore GetRecipientInfos() => this.recipientInfoStore;

        public ContentInfo ContentInfo => this.contentInfo;

        public Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs() => this.authAttrs == null ? null : new Org.BouncyCastle.Asn1.Cms.AttributeTable( this.authAttrs );

        public Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs() => this.unauthAttrs == null ? null : new Org.BouncyCastle.Asn1.Cms.AttributeTable( this.unauthAttrs );

        public byte[] GetEncoded() => this.contentInfo.GetEncoded();
    }
}
