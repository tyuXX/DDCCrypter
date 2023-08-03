// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.RecordStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class RecordStream
    {
        private const int DEFAULT_PLAINTEXT_LIMIT = 16384;
        internal const int TLS_HEADER_SIZE = 5;
        internal const int TLS_HEADER_TYPE_OFFSET = 0;
        internal const int TLS_HEADER_VERSION_OFFSET = 1;
        internal const int TLS_HEADER_LENGTH_OFFSET = 3;
        private TlsProtocol mHandler;
        private Stream mInput;
        private Stream mOutput;
        private TlsCompression mPendingCompression = null;
        private TlsCompression mReadCompression = null;
        private TlsCompression mWriteCompression = null;
        private TlsCipher mPendingCipher = null;
        private TlsCipher mReadCipher = null;
        private TlsCipher mWriteCipher = null;
        private long mReadSeqNo = 0;
        private long mWriteSeqNo = 0;
        private MemoryStream mBuffer = new();
        private TlsHandshakeHash mHandshakeHash = null;
        private ProtocolVersion mReadVersion = null;
        private ProtocolVersion mWriteVersion = null;
        private bool mRestrictReadVersion = true;
        private int mPlaintextLimit;
        private int mCompressedLimit;
        private int mCiphertextLimit;

        internal RecordStream( TlsProtocol handler, Stream input, Stream output )
        {
            this.mHandler = handler;
            this.mInput = input;
            this.mOutput = output;
            this.mReadCompression = new TlsNullCompression();
            this.mWriteCompression = this.mReadCompression;
        }

        internal virtual void Init( TlsContext context )
        {
            this.mReadCipher = new TlsNullCipher( context );
            this.mWriteCipher = this.mReadCipher;
            this.mHandshakeHash = new DeferredHash();
            this.mHandshakeHash.Init( context );
            this.SetPlaintextLimit( 16384 );
        }

        internal virtual int GetPlaintextLimit() => this.mPlaintextLimit;

        internal virtual void SetPlaintextLimit( int plaintextLimit )
        {
            this.mPlaintextLimit = plaintextLimit;
            this.mCompressedLimit = this.mPlaintextLimit + 1024;
            this.mCiphertextLimit = this.mCompressedLimit + 1024;
        }

        internal virtual ProtocolVersion ReadVersion
        {
            get => this.mReadVersion;
            set => this.mReadVersion = value;
        }

        internal virtual void SetWriteVersion( ProtocolVersion writeVersion ) => this.mWriteVersion = writeVersion;

        internal virtual void SetRestrictReadVersion( bool enabled ) => this.mRestrictReadVersion = enabled;

        internal virtual void SetPendingConnectionState(
          TlsCompression tlsCompression,
          TlsCipher tlsCipher )
        {
            this.mPendingCompression = tlsCompression;
            this.mPendingCipher = tlsCipher;
        }

        internal virtual void SentWriteCipherSpec()
        {
            this.mWriteCompression = this.mPendingCompression != null && this.mPendingCipher != null ? this.mPendingCompression : throw new TlsFatalAlert( 40 );
            this.mWriteCipher = this.mPendingCipher;
            this.mWriteSeqNo = 0L;
        }

        internal virtual void ReceivedReadCipherSpec()
        {
            this.mReadCompression = this.mPendingCompression != null && this.mPendingCipher != null ? this.mPendingCompression : throw new TlsFatalAlert( 40 );
            this.mReadCipher = this.mPendingCipher;
            this.mReadSeqNo = 0L;
        }

        internal virtual void FinaliseHandshake()
        {
            if (this.mReadCompression != this.mPendingCompression || this.mWriteCompression != this.mPendingCompression || this.mReadCipher != this.mPendingCipher || this.mWriteCipher != this.mPendingCipher)
                throw new TlsFatalAlert( 40 );
            this.mPendingCompression = null;
            this.mPendingCipher = null;
        }

        internal virtual bool ReadRecord()
        {
            byte[] buf1 = TlsUtilities.ReadAllOrNothing( 5, this.mInput );
            if (buf1 == null)
                return false;
            byte num = TlsUtilities.ReadUint8( buf1, 0 );
            CheckType( num, 10 );
            if (!this.mRestrictReadVersion)
            {
                if ((TlsUtilities.ReadVersionRaw( buf1, 1 ) & 4294967040L) != 768L)
                    throw new TlsFatalAlert( 47 );
            }
            else
            {
                ProtocolVersion protocolVersion = TlsUtilities.ReadVersion( buf1, 1 );
                if (this.mReadVersion == null)
                    this.mReadVersion = protocolVersion;
                else if (!protocolVersion.Equals( this.mReadVersion ))
                    throw new TlsFatalAlert( 47 );
            }
            int len = TlsUtilities.ReadUint16( buf1, 3 );
            byte[] buf2 = this.DecodeAndVerify( num, this.mInput, len );
            this.mHandler.ProcessRecord( num, buf2, 0, buf2.Length );
            return true;
        }

        internal virtual byte[] DecodeAndVerify( byte type, Stream input, int len )
        {
            CheckLength( len, this.mCiphertextLimit, 22 );
            byte[] ciphertext = TlsUtilities.ReadFully( len, input );
            byte[] buffer = this.mReadCipher.DecodeCiphertext( this.mReadSeqNo++, type, ciphertext, 0, ciphertext.Length );
            CheckLength( buffer.Length, this.mCompressedLimit, 22 );
            Stream stream = this.mReadCompression.Decompress( mBuffer );
            if (stream != this.mBuffer)
            {
                stream.Write( buffer, 0, buffer.Length );
                stream.Flush();
                buffer = this.GetBufferContents();
            }
            CheckLength( buffer.Length, this.mPlaintextLimit, 30 );
            if (buffer.Length < 1 && type != 23)
                throw new TlsFatalAlert( 47 );
            return buffer;
        }

        internal virtual void WriteRecord(
          byte type,
          byte[] plaintext,
          int plaintextOffset,
          int plaintextLength )
        {
            if (this.mWriteVersion == null)
                return;
            CheckType( type, 80 );
            CheckLength( plaintextLength, this.mPlaintextLimit, 80 );
            if (plaintextLength < 1 && type != 23)
                throw new TlsFatalAlert( 80 );
            if (type == 22)
                this.UpdateHandshakeData( plaintext, plaintextOffset, plaintextLength );
            Stream stream = this.mWriteCompression.Compress( mBuffer );
            byte[] sourceArray;
            if (stream == this.mBuffer)
            {
                sourceArray = this.mWriteCipher.EncodePlaintext( this.mWriteSeqNo++, type, plaintext, plaintextOffset, plaintextLength );
            }
            else
            {
                stream.Write( plaintext, plaintextOffset, plaintextLength );
                stream.Flush();
                byte[] bufferContents = this.GetBufferContents();
                CheckLength( bufferContents.Length, plaintextLength + 1024, 80 );
                sourceArray = this.mWriteCipher.EncodePlaintext( this.mWriteSeqNo++, type, bufferContents, 0, bufferContents.Length );
            }
            CheckLength( sourceArray.Length, this.mCiphertextLimit, 80 );
            byte[] numArray = new byte[sourceArray.Length + 5];
            TlsUtilities.WriteUint8( type, numArray, 0 );
            TlsUtilities.WriteVersion( this.mWriteVersion, numArray, 1 );
            TlsUtilities.WriteUint16( sourceArray.Length, numArray, 3 );
            Array.Copy( sourceArray, 0, numArray, 5, sourceArray.Length );
            this.mOutput.Write( numArray, 0, numArray.Length );
            this.mOutput.Flush();
        }

        internal virtual void NotifyHelloComplete() => this.mHandshakeHash = this.mHandshakeHash.NotifyPrfDetermined();

        internal virtual TlsHandshakeHash HandshakeHash => this.mHandshakeHash;

        internal virtual TlsHandshakeHash PrepareToFinish()
        {
            TlsHandshakeHash mHandshakeHash = this.mHandshakeHash;
            this.mHandshakeHash = this.mHandshakeHash.StopTracking();
            return mHandshakeHash;
        }

        internal virtual void UpdateHandshakeData( byte[] message, int offset, int len ) => this.mHandshakeHash.BlockUpdate( message, offset, len );

        internal virtual void SafeClose()
        {
            try
            {
                Platform.Dispose( this.mInput );
            }
            catch (IOException ex)
            {
            }
            try
            {
                Platform.Dispose( this.mOutput );
            }
            catch (IOException ex)
            {
            }
        }

        internal virtual void Flush() => this.mOutput.Flush();

        private byte[] GetBufferContents()
        {
            byte[] array = this.mBuffer.ToArray();
            this.mBuffer.SetLength( 0L );
            return array;
        }

        private static void CheckType( byte type, byte alertDescription )
        {
            switch (type)
            {
                case 20:
                    break;
                case 21:
                    break;
                case 22:
                    break;
                case 23:
                    break;
                case 24:
                    break;
                default:
                    throw new TlsFatalAlert( alertDescription );
            }
        }

        private static void CheckLength( int length, int limit, byte alertDescription )
        {
            if (length > limit)
                throw new TlsFatalAlert( alertDescription );
        }
    }
}
