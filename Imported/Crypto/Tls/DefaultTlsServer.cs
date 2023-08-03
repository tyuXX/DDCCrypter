// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DefaultTlsServer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class DefaultTlsServer : AbstractTlsServer
    {
        public DefaultTlsServer()
        {
        }

        public DefaultTlsServer( TlsCipherFactory cipherFactory )
          : base( cipherFactory )
        {
        }

        protected virtual TlsSignerCredentials GetDsaSignerCredentials() => throw new TlsFatalAlert( 80 );

        protected virtual TlsSignerCredentials GetECDsaSignerCredentials() => throw new TlsFatalAlert( 80 );

        protected virtual TlsEncryptionCredentials GetRsaEncryptionCredentials() => throw new TlsFatalAlert( 80 );

        protected virtual TlsSignerCredentials GetRsaSignerCredentials() => throw new TlsFatalAlert( 80 );

        protected virtual DHParameters GetDHParameters() => DHStandardGroups.rfc5114_2048_256;

        protected override int[] GetCipherSuites() => new int[18]
        {
      49200,
      49199,
      49192,
      49191,
      49172,
      49171,
      159,
      158,
      107,
      103,
      57,
      51,
      157,
      156,
      61,
      60,
      53,
      47
        };

        public override TlsCredentials GetCredentials()
        {
            switch (TlsUtilities.GetKeyExchangeAlgorithm( this.mSelectedCipherSuite ))
            {
                case 1:
                    return this.GetRsaEncryptionCredentials();
                case 3:
                case 7:
                    return this.GetDsaSignerCredentials();
                case 5:
                case 19:
                    return this.GetRsaSignerCredentials();
                case 16:
                case 17:
                    return this.GetECDsaSignerCredentials();
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public override TlsKeyExchange GetKeyExchange()
        {
            int exchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm( this.mSelectedCipherSuite );
            switch (exchangeAlgorithm)
            {
                case 1:
                    return this.CreateRsaKeyExchange();
                case 3:
                case 5:
                    return this.CreateDheKeyExchange( exchangeAlgorithm );
                case 7:
                case 9:
                    return this.CreateDHKeyExchange( exchangeAlgorithm );
                case 16:
                case 18:
                    return this.CreateECDHKeyExchange( exchangeAlgorithm );
                case 17:
                case 19:
                    return this.CreateECDheKeyExchange( exchangeAlgorithm );
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        protected virtual TlsKeyExchange CreateDHKeyExchange( int keyExchange ) => new TlsDHKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.GetDHParameters() );

        protected virtual TlsKeyExchange CreateDheKeyExchange( int keyExchange ) => new TlsDheKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.GetDHParameters() );

        protected virtual TlsKeyExchange CreateECDHKeyExchange( int keyExchange ) => new TlsECDHKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats );

        protected virtual TlsKeyExchange CreateECDheKeyExchange( int keyExchange ) => new TlsECDheKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats );

        protected virtual TlsKeyExchange CreateRsaKeyExchange() => new TlsRsaKeyExchange( this.mSupportedSignatureAlgorithms );
    }
}
