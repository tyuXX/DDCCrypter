// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.IetfAttrSyntax
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class IetfAttrSyntax : Asn1Encodable
    {
        public const int ValueOctets = 1;
        public const int ValueOid = 2;
        public const int ValueUtf8 = 3;
        internal readonly GeneralNames policyAuthority;
        internal readonly Asn1EncodableVector values = new( new Asn1Encodable[0] );
        internal int valueChoice = -1;

        public IetfAttrSyntax( Asn1Sequence seq )
        {
            int index = 0;
            if (seq[0] is Asn1TaggedObject)
            {
                this.policyAuthority = GeneralNames.GetInstance( (Asn1TaggedObject)seq[0], false );
                ++index;
            }
            else if (seq.Count == 2)
            {
                this.policyAuthority = GeneralNames.GetInstance( seq[0] );
                ++index;
            }
            seq = seq[index] is Asn1Sequence ? (Asn1Sequence)seq[index] : throw new ArgumentException( "Non-IetfAttrSyntax encoding" );
            foreach (Asn1Object asn1Object in seq)
            {
                int num;
                switch (asn1Object)
                {
                    case DerObjectIdentifier _:
                        num = 2;
                        break;
                    case DerUtf8String _:
                        num = 3;
                        break;
                    case DerOctetString _:
                        num = 1;
                        break;
                    default:
                        throw new ArgumentException( "Bad value type encoding IetfAttrSyntax" );
                }
                if (this.valueChoice < 0)
                    this.valueChoice = num;
                if (num != this.valueChoice)
                    throw new ArgumentException( "Mix of value types in IetfAttrSyntax" );
                this.values.Add( asn1Object );
            }
        }

        public GeneralNames PolicyAuthority => this.policyAuthority;

        public int ValueType => this.valueChoice;

        public object[] GetValues()
        {
            if (this.ValueType == 1)
            {
                Asn1OctetString[] values = new Asn1OctetString[this.values.Count];
                for (int index = 0; index != values.Length; ++index)
                    values[index] = (Asn1OctetString)this.values[index];
                return values;
            }
            if (this.ValueType == 2)
            {
                DerObjectIdentifier[] values = new DerObjectIdentifier[this.values.Count];
                for (int index = 0; index != values.Length; ++index)
                    values[index] = (DerObjectIdentifier)this.values[index];
                return values;
            }
            DerUtf8String[] values1 = new DerUtf8String[this.values.Count];
            for (int index = 0; index != values1.Length; ++index)
                values1[index] = (DerUtf8String)this.values[index];
            return values1;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.policyAuthority != null)
                v.Add( new DerTaggedObject( 0, policyAuthority ) );
            v.Add( new DerSequence( this.values ) );
            return new DerSequence( v );
        }
    }
}
