// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.EnvelopedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class EnvelopedDataParser
    {
        private Asn1SequenceParser _seq;
        private DerInteger _version;
        private IAsn1Convertible _nextObject;
        private bool _originatorInfoCalled;

        public EnvelopedDataParser( Asn1SequenceParser seq )
        {
            this._seq = seq;
            this._version = (DerInteger)seq.ReadObject();
        }

        public DerInteger Version => this._version;

        public OriginatorInfo GetOriginatorInfo()
        {
            this._originatorInfoCalled = true;
            if (this._nextObject == null)
                this._nextObject = this._seq.ReadObject();
            if (!(this._nextObject is Asn1TaggedObjectParser) || ((Asn1TaggedObjectParser)this._nextObject).TagNo != 0)
                return null;
            Asn1SequenceParser objectParser = (Asn1SequenceParser)((Asn1TaggedObjectParser)this._nextObject).GetObjectParser( 16, false );
            this._nextObject = null;
            return OriginatorInfo.GetInstance( objectParser.ToAsn1Object() );
        }

        public Asn1SetParser GetRecipientInfos()
        {
            if (!this._originatorInfoCalled)
                this.GetOriginatorInfo();
            if (this._nextObject == null)
                this._nextObject = this._seq.ReadObject();
            Asn1SetParser nextObject = (Asn1SetParser)this._nextObject;
            this._nextObject = null;
            return nextObject;
        }

        public EncryptedContentInfoParser GetEncryptedContentInfo()
        {
            if (this._nextObject == null)
                this._nextObject = this._seq.ReadObject();
            if (this._nextObject == null)
                return null;
            Asn1SequenceParser nextObject = (Asn1SequenceParser)this._nextObject;
            this._nextObject = null;
            return new EncryptedContentInfoParser( nextObject );
        }

        public Asn1SetParser GetUnprotectedAttrs()
        {
            if (this._nextObject == null)
                this._nextObject = this._seq.ReadObject();
            if (this._nextObject == null)
                return null;
            IAsn1Convertible nextObject = this._nextObject;
            this._nextObject = null;
            return (Asn1SetParser)((Asn1TaggedObjectParser)nextObject).GetObjectParser( 17, false );
        }
    }
}
