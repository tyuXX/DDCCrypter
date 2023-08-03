// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DefaultTlsSignerCredentials
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DefaultTlsSignerCredentials : AbstractTlsSignerCredentials
    {
        protected readonly TlsContext mContext;
        protected readonly Certificate mCertificate;
        protected readonly AsymmetricKeyParameter mPrivateKey;
        protected readonly SignatureAndHashAlgorithm mSignatureAndHashAlgorithm;
        protected readonly TlsSigner mSigner;

        public DefaultTlsSignerCredentials(
          TlsContext context,
          Certificate certificate,
          AsymmetricKeyParameter privateKey )
          : this( context, certificate, privateKey, null )
        {
        }

        public DefaultTlsSignerCredentials(
          TlsContext context,
          Certificate certificate,
          AsymmetricKeyParameter privateKey,
          SignatureAndHashAlgorithm signatureAndHashAlgorithm )
        {
            if (certificate == null)
                throw new ArgumentNullException( nameof( certificate ) );
            if (certificate.IsEmpty)
                throw new ArgumentException( "cannot be empty", "clientCertificate" );
            if (privateKey == null)
                throw new ArgumentNullException( nameof( privateKey ) );
            if (!privateKey.IsPrivate)
                throw new ArgumentException( "must be private", nameof( privateKey ) );
            if (TlsUtilities.IsTlsV12( context ) && signatureAndHashAlgorithm == null)
                throw new ArgumentException( "cannot be null for (D)TLS 1.2+", nameof( signatureAndHashAlgorithm ) );
            switch (privateKey)
            {
                case RsaKeyParameters _:
                    this.mSigner = new TlsRsaSigner();
                    break;
                case DsaPrivateKeyParameters _:
                    this.mSigner = new TlsDssSigner();
                    break;
                case ECPrivateKeyParameters _:
                    this.mSigner = new TlsECDsaSigner();
                    break;
                default:
                    throw new ArgumentException( "type not supported: " + Platform.GetTypeName( privateKey ), nameof( privateKey ) );
            }
            this.mSigner.Init( context );
            this.mContext = context;
            this.mCertificate = certificate;
            this.mPrivateKey = privateKey;
            this.mSignatureAndHashAlgorithm = signatureAndHashAlgorithm;
        }

        public override Certificate Certificate => this.mCertificate;

        public override byte[] GenerateCertificateSignature( byte[] hash )
        {
            try
            {
                return TlsUtilities.IsTlsV12( this.mContext ) ? this.mSigner.GenerateRawSignature( this.mSignatureAndHashAlgorithm, this.mPrivateKey, hash ) : this.mSigner.GenerateRawSignature( this.mPrivateKey, hash );
            }
            catch (CryptoException ex)
            {
                throw new TlsFatalAlert( 80, ex );
            }
        }

        public override SignatureAndHashAlgorithm SignatureAndHashAlgorithm => this.mSignatureAndHashAlgorithm;
    }
}
