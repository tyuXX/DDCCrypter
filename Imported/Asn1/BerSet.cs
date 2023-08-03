// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerSet
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class BerSet : DerSet
    {
        public static readonly BerSet Empty = new();

        public static BerSet FromVector( Asn1EncodableVector v ) => v.Count >= 1 ? new BerSet( v ) : Empty;

        internal static BerSet FromVector( Asn1EncodableVector v, bool needsSorting ) => v.Count >= 1 ? new BerSet( v, needsSorting ) : Empty;

        public BerSet()
        {
        }

        public BerSet( Asn1Encodable obj )
          : base( obj )
        {
        }

        public BerSet( Asn1EncodableVector v )
          : base( v, false )
        {
        }

        internal BerSet( Asn1EncodableVector v, bool needsSorting )
          : base( v, needsSorting )
        {
        }

        internal override void Encode( DerOutputStream derOut )
        {
            switch (derOut)
            {
                case Asn1OutputStream _:
                case BerOutputStream _:
                    derOut.WriteByte( 49 );
                    derOut.WriteByte( 128 );
                    foreach (Asn1Encodable asn1Encodable in (Asn1Set)this)
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
