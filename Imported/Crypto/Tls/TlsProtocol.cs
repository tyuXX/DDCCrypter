// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsProtocol
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsProtocol
    {
        protected const short CS_START = 0;
        protected const short CS_CLIENT_HELLO = 1;
        protected const short CS_SERVER_HELLO = 2;
        protected const short CS_SERVER_SUPPLEMENTAL_DATA = 3;
        protected const short CS_SERVER_CERTIFICATE = 4;
        protected const short CS_CERTIFICATE_STATUS = 5;
        protected const short CS_SERVER_KEY_EXCHANGE = 6;
        protected const short CS_CERTIFICATE_REQUEST = 7;
        protected const short CS_SERVER_HELLO_DONE = 8;
        protected const short CS_CLIENT_SUPPLEMENTAL_DATA = 9;
        protected const short CS_CLIENT_CERTIFICATE = 10;
        protected const short CS_CLIENT_KEY_EXCHANGE = 11;
        protected const short CS_CERTIFICATE_VERIFY = 12;
        protected const short CS_CLIENT_FINISHED = 13;
        protected const short CS_SERVER_SESSION_TICKET = 14;
        protected const short CS_SERVER_FINISHED = 15;
        protected const short CS_END = 16;
        private static readonly string TLS_ERROR_MESSAGE = "Internal TLS error, this could be an attack";
        private ByteQueue mApplicationDataQueue = new();
        private ByteQueue mAlertQueue = new( 2 );
        private ByteQueue mHandshakeQueue = new();
        internal RecordStream mRecordStream;
        protected SecureRandom mSecureRandom;
        private TlsStream mTlsStream = null;
        private volatile bool mClosed = false;
        private volatile bool mFailedWithError = false;
        private volatile bool mAppDataReady = false;
        private volatile bool mSplitApplicationDataRecords = true;
        private byte[] mExpectedVerifyData = null;
        protected TlsSession mTlsSession = null;
        protected SessionParameters mSessionParameters = null;
        protected SecurityParameters mSecurityParameters = null;
        protected Certificate mPeerCertificate = null;
        protected int[] mOfferedCipherSuites = null;
        protected byte[] mOfferedCompressionMethods = null;
        protected IDictionary mClientExtensions = null;
        protected IDictionary mServerExtensions = null;
        protected short mConnectionState = 0;
        protected bool mResumedSession = false;
        protected bool mReceivedChangeCipherSpec = false;
        protected bool mSecureRenegotiation = false;
        protected bool mAllowCertificateStatus = false;
        protected bool mExpectSessionTicket = false;
        protected bool mBlocking = true;
        protected ByteQueueStream mInputBuffers = null;
        protected ByteQueueStream mOutputBuffer = null;

        public TlsProtocol( Stream stream, SecureRandom secureRandom )
          : this( stream, stream, secureRandom )
        {
        }

        public TlsProtocol( Stream input, Stream output, SecureRandom secureRandom )
        {
            this.mRecordStream = new RecordStream( this, input, output );
            this.mSecureRandom = secureRandom;
        }

        public TlsProtocol( SecureRandom secureRandom )
        {
            this.mBlocking = false;
            this.mInputBuffers = new ByteQueueStream();
            this.mOutputBuffer = new ByteQueueStream();
            this.mRecordStream = new RecordStream( this, mInputBuffers, mOutputBuffer );
            this.mSecureRandom = secureRandom;
        }

        protected abstract TlsContext Context { get; }

        internal abstract AbstractTlsContext ContextAdmin { get; }

        protected abstract TlsPeer Peer { get; }

        protected virtual void HandleChangeCipherSpecMessage()
        {
        }

        protected abstract void HandleHandshakeMessage( byte type, byte[] buf );

        protected virtual void HandleWarningMessage( byte description )
        {
        }

        protected virtual void ApplyMaxFragmentLengthExtension()
        {
            if (this.mSecurityParameters.maxFragmentLength < 0)
                return;
            if (!MaxFragmentLength.IsValid( (byte)this.mSecurityParameters.maxFragmentLength ))
                throw new TlsFatalAlert( 80 );
            this.mRecordStream.SetPlaintextLimit( 1 << (8 + mSecurityParameters.maxFragmentLength) );
        }

        protected virtual void CheckReceivedChangeCipherSpec( bool expected )
        {
            if (expected != this.mReceivedChangeCipherSpec)
                throw new TlsFatalAlert( 10 );
        }

        protected virtual void CleanupHandshake()
        {
            if (this.mExpectedVerifyData != null)
            {
                Arrays.Fill( this.mExpectedVerifyData, 0 );
                this.mExpectedVerifyData = null;
            }
            this.mSecurityParameters.Clear();
            this.mPeerCertificate = null;
            this.mOfferedCipherSuites = null;
            this.mOfferedCompressionMethods = null;
            this.mClientExtensions = null;
            this.mServerExtensions = null;
            this.mResumedSession = false;
            this.mReceivedChangeCipherSpec = false;
            this.mSecureRenegotiation = false;
            this.mAllowCertificateStatus = false;
            this.mExpectSessionTicket = false;
        }

        protected virtual void BlockForHandshake()
        {
            if (!this.mBlocking)
                return;
            while (this.mConnectionState != 16)
            {
                int num = this.mClosed ? 1 : 0;
                this.SafeReadRecord();
            }
        }

        protected virtual void CompleteHandshake()
        {
            try
            {
                this.mRecordStream.FinaliseHandshake();
                this.mSplitApplicationDataRecords = !TlsUtilities.IsTlsV11( this.Context );
                if (!this.mAppDataReady)
                {
                    this.mAppDataReady = true;
                    if (this.mBlocking)
                        this.mTlsStream = new TlsStream( this );
                }
                if (this.mTlsSession != null)
                {
                    if (this.mSessionParameters == null)
                    {
                        this.mSessionParameters = new SessionParameters.Builder().SetCipherSuite( this.mSecurityParameters.CipherSuite ).SetCompressionAlgorithm( this.mSecurityParameters.CompressionAlgorithm ).SetMasterSecret( this.mSecurityParameters.MasterSecret ).SetPeerCertificate( this.mPeerCertificate ).SetPskIdentity( this.mSecurityParameters.PskIdentity ).SetSrpIdentity( this.mSecurityParameters.SrpIdentity ).SetServerExtensions( this.mServerExtensions ).Build();
                        this.mTlsSession = new TlsSessionImpl( this.mTlsSession.SessionID, this.mSessionParameters );
                    }
                    this.ContextAdmin.SetResumableSession( this.mTlsSession );
                }
                this.Peer.NotifyHandshakeComplete();
            }
            finally
            {
                this.CleanupHandshake();
            }
        }

        protected internal void ProcessRecord( byte protocol, byte[] buf, int offset, int len )
        {
            switch (protocol)
            {
                case 20:
                    this.ProcessChangeCipherSpec( buf, offset, len );
                    break;
                case 21:
                    this.mAlertQueue.AddData( buf, offset, len );
                    this.ProcessAlert();
                    break;
                case 22:
                    this.mHandshakeQueue.AddData( buf, offset, len );
                    this.ProcessHandshake();
                    break;
                case 23:
                    if (!this.mAppDataReady)
                        throw new TlsFatalAlert( 10 );
                    this.mApplicationDataQueue.AddData( buf, offset, len );
                    this.ProcessApplicationData();
                    break;
                case 24:
                    if (this.mAppDataReady)
                        break;
                    throw new TlsFatalAlert( 10 );
            }
        }

        private void ProcessHandshake()
        {
            bool flag;
            do
            {
                flag = false;
                if (this.mHandshakeQueue.Available >= 4)
                {
                    byte[] numArray1 = new byte[4];
                    this.mHandshakeQueue.Read( numArray1, 0, 4, 0 );
                    byte type = TlsUtilities.ReadUint8( numArray1, 0 );
                    int len = TlsUtilities.ReadUint24( numArray1, 1 );
                    if (this.mHandshakeQueue.Available >= len + 4)
                    {
                        byte[] numArray2 = this.mHandshakeQueue.RemoveData( len, 4 );
                        this.CheckReceivedChangeCipherSpec( this.mConnectionState == 16 || type == 20 );
                        if (type != 0)
                        {
                            TlsContext context = this.Context;
                            if (type == 20 && this.mExpectedVerifyData == null && context.SecurityParameters.MasterSecret != null)
                                this.mExpectedVerifyData = this.CreateVerifyData( !context.IsServer );
                            this.mRecordStream.UpdateHandshakeData( numArray1, 0, 4 );
                            this.mRecordStream.UpdateHandshakeData( numArray2, 0, len );
                        }
                        this.HandleHandshakeMessage( type, numArray2 );
                        flag = true;
                    }
                }
            }
            while (flag);
        }

        private void ProcessApplicationData()
        {
        }

        private void ProcessAlert()
        {
            while (this.mAlertQueue.Available >= 2)
            {
                byte[] numArray = this.mAlertQueue.RemoveData( 2, 0 );
                byte alertLevel = numArray[0];
                byte num = numArray[1];
                this.Peer.NotifyAlertReceived( alertLevel, num );
                if (alertLevel == 2)
                {
                    this.InvalidateSession();
                    this.mFailedWithError = true;
                    this.mClosed = true;
                    this.mRecordStream.SafeClose();
                    throw new IOException( TLS_ERROR_MESSAGE );
                }
                if (num == 0)
                    this.HandleClose( false );
                this.HandleWarningMessage( num );
            }
        }

        private void ProcessChangeCipherSpec( byte[] buf, int off, int len )
        {
            for (int index = 0; index < len; ++index)
            {
                if (TlsUtilities.ReadUint8( buf, off + index ) != 1)
                    throw new TlsFatalAlert( 50 );
                if (this.mReceivedChangeCipherSpec || this.mAlertQueue.Available > 0 || this.mHandshakeQueue.Available > 0)
                    throw new TlsFatalAlert( 10 );
                this.mRecordStream.ReceivedReadCipherSpec();
                this.mReceivedChangeCipherSpec = true;
                this.HandleChangeCipherSpecMessage();
            }
        }

        protected internal virtual int ApplicationDataAvailable() => this.mApplicationDataQueue.Available;

        protected internal virtual int ReadApplicationData( byte[] buf, int offset, int len )
        {
            if (len < 1)
                return 0;
            while (this.mApplicationDataQueue.Available == 0)
            {
                if (this.mClosed)
                {
                    if (this.mFailedWithError)
                        throw new IOException( TLS_ERROR_MESSAGE );
                    return 0;
                }
                this.SafeReadRecord();
            }
            len = System.Math.Min( len, this.mApplicationDataQueue.Available );
            this.mApplicationDataQueue.RemoveData( buf, offset, len, 0 );
            return len;
        }

        protected virtual void SafeReadRecord()
        {
            try
            {
                if (!this.mRecordStream.ReadRecord())
                    throw new EndOfStreamException();
            }
            catch (TlsFatalAlert ex)
            {
                if (!this.mClosed)
                    this.FailWithError( 2, ex.AlertDescription, "Failed to read record", ex );
                throw ex;
            }
            catch (Exception ex)
            {
                if (!this.mClosed)
                    this.FailWithError( 2, 80, "Failed to read record", ex );
                throw ex;
            }
        }

        protected virtual void SafeWriteRecord( byte type, byte[] buf, int offset, int len )
        {
            try
            {
                this.mRecordStream.WriteRecord( type, buf, offset, len );
            }
            catch (TlsFatalAlert ex)
            {
                if (!this.mClosed)
                    this.FailWithError( 2, ex.AlertDescription, "Failed to write record", ex );
                throw ex;
            }
            catch (Exception ex)
            {
                if (!this.mClosed)
                    this.FailWithError( 2, 80, "Failed to write record", ex );
                throw ex;
            }
        }

        protected internal virtual void WriteData( byte[] buf, int offset, int len )
        {
            if (this.mClosed)
            {
                if (this.mFailedWithError)
                    throw new IOException( TLS_ERROR_MESSAGE );
                throw new IOException( "Sorry, connection has been closed, you cannot write more data" );
            }
            while (len > 0)
            {
                if (this.mSplitApplicationDataRecords)
                {
                    this.SafeWriteRecord( 23, buf, offset, 1 );
                    ++offset;
                    --len;
                }
                if (len > 0)
                {
                    int len1 = System.Math.Min( len, this.mRecordStream.GetPlaintextLimit() );
                    this.SafeWriteRecord( 23, buf, offset, len1 );
                    offset += len1;
                    len -= len1;
                }
            }
        }

        protected virtual void WriteHandshakeMessage( byte[] buf, int off, int len )
        {
            int len1;
            for (; len > 0; len -= len1)
            {
                len1 = System.Math.Min( len, this.mRecordStream.GetPlaintextLimit() );
                this.SafeWriteRecord( 22, buf, off, len1 );
                off += len1;
            }
        }

        public virtual Stream Stream
        {
            get
            {
                if (!this.mBlocking)
                    throw new InvalidOperationException( "Cannot use Stream in non-blocking mode! Use OfferInput()/OfferOutput() instead." );
                return mTlsStream;
            }
        }

        public virtual void OfferInput( byte[] input )
        {
            if (this.mBlocking)
                throw new InvalidOperationException( "Cannot use OfferInput() in blocking mode! Use Stream instead." );
            if (this.mClosed)
                throw new IOException( "Connection is closed, cannot accept any more input" );
            this.mInputBuffers.Write( input );
            while (this.mInputBuffers.Available >= 5)
            {
                byte[] buf = new byte[5];
                this.mInputBuffers.Peek( buf );
                if (this.mInputBuffers.Available < TlsUtilities.ReadUint16( buf, 3 ) + 5)
                    break;
                this.SafeReadRecord();
            }
        }

        public virtual int GetAvailableInputBytes()
        {
            if (this.mBlocking)
                throw new InvalidOperationException( "Cannot use GetAvailableInputBytes() in blocking mode! Use ApplicationDataAvailable() instead." );
            return this.ApplicationDataAvailable();
        }

        public virtual int ReadInput( byte[] buffer, int offset, int length )
        {
            if (this.mBlocking)
                throw new InvalidOperationException( "Cannot use ReadInput() in blocking mode! Use Stream instead." );
            return this.ReadApplicationData( buffer, offset, System.Math.Min( length, this.ApplicationDataAvailable() ) );
        }

        public virtual void OfferOutput( byte[] buffer, int offset, int length )
        {
            if (this.mBlocking)
                throw new InvalidOperationException( "Cannot use OfferOutput() in blocking mode! Use Stream instead." );
            if (!this.mAppDataReady)
                throw new IOException( "Application data cannot be sent until the handshake is complete!" );
            this.WriteData( buffer, offset, length );
        }

        public virtual int GetAvailableOutputBytes()
        {
            if (this.mBlocking)
                throw new InvalidOperationException( "Cannot use GetAvailableOutputBytes() in blocking mode! Use Stream instead." );
            return this.mOutputBuffer.Available;
        }

        public virtual int ReadOutput( byte[] buffer, int offset, int length )
        {
            if (this.mBlocking)
                throw new InvalidOperationException( "Cannot use ReadOutput() in blocking mode! Use Stream instead." );
            return this.mOutputBuffer.Read( buffer, offset, length );
        }

        protected virtual void FailWithError(
          byte alertLevel,
          byte alertDescription,
          string message,
          Exception cause )
        {
            this.mClosed = !this.mClosed ? true : throw new IOException( TLS_ERROR_MESSAGE );
            if (alertLevel == 2)
            {
                this.InvalidateSession();
                this.mFailedWithError = true;
            }
            this.RaiseAlert( alertLevel, alertDescription, message, cause );
            this.mRecordStream.SafeClose();
            if (alertLevel != 2)
                ;
        }

        protected virtual void InvalidateSession()
        {
            if (this.mSessionParameters != null)
            {
                this.mSessionParameters.Clear();
                this.mSessionParameters = null;
            }
            if (this.mTlsSession == null)
                return;
            this.mTlsSession.Invalidate();
            this.mTlsSession = null;
        }

        protected virtual void ProcessFinishedMessage( MemoryStream buf )
        {
            if (this.mExpectedVerifyData == null)
                throw new TlsFatalAlert( 80 );
            byte[] b = TlsUtilities.ReadFully( this.mExpectedVerifyData.Length, buf );
            AssertEmpty( buf );
            if (!Arrays.ConstantTimeAreEqual( this.mExpectedVerifyData, b ))
                throw new TlsFatalAlert( 51 );
        }

        protected virtual void RaiseAlert(
          byte alertLevel,
          byte alertDescription,
          string message,
          Exception cause )
        {
            this.Peer.NotifyAlertRaised( alertLevel, alertDescription, message, cause );
            this.SafeWriteRecord( 21, new byte[2]
            {
        alertLevel,
        alertDescription
            }, 0, 2 );
        }

        protected virtual void RaiseWarning( byte alertDescription, string message ) => this.RaiseAlert( 1, alertDescription, message, null );

        protected virtual void SendCertificateMessage( Certificate certificate )
        {
            if (certificate == null)
                certificate = Certificate.EmptyChain;
            if (certificate.IsEmpty && !this.Context.IsServer)
            {
                ProtocolVersion serverVersion = this.Context.ServerVersion;
                if (serverVersion.IsSsl)
                {
                    this.RaiseWarning( 41, serverVersion.ToString() + " client didn't provide credentials" );
                    return;
                }
            }
            TlsProtocol.HandshakeMessage output = new( 11 );
            certificate.Encode( output );
            output.WriteToRecordStream( this );
        }

        protected virtual void SendChangeCipherSpecMessage()
        {
            byte[] buf = new byte[1] { 1 };
            this.SafeWriteRecord( 20, buf, 0, buf.Length );
            this.mRecordStream.SentWriteCipherSpec();
        }

        protected virtual void SendFinishedMessage()
        {
            byte[] verifyData = this.CreateVerifyData( this.Context.IsServer );
            TlsProtocol.HandshakeMessage handshakeMessage = new( 20, verifyData.Length );
            handshakeMessage.Write( verifyData, 0, verifyData.Length );
            handshakeMessage.WriteToRecordStream( this );
        }

        protected virtual void SendSupplementalDataMessage( IList supplementalData )
        {
            TlsProtocol.HandshakeMessage output = new( 23 );
            WriteSupplementalData( output, supplementalData );
            output.WriteToRecordStream( this );
        }

        protected virtual byte[] CreateVerifyData( bool isServer )
        {
            TlsContext context = this.Context;
            string asciiLabel = isServer ? "server finished" : "client finished";
            byte[] sslSender = isServer ? TlsUtilities.SSL_SERVER : TlsUtilities.SSL_CLIENT;
            byte[] currentPrfHash = GetCurrentPrfHash( context, this.mRecordStream.HandshakeHash, sslSender );
            return TlsUtilities.CalculateVerifyData( context, asciiLabel, currentPrfHash );
        }

        public virtual void Close() => this.HandleClose( true );

        protected virtual void HandleClose( bool user_canceled )
        {
            if (this.mClosed)
                return;
            if (user_canceled && !this.mAppDataReady)
                this.RaiseWarning( 90, "User canceled handshake" );
            this.FailWithError( 1, 0, "Connection closed", null );
        }

        protected internal virtual void Flush() => this.mRecordStream.Flush();

        public virtual bool IsClosed => this.mClosed;

        protected virtual short ProcessMaxFragmentLengthExtension(
          IDictionary clientExtensions,
          IDictionary serverExtensions,
          byte alertDescription )
        {
            short fragmentLengthExtension = TlsExtensionsUtilities.GetMaxFragmentLengthExtension( serverExtensions );
            if (fragmentLengthExtension >= 0 && (!MaxFragmentLength.IsValid( (byte)fragmentLengthExtension ) || (!this.mResumedSession && fragmentLengthExtension != TlsExtensionsUtilities.GetMaxFragmentLengthExtension( clientExtensions ))))
                throw new TlsFatalAlert( alertDescription );
            return fragmentLengthExtension;
        }

        protected virtual void RefuseRenegotiation()
        {
            if (TlsUtilities.IsSsl( this.Context ))
                throw new TlsFatalAlert( 40 );
            this.RaiseWarning( 100, "Renegotiation not supported" );
        }

        protected internal static void AssertEmpty( MemoryStream buf )
        {
            if (buf.Position < buf.Length)
                throw new TlsFatalAlert( 50 );
        }

        protected internal static byte[] CreateRandomBlock(
          bool useGmtUnixTime,
          IRandomGenerator randomGenerator )
        {
            byte[] randomBlock = new byte[32];
            randomGenerator.NextBytes( randomBlock );
            if (useGmtUnixTime)
                TlsUtilities.WriteGmtUnixTime( randomBlock, 0 );
            return randomBlock;
        }

        protected internal static byte[] CreateRenegotiationInfo( byte[] renegotiated_connection ) => TlsUtilities.EncodeOpaque8( renegotiated_connection );

        protected internal static void EstablishMasterSecret(
          TlsContext context,
          TlsKeyExchange keyExchange )
        {
            byte[] premasterSecret = keyExchange.GeneratePremasterSecret();
            try
            {
                context.SecurityParameters.masterSecret = TlsUtilities.CalculateMasterSecret( context, premasterSecret );
            }
            finally
            {
                if (premasterSecret != null)
                    Arrays.Fill( premasterSecret, 0 );
            }
        }

        protected internal static byte[] GetCurrentPrfHash(
          TlsContext context,
          TlsHandshakeHash handshakeHash,
          byte[] sslSender )
        {
            IDigest digest = handshakeHash.ForkPrfHash();
            if (sslSender != null && TlsUtilities.IsSsl( context ))
                digest.BlockUpdate( sslSender, 0, sslSender.Length );
            return DigestUtilities.DoFinal( digest );
        }

        protected internal static IDictionary ReadExtensions( MemoryStream input )
        {
            if (input.Position >= input.Length)
                return null;
            byte[] buffer = TlsUtilities.ReadOpaque16( input );
            AssertEmpty( input );
            MemoryStream input1 = new( buffer, false );
            IDictionary hashtable = Platform.CreateHashtable();
            while (input1.Position < input1.Length)
            {
                int key = TlsUtilities.ReadUint16( input1 );
                byte[] numArray = TlsUtilities.ReadOpaque16( input1 );
                if (hashtable.Contains( key ))
                    throw new TlsFatalAlert( 47 );
                hashtable.Add( key, numArray );
            }
            return hashtable;
        }

        protected internal static IList ReadSupplementalDataMessage( MemoryStream input )
        {
            byte[] buffer = TlsUtilities.ReadOpaque24( input );
            AssertEmpty( input );
            MemoryStream input1 = new( buffer, false );
            IList arrayList = Platform.CreateArrayList();
            while (input1.Position < input1.Length)
            {
                int dataType = TlsUtilities.ReadUint16( input1 );
                byte[] data = TlsUtilities.ReadOpaque16( input1 );
                arrayList.Add( new SupplementalDataEntry( dataType, data ) );
            }
            return arrayList;
        }

        protected internal static void WriteExtensions( Stream output, IDictionary extensions )
        {
            MemoryStream output1 = new();
            foreach (int key in (IEnumerable)extensions.Keys)
            {
                byte[] extension = (byte[])extensions[key];
                TlsUtilities.CheckUint16( key );
                TlsUtilities.WriteUint16( key, output1 );
                TlsUtilities.WriteOpaque16( extension, output1 );
            }
            TlsUtilities.WriteOpaque16( output1.ToArray(), output );
        }

        protected internal static void WriteSupplementalData( Stream output, IList supplementalData )
        {
            MemoryStream output1 = new();
            foreach (SupplementalDataEntry supplementalDataEntry in (IEnumerable)supplementalData)
            {
                int dataType = supplementalDataEntry.DataType;
                TlsUtilities.CheckUint16( dataType );
                TlsUtilities.WriteUint16( dataType, output1 );
                TlsUtilities.WriteOpaque16( supplementalDataEntry.Data, output1 );
            }
            TlsUtilities.WriteOpaque24( output1.ToArray(), output );
        }

        protected internal static int GetPrfAlgorithm( TlsContext context, int ciphersuite )
        {
            bool flag = TlsUtilities.IsTlsV12( context );
            switch (ciphersuite)
            {
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 156:
                case 158:
                case 160:
                case 162:
                case 164:
                case 168:
                case 170:
                case 172:
                case 186:
                case 187:
                case 188:
                case 189:
                case 190:
                case 191:
                case 192:
                case 193:
                case 194:
                case 195:
                case 196:
                case 197:
                case 49187:
                case 49189:
                case 49191:
                case 49193:
                case 49195:
                case 49197:
                case 49199:
                case 49201:
                case 49266:
                case 49268:
                case 49270:
                case 49272:
                case 49274:
                case 49276:
                case 49278:
                case 49280:
                case 49282:
                case 49284:
                case 49286:
                case 49288:
                case 49290:
                case 49292:
                case 49294:
                case 49296:
                case 49298:
                case 49308:
                case 49309:
                case 49310:
                case 49311:
                case 49312:
                case 49313:
                case 49314:
                case 49315:
                case 49316:
                case 49317:
                case 49318:
                case 49319:
                case 49320:
                case 49321:
                case 49322:
                case 49323:
                case 49324:
                case 49325:
                case 49326:
                case 49327:
                case 52243:
                case 52244:
                case 52245:
                    if (flag)
                        return 1;
                    throw new TlsFatalAlert( 47 );
                case 157:
                case 159:
                case 161:
                case 163:
                case 165:
                case 169:
                case 171:
                case 173:
                case 49188:
                case 49190:
                case 49192:
                case 49194:
                case 49196:
                case 49198:
                case 49200:
                case 49202:
                case 49267:
                case 49269:
                case 49271:
                case 49273:
                case 49275:
                case 49277:
                case 49279:
                case 49281:
                case 49283:
                case 49285:
                case 49287:
                case 49289:
                case 49291:
                case 49293:
                case 49295:
                case 49297:
                case 49299:
                    if (flag)
                        return 2;
                    throw new TlsFatalAlert( 47 );
                case 175:
                case 177:
                case 179:
                case 181:
                case 183:
                case 185:
                case 49208:
                case 49211:
                case 49301:
                case 49303:
                case 49305:
                case 49307:
                    return flag ? 2 : 0;
                default:
                    return flag ? 1 : 0;
            }
        }

        internal class HandshakeMessage : MemoryStream
        {
            internal HandshakeMessage( byte handshakeType )
              : this( handshakeType, 60 )
            {
            }

            internal HandshakeMessage( byte handshakeType, int length )
              : base( length + 4 )
            {
                TlsUtilities.WriteUint8( handshakeType, this );
                TlsUtilities.WriteUint24( 0, this );
            }

            internal void Write( byte[] data ) => this.Write( data, 0, data.Length );

            internal void WriteToRecordStream( TlsProtocol protocol )
            {
                long i = this.Length - 4L;
                TlsUtilities.CheckUint24( i );
                this.Position = 1L;
                TlsUtilities.WriteUint24( (int)i, this );
                byte[] buffer = this.GetBuffer();
                int length = (int)this.Length;
                protocol.WriteHandshakeMessage( buffer, 0, length );
                Platform.Dispose( this );
            }
        }
    }
}
