// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerNull
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class DerNull : Asn1Null
    {
        public static readonly DerNull Instance = new( 0 );
        private byte[] zeroBytes = new byte[0];

        [Obsolete( "Use static Instance object" )]
        public DerNull()
        {
        }

        protected internal DerNull( int dummy )
        {
        }

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 5, this.zeroBytes );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerNull;

        protected override int Asn1GetHashCode() => -1;
    }
}
