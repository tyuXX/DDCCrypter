// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsECDHKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsECDHKeyExchange : AbstractTlsKeyExchange
    {
        protected TlsSigner mTlsSigner;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected AsymmetricKeyParameter mServerPublicKey;
        protected TlsAgreementCredentials mAgreementCredentials;
        protected ECPrivateKeyParameters mECAgreePrivateKey;
        protected ECPublicKeyParameters mECAgreePublicKey;

        public TlsECDHKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          int[] namedCurves,
          byte[] clientECPointFormats,
          byte[] serverECPointFormats )
          : base( keyExchange, supportedSignatureAlgorithms )
        {
            switch (keyExchange)
            {
                case 16:
                case 18:
                    this.mTlsSigner = null;
                    break;
                case 17:
                    this.mTlsSigner = new TlsECDsaSigner();
                    break;
                case 19:
                    this.mTlsSigner = new TlsRsaSigner();
                    break;
                default:
                    throw new InvalidOperationException( "unsupported key exchange algorithm" );
            }
            this.mNamedCurves = namedCurves;
            this.mClientECPointFormats = clientECPointFormats;
            this.mServerECPointFormats = serverECPointFormats;
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
                    this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey( (ECPublicKeyParameters)this.mServerPublicKey );
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
                    case 17:
                    case 19:
                    case 20:
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
                    case 64:
                    case 65:
                    case 66:
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
            this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange( this.mContext.SecureRandom, this.mServerECPointFormats, this.mECAgreePublicKey.Parameters, output );
        }

        public override void ProcessClientCertificate( Certificate clientCertificate )
        {
        }

        public override void ProcessClientKeyExchange( Stream input )
        {
            if (this.mECAgreePublicKey != null)
                return;
            this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey( TlsEccUtilities.DeserializeECPublicKey( this.mServerECPointFormats, this.mECAgreePrivateKey.Parameters, TlsUtilities.ReadOpaque8( input ) ) );
        }

        public override byte[] GeneratePremasterSecret()
        {
            if (this.mAgreementCredentials != null)
                return this.mAgreementCredentials.GenerateAgreement( mECAgreePublicKey );
            return this.mECAgreePrivateKey != null ? TlsEccUtilities.CalculateECDHBasicAgreement( this.mECAgreePublicKey, this.mECAgreePrivateKey ) : throw new TlsFatalAlert( 80 );
        }
    }
}
