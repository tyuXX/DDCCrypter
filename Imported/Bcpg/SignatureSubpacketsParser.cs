// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SignatureSubpacketsParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class SignatureSubpacketsParser
    {
        private readonly Stream input;

        public SignatureSubpacketsParser( Stream input ) => this.input = input;

        public SignatureSubpacket ReadPacket()
        {
            int num1 = this.input.ReadByte();
            if (num1 < 0)
                return null;
            bool isLongLength = false;
            int num2;
            if (num1 < 192)
                num2 = num1;
            else if (num1 <= 223)
            {
                num2 = ((num1 - 192) << 8) + this.input.ReadByte() + 192;
            }
            else
            {
                if (num1 != byte.MaxValue)
                    throw new IOException( "unexpected length header" );
                isLongLength = true;
                num2 = (this.input.ReadByte() << 24) | (this.input.ReadByte() << 16) | (this.input.ReadByte() << 8) | this.input.ReadByte();
            }
            int num3 = this.input.ReadByte();
            if (num3 < 0)
                throw new EndOfStreamException( "unexpected EOF reading signature sub packet" );
            byte[] numArray = new byte[num2 - 1];
            int bytesRead = Streams.ReadFully( this.input, numArray );
            bool critical = (num3 & 128) != 0;
            SignatureSubpacketTag type = (SignatureSubpacketTag)(num3 & sbyte.MaxValue);
            if (bytesRead != numArray.Length)
            {
                switch (type)
                {
                    case SignatureSubpacketTag.CreationTime:
                        numArray = this.CheckData( numArray, 4, bytesRead, "Signature Creation Time" );
                        break;
                    case SignatureSubpacketTag.ExpireTime:
                        numArray = this.CheckData( numArray, 4, bytesRead, "Signature Expiration Time" );
                        break;
                    case SignatureSubpacketTag.KeyExpireTime:
                        numArray = this.CheckData( numArray, 4, bytesRead, "Signature Key Expiration Time" );
                        break;
                    case SignatureSubpacketTag.IssuerKeyId:
                        numArray = this.CheckData( numArray, 8, bytesRead, "Issuer" );
                        break;
                    default:
                        throw new EndOfStreamException( "truncated subpacket data." );
                }
            }
            switch (type)
            {
                case SignatureSubpacketTag.CreationTime:
                    return new SignatureCreationTime( critical, isLongLength, numArray );
                case SignatureSubpacketTag.ExpireTime:
                    return new SignatureExpirationTime( critical, isLongLength, numArray );
                case SignatureSubpacketTag.Exportable:
                    return new Exportable( critical, isLongLength, numArray );
                case SignatureSubpacketTag.TrustSig:
                    return new TrustSignature( critical, isLongLength, numArray );
                case SignatureSubpacketTag.Revocable:
                    return new Revocable( critical, isLongLength, numArray );
                case SignatureSubpacketTag.KeyExpireTime:
                    return new KeyExpirationTime( critical, isLongLength, numArray );
                case SignatureSubpacketTag.PreferredSymmetricAlgorithms:
                case SignatureSubpacketTag.PreferredHashAlgorithms:
                case SignatureSubpacketTag.PreferredCompressionAlgorithms:
                    return new PreferredAlgorithms( type, critical, isLongLength, numArray );
                case SignatureSubpacketTag.IssuerKeyId:
                    return new IssuerKeyId( critical, isLongLength, numArray );
                case SignatureSubpacketTag.NotationData:
                    return new NotationData( critical, isLongLength, numArray );
                case SignatureSubpacketTag.PrimaryUserId:
                    return new PrimaryUserId( critical, isLongLength, numArray );
                case SignatureSubpacketTag.KeyFlags:
                    return new KeyFlags( critical, isLongLength, numArray );
                case SignatureSubpacketTag.SignerUserId:
                    return new SignerUserId( critical, isLongLength, numArray );
                default:
                    return new SignatureSubpacket( type, critical, isLongLength, numArray );
            }
        }

        private byte[] CheckData( byte[] data, int expected, int bytesRead, string name ) => bytesRead == expected ? Arrays.CopyOfRange( data, 0, expected ) : throw new EndOfStreamException( "truncated " + name + " subpacket data." );
    }
}
