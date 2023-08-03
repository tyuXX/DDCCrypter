// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SymmetricEncIntegrityPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public class SymmetricEncIntegrityPacket : InputStreamPacket
    {
        internal readonly int version;

        internal SymmetricEncIntegrityPacket( BcpgInputStream bcpgIn )
          : base( bcpgIn )
        {
            this.version = bcpgIn.ReadByte();
        }
    }
}
