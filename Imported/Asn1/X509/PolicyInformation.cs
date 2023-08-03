// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.PolicyInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class PolicyInformation : Asn1Encodable
    {
        private readonly DerObjectIdentifier policyIdentifier;
        private readonly Asn1Sequence policyQualifiers;

        private PolicyInformation( Asn1Sequence seq )
        {
            this.policyIdentifier = seq.Count >= 1 && seq.Count <= 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            if (seq.Count <= 1)
                return;
            this.policyQualifiers = Asn1Sequence.GetInstance( seq[1] );
        }

        public PolicyInformation( DerObjectIdentifier policyIdentifier ) => this.policyIdentifier = policyIdentifier;

        public PolicyInformation( DerObjectIdentifier policyIdentifier, Asn1Sequence policyQualifiers )
        {
            this.policyIdentifier = policyIdentifier;
            this.policyQualifiers = policyQualifiers;
        }

        public static PolicyInformation GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case PolicyInformation _:
                    return (PolicyInformation)obj;
                default:
                    return new PolicyInformation( Asn1Sequence.GetInstance( obj ) );
            }
        }

        public DerObjectIdentifier PolicyIdentifier => this.policyIdentifier;

        public Asn1Sequence PolicyQualifiers => this.policyQualifiers;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         policyIdentifier
            } );
            if (this.policyQualifiers != null)
                v.Add( policyQualifiers );
            return new DerSequence( v );
        }
    }
}
