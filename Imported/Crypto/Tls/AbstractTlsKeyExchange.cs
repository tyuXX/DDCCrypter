// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsKeyExchange : TlsKeyExchange
    {
        protected readonly int mKeyExchange;
        protected IList mSupportedSignatureAlgorithms;
        protected TlsContext mContext;

        protected AbstractTlsKeyExchange( int keyExchange, IList supportedSignatureAlgorithms )
        {
            this.mKeyExchange = keyExchange;
            this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
        }

        protected virtual DigitallySigned ParseSignature( Stream input )
        {
            DigitallySigned signature = DigitallySigned.Parse( this.mContext, input );
            SignatureAndHashAlgorithm algorithm = signature.Algorithm;
            if (algorithm != null)
                TlsUtilities.VerifySupportedSignatureAlgorithm( this.mSupportedSignatureAlgorithms, algorithm );
            return signature;
        }

        public virtual void Init( TlsContext context )
        {
            this.mContext = context;
            ProtocolVersion clientVersion = context.ClientVersion;
            if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed( clientVersion ))
            {
                if (this.mSupportedSignatureAlgorithms != null)
                    return;
                switch (this.mKeyExchange)
                {
                    case 1:
                    case 5:
                    case 9:
                    case 15:
                    case 18:
                    case 19:
                    case 23:
                        this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultRsaSignatureAlgorithms();
                        break;
                    case 3:
                    case 7:
                    case 22:
                        this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultDssSignatureAlgorithms();
                        break;
                    case 13:
                        break;
                    case 14:
                        break;
                    case 16:
                    case 17:
                        this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultECDsaSignatureAlgorithms();
                        break;
                    case 21:
                        break;
                    case 24:
                        break;
                    default:
                        throw new InvalidOperationException( "unsupported key exchange algorithm" );
                }
            }
            else if (this.mSupportedSignatureAlgorithms != null)
                throw new InvalidOperationException( "supported_signature_algorithms not allowed for " + clientVersion );
        }

        public abstract void SkipServerCredentials();

        public virtual void ProcessServerCertificate( Certificate serverCertificate )
        {
            if (this.mSupportedSignatureAlgorithms != null)
                ;
        }

        public virtual void ProcessServerCredentials( TlsCredentials serverCredentials ) => this.ProcessServerCertificate( serverCredentials.Certificate );

        public virtual bool RequiresServerKeyExchange => false;

        public virtual byte[] GenerateServerKeyExchange()
        {
            if (this.RequiresServerKeyExchange)
                throw new TlsFatalAlert( 80 );
            return null;
        }

        public virtual void SkipServerKeyExchange()
        {
            if (this.RequiresServerKeyExchange)
                throw new TlsFatalAlert( 10 );
        }

        public virtual void ProcessServerKeyExchange( Stream input )
        {
            if (!this.RequiresServerKeyExchange)
                throw new TlsFatalAlert( 10 );
        }

        public abstract void ValidateCertificateRequest( CertificateRequest certificateRequest );

        public virtual void SkipClientCredentials()
        {
        }

        public abstract void ProcessClientCredentials( TlsCredentials clientCredentials );

        public virtual void ProcessClientCertificate( Certificate clientCertificate )
        {
        }

        public abstract void GenerateClientKeyExchange( Stream output );

        public virtual void ProcessClientKeyExchange( Stream input ) => throw new TlsFatalAlert( 80 );

        public abstract byte[] GeneratePremasterSecret();
    }
}
