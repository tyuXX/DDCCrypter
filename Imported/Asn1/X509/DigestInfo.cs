// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.DigestInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class DigestInfo : Asn1Encodable
    {
        private readonly byte[] digest;
        private readonly AlgorithmIdentifier algID;

        public static DigestInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static DigestInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case DigestInfo _:
                    return (DigestInfo)obj;
                case Asn1Sequence _:
                    return new DigestInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public DigestInfo( AlgorithmIdentifier algID, byte[] digest )
        {
            this.digest = digest;
            this.algID = algID;
        }

        private DigestInfo( Asn1Sequence seq )
        {
            this.algID = seq.Count == 2 ? AlgorithmIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.digest = Asn1OctetString.GetInstance( seq[1] ).GetOctets();
        }

        public AlgorithmIdentifier AlgorithmID => this.algID;

        public byte[] GetDigest() => this.digest;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       algID,
       new DerOctetString(this.digest)
        } );
    }
}
