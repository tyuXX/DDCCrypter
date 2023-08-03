// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.MarkerPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public class MarkerPacket : ContainedPacket
    {
        private byte[] marker = new byte[3]
        {
       80,
       71,
       80
        };

        public MarkerPacket( BcpgInputStream bcpgIn ) => bcpgIn.ReadFully( this.marker );

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.Marker, this.marker, true );
    }
}
