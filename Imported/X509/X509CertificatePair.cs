// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509CertificatePair
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;

namespace Org.BouncyCastle.X509
{
    public class X509CertificatePair
    {
        private readonly X509Certificate forward;
        private readonly X509Certificate reverse;

        public X509CertificatePair( X509Certificate forward, X509Certificate reverse )
        {
            this.forward = forward;
            this.reverse = reverse;
        }

        public X509CertificatePair( CertificatePair pair )
        {
            if (pair.Forward != null)
                this.forward = new X509Certificate( pair.Forward );
            if (pair.Reverse == null)
                return;
            this.reverse = new X509Certificate( pair.Reverse );
        }

        public byte[] GetEncoded()
        {
            try
            {
                X509CertificateStructure forward = null;
                X509CertificateStructure reverse = null;
                if (this.forward != null)
                {
                    forward = X509CertificateStructure.GetInstance( Asn1Object.FromByteArray( this.forward.GetEncoded() ) );
                    if (forward == null)
                        throw new CertificateEncodingException( "unable to get encoding for forward" );
                }
                if (this.reverse != null)
                {
                    reverse = X509CertificateStructure.GetInstance( Asn1Object.FromByteArray( this.reverse.GetEncoded() ) );
                    if (reverse == null)
                        throw new CertificateEncodingException( "unable to get encoding for reverse" );
                }
                return new CertificatePair( forward, reverse ).GetDerEncoded();
            }
            catch (Exception ex)
            {
                throw new CertificateEncodingException( ex.Message, ex );
            }
        }

        public X509Certificate Forward => this.forward;

        public X509Certificate Reverse => this.reverse;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is X509CertificatePair x509CertificatePair && Equals( forward, x509CertificatePair.forward ) && Equals( reverse, x509CertificatePair.reverse );
        }

        public override int GetHashCode()
        {
            int hashCode = -1;
            if (this.forward != null)
                hashCode ^= this.forward.GetHashCode();
            if (this.reverse != null)
                hashCode = (hashCode * 17) ^ this.reverse.GetHashCode();
            return hashCode;
        }
    }
}
