// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.SigCalculator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Operators
{
    internal class SigCalculator : IStreamCalculator
    {
        private readonly ISigner sig;
        private readonly Stream stream;

        internal SigCalculator( ISigner sig )
        {
            this.sig = sig;
            this.stream = new SignerBucket( sig );
        }

        public Stream Stream => this.stream;

        public object GetResult() => new SigResult( this.sig );
    }
}
