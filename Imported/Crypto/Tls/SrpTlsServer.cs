// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SrpTlsServer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class SrpTlsServer : AbstractTlsServer
    {
        protected TlsSrpIdentityManager mSrpIdentityManager;
        protected byte[] mSrpIdentity = null;
        protected TlsSrpLoginParameters mLoginParameters = null;

        public SrpTlsServer( TlsSrpIdentityManager srpIdentityManager )
          : this( new DefaultTlsCipherFactory(), srpIdentityManager )
        {
        }

        public SrpTlsServer( TlsCipherFactory cipherFactory, TlsSrpIdentityManager srpIdentityManager )
          : base( cipherFactory )
        {
            this.mSrpIdentityManager = srpIdentityManager;
        }

        protected virtual TlsSignerCredentials GetDsaSignerCredentials() => throw new TlsFatalAlert( 80 );

        protected virtual TlsSignerCredentials GetRsaSignerCredentials() => throw new TlsFatalAlert( 80 );

        protected override int[] GetCipherSuites() => new int[6]
        {
      49186,
      49183,
      49185,
      49182,
      49184,
      49181
        };

        public override void ProcessClientExtensions( IDictionary clientExtensions )
        {
            base.ProcessClientExtensions( clientExtensions );
            this.mSrpIdentity = TlsSrpUtilities.GetSrpExtension( clientExtensions );
        }

        public override int GetSelectedCipherSuite()
        {
            int selectedCipherSuite = base.GetSelectedCipherSuite();
            if (TlsSrpUtilities.IsSrpCipherSuite( selectedCipherSuite ))
            {
                if (this.mSrpIdentity != null)
                    this.mLoginParameters = this.mSrpIdentityManager.GetLoginParameters( this.mSrpIdentity );
                if (this.mLoginParameters == null)
                    throw new TlsFatalAlert( 115 );
            }
            return selectedCipherSuite;
        }

        public override TlsCredentials GetCredentials()
        {
            switch (TlsUtilities.GetKeyExchangeAlgorithm( this.mSelectedCipherSuite ))
            {
                case 21:
                    return null;
                case 22:
                    return this.GetDsaSignerCredentials();
                case 23:
                    return this.GetRsaSignerCredentials();
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public override TlsKeyExchange GetKeyExchange()
        {
            int exchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm( this.mSelectedCipherSuite );
            switch (exchangeAlgorithm)
            {
                case 21:
                case 22:
                case 23:
                    return this.CreateSrpKeyExchange( exchangeAlgorithm );
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        protected virtual TlsKeyExchange CreateSrpKeyExchange( int keyExchange ) => new TlsSrpKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mSrpIdentity, this.mLoginParameters );
    }
}
