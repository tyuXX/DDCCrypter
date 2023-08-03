// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.CompressedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class CompressedDataParser
    {
        private DerInteger _version;
        private AlgorithmIdentifier _compressionAlgorithm;
        private ContentInfoParser _encapContentInfo;

        public CompressedDataParser( Asn1SequenceParser seq )
        {
            this._version = (DerInteger)seq.ReadObject();
            this._compressionAlgorithm = AlgorithmIdentifier.GetInstance( seq.ReadObject().ToAsn1Object() );
            this._encapContentInfo = new ContentInfoParser( (Asn1SequenceParser)seq.ReadObject() );
        }

        public DerInteger Version => this._version;

        public AlgorithmIdentifier CompressionAlgorithmIdentifier => this._compressionAlgorithm;

        public ContentInfoParser GetEncapContentInfo() => this._encapContentInfo;
    }
}
