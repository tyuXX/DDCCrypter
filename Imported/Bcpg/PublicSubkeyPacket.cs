// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.PublicSubkeyPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public class PublicSubkeyPacket : PublicKeyPacket
    {
        internal PublicSubkeyPacket( BcpgInputStream bcpgIn )
          : base( bcpgIn )
        {
        }

        public PublicSubkeyPacket( PublicKeyAlgorithmTag algorithm, DateTime time, IBcpgKey key )
          : base( algorithm, time, key )
        {
        }

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.PublicSubkey, this.GetEncodedContents(), true );
    }
}
