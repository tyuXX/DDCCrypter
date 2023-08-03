// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsClient
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsClient : AbstractTlsPeer, TlsClient, TlsPeer
    {
        protected TlsCipherFactory mCipherFactory;
        protected TlsClientContext mContext;
        protected IList mSupportedSignatureAlgorithms;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected int mSelectedCipherSuite;
        protected short mSelectedCompressionMethod;

        public AbstractTlsClient()
          : this( new DefaultTlsCipherFactory() )
        {
        }

        public AbstractTlsClient( TlsCipherFactory cipherFactory ) => this.mCipherFactory = cipherFactory;

        protected virtual bool AllowUnexpectedServerExtension( int extensionType, byte[] extensionData )
        {
            if (extensionType != 10)
                return false;
            TlsEccUtilities.ReadSupportedEllipticCurvesExtension( extensionData );
            return true;
        }

        protected virtual void CheckForUnexpectedServerExtension(
          IDictionary serverExtensions,
          int extensionType )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( serverExtensions, extensionType );
            if (extensionData != null && !this.AllowUnexpectedServerExtension( extensionType, extensionData ))
                throw new TlsFatalAlert( 47 );
        }

        public virtual void Init( TlsClientContext context ) => this.mContext = context;

        public virtual TlsSession GetSessionToResume() => null;

        public virtual ProtocolVersion ClientHelloRecordLayerVersion => this.ClientVersion;

        public virtual ProtocolVersion ClientVersion => ProtocolVersion.TLSv12;

        public virtual bool IsFallback => false;

        public virtual IDictionary GetClientExtensions()
        {
            IDictionary extensions = null;
            if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed( this.mContext.ClientVersion ))
            {
                this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultSupportedSignatureAlgorithms();
                extensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( extensions );
                TlsUtilities.AddSignatureAlgorithmsExtension( extensions, this.mSupportedSignatureAlgorithms );
            }
            if (TlsEccUtilities.ContainsEccCipherSuites( this.GetCipherSuites() ))
            {
                this.mNamedCurves = new int[2] { 23, 24 };
                this.mClientECPointFormats = new byte[3]
                {
           0,
           1,
           2
                };
                extensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( extensions );
                TlsEccUtilities.AddSupportedEllipticCurvesExtension( extensions, this.mNamedCurves );
                TlsEccUtilities.AddSupportedPointFormatsExtension( extensions, this.mClientECPointFormats );
            }
            return extensions;
        }

        public virtual ProtocolVersion MinimumVersion => ProtocolVersion.TLSv10;

        public virtual void NotifyServerVersion( ProtocolVersion serverVersion )
        {
            if (!this.MinimumVersion.IsEqualOrEarlierVersionOf( serverVersion ))
                throw new TlsFatalAlert( 70 );
        }

        public abstract int[] GetCipherSuites();

        public virtual byte[] GetCompressionMethods() => new byte[1];

        public virtual void NotifySessionID( byte[] sessionID )
        {
        }

        public virtual void NotifySelectedCipherSuite( int selectedCipherSuite ) => this.mSelectedCipherSuite = selectedCipherSuite;

        public virtual void NotifySelectedCompressionMethod( byte selectedCompressionMethod ) => this.mSelectedCompressionMethod = selectedCompressionMethod;

        public virtual void ProcessServerExtensions( IDictionary serverExtensions )
        {
            if (serverExtensions == null)
                return;
            this.CheckForUnexpectedServerExtension( serverExtensions, 13 );
            this.CheckForUnexpectedServerExtension( serverExtensions, 10 );
            if (TlsEccUtilities.IsEccCipherSuite( this.mSelectedCipherSuite ))
                this.mServerECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension( serverExtensions );
            else
                this.CheckForUnexpectedServerExtension( serverExtensions, 11 );
        }

        public virtual void ProcessServerSupplementalData( IList serverSupplementalData )
        {
            if (serverSupplementalData != null)
                throw new TlsFatalAlert( 10 );
        }

        public abstract TlsKeyExchange GetKeyExchange();

        public abstract TlsAuthentication GetAuthentication();

        public virtual IList GetClientSupplementalData() => null;

        public override TlsCompression GetCompression()
        {
            switch (this.mSelectedCompressionMethod)
            {
                case 0:
                    return new TlsNullCompression();
                case 1:
                    return new TlsDeflateCompression();
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public override TlsCipher GetCipher() => this.mCipherFactory.CreateCipher( mContext, TlsUtilities.GetEncryptionAlgorithm( this.mSelectedCipherSuite ), TlsUtilities.GetMacAlgorithm( this.mSelectedCipherSuite ) );

        public virtual void NotifyNewSessionTicket( NewSessionTicket newSessionTicket )
        {
        }
    }
}
