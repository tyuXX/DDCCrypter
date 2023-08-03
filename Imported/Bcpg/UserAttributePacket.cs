// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.UserAttributePacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class UserAttributePacket : ContainedPacket
    {
        private readonly UserAttributeSubpacket[] subpackets;

        public UserAttributePacket( BcpgInputStream bcpgIn )
        {
            UserAttributeSubpacketsParser subpacketsParser = new UserAttributeSubpacketsParser( bcpgIn );
            IList arrayList = Platform.CreateArrayList();
            UserAttributeSubpacket attributeSubpacket;
            while ((attributeSubpacket = subpacketsParser.ReadPacket()) != null)
                arrayList.Add( attributeSubpacket );
            this.subpackets = new UserAttributeSubpacket[arrayList.Count];
            for (int index = 0; index != this.subpackets.Length; ++index)
                this.subpackets[index] = (UserAttributeSubpacket)arrayList[index];
        }

        public UserAttributePacket( UserAttributeSubpacket[] subpackets ) => this.subpackets = subpackets;

        public UserAttributeSubpacket[] GetSubpackets() => this.subpackets;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            MemoryStream os = new MemoryStream();
            for (int index = 0; index != this.subpackets.Length; ++index)
                this.subpackets[index].Encode( os );
            bcpgOut.WritePacket( PacketTag.UserAttribute, os.ToArray(), false );
        }
    }
}
