// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathValidatorResult
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCertPathValidatorResult
    {
        private TrustAnchor trustAnchor;
        private PkixPolicyNode policyTree;
        private AsymmetricKeyParameter subjectPublicKey;

        public PkixPolicyNode PolicyTree => this.policyTree;

        public TrustAnchor TrustAnchor => this.trustAnchor;

        public AsymmetricKeyParameter SubjectPublicKey => this.subjectPublicKey;

        public PkixCertPathValidatorResult(
          TrustAnchor trustAnchor,
          PkixPolicyNode policyTree,
          AsymmetricKeyParameter subjectPublicKey )
        {
            if (subjectPublicKey == null)
                throw new NullReferenceException( "subjectPublicKey must be non-null" );
            this.trustAnchor = trustAnchor != null ? trustAnchor : throw new NullReferenceException( "trustAnchor must be non-null" );
            this.policyTree = policyTree;
            this.subjectPublicKey = subjectPublicKey;
        }

        public object Clone() => new PkixCertPathValidatorResult( this.TrustAnchor, this.PolicyTree, this.SubjectPublicKey );

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append( "PKIXCertPathValidatorResult: [ \n" );
            stringBuilder.Append( "  Trust Anchor: " ).Append( TrustAnchor ).Append( '\n' );
            stringBuilder.Append( "  Policy Tree: " ).Append( PolicyTree ).Append( '\n' );
            stringBuilder.Append( "  Subject Public Key: " ).Append( SubjectPublicKey ).Append( "\n]" );
            return stringBuilder.ToString();
        }
    }
}
