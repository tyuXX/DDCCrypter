// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.IssuerKeyId
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class IssuerKeyId : SignatureSubpacket
    {
        protected static byte[] KeyIdToBytes( long keyId ) => new byte[8]
        {
      (byte) (keyId >> 56),
      (byte) (keyId >> 48),
      (byte) (keyId >> 40),
      (byte) (keyId >> 32),
      (byte) (keyId >> 24),
      (byte) (keyId >> 16),
      (byte) (keyId >> 8),
      (byte) keyId
        };

        public IssuerKeyId( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.IssuerKeyId, critical, isLongLength, data )
        {
        }

        public IssuerKeyId( bool critical, long keyId )
          : base( SignatureSubpacketTag.IssuerKeyId, critical, false, KeyIdToBytes( keyId ) )
        {
        }

        public long KeyId => ((long)(this.data[0] & byte.MaxValue) << 56) | ((long)(this.data[1] & byte.MaxValue) << 48) | ((long)(this.data[2] & byte.MaxValue) << 40) | ((long)(this.data[3] & byte.MaxValue) << 32) | ((long)(this.data[4] & byte.MaxValue) << 24) | ((long)(this.data[5] & byte.MaxValue) << 16) | ((long)(this.data[6] & byte.MaxValue) << 8) | (this.data[7] & (long)byte.MaxValue);
    }
}
