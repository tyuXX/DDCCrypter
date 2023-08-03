// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsReliableHandshake
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DtlsReliableHandshake
    {
        private const int MAX_RECEIVE_AHEAD = 10;
        private readonly DtlsRecordLayer mRecordLayer;
        private TlsHandshakeHash mHandshakeHash;
        private IDictionary mCurrentInboundFlight = Platform.CreateHashtable();
        private IDictionary mPreviousInboundFlight = null;
        private IList mOutboundFlight = Platform.CreateArrayList();
        private bool mSending = true;
        private int mMessageSeq = 0;
        private int mNextReceiveSeq = 0;

        internal DtlsReliableHandshake( TlsContext context, DtlsRecordLayer transport )
        {
            this.mRecordLayer = transport;
            this.mHandshakeHash = new DeferredHash();
            this.mHandshakeHash.Init( context );
        }

        internal void NotifyHelloComplete() => this.mHandshakeHash = this.mHandshakeHash.NotifyPrfDetermined();

        internal TlsHandshakeHash HandshakeHash => this.mHandshakeHash;

        internal TlsHandshakeHash PrepareToFinish()
        {
            TlsHandshakeHash mHandshakeHash = this.mHandshakeHash;
            this.mHandshakeHash = this.mHandshakeHash.StopTracking();
            return mHandshakeHash;
        }

        internal void SendMessage( byte msg_type, byte[] body )
        {
            TlsUtilities.CheckUint24( body.Length );
            if (!this.mSending)
            {
                this.CheckInboundFlight();
                this.mSending = true;
                this.mOutboundFlight.Clear();
            }
            DtlsReliableHandshake.Message message = new DtlsReliableHandshake.Message( this.mMessageSeq++, msg_type, body );
            this.mOutboundFlight.Add( message );
            this.WriteMessage( message );
            this.UpdateHandshakeMessagesDigest( message );
        }

        internal byte[] ReceiveMessageBody( byte msg_type )
        {
            DtlsReliableHandshake.Message message = this.ReceiveMessage();
            if (message.Type != msg_type)
                throw new TlsFatalAlert( 10 );
            return message.Body;
        }

        internal DtlsReliableHandshake.Message ReceiveMessage()
        {
            if (this.mSending)
            {
                this.mSending = false;
                this.PrepareInboundFlight();
            }
            DtlsReassembler dtlsReassembler1 = (DtlsReassembler)this.mCurrentInboundFlight[mNextReceiveSeq];
            if (dtlsReassembler1 != null)
            {
                byte[] bodyIfComplete = dtlsReassembler1.GetBodyIfComplete();
                if (bodyIfComplete != null)
                {
                    this.mPreviousInboundFlight = null;
                    return this.UpdateHandshakeMessagesDigest( new DtlsReliableHandshake.Message( this.mNextReceiveSeq++, dtlsReassembler1.MsgType, bodyIfComplete ) );
                }
            }
            byte[] buf = null;
            int waitMillis = 1000;
            while (true)
            {
                int receiveLimit = this.mRecordLayer.GetReceiveLimit();
                if (buf != null)
                {
                    if (buf.Length >= receiveLimit)
                        goto label_9;
                }
                buf = new byte[receiveLimit];
            label_9:
                try
                {
                    DtlsReassembler dtlsReassembler2 = new DtlsReassembler();
                    byte[] bodyIfComplete;
                    do
                    {
                        int key;
                        do
                        {
                            int fragment_length;
                            byte msg_type;
                            int length;
                            int fragment_offset;
                            do
                            {
                                do
                                {
                                    int num;
                                    do
                                    {
                                        do
                                        {
                                            num = this.mRecordLayer.Receive( buf, 0, receiveLimit, waitMillis );
                                            if (num < 0)
                                                goto label_25;
                                        }
                                        while (num < 12);
                                        fragment_length = TlsUtilities.ReadUint24( buf, 9 );
                                    }
                                    while (num != fragment_length + 12);
                                    key = TlsUtilities.ReadUint16( buf, 4 );
                                }
                                while (key > this.mNextReceiveSeq + 10);
                                msg_type = TlsUtilities.ReadUint8( buf, 0 );
                                length = TlsUtilities.ReadUint24( buf, 1 );
                                fragment_offset = TlsUtilities.ReadUint24( buf, 6 );
                            }
                            while (fragment_offset + fragment_length > length);
                            if (key < this.mNextReceiveSeq)
                            {
                                if (this.mPreviousInboundFlight != null)
                                {
                                    DtlsReassembler dtlsReassembler3 = (DtlsReassembler)this.mPreviousInboundFlight[key];
                                    if (dtlsReassembler3 != null)
                                    {
                                        dtlsReassembler3.ContributeFragment( msg_type, length, buf, 12, fragment_offset, fragment_length );
                                        if (CheckAll( this.mPreviousInboundFlight ))
                                        {
                                            this.ResendOutboundFlight();
                                            waitMillis = System.Math.Min( waitMillis * 2, 60000 );
                                            ResetAll( this.mPreviousInboundFlight );
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dtlsReassembler2 = (DtlsReassembler)this.mCurrentInboundFlight[key];
                                if (dtlsReassembler2 == null)
                                {
                                    dtlsReassembler2 = new DtlsReassembler( msg_type, length );
                                    this.mCurrentInboundFlight[key] = dtlsReassembler2;
                                }
                                dtlsReassembler2.ContributeFragment( msg_type, length, buf, 12, fragment_offset, fragment_length );
                            }
                        }
                        while (key != this.mNextReceiveSeq);
                        bodyIfComplete = dtlsReassembler2.GetBodyIfComplete();
                    }
                    while (bodyIfComplete == null);
                    this.mPreviousInboundFlight = null;
                    return this.UpdateHandshakeMessagesDigest( new DtlsReliableHandshake.Message( this.mNextReceiveSeq++, dtlsReassembler2.MsgType, bodyIfComplete ) );
                }
                catch (IOException ex)
                {
                }
            label_25:
                this.ResendOutboundFlight();
                waitMillis = System.Math.Min( waitMillis * 2, 60000 );
            }
        }

        internal void Finish()
        {
            DtlsHandshakeRetransmit retransmit = null;
            if (!this.mSending)
                this.CheckInboundFlight();
            else if (this.mCurrentInboundFlight != null)
                retransmit = new DtlsReliableHandshake.Retransmit( this );
            this.mRecordLayer.HandshakeSuccessful( retransmit );
        }

        internal void ResetHandshakeMessagesDigest() => this.mHandshakeHash.Reset();

        private void HandleRetransmittedHandshakeRecord( int epoch, byte[] buf, int off, int len )
        {
            if (len < 12)
                return;
            int fragment_length = TlsUtilities.ReadUint24( buf, off + 9 );
            if (len != fragment_length + 12)
                return;
            int key = TlsUtilities.ReadUint16( buf, off + 4 );
            if (key >= this.mNextReceiveSeq)
                return;
            byte msg_type = TlsUtilities.ReadUint8( buf, off );
            int num = msg_type == 20 ? 1 : 0;
            if (epoch != num)
                return;
            int length = TlsUtilities.ReadUint24( buf, off + 1 );
            int fragment_offset = TlsUtilities.ReadUint24( buf, off + 6 );
            if (fragment_offset + fragment_length > length)
                return;
            DtlsReassembler dtlsReassembler = (DtlsReassembler)this.mCurrentInboundFlight[key];
            if (dtlsReassembler == null)
                return;
            dtlsReassembler.ContributeFragment( msg_type, length, buf, off + 12, fragment_offset, fragment_length );
            if (!CheckAll( this.mCurrentInboundFlight ))
                return;
            this.ResendOutboundFlight();
            ResetAll( this.mCurrentInboundFlight );
        }

        private void CheckInboundFlight()
        {
            foreach (int key in (IEnumerable)this.mCurrentInboundFlight.Keys)
            {
                int mNextReceiveSeq = this.mNextReceiveSeq;
            }
        }

        private void PrepareInboundFlight()
        {
            ResetAll( this.mCurrentInboundFlight );
            this.mPreviousInboundFlight = this.mCurrentInboundFlight;
            this.mCurrentInboundFlight = Platform.CreateHashtable();
        }

        private void ResendOutboundFlight()
        {
            this.mRecordLayer.ResetWriteEpoch();
            for (int index = 0; index < this.mOutboundFlight.Count; ++index)
                this.WriteMessage( (DtlsReliableHandshake.Message)this.mOutboundFlight[index] );
        }

        private DtlsReliableHandshake.Message UpdateHandshakeMessagesDigest(
          DtlsReliableHandshake.Message message )
        {
            if (message.Type != 0)
            {
                byte[] body = message.Body;
                byte[] numArray = new byte[12];
                TlsUtilities.WriteUint8( message.Type, numArray, 0 );
                TlsUtilities.WriteUint24( body.Length, numArray, 1 );
                TlsUtilities.WriteUint16( message.Seq, numArray, 4 );
                TlsUtilities.WriteUint24( 0, numArray, 6 );
                TlsUtilities.WriteUint24( body.Length, numArray, 9 );
                this.mHandshakeHash.BlockUpdate( numArray, 0, numArray.Length );
                this.mHandshakeHash.BlockUpdate( body, 0, body.Length );
            }
            return message;
        }

        private void WriteMessage( DtlsReliableHandshake.Message message )
        {
            int val2 = this.mRecordLayer.GetSendLimit() - 12;
            if (val2 < 1)
                throw new TlsFatalAlert( 80 );
            int length = message.Body.Length;
            int fragment_offset = 0;
            do
            {
                int fragment_length = System.Math.Min( length - fragment_offset, val2 );
                this.WriteHandshakeFragment( message, fragment_offset, fragment_length );
                fragment_offset += fragment_length;
            }
            while (fragment_offset < length);
        }

        private void WriteHandshakeFragment(
          DtlsReliableHandshake.Message message,
          int fragment_offset,
          int fragment_length )
        {
            DtlsReliableHandshake.RecordLayerBuffer output = new DtlsReliableHandshake.RecordLayerBuffer( 12 + fragment_length );
            TlsUtilities.WriteUint8( message.Type, output );
            TlsUtilities.WriteUint24( message.Body.Length, output );
            TlsUtilities.WriteUint16( message.Seq, output );
            TlsUtilities.WriteUint24( fragment_offset, output );
            TlsUtilities.WriteUint24( fragment_length, output );
            output.Write( message.Body, fragment_offset, fragment_length );
            output.SendToRecordLayer( this.mRecordLayer );
        }

        private static bool CheckAll( IDictionary inboundFlight )
        {
            foreach (DtlsReassembler dtlsReassembler in (IEnumerable)inboundFlight.Values)
            {
                if (dtlsReassembler.GetBodyIfComplete() == null)
                    return false;
            }
            return true;
        }

        private static void ResetAll( IDictionary inboundFlight )
        {
            foreach (DtlsReassembler dtlsReassembler in (IEnumerable)inboundFlight.Values)
                dtlsReassembler.Reset();
        }

        internal class Message
        {
            private readonly int mMessageSeq;
            private readonly byte mMsgType;
            private readonly byte[] mBody;

            internal Message( int message_seq, byte msg_type, byte[] body )
            {
                this.mMessageSeq = message_seq;
                this.mMsgType = msg_type;
                this.mBody = body;
            }

            public int Seq => this.mMessageSeq;

            public byte Type => this.mMsgType;

            public byte[] Body => this.mBody;
        }

        internal class RecordLayerBuffer : MemoryStream
        {
            internal RecordLayerBuffer( int size )
              : base( size )
            {
            }

            internal void SendToRecordLayer( DtlsRecordLayer recordLayer )
            {
                byte[] buffer = this.GetBuffer();
                int length = (int)this.Length;
                recordLayer.Send( buffer, 0, length );
                Platform.Dispose( this );
            }
        }

        internal class Retransmit : DtlsHandshakeRetransmit
        {
            private readonly DtlsReliableHandshake mOuter;

            internal Retransmit( DtlsReliableHandshake outer ) => this.mOuter = outer;

            public void ReceivedHandshakeRecord( int epoch, byte[] buf, int off, int len ) => this.mOuter.HandleRetransmittedHandshakeRecord( epoch, buf, off, len );
        }
    }
}
