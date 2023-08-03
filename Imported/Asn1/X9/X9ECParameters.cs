// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9ECParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Asn1.X9
{
    public class X9ECParameters : Asn1Encodable
    {
        private X9FieldID fieldID;
        private ECCurve curve;
        private X9ECPoint g;
        private BigInteger n;
        private BigInteger h;
        private byte[] seed;

        public static X9ECParameters GetInstance( object obj )
        {
            if (obj is X9ECParameters)
                return (X9ECParameters)obj;
            return obj != null ? new X9ECParameters( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public X9ECParameters( Asn1Sequence seq )
        {
            if (!(seq[0] is DerInteger) || !((DerInteger)seq[0]).Value.Equals( BigInteger.One ))
                throw new ArgumentException( "bad version in X9ECParameters" );
            X9Curve x9Curve = new( X9FieldID.GetInstance( seq[1] ), Asn1Sequence.GetInstance( seq[2] ) );
            this.curve = x9Curve.Curve;
            object s = seq[3];
            this.g = !(s is X9ECPoint) ? new X9ECPoint( this.curve, (Asn1OctetString)s ) : (X9ECPoint)s;
            this.n = ((DerInteger)seq[4]).Value;
            this.seed = x9Curve.GetSeed();
            if (seq.Count != 6)
                return;
            this.h = ((DerInteger)seq[5]).Value;
        }

        public X9ECParameters( ECCurve curve, ECPoint g, BigInteger n )
          : this( curve, g, n, null, null )
        {
        }

        public X9ECParameters( ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h )
          : this( curve, g, n, h, null )
        {
        }

        public X9ECParameters( ECCurve curve, ECPoint g, BigInteger n, BigInteger h )
          : this( curve, g, n, h, null )
        {
        }

        public X9ECParameters( ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed )
          : this( curve, new X9ECPoint( g ), n, h, seed )
        {
        }

        public X9ECParameters( ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h, byte[] seed )
        {
            this.curve = curve;
            this.g = g;
            this.n = n;
            this.h = h;
            this.seed = seed;
            if (ECAlgorithms.IsFpCurve( curve ))
            {
                this.fieldID = new X9FieldID( curve.Field.Characteristic );
            }
            else
            {
                if (!ECAlgorithms.IsF2mCurve( curve ))
                    throw new ArgumentException( "'curve' is of an unsupported type" );
                int[] exponentsPresent = ((IPolynomialExtensionField)curve.Field).MinimalPolynomial.GetExponentsPresent();
                if (exponentsPresent.Length == 3)
                    this.fieldID = new X9FieldID( exponentsPresent[2], exponentsPresent[1] );
                else
                    this.fieldID = exponentsPresent.Length == 5 ? new X9FieldID( exponentsPresent[4], exponentsPresent[1], exponentsPresent[2], exponentsPresent[3] ) : throw new ArgumentException( "Only trinomial and pentomial curves are supported" );
            }
        }

        public ECCurve Curve => this.curve;

        public ECPoint G => this.g.Point;

        public BigInteger N => this.n;

        public BigInteger H => this.h;

        public byte[] GetSeed() => this.seed;

        public X9Curve CurveEntry => new( this.curve, this.seed );

        public X9FieldID FieldIDEntry => this.fieldID;

        public X9ECPoint BaseEntry => this.g;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[5]
            {
         new DerInteger(BigInteger.One),
         fieldID,
         new X9Curve(this.curve, this.seed),
         g,
         new DerInteger(this.n)
            } );
            if (this.h != null)
                v.Add( new DerInteger( this.h ) );
            return new DerSequence( v );
        }
    }
}
