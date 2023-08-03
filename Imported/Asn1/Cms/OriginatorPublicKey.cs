// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.OriginatorPublicKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class OriginatorPublicKey : Asn1Encodable
    {
        private readonly AlgorithmIdentifier mAlgorithm;
        private readonly DerBitString mPublicKey;

        public OriginatorPublicKey( AlgorithmIdentifier algorithm, byte[] publicKey )
        {
            this.mAlgorithm = algorithm;
            this.mPublicKey = new DerBitString( publicKey );
        }

        [Obsolete( "Use 'GetInstance' instead" )]
        public OriginatorPublicKey( Asn1Sequence seq )
        {
            this.mAlgorithm = AlgorithmIdentifier.GetInstance( seq[0] );
            this.mPublicKey = DerBitString.GetInstance( seq[1] );
        }

        public static OriginatorPublicKey GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static OriginatorPublicKey GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OriginatorPublicKey _:
                    return (OriginatorPublicKey)obj;
                case Asn1Sequence _:
                    return new OriginatorPublicKey( Asn1Sequence.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "Invalid OriginatorPublicKey: " + Platform.GetTypeName( obj ) );
            }
        }

        public AlgorithmIdentifier Algorithm => this.mAlgorithm;

        public DerBitString PublicKey => this.mPublicKey;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       mAlgorithm,
       mPublicKey
        } );
    }
}
