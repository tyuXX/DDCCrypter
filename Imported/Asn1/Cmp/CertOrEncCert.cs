// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CertOrEncCert
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CertOrEncCert : Asn1Encodable, IAsn1Choice
    {
        private readonly CmpCertificate certificate;
        private readonly EncryptedValue encryptedCert;

        private CertOrEncCert( Asn1TaggedObject tagged )
        {
            if (tagged.TagNo == 0)
                this.certificate = CmpCertificate.GetInstance( tagged.GetObject() );
            else
                this.encryptedCert = tagged.TagNo == 1 ? EncryptedValue.GetInstance( tagged.GetObject() ) : throw new ArgumentException( "unknown tag: " + tagged.TagNo, nameof( tagged ) );
        }

        public static CertOrEncCert GetInstance( object obj )
        {
            switch (obj)
            {
                case CertOrEncCert _:
                    return (CertOrEncCert)obj;
                case Asn1TaggedObject _:
                    return new CertOrEncCert( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertOrEncCert( CmpCertificate certificate ) => this.certificate = certificate != null ? certificate : throw new ArgumentNullException( nameof( certificate ) );

        public CertOrEncCert( EncryptedValue encryptedCert ) => this.encryptedCert = encryptedCert != null ? encryptedCert : throw new ArgumentNullException( nameof( encryptedCert ) );

        public virtual CmpCertificate Certificate => this.certificate;

        public virtual EncryptedValue EncryptedCert => this.encryptedCert;

        public override Asn1Object ToAsn1Object() => this.certificate != null ? new DerTaggedObject( true, 0, certificate ) : (Asn1Object)new DerTaggedObject( true, 1, encryptedCert );
    }
}
