// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.KeyParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class KeyParameter : ICipherParameters
    {
        private readonly byte[] key;

        public KeyParameter( byte[] key ) => this.key = key != null ? (byte[])key.Clone() : throw new ArgumentNullException( nameof( key ) );

        public KeyParameter( byte[] key, int keyOff, int keyLen )
        {
            if (key == null)
                throw new ArgumentNullException( nameof( key ) );
            if (keyOff < 0 || keyOff > key.Length)
                throw new ArgumentOutOfRangeException( nameof( keyOff ) );
            if (keyLen < 0 || keyOff + keyLen > key.Length)
                throw new ArgumentOutOfRangeException( nameof( keyLen ) );
            this.key = new byte[keyLen];
            Array.Copy( key, keyOff, this.key, 0, keyLen );
        }

        public byte[] GetKey() => (byte[])this.key.Clone();
    }
}
