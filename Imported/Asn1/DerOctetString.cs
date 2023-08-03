// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerOctetString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class DerOctetString : Asn1OctetString
    {
        public DerOctetString( byte[] str )
          : base( str )
        {
        }

        public DerOctetString( Asn1Encodable obj )
          : base( obj )
        {
        }

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 4, this.str );

        internal static void Encode( DerOutputStream derOut, byte[] bytes, int offset, int length ) => derOut.WriteEncoded( 4, bytes, offset, length );
    }
}
