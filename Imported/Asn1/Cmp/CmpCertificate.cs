// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CmpCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CmpCertificate : Asn1Encodable, IAsn1Choice
    {
        private readonly X509CertificateStructure x509v3PKCert;
        private readonly AttributeCertificate x509v2AttrCert;

        public CmpCertificate( AttributeCertificate x509v2AttrCert ) => this.x509v2AttrCert = x509v2AttrCert;

        public CmpCertificate( X509CertificateStructure x509v3PKCert ) => this.x509v3PKCert = x509v3PKCert.Version == 3 ? x509v3PKCert : throw new ArgumentException( "only version 3 certificates allowed", nameof( x509v3PKCert ) );

        public static CmpCertificate GetInstance( object obj )
        {
            switch (obj)
            {
                case CmpCertificate _:
                    return (CmpCertificate)obj;
                case Asn1Sequence _:
                    return new CmpCertificate( X509CertificateStructure.GetInstance( obj ) );
                case Asn1TaggedObject _:
                    return new CmpCertificate( AttributeCertificate.GetInstance( ((Asn1TaggedObject)obj).GetObject() ) );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual bool IsX509v3PKCert => this.x509v3PKCert != null;

        public virtual X509CertificateStructure X509v3PKCert => this.x509v3PKCert;

        public virtual AttributeCertificate X509v2AttrCert => this.x509v2AttrCert;

        public override Asn1Object ToAsn1Object() => this.x509v2AttrCert != null ? new DerTaggedObject( true, 1, x509v2AttrCert ) : this.x509v3PKCert.ToAsn1Object();
    }
}
