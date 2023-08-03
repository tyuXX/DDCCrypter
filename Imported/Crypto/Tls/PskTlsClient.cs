// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.PskTlsClient
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public class PskTlsClient : AbstractTlsClient
    {
        protected TlsPskIdentity mPskIdentity;

        public PskTlsClient( TlsPskIdentity pskIdentity )
          : this( new DefaultTlsCipherFactory(), pskIdentity )
        {
        }

        public PskTlsClient( TlsCipherFactory cipherFactory, TlsPskIdentity pskIdentity )
          : base( cipherFactory )
        {
            this.mPskIdentity = pskIdentity;
        }

        public override int[] GetCipherSuites() => new int[4]
        {
      49207,
      49205,
      178,
      144
        };

        public override TlsKeyExchange GetKeyExchange()
        {
            int exchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm( this.mSelectedCipherSuite );
            switch (exchangeAlgorithm)
            {
                case 13:
                case 14:
                case 15:
                case 24:
                    return this.CreatePskKeyExchange( exchangeAlgorithm );
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public override TlsAuthentication GetAuthentication() => throw new TlsFatalAlert( 80 );

        protected virtual TlsKeyExchange CreatePskKeyExchange( int keyExchange ) => new TlsPskKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mPskIdentity, null, null, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats );
    }
}
