// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerSequence
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class BerSequence : DerSequence
    {
        public static readonly BerSequence Empty = new BerSequence();

        public static BerSequence FromVector( Asn1EncodableVector v ) => v.Count >= 1 ? new BerSequence( v ) : Empty;

        public BerSequence()
        {
        }

        public BerSequence( Asn1Encodable obj )
          : base( obj )
        {
        }

        public BerSequence( params Asn1Encodable[] v )
          : base( v )
        {
        }

        public BerSequence( Asn1EncodableVector v )
          : base( v )
        {
        }

        internal override void Encode( DerOutputStream derOut )
        {
            switch (derOut)
            {
                case Asn1OutputStream _:
                case BerOutputStream _:
                    derOut.WriteByte( 48 );
                    derOut.WriteByte( 128 );
                    foreach (Asn1Encodable asn1Encodable in (Asn1Sequence)this)
                        derOut.WriteObject( asn1Encodable );
                    derOut.WriteByte( 0 );
                    derOut.WriteByte( 0 );
                    break;
                default:
                    base.Encode( derOut );
                    break;
            }
        }
    }
}
