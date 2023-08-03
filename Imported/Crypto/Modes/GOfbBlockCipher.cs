// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.GOfbBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class GOfbBlockCipher : IBlockCipher
    {
        private const int C1 = 16843012;
        private const int C2 = 16843009;
        private byte[] IV;
        private byte[] ofbV;
        private byte[] ofbOutV;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;
        private bool firstStep = true;
        private int N3;
        private int N4;

        public GOfbBlockCipher( IBlockCipher cipher )
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            if (this.blockSize != 8)
                throw new ArgumentException( "GCTR only for 64 bit block ciphers" );
            this.IV = new byte[cipher.GetBlockSize()];
            this.ofbV = new byte[cipher.GetBlockSize()];
            this.ofbOutV = new byte[cipher.GetBlockSize()];
        }

        public IBlockCipher GetUnderlyingCipher() => this.cipher;

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.firstStep = true;
            this.N3 = 0;
            this.N4 = 0;
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

        public string AlgorithmName => this.cipher.AlgorithmName + "/GCTR";

        public bool IsPartialBlockOkay => true;

        public int GetBlockSize() => this.blockSize;

        public int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (inOff + this.blockSize > input.Length)
                throw new DataLengthException( "input buffer too short" );
            if (outOff + this.blockSize > output.Length)
                throw new DataLengthException( "output buffer too short" );
            if (this.firstStep)
            {
                this.firstStep = false;
                this.cipher.ProcessBlock( this.ofbV, 0, this.ofbOutV, 0 );
                this.N3 = this.bytesToint( this.ofbOutV, 0 );
                this.N4 = this.bytesToint( this.ofbOutV, 4 );
            }
            this.N3 += 16843009;
            this.N4 += 16843012;
            this.intTobytes( this.N3, this.ofbV, 0 );
            this.intTobytes( this.N4, this.ofbV, 4 );
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

        private int bytesToint( byte[] inBytes, int inOff ) => (int)(inBytes[inOff + 3] << 24 & 4278190080L) + ((inBytes[inOff + 2] << 16) & 16711680) + ((inBytes[inOff + 1] << 8) & 65280) + (inBytes[inOff] & byte.MaxValue);

        private void intTobytes( int num, byte[] outBytes, int outOff )
        {
            outBytes[outOff + 3] = (byte)(num >> 24);
            outBytes[outOff + 2] = (byte)(num >> 16);
            outBytes[outOff + 1] = (byte)(num >> 8);
            outBytes[outOff] = (byte)num;
        }
    }
}
