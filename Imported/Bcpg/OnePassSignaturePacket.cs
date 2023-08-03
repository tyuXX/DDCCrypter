// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OnePassSignaturePacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class OnePassSignaturePacket : ContainedPacket
    {
        private int version;
        private int sigType;
        private HashAlgorithmTag hashAlgorithm;
        private PublicKeyAlgorithmTag keyAlgorithm;
        private long keyId;
        private int nested;

        internal OnePassSignaturePacket( BcpgInputStream bcpgIn )
        {
            this.version = bcpgIn.ReadByte();
            this.sigType = bcpgIn.ReadByte();
            this.hashAlgorithm = (HashAlgorithmTag)bcpgIn.ReadByte();
            this.keyAlgorithm = (PublicKeyAlgorithmTag)bcpgIn.ReadByte();
            this.keyId |= (long)bcpgIn.ReadByte() << 56;
            this.keyId |= (long)bcpgIn.ReadByte() << 48;
            this.keyId |= (long)bcpgIn.ReadByte() << 40;
            this.keyId |= (long)bcpgIn.ReadByte() << 32;
            this.keyId |= (long)bcpgIn.ReadByte() << 24;
            this.keyId |= (long)bcpgIn.ReadByte() << 16;
            this.keyId |= (long)bcpgIn.ReadByte() << 8;
            this.keyId |= (uint)bcpgIn.ReadByte();
            this.nested = bcpgIn.ReadByte();
        }

        public OnePassSignaturePacket(
          int sigType,
          HashAlgorithmTag hashAlgorithm,
          PublicKeyAlgorithmTag keyAlgorithm,
          long keyId,
          bool isNested )
        {
            this.version = 3;
            this.sigType = sigType;
            this.hashAlgorithm = hashAlgorithm;
            this.keyAlgorithm = keyAlgorithm;
            this.keyId = keyId;
            this.nested = isNested ? 0 : 1;
        }

        public int SignatureType => this.sigType;

        public PublicKeyAlgorithmTag KeyAlgorithm => this.keyAlgorithm;

        public HashAlgorithmTag HashAlgorithm => this.hashAlgorithm;

        public long KeyId => this.keyId;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            MemoryStream outStr = new();
            BcpgOutputStream bcpgOutputStream = new( outStr );
            bcpgOutputStream.Write( (byte)this.version, (byte)this.sigType, (byte)this.hashAlgorithm, (byte)this.keyAlgorithm );
            bcpgOutputStream.WriteLong( this.keyId );
            bcpgOutputStream.WriteByte( (byte)this.nested );
            bcpgOut.WritePacket( PacketTag.OnePassSignature, outStr.ToArray(), true );
        }
    }
}
