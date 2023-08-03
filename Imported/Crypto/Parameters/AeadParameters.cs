// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.AeadParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class AeadParameters : ICipherParameters
    {
        private readonly byte[] associatedText;
        private readonly byte[] nonce;
        private readonly KeyParameter key;
        private readonly int macSize;

        public AeadParameters( KeyParameter key, int macSize, byte[] nonce )
          : this( key, macSize, nonce, null )
        {
        }

        public AeadParameters( KeyParameter key, int macSize, byte[] nonce, byte[] associatedText )
        {
            this.key = key;
            this.nonce = nonce;
            this.macSize = macSize;
            this.associatedText = associatedText;
        }

        public virtual KeyParameter Key => this.key;

        public virtual int MacSize => this.macSize;

        public virtual byte[] GetAssociatedText() => this.associatedText;

        public virtual byte[] GetNonce() => this.nonce;
    }
}
