// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsServerProtocol
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsServerProtocol : TlsProtocol
    {
        protected TlsServer mTlsServer = null;
        internal TlsServerContextImpl mTlsServerContext = null;
        protected TlsKeyExchange mKeyExchange = null;
        protected TlsCredentials mServerCredentials = null;
        protected CertificateRequest mCertificateRequest = null;
        protected short mClientCertificateType = -1;
        protected TlsHandshakeHash mPrepareFinishHash = null;

        public TlsServerProtocol( Stream stream, SecureRandom secureRandom )
          : base( stream, secureRandom )
        {
        }

        public TlsServerProtocol( Stream input, Stream output, SecureRandom secureRandom )
          : base( input, output, secureRandom )
        {
        }

        public TlsServerProtocol( SecureRandom secureRandom )
          : base( secureRandom )
        {
        }

        public virtual void Accept( TlsServer tlsServer )
        {
            if (tlsServer == null)
                throw new ArgumentNullException( nameof( tlsServer ) );
            this.mTlsServer = this.mTlsServer == null ? tlsServer : throw new InvalidOperationException( "'Accept' can only be called once" );
            this.mSecurityParameters = new SecurityParameters
            {
                entity = 0
            };
            this.mTlsServerContext = new TlsServerContextImpl( this.mSecureRandom, this.mSecurityParameters );
            this.mSecurityParameters.serverRandom = CreateRandomBlock( tlsServer.ShouldUseGmtUnixTime(), this.mTlsServerContext.NonceRandomGenerator );
            this.mTlsServer.Init( mTlsServerContext );
            this.mRecordStream.Init( mTlsServerContext );
            this.mRecordStream.SetRestrictReadVersion( false );
            this.BlockForHandshake();
        }

        protected override void CleanupHandshake()
        {
            base.CleanupHandshake();
            this.mKeyExchange = null;
            this.mServerCredentials = null;
            this.mCertificateRequest = null;
            this.mPrepareFinishHash = null;
        }

        protected override TlsContext Context => mTlsServerContext;

        internal override AbstractTlsContext ContextAdmin => mTlsServerContext;

        protected override TlsPeer Peer => mTlsServer;

        protected override void HandleHandshakeMessage( byte type, byte[] data )
        {
            MemoryStream memoryStream = new( data );
            switch (type)
            {
                case 1:
                    switch (this.mConnectionState)
                    {
                        case 0:
                            this.ReceiveClientHelloMessage( memoryStream );
                            this.mConnectionState = 1;
                            this.SendServerHelloMessage();
                            this.mConnectionState = 2;
                            this.mRecordStream.NotifyHelloComplete();
                            IList supplementalData = this.mTlsServer.GetServerSupplementalData();
                            if (supplementalData != null)
                                this.SendSupplementalDataMessage( supplementalData );
                            this.mConnectionState = 3;
                            this.mKeyExchange = this.mTlsServer.GetKeyExchange();
                            this.mKeyExchange.Init( this.Context );
                            this.mServerCredentials = this.mTlsServer.GetCredentials();
                            Certificate certificate = null;
                            if (this.mServerCredentials == null)
                            {
                                this.mKeyExchange.SkipServerCredentials();
                            }
                            else
                            {
                                this.mKeyExchange.ProcessServerCredentials( this.mServerCredentials );
                                certificate = this.mServerCredentials.Certificate;
                                this.SendCertificateMessage( certificate );
                            }
                            this.mConnectionState = 4;
                            if (certificate == null || certificate.IsEmpty)
                                this.mAllowCertificateStatus = false;
                            if (this.mAllowCertificateStatus)
                            {
                                CertificateStatus certificateStatus = this.mTlsServer.GetCertificateStatus();
                                if (certificateStatus != null)
                                    this.SendCertificateStatusMessage( certificateStatus );
                            }
                            this.mConnectionState = 5;
                            byte[] serverKeyExchange = this.mKeyExchange.GenerateServerKeyExchange();
                            if (serverKeyExchange != null)
                                this.SendServerKeyExchangeMessage( serverKeyExchange );
                            this.mConnectionState = 6;
                            if (this.mServerCredentials != null)
                            {
                                this.mCertificateRequest = this.mTlsServer.GetCertificateRequest();
                                if (this.mCertificateRequest != null)
                                {
                                    if (TlsUtilities.IsTlsV12( this.Context ) != (this.mCertificateRequest.SupportedSignatureAlgorithms != null))
                                        throw new TlsFatalAlert( 80 );
                                    this.mKeyExchange.ValidateCertificateRequest( this.mCertificateRequest );
                                    this.SendCertificateRequestMessage( this.mCertificateRequest );
                                    TlsUtilities.TrackHashAlgorithms( this.mRecordStream.HandshakeHash, this.mCertificateRequest.SupportedSignatureAlgorithms );
                                }
                            }
                            this.mConnectionState = 7;
                            this.SendServerHelloDoneMessage();
                            this.mConnectionState = 8;
                            this.mRecordStream.HandshakeHash.SealHashAlgorithms();
                            return;
                        case 16:
                            this.RefuseRenegotiation();
                            return;
                        default:
                            throw new TlsFatalAlert( 10 );
                    }
                case 11:
                    switch (this.mConnectionState)
                    {
                        case 8:
                        case 9:
                            if (this.mConnectionState < 9)
                                this.mTlsServer.ProcessClientSupplementalData( null );
                            if (this.mCertificateRequest == null)
                                throw new TlsFatalAlert( 10 );
                            this.ReceiveCertificateMessage( memoryStream );
                            this.mConnectionState = 10;
                            return;
                        default:
                            throw new TlsFatalAlert( 10 );
                    }
                case 15:
                    if (this.mConnectionState != 11)
                        throw new TlsFatalAlert( 10 );
                    if (!this.ExpectCertificateVerifyMessage())
                        throw new TlsFatalAlert( 10 );
                    this.ReceiveCertificateVerifyMessage( memoryStream );
                    this.mConnectionState = 12;
                    break;
                case 16:
                    switch (this.mConnectionState)
                    {
                        case 8:
                        case 9:
                        case 10:
                            if (this.mConnectionState < 9)
                                this.mTlsServer.ProcessClientSupplementalData( null );
                            if (this.mConnectionState < 10)
                            {
                                if (this.mCertificateRequest == null)
                                {
                                    this.mKeyExchange.SkipClientCredentials();
                                }
                                else
                                {
                                    if (TlsUtilities.IsTlsV12( this.Context ))
                                        throw new TlsFatalAlert( 10 );
                                    if (TlsUtilities.IsSsl( this.Context ))
                                    {
                                        if (this.mPeerCertificate == null)
                                            throw new TlsFatalAlert( 10 );
                                    }
                                    else
                                        this.NotifyClientCertificate( Certificate.EmptyChain );
                                }
                            }
                            this.ReceiveClientKeyExchangeMessage( memoryStream );
                            this.mConnectionState = 11;
                            return;
                        default:
                            throw new TlsFatalAlert( 10 );
                    }
                case 20:
                    switch (this.mConnectionState)
                    {
                        case 11:
                        case 12:
                            if (this.mConnectionState < 12 && this.ExpectCertificateVerifyMessage())
                                throw new TlsFatalAlert( 10 );
                            this.ProcessFinishedMessage( memoryStream );
                            this.mConnectionState = 13;
                            if (this.mExpectSessionTicket)
                            {
                                this.SendNewSessionTicketMessage( this.mTlsServer.GetNewSessionTicket() );
                                this.SendChangeCipherSpecMessage();
                            }
                            this.mConnectionState = 14;
                            this.SendFinishedMessage();
                            this.mConnectionState = 15;
                            this.mConnectionState = 16;
                            this.CompleteHandshake();
                            return;
                        default:
                            throw new TlsFatalAlert( 10 );
                    }
                case 23:
                    if (this.mConnectionState != 8)
                        throw new TlsFatalAlert( 10 );
                    this.mTlsServer.ProcessClientSupplementalData( ReadSupplementalDataMessage( memoryStream ) );
                    this.mConnectionState = 9;
                    break;
                default:
                    throw new TlsFatalAlert( 10 );
            }
        }

        protected override void HandleWarningMessage( byte description )
        {
            if (description == 41)
            {
                if (!TlsUtilities.IsSsl( this.Context ) || this.mCertificateRequest == null)
                    return;
                this.NotifyClientCertificate( Certificate.EmptyChain );
            }
            else
                base.HandleWarningMessage( description );
        }

        protected virtual void NotifyClientCertificate( Certificate clientCertificate )
        {
            if (this.mCertificateRequest == null)
                throw new InvalidOperationException();
            this.mPeerCertificate = this.mPeerCertificate == null ? clientCertificate : throw new TlsFatalAlert( 10 );
            if (clientCertificate.IsEmpty)
            {
                this.mKeyExchange.SkipClientCredentials();
            }
            else
            {
                this.mClientCertificateType = TlsUtilities.GetClientCertificateType( clientCertificate, this.mServerCredentials.Certificate );
                this.mKeyExchange.ProcessClientCertificate( clientCertificate );
            }
            this.mTlsServer.NotifyClientCertificate( clientCertificate );
        }

        protected virtual void ReceiveCertificateMessage( MemoryStream buf )
        {
            Certificate clientCertificate = Certificate.Parse( buf );
            AssertEmpty( buf );
            this.NotifyClientCertificate( clientCertificate );
        }

        protected virtual void ReceiveCertificateVerifyMessage( MemoryStream buf )
        {
            if (this.mCertificateRequest == null)
                throw new InvalidOperationException();
            DigitallySigned digitallySigned = DigitallySigned.Parse( this.Context, buf );
            AssertEmpty( buf );
            try
            {
                SignatureAndHashAlgorithm algorithm = digitallySigned.Algorithm;
                byte[] hash;
                if (TlsUtilities.IsTlsV12( this.Context ))
                {
                    TlsUtilities.VerifySupportedSignatureAlgorithm( this.mCertificateRequest.SupportedSignatureAlgorithms, algorithm );
                    hash = this.mPrepareFinishHash.GetFinalHash( algorithm.Hash );
                }
                else
                    hash = this.mSecurityParameters.SessionHash;
                AsymmetricKeyParameter key = PublicKeyFactory.CreateKey( this.mPeerCertificate.GetCertificateAt( 0 ).SubjectPublicKeyInfo );
                TlsSigner tlsSigner = TlsUtilities.CreateTlsSigner( (byte)this.mClientCertificateType );
                tlsSigner.Init( this.Context );
                if (!tlsSigner.VerifyRawSignature( algorithm, digitallySigned.Signature, key, hash ))
                    throw new TlsFatalAlert( 51 );
            }
            catch (TlsFatalAlert ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 51, ex );
            }
        }

        protected virtual void ReceiveClientHelloMessage( MemoryStream buf )
        {
            ProtocolVersion protocolVersion = TlsUtilities.ReadVersion( buf );
            this.mRecordStream.SetWriteVersion( protocolVersion );
            if (protocolVersion.IsDtls)
                throw new TlsFatalAlert( 47 );
            byte[] numArray = TlsUtilities.ReadFully( 32, buf );
            int num = TlsUtilities.ReadOpaque8( buf ).Length <= 32 ? TlsUtilities.ReadUint16( buf ) : throw new TlsFatalAlert( 47 );
            this.mOfferedCipherSuites = num >= 2 && (num & 1) == 0 ? TlsUtilities.ReadUint16Array( num / 2, buf ) : throw new TlsFatalAlert( 50 );
            int count = TlsUtilities.ReadUint8( buf );
            this.mOfferedCompressionMethods = count >= 1 ? TlsUtilities.ReadUint8Array( count, buf ) : throw new TlsFatalAlert( 47 );
            this.mClientExtensions = ReadExtensions( buf );
            this.mSecurityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension( this.mClientExtensions );
            this.ContextAdmin.SetClientVersion( protocolVersion );
            this.mTlsServer.NotifyClientVersion( protocolVersion );
            this.mTlsServer.NotifyFallback( Arrays.Contains( this.mOfferedCipherSuites, 22016 ) );
            this.mSecurityParameters.clientRandom = numArray;
            this.mTlsServer.NotifyOfferedCipherSuites( this.mOfferedCipherSuites );
            this.mTlsServer.NotifyOfferedCompressionMethods( this.mOfferedCompressionMethods );
            if (Arrays.Contains( this.mOfferedCipherSuites, byte.MaxValue ))
                this.mSecureRenegotiation = true;
            byte[] extensionData = TlsUtilities.GetExtensionData( this.mClientExtensions, 65281 );
            if (extensionData != null)
            {
                this.mSecureRenegotiation = true;
                if (!Arrays.ConstantTimeAreEqual( extensionData, CreateRenegotiationInfo( TlsUtilities.EmptyBytes ) ))
                    throw new TlsFatalAlert( 40 );
            }
            this.mTlsServer.NotifySecureRenegotiation( this.mSecureRenegotiation );
            if (this.mClientExtensions == null)
                return;
            this.mTlsServer.ProcessClientExtensions( this.mClientExtensions );
        }

        protected virtual void ReceiveClientKeyExchangeMessage( MemoryStream buf )
        {
            this.mKeyExchange.ProcessClientKeyExchange( buf );
            AssertEmpty( buf );
            this.mPrepareFinishHash = this.mRecordStream.PrepareToFinish();
            this.mSecurityParameters.sessionHash = GetCurrentPrfHash( this.Context, this.mPrepareFinishHash, null );
            EstablishMasterSecret( this.Context, this.mKeyExchange );
            this.mRecordStream.SetPendingConnectionState( this.Peer.GetCompression(), this.Peer.GetCipher() );
            if (this.mExpectSessionTicket)
                return;
            this.SendChangeCipherSpecMessage();
        }

        protected virtual void SendCertificateRequestMessage( CertificateRequest certificateRequest )
        {
            TlsProtocol.HandshakeMessage output = new( 13 );
            certificateRequest.Encode( output );
            output.WriteToRecordStream( this );
        }

        protected virtual void SendCertificateStatusMessage( CertificateStatus certificateStatus )
        {
            TlsProtocol.HandshakeMessage output = new( 22 );
            certificateStatus.Encode( output );
            output.WriteToRecordStream( this );
        }

        protected virtual void SendNewSessionTicketMessage( NewSessionTicket newSessionTicket )
        {
            if (newSessionTicket == null)
                throw new TlsFatalAlert( 80 );
            TlsProtocol.HandshakeMessage output = new( 4 );
            newSessionTicket.Encode( output );
            output.WriteToRecordStream( this );
        }

        protected virtual void SendServerHelloMessage()
        {
            TlsProtocol.HandshakeMessage output = new( 2 );
            ProtocolVersion serverVersion = this.mTlsServer.GetServerVersion();
            if (!serverVersion.IsEqualOrEarlierVersionOf( this.Context.ClientVersion ))
                throw new TlsFatalAlert( 80 );
            this.mRecordStream.ReadVersion = serverVersion;
            this.mRecordStream.SetWriteVersion( serverVersion );
            this.mRecordStream.SetRestrictReadVersion( true );
            this.ContextAdmin.SetServerVersion( serverVersion );
            TlsUtilities.WriteVersion( serverVersion, output );
            output.Write( this.mSecurityParameters.serverRandom );
            TlsUtilities.WriteOpaque8( TlsUtilities.EmptyBytes, output );
            int selectedCipherSuite = this.mTlsServer.GetSelectedCipherSuite();
            if (!Arrays.Contains( this.mOfferedCipherSuites, selectedCipherSuite ) || selectedCipherSuite == 0 || CipherSuite.IsScsv( selectedCipherSuite ) || !TlsUtilities.IsValidCipherSuiteForVersion( selectedCipherSuite, this.Context.ServerVersion ))
                throw new TlsFatalAlert( 80 );
            this.mSecurityParameters.cipherSuite = selectedCipherSuite;
            byte compressionMethod = this.mTlsServer.GetSelectedCompressionMethod();
            this.mSecurityParameters.compressionAlgorithm = Arrays.Contains( this.mOfferedCompressionMethods, compressionMethod ) ? compressionMethod : throw new TlsFatalAlert( 80 );
            TlsUtilities.WriteUint16( selectedCipherSuite, output );
            TlsUtilities.WriteUint8( compressionMethod, output );
            this.mServerExtensions = this.mTlsServer.GetServerExtensions();
            if (this.mSecureRenegotiation && null == TlsUtilities.GetExtensionData( this.mServerExtensions, 65281 ))
            {
                this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( this.mServerExtensions );
                this.mServerExtensions[65281] = CreateRenegotiationInfo( TlsUtilities.EmptyBytes );
            }
            if (this.mSecurityParameters.extendedMasterSecret)
            {
                this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( this.mServerExtensions );
                TlsExtensionsUtilities.AddExtendedMasterSecretExtension( this.mServerExtensions );
            }
            if (this.mServerExtensions != null)
            {
                this.mSecurityParameters.encryptThenMac = TlsExtensionsUtilities.HasEncryptThenMacExtension( this.mServerExtensions );
                this.mSecurityParameters.maxFragmentLength = this.ProcessMaxFragmentLengthExtension( this.mClientExtensions, this.mServerExtensions, 80 );
                this.mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension( this.mServerExtensions );
                this.mAllowCertificateStatus = !this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData( this.mServerExtensions, 5, 80 );
                this.mExpectSessionTicket = !this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData( this.mServerExtensions, 35, 80 );
                WriteExtensions( output, this.mServerExtensions );
            }
            this.mSecurityParameters.prfAlgorithm = GetPrfAlgorithm( this.Context, this.mSecurityParameters.CipherSuite );
            this.mSecurityParameters.verifyDataLength = 12;
            this.ApplyMaxFragmentLengthExtension();
            output.WriteToRecordStream( this );
        }

        protected virtual void SendServerHelloDoneMessage()
        {
            byte[] buf = new byte[4];
            TlsUtilities.WriteUint8( 14, buf, 0 );
            TlsUtilities.WriteUint24( 0, buf, 1 );
            this.WriteHandshakeMessage( buf, 0, buf.Length );
        }

        protected virtual void SendServerKeyExchangeMessage( byte[] serverKeyExchange )
        {
            TlsProtocol.HandshakeMessage handshakeMessage = new( 12, serverKeyExchange.Length );
            handshakeMessage.Write( serverKeyExchange );
            handshakeMessage.WriteToRecordStream( this );
        }

        protected virtual bool ExpectCertificateVerifyMessage() => this.mClientCertificateType >= 0 && TlsUtilities.HasSigningCapability( (byte)this.mClientCertificateType );
    }
}
