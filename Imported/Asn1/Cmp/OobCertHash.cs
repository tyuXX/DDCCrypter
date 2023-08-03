// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.OobCertHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class OobCertHash : Asn1Encodable
    {
        private readonly AlgorithmIdentifier hashAlg;
        private readonly CertId certId;
        private readonly DerBitString hashVal;

        private OobCertHash( Asn1Sequence seq )
        {
            int num1 = seq.Count - 1;
            Asn1Sequence asn1Sequence = seq;
            int index1 = num1;
            int num2 = index1 - 1;
            this.hashVal = DerBitString.GetInstance( asn1Sequence[index1] );
            for (int index2 = num2; index2 >= 0; --index2)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index2];
                if (asn1TaggedObject.TagNo == 0)
                    this.hashAlg = AlgorithmIdentifier.GetInstance( asn1TaggedObject, true );
                else
                    this.certId = CertId.GetInstance( asn1TaggedObject, true );
            }
        }

        public static OobCertHash GetInstance( object obj )
        {
            switch (obj)
            {
                case OobCertHash _:
                    return (OobCertHash)obj;
                case Asn1Sequence _:
                    return new OobCertHash( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual AlgorithmIdentifier HashAlg => this.hashAlg;

        public virtual CertId CertID => this.certId;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            this.AddOptional( v, 0, hashAlg );
            this.AddOptional( v, 1, certId );
            v.Add( hashVal );
            return new DerSequence( v );
        }

        private void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( true, tagNo, obj ) );
        }
    }
}
