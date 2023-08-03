// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.UserAttributeSubpacketsParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Attr;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class UserAttributeSubpacketsParser
    {
        private readonly Stream input;

        public UserAttributeSubpacketsParser( Stream input ) => this.input = input;

        public virtual UserAttributeSubpacket ReadPacket()
        {
            int num1 = this.input.ReadByte();
            if (num1 < 0)
                return null;
            bool forceLongLength = false;
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
                    throw new IOException( "unrecognised length reading user attribute sub packet" );
                num2 = (this.input.ReadByte() << 24) | (this.input.ReadByte() << 16) | (this.input.ReadByte() << 8) | this.input.ReadByte();
                forceLongLength = true;
            }
            int num3 = this.input.ReadByte();
            if (num3 < 0)
                throw new EndOfStreamException( "unexpected EOF reading user attribute sub packet" );
            byte[] numArray = new byte[num2 - 1];
            if (Streams.ReadFully( this.input, numArray ) < numArray.Length)
                throw new EndOfStreamException();
            UserAttributeSubpacketTag type = (UserAttributeSubpacketTag)num3;
            return type == UserAttributeSubpacketTag.ImageAttribute ? new ImageAttrib( forceLongLength, numArray ) : new UserAttributeSubpacket( type, forceLongLength, numArray );
        }
    }
}
