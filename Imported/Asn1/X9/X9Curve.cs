// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
    public class X9Curve : Asn1Encodable
    {
        private readonly ECCurve curve;
        private readonly byte[] seed;
        private readonly DerObjectIdentifier fieldIdentifier;

        public X9Curve( ECCurve curve )
          : this( curve, null )
        {
        }

        public X9Curve( ECCurve curve, byte[] seed )
        {
            this.curve = curve != null ? curve : throw new ArgumentNullException( nameof( curve ) );
            this.seed = Arrays.Clone( seed );
            if (ECAlgorithms.IsFpCurve( curve ))
            {
                this.fieldIdentifier = X9ObjectIdentifiers.PrimeField;
            }
            else
            {
                if (!ECAlgorithms.IsF2mCurve( curve ))
                    throw new ArgumentException( "This type of ECCurve is not implemented" );
                this.fieldIdentifier = X9ObjectIdentifiers.CharacteristicTwoField;
            }
        }

        public X9Curve( X9FieldID fieldID, Asn1Sequence seq )
        {
            if (fieldID == null)
                throw new ArgumentNullException( nameof( fieldID ) );
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.fieldIdentifier = fieldID.Identifier;
            if (this.fieldIdentifier.Equals( X9ObjectIdentifiers.PrimeField ))
            {
                BigInteger bigInteger = ((DerInteger)fieldID.Parameters).Value;
                X9FieldElement x9FieldElement1 = new( bigInteger, (Asn1OctetString)seq[0] );
                X9FieldElement x9FieldElement2 = new( bigInteger, (Asn1OctetString)seq[1] );
                this.curve = new FpCurve( bigInteger, x9FieldElement1.Value.ToBigInteger(), x9FieldElement2.Value.ToBigInteger() );
            }
            else if (this.fieldIdentifier.Equals( X9ObjectIdentifiers.CharacteristicTwoField ))
            {
                DerSequence parameters = (DerSequence)fieldID.Parameters;
                int intValue1 = ((DerInteger)parameters[0]).Value.IntValue;
                DerObjectIdentifier objectIdentifier = (DerObjectIdentifier)parameters[1];
                int k2 = 0;
                int k3 = 0;
                int intValue2;
                if (objectIdentifier.Equals( X9ObjectIdentifiers.TPBasis ))
                {
                    intValue2 = ((DerInteger)parameters[2]).Value.IntValue;
                }
                else
                {
                    DerSequence derSequence = (DerSequence)parameters[2];
                    intValue2 = ((DerInteger)derSequence[0]).Value.IntValue;
                    k2 = ((DerInteger)derSequence[1]).Value.IntValue;
                    k3 = ((DerInteger)derSequence[2]).Value.IntValue;
                }
                X9FieldElement x9FieldElement3 = new( intValue1, intValue2, k2, k3, (Asn1OctetString)seq[0] );
                X9FieldElement x9FieldElement4 = new( intValue1, intValue2, k2, k3, (Asn1OctetString)seq[1] );
                this.curve = new F2mCurve( intValue1, intValue2, k2, k3, x9FieldElement3.Value.ToBigInteger(), x9FieldElement4.Value.ToBigInteger() );
            }
            if (seq.Count != 3)
                return;
            this.seed = ((DerBitString)seq[2]).GetBytes();
        }

        public ECCurve Curve => this.curve;

        public byte[] GetSeed() => Arrays.Clone( this.seed );

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.fieldIdentifier.Equals( X9ObjectIdentifiers.PrimeField ) || this.fieldIdentifier.Equals( X9ObjectIdentifiers.CharacteristicTwoField ))
            {
                v.Add( new X9FieldElement( this.curve.A ).ToAsn1Object() );
                v.Add( new X9FieldElement( this.curve.B ).ToAsn1Object() );
            }
            if (this.seed != null)
                v.Add( new DerBitString( this.seed ) );
            return new DerSequence( v );
        }
    }
}
