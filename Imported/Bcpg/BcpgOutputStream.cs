// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.BcpgOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class BcpgOutputStream : BaseOutputStream
    {
        private const int BufferSizePower = 16;
        private Stream outStr;
        private byte[] partialBuffer;
        private int partialBufferLength;
        private int partialPower;
        private int partialOffset;

        internal static BcpgOutputStream Wrap( Stream outStr ) => outStr is BcpgOutputStream ? (BcpgOutputStream)outStr : new BcpgOutputStream( outStr );

        public BcpgOutputStream( Stream outStr ) => this.outStr = outStr != null ? outStr : throw new ArgumentNullException( nameof( outStr ) );

        public BcpgOutputStream( Stream outStr, PacketTag tag )
        {
            this.outStr = outStr != null ? outStr : throw new ArgumentNullException( nameof( outStr ) );
            this.WriteHeader( tag, true, true, 0L );
        }

        public BcpgOutputStream( Stream outStr, PacketTag tag, long length, bool oldFormat )
        {
            this.outStr = outStr != null ? outStr : throw new ArgumentNullException( nameof( outStr ) );
            if (length > uint.MaxValue)
            {
                this.WriteHeader( tag, false, true, 0L );
                this.partialBufferLength = 65536;
                this.partialBuffer = new byte[this.partialBufferLength];
                this.partialPower = 16;
                this.partialOffset = 0;
            }
            else
                this.WriteHeader( tag, oldFormat, false, length );
        }

        public BcpgOutputStream( Stream outStr, PacketTag tag, long length )
        {
            this.outStr = outStr != null ? outStr : throw new ArgumentNullException( nameof( outStr ) );
            this.WriteHeader( tag, false, false, length );
        }

        public BcpgOutputStream( Stream outStr, PacketTag tag, byte[] buffer )
        {
            this.outStr = outStr != null ? outStr : throw new ArgumentNullException( nameof( outStr ) );
            this.WriteHeader( tag, false, true, 0L );
            this.partialBuffer = buffer;
            uint length = (uint)this.partialBuffer.Length;
            this.partialPower = 0;
            while (length != 1U)
            {
                length >>= 1;
                ++this.partialPower;
            }
            if (this.partialPower > 30)
                throw new IOException( "Buffer cannot be greater than 2^30 in length." );
            this.partialBufferLength = 1 << this.partialPower;
            this.partialOffset = 0;
        }

        private void WriteNewPacketLength( long bodyLen )
        {
            if (bodyLen < 192L)
                this.outStr.WriteByte( (byte)bodyLen );
            else if (bodyLen <= 8383L)
            {
                bodyLen -= 192L;
                this.outStr.WriteByte( (byte)((ulong)((bodyLen >> 8) & byte.MaxValue) + 192UL) );
                this.outStr.WriteByte( (byte)bodyLen );
            }
            else
            {
                this.outStr.WriteByte( byte.MaxValue );
                this.outStr.WriteByte( (byte)(bodyLen >> 24) );
                this.outStr.WriteByte( (byte)(bodyLen >> 16) );
                this.outStr.WriteByte( (byte)(bodyLen >> 8) );
                this.outStr.WriteByte( (byte)bodyLen );
            }
        }

        private void WriteHeader( PacketTag tag, bool oldPackets, bool partial, long bodyLen )
        {
            int num1 = 128;
            if (this.partialBuffer != null)
            {
                this.PartialFlush( true );
                this.partialBuffer = null;
            }
            if (oldPackets)
            {
                int num2 = num1 | ((int)tag << 2);
                if (partial)
                    this.WriteByte( (byte)(num2 | 3) );
                else if (bodyLen <= byte.MaxValue)
                {
                    this.WriteByte( (byte)num2 );
                    this.WriteByte( (byte)bodyLen );
                }
                else if (bodyLen <= ushort.MaxValue)
                {
                    this.WriteByte( (byte)(num2 | 1) );
                    this.WriteByte( (byte)(bodyLen >> 8) );
                    this.WriteByte( (byte)bodyLen );
                }
                else
                {
                    this.WriteByte( (byte)(num2 | 2) );
                    this.WriteByte( (byte)(bodyLen >> 24) );
                    this.WriteByte( (byte)(bodyLen >> 16) );
                    this.WriteByte( (byte)(bodyLen >> 8) );
                    this.WriteByte( (byte)bodyLen );
                }
            }
            else
            {
                this.WriteByte( (byte)((PacketTag)num1 | (PacketTag)64 | tag) );
                if (partial)
                    this.partialOffset = 0;
                else
                    this.WriteNewPacketLength( bodyLen );
            }
        }

        private void PartialFlush( bool isLast )
        {
            if (isLast)
            {
                this.WriteNewPacketLength( partialOffset );
                this.outStr.Write( this.partialBuffer, 0, this.partialOffset );
            }
            else
            {
                this.outStr.WriteByte( (byte)(224 | this.partialPower) );
                this.outStr.Write( this.partialBuffer, 0, this.partialBufferLength );
            }
            this.partialOffset = 0;
        }

        private void WritePartial( byte b )
        {
            if (this.partialOffset == this.partialBufferLength)
                this.PartialFlush( false );
            this.partialBuffer[this.partialOffset++] = b;
        }

        private void WritePartial( byte[] buffer, int off, int len )
        {
            if (this.partialOffset == this.partialBufferLength)
                this.PartialFlush( false );
            if (len <= this.partialBufferLength - this.partialOffset)
            {
                Array.Copy( buffer, off, partialBuffer, this.partialOffset, len );
                this.partialOffset += len;
            }
            else
            {
                int length = this.partialBufferLength - this.partialOffset;
                Array.Copy( buffer, off, partialBuffer, this.partialOffset, length );
                off += length;
                len -= length;
                this.PartialFlush( false );
                while (len > this.partialBufferLength)
                {
                    Array.Copy( buffer, off, partialBuffer, 0, this.partialBufferLength );
                    off += this.partialBufferLength;
                    len -= this.partialBufferLength;
                    this.PartialFlush( false );
                }
                Array.Copy( buffer, off, partialBuffer, 0, len );
                this.partialOffset += len;
            }
        }

        public override void WriteByte( byte value )
        {
            if (this.partialBuffer != null)
                this.WritePartial( value );
            else
                this.outStr.WriteByte( value );
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            if (this.partialBuffer != null)
                this.WritePartial( buffer, offset, count );
            else
                this.outStr.Write( buffer, offset, count );
        }

        internal virtual void WriteShort( short n ) => this.Write( (byte)((uint)n >> 8), (byte)n );

        internal virtual void WriteInt( int n ) => this.Write( (byte)(n >> 24), (byte)(n >> 16), (byte)(n >> 8), (byte)n );

        internal virtual void WriteLong( long n ) => this.Write( (byte)(n >> 56), (byte)(n >> 48), (byte)(n >> 40), (byte)(n >> 32), (byte)(n >> 24), (byte)(n >> 16), (byte)(n >> 8), (byte)n );

        public void WritePacket( ContainedPacket p ) => p.Encode( this );

        internal void WritePacket( PacketTag tag, byte[] body, bool oldFormat )
        {
            this.WriteHeader( tag, oldFormat, false, body.Length );
            this.Write( body );
        }

        public void WriteObject( BcpgObject bcpgObject ) => bcpgObject.Encode( this );

        public void WriteObjects( params BcpgObject[] v )
        {
            foreach (BcpgObject bcpgObject in v)
                bcpgObject.Encode( this );
        }

        public override void Flush() => this.outStr.Flush();

        public void Finish()
        {
            if (this.partialBuffer == null)
                return;
            this.PartialFlush( true );
            this.partialBuffer = null;
        }

        public override void Close()
        {
            this.Finish();
            this.outStr.Flush();
            Platform.Dispose( this.outStr );
            base.Close();
        }
    }
}
