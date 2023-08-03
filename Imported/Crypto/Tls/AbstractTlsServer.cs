﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsServer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsServer : AbstractTlsPeer, TlsServer, TlsPeer
    {
        protected TlsCipherFactory mCipherFactory;
        protected TlsServerContext mContext;
        protected ProtocolVersion mClientVersion;
        protected int[] mOfferedCipherSuites;
        protected byte[] mOfferedCompressionMethods;
        protected IDictionary mClientExtensions;
        protected bool mEncryptThenMacOffered;
        protected short mMaxFragmentLengthOffered;
        protected bool mTruncatedHMacOffered;
        protected IList mSupportedSignatureAlgorithms;
        protected bool mEccCipherSuitesOffered;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected ProtocolVersion mServerVersion;
        protected int mSelectedCipherSuite;
        protected byte mSelectedCompressionMethod;
        protected IDictionary mServerExtensions;

        public AbstractTlsServer()
          : this( new DefaultTlsCipherFactory() )
        {
        }

        public AbstractTlsServer( TlsCipherFactory cipherFactory ) => this.mCipherFactory = cipherFactory;

        protected virtual bool AllowEncryptThenMac => true;

        protected virtual bool AllowTruncatedHMac => false;

        protected virtual IDictionary CheckServerExtensions() => this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( this.mServerExtensions );

        protected abstract int[] GetCipherSuites();

        protected byte[] GetCompressionMethods() => new byte[1];

        protected virtual ProtocolVersion MaximumVersion => ProtocolVersion.TLSv11;

        protected virtual ProtocolVersion MinimumVersion => ProtocolVersion.TLSv10;

        protected virtual bool SupportsClientEccCapabilities( int[] namedCurves, byte[] ecPointFormats )
        {
            if (namedCurves == null)
                return TlsEccUtilities.HasAnySupportedNamedCurves();
            for (int index = 0; index < namedCurves.Length; ++index)
            {
                int namedCurve = namedCurves[index];
                if (NamedCurve.IsValid( namedCurve ) && (!NamedCurve.RefersToASpecificNamedCurve( namedCurve ) || TlsEccUtilities.IsSupportedNamedCurve( namedCurve )))
                    return true;
            }
            return false;
        }

        public virtual void Init( TlsServerContext context ) => this.mContext = context;

        public virtual void NotifyClientVersion( ProtocolVersion clientVersion ) => this.mClientVersion = clientVersion;

        public virtual void NotifyFallback( bool isFallback )
        {
            if (isFallback && this.MaximumVersion.IsLaterVersionOf( this.mClientVersion ))
                throw new TlsFatalAlert( 86 );
        }

        public virtual void NotifyOfferedCipherSuites( int[] offeredCipherSuites )
        {
            this.mOfferedCipherSuites = offeredCipherSuites;
            this.mEccCipherSuitesOffered = TlsEccUtilities.ContainsEccCipherSuites( this.mOfferedCipherSuites );
        }

        public virtual void NotifyOfferedCompressionMethods( byte[] offeredCompressionMethods ) => this.mOfferedCompressionMethods = offeredCompressionMethods;

        public virtual void ProcessClientExtensions( IDictionary clientExtensions )
        {
            this.mClientExtensions = clientExtensions;
            if (clientExtensions == null)
                return;
            this.mEncryptThenMacOffered = TlsExtensionsUtilities.HasEncryptThenMacExtension( clientExtensions );
            this.mMaxFragmentLengthOffered = TlsExtensionsUtilities.GetMaxFragmentLengthExtension( clientExtensions );
            if (this.mMaxFragmentLengthOffered >= 0 && !MaxFragmentLength.IsValid( (byte)this.mMaxFragmentLengthOffered ))
                throw new TlsFatalAlert( 47 );
            this.mTruncatedHMacOffered = TlsExtensionsUtilities.HasTruncatedHMacExtension( clientExtensions );
            this.mSupportedSignatureAlgorithms = TlsUtilities.GetSignatureAlgorithmsExtension( clientExtensions );
            if (this.mSupportedSignatureAlgorithms != null && !TlsUtilities.IsSignatureAlgorithmsExtensionAllowed( this.mClientVersion ))
                throw new TlsFatalAlert( 47 );
            this.mNamedCurves = TlsEccUtilities.GetSupportedEllipticCurvesExtension( clientExtensions );
            this.mClientECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension( clientExtensions );
        }

        public virtual ProtocolVersion GetServerVersion()
        {
            if (this.MinimumVersion.IsEqualOrEarlierVersionOf( this.mClientVersion ))
            {
                ProtocolVersion maximumVersion = this.MaximumVersion;
                if (this.mClientVersion.IsEqualOrEarlierVersionOf( maximumVersion ))
                    return this.mServerVersion = this.mClientVersion;
                if (this.mClientVersion.IsLaterVersionOf( maximumVersion ))
                    return this.mServerVersion = maximumVersion;
            }
            throw new TlsFatalAlert( 70 );
        }

        public virtual int GetSelectedCipherSuite()
        {
            bool flag = this.SupportsClientEccCapabilities( this.mNamedCurves, this.mClientECPointFormats );
            foreach (int cipherSuite in this.GetCipherSuites())
            {
                if (Arrays.Contains( this.mOfferedCipherSuites, cipherSuite ) && (flag || !TlsEccUtilities.IsEccCipherSuite( cipherSuite )) && TlsUtilities.IsValidCipherSuiteForVersion( cipherSuite, this.mServerVersion ))
                    return this.mSelectedCipherSuite = cipherSuite;
            }
            throw new TlsFatalAlert( 40 );
        }

        public virtual byte GetSelectedCompressionMethod()
        {
            byte[] compressionMethods = this.GetCompressionMethods();
            for (int index = 0; index < compressionMethods.Length; ++index)
            {
                if (Arrays.Contains( this.mOfferedCompressionMethods, compressionMethods[index] ))
                    return this.mSelectedCompressionMethod = compressionMethods[index];
            }
            throw new TlsFatalAlert( 40 );
        }

        public virtual IDictionary GetServerExtensions()
        {
            if (this.mEncryptThenMacOffered && this.AllowEncryptThenMac && TlsUtilities.IsBlockCipherSuite( this.mSelectedCipherSuite ))
                TlsExtensionsUtilities.AddEncryptThenMacExtension( this.CheckServerExtensions() );
            if (this.mMaxFragmentLengthOffered >= 0 && TlsUtilities.IsValidUint8( mMaxFragmentLengthOffered ) && MaxFragmentLength.IsValid( (byte)this.mMaxFragmentLengthOffered ))
                TlsExtensionsUtilities.AddMaxFragmentLengthExtension( this.CheckServerExtensions(), (byte)this.mMaxFragmentLengthOffered );
            if (this.mTruncatedHMacOffered && this.AllowTruncatedHMac)
                TlsExtensionsUtilities.AddTruncatedHMacExtension( this.CheckServerExtensions() );
            if (this.mClientECPointFormats != null && TlsEccUtilities.IsEccCipherSuite( this.mSelectedCipherSuite ))
            {
                this.mServerECPointFormats = new byte[3]
                {
           0,
           1,
           2
                };
                TlsEccUtilities.AddSupportedPointFormatsExtension( this.CheckServerExtensions(), this.mServerECPointFormats );
            }
            return this.mServerExtensions;
        }

        public virtual IList GetServerSupplementalData() => null;

        public abstract TlsCredentials GetCredentials();

        public virtual CertificateStatus GetCertificateStatus() => null;

        public abstract TlsKeyExchange GetKeyExchange();

        public virtual CertificateRequest GetCertificateRequest() => null;

        public virtual void ProcessClientSupplementalData( IList clientSupplementalData )
        {
            if (clientSupplementalData != null)
                throw new TlsFatalAlert( 10 );
        }

        public virtual void NotifyClientCertificate( Certificate clientCertificate ) => throw new TlsFatalAlert( 80 );

        public override TlsCompression GetCompression()
        {
            if (this.mSelectedCompressionMethod == 0)
                return new TlsNullCompression();
            throw new TlsFatalAlert( 80 );
        }

        public override TlsCipher GetCipher() => this.mCipherFactory.CreateCipher( mContext, TlsUtilities.GetEncryptionAlgorithm( this.mSelectedCipherSuite ), TlsUtilities.GetMacAlgorithm( this.mSelectedCipherSuite ) );

        public virtual NewSessionTicket GetNewSessionTicket() => new NewSessionTicket( 0L, TlsUtilities.EmptyBytes );
    }
}
