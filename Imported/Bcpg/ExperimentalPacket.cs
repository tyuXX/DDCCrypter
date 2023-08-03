// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ExperimentalPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public class ExperimentalPacket : ContainedPacket
    {
        private readonly PacketTag tag;
        private readonly byte[] contents;

        internal ExperimentalPacket( PacketTag tag, BcpgInputStream bcpgIn )
        {
            this.tag = tag;
            this.contents = bcpgIn.ReadAll();
        }

        public PacketTag Tag => this.tag;

        public byte[] GetContents() => (byte[])this.contents.Clone();

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( this.tag, this.contents, true );
    }
}
