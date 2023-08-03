// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.SigningCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ess
{
    public class SigningCertificate : Asn1Encodable
    {
        private Asn1Sequence certs;
        private Asn1Sequence policies;

        public static SigningCertificate GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case SigningCertificate _:
                    return (SigningCertificate)o;
                case Asn1Sequence _:
                    return new SigningCertificate( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in 'SigningCertificate' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        public SigningCertificate( Asn1Sequence seq )
        {
            this.certs = seq.Count >= 1 && seq.Count <= 2 ? Asn1Sequence.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            if (seq.Count <= 1)
                return;
            this.policies = Asn1Sequence.GetInstance( seq[1] );
        }

        public SigningCertificate( EssCertID essCertID ) => this.certs = new DerSequence( essCertID );

        public EssCertID[] GetCerts()
        {
            EssCertID[] certs = new EssCertID[this.certs.Count];
            for (int index = 0; index != this.certs.Count; ++index)
                certs[index] = EssCertID.GetInstance( this.certs[index] );
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
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         certs
            } );
            if (this.policies != null)
                v.Add( policies );
            return new DerSequence( v );
        }
    }
}
