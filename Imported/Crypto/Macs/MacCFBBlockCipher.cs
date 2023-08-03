// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.MacCFBBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Macs
{
    internal class MacCFBBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] cfbV;
        private byte[] cfbOutV;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;

        public MacCFBBlockCipher( IBlockCipher cipher, int bitBlockSize )
        {
            this.cipher = cipher;
            this.blockSize = bitBlockSize / 8;
            this.IV = new byte[cipher.GetBlockSize()];
            this.cfbV = new byte[cipher.GetBlockSize()];
            this.cfbOutV = new byte[cipher.GetBlockSize()];
        }

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                byte[] iv = parametersWithIv.GetIV();
                if (iv.Length < this.IV.Length)
                    Array.Copy( iv, 0, IV, this.IV.Length - iv.Length, iv.Length );
                else
                    Array.Copy( iv, 0, IV, 0, this.IV.Length );
                parameters = parametersWithIv.Parameters;
            }
            this.Reset();
            this.cipher.Init( true, parameters );
        }

        public string AlgorithmName => this.cipher.AlgorithmName + "/CFB" + (this.blockSize * 8);

        public bool IsPartialBlockOkay => true;

        public int GetBlockSize() => this.blockSize;

        public int ProcessBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            if (inOff + this.blockSize > input.Length)
                throw new DataLengthException( "input buffer too short" );
            if (outOff + this.blockSize > outBytes.Length)
                throw new DataLengthException( "output buffer too short" );
            this.cipher.ProcessBlock( this.cfbV, 0, this.cfbOutV, 0 );
            for (int index = 0; index < this.blockSize; ++index)
                outBytes[outOff + index] = (byte)(this.cfbOutV[index] ^ (uint)input[inOff + index]);
            Array.Copy( cfbV, this.blockSize, cfbV, 0, this.cfbV.Length - this.blockSize );
            Array.Copy( outBytes, outOff, cfbV, this.cfbV.Length - this.blockSize, this.blockSize );
            return this.blockSize;
        }

        public void Reset()
        {
            this.IV.CopyTo( cfbV, 0 );
            this.cipher.Reset();
        }

        public void GetMacBlock( byte[] mac ) => this.cipher.ProcessBlock( this.cfbV, 0, mac, 0 );
    }
}
