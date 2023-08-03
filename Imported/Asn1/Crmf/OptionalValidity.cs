// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.OptionalValidity
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class OptionalValidity : Asn1Encodable
    {
        private readonly Time notBefore;
        private readonly Time notAfter;

        private OptionalValidity( Asn1Sequence seq )
        {
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                if (asn1TaggedObject.TagNo == 0)
                    this.notBefore = Time.GetInstance( asn1TaggedObject, true );
                else
                    this.notAfter = Time.GetInstance( asn1TaggedObject, true );
            }
        }

        public static OptionalValidity GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OptionalValidity _:
                    return (OptionalValidity)obj;
                default:
                    return new OptionalValidity( Asn1Sequence.GetInstance( obj ) );
            }
        }

        public virtual Time NotBefore => this.notBefore;

        public virtual Time NotAfter => this.notAfter;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.notBefore != null)
                v.Add( new DerTaggedObject( true, 0, notBefore ) );
            if (this.notAfter != null)
                v.Add( new DerTaggedObject( true, 1, notAfter ) );
            return new DerSequence( v );
        }
    }
}
