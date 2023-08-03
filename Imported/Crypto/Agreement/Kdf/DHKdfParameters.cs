// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.Kdf.DHKdfParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;

namespace Org.BouncyCastle.Crypto.Agreement.Kdf
{
    public class DHKdfParameters : IDerivationParameters
    {
        private readonly DerObjectIdentifier algorithm;
        private readonly int keySize;
        private readonly byte[] z;
        private readonly byte[] extraInfo;

        public DHKdfParameters( DerObjectIdentifier algorithm, int keySize, byte[] z )
          : this( algorithm, keySize, z, null )
        {
        }

        public DHKdfParameters( DerObjectIdentifier algorithm, int keySize, byte[] z, byte[] extraInfo )
        {
            this.algorithm = algorithm;
            this.keySize = keySize;
            this.z = z;
            this.extraInfo = extraInfo;
        }

        public DerObjectIdentifier Algorithm => this.algorithm;

        public int KeySize => this.keySize;

        public byte[] GetZ() => this.z;

        public byte[] GetExtraInfo() => this.extraInfo;
    }
}
