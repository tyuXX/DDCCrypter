// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.CfbBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class CfbBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] cfbV;
        private byte[] cfbOutV;
        private bool encrypting;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;

        public CfbBlockCipher( IBlockCipher cipher, int bitBlockSize )
        {
            this.cipher = cipher;
            this.blockSize = bitBlockSize / 8;
            this.IV = new byte[cipher.GetBlockSize()];
            this.cfbV = new byte[cipher.GetBlockSize()];
            this.cfbOutV = new byte[cipher.GetBlockSize()];
        }

        public IBlockCipher GetUnderlyingCipher() => this.cipher;

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.encrypting = forEncryption;
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                byte[] iv = parametersWithIv.GetIV();
                int num = this.IV.Length - iv.Length;
                Array.Copy( iv, 0, IV, num, iv.Length );
                Array.Clear( IV, 0, num );
                parameters = parametersWithIv.Parameters;
            }
            this.Reset();
            if (parameters == null)
                return;
            this.cipher.Init( true, parameters );
        }

        public string AlgorithmName => this.cipher.AlgorithmName + "/CFB" + this.blockSize * 8;

        public bool IsPartialBlockOkay => true;

        public int GetBlockSize() => this.blockSize;

        public int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff ) => !this.encrypting ? this.DecryptBlock( input, inOff, output, outOff ) : this.EncryptBlock( input, inOff, output, outOff );

        public int EncryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
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

        public int DecryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            if (inOff + this.blockSize > input.Length)
                throw new DataLengthException( "input buffer too short" );
            if (outOff + this.blockSize > outBytes.Length)
                throw new DataLengthException( "output buffer too short" );
            this.cipher.ProcessBlock( this.cfbV, 0, this.cfbOutV, 0 );
            Array.Copy( cfbV, this.blockSize, cfbV, 0, this.cfbV.Length - this.blockSize );
            Array.Copy( input, inOff, cfbV, this.cfbV.Length - this.blockSize, this.blockSize );
            for (int index = 0; index < this.blockSize; ++index)
                outBytes[outOff + index] = (byte)(this.cfbOutV[index] ^ (uint)input[inOff + index]);
            return this.blockSize;
        }

        public void Reset()
        {
            Array.Copy( IV, 0, cfbV, 0, this.IV.Length );
            this.cipher.Reset();
        }
    }
}
