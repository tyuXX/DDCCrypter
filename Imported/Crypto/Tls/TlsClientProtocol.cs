// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsClientProtocol
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsClientProtocol : TlsProtocol
    {
        protected TlsClient mTlsClient = null;
        internal TlsClientContextImpl mTlsClientContext = null;
        protected byte[] mSelectedSessionID = null;
        protected TlsKeyExchange mKeyExchange = null;
        protected TlsAuthentication mAuthentication = null;
        protected CertificateStatus mCertificateStatus = null;
        protected CertificateRequest mCertificateRequest = null;

        public TlsClientProtocol( Stream stream, SecureRandom secureRandom )
          : base( stream, secureRandom )
        {
        }

        public TlsClientProtocol( Stream input, Stream output, SecureRandom secureRandom )
          : base( input, output, secureRandom )
        {
        }

        public TlsClientProtocol( SecureRandom secureRandom )
          : base( secureRandom )
        {
        }

        public virtual void Connect( TlsClient tlsClient )
        {
            if (tlsClient == null)
                throw new ArgumentNullException( nameof( tlsClient ) );
            this.mTlsClient = this.mTlsClient == null ? tlsClient : throw new InvalidOperationException( "'Connect' can only be called once" );
            this.mSecurityParameters = new SecurityParameters
            {
                entity = 1
            };
            this.mTlsClientContext = new TlsClientContextImpl( this.mSecureRandom, this.mSecurityParameters );
            this.mSecurityParameters.clientRandom = CreateRandomBlock( tlsClient.ShouldUseGmtUnixTime(), this.mTlsClientContext.NonceRandomGenerator );
            this.mTlsClient.Init( mTlsClientContext );
            this.mRecordStream.Init( mTlsClientContext );
            TlsSession sessionToResume = tlsClient.GetSessionToResume();
            if (sessionToResume != null && sessionToResume.IsResumable)
            {
                SessionParameters sessionParameters = sessionToResume.ExportSessionParameters();
                if (sessionParameters != null)
                {
                    this.mTlsSession = sessionToResume;
                    this.mSessionParameters = sessionParameters;
                }
            }
            this.SendClientHelloMessage();
            this.mConnectionState = 1;
            this.BlockForHandshake();
        }

        protected override void CleanupHandshake()
        {
            base.CleanupHandshake();
            this.mSelectedSessionID = null;
            this.mKeyExchange = null;
            this.mAuthentication = null;
            this.mCertificateStatus = null;
            this.mCertificateRequest = null;
        }

        protected override TlsContext Context => mTlsClientContext;

        internal override AbstractTlsContext ContextAdmin => mTlsClientContext;

        protected override TlsPeer Peer => mTlsClient;

        protected override void HandleHandshakeMessage( byte type, byte[] data )
        {
            MemoryStream memoryStream = new( data, false );
            if (this.mResumedSession)
            {
                if (type != 20 || this.mConnectionState != 2)
                    throw new TlsFatalAlert( 10 );
                this.ProcessFinishedMessage( memoryStream );
                this.mConnectionState = 15;
                this.SendFinishedMessage();
                this.mConnectionState = 13;
                this.mConnectionState = 16;
                this.CompleteHandshake();
            }
            else
            {
                switch (type)
                {
                    case 0:
                        AssertEmpty( memoryStream );
                        if (this.mConnectionState != 16)
                            break;
                        this.RefuseRenegotiation();
                        break;
                    case 2:
                        if (this.mConnectionState != 1)
                            throw new TlsFatalAlert( 10 );
                        this.ReceiveServerHelloMessage( memoryStream );
                        this.mConnectionState = 2;
                        this.mRecordStream.NotifyHelloComplete();
                        this.ApplyMaxFragmentLengthExtension();
                        if (this.mResumedSession)
                        {
                            this.mSecurityParameters.masterSecret = Arrays.Clone( this.mSessionParameters.MasterSecret );
                            this.mRecordStream.SetPendingConnectionState( this.Peer.GetCompression(), this.Peer.GetCipher() );
                            this.SendChangeCipherSpecMessage();
                            break;
                        }
                        this.InvalidateSession();
                        if (this.mSelectedSessionID.Length <= 0)
                            break;
                        this.mTlsSession = new TlsSessionImpl( this.mSelectedSessionID, null );
                        break;
                    case 4:
                        if (this.mConnectionState != 13)
                            throw new TlsFatalAlert( 10 );
                        if (!this.mExpectSessionTicket)
                            throw new TlsFatalAlert( 10 );
                        this.InvalidateSession();
                        this.ReceiveNewSessionTicketMessage( memoryStream );
                        this.mConnectionState = 14;
                        break;
                    case 11:
                        switch (this.mConnectionState)
                        {
                            case 2:
                            case 3:
                                if (this.mConnectionState == 2)
                                    this.HandleSupplementalData( null );
                                this.mPeerCertificate = Certificate.Parse( memoryStream );
                                AssertEmpty( memoryStream );
                                if (this.mPeerCertificate == null || this.mPeerCertificate.IsEmpty)
                                    this.mAllowCertificateStatus = false;
                                this.mKeyExchange.ProcessServerCertificate( this.mPeerCertificate );
                                this.mAuthentication = this.mTlsClient.GetAuthentication();
                                this.mAuthentication.NotifyServerCertificate( this.mPeerCertificate );
                                this.mConnectionState = 4;
                                return;
                            default:
                                throw new TlsFatalAlert( 10 );
                        }
                    case 12:
                        switch (this.mConnectionState)
                        {
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                if (this.mConnectionState < 3)
                                    this.HandleSupplementalData( null );
                                if (this.mConnectionState < 4)
                                {
                                    this.mKeyExchange.SkipServerCredentials();
                                    this.mAuthentication = null;
                                }
                                this.mKeyExchange.ProcessServerKeyExchange( memoryStream );
                                AssertEmpty( memoryStream );
                                this.mConnectionState = 6;
                                return;
                            default:
                                throw new TlsFatalAlert( 10 );
                        }
                    case 13:
                        switch (this.mConnectionState)
                        {
                            case 4:
                            case 5:
                            case 6:
                                if (this.mConnectionState != 6)
                                    this.mKeyExchange.SkipServerKeyExchange();
                                if (this.mAuthentication == null)
                                    throw new TlsFatalAlert( 40 );
                                this.mCertificateRequest = CertificateRequest.Parse( this.Context, memoryStream );
                                AssertEmpty( memoryStream );
                                this.mKeyExchange.ValidateCertificateRequest( this.mCertificateRequest );
                                TlsUtilities.TrackHashAlgorithms( this.mRecordStream.HandshakeHash, this.mCertificateRequest.SupportedSignatureAlgorithms );
                                this.mConnectionState = 7;
                                return;
                            default:
                                throw new TlsFatalAlert( 10 );
                        }
                    case 14:
                        switch (this.mConnectionState)
                        {
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (this.mConnectionState < 3)
                                    this.HandleSupplementalData( null );
                                if (this.mConnectionState < 4)
                                {
                                    this.mKeyExchange.SkipServerCredentials();
                                    this.mAuthentication = null;
                                }
                                if (this.mConnectionState < 6)
                                    this.mKeyExchange.SkipServerKeyExchange();
                                AssertEmpty( memoryStream );
                                this.mConnectionState = 8;
                                this.mRecordStream.HandshakeHash.SealHashAlgorithms();
                                IList supplementalData = this.mTlsClient.GetClientSupplementalData();
                                if (supplementalData != null)
                                    this.SendSupplementalDataMessage( supplementalData );
                                this.mConnectionState = 9;
                                TlsCredentials clientCredentials = null;
                                if (this.mCertificateRequest == null)
                                {
                                    this.mKeyExchange.SkipClientCredentials();
                                }
                                else
                                {
                                    clientCredentials = this.mAuthentication.GetClientCredentials( this.mCertificateRequest );
                                    if (clientCredentials == null)
                                    {
                                        this.mKeyExchange.SkipClientCredentials();
                                        this.SendCertificateMessage( Certificate.EmptyChain );
                                    }
                                    else
                                    {
                                        this.mKeyExchange.ProcessClientCredentials( clientCredentials );
                                        this.SendCertificateMessage( clientCredentials.Certificate );
                                    }
                                }
                                this.mConnectionState = 10;
                                this.SendClientKeyExchangeMessage();
                                this.mConnectionState = 11;
                                TlsHandshakeHash finish = this.mRecordStream.PrepareToFinish();
                                this.mSecurityParameters.sessionHash = GetCurrentPrfHash( this.Context, finish, null );
                                EstablishMasterSecret( this.Context, this.mKeyExchange );
                                this.mRecordStream.SetPendingConnectionState( this.Peer.GetCompression(), this.Peer.GetCipher() );
                                if (clientCredentials != null && clientCredentials is TlsSignerCredentials)
                                {
                                    TlsSignerCredentials signerCredentials = (TlsSignerCredentials)clientCredentials;
                                    SignatureAndHashAlgorithm andHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm( this.Context, signerCredentials );
                                    byte[] hash = andHashAlgorithm != null ? finish.GetFinalHash( andHashAlgorithm.Hash ) : this.mSecurityParameters.SessionHash;
                                    byte[] certificateSignature = signerCredentials.GenerateCertificateSignature( hash );
                                    this.SendCertificateVerifyMessage( new DigitallySigned( andHashAlgorithm, certificateSignature ) );
                                    this.mConnectionState = 12;
                                }
                                this.SendChangeCipherSpecMessage();
                                this.SendFinishedMessage();
                                this.mConnectionState = 13;
                                return;
                            default:
                                throw new TlsFatalAlert( 40 );
                        }
                    case 20:
                        switch (this.mConnectionState)
                        {
                            case 13:
                            case 14:
                                if (this.mConnectionState == 13 && this.mExpectSessionTicket)
                                    throw new TlsFatalAlert( 10 );
                                this.ProcessFinishedMessage( memoryStream );
                                this.mConnectionState = 15;
                                this.mConnectionState = 16;
                                this.CompleteHandshake();
                                return;
                            default:
                                throw new TlsFatalAlert( 10 );
                        }
                    case 22:
                        if (this.mConnectionState != 4)
                            throw new TlsFatalAlert( 10 );
                        if (!this.mAllowCertificateStatus)
                            throw new TlsFatalAlert( 10 );
                        this.mCertificateStatus = CertificateStatus.Parse( memoryStream );
                        AssertEmpty( memoryStream );
                        this.mConnectionState = 5;
                        break;
                    case 23:
                        if (this.mConnectionState != 2)
                            throw new TlsFatalAlert( 10 );
                        this.HandleSupplementalData( ReadSupplementalDataMessage( memoryStream ) );
                        break;
                    default:
                        throw new TlsFatalAlert( 10 );
                }
            }
        }

        protected virtual void HandleSupplementalData( IList serverSupplementalData )
        {
            this.mTlsClient.ProcessServerSupplementalData( serverSupplementalData );
            this.mConnectionState = 3;
            this.mKeyExchange = this.mTlsClient.GetKeyExchange();
            this.mKeyExchange.Init( this.Context );
        }

        protected virtual void ReceiveNewSessionTicketMessage( MemoryStream buf )
        {
            NewSessionTicket newSessionTicket = NewSessionTicket.Parse( buf );
            AssertEmpty( buf );
            this.mTlsClient.NotifyNewSessionTicket( newSessionTicket );
        }

        protected virtual void ReceiveServerHelloMessage( MemoryStream buf )
        {
            ProtocolVersion protocolVersion = TlsUtilities.ReadVersion( buf );
            if (protocolVersion.IsDtls)
                throw new TlsFatalAlert( 47 );
            if (!protocolVersion.Equals( this.mRecordStream.ReadVersion ))
                throw new TlsFatalAlert( 47 );
            ProtocolVersion clientVersion = this.Context.ClientVersion;
            if (!protocolVersion.IsEqualOrEarlierVersionOf( clientVersion ))
                throw new TlsFatalAlert( 47 );
            this.mRecordStream.SetWriteVersion( protocolVersion );
            this.ContextAdmin.SetServerVersion( protocolVersion );
            this.mTlsClient.NotifyServerVersion( protocolVersion );
            this.mSecurityParameters.serverRandom = TlsUtilities.ReadFully( 32, buf );
            this.mSelectedSessionID = TlsUtilities.ReadOpaque8( buf );
            if (this.mSelectedSessionID.Length > 32)
                throw new TlsFatalAlert( 47 );
            this.mTlsClient.NotifySessionID( this.mSelectedSessionID );
            this.mResumedSession = this.mSelectedSessionID.Length > 0 && this.mTlsSession != null && Arrays.AreEqual( this.mSelectedSessionID, this.mTlsSession.SessionID );
            int num1 = TlsUtilities.ReadUint16( buf );
            if (!Arrays.Contains( this.mOfferedCipherSuites, num1 ) || num1 == 0 || CipherSuite.IsScsv( num1 ) || !TlsUtilities.IsValidCipherSuiteForVersion( num1, this.Context.ServerVersion ))
                throw new TlsFatalAlert( 47 );
            this.mTlsClient.NotifySelectedCipherSuite( num1 );
            byte num2 = TlsUtilities.ReadUint8( buf );
            if (!Arrays.Contains( this.mOfferedCompressionMethods, num2 ))
                throw new TlsFatalAlert( 47 );
            this.mTlsClient.NotifySelectedCompressionMethod( num2 );
            this.mServerExtensions = ReadExtensions( buf );
            if (this.mServerExtensions != null)
            {
                foreach (int key in (IEnumerable)this.mServerExtensions.Keys)
                {
                    if (key != 65281)
                    {
                        if (TlsUtilities.GetExtensionData( this.mClientExtensions, key ) == null)
                            throw new TlsFatalAlert( 110 );
                        int num3 = this.mResumedSession ? 1 : 0;
                    }
                }
            }
            byte[] extensionData = TlsUtilities.GetExtensionData( this.mServerExtensions, 65281 );
            if (extensionData != null)
            {
                this.mSecureRenegotiation = true;
                if (!Arrays.ConstantTimeAreEqual( extensionData, CreateRenegotiationInfo( TlsUtilities.EmptyBytes ) ))
                    throw new TlsFatalAlert( 40 );
            }
            this.mTlsClient.NotifySecureRenegotiation( this.mSecureRenegotiation );
            IDictionary clientExtensions = this.mClientExtensions;
            IDictionary dictionary = this.mServerExtensions;
            if (this.mResumedSession)
            {
                if (num1 != this.mSessionParameters.CipherSuite || num2 != mSessionParameters.CompressionAlgorithm)
                    throw new TlsFatalAlert( 47 );
                clientExtensions = null;
                dictionary = this.mSessionParameters.ReadServerExtensions();
            }
            this.mSecurityParameters.cipherSuite = num1;
            this.mSecurityParameters.compressionAlgorithm = num2;
            if (dictionary != null)
            {
                bool flag = TlsExtensionsUtilities.HasEncryptThenMacExtension( dictionary );
                this.mSecurityParameters.encryptThenMac = !flag || TlsUtilities.IsBlockCipherSuite( num1 ) ? flag : throw new TlsFatalAlert( 47 );
                this.mSecurityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension( dictionary );
                this.mSecurityParameters.maxFragmentLength = this.ProcessMaxFragmentLengthExtension( clientExtensions, dictionary, 47 );
                this.mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension( dictionary );
                this.mAllowCertificateStatus = !this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData( dictionary, 5, 47 );
                this.mExpectSessionTicket = !this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData( dictionary, 35, 47 );
            }
            if (clientExtensions != null)
                this.mTlsClient.ProcessServerExtensions( dictionary );
            this.mSecurityParameters.prfAlgorithm = GetPrfAlgorithm( this.Context, this.mSecurityParameters.CipherSuite );
            this.mSecurityParameters.verifyDataLength = 12;
        }

        protected virtual void SendCertificateVerifyMessage( DigitallySigned certificateVerify )
        {
            TlsProtocol.HandshakeMessage output = new( 15 );
            certificateVerify.Encode( output );
            output.WriteToRecordStream( this );
        }

        protected virtual void SendClientHelloMessage()
        {
            this.mRecordStream.SetWriteVersion( this.mTlsClient.ClientHelloRecordLayerVersion );
            ProtocolVersion clientVersion = this.mTlsClient.ClientVersion;
            if (clientVersion.IsDtls)
                throw new TlsFatalAlert( 80 );
            this.ContextAdmin.SetClientVersion( clientVersion );
            byte[] buf = TlsUtilities.EmptyBytes;
            if (this.mTlsSession != null)
            {
                buf = this.mTlsSession.SessionID;
                if (buf == null || buf.Length > 32)
                    buf = TlsUtilities.EmptyBytes;
            }
            bool isFallback = this.mTlsClient.IsFallback;
            this.mOfferedCipherSuites = this.mTlsClient.GetCipherSuites();
            this.mOfferedCompressionMethods = this.mTlsClient.GetCompressionMethods();
            if (buf.Length > 0 && this.mSessionParameters != null && (!Arrays.Contains( this.mOfferedCipherSuites, this.mSessionParameters.CipherSuite ) || !Arrays.Contains( this.mOfferedCompressionMethods, this.mSessionParameters.CompressionAlgorithm )))
                buf = TlsUtilities.EmptyBytes;
            this.mClientExtensions = this.mTlsClient.GetClientExtensions();
            TlsProtocol.HandshakeMessage output = new( 1 );
            TlsUtilities.WriteVersion( clientVersion, output );
            output.Write( this.mSecurityParameters.ClientRandom );
            TlsUtilities.WriteOpaque8( buf, output );
            bool flag1 = null == TlsUtilities.GetExtensionData( this.mClientExtensions, 65281 );
            bool flag2 = !Arrays.Contains( this.mOfferedCipherSuites, byte.MaxValue );
            if (flag1 && flag2)
                this.mOfferedCipherSuites = Arrays.Append( this.mOfferedCipherSuites, byte.MaxValue );
            if (isFallback && !Arrays.Contains( this.mOfferedCipherSuites, 22016 ))
                this.mOfferedCipherSuites = Arrays.Append( this.mOfferedCipherSuites, 22016 );
            TlsUtilities.WriteUint16ArrayWithUint16Length( this.mOfferedCipherSuites, output );
            TlsUtilities.WriteUint8ArrayWithUint8Length( this.mOfferedCompressionMethods, output );
            if (this.mClientExtensions != null)
                WriteExtensions( output, this.mClientExtensions );
            output.WriteToRecordStream( this );
        }

        protected virtual void SendClientKeyExchangeMessage()
        {
            TlsProtocol.HandshakeMessage output = new( 16 );
            this.mKeyExchange.GenerateClientKeyExchange( output );
            output.WriteToRecordStream( this );
        }
    }
}
