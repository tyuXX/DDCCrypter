// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsRsaKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsRsaKeyExchange : AbstractTlsKeyExchange
    {
        protected AsymmetricKeyParameter mServerPublicKey = null;
        protected RsaKeyParameters mRsaServerPublicKey = null;
        protected TlsEncryptionCredentials mServerCredentials = null;
        protected byte[] mPremasterSecret;

        public TlsRsaKeyExchange( IList supportedSignatureAlgorithms )
          : base( 1, supportedSignatureAlgorithms )
        {
        }

        public override void SkipServerCredentials() => throw new TlsFatalAlert( 10 );

        public override void ProcessServerCredentials( TlsCredentials serverCredentials )
        {
            if (!(serverCredentials is TlsEncryptionCredentials))
                throw new TlsFatalAlert( 80 );
            this.ProcessServerCertificate( serverCredentials.Certificate );
            this.mServerCredentials = (TlsEncryptionCredentials)serverCredentials;
        }

        public override void ProcessServerCertificate( Certificate serverCertificate )
        {
            X509CertificateStructure c = !serverCertificate.IsEmpty ? serverCertificate.GetCertificateAt( 0 ) : throw new TlsFatalAlert( 42 );
            SubjectPublicKeyInfo subjectPublicKeyInfo = c.SubjectPublicKeyInfo;
            try
            {
                this.mServerPublicKey = PublicKeyFactory.CreateKey( subjectPublicKeyInfo );
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 43, ex );
            }
            this.mRsaServerPublicKey = !this.mServerPublicKey.IsPrivate ? this.ValidateRsaPublicKey( (RsaKeyParameters)this.mServerPublicKey ) : throw new TlsFatalAlert( 80 );
            TlsUtilities.ValidateKeyUsage( c, 32 );
            base.ProcessServerCertificate( serverCertificate );
        }

        public override void ValidateCertificateRequest( CertificateRequest certificateRequest )
        {
            foreach (byte certificateType in certificateRequest.CertificateTypes)
            {
                switch (certificateType)
                {
                    case 1:
                    case 2:
                    case 64:
                        continue;
                    default:
                        throw new TlsFatalAlert( 47 );
                }
            }
        }

        public override void ProcessClientCredentials( TlsCredentials clientCredentials )
        {
            if (!(clientCredentials is TlsSignerCredentials))
                throw new TlsFatalAlert( 80 );
        }

        public override void GenerateClientKeyExchange( Stream output ) => this.mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret( this.mContext, this.mRsaServerPublicKey, output );

        public override void ProcessClientKeyExchange( Stream input ) => this.mPremasterSecret = this.mServerCredentials.DecryptPreMasterSecret( !TlsUtilities.IsSsl( this.mContext ) ? TlsUtilities.ReadOpaque16( input ) : Streams.ReadAll( input ) );

        public override byte[] GeneratePremasterSecret()
        {
            byte[] premasterSecret = this.mPremasterSecret != null ? this.mPremasterSecret : throw new TlsFatalAlert( 80 );
            this.mPremasterSecret = null;
            return premasterSecret;
        }

        protected virtual RsaKeyParameters ValidateRsaPublicKey( RsaKeyParameters key ) => key.Exponent.IsProbablePrime( 2 ) ? key : throw new TlsFatalAlert( 47 );
    }
}
