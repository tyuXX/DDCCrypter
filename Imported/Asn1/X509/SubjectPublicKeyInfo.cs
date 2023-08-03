// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class SubjectPublicKeyInfo : Asn1Encodable
    {
        private readonly AlgorithmIdentifier algID;
        private readonly DerBitString keyData;

        public static SubjectPublicKeyInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static SubjectPublicKeyInfo GetInstance( object obj )
        {
            if (obj is SubjectPublicKeyInfo)
                return (SubjectPublicKeyInfo)obj;
            return obj != null ? new SubjectPublicKeyInfo( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public SubjectPublicKeyInfo( AlgorithmIdentifier algID, Asn1Encodable publicKey )
        {
            this.keyData = new DerBitString( publicKey );
            this.algID = algID;
        }

        public SubjectPublicKeyInfo( AlgorithmIdentifier algID, byte[] publicKey )
        {
            this.keyData = new DerBitString( publicKey );
            this.algID = algID;
        }

        private SubjectPublicKeyInfo( Asn1Sequence seq )
        {
            this.algID = seq.Count == 2 ? AlgorithmIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.keyData = DerBitString.GetInstance( seq[1] );
        }

        public AlgorithmIdentifier AlgorithmID => this.algID;

        public Asn1Object GetPublicKey() => Asn1Object.FromByteArray( this.keyData.GetOctets() );

        public DerBitString PublicKeyData => this.keyData;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       algID,
       keyData
        } );
    }
}
