// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ModDetectionCodePacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Bcpg
{
    public class ModDetectionCodePacket : ContainedPacket
    {
        private readonly byte[] digest;

        internal ModDetectionCodePacket( BcpgInputStream bcpgIn )
        {
            if (bcpgIn == null)
                throw new ArgumentNullException( nameof( bcpgIn ) );
            this.digest = new byte[20];
            bcpgIn.ReadFully( this.digest );
        }

        public ModDetectionCodePacket( byte[] digest ) => this.digest = digest != null ? (byte[])digest.Clone() : throw new ArgumentNullException( nameof( digest ) );

        public byte[] GetDigest() => (byte[])this.digest.Clone();

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.ModificationDetectionCode, this.digest, false );
    }
}
