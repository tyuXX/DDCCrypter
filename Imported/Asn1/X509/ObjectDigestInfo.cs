// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.ObjectDigestInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class ObjectDigestInfo : Asn1Encodable
    {
        public const int PublicKey = 0;
        public const int PublicKeyCert = 1;
        public const int OtherObjectDigest = 2;
        internal readonly DerEnumerated digestedObjectType;
        internal readonly DerObjectIdentifier otherObjectTypeID;
        internal readonly AlgorithmIdentifier digestAlgorithm;
        internal readonly DerBitString objectDigest;

        public static ObjectDigestInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ObjectDigestInfo _:
                    return (ObjectDigestInfo)obj;
                case Asn1Sequence _:
                    return new ObjectDigestInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static ObjectDigestInfo GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public ObjectDigestInfo(
          int digestedObjectType,
          string otherObjectTypeID,
          AlgorithmIdentifier digestAlgorithm,
          byte[] objectDigest )
        {
            this.digestedObjectType = new DerEnumerated( digestedObjectType );
            if (digestedObjectType == 2)
                this.otherObjectTypeID = new DerObjectIdentifier( otherObjectTypeID );
            this.digestAlgorithm = digestAlgorithm;
            this.objectDigest = new DerBitString( objectDigest );
        }

        private ObjectDigestInfo( Asn1Sequence seq )
        {
            this.digestedObjectType = seq.Count <= 4 && seq.Count >= 3 ? DerEnumerated.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            int num = 0;
            if (seq.Count == 4)
            {
                this.otherObjectTypeID = DerObjectIdentifier.GetInstance( seq[1] );
                ++num;
            }
            this.digestAlgorithm = AlgorithmIdentifier.GetInstance( seq[1 + num] );
            this.objectDigest = DerBitString.GetInstance( seq[2 + num] );
        }

        public DerEnumerated DigestedObjectType => this.digestedObjectType;

        public DerObjectIdentifier OtherObjectTypeID => this.otherObjectTypeID;

        public AlgorithmIdentifier DigestAlgorithm => this.digestAlgorithm;

        public DerBitString ObjectDigest => this.objectDigest;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         digestedObjectType
            } );
            if (this.otherObjectTypeID != null)
                v.Add( otherObjectTypeID );
            v.Add( digestAlgorithm, objectDigest );
            return new DerSequence( v );
        }
    }
}
