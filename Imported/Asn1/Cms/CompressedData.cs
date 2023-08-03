// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.CompressedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class CompressedData : Asn1Encodable
    {
        private DerInteger version;
        private AlgorithmIdentifier compressionAlgorithm;
        private ContentInfo encapContentInfo;

        public CompressedData( AlgorithmIdentifier compressionAlgorithm, ContentInfo encapContentInfo )
        {
            this.version = new DerInteger( 0 );
            this.compressionAlgorithm = compressionAlgorithm;
            this.encapContentInfo = encapContentInfo;
        }

        public CompressedData( Asn1Sequence seq )
        {
            this.version = (DerInteger)seq[0];
            this.compressionAlgorithm = AlgorithmIdentifier.GetInstance( seq[1] );
            this.encapContentInfo = ContentInfo.GetInstance( seq[2] );
        }

        public static CompressedData GetInstance( Asn1TaggedObject ato, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( ato, explicitly ) );

        public static CompressedData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CompressedData _:
                    return (CompressedData)obj;
                case Asn1Sequence _:
                    return new CompressedData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid CompressedData: " + Platform.GetTypeName( obj ) );
            }
        }

        public DerInteger Version => this.version;

        public AlgorithmIdentifier CompressionAlgorithmIdentifier => this.compressionAlgorithm;

        public ContentInfo EncapContentInfo => this.encapContentInfo;

        public override Asn1Object ToAsn1Object() => new BerSequence( new Asn1Encodable[3]
        {
       version,
       compressionAlgorithm,
       encapContentInfo
        } );
    }
}
