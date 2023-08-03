// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsSrpLoginParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsSrpLoginParameters
    {
        protected readonly Srp6GroupParameters mGroup;
        protected readonly BigInteger mVerifier;
        protected readonly byte[] mSalt;

        public TlsSrpLoginParameters( Srp6GroupParameters group, BigInteger verifier, byte[] salt )
        {
            this.mGroup = group;
            this.mVerifier = verifier;
            this.mSalt = salt;
        }

        public virtual Srp6GroupParameters Group => this.mGroup;

        public virtual byte[] Salt => this.mSalt;

        public virtual BigInteger Verifier => this.mVerifier;
    }
}
