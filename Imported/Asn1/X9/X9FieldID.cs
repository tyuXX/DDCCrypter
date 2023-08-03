// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9FieldID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.X9
{
    public class X9FieldID : Asn1Encodable
    {
        private readonly DerObjectIdentifier id;
        private readonly Asn1Object parameters;

        public X9FieldID( BigInteger primeP )
        {
            this.id = X9ObjectIdentifiers.PrimeField;
            this.parameters = new DerInteger( primeP );
        }

        public X9FieldID( int m, int k1 )
          : this( m, k1, 0, 0 )
        {
        }

        public X9FieldID( int m, int k1, int k2, int k3 )
        {
            this.id = X9ObjectIdentifiers.CharacteristicTwoField;
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         new DerInteger(m)
            } );
            if (k2 == 0)
            {
                if (k3 != 0)
                    throw new ArgumentException( "inconsistent k values" );
                v.Add( X9ObjectIdentifiers.TPBasis, new DerInteger( k1 ) );
            }
            else
            {
                if (k2 <= k1 || k3 <= k2)
                    throw new ArgumentException( "inconsistent k values" );
                v.Add( X9ObjectIdentifiers.PPBasis, new DerSequence( new Asn1Encodable[3]
                {
           new DerInteger(k1),
           new DerInteger(k2),
           new DerInteger(k3)
                } ) );
            }
            this.parameters = new DerSequence( v );
        }

        private X9FieldID( Asn1Sequence seq )
        {
            this.id = DerObjectIdentifier.GetInstance( seq[0] );
            this.parameters = seq[1].ToAsn1Object();
        }

        public static X9FieldID GetInstance( object obj )
        {
            if (obj is X9FieldID)
                return (X9FieldID)obj;
            return obj == null ? null : new X9FieldID( Asn1Sequence.GetInstance( obj ) );
        }

        public DerObjectIdentifier Identifier => this.id;

        public Asn1Object Parameters => this.parameters;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       id,
       parameters
        } );
    }
}
