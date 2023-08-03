// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.NullEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Engines
{
    public class NullEngine : IBlockCipher
    {
        private const int BlockSize = 1;
        private bool initialised;

        public virtual void Init( bool forEncryption, ICipherParameters parameters ) => this.initialised = true;

        public virtual string AlgorithmName => "Null";

        public virtual bool IsPartialBlockOkay => true;

        public virtual int GetBlockSize() => 1;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (!this.initialised)
                throw new InvalidOperationException( "Null engine not initialised" );
            Check.DataLength( input, inOff, 1, "input buffer too short" );
            Check.OutputLength( output, outOff, 1, "output buffer too short" );
            for (int index = 0; index < 1; ++index)
                output[outOff + index] = input[inOff + index];
            return 1;
        }

        public virtual void Reset()
        {
        }
    }
}
