﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.KdfParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class KdfParameters : IDerivationParameters
    {
        private byte[] iv;
        private byte[] shared;

        public KdfParameters( byte[] shared, byte[] iv )
        {
            this.shared = shared;
            this.iv = iv;
        }

        public byte[] GetSharedSecret() => this.shared;

        public byte[] GetIV() => this.iv;
    }
}