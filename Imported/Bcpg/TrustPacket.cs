// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.TrustPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class TrustPacket : ContainedPacket
    {
        private readonly byte[] levelAndTrustAmount;

        public TrustPacket( BcpgInputStream bcpgIn )
        {
            MemoryStream memoryStream = new MemoryStream();
            int num;
            while ((num = bcpgIn.ReadByte()) >= 0)
                memoryStream.WriteByte( (byte)num );
            this.levelAndTrustAmount = memoryStream.ToArray();
        }

        public TrustPacket( int trustCode ) => this.levelAndTrustAmount = new byte[1]
        {
      (byte) trustCode
        };

        public byte[] GetLevelAndTrustAmount() => (byte[])this.levelAndTrustAmount.Clone();

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.Trust, this.levelAndTrustAmount, true );
    }
}
