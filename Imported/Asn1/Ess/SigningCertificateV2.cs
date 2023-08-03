// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.SigningCertificateV2
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ess
{
    public class SigningCertificateV2 : Asn1Encodable
    {
        private readonly Asn1Sequence certs;
        private readonly Asn1Sequence policies;

        public static SigningCertificateV2 GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case SigningCertificateV2 _:
                    return (SigningCertificateV2)o;
                case Asn1Sequence _:
                    return new SigningCertificateV2( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in 'SigningCertificateV2' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        private SigningCertificateV2( Asn1Sequence seq )
        {
            this.certs = seq.Count >= 1 && seq.Count <= 2 ? Asn1Sequence.GetInstance( seq[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.policies = Asn1Sequence.GetInstance( seq[1].ToAsn1Object() );
        }

        public SigningCertificateV2( EssCertIDv2 cert ) => this.certs = new DerSequence( cert );

        public SigningCertificateV2( EssCertIDv2[] certs ) => this.certs = new DerSequence( certs );

        public SigningCertificateV2( EssCertIDv2[] certs, PolicyInformation[] policies )
        {
            this.certs = new DerSequence( certs );
            if (policies == null)
                return;
            this.policies = new DerSequence( policies );
        }

        public EssCertIDv2[] GetCerts()
        {
            EssCertIDv2[] certs = new EssCertIDv2[this.certs.Count];
            for (int index = 0; index != this.certs.Count; ++index)
                certs[index] = EssCertIDv2.GetInstance( this.certs[index] );
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
