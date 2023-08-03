// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.CbcBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class CbcBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] cbcV;
        private byte[] cbcNextV;
        private int blockSize;
        private IBlockCipher cipher;
        private bool encrypting;

        public CbcBlockCipher( IBlockCipher cipher )
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            this.IV = new byte[this.blockSize];
            this.cbcV = new byte[this.blockSize];
            this.cbcNextV = new byte[this.blockSize];
        }

        public IBlockCipher GetUnderlyingCipher() => this.cipher;

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            bool encrypting = this.encrypting;
            this.encrypting = forEncryption;
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                byte[] iv = parametersWithIv.GetIV();
                if (iv.Length != this.blockSize)
                    throw new ArgumentException( "initialisation vector must be the same length as block size" );
                Array.Copy( iv, 0, IV, 0, iv.Length );
                parameters = parametersWithIv.Parameters;
            }
            this.Reset();
            if (parameters != null)
                this.cipher.Init( this.encrypting, parameters );
            else if (encrypting != this.encrypting)
                throw new ArgumentException( "cannot change encrypting state without providing key." );
        }

        public string AlgorithmName => this.cipher.AlgorithmName + "/CBC";

        public bool IsPartialBlockOkay => false;

        public int GetBlockSize() => this.cipher.GetBlockSize();

        public int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff ) => !this.encrypting ? this.DecryptBlock( input, inOff, output, outOff ) : this.EncryptBlock( input, inOff, output, outOff );

        public void Reset()
        {
            Array.Copy( IV, 0, cbcV, 0, this.IV.Length );
            Array.Clear( cbcNextV, 0, this.cbcNextV.Length );
            this.cipher.Reset();
        }

        private int EncryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            if (inOff + this.blockSize > input.Length)
                throw new DataLengthException( "input buffer too short" );
            for (int index1 = 0; index1 < this.blockSize; ++index1)
            {
                byte[] cbcV;
                IntPtr index2;
                (cbcV = this.cbcV)[(int)(index2 = (IntPtr)index1)] = (byte)(cbcV[(int)index2] ^ (uint)input[inOff + index1]);
            }
            int num = this.cipher.ProcessBlock( this.cbcV, 0, outBytes, outOff );
            Array.Copy( outBytes, outOff, cbcV, 0, this.cbcV.Length );
            return num;
        }

        private int DecryptBlock( byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            if (inOff + this.blockSize > input.Length)
                throw new DataLengthException( "input buffer too short" );
            Array.Copy( input, inOff, cbcNextV, 0, this.blockSize );
            int num = this.cipher.ProcessBlock( input, inOff, outBytes, outOff );
            for (int index1 = 0; index1 < this.blockSize; ++index1)
            {
                byte[] numArray;
                IntPtr index2;
                (numArray = outBytes)[(int)(index2 = (IntPtr)(outOff + index1))] = (byte)(numArray[(int)index2] ^ (uint)this.cbcV[index1]);
            }
            byte[] cbcV = this.cbcV;
            this.cbcV = this.cbcNextV;
            this.cbcNextV = cbcV;
            return num;
        }
    }
}
