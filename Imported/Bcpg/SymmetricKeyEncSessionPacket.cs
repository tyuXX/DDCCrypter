// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SymmetricKeyEncSessionPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class SymmetricKeyEncSessionPacket : ContainedPacket
    {
        private int version;
        private SymmetricKeyAlgorithmTag encAlgorithm;
        private S2k s2k;
        private readonly byte[] secKeyData;

        public SymmetricKeyEncSessionPacket( BcpgInputStream bcpgIn )
        {
            this.version = bcpgIn.ReadByte();
            this.encAlgorithm = (SymmetricKeyAlgorithmTag)bcpgIn.ReadByte();
            this.s2k = new S2k( bcpgIn );
            this.secKeyData = bcpgIn.ReadAll();
        }

        public SymmetricKeyEncSessionPacket(
          SymmetricKeyAlgorithmTag encAlgorithm,
          S2k s2k,
          byte[] secKeyData )
        {
            this.version = 4;
            this.encAlgorithm = encAlgorithm;
            this.s2k = s2k;
            this.secKeyData = secKeyData;
        }

        public SymmetricKeyAlgorithmTag EncAlgorithm => this.encAlgorithm;

        public S2k S2k => this.s2k;

        public byte[] GetSecKeyData() => this.secKeyData;

        public int Version => this.version;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            MemoryStream outStr = new MemoryStream();
            BcpgOutputStream bcpgOutputStream = new BcpgOutputStream( outStr );
            bcpgOutputStream.Write( (byte)this.version, (byte)this.encAlgorithm );
            bcpgOutputStream.WriteObject( s2k );
            if (this.secKeyData != null && this.secKeyData.Length > 0)
                bcpgOutputStream.Write( this.secKeyData );
            bcpgOut.WritePacket( PacketTag.SymmetricKeyEncryptedSessionKey, outStr.ToArray(), true );
        }
    }
}
