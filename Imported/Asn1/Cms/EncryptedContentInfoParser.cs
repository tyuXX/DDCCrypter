// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.EncryptedContentInfoParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class EncryptedContentInfoParser
    {
        private DerObjectIdentifier _contentType;
        private AlgorithmIdentifier _contentEncryptionAlgorithm;
        private Asn1TaggedObjectParser _encryptedContent;

        public EncryptedContentInfoParser( Asn1SequenceParser seq )
        {
            this._contentType = (DerObjectIdentifier)seq.ReadObject();
            this._contentEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( seq.ReadObject().ToAsn1Object() );
            this._encryptedContent = (Asn1TaggedObjectParser)seq.ReadObject();
        }

        public DerObjectIdentifier ContentType => this._contentType;

        public AlgorithmIdentifier ContentEncryptionAlgorithm => this._contentEncryptionAlgorithm;

        public IAsn1Convertible GetEncryptedContent( int tag ) => this._encryptedContent.GetObjectParser( tag, false );
    }
}
