// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.BcpgInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class BcpgInputStream : BaseInputStream
    {
        private Stream m_in;
        private bool next = false;
        private int nextB;

        internal static BcpgInputStream Wrap( Stream inStr ) => inStr is BcpgInputStream ? (BcpgInputStream)inStr : new BcpgInputStream( inStr );

        private BcpgInputStream( Stream inputStream ) => this.m_in = inputStream;

        public override int ReadByte()
        {
            if (!this.next)
                return this.m_in.ReadByte();
            this.next = false;
            return this.nextB;
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            if (!this.next)
                return this.m_in.Read( buffer, offset, count );
            if (this.nextB < 0)
                return 0;
            if (buffer == null)
                throw new ArgumentNullException( nameof( buffer ) );
            buffer[offset] = (byte)this.nextB;
            this.next = false;
            return 1;
        }

        public byte[] ReadAll() => Streams.ReadAll( this );

        public void ReadFully( byte[] buffer, int off, int len )
        {
            if (Streams.ReadFully( this, buffer, off, len ) < len)
                throw new EndOfStreamException();
        }

        public void ReadFully( byte[] buffer ) => this.ReadFully( buffer, 0, buffer.Length );

        public PacketTag NextPacketTag()
        {
            if (!this.next)
            {
                try
                {
                    this.nextB = this.m_in.ReadByte();
                }
                catch (EndOfStreamException ex)
                {
                    this.nextB = -1;
                }
                this.next = true;
            }
            if (this.nextB < 0)
                return (PacketTag)this.nextB;
            int num = this.nextB & 63;
            if ((this.nextB & 64) == 0)
                num >>= 2;
            return (PacketTag)num;
        }

        public Packet ReadPacket()
        {
            int num1 = this.ReadByte();
            if (num1 < 0)
                return null;
            if ((num1 & 128) == 0)
                throw new IOException( "invalid header encountered" );
            bool flag = (num1 & 64) != 0;
            int dataLength = 0;
            bool partial = false;
            PacketTag tag;
            if (flag)
            {
                tag = (PacketTag)(num1 & 63);
                int num2 = this.ReadByte();
                if (num2 < 192)
                    dataLength = num2;
                else if (num2 <= 223)
                {
                    int num3 = this.m_in.ReadByte();
                    dataLength = ((num2 - 192) << 8) + num3 + 192;
                }
                else if (num2 == byte.MaxValue)
                {
                    dataLength = (this.m_in.ReadByte() << 24) | (this.m_in.ReadByte() << 16) | (this.m_in.ReadByte() << 8) | this.m_in.ReadByte();
                }
                else
                {
                    partial = true;
                    dataLength = 1 << num2;
                }
            }
            else
            {
                int num4 = num1 & 3;
                tag = (PacketTag)((num1 & 63) >> 2);
                switch (num4)
                {
                    case 0:
                        dataLength = this.ReadByte();
                        break;
                    case 1:
                        dataLength = (this.ReadByte() << 8) | this.ReadByte();
                        break;
                    case 2:
                        dataLength = (this.ReadByte() << 24) | (this.ReadByte() << 16) | (this.ReadByte() << 8) | this.ReadByte();
                        break;
                    case 3:
                        partial = true;
                        break;
                    default:
                        throw new IOException( "unknown length type encountered" );
                }
            }
            BcpgInputStream bcpgIn = dataLength != 0 || !partial ? new BcpgInputStream( new BcpgInputStream.PartialInputStream( this, partial, dataLength ) ) : this;
            switch (tag)
            {
                case PacketTag.Reserved:
                    return new InputStreamPacket( bcpgIn );
                case PacketTag.PublicKeyEncryptedSession:
                    return new PublicKeyEncSessionPacket( bcpgIn );
                case PacketTag.Signature:
                    return new SignaturePacket( bcpgIn );
                case PacketTag.SymmetricKeyEncryptedSessionKey:
                    return new SymmetricKeyEncSessionPacket( bcpgIn );
                case PacketTag.OnePassSignature:
                    return new OnePassSignaturePacket( bcpgIn );
                case PacketTag.SecretKey:
                    return new SecretKeyPacket( bcpgIn );
                case PacketTag.PublicKey:
                    return new PublicKeyPacket( bcpgIn );
                case PacketTag.SecretSubkey:
                    return new SecretSubkeyPacket( bcpgIn );
                case PacketTag.CompressedData:
                    return new CompressedDataPacket( bcpgIn );
                case PacketTag.SymmetricKeyEncrypted:
                    return new SymmetricEncDataPacket( bcpgIn );
                case PacketTag.Marker:
                    return new MarkerPacket( bcpgIn );
                case PacketTag.LiteralData:
                    return new LiteralDataPacket( bcpgIn );
                case PacketTag.Trust:
                    return new TrustPacket( bcpgIn );
                case PacketTag.UserId:
                    return new UserIdPacket( bcpgIn );
                case PacketTag.PublicSubkey:
                    return new PublicSubkeyPacket( bcpgIn );
                case PacketTag.UserAttribute:
                    return new UserAttributePacket( bcpgIn );
                case PacketTag.SymmetricEncryptedIntegrityProtected:
                    return new SymmetricEncIntegrityPacket( bcpgIn );
                case PacketTag.ModificationDetectionCode:
                    return new ModDetectionCodePacket( bcpgIn );
                case PacketTag.Experimental1:
                case PacketTag.Experimental2:
                case PacketTag.Experimental3:
                case PacketTag.Experimental4:
                    return new ExperimentalPacket( tag, bcpgIn );
                default:
                    throw new IOException( "unknown packet type encountered: " + tag );
            }
        }

        public override void Close()
        {
            Platform.Dispose( this.m_in );
            base.Close();
        }

        private class PartialInputStream : BaseInputStream
        {
            private BcpgInputStream m_in;
            private bool partial;
            private int dataLength;

            internal PartialInputStream( BcpgInputStream bcpgIn, bool partial, int dataLength )
            {
                this.m_in = bcpgIn;
                this.partial = partial;
                this.dataLength = dataLength;
            }

            public override int ReadByte()
            {
                while (this.dataLength == 0)
                {
                    if (!this.partial || this.ReadPartialDataLength() < 0)
                        return -1;
                }
                int num = this.m_in.ReadByte();
                if (num < 0)
                    throw new EndOfStreamException( "Premature end of stream in PartialInputStream" );
                --this.dataLength;
                return num;
            }

            public override int Read( byte[] buffer, int offset, int count )
            {
                while (this.dataLength == 0)
                {
                    if (!this.partial || this.ReadPartialDataLength() < 0)
                        return 0;
                }
                int count1 = this.dataLength > count || this.dataLength < 0 ? count : this.dataLength;
                int num = this.m_in.Read( buffer, offset, count1 );
                if (num < 1)
                    throw new EndOfStreamException( "Premature end of stream in PartialInputStream" );
                this.dataLength -= num;
                return num;
            }

            private int ReadPartialDataLength()
            {
                int num = this.m_in.ReadByte();
                if (num < 0)
                    return -1;
                this.partial = false;
                if (num < 192)
                    this.dataLength = num;
                else if (num <= 223)
                    this.dataLength = ((num - 192) << 8) + this.m_in.ReadByte() + 192;
                else if (num == byte.MaxValue)
                {
                    this.dataLength = (this.m_in.ReadByte() << 24) | (this.m_in.ReadByte() << 16) | (this.m_in.ReadByte() << 8) | this.m_in.ReadByte();
                }
                else
                {
                    this.partial = true;
                    this.dataLength = 1 << num;
                }
                return 0;
            }
        }
    }
}
