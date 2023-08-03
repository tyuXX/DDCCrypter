// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.UserAttributeSubpacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class UserAttributeSubpacket
    {
        internal readonly UserAttributeSubpacketTag type;
        private readonly bool longLength;
        protected readonly byte[] data;

        protected internal UserAttributeSubpacket( UserAttributeSubpacketTag type, byte[] data )
          : this( type, false, data )
        {
        }

        protected internal UserAttributeSubpacket(
          UserAttributeSubpacketTag type,
          bool forceLongLength,
          byte[] data )
        {
            this.type = type;
            this.longLength = forceLongLength;
            this.data = data;
        }

        public virtual UserAttributeSubpacketTag SubpacketType => this.type;

        public virtual byte[] GetData() => this.data;

        public virtual void Encode( Stream os )
        {
            int num1 = this.data.Length + 1;
            if (num1 < 192 && !this.longLength)
                os.WriteByte( (byte)num1 );
            else if (num1 <= 8383 && !this.longLength)
            {
                int num2 = num1 - 192;
                os.WriteByte( (byte)(((num2 >> 8) & byte.MaxValue) + 192) );
                os.WriteByte( (byte)num2 );
            }
            else
            {
                os.WriteByte( byte.MaxValue );
                os.WriteByte( (byte)(num1 >> 24) );
                os.WriteByte( (byte)(num1 >> 16) );
                os.WriteByte( (byte)(num1 >> 8) );
                os.WriteByte( (byte)num1 );
            }
            os.WriteByte( (byte)this.type );
            os.Write( this.data, 0, this.data.Length );
        }

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is UserAttributeSubpacket attributeSubpacket && this.type == attributeSubpacket.type && Arrays.AreEqual( this.data, attributeSubpacket.data );
        }

        public override int GetHashCode() => this.type.GetHashCode() ^ Arrays.GetHashCode( this.data );
    }
}
