// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Extension.SubjectKeyIdentifierStructure
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security.Certificates;

namespace Org.BouncyCastle.X509.Extension
{
    public class SubjectKeyIdentifierStructure : SubjectKeyIdentifier
    {
        public SubjectKeyIdentifierStructure( Asn1OctetString encodedValue )
          : base( (Asn1OctetString)X509ExtensionUtilities.FromExtensionValue( encodedValue ) )
        {
        }

        private static Asn1OctetString FromPublicKey( AsymmetricKeyParameter pubKey )
        {
            try
            {
                return (Asn1OctetString)new SubjectKeyIdentifier( SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( pubKey ) ).ToAsn1Object();
            }
            catch (Exception ex)
            {
                throw new CertificateParsingException( "Exception extracting certificate details: " + ex.ToString() );
            }
        }

        public SubjectKeyIdentifierStructure( AsymmetricKeyParameter pubKey )
          : base( FromPublicKey( pubKey ) )
        {
        }
    }
}
