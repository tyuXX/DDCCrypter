// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.PskTlsServer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class PskTlsServer : AbstractTlsServer
    {
        protected TlsPskIdentityManager mPskIdentityManager;

        public PskTlsServer( TlsPskIdentityManager pskIdentityManager )
          : this( new DefaultTlsCipherFactory(), pskIdentityManager )
        {
        }

        public PskTlsServer( TlsCipherFactory cipherFactory, TlsPskIdentityManager pskIdentityManager )
          : base( cipherFactory )
        {
            this.mPskIdentityManager = pskIdentityManager;
        }

        protected virtual TlsEncryptionCredentials GetRsaEncryptionCredentials() => throw new TlsFatalAlert( 80 );

        protected virtual DHParameters GetDHParameters() => DHStandardGroups.rfc5114_2048_256;

        protected override int[] GetCipherSuites() => new int[4]
        {
      49207,
      49205,
      178,
      144
        };

        public override TlsCredentials GetCredentials()
        {
            switch (TlsUtilities.GetKeyExchangeAlgorithm( this.mSelectedCipherSuite ))
            {
                case 13:
                case 14:
                case 24:
                    return null;
                case 15:
                    return this.GetRsaEncryptionCredentials();
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

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

        protected virtual TlsKeyExchange CreatePskKeyExchange( int keyExchange ) => new TlsPskKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, null, this.mPskIdentityManager, this.GetDHParameters(), this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats );
    }
}
