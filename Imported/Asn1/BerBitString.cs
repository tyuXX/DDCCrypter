// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerBitString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class BerBitString : DerBitString
    {
        public BerBitString( byte[] data, int padBits )
          : base( data, padBits )
        {
        }

        public BerBitString( byte[] data )
          : base( data )
        {
        }

        public BerBitString( int namedBits )
          : base( namedBits )
        {
        }

        public BerBitString( Asn1Encodable obj )
          : base( obj )
        {
        }

        internal override void Encode( DerOutputStream derOut )
        {
            switch (derOut)
            {
                case Asn1OutputStream _:
                case BerOutputStream _:
                    derOut.WriteEncoded( 3, (byte)this.mPadBits, this.mData );
                    break;
                default:
                    base.Encode( derOut );
                    break;
            }
        }
    }
}
