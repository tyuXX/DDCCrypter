// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpUserAttributeSubpacketVector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Attr;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpUserAttributeSubpacketVector
    {
        private readonly UserAttributeSubpacket[] packets;

        internal PgpUserAttributeSubpacketVector( UserAttributeSubpacket[] packets ) => this.packets = packets;

        public UserAttributeSubpacket GetSubpacket( UserAttributeSubpacketTag type )
        {
            for (int index = 0; index != this.packets.Length; ++index)
            {
                if (this.packets[index].SubpacketType == type)
                    return this.packets[index];
            }
            return null;
        }

        public ImageAttrib GetImageAttribute() => (ImageAttrib)this.GetSubpacket( UserAttributeSubpacketTag.ImageAttribute ) ?? null;

        internal UserAttributeSubpacket[] ToSubpacketArray() => this.packets;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            if (!(obj is PgpUserAttributeSubpacketVector attributeSubpacketVector) || attributeSubpacketVector.packets.Length != this.packets.Length)
                return false;
            for (int index = 0; index != this.packets.Length; ++index)
            {
                if (!attributeSubpacketVector.packets[index].Equals( this.packets[index] ))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (object packet in this.packets)
                hashCode ^= packet.GetHashCode();
            return hashCode;
        }
    }
}
