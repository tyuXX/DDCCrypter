// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SecretSubkeyPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public class SecretSubkeyPacket : SecretKeyPacket
    {
        internal SecretSubkeyPacket( BcpgInputStream bcpgIn )
          : base( bcpgIn )
        {
        }

        public SecretSubkeyPacket(
          PublicKeyPacket pubKeyPacket,
          SymmetricKeyAlgorithmTag encAlgorithm,
          S2k s2k,
          byte[] iv,
          byte[] secKeyData )
          : base( pubKeyPacket, encAlgorithm, s2k, iv, secKeyData )
        {
        }

        public SecretSubkeyPacket(
          PublicKeyPacket pubKeyPacket,
          SymmetricKeyAlgorithmTag encAlgorithm,
          int s2kUsage,
          S2k s2k,
          byte[] iv,
          byte[] secKeyData )
          : base( pubKeyPacket, encAlgorithm, s2kUsage, s2k, iv, secKeyData )
        {
        }

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.SecretSubkey, this.GetEncodedContents(), true );
    }
}
