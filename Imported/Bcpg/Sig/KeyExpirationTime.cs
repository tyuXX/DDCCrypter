// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.KeyExpirationTime
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class KeyExpirationTime : SignatureSubpacket
    {
        protected static byte[] TimeToBytes( long t ) => new byte[4]
        {
      (byte) (t >> 24),
      (byte) (t >> 16),
      (byte) (t >> 8),
      (byte) t
        };

        public KeyExpirationTime( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.KeyExpireTime, critical, isLongLength, data )
        {
        }

        public KeyExpirationTime( bool critical, long seconds )
          : base( SignatureSubpacketTag.KeyExpireTime, critical, false, TimeToBytes( seconds ) )
        {
        }

        public long Time => ((long)(this.data[0] & byte.MaxValue) << 24) | ((long)(this.data[1] & byte.MaxValue) << 16) | ((long)(this.data[2] & byte.MaxValue) << 8) | (this.data[3] & (long)byte.MaxValue);
    }
}
