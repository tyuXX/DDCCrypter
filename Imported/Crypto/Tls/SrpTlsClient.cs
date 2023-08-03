// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SrpTlsClient
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class SrpTlsClient : AbstractTlsClient
    {
        protected TlsSrpGroupVerifier mGroupVerifier;
        protected byte[] mIdentity;
        protected byte[] mPassword;

        public SrpTlsClient( byte[] identity, byte[] password )
          : this( new DefaultTlsCipherFactory(), new DefaultTlsSrpGroupVerifier(), identity, password )
        {
        }

        public SrpTlsClient( TlsCipherFactory cipherFactory, byte[] identity, byte[] password )
          : this( cipherFactory, new DefaultTlsSrpGroupVerifier(), identity, password )
        {
        }

        public SrpTlsClient(
          TlsCipherFactory cipherFactory,
          TlsSrpGroupVerifier groupVerifier,
          byte[] identity,
          byte[] password )
          : base( cipherFactory )
        {
            this.mGroupVerifier = groupVerifier;
            this.mIdentity = Arrays.Clone( identity );
            this.mPassword = Arrays.Clone( password );
        }

        protected virtual bool RequireSrpServerExtension => false;

        public override int[] GetCipherSuites() => new int[1]
        {
      49182
        };

        public override IDictionary GetClientExtensions()
        {
            IDictionary extensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( base.GetClientExtensions() );
            TlsSrpUtilities.AddSrpExtension( extensions, this.mIdentity );
            return extensions;
        }

        public override void ProcessServerExtensions( IDictionary serverExtensions )
        {
            if (!TlsUtilities.HasExpectedEmptyExtensionData( serverExtensions, 12, 47 ) && this.RequireSrpServerExtension)
                throw new TlsFatalAlert( 47 );
            base.ProcessServerExtensions( serverExtensions );
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

        public override TlsAuthentication GetAuthentication() => throw new TlsFatalAlert( 80 );

        protected virtual TlsKeyExchange CreateSrpKeyExchange( int keyExchange ) => new TlsSrpKeyExchange( keyExchange, this.mSupportedSignatureAlgorithms, this.mGroupVerifier, this.mIdentity, this.mPassword );
    }
}
