// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.DigestRandomGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class DigestRandomGenerator : IRandomGenerator
    {
        private const long CYCLE_COUNT = 10;
        private long stateCounter;
        private long seedCounter;
        private IDigest digest;
        private byte[] state;
        private byte[] seed;

        public DigestRandomGenerator( IDigest digest )
        {
            this.digest = digest;
            this.seed = new byte[digest.GetDigestSize()];
            this.seedCounter = 1L;
            this.state = new byte[digest.GetDigestSize()];
            this.stateCounter = 1L;
        }

        public void AddSeedMaterial( byte[] inSeed )
        {
            lock (this)
            {
                this.DigestUpdate( inSeed );
                this.DigestUpdate( this.seed );
                this.DigestDoFinal( this.seed );
            }
        }

        public void AddSeedMaterial( long rSeed )
        {
            lock (this)
            {
                this.DigestAddCounter( rSeed );
                this.DigestUpdate( this.seed );
                this.DigestDoFinal( this.seed );
            }
        }

        public void NextBytes( byte[] bytes ) => this.NextBytes( bytes, 0, bytes.Length );

        public void NextBytes( byte[] bytes, int start, int len )
        {
            lock (this)
            {
                int num1 = 0;
                this.GenerateState();
                int num2 = start + len;
                for (int index = start; index < num2; ++index)
                {
                    if (num1 == this.state.Length)
                    {
                        this.GenerateState();
                        num1 = 0;
                    }
                    bytes[index] = this.state[num1++];
                }
            }
        }

        private void CycleSeed()
        {
            this.DigestUpdate( this.seed );
            this.DigestAddCounter( this.seedCounter++ );
            this.DigestDoFinal( this.seed );
        }

        private void GenerateState()
        {
            this.DigestAddCounter( this.stateCounter++ );
            this.DigestUpdate( this.state );
            this.DigestUpdate( this.seed );
            this.DigestDoFinal( this.state );
            if (this.stateCounter % 10L != 0L)
                return;
            this.CycleSeed();
        }

        private void DigestAddCounter( long seedVal )
        {
            byte[] numArray = new byte[8];
            Pack.UInt64_To_LE( (ulong)seedVal, numArray );
            this.digest.BlockUpdate( numArray, 0, numArray.Length );
        }

        private void DigestUpdate( byte[] inSeed ) => this.digest.BlockUpdate( inSeed, 0, inSeed.Length );

        private void DigestDoFinal( byte[] result ) => this.digest.DoFinal( result, 0 );
    }
}
