// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.IesParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class IesParameters : ICipherParameters
    {
        private byte[] derivation;
        private byte[] encoding;
        private int macKeySize;

        public IesParameters( byte[] derivation, byte[] encoding, int macKeySize )
        {
            this.derivation = derivation;
            this.encoding = encoding;
            this.macKeySize = macKeySize;
        }

        public byte[] GetDerivationV() => this.derivation;

        public byte[] GetEncodingV() => this.encoding;

        public int MacKeySize => this.macKeySize;
    }
}
