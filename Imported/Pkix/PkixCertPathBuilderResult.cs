// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathBuilderResult
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCertPathBuilderResult : PkixCertPathValidatorResult
    {
        private PkixCertPath certPath;

        public PkixCertPathBuilderResult(
          PkixCertPath certPath,
          TrustAnchor trustAnchor,
          PkixPolicyNode policyTree,
          AsymmetricKeyParameter subjectPublicKey )
          : base( trustAnchor, policyTree, subjectPublicKey )
        {
            this.certPath = certPath != null ? certPath : throw new ArgumentNullException( nameof( certPath ) );
        }

        public PkixCertPath CertPath => this.certPath;

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append( "SimplePKIXCertPathBuilderResult: [\n" );
            stringBuilder.Append( "  Certification Path: " ).Append( CertPath ).Append( '\n' );
            stringBuilder.Append( "  Trust Anchor: " ).Append( this.TrustAnchor.TrustedCert.IssuerDN.ToString() ).Append( '\n' );
            stringBuilder.Append( "  Subject Public Key: " ).Append( SubjectPublicKey ).Append( "\n]" );
            return stringBuilder.ToString();
        }
    }
}
