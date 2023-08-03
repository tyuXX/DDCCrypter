// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.PolicyQualifierInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class PolicyQualifierInfo : Asn1Encodable
    {
        private readonly DerObjectIdentifier policyQualifierId;
        private readonly Asn1Encodable qualifier;

        public PolicyQualifierInfo( DerObjectIdentifier policyQualifierId, Asn1Encodable qualifier )
        {
            this.policyQualifierId = policyQualifierId;
            this.qualifier = qualifier;
        }

        public PolicyQualifierInfo( string cps )
        {
            this.policyQualifierId = PolicyQualifierID.IdQtCps;
            this.qualifier = new DerIA5String( cps );
        }

        private PolicyQualifierInfo( Asn1Sequence seq )
        {
            this.policyQualifierId = seq.Count == 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.qualifier = seq[1];
        }

        public static PolicyQualifierInfo GetInstance( object obj )
        {
            if (obj is PolicyQualifierInfo)
                return (PolicyQualifierInfo)obj;
            return obj == null ? null : new PolicyQualifierInfo( Asn1Sequence.GetInstance( obj ) );
        }

        public virtual DerObjectIdentifier PolicyQualifierId => this.policyQualifierId;

        public virtual Asn1Encodable Qualifier => this.qualifier;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       policyQualifierId,
      this.qualifier
        } );
    }
}
