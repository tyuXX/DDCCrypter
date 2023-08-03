// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9FieldElement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Asn1.X9
{
    public class X9FieldElement : Asn1Encodable
    {
        private ECFieldElement f;

        public X9FieldElement( ECFieldElement f ) => this.f = f;

        public X9FieldElement( BigInteger p, Asn1OctetString s )
          : this( new FpFieldElement( p, new BigInteger( 1, s.GetOctets() ) ) )
        {
        }

        public X9FieldElement( int m, int k1, int k2, int k3, Asn1OctetString s )
          : this( new F2mFieldElement( m, k1, k2, k3, new BigInteger( 1, s.GetOctets() ) ) )
        {
        }

        public ECFieldElement Value => this.f;

        public override Asn1Object ToAsn1Object()
        {
            int byteLength = X9IntegerConverter.GetByteLength( this.f );
            return new DerOctetString( X9IntegerConverter.IntegerToBytes( this.f.ToBigInteger(), byteLength ) );
        }
    }
}
