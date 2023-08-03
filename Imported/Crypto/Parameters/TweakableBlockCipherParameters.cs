// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.TweakableBlockCipherParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class TweakableBlockCipherParameters : ICipherParameters
    {
        private readonly byte[] tweak;
        private readonly KeyParameter key;

        public TweakableBlockCipherParameters( KeyParameter key, byte[] tweak )
        {
            this.key = key;
            this.tweak = Arrays.Clone( tweak );
        }

        public KeyParameter Key => this.key;

        public byte[] Tweak => this.tweak;
    }
}
