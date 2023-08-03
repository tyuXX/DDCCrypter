// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.CryptoPro.ECGost3410ParamSetParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
    public class ECGost3410ParamSetParameters : Asn1Encodable
    {
        internal readonly DerInteger p;
        internal readonly DerInteger q;
        internal readonly DerInteger a;
        internal readonly DerInteger b;
        internal readonly DerInteger x;
        internal readonly DerInteger y;

        public static ECGost3410ParamSetParameters GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static ECGost3410ParamSetParameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ECGost3410ParamSetParameters _:
                    return (ECGost3410ParamSetParameters)obj;
                case Asn1Sequence _:
                    return new ECGost3410ParamSetParameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid GOST3410Parameter: " + Platform.GetTypeName( obj ) );
            }
        }

        public ECGost3410ParamSetParameters(
          BigInteger a,
          BigInteger b,
          BigInteger p,
          BigInteger q,
          int x,
          BigInteger y )
        {
            this.a = new DerInteger( a );
            this.b = new DerInteger( b );
            this.p = new DerInteger( p );
            this.q = new DerInteger( q );
            this.x = new DerInteger( x );
            this.y = new DerInteger( y );
        }

        public ECGost3410ParamSetParameters( Asn1Sequence seq )
        {
            this.a = seq.Count == 6 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.b = DerInteger.GetInstance( seq[1] );
            this.p = DerInteger.GetInstance( seq[2] );
            this.q = DerInteger.GetInstance( seq[3] );
            this.x = DerInteger.GetInstance( seq[4] );
            this.y = DerInteger.GetInstance( seq[5] );
        }

        public BigInteger P => this.p.PositiveValue;

        public BigInteger Q => this.q.PositiveValue;

        public BigInteger A => this.a.PositiveValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[6]
        {
       a,
       b,
       p,
       q,
       x,
       y
        } );
    }
}
