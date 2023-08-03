// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsRecordLayer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Date;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DtlsRecordLayer : DatagramTransport
    {
        private const int RECORD_HEADER_LENGTH = 13;
        private const int MAX_FRAGMENT_LENGTH = 16384;
        private const long TCP_MSL = 120000;
        private const long RETRANSMIT_TIMEOUT = 240000;
        private readonly DatagramTransport mTransport;
        private readonly TlsContext mContext;
        private readonly TlsPeer mPeer;
        private readonly ByteQueue mRecordQueue = new ByteQueue();
        private volatile bool mClosed = false;
        private volatile bool mFailed = false;
        private volatile ProtocolVersion mReadVersion = null;
        private volatile ProtocolVersion mWriteVersion = null;
        private volatile bool mInHandshake;
        private volatile int mPlaintextLimit;
        private DtlsEpoch mCurrentEpoch;
        private DtlsEpoch mPendingEpoch;
        private DtlsEpoch mReadEpoch;
        private DtlsEpoch mWriteEpoch;
        private DtlsHandshakeRetransmit mRetransmit = null;
        private DtlsEpoch mRetransmitEpoch = null;
        private long mRetransmitExpiry = 0;

        internal DtlsRecordLayer(
          DatagramTransport transport,
          TlsContext context,
          TlsPeer peer,
          byte contentType )
        {
            this.mTransport = transport;
            this.mContext = context;
            this.mPeer = peer;
            this.mInHandshake = true;
            this.mCurrentEpoch = new DtlsEpoch( 0, new TlsNullCipher( context ) );
            this.mPendingEpoch = null;
            this.mReadEpoch = this.mCurrentEpoch;
            this.mWriteEpoch = this.mCurrentEpoch;
            this.SetPlaintextLimit( 16384 );
        }

        internal virtual void SetPlaintextLimit( int plaintextLimit ) => this.mPlaintextLimit = plaintextLimit;

        internal virtual ProtocolVersion ReadVersion
        {
            get => this.mReadVersion;
            set => this.mReadVersion = value;
        }

        internal virtual void SetWriteVersion( ProtocolVersion writeVersion ) => this.mWriteVersion = writeVersion;

        internal virtual void InitPendingEpoch( TlsCipher pendingCipher )
        {
            if (this.mPendingEpoch != null)
                throw new InvalidOperationException();
            this.mPendingEpoch = new DtlsEpoch( this.mWriteEpoch.Epoch + 1, pendingCipher );
        }

        internal virtual void HandshakeSuccessful( DtlsHandshakeRetransmit retransmit )
        {
            if (this.mReadEpoch == this.mCurrentEpoch || this.mWriteEpoch == this.mCurrentEpoch)
                throw new InvalidOperationException();
            if (retransmit != null)
            {
                this.mRetransmit = retransmit;
                this.mRetransmitEpoch = this.mCurrentEpoch;
                this.mRetransmitExpiry = DateTimeUtilities.CurrentUnixMs() + 240000L;
            }
            this.mInHandshake = false;
            this.mCurrentEpoch = this.mPendingEpoch;
            this.mPendingEpoch = null;
        }

        internal virtual void ResetWriteEpoch()
        {
            if (this.mRetransmitEpoch != null)
                this.mWriteEpoch = this.mRetransmitEpoch;
            else
                this.mWriteEpoch = this.mCurrentEpoch;
        }

        public virtual int GetReceiveLimit() => System.Math.Min( this.mPlaintextLimit, this.mReadEpoch.Cipher.GetPlaintextLimit( this.mTransport.GetReceiveLimit() - 13 ) );

        public virtual int GetSendLimit() => System.Math.Min( this.mPlaintextLimit, this.mWriteEpoch.Cipher.GetPlaintextLimit( this.mTransport.GetSendLimit() - 13 ) );

        public virtual int Receive( byte[] buf, int off, int len, int waitMillis )
        {
            byte[] numArray1 = null;
            while (true)
            {
                int len1 = System.Math.Min( len, this.GetReceiveLimit() ) + 13;
                if (numArray1 != null)
                {
                    if (numArray1.Length >= len1)
                        goto label_4;
                }
                numArray1 = new byte[len1];
            label_4:
                try
                {
                    if (this.mRetransmit != null && DateTimeUtilities.CurrentUnixMs() > this.mRetransmitExpiry)
                    {
                        this.mRetransmit = null;
                        this.mRetransmitEpoch = null;
                    }
                    int record = this.ReceiveRecord( numArray1, 0, len1, waitMillis );
                    if (record < 0)
                        return record;
                    if (record >= 13)
                    {
                        int num1 = TlsUtilities.ReadUint16( numArray1, 11 );
                        if (record == num1 + 13)
                        {
                            byte type = TlsUtilities.ReadUint8( numArray1, 0 );
                            switch (type)
                            {
                                case 20:
                                case 21:
                                case 22:
                                case 23:
                                case 24:
                                    int epoch = TlsUtilities.ReadUint16( numArray1, 3 );
                                    DtlsEpoch dtlsEpoch = null;
                                    if (epoch == this.mReadEpoch.Epoch)
                                        dtlsEpoch = this.mReadEpoch;
                                    else if (type == 22 && this.mRetransmitEpoch != null && epoch == this.mRetransmitEpoch.Epoch)
                                        dtlsEpoch = this.mRetransmitEpoch;
                                    if (dtlsEpoch != null)
                                    {
                                        long num2 = TlsUtilities.ReadUint48( numArray1, 5 );
                                        if (!dtlsEpoch.ReplayWindow.ShouldDiscard( num2 ))
                                        {
                                            ProtocolVersion other = TlsUtilities.ReadVersion( numArray1, 1 );
                                            if (other.IsDtls)
                                            {
                                                if (this.mReadVersion != null)
                                                {
                                                    if (!this.mReadVersion.Equals( other ))
                                                        continue;
                                                }
                                                byte[] numArray2 = dtlsEpoch.Cipher.DecodeCiphertext( GetMacSequenceNumber( dtlsEpoch.Epoch, num2 ), type, numArray1, 13, record - 13 );
                                                dtlsEpoch.ReplayWindow.ReportAuthenticated( num2 );
                                                if (numArray2.Length <= this.mPlaintextLimit)
                                                {
                                                    if (this.mReadVersion == null)
                                                        this.mReadVersion = other;
                                                    switch (type)
                                                    {
                                                        case 20:
                                                            for (int offset = 0; offset < numArray2.Length; ++offset)
                                                            {
                                                                if (TlsUtilities.ReadUint8( numArray2, offset ) == 1 && this.mPendingEpoch != null)
                                                                    this.mReadEpoch = this.mPendingEpoch;
                                                            }
                                                            continue;
                                                        case 21:
                                                            if (numArray2.Length == 2)
                                                            {
                                                                byte alertLevel = numArray2[0];
                                                                byte alertDescription = numArray2[1];
                                                                this.mPeer.NotifyAlertReceived( alertLevel, alertDescription );
                                                                if (alertLevel == 2)
                                                                {
                                                                    this.Fail( alertDescription );
                                                                    throw new TlsFatalAlert( alertDescription );
                                                                }
                                                                if (alertDescription == 0)
                                                                {
                                                                    this.CloseTransport();
                                                                    continue;
                                                                }
                                                                continue;
                                                            }
                                                            continue;
                                                        case 22:
                                                            if (!this.mInHandshake)
                                                            {
                                                                if (this.mRetransmit != null)
                                                                {
                                                                    this.mRetransmit.ReceivedHandshakeRecord( epoch, numArray2, 0, numArray2.Length );
                                                                    continue;
                                                                }
                                                                continue;
                                                            }
                                                            break;
                                                        case 23:
                                                            if (!this.mInHandshake)
                                                                break;
                                                            continue;
                                                        case 24:
                                                            continue;
                                                    }
                                                    if (!this.mInHandshake && this.mRetransmit != null)
                                                    {
                                                        this.mRetransmit = null;
                                                        this.mRetransmitEpoch = null;
                                                    }
                                                    Array.Copy( numArray2, 0, buf, off, numArray2.Length );
                                                    return numArray2.Length;
                                                }
                                                continue;
                                            }
                                            continue;
                                        }
                                        continue;
                                    }
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    throw ex;
                }
            }
        }

        public virtual void Send( byte[] buf, int off, int len )
        {
            byte contentType = 23;
            if (this.mInHandshake || this.mWriteEpoch == this.mRetransmitEpoch)
            {
                contentType = 22;
                if (TlsUtilities.ReadUint8( buf, off ) == 20)
                {
                    DtlsEpoch dtlsEpoch = null;
                    if (this.mInHandshake)
                        dtlsEpoch = this.mPendingEpoch;
                    else if (this.mWriteEpoch == this.mRetransmitEpoch)
                        dtlsEpoch = this.mCurrentEpoch;
                    if (dtlsEpoch == null)
                        throw new InvalidOperationException();
                    byte[] buf1 = new byte[1] { 1 };
                    this.SendRecord( 20, buf1, 0, buf1.Length );
                    this.mWriteEpoch = dtlsEpoch;
                }
            }
            this.SendRecord( contentType, buf, off, len );
        }

        public virtual void Close()
        {
            if (this.mClosed)
                return;
            if (this.mInHandshake)
                this.Warn( 90, "User canceled handshake" );
            this.CloseTransport();
        }

        internal virtual void Fail( byte alertDescription )
        {
            if (this.mClosed)
                return;
            try
            {
                this.RaiseAlert( 2, alertDescription, null, null );
            }
            catch (Exception ex)
            {
            }
            this.mFailed = true;
            this.CloseTransport();
        }

        internal virtual void Warn( byte alertDescription, string message ) => this.RaiseAlert( 1, alertDescription, message, null );

        private void CloseTransport()
        {
            if (this.mClosed)
                return;
            try
            {
                if (!this.mFailed)
                    this.Warn( 0, null );
                this.mTransport.Close();
            }
            catch (Exception ex)
            {
            }
            this.mClosed = true;
        }

        private void RaiseAlert(
          byte alertLevel,
          byte alertDescription,
          string message,
          Exception cause )
        {
            this.mPeer.NotifyAlertRaised( alertLevel, alertDescription, message, cause );
            this.SendRecord( 21, new byte[2]
            {
        alertLevel,
        alertDescription
            }, 0, 2 );
        }

        private int ReceiveRecord( byte[] buf, int off, int len, int waitMillis )
        {
            if (this.mRecordQueue.Available > 0)
            {
                int num = 0;
                if (this.mRecordQueue.Available >= 13)
                {
                    byte[] buf1 = new byte[2];
                    this.mRecordQueue.Read( buf1, 0, 2, 11 );
                    num = TlsUtilities.ReadUint16( buf1, 0 );
                }
                int len1 = System.Math.Min( this.mRecordQueue.Available, 13 + num );
                this.mRecordQueue.RemoveData( buf, off, len1, 0 );
                return len1;
            }
            int record = this.mTransport.Receive( buf, off, len, waitMillis );
            if (record >= 13)
            {
                int num = 13 + TlsUtilities.ReadUint16( buf, off + 11 );
                if (record > num)
                {
                    this.mRecordQueue.AddData( buf, off + num, record - num );
                    record = num;
                }
            }
            return record;
        }

        private void SendRecord( byte contentType, byte[] buf, int off, int len )
        {
            if (this.mWriteVersion == null)
                return;
            if (len > this.mPlaintextLimit)
                throw new TlsFatalAlert( 80 );
            if (len < 1 && contentType != 23)
                throw new TlsFatalAlert( 80 );
            int epoch = this.mWriteEpoch.Epoch;
            long num = this.mWriteEpoch.AllocateSequenceNumber();
            byte[] sourceArray = this.mWriteEpoch.Cipher.EncodePlaintext( GetMacSequenceNumber( epoch, num ), contentType, buf, off, len );
            byte[] numArray = new byte[sourceArray.Length + 13];
            TlsUtilities.WriteUint8( contentType, numArray, 0 );
            TlsUtilities.WriteVersion( this.mWriteVersion, numArray, 1 );
            TlsUtilities.WriteUint16( epoch, numArray, 3 );
            TlsUtilities.WriteUint48( num, numArray, 5 );
            TlsUtilities.WriteUint16( sourceArray.Length, numArray, 11 );
            Array.Copy( sourceArray, 0, numArray, 13, sourceArray.Length );
            this.mTransport.Send( numArray, 0, numArray.Length );
        }

        private static long GetMacSequenceNumber( int epoch, long sequence_number ) => ((epoch & uint.MaxValue) << 48) | sequence_number;
    }
}
