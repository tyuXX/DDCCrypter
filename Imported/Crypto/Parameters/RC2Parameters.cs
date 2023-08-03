// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.RC2Parameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class RC2Parameters : KeyParameter
    {
        private readonly int bits;

        public RC2Parameters( byte[] key )
          : this( key, key.Length > 128 ? 1024 : key.Length * 8 )
        {
        }

        public RC2Parameters( byte[] key, int keyOff, int keyLen )
          : this( key, keyOff, keyLen, keyLen > 128 ? 1024 : keyLen * 8 )
        {
        }

        public RC2Parameters( byte[] key, int bits )
          : base( key )
        {
            this.bits = bits;
        }

        public RC2Parameters( byte[] key, int keyOff, int keyLen, int bits )
          : base( key, keyOff, keyLen )
        {
            this.bits = bits;
        }

        public int EffectiveKeyBits => this.bits;
    }
}
