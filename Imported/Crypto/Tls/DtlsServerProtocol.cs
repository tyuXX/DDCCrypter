// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsServerProtocol
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DtlsServerProtocol : DtlsProtocol
    {
        protected bool mVerifyRequests = true;

        public DtlsServerProtocol( SecureRandom secureRandom )
          : base( secureRandom )
        {
        }

        public virtual bool VerifyRequests
        {
            get => this.mVerifyRequests;
            set => this.mVerifyRequests = value;
        }

        public virtual DtlsTransport Accept( TlsServer server, DatagramTransport transport )
        {
            if (server == null)
                throw new ArgumentNullException( nameof( server ) );
            if (transport == null)
                throw new ArgumentNullException( nameof( transport ) );
            SecurityParameters securityParameters = new()
            {
                entity = 0
            };
            DtlsServerProtocol.ServerHandshakeState state = new()
            {
                server = server,
                serverContext = new TlsServerContextImpl( this.mSecureRandom, securityParameters )
            };
            securityParameters.serverRandom = TlsProtocol.CreateRandomBlock( server.ShouldUseGmtUnixTime(), state.serverContext.NonceRandomGenerator );
            server.Init( state.serverContext );
            DtlsRecordLayer recordLayer = new( transport, state.serverContext, server, 22 );
            try
            {
                return this.ServerHandshake( state, recordLayer );
            }
            catch (TlsFatalAlert ex)
            {
                recordLayer.Fail( ex.AlertDescription );
                throw ex;
            }
            catch (IOException ex)
            {
                recordLayer.Fail( 80 );
                throw ex;
            }
            catch (Exception ex)
            {
                recordLayer.Fail( 80 );
                throw new TlsFatalAlert( 80, ex );
            }
        }

        internal virtual DtlsTransport ServerHandshake(
          DtlsServerProtocol.ServerHandshakeState state,
          DtlsRecordLayer recordLayer )
        {
            SecurityParameters securityParameters = state.serverContext.SecurityParameters;
            DtlsReliableHandshake reliableHandshake = new( state.serverContext, recordLayer );
            DtlsReliableHandshake.Message message1 = reliableHandshake.ReceiveMessage();
            if (message1.Type != 1)
                throw new TlsFatalAlert( 10 );
            this.ProcessClientHello( state, message1.Body );
            byte[] serverHello = this.GenerateServerHello( state );
            ApplyMaxFragmentLengthExtension( recordLayer, securityParameters.maxFragmentLength );
            ProtocolVersion serverVersion = state.serverContext.ServerVersion;
            recordLayer.ReadVersion = serverVersion;
            recordLayer.SetWriteVersion( serverVersion );
            reliableHandshake.SendMessage( 2, serverHello );
            reliableHandshake.NotifyHelloComplete();
            IList supplementalData1 = state.server.GetServerSupplementalData();
            if (supplementalData1 != null)
            {
                byte[] supplementalData2 = GenerateSupplementalData( supplementalData1 );
                reliableHandshake.SendMessage( 23, supplementalData2 );
            }
            state.keyExchange = state.server.GetKeyExchange();
            state.keyExchange.Init( state.serverContext );
            state.serverCredentials = state.server.GetCredentials();
            Certificate certificate1 = null;
            if (state.serverCredentials == null)
            {
                state.keyExchange.SkipServerCredentials();
            }
            else
            {
                state.keyExchange.ProcessServerCredentials( state.serverCredentials );
                certificate1 = state.serverCredentials.Certificate;
                byte[] certificate2 = GenerateCertificate( certificate1 );
                reliableHandshake.SendMessage( 11, certificate2 );
            }
            if (certificate1 == null || certificate1.IsEmpty)
                state.allowCertificateStatus = false;
            if (state.allowCertificateStatus)
            {
                CertificateStatus certificateStatus1 = state.server.GetCertificateStatus();
                if (certificateStatus1 != null)
                {
                    byte[] certificateStatus2 = this.GenerateCertificateStatus( state, certificateStatus1 );
                    reliableHandshake.SendMessage( 22, certificateStatus2 );
                }
            }
            byte[] serverKeyExchange = state.keyExchange.GenerateServerKeyExchange();
            if (serverKeyExchange != null)
                reliableHandshake.SendMessage( 12, serverKeyExchange );
            if (state.serverCredentials != null)
            {
                state.certificateRequest = state.server.GetCertificateRequest();
                if (state.certificateRequest != null)
                {
                    if (TlsUtilities.IsTlsV12( state.serverContext ) != (state.certificateRequest.SupportedSignatureAlgorithms != null))
                        throw new TlsFatalAlert( 80 );
                    state.keyExchange.ValidateCertificateRequest( state.certificateRequest );
                    byte[] certificateRequest = this.GenerateCertificateRequest( state, state.certificateRequest );
                    reliableHandshake.SendMessage( 13, certificateRequest );
                    TlsUtilities.TrackHashAlgorithms( reliableHandshake.HandshakeHash, state.certificateRequest.SupportedSignatureAlgorithms );
                }
            }
            reliableHandshake.SendMessage( 14, TlsUtilities.EmptyBytes );
            reliableHandshake.HandshakeHash.SealHashAlgorithms();
            DtlsReliableHandshake.Message message2 = reliableHandshake.ReceiveMessage();
            if (message2.Type == 23)
            {
                this.ProcessClientSupplementalData( state, message2.Body );
                message2 = reliableHandshake.ReceiveMessage();
            }
            else
                state.server.ProcessClientSupplementalData( null );
            if (state.certificateRequest == null)
                state.keyExchange.SkipClientCredentials();
            else if (message2.Type == 11)
            {
                this.ProcessClientCertificate( state, message2.Body );
                message2 = reliableHandshake.ReceiveMessage();
            }
            else
            {
                if (TlsUtilities.IsTlsV12( state.serverContext ))
                    throw new TlsFatalAlert( 10 );
                this.NotifyClientCertificate( state, Certificate.EmptyChain );
            }
            if (message2.Type != 16)
                throw new TlsFatalAlert( 10 );
            this.ProcessClientKeyExchange( state, message2.Body );
            TlsHandshakeHash finish = reliableHandshake.PrepareToFinish();
            securityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash( state.serverContext, finish, null );
            TlsProtocol.EstablishMasterSecret( state.serverContext, state.keyExchange );
            recordLayer.InitPendingEpoch( state.server.GetCipher() );
            if (this.ExpectCertificateVerifyMessage( state ))
            {
                byte[] messageBody = reliableHandshake.ReceiveMessageBody( 15 );
                this.ProcessCertificateVerify( state, messageBody, finish );
            }
            byte[] verifyData1 = TlsUtilities.CalculateVerifyData( state.serverContext, "client finished", TlsProtocol.GetCurrentPrfHash( state.serverContext, reliableHandshake.HandshakeHash, null ) );
            this.ProcessFinished( reliableHandshake.ReceiveMessageBody( 20 ), verifyData1 );
            if (state.expectSessionTicket)
            {
                NewSessionTicket newSessionTicket1 = state.server.GetNewSessionTicket();
                byte[] newSessionTicket2 = this.GenerateNewSessionTicket( state, newSessionTicket1 );
                reliableHandshake.SendMessage( 4, newSessionTicket2 );
            }
            byte[] verifyData2 = TlsUtilities.CalculateVerifyData( state.serverContext, "server finished", TlsProtocol.GetCurrentPrfHash( state.serverContext, reliableHandshake.HandshakeHash, null ) );
            reliableHandshake.SendMessage( 20, verifyData2 );
            reliableHandshake.Finish();
            state.server.NotifyHandshakeComplete();
            return new DtlsTransport( recordLayer );
        }

        protected virtual byte[] GenerateCertificateRequest(
          DtlsServerProtocol.ServerHandshakeState state,
          CertificateRequest certificateRequest )
        {
            MemoryStream output = new();
            certificateRequest.Encode( output );
            return output.ToArray();
        }

        protected virtual byte[] GenerateCertificateStatus(
          DtlsServerProtocol.ServerHandshakeState state,
          CertificateStatus certificateStatus )
        {
            MemoryStream output = new();
            certificateStatus.Encode( output );
            return output.ToArray();
        }

        protected virtual byte[] GenerateNewSessionTicket(
          DtlsServerProtocol.ServerHandshakeState state,
          NewSessionTicket newSessionTicket )
        {
            MemoryStream output = new();
            newSessionTicket.Encode( output );
            return output.ToArray();
        }

        protected virtual byte[] GenerateServerHello( DtlsServerProtocol.ServerHandshakeState state )
        {
            SecurityParameters securityParameters = state.serverContext.SecurityParameters;
            MemoryStream output = new();
            ProtocolVersion serverVersion = state.server.GetServerVersion();
            if (!serverVersion.IsEqualOrEarlierVersionOf( state.serverContext.ClientVersion ))
                throw new TlsFatalAlert( 80 );
            state.serverContext.SetServerVersion( serverVersion );
            TlsUtilities.WriteVersion( state.serverContext.ServerVersion, output );
            output.Write( securityParameters.ServerRandom, 0, securityParameters.ServerRandom.Length );
            TlsUtilities.WriteOpaque8( TlsUtilities.EmptyBytes, output );
            int selectedCipherSuite = state.server.GetSelectedCipherSuite();
            if (!Arrays.Contains( state.offeredCipherSuites, selectedCipherSuite ) || selectedCipherSuite == 0 || CipherSuite.IsScsv( selectedCipherSuite ) || !TlsUtilities.IsValidCipherSuiteForVersion( selectedCipherSuite, state.serverContext.ServerVersion ))
                throw new TlsFatalAlert( 80 );
            ValidateSelectedCipherSuite( selectedCipherSuite, 80 );
            securityParameters.cipherSuite = selectedCipherSuite;
            byte compressionMethod = state.server.GetSelectedCompressionMethod();
            if (!Arrays.Contains( state.offeredCompressionMethods, compressionMethod ))
                throw new TlsFatalAlert( 80 );
            securityParameters.compressionAlgorithm = compressionMethod;
            TlsUtilities.WriteUint16( selectedCipherSuite, output );
            TlsUtilities.WriteUint8( compressionMethod, output );
            state.serverExtensions = state.server.GetServerExtensions();
            if (state.secure_renegotiation && null == TlsUtilities.GetExtensionData( state.serverExtensions, 65281 ))
            {
                state.serverExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( state.serverExtensions );
                state.serverExtensions[65281] = TlsProtocol.CreateRenegotiationInfo( TlsUtilities.EmptyBytes );
            }
            if (securityParameters.extendedMasterSecret)
            {
                state.serverExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised( state.serverExtensions );
                TlsExtensionsUtilities.AddExtendedMasterSecretExtension( state.serverExtensions );
            }
            if (state.serverExtensions != null)
            {
                securityParameters.encryptThenMac = TlsExtensionsUtilities.HasEncryptThenMacExtension( state.serverExtensions );
                securityParameters.maxFragmentLength = EvaluateMaxFragmentLengthExtension( state.resumedSession, state.clientExtensions, state.serverExtensions, 80 );
                securityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension( state.serverExtensions );
                state.allowCertificateStatus = !state.resumedSession && TlsUtilities.HasExpectedEmptyExtensionData( state.serverExtensions, 5, 80 );
                state.expectSessionTicket = !state.resumedSession && TlsUtilities.HasExpectedEmptyExtensionData( state.serverExtensions, 35, 80 );
                TlsProtocol.WriteExtensions( output, state.serverExtensions );
            }
            securityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm( state.serverContext, securityParameters.CipherSuite );
            securityParameters.verifyDataLength = 12;
            return output.ToArray();
        }

        protected virtual void NotifyClientCertificate(
          DtlsServerProtocol.ServerHandshakeState state,
          Certificate clientCertificate )
        {
            if (state.certificateRequest == null)
                throw new InvalidOperationException();
            state.clientCertificate = state.clientCertificate == null ? clientCertificate : throw new TlsFatalAlert( 10 );
            if (clientCertificate.IsEmpty)
            {
                state.keyExchange.SkipClientCredentials();
            }
            else
            {
                state.clientCertificateType = TlsUtilities.GetClientCertificateType( clientCertificate, state.serverCredentials.Certificate );
                state.keyExchange.ProcessClientCertificate( clientCertificate );
            }
            state.server.NotifyClientCertificate( clientCertificate );
        }

        protected virtual void ProcessClientCertificate(
          DtlsServerProtocol.ServerHandshakeState state,
          byte[] body )
        {
            MemoryStream memoryStream = new( body, false );
            Certificate clientCertificate = Certificate.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            this.NotifyClientCertificate( state, clientCertificate );
        }

        protected virtual void ProcessCertificateVerify(
          DtlsServerProtocol.ServerHandshakeState state,
          byte[] body,
          TlsHandshakeHash prepareFinishHash )
        {
            if (state.certificateRequest == null)
                throw new InvalidOperationException();
            MemoryStream memoryStream = new( body, false );
            TlsServerContextImpl serverContext = state.serverContext;
            DigitallySigned digitallySigned = DigitallySigned.Parse( serverContext, memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            try
            {
                SignatureAndHashAlgorithm algorithm = digitallySigned.Algorithm;
                byte[] hash;
                if (TlsUtilities.IsTlsV12( serverContext ))
                {
                    TlsUtilities.VerifySupportedSignatureAlgorithm( state.certificateRequest.SupportedSignatureAlgorithms, algorithm );
                    hash = prepareFinishHash.GetFinalHash( algorithm.Hash );
                }
                else
                    hash = serverContext.SecurityParameters.SessionHash;
                AsymmetricKeyParameter key = PublicKeyFactory.CreateKey( state.clientCertificate.GetCertificateAt( 0 ).SubjectPublicKeyInfo );
                TlsSigner tlsSigner = TlsUtilities.CreateTlsSigner( (byte)state.clientCertificateType );
                tlsSigner.Init( serverContext );
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

        protected virtual void ProcessClientHello(
          DtlsServerProtocol.ServerHandshakeState state,
          byte[] body )
        {
            MemoryStream input = new( body, false );
            ProtocolVersion clientVersion = TlsUtilities.ReadVersion( input );
            if (!clientVersion.IsDtls)
                throw new TlsFatalAlert( 47 );
            byte[] numArray = TlsUtilities.ReadFully( 32, input );
            if (TlsUtilities.ReadOpaque8( input ).Length > 32)
                throw new TlsFatalAlert( 47 );
            TlsUtilities.ReadOpaque8( input );
            int num = TlsUtilities.ReadUint16( input );
            state.offeredCipherSuites = num >= 2 && (num & 1) == 0 ? TlsUtilities.ReadUint16Array( num / 2, input ) : throw new TlsFatalAlert( 50 );
            int count = TlsUtilities.ReadUint8( input );
            state.offeredCompressionMethods = count >= 1 ? TlsUtilities.ReadUint8Array( count, input ) : throw new TlsFatalAlert( 47 );
            state.clientExtensions = TlsProtocol.ReadExtensions( input );
            TlsServerContextImpl serverContext = state.serverContext;
            SecurityParameters securityParameters = serverContext.SecurityParameters;
            securityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension( state.clientExtensions );
            serverContext.SetClientVersion( clientVersion );
            state.server.NotifyClientVersion( clientVersion );
            state.server.NotifyFallback( Arrays.Contains( state.offeredCipherSuites, 22016 ) );
            securityParameters.clientRandom = numArray;
            state.server.NotifyOfferedCipherSuites( state.offeredCipherSuites );
            state.server.NotifyOfferedCompressionMethods( state.offeredCompressionMethods );
            if (Arrays.Contains( state.offeredCipherSuites, byte.MaxValue ))
                state.secure_renegotiation = true;
            byte[] extensionData = TlsUtilities.GetExtensionData( state.clientExtensions, 65281 );
            if (extensionData != null)
            {
                state.secure_renegotiation = true;
                if (!Arrays.ConstantTimeAreEqual( extensionData, TlsProtocol.CreateRenegotiationInfo( TlsUtilities.EmptyBytes ) ))
                    throw new TlsFatalAlert( 40 );
            }
            state.server.NotifySecureRenegotiation( state.secure_renegotiation );
            if (state.clientExtensions == null)
                return;
            state.server.ProcessClientExtensions( state.clientExtensions );
        }

        protected virtual void ProcessClientKeyExchange(
          DtlsServerProtocol.ServerHandshakeState state,
          byte[] body )
        {
            MemoryStream memoryStream = new( body, false );
            state.keyExchange.ProcessClientKeyExchange( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
        }

        protected virtual void ProcessClientSupplementalData(
          DtlsServerProtocol.ServerHandshakeState state,
          byte[] body )
        {
            IList clientSupplementalData = TlsProtocol.ReadSupplementalDataMessage( new MemoryStream( body, false ) );
            state.server.ProcessClientSupplementalData( clientSupplementalData );
        }

        protected virtual bool ExpectCertificateVerifyMessage(
          DtlsServerProtocol.ServerHandshakeState state )
        {
            return state.clientCertificateType >= 0 && TlsUtilities.HasSigningCapability( (byte)state.clientCertificateType );
        }

        protected internal class ServerHandshakeState
        {
            internal TlsServer server = null;
            internal TlsServerContextImpl serverContext = null;
            internal int[] offeredCipherSuites = null;
            internal byte[] offeredCompressionMethods = null;
            internal IDictionary clientExtensions = null;
            internal IDictionary serverExtensions = null;
            internal bool resumedSession = false;
            internal bool secure_renegotiation = false;
            internal bool allowCertificateStatus = false;
            internal bool expectSessionTicket = false;
            internal TlsKeyExchange keyExchange = null;
            internal TlsCredentials serverCredentials = null;
            internal CertificateRequest certificateRequest = null;
            internal short clientCertificateType = -1;
            internal Certificate clientCertificate = null;
        }
    }
}
