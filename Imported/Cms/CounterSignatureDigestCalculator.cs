// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CounterSignatureDigestCalculator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Cms
{
    internal class CounterSignatureDigestCalculator : IDigestCalculator
    {
        private readonly string alg;
        private readonly byte[] data;

        internal CounterSignatureDigestCalculator( string alg, byte[] data )
        {
            this.alg = alg;
            this.data = data;
        }

        public byte[] GetDigest() => DigestUtilities.DoFinal( CmsSignedHelper.Instance.GetDigestInstance( this.alg ), this.data );
    }
}
