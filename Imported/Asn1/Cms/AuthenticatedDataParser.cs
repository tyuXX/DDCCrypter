// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.AuthenticatedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class AuthenticatedDataParser
    {
        private Asn1SequenceParser seq;
        private DerInteger version;
        private IAsn1Convertible nextObject;
        private bool originatorInfoCalled;

        public AuthenticatedDataParser( Asn1SequenceParser seq )
        {
            this.seq = seq;
            this.version = (DerInteger)seq.ReadObject();
        }

        public DerInteger Version => this.version;

        public OriginatorInfo GetOriginatorInfo()
        {
            this.originatorInfoCalled = true;
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            if (!(this.nextObject is Asn1TaggedObjectParser) || ((Asn1TaggedObjectParser)this.nextObject).TagNo != 0)
                return null;
            Asn1SequenceParser objectParser = (Asn1SequenceParser)((Asn1TaggedObjectParser)this.nextObject).GetObjectParser( 16, false );
            this.nextObject = null;
            return OriginatorInfo.GetInstance( objectParser.ToAsn1Object() );
        }

        public Asn1SetParser GetRecipientInfos()
        {
            if (!this.originatorInfoCalled)
                this.GetOriginatorInfo();
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            Asn1SetParser nextObject = (Asn1SetParser)this.nextObject;
            this.nextObject = null;
            return nextObject;
        }

        public AlgorithmIdentifier GetMacAlgorithm()
        {
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            if (this.nextObject == null)
                return null;
            Asn1SequenceParser nextObject = (Asn1SequenceParser)this.nextObject;
            this.nextObject = null;
            return AlgorithmIdentifier.GetInstance( nextObject.ToAsn1Object() );
        }

        public AlgorithmIdentifier GetDigestAlgorithm()
        {
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            if (!(this.nextObject is Asn1TaggedObjectParser))
                return null;
            AlgorithmIdentifier instance = AlgorithmIdentifier.GetInstance( (Asn1TaggedObject)this.nextObject.ToAsn1Object(), false );
            this.nextObject = null;
            return instance;
        }

        public ContentInfoParser GetEnapsulatedContentInfo()
        {
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            if (this.nextObject == null)
                return null;
            Asn1SequenceParser nextObject = (Asn1SequenceParser)this.nextObject;
            this.nextObject = null;
            return new ContentInfoParser( nextObject );
        }

        public Asn1SetParser GetAuthAttrs()
        {
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            if (!(this.nextObject is Asn1TaggedObjectParser))
                return null;
            IAsn1Convertible nextObject = this.nextObject;
            this.nextObject = null;
            return (Asn1SetParser)((Asn1TaggedObjectParser)nextObject).GetObjectParser( 17, false );
        }

        public Asn1OctetString GetMac()
        {
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            IAsn1Convertible nextObject = this.nextObject;
            this.nextObject = null;
            return Asn1OctetString.GetInstance( nextObject.ToAsn1Object() );
        }

        public Asn1SetParser GetUnauthAttrs()
        {
            if (this.nextObject == null)
                this.nextObject = this.seq.ReadObject();
            if (this.nextObject == null)
                return null;
            IAsn1Convertible nextObject = this.nextObject;
            this.nextObject = null;
            return (Asn1SetParser)((Asn1TaggedObjectParser)nextObject).GetObjectParser( 17, false );
        }
    }
}
