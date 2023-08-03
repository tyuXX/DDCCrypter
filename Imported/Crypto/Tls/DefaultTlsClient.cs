// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DefaultTlsClient
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class DefaultTlsClient : AbstractTlsClient
    {
        public DefaultTlsClient()
        {
        }

        public DefaultTlsClient( TlsCipherFactory cipherFactory )
          : base( cipherFactory )
        {
        }

        public override int[] GetCipherSuites() => new int[15]
        {
      49195,
      49187,
      49161,
      49199,
      49191,
      49171,
      162,
      64,
      50,
      158,
      103,
      51,
      156,
      60,
      47
        };

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

        protected virtual TlsKeyExchange CreateDHKeyExchange( int keyExchange ) => new TlsDHKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, null );

        protected virtual TlsKeyExchange CreateDheKeyExchange( int keyExchange ) => new TlsDheKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, null );

        protected virtual TlsKeyExchange CreateECDHKeyExchange( int keyExchange ) => new TlsECDHKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats );

        protected virtual TlsKeyExchange CreateECDheKeyExchange( int keyExchange ) => new TlsECDheKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats );

        protected virtual TlsKeyExchange CreateRsaKeyExchange() => new TlsRsaKeyExchange( this.mSupportedSignatureAlgorithms );
    }
}
