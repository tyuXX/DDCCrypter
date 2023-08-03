// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.UserIdPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Text;

namespace Org.BouncyCastle.Bcpg
{
    public class UserIdPacket : ContainedPacket
    {
        private readonly byte[] idData;

        public UserIdPacket( BcpgInputStream bcpgIn ) => this.idData = bcpgIn.ReadAll();

        public UserIdPacket( string id ) => this.idData = Encoding.UTF8.GetBytes( id );

        public string GetId() => Encoding.UTF8.GetString( this.idData, 0, this.idData.Length );

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.UserId, this.idData, true );
    }
}
