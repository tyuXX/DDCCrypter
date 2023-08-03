// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.SicBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class SicBlockCipher : IBlockCipher
    {
        private readonly IBlockCipher cipher;
        private readonly int blockSize;
        private readonly byte[] counter;
        private readonly byte[] counterOut;
        private byte[] IV;

        public SicBlockCipher( IBlockCipher cipher )
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            this.counter = new byte[this.blockSize];
            this.counterOut = new byte[this.blockSize];
            this.IV = new byte[this.blockSize];
        }

        public virtual IBlockCipher GetUnderlyingCipher() => this.cipher;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.IV = parameters is ParametersWithIV parametersWithIv ? Arrays.Clone( parametersWithIv.GetIV() ) : throw new ArgumentException( "CTR/SIC mode requires ParametersWithIV", nameof( parameters ) );
            if (this.blockSize < this.IV.Length)
                throw new ArgumentException( "CTR/SIC mode requires IV no greater than: " + blockSize + " bytes." );
            int num = System.Math.Min( 8, this.blockSize / 2 );
            if (this.blockSize - this.IV.Length > num)
                throw new ArgumentException( "CTR/SIC mode requires IV of at least: " + (this.blockSize - num) + " bytes." );
            if (parametersWithIv.Parameters != null)
                this.cipher.Init( true, parametersWithIv.Parameters );
            this.Reset();
        }

        public virtual string AlgorithmName => this.cipher.AlgorithmName + "/SIC";

        public virtual bool IsPartialBlockOkay => true;

        public virtual int GetBlockSize() => this.cipher.GetBlockSize();

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            this.cipher.ProcessBlock( this.counter, 0, this.counterOut, 0 );
            for (int index = 0; index < this.counterOut.Length; ++index)
                output[outOff + index] = (byte)(this.counterOut[index] ^ (uint)input[inOff + index]);
            int length = this.counter.Length;
            byte[] counter;
            IntPtr index1;
            do
                ;
            while (--length >= 0 && ((counter = this.counter)[(int)(index1 = (IntPtr)length)] = (byte)(counter[(int)index1] + 1U)) == 0);
            return this.counter.Length;
        }

        public virtual void Reset()
        {
            Arrays.Fill( this.counter, 0 );
            Array.Copy( IV, 0, counter, 0, this.IV.Length );
            this.cipher.Reset();
        }
    }
}
