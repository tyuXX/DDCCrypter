// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.ISO9797Alg3Mac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class ISO9797Alg3Mac : IMac
    {
        private byte[] mac;
        private byte[] buf;
        private int bufOff;
        private IBlockCipher cipher;
        private IBlockCipherPadding padding;
        private int macSize;
        private KeyParameter lastKey2;
        private KeyParameter lastKey3;

        public ISO9797Alg3Mac( IBlockCipher cipher )
          : this( cipher, cipher.GetBlockSize() * 8, null )
        {
        }

        public ISO9797Alg3Mac( IBlockCipher cipher, IBlockCipherPadding padding )
          : this( cipher, cipher.GetBlockSize() * 8, padding )
        {
        }

        public ISO9797Alg3Mac( IBlockCipher cipher, int macSizeInBits )
          : this( cipher, macSizeInBits, null )
        {
        }

        public ISO9797Alg3Mac( IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding )
        {
            if (macSizeInBits % 8 != 0)
                throw new ArgumentException( "MAC size must be multiple of 8" );
            this.cipher = cipher is DesEngine ? (IBlockCipher)new CbcBlockCipher( cipher ) : throw new ArgumentException( "cipher must be instance of DesEngine" );
            this.padding = padding;
            this.macSize = macSizeInBits / 8;
            this.mac = new byte[cipher.GetBlockSize()];
            this.buf = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public string AlgorithmName => "ISO9797Alg3";

        public void Init( ICipherParameters parameters )
        {
            this.Reset();
            switch (parameters)
            {
                case KeyParameter _:
                case ParametersWithIV _:
                    byte[] key = (!(parameters is KeyParameter) ? (KeyParameter)((ParametersWithIV)parameters).Parameters : (KeyParameter)parameters).GetKey();
                    KeyParameter parameters1;
                    if (key.Length == 16)
                    {
                        parameters1 = new KeyParameter( key, 0, 8 );
                        this.lastKey2 = new KeyParameter( key, 8, 8 );
                        this.lastKey3 = parameters1;
                    }
                    else
                    {
                        parameters1 = key.Length == 24 ? new KeyParameter( key, 0, 8 ) : throw new ArgumentException( "Key must be either 112 or 168 bit long" );
                        this.lastKey2 = new KeyParameter( key, 8, 8 );
                        this.lastKey3 = new KeyParameter( key, 16, 8 );
                    }
                    if (parameters is ParametersWithIV)
                    {
                        this.cipher.Init( true, new ParametersWithIV( parameters1, ((ParametersWithIV)parameters).GetIV() ) );
                        break;
                    }
                    this.cipher.Init( true, parameters1 );
                    break;
                default:
                    throw new ArgumentException( "parameters must be an instance of KeyParameter or ParametersWithIV" );
            }
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

        public void BlockUpdate( byte[] input, int inOff, int len )
        {
            if (len < 0)
                throw new ArgumentException( "Can't have a negative input length!" );
            int blockSize = this.cipher.GetBlockSize();
            int num1 = 0;
            int length = blockSize - this.bufOff;
            if (len > length)
            {
                Array.Copy( input, inOff, buf, this.bufOff, length );
                int num2 = num1 + this.cipher.ProcessBlock( this.buf, 0, this.mac, 0 );
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > blockSize)
                {
                    num2 += this.cipher.ProcessBlock( input, inOff, this.mac, 0 );
                    len -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy( input, inOff, buf, this.bufOff, len );
            this.bufOff += len;
        }

        public int DoFinal( byte[] output, int outOff )
        {
            int blockSize = this.cipher.GetBlockSize();
            if (this.padding == null)
            {
                while (this.bufOff < blockSize)
                    this.buf[this.bufOff++] = 0;
            }
            else
            {
                if (this.bufOff == blockSize)
                {
                    this.cipher.ProcessBlock( this.buf, 0, this.mac, 0 );
                    this.bufOff = 0;
                }
                this.padding.AddPadding( this.buf, this.bufOff );
            }
            this.cipher.ProcessBlock( this.buf, 0, this.mac, 0 );
            DesEngine desEngine = new DesEngine();
            desEngine.Init( false, lastKey2 );
            desEngine.ProcessBlock( this.mac, 0, this.mac, 0 );
            desEngine.Init( true, lastKey3 );
            desEngine.ProcessBlock( this.mac, 0, this.mac, 0 );
            Array.Copy( mac, 0, output, outOff, this.macSize );
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
