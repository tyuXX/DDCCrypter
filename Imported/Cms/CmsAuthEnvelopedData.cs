// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsAuthEnvelopedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    internal class CmsAuthEnvelopedData
    {
        internal RecipientInformationStore recipientInfoStore;
        internal ContentInfo contentInfo;
        private OriginatorInfo originator;
        private AlgorithmIdentifier authEncAlg;
        private Asn1Set authAttrs;
        private byte[] mac;
        private Asn1Set unauthAttrs;

        public CmsAuthEnvelopedData( byte[] authEnvData )
          : this( CmsUtilities.ReadContentInfo( authEnvData ) )
        {
        }

        public CmsAuthEnvelopedData( Stream authEnvData )
          : this( CmsUtilities.ReadContentInfo( authEnvData ) )
        {
        }

        public CmsAuthEnvelopedData( ContentInfo contentInfo )
        {
            this.contentInfo = contentInfo;
            AuthEnvelopedData instance = AuthEnvelopedData.GetInstance( contentInfo.Content );
            this.originator = instance.OriginatorInfo;
            Asn1Set recipientInfos = instance.RecipientInfos;
            this.authEncAlg = instance.AuthEncryptedContentInfo.ContentEncryptionAlgorithm;
            CmsSecureReadable secureReadable = new CmsAuthEnvelopedData.AuthEnvelopedSecureReadable( this );
            this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore( recipientInfos, secureReadable );
            this.authAttrs = instance.AuthAttrs;
            this.mac = instance.Mac.GetOctets();
            this.unauthAttrs = instance.UnauthAttrs;
        }

        private class AuthEnvelopedSecureReadable : CmsSecureReadable
        {
            private readonly CmsAuthEnvelopedData parent;

            internal AuthEnvelopedSecureReadable( CmsAuthEnvelopedData parent ) => this.parent = parent;

            public AlgorithmIdentifier Algorithm => this.parent.authEncAlg;

            public object CryptoObject => null;

            public CmsReadable GetReadable( KeyParameter key ) => throw new CmsException( "AuthEnveloped data decryption not yet implemented" );
        }
    }
}
