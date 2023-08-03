// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerNull
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class BerNull : DerNull
    {
        public static readonly BerNull Instance = new( 0 );

        [Obsolete( "Use static Instance object" )]
        public BerNull()
        {
        }

        private BerNull( int dummy )
          : base( dummy )
        {
        }

        internal override void Encode( DerOutputStream derOut )
        {
            switch (derOut)
            {
                case Asn1OutputStream _:
                case BerOutputStream _:
                    derOut.WriteByte( 5 );
                    break;
                default:
                    base.Encode( derOut );
                    break;
            }
        }
    }
}
