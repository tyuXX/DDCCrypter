// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.Srp.Srp6VerifierGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Agreement.Srp
{
    public class Srp6VerifierGenerator
    {
        protected BigInteger N;
        protected BigInteger g;
        protected IDigest digest;

        public virtual void Init( BigInteger N, BigInteger g, IDigest digest )
        {
            this.N = N;
            this.g = g;
            this.digest = digest;
        }

        public virtual void Init( Srp6GroupParameters group, IDigest digest ) => this.Init( group.N, group.G, digest );

        public virtual BigInteger GenerateVerifier( byte[] salt, byte[] identity, byte[] password ) => this.g.ModPow( Srp6Utilities.CalculateX( this.digest, this.N, salt, identity, password ), this.N );
    }
}
