// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.CryptoPro.Gost3410ParamSetParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
    public class Gost3410ParamSetParameters : Asn1Encodable
    {
        private readonly int keySize;
        private readonly DerInteger p;
        private readonly DerInteger q;
        private readonly DerInteger a;

        public static Gost3410ParamSetParameters GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static Gost3410ParamSetParameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Gost3410ParamSetParameters _:
                    return (Gost3410ParamSetParameters)obj;
                case Asn1Sequence _:
                    return new Gost3410ParamSetParameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid GOST3410Parameter: " + Platform.GetTypeName( obj ) );
            }
        }

        public Gost3410ParamSetParameters( int keySize, BigInteger p, BigInteger q, BigInteger a )
        {
            this.keySize = keySize;
            this.p = new DerInteger( p );
            this.q = new DerInteger( q );
            this.a = new DerInteger( a );
        }

        private Gost3410ParamSetParameters( Asn1Sequence seq )
        {
            if (seq.Count != 4)
                throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.keySize = DerInteger.GetInstance( seq[0] ).Value.IntValue;
            this.p = DerInteger.GetInstance( seq[1] );
            this.q = DerInteger.GetInstance( seq[2] );
            this.a = DerInteger.GetInstance( seq[3] );
        }

        public int KeySize => this.keySize;

        public BigInteger P => this.p.PositiveValue;

        public BigInteger Q => this.q.PositiveValue;

        public BigInteger A => this.a.PositiveValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[4]
        {
       new DerInteger(this.keySize),
       p,
       q,
       a
        } );
    }
}
