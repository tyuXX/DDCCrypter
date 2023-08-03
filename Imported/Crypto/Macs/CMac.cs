// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.CMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class CMac : IMac
    {
        private const byte CONSTANT_128 = 135;
        private const byte CONSTANT_64 = 27;
        private byte[] ZEROES;
        private byte[] mac;
        private byte[] buf;
        private int bufOff;
        private IBlockCipher cipher;
        private int macSize;
        private byte[] L;
        private byte[] Lu;
        private byte[] Lu2;

        public CMac( IBlockCipher cipher )
          : this( cipher, cipher.GetBlockSize() * 8 )
        {
        }

        public CMac( IBlockCipher cipher, int macSizeInBits )
        {
            if (macSizeInBits % 8 != 0)
                throw new ArgumentException( "MAC size must be multiple of 8" );
            if (macSizeInBits > cipher.GetBlockSize() * 8)
                throw new ArgumentException( "MAC size must be less or equal to " + (cipher.GetBlockSize() * 8) );
            this.cipher = cipher.GetBlockSize() == 8 || cipher.GetBlockSize() == 16 ? (IBlockCipher)new CbcBlockCipher( cipher ) : throw new ArgumentException( "Block size must be either 64 or 128 bits" );
            this.macSize = macSizeInBits / 8;
            this.mac = new byte[cipher.GetBlockSize()];
            this.buf = new byte[cipher.GetBlockSize()];
            this.ZEROES = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public string AlgorithmName => this.cipher.AlgorithmName;

        private static int ShiftLeft( byte[] block, byte[] output )
        {
            int length = block.Length;
            uint num1 = 0;
            while (--length >= 0)
            {
                uint num2 = block[length];
                output[length] = (byte)((num2 << 1) | num1);
                num1 = (num2 >> 7) & 1U;
            }
            return (int)num1;
        }

        private static byte[] DoubleLu( byte[] input )
        {
            byte[] output = new byte[input.Length];
            int num1 = ShiftLeft( input, output );
            int num2 = input.Length == 16 ? 135 : 27;
            byte[] numArray;
            IntPtr index;
            (numArray = output)[(int)(index = (IntPtr)(input.Length - 1))] = (byte)(numArray[(int)index] ^ (uint)(byte)(num2 >> ((1 - num1) << 3)));
            return output;
        }

        public void Init( ICipherParameters parameters )
        {
            if (parameters is KeyParameter)
            {
                this.cipher.Init( true, parameters );
                this.L = new byte[this.ZEROES.Length];
                this.cipher.ProcessBlock( this.ZEROES, 0, this.L, 0 );
                this.Lu = DoubleLu( this.L );
                this.Lu2 = DoubleLu( this.Lu );
            }
            else if (parameters != null)
                throw new ArgumentException( "CMac mode only permits key to be set.", nameof( parameters ) );
            this.Reset();
        }

        public int GetMacSize() => this.macSize;

        public void Update( byte input )
        {
            if (this.bufOff == this.buf.Length)
            {
                this.cipher.ProcessBlock( this.buf, 0, this.mac, 0 );
                this.bufOff = 0;
            }
            this.buf[this.bufOff++] = input;
        }

        public void BlockUpdate( byte[] inBytes, int inOff, int len )
        {
            if (len < 0)
                throw new ArgumentException( "Can't have a negative input length!" );
            int blockSize = this.cipher.GetBlockSize();
            int length = blockSize - this.bufOff;
            if (len > length)
            {
                Array.Copy( inBytes, inOff, buf, this.bufOff, length );
                this.cipher.ProcessBlock( this.buf, 0, this.mac, 0 );
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > blockSize)
                {
                    this.cipher.ProcessBlock( inBytes, inOff, this.mac, 0 );
                    len -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy( inBytes, inOff, buf, this.bufOff, len );
            this.bufOff += len;
        }

        public int DoFinal( byte[] outBytes, int outOff )
        {
            byte[] numArray;
            if (this.bufOff == this.cipher.GetBlockSize())
            {
                numArray = this.Lu;
            }
            else
            {
                new ISO7816d4Padding().AddPadding( this.buf, this.bufOff );
                numArray = this.Lu2;
            }
            for (int index1 = 0; index1 < this.mac.Length; ++index1)
            {
                byte[] buf;
                IntPtr index2;
                (buf = this.buf)[(int)(index2 = (IntPtr)index1)] = (byte)(buf[(int)index2] ^ (uint)numArray[index1]);
            }
            this.cipher.ProcessBlock( this.buf, 0, this.mac, 0 );
            Array.Copy( mac, 0, outBytes, outOff, this.macSize );
            this.Reset();
            return this.macSize;
        }

        public void Reset()
        {
            Array.Clear( buf, 0, this.buf.Length );
            this.bufOff = 0;
            this.cipher.Reset();
        }
    }
}
