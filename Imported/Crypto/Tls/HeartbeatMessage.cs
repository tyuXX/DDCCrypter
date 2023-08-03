// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.HeartbeatMessage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class HeartbeatMessage
    {
        protected readonly byte mType;
        protected readonly byte[] mPayload;
        protected readonly int mPaddingLength;

        public HeartbeatMessage( byte type, byte[] payload, int paddingLength )
        {
            if (!HeartbeatMessageType.IsValid( type ))
                throw new ArgumentException( "not a valid HeartbeatMessageType value", nameof( type ) );
            if (payload == null || payload.Length >= 65536)
                throw new ArgumentException( "must have length < 2^16", nameof( payload ) );
            if (paddingLength < 16)
                throw new ArgumentException( "must be at least 16", nameof( paddingLength ) );
            this.mType = type;
            this.mPayload = payload;
            this.mPaddingLength = paddingLength;
        }

        public virtual void Encode( TlsContext context, Stream output )
        {
            TlsUtilities.WriteUint8( this.mType, output );
            TlsUtilities.CheckUint16( this.mPayload.Length );
            TlsUtilities.WriteUint16( this.mPayload.Length, output );
            output.Write( this.mPayload, 0, this.mPayload.Length );
            byte[] numArray = new byte[this.mPaddingLength];
            context.NonceRandomGenerator.NextBytes( numArray );
            output.Write( numArray, 0, numArray.Length );
        }

        public static HeartbeatMessage Parse( Stream input )
        {
            byte num = TlsUtilities.ReadUint8( input );
            if (!HeartbeatMessageType.IsValid( num ))
                throw new TlsFatalAlert( 47 );
            int payloadLength = TlsUtilities.ReadUint16( input );
            HeartbeatMessage.PayloadBuffer outStr = new();
            Streams.PipeAll( input, outStr );
            byte[] truncatedByteArray = outStr.ToTruncatedByteArray( payloadLength );
            if (truncatedByteArray == null)
                return null;
            TlsUtilities.CheckUint16( outStr.Length );
            int paddingLength = (int)outStr.Length - truncatedByteArray.Length;
            return new HeartbeatMessage( num, truncatedByteArray, paddingLength );
        }

        internal class PayloadBuffer : MemoryStream
        {
            internal byte[] ToTruncatedByteArray( int payloadLength ) => this.Length < payloadLength + 16 ? null : Arrays.CopyOf( this.GetBuffer(), payloadLength );
        }
    }
}
