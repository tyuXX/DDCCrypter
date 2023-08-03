// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.OfbBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class OfbBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] ofbV;
        private byte[] ofbOutV;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;

        public OfbBlockCipher( IBlockCipher cipher, int blockSize )
        {
            this.cipher = cipher;
            this.blockSize = blockSize / 8;
            this.IV = new byte[cipher.GetBlockSize()];
            this.ofbV = new byte[cipher.GetBlockSize()];
            this.ofbOutV = new byte[cipher.GetBlockSize()];
        }

        public IBlockCipher GetUnderlyingCipher() => this.cipher;

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                byte[] iv = parametersWithIv.GetIV();
                if (iv.Length < this.IV.Length)
                {
                    Array.Copy( iv, 0, IV, this.IV.Length - iv.Length, iv.Length );
                    for (int index = 0; index < this.IV.Length - iv.Length; ++index)
                        this.IV[index] = 0;
                }
                else
                    Array.Copy( iv, 0, IV, 0, this.IV.Length );
                parameters = parametersWithIv.Parameters;
            }
            this.Reset();
            if (parameters == null)
                return;
            this.cipher.Init( true, parameters );
        }

        public string AlgorithmName => this.cipher.AlgorithmName + "/OFB" + (this.blockSize * 8);

        public bool IsPartialBlockOkay => true;

        public int GetBlockSize() => this.blockSize;

        public int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (inOff + this.blockSize > input.Length)
                throw new DataLengthException( "input buffer too short" );
            if (outOff + this.blockSize > output.Length)
                throw new DataLengthException( "output buffer too short" );
            this.cipher.ProcessBlock( this.ofbV, 0, this.ofbOutV, 0 );
            for (int index = 0; index < this.blockSize; ++index)
                output[outOff + index] = (byte)(this.ofbOutV[index] ^ (uint)input[inOff + index]);
            Array.Copy( ofbV, this.blockSize, ofbV, 0, this.ofbV.Length - this.blockSize );
            Array.Copy( ofbOutV, 0, ofbV, this.ofbV.Length - this.blockSize, this.blockSize );
            return this.blockSize;
        }

        public void Reset()
        {
            Array.Copy( IV, 0, ofbV, 0, this.IV.Length );
            this.cipher.Reset();
        }
    }
}
