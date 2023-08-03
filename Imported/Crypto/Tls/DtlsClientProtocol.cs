// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsClientProtocol
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DtlsClientProtocol : DtlsProtocol
    {
        public DtlsClientProtocol( SecureRandom secureRandom )
          : base( secureRandom )
        {
        }

        public virtual DtlsTransport Connect( TlsClient client, DatagramTransport transport )
        {
            if (client == null)
                throw new ArgumentNullException( nameof( client ) );
            if (transport == null)
                throw new ArgumentNullException( nameof( transport ) );
            SecurityParameters securityParameters = new()
            {
                entity = 1
            };
            DtlsClientProtocol.ClientHandshakeState state = new()
            {
                client = client,
                clientContext = new TlsClientContextImpl( this.mSecureRandom, securityParameters )
            };
            securityParameters.clientRandom = TlsProtocol.CreateRandomBlock( client.ShouldUseGmtUnixTime(), state.clientContext.NonceRandomGenerator );
            client.Init( state.clientContext );
            DtlsRecordLayer recordLayer = new( transport, state.clientContext, client, 22 );
            TlsSession sessionToResume = state.client.GetSessionToResume();
            if (sessionToResume != null)
            {
                if (sessionToResume.IsResumable)
                {
                    SessionParameters sessionParameters = sessionToResume.ExportSessionParameters();
                    if (sessionParameters != null)
                    {
                        state.tlsSession = sessionToResume;
                        state.sessionParameters = sessionParameters;
                    }
                }
            }
            try
            {
                return this.ClientHandshake( state, recordLayer );
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

        internal virtual DtlsTransport ClientHandshake(
          DtlsClientProtocol.ClientHandshakeState state,
          DtlsRecordLayer recordLayer )
        {
            SecurityParameters securityParameters = state.clientContext.SecurityParameters;
            DtlsReliableHandshake reliableHandshake = new( state.clientContext, recordLayer );
            byte[] clientHello = this.GenerateClientHello( state, state.client );
            recordLayer.SetWriteVersion( ProtocolVersion.DTLSv10 );
            reliableHandshake.SendMessage( 1, clientHello );
            DtlsReliableHandshake.Message message1;
            for (message1 = reliableHandshake.ReceiveMessage(); message1.Type == 3; message1 = reliableHandshake.ReceiveMessage())
            {
                if (!recordLayer.ReadVersion.IsEqualOrEarlierVersionOf( state.clientContext.ClientVersion ))
                    throw new TlsFatalAlert( 47 );
                recordLayer.ReadVersion = null;
                byte[] cookie = this.ProcessHelloVerifyRequest( state, message1.Body );
                byte[] body = PatchClientHelloWithCookie( clientHello, cookie );
                reliableHandshake.ResetHandshakeMessagesDigest();
                reliableHandshake.SendMessage( 1, body );
            }
            if (message1.Type != 2)
                throw new TlsFatalAlert( 10 );
            ProtocolVersion readVersion = recordLayer.ReadVersion;
            this.ReportServerVersion( state, readVersion );
            recordLayer.SetWriteVersion( readVersion );
            this.ProcessServerHello( state, message1.Body );
            reliableHandshake.NotifyHelloComplete();
            ApplyMaxFragmentLengthExtension( recordLayer, securityParameters.maxFragmentLength );
            if (state.resumedSession)
            {
                securityParameters.masterSecret = Arrays.Clone( state.sessionParameters.MasterSecret );
                recordLayer.InitPendingEpoch( state.client.GetCipher() );
                byte[] verifyData1 = TlsUtilities.CalculateVerifyData( state.clientContext, "server finished", TlsProtocol.GetCurrentPrfHash( state.clientContext, reliableHandshake.HandshakeHash, null ) );
                this.ProcessFinished( reliableHandshake.ReceiveMessageBody( 20 ), verifyData1 );
                byte[] verifyData2 = TlsUtilities.CalculateVerifyData( state.clientContext, "client finished", TlsProtocol.GetCurrentPrfHash( state.clientContext, reliableHandshake.HandshakeHash, null ) );
                reliableHandshake.SendMessage( 20, verifyData2 );
                reliableHandshake.Finish();
                state.clientContext.SetResumableSession( state.tlsSession );
                state.client.NotifyHandshakeComplete();
                return new DtlsTransport( recordLayer );
            }
            this.InvalidateSession( state );
            if (state.selectedSessionID.Length > 0)
                state.tlsSession = new TlsSessionImpl( state.selectedSessionID, null );
            DtlsReliableHandshake.Message message2 = reliableHandshake.ReceiveMessage();
            if (message2.Type == 23)
            {
                this.ProcessServerSupplementalData( state, message2.Body );
                message2 = reliableHandshake.ReceiveMessage();
            }
            else
                state.client.ProcessServerSupplementalData( null );
            state.keyExchange = state.client.GetKeyExchange();
            state.keyExchange.Init( state.clientContext );
            Certificate peerCertificate = null;
            if (message2.Type == 11)
            {
                peerCertificate = this.ProcessServerCertificate( state, message2.Body );
                message2 = reliableHandshake.ReceiveMessage();
            }
            else
                state.keyExchange.SkipServerCredentials();
            if (peerCertificate == null || peerCertificate.IsEmpty)
                state.allowCertificateStatus = false;
            if (message2.Type == 22)
            {
                this.ProcessCertificateStatus( state, message2.Body );
                message2 = reliableHandshake.ReceiveMessage();
            }
            if (message2.Type == 12)
            {
                this.ProcessServerKeyExchange( state, message2.Body );
                message2 = reliableHandshake.ReceiveMessage();
            }
            else
                state.keyExchange.SkipServerKeyExchange();
            if (message2.Type == 13)
            {
                this.ProcessCertificateRequest( state, message2.Body );
                TlsUtilities.TrackHashAlgorithms( reliableHandshake.HandshakeHash, state.certificateRequest.SupportedSignatureAlgorithms );
                message2 = reliableHandshake.ReceiveMessage();
            }
            if (message2.Type != 14)
                throw new TlsFatalAlert( 10 );
            if (message2.Body.Length != 0)
                throw new TlsFatalAlert( 50 );
            reliableHandshake.HandshakeHash.SealHashAlgorithms();
            IList supplementalData1 = state.client.GetClientSupplementalData();
            if (supplementalData1 != null)
            {
                byte[] supplementalData2 = GenerateSupplementalData( supplementalData1 );
                reliableHandshake.SendMessage( 23, supplementalData2 );
            }
            if (state.certificateRequest != null)
            {
                state.clientCredentials = state.authentication.GetClientCredentials( state.certificateRequest );
                Certificate certificate1 = null;
                if (state.clientCredentials != null)
                    certificate1 = state.clientCredentials.Certificate;
                if (certificate1 == null)
                    certificate1 = Certificate.EmptyChain;
                byte[] certificate2 = GenerateCertificate( certificate1 );
                reliableHandshake.SendMessage( 11, certificate2 );
            }
            if (state.clientCredentials != null)
                state.keyExchange.ProcessClientCredentials( state.clientCredentials );
            else
                state.keyExchange.SkipClientCredentials();
            byte[] clientKeyExchange = this.GenerateClientKeyExchange( state );
            reliableHandshake.SendMessage( 16, clientKeyExchange );
            TlsHandshakeHash finish = reliableHandshake.PrepareToFinish();
            securityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash( state.clientContext, finish, null );
            TlsProtocol.EstablishMasterSecret( state.clientContext, state.keyExchange );
            recordLayer.InitPendingEpoch( state.client.GetCipher() );
            if (state.clientCredentials != null && state.clientCredentials is TlsSignerCredentials)
            {
                TlsSignerCredentials clientCredentials = (TlsSignerCredentials)state.clientCredentials;
                SignatureAndHashAlgorithm andHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm( state.clientContext, clientCredentials );
                byte[] hash = andHashAlgorithm != null ? finish.GetFinalHash( andHashAlgorithm.Hash ) : securityParameters.SessionHash;
                byte[] certificateSignature = clientCredentials.GenerateCertificateSignature( hash );
                DigitallySigned certificateVerify1 = new( andHashAlgorithm, certificateSignature );
                byte[] certificateVerify2 = this.GenerateCertificateVerify( state, certificateVerify1 );
                reliableHandshake.SendMessage( 15, certificateVerify2 );
            }
            byte[] verifyData3 = TlsUtilities.CalculateVerifyData( state.clientContext, "client finished", TlsProtocol.GetCurrentPrfHash( state.clientContext, reliableHandshake.HandshakeHash, null ) );
            reliableHandshake.SendMessage( 20, verifyData3 );
            if (state.expectSessionTicket)
            {
                DtlsReliableHandshake.Message message3 = reliableHandshake.ReceiveMessage();
                if (message3.Type != 4)
                    throw new TlsFatalAlert( 10 );
                this.ProcessNewSessionTicket( state, message3.Body );
            }
            byte[] verifyData4 = TlsUtilities.CalculateVerifyData( state.clientContext, "server finished", TlsProtocol.GetCurrentPrfHash( state.clientContext, reliableHandshake.HandshakeHash, null ) );
            this.ProcessFinished( reliableHandshake.ReceiveMessageBody( 20 ), verifyData4 );
            reliableHandshake.Finish();
            if (state.tlsSession != null)
            {
                state.sessionParameters = new SessionParameters.Builder().SetCipherSuite( securityParameters.CipherSuite ).SetCompressionAlgorithm( securityParameters.CompressionAlgorithm ).SetMasterSecret( securityParameters.MasterSecret ).SetPeerCertificate( peerCertificate ).SetPskIdentity( securityParameters.PskIdentity ).SetSrpIdentity( securityParameters.SrpIdentity ).SetServerExtensions( state.serverExtensions ).Build();
                state.tlsSession = TlsUtilities.ImportSession( state.tlsSession.SessionID, state.sessionParameters );
                state.clientContext.SetResumableSession( state.tlsSession );
            }
            state.client.NotifyHandshakeComplete();
            return new DtlsTransport( recordLayer );
        }

        protected virtual byte[] GenerateCertificateVerify(
          DtlsClientProtocol.ClientHandshakeState state,
          DigitallySigned certificateVerify )
        {
            MemoryStream output = new();
            certificateVerify.Encode( output );
            return output.ToArray();
        }

        protected virtual byte[] GenerateClientHello(
          DtlsClientProtocol.ClientHandshakeState state,
          TlsClient client )
        {
            MemoryStream output = new();
            ProtocolVersion clientVersion = client.ClientVersion;
            if (!clientVersion.IsDtls)
                throw new TlsFatalAlert( 80 );
            TlsClientContextImpl clientContext = state.clientContext;
            clientContext.SetClientVersion( clientVersion );
            TlsUtilities.WriteVersion( clientVersion, output );
            SecurityParameters securityParameters = clientContext.SecurityParameters;
            output.Write( securityParameters.ClientRandom, 0, securityParameters.ClientRandom.Length );
            byte[] buf = TlsUtilities.EmptyBytes;
            if (state.tlsSession != null)
            {
                buf = state.tlsSession.SessionID;
                if (buf == null || buf.Length > 32)
                    buf = TlsUtilities.EmptyBytes;
            }
            TlsUtilities.WriteOpaque8( buf, output );
            TlsUtilities.WriteOpaque8( TlsUtilities.EmptyBytes, output );
            bool isFallback = client.IsFallback;
            state.offeredCipherSuites = client.GetCipherSuites();
            state.clientExtensions = client.GetClientExtensions();
            bool flag1 = null == TlsUtilities.GetExtensionData( state.clientExtensions, 65281 );
            bool flag2 = !Arrays.Contains( state.offeredCipherSuites, byte.MaxValue );
            if (flag1 && flag2)
                state.offeredCipherSuites = Arrays.Append( state.offeredCipherSuites, byte.MaxValue );
            if (isFallback && !Arrays.Contains( state.offeredCipherSuites, 22016 ))
                state.offeredCipherSuites = Arrays.Append( state.offeredCipherSuites, 22016 );
            TlsUtilities.WriteUint16ArrayWithUint16Length( state.offeredCipherSuites, output );
            state.offeredCompressionMethods = new byte[1];
            TlsUtilities.WriteUint8ArrayWithUint8Length( state.offeredCompressionMethods, output );
            if (state.clientExtensions != null)
                TlsProtocol.WriteExtensions( output, state.clientExtensions );
            return output.ToArray();
        }

        protected virtual byte[] GenerateClientKeyExchange( DtlsClientProtocol.ClientHandshakeState state )
        {
            MemoryStream output = new();
            state.keyExchange.GenerateClientKeyExchange( output );
            return output.ToArray();
        }

        protected virtual void InvalidateSession( DtlsClientProtocol.ClientHandshakeState state )
        {
            if (state.sessionParameters != null)
            {
                state.sessionParameters.Clear();
                state.sessionParameters = null;
            }
            if (state.tlsSession == null)
                return;
            state.tlsSession.Invalidate();
            state.tlsSession = null;
        }

        protected virtual void ProcessCertificateRequest(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            if (state.authentication == null)
                throw new TlsFatalAlert( 40 );
            MemoryStream memoryStream = new( body, false );
            state.certificateRequest = CertificateRequest.Parse( state.clientContext, memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            state.keyExchange.ValidateCertificateRequest( state.certificateRequest );
        }

        protected virtual void ProcessCertificateStatus(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            if (!state.allowCertificateStatus)
                throw new TlsFatalAlert( 10 );
            MemoryStream memoryStream = new( body, false );
            state.certificateStatus = CertificateStatus.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
        }

        protected virtual byte[] ProcessHelloVerifyRequest(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            MemoryStream memoryStream = new( body, false );
            ProtocolVersion version = TlsUtilities.ReadVersion( memoryStream );
            byte[] numArray = TlsUtilities.ReadOpaque8( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            if (!version.IsEqualOrEarlierVersionOf( state.clientContext.ClientVersion ))
                throw new TlsFatalAlert( 47 );
            if (!ProtocolVersion.DTLSv12.IsEqualOrEarlierVersionOf( version ) && numArray.Length > 32)
                throw new TlsFatalAlert( 47 );
            return numArray;
        }

        protected virtual void ProcessNewSessionTicket(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            MemoryStream memoryStream = new( body, false );
            NewSessionTicket newSessionTicket = NewSessionTicket.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            state.client.NotifyNewSessionTicket( newSessionTicket );
        }

        protected virtual Certificate ProcessServerCertificate(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            MemoryStream memoryStream = new( body, false );
            Certificate serverCertificate = Certificate.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            state.keyExchange.ProcessServerCertificate( serverCertificate );
            state.authentication = state.client.GetAuthentication();
            state.authentication.NotifyServerCertificate( serverCertificate );
            return serverCertificate;
        }

        protected virtual void ProcessServerHello(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            SecurityParameters securityParameters = state.clientContext.SecurityParameters;
            MemoryStream input = new( body, false );
            ProtocolVersion server_version = TlsUtilities.ReadVersion( input );
            this.ReportServerVersion( state, server_version );
            securityParameters.serverRandom = TlsUtilities.ReadFully( 32, input );
            state.selectedSessionID = TlsUtilities.ReadOpaque8( input );
            if (state.selectedSessionID.Length > 32)
                throw new TlsFatalAlert( 47 );
            state.client.NotifySessionID( state.selectedSessionID );
            state.resumedSession = state.selectedSessionID.Length > 0 && state.tlsSession != null && Arrays.AreEqual( state.selectedSessionID, state.tlsSession.SessionID );
            int num1 = TlsUtilities.ReadUint16( input );
            if (!Arrays.Contains( state.offeredCipherSuites, num1 ) || num1 == 0 || CipherSuite.IsScsv( num1 ) || !TlsUtilities.IsValidCipherSuiteForVersion( num1, state.clientContext.ServerVersion ))
                throw new TlsFatalAlert( 47 );
            ValidateSelectedCipherSuite( num1, 47 );
            state.client.NotifySelectedCipherSuite( num1 );
            byte num2 = TlsUtilities.ReadUint8( input );
            if (!Arrays.Contains( state.offeredCompressionMethods, num2 ))
                throw new TlsFatalAlert( 47 );
            state.client.NotifySelectedCompressionMethod( num2 );
            state.serverExtensions = TlsProtocol.ReadExtensions( input );
            if (state.serverExtensions != null)
            {
                foreach (int key in (IEnumerable)state.serverExtensions.Keys)
                {
                    if (key != 65281)
                    {
                        if (TlsUtilities.GetExtensionData( state.clientExtensions, key ) == null)
                            throw new TlsFatalAlert( 110 );
                        int num3 = state.resumedSession ? 1 : 0;
                    }
                }
            }
            byte[] extensionData = TlsUtilities.GetExtensionData( state.serverExtensions, 65281 );
            if (extensionData != null)
            {
                state.secure_renegotiation = true;
                if (!Arrays.ConstantTimeAreEqual( extensionData, TlsProtocol.CreateRenegotiationInfo( TlsUtilities.EmptyBytes ) ))
                    throw new TlsFatalAlert( 40 );
            }
            state.client.NotifySecureRenegotiation( state.secure_renegotiation );
            IDictionary clientExtensions = state.clientExtensions;
            IDictionary dictionary = state.serverExtensions;
            if (state.resumedSession)
            {
                if (num1 != state.sessionParameters.CipherSuite || num2 != state.sessionParameters.CompressionAlgorithm)
                    throw new TlsFatalAlert( 47 );
                clientExtensions = null;
                dictionary = state.sessionParameters.ReadServerExtensions();
            }
            securityParameters.cipherSuite = num1;
            securityParameters.compressionAlgorithm = num2;
            if (dictionary != null)
            {
                bool flag = TlsExtensionsUtilities.HasEncryptThenMacExtension( dictionary );
                if (flag && !TlsUtilities.IsBlockCipherSuite( securityParameters.CipherSuite ))
                    throw new TlsFatalAlert( 47 );
                securityParameters.encryptThenMac = flag;
                securityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension( dictionary );
                securityParameters.maxFragmentLength = EvaluateMaxFragmentLengthExtension( state.resumedSession, clientExtensions, dictionary, 47 );
                securityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension( dictionary );
                state.allowCertificateStatus = !state.resumedSession && TlsUtilities.HasExpectedEmptyExtensionData( dictionary, 5, 47 );
                state.expectSessionTicket = !state.resumedSession && TlsUtilities.HasExpectedEmptyExtensionData( dictionary, 35, 47 );
            }
            if (clientExtensions != null)
                state.client.ProcessServerExtensions( dictionary );
            securityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm( state.clientContext, securityParameters.CipherSuite );
            securityParameters.verifyDataLength = 12;
        }

        protected virtual void ProcessServerKeyExchange(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            MemoryStream memoryStream = new( body, false );
            state.keyExchange.ProcessServerKeyExchange( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
        }

        protected virtual void ProcessServerSupplementalData(
          DtlsClientProtocol.ClientHandshakeState state,
          byte[] body )
        {
            IList serverSupplementalData = TlsProtocol.ReadSupplementalDataMessage( new MemoryStream( body, false ) );
            state.client.ProcessServerSupplementalData( serverSupplementalData );
        }

        protected virtual void ReportServerVersion(
          DtlsClientProtocol.ClientHandshakeState state,
          ProtocolVersion server_version )
        {
            TlsClientContextImpl clientContext = state.clientContext;
            ProtocolVersion serverVersion = clientContext.ServerVersion;
            if (serverVersion == null)
            {
                clientContext.SetServerVersion( server_version );
                state.client.NotifyServerVersion( server_version );
            }
            else if (!serverVersion.Equals( server_version ))
                throw new TlsFatalAlert( 47 );
        }

        protected static byte[] PatchClientHelloWithCookie( byte[] clientHelloBody, byte[] cookie )
        {
            int offset = 34;
            int num1 = TlsUtilities.ReadUint8( clientHelloBody, offset );
            int num2 = offset + 1 + num1;
            int num3 = num2 + 1;
            byte[] numArray = new byte[clientHelloBody.Length + cookie.Length];
            Array.Copy( clientHelloBody, 0, numArray, 0, num2 );
            TlsUtilities.CheckUint8( cookie.Length );
            TlsUtilities.WriteUint8( (byte)cookie.Length, numArray, num2 );
            Array.Copy( cookie, 0, numArray, num3, cookie.Length );
            Array.Copy( clientHelloBody, num3, numArray, num3 + cookie.Length, clientHelloBody.Length - num3 );
            return numArray;
        }

        protected internal class ClientHandshakeState
        {
            internal TlsClient client = null;
            internal TlsClientContextImpl clientContext = null;
            internal TlsSession tlsSession = null;
            internal SessionParameters sessionParameters = null;
            internal SessionParameters.Builder sessionParametersBuilder = null;
            internal int[] offeredCipherSuites = null;
            internal byte[] offeredCompressionMethods = null;
            internal IDictionary clientExtensions = null;
            internal IDictionary serverExtensions = null;
            internal byte[] selectedSessionID = null;
            internal bool resumedSession = false;
            internal bool secure_renegotiation = false;
            internal bool allowCertificateStatus = false;
            internal bool expectSessionTicket = false;
            internal TlsKeyExchange keyExchange = null;
            internal TlsAuthentication authentication = null;
            internal CertificateStatus certificateStatus = null;
            internal CertificateRequest certificateRequest = null;
            internal TlsCredentials clientCredentials = null;
        }
    }
}
