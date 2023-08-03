// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.PrincipalUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;

namespace Org.BouncyCastle.X509
{
    public class PrincipalUtilities
    {
        public static X509Name GetIssuerX509Principal( X509Certificate cert )
        {
            try
            {
                return TbsCertificateStructure.GetInstance( Asn1Object.FromByteArray( cert.GetTbsCertificate() ) ).Issuer;
            }
            catch (Exception ex)
            {
                throw new CertificateEncodingException( "Could not extract issuer", ex );
            }
        }

        public static X509Name GetSubjectX509Principal( X509Certificate cert )
        {
            try
            {
                return TbsCertificateStructure.GetInstance( Asn1Object.FromByteArray( cert.GetTbsCertificate() ) ).Subject;
            }
            catch (Exception ex)
            {
                throw new CertificateEncodingException( "Could not extract subject", ex );
            }
        }

        public static X509Name GetIssuerX509Principal( X509Crl crl )
        {
            try
            {
                return TbsCertificateList.GetInstance( Asn1Object.FromByteArray( crl.GetTbsCertList() ) ).Issuer;
            }
            catch (Exception ex)
            {
                throw new CrlException( "Could not extract issuer", ex );
            }
        }
    }
}
