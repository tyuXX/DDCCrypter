// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SecretKeyPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class SecretKeyPacket : ContainedPacket
    {
        public const int UsageNone = 0;
        public const int UsageChecksum = 255;
        public const int UsageSha1 = 254;
        private PublicKeyPacket pubKeyPacket;
        private readonly byte[] secKeyData;
        private int s2kUsage;
        private SymmetricKeyAlgorithmTag encAlgorithm;
        private S2k s2k;
        private byte[] iv;

        internal SecretKeyPacket( BcpgInputStream bcpgIn )
        {
            this.pubKeyPacket = !(this is SecretSubkeyPacket) ? new PublicKeyPacket( bcpgIn ) : new PublicSubkeyPacket( bcpgIn );
            this.s2kUsage = bcpgIn.ReadByte();
            if (this.s2kUsage == byte.MaxValue || this.s2kUsage == 254)
            {
                this.encAlgorithm = (SymmetricKeyAlgorithmTag)bcpgIn.ReadByte();
                this.s2k = new S2k( bcpgIn );
            }
            else
                this.encAlgorithm = (SymmetricKeyAlgorithmTag)this.s2kUsage;
            if ((this.s2k == null || this.s2k.Type != 101 || this.s2k.ProtectionMode != 1) && this.s2kUsage != 0)
            {
                this.iv = this.encAlgorithm >= SymmetricKeyAlgorithmTag.Aes128 ? new byte[16] : new byte[8];
                bcpgIn.ReadFully( this.iv );
            }
            this.secKeyData = bcpgIn.ReadAll();
        }

        public SecretKeyPacket(
          PublicKeyPacket pubKeyPacket,
          SymmetricKeyAlgorithmTag encAlgorithm,
          S2k s2k,
          byte[] iv,
          byte[] secKeyData )
        {
            this.pubKeyPacket = pubKeyPacket;
            this.encAlgorithm = encAlgorithm;
            this.s2kUsage = encAlgorithm == SymmetricKeyAlgorithmTag.Null ? 0 : byte.MaxValue;
            this.s2k = s2k;
            this.iv = Arrays.Clone( iv );
            this.secKeyData = secKeyData;
        }

        public SecretKeyPacket(
          PublicKeyPacket pubKeyPacket,
          SymmetricKeyAlgorithmTag encAlgorithm,
          int s2kUsage,
          S2k s2k,
          byte[] iv,
          byte[] secKeyData )
        {
            this.pubKeyPacket = pubKeyPacket;
            this.encAlgorithm = encAlgorithm;
            this.s2kUsage = s2kUsage;
            this.s2k = s2k;
            this.iv = Arrays.Clone( iv );
            this.secKeyData = secKeyData;
        }

        public SymmetricKeyAlgorithmTag EncAlgorithm => this.encAlgorithm;

        public int S2kUsage => this.s2kUsage;

        public byte[] GetIV() => Arrays.Clone( this.iv );

        public S2k S2k => this.s2k;

        public PublicKeyPacket PublicKeyPacket => this.pubKeyPacket;

        public byte[] GetSecretKeyData() => this.secKeyData;

        public byte[] GetEncodedContents()
        {
            MemoryStream outStr = new();
            BcpgOutputStream bcpgOutputStream = new( outStr );
            bcpgOutputStream.Write( this.pubKeyPacket.GetEncodedContents() );
            bcpgOutputStream.WriteByte( (byte)this.s2kUsage );
            if (this.s2kUsage == byte.MaxValue || this.s2kUsage == 254)
            {
                bcpgOutputStream.WriteByte( (byte)this.encAlgorithm );
                bcpgOutputStream.WriteObject( s2k );
            }
            if (this.iv != null)
                bcpgOutputStream.Write( this.iv );
            if (this.secKeyData != null && this.secKeyData.Length > 0)
                bcpgOutputStream.Write( this.secKeyData );
            return outStr.ToArray();
        }

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.SecretKey, this.GetEncodedContents(), true );
    }
}
