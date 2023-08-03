// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsAuthenticatedDataParser
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
    public class CmsAuthenticatedDataParser : CmsContentInfoParser
    {
        internal RecipientInformationStore _recipientInfoStore;
        internal AuthenticatedDataParser authData;
        private AlgorithmIdentifier macAlg;
        private byte[] mac;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable authAttrs;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable unauthAttrs;
        private bool authAttrNotRead;
        private bool unauthAttrNotRead;

        public CmsAuthenticatedDataParser( byte[] envelopedData )
          : this( new MemoryStream( envelopedData, false ) )
        {
        }

        public CmsAuthenticatedDataParser( Stream envelopedData )
          : base( envelopedData )
        {
            this.authAttrNotRead = true;
            this.authData = new AuthenticatedDataParser( (Asn1SequenceParser)this.contentInfo.GetContent( 16 ) );
            Asn1Set instance = Asn1Set.GetInstance( this.authData.GetRecipientInfos().ToAsn1Object() );
            this.macAlg = this.authData.GetMacAlgorithm();
            CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable( this.macAlg, new CmsProcessableInputStream( ((Asn1OctetStringParser)this.authData.GetEnapsulatedContentInfo().GetContent( 4 )).GetOctetStream() ) );
            this._recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore( instance, secureReadable );
        }

        public AlgorithmIdentifier MacAlgorithmID => this.macAlg;

        public string MacAlgOid => this.macAlg.Algorithm.Id;

        public Asn1Object MacAlgParams => this.macAlg.Parameters?.ToAsn1Object();

        public RecipientInformationStore GetRecipientInfos() => this._recipientInfoStore;

        public byte[] GetMac()
        {
            if (this.mac == null)
            {
                this.GetAuthAttrs();
                this.mac = this.authData.GetMac().GetOctets();
            }
            return Arrays.Clone( this.mac );
        }

        public Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs()
        {
            if (this.authAttrs == null && this.authAttrNotRead)
            {
                Asn1SetParser authAttrs = this.authData.GetAuthAttrs();
                this.authAttrNotRead = false;
                if (authAttrs != null)
                {
                    Asn1EncodableVector v = new( new Asn1Encodable[0] );
                    IAsn1Convertible asn1Convertible;
                    while ((asn1Convertible = authAttrs.ReadObject()) != null)
                    {
                        Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
                        v.Add( asn1SequenceParser.ToAsn1Object() );
                    }
                    this.authAttrs = new Org.BouncyCastle.Asn1.Cms.AttributeTable( new DerSet( v ) );
                }
            }
            return this.authAttrs;
        }

        public Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs()
        {
            if (this.unauthAttrs == null && this.unauthAttrNotRead)
            {
                Asn1SetParser unauthAttrs = this.authData.GetUnauthAttrs();
                this.unauthAttrNotRead = false;
                if (unauthAttrs != null)
                {
                    Asn1EncodableVector v = new( new Asn1Encodable[0] );
                    IAsn1Convertible asn1Convertible;
                    while ((asn1Convertible = unauthAttrs.ReadObject()) != null)
                    {
                        Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
                        v.Add( asn1SequenceParser.ToAsn1Object() );
                    }
                    this.unauthAttrs = new Org.BouncyCastle.Asn1.Cms.AttributeTable( new DerSet( v ) );
                }
            }
            return this.unauthAttrs;
        }
    }
}
