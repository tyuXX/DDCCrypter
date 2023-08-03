// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CertificatePolicies
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class CertificatePolicies : Asn1Encodable
    {
        private readonly PolicyInformation[] policyInformation;

        public static CertificatePolicies GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CertificatePolicies _:
                    return (CertificatePolicies)obj;
                default:
                    return new CertificatePolicies( Asn1Sequence.GetInstance( obj ) );
            }
        }

        public static CertificatePolicies GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public CertificatePolicies( PolicyInformation name ) => this.policyInformation = new PolicyInformation[1]
        {
      name
        };

        public CertificatePolicies( PolicyInformation[] policyInformation ) => this.policyInformation = policyInformation;

        private CertificatePolicies( Asn1Sequence seq )
        {
            this.policyInformation = new PolicyInformation[seq.Count];
            for (int index = 0; index < seq.Count; ++index)
                this.policyInformation[index] = PolicyInformation.GetInstance( seq[index] );
        }

        public virtual PolicyInformation[] GetPolicyInformation() => (PolicyInformation[])this.policyInformation.Clone();

        public override Asn1Object ToAsn1Object() => new DerSequence( policyInformation );

        public override string ToString()
        {
            StringBuilder stringBuilder = new( "CertificatePolicies:" );
            if (this.policyInformation != null && this.policyInformation.Length > 0)
            {
                stringBuilder.Append( ' ' );
                stringBuilder.Append( this.policyInformation[0] );
                for (int index = 1; index < this.policyInformation.Length; ++index)
                {
                    stringBuilder.Append( ", " );
                    stringBuilder.Append( this.policyInformation[index] );
                }
            }
            return stringBuilder.ToString();
        }
    }
}
