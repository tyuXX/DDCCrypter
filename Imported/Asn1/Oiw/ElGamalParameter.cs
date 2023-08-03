// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Oiw.ElGamalParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.Oiw
{
    public class ElGamalParameter : Asn1Encodable
    {
        internal DerInteger p;
        internal DerInteger g;

        public ElGamalParameter( BigInteger p, BigInteger g )
        {
            this.p = new DerInteger( p );
            this.g = new DerInteger( g );
        }

        public ElGamalParameter( Asn1Sequence seq )
        {
            this.p = seq.Count == 2 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.g = DerInteger.GetInstance( seq[1] );
        }

        public BigInteger P => this.p.PositiveValue;

        public BigInteger G => this.g.PositiveValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       p,
       g
        } );
    }
}
