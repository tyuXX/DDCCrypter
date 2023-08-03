// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.OtherSigningCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ess
{
    [Obsolete( "Use version in Asn1.Esf instead" )]
    public class OtherSigningCertificate : Asn1Encodable
    {
        private Asn1Sequence certs;
        private Asn1Sequence policies;

        public static OtherSigningCertificate GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case OtherSigningCertificate _:
                    return (OtherSigningCertificate)o;
                case Asn1Sequence _:
                    return new OtherSigningCertificate( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in 'OtherSigningCertificate' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        public OtherSigningCertificate( Asn1Sequence seq )
        {
            this.certs = seq.Count >= 1 && seq.Count <= 2 ? Asn1Sequence.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            if (seq.Count <= 1)
                return;
            this.policies = Asn1Sequence.GetInstance( seq[1] );
        }

        public OtherSigningCertificate( OtherCertID otherCertID ) => this.certs = new DerSequence( otherCertID );

        public OtherCertID[] GetCerts()
        {
            OtherCertID[] certs = new OtherCertID[this.certs.Count];
            for (int index = 0; index != this.certs.Count; ++index)
                certs[index] = OtherCertID.GetInstance( this.certs[index] );
            return certs;
        }

        public PolicyInformation[] GetPolicies()
        {
            if (this.policies == null)
                return null;
            PolicyInformation[] policies = new PolicyInformation[this.policies.Count];
            for (int index = 0; index != this.policies.Count; ++index)
                policies[index] = PolicyInformation.GetInstance( this.policies[index] );
            return policies;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         certs
            } );
            if (this.policies != null)
                v.Add( policies );
            return new DerSequence( v );
        }
    }
}
