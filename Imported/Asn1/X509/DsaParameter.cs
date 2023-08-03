// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.DsaParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class DsaParameter : Asn1Encodable
    {
        internal readonly DerInteger p;
        internal readonly DerInteger q;
        internal readonly DerInteger g;

        public static DsaParameter GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static DsaParameter GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DsaParameter _:
                    return (DsaParameter)obj;
                case Asn1Sequence _:
                    return new DsaParameter( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid DsaParameter: " + Platform.GetTypeName( obj ) );
            }
        }

        public DsaParameter( BigInteger p, BigInteger q, BigInteger g )
        {
            this.p = new DerInteger( p );
            this.q = new DerInteger( q );
            this.g = new DerInteger( g );
        }

        private DsaParameter( Asn1Sequence seq )
        {
            this.p = seq.Count == 3 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.q = DerInteger.GetInstance( seq[1] );
            this.g = DerInteger.GetInstance( seq[2] );
        }

        public BigInteger P => this.p.PositiveValue;

        public BigInteger Q => this.q.PositiveValue;

        public BigInteger G => this.g.PositiveValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       p,
       q,
       g
        } );
    }
}
