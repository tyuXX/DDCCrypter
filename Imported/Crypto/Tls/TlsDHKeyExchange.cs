// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsDHKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsDHKeyExchange : AbstractTlsKeyExchange
    {
        protected TlsSigner mTlsSigner;
        protected DHParameters mDHParameters;
        protected AsymmetricKeyParameter mServerPublicKey;
        protected TlsAgreementCredentials mAgreementCredentials;
        protected DHPrivateKeyParameters mDHAgreePrivateKey;
        protected DHPublicKeyParameters mDHAgreePublicKey;

        public TlsDHKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          DHParameters dhParameters )
          : base( keyExchange, supportedSignatureAlgorithms )
        {
            switch (keyExchange)
            {
                case 3:
                    this.mTlsSigner = new TlsDssSigner();
                    break;
                case 5:
                    this.mTlsSigner = new TlsRsaSigner();
                    break;
                case 7:
                case 9:
                    this.mTlsSigner = null;
                    break;
                default:
                    throw new InvalidOperationException( "unsupported key exchange algorithm" );
            }
            this.mDHParameters = dhParameters;
        }

        public override void Init( TlsContext context )
        {
            base.Init( context );
            if (this.mTlsSigner == null)
                return;
            this.mTlsSigner.Init( context );
        }

        public override void SkipServerCredentials() => throw new TlsFatalAlert( 10 );

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
            if (this.mTlsSigner == null)
            {
                try
                {
                    this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey( (DHPublicKeyParameters)this.mServerPublicKey );
                    this.mDHParameters = this.ValidateDHParameters( this.mDHAgreePublicKey.Parameters );
                }
                catch (InvalidCastException ex)
                {
                    throw new TlsFatalAlert( 46, ex );
                }
                TlsUtilities.ValidateKeyUsage( c, 8 );
            }
            else
            {
                if (!this.mTlsSigner.IsValidPublicKey( this.mServerPublicKey ))
                    throw new TlsFatalAlert( 46 );
                TlsUtilities.ValidateKeyUsage( c, 128 );
            }
            base.ProcessServerCertificate( serverCertificate );
        }

        public override bool RequiresServerKeyExchange
        {
            get
            {
                switch (this.mKeyExchange)
                {
                    case 3:
                    case 5:
                    case 11:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override void ValidateCertificateRequest( CertificateRequest certificateRequest )
        {
            foreach (byte certificateType in certificateRequest.CertificateTypes)
            {
                switch (certificateType)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 64:
                        continue;
                    default:
                        throw new TlsFatalAlert( 47 );
                }
            }
        }

        public override void ProcessClientCredentials( TlsCredentials clientCredentials )
        {
            switch (clientCredentials)
            {
                case TlsAgreementCredentials _:
                    this.mAgreementCredentials = (TlsAgreementCredentials)clientCredentials;
                    break;
                case TlsSignerCredentials _:
                    break;
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public override void GenerateClientKeyExchange( Stream output )
        {
            if (this.mAgreementCredentials != null)
                return;
            this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange( this.mContext.SecureRandom, this.mDHParameters, output );
        }

        public override void ProcessClientCertificate( Certificate clientCertificate )
        {
        }

        public override void ProcessClientKeyExchange( Stream input )
        {
            if (this.mDHAgreePublicKey != null)
                return;
            this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey( new DHPublicKeyParameters( TlsDHUtilities.ReadDHParameter( input ), this.mDHParameters ) );
        }

        public override byte[] GeneratePremasterSecret()
        {
            if (this.mAgreementCredentials != null)
                return this.mAgreementCredentials.GenerateAgreement( mDHAgreePublicKey );
            return this.mDHAgreePrivateKey != null ? TlsDHUtilities.CalculateDHBasicAgreement( this.mDHAgreePublicKey, this.mDHAgreePrivateKey ) : throw new TlsFatalAlert( 80 );
        }

        protected virtual int MinimumPrimeBits => 1024;

        protected virtual DHParameters ValidateDHParameters( DHParameters parameters )
        {
            if (parameters.P.BitLength < this.MinimumPrimeBits)
                throw new TlsFatalAlert( 71 );
            return TlsDHUtilities.ValidateDHParameters( parameters );
        }
    }
}
