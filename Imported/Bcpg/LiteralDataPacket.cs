// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.LiteralDataPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Bcpg
{
    public class LiteralDataPacket : InputStreamPacket
    {
        private int format;
        private byte[] fileName;
        private long modDate;

        internal LiteralDataPacket( BcpgInputStream bcpgIn )
          : base( bcpgIn )
        {
            this.format = bcpgIn.ReadByte();
            int length = bcpgIn.ReadByte();
            this.fileName = new byte[length];
            for (int index = 0; index != length; ++index)
                this.fileName[index] = (byte)bcpgIn.ReadByte();
            this.modDate = (uint)((bcpgIn.ReadByte() << 24) | (bcpgIn.ReadByte() << 16) | (bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte()) * 1000L;
        }

        public int Format => this.format;

        public long ModificationTime => this.modDate;

        public string FileName => Strings.FromUtf8ByteArray( this.fileName );

        public byte[] GetRawFileName() => Arrays.Clone( this.fileName );
    }
}
