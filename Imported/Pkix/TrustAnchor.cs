// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.TrustAnchor
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Text;

namespace Org.BouncyCastle.Pkix
{
    public class TrustAnchor
    {
        private readonly AsymmetricKeyParameter pubKey;
        private readonly string caName;
        private readonly X509Name caPrincipal;
        private readonly X509Certificate trustedCert;
        private byte[] ncBytes;
        private NameConstraints nc;

        public TrustAnchor( X509Certificate trustedCert, byte[] nameConstraints )
        {
            this.trustedCert = trustedCert != null ? trustedCert : throw new ArgumentNullException( nameof( trustedCert ) );
            this.pubKey = null;
            this.caName = null;
            this.caPrincipal = null;
            this.setNameConstraints( nameConstraints );
        }

        public TrustAnchor( X509Name caPrincipal, AsymmetricKeyParameter pubKey, byte[] nameConstraints )
        {
            if (caPrincipal == null)
                throw new ArgumentNullException( nameof( caPrincipal ) );
            if (pubKey == null)
                throw new ArgumentNullException( nameof( pubKey ) );
            this.trustedCert = null;
            this.caPrincipal = caPrincipal;
            this.caName = caPrincipal.ToString();
            this.pubKey = pubKey;
            this.setNameConstraints( nameConstraints );
        }

        public TrustAnchor( string caName, AsymmetricKeyParameter pubKey, byte[] nameConstraints )
        {
            if (caName == null)
                throw new ArgumentNullException( nameof( caName ) );
            if (pubKey == null)
                throw new ArgumentNullException( nameof( pubKey ) );
            this.caPrincipal = caName.Length != 0 ? new X509Name( caName ) : throw new ArgumentException( "caName can not be an empty string" );
            this.pubKey = pubKey;
            this.caName = caName;
            this.trustedCert = null;
            this.setNameConstraints( nameConstraints );
        }

        public X509Certificate TrustedCert => this.trustedCert;

        public X509Name CA => this.caPrincipal;

        public string CAName => this.caName;

        public AsymmetricKeyParameter CAPublicKey => this.pubKey;

        private void setNameConstraints( byte[] bytes )
        {
            if (bytes == null)
            {
                this.ncBytes = null;
                this.nc = null;
            }
            else
            {
                this.ncBytes = (byte[])bytes.Clone();
                this.nc = NameConstraints.GetInstance( Asn1Object.FromByteArray( bytes ) );
            }
        }

        public byte[] GetNameConstraints => Arrays.Clone( this.ncBytes );

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( "[" );
            stringBuilder.Append( newLine );
            if (this.pubKey != null)
            {
                stringBuilder.Append( "  Trusted CA Public Key: " ).Append( pubKey ).Append( newLine );
                stringBuilder.Append( "  Trusted CA Issuer Name: " ).Append( this.caName ).Append( newLine );
            }
            else
                stringBuilder.Append( "  Trusted CA cert: " ).Append( TrustedCert ).Append( newLine );
            if (this.nc != null)
                stringBuilder.Append( "  Name Constraints: " ).Append( nc ).Append( newLine );
            return stringBuilder.ToString();
        }
    }
}
