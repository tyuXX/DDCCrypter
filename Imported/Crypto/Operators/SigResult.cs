// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.SigResult
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Operators
{
    internal class SigResult : IBlockResult
    {
        private readonly ISigner sig;

        internal SigResult( ISigner sig ) => this.sig = sig;

        public byte[] Collect() => this.sig.GenerateSignature();

        public int Collect( byte[] destination, int offset )
        {
            byte[] sourceArray = this.Collect();
            Array.Copy( sourceArray, 0, destination, offset, sourceArray.Length );
            return sourceArray.Length;
        }
    }
}
