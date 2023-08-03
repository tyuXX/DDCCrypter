﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.CbcBlockCipherMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using System;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class CbcBlockCipherMac : IMac
    {
        private byte[] buf;
        private int bufOff;
        private IBlockCipher cipher;
        private IBlockCipherPadding padding;
        private int macSize;

        public CbcBlockCipherMac( IBlockCipher cipher )
          : this( cipher, cipher.GetBlockSize() * 8 / 2, null )
        {
        }

        public CbcBlockCipherMac( IBlockCipher cipher, IBlockCipherPadding padding )
          : this( cipher, cipher.GetBlockSize() * 8 / 2, padding )
        {
        }

        public CbcBlockCipherMac( IBlockCipher cipher, int macSizeInBits )
          : this( cipher, macSizeInBits, null )
        {
        }

        public CbcBlockCipherMac( IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding )
        {
            if (macSizeInBits % 8 != 0)
                throw new ArgumentException( "MAC size must be multiple of 8" );
            this.cipher = new CbcBlockCipher( cipher );
            this.padding = padding;
            this.macSize = macSizeInBits / 8;
            this.buf = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public string AlgorithmName => this.cipher.AlgorithmName;

        public void Init( ICipherParameters parameters )
        {
            this.Reset();
            this.cipher.Init( true, parameters );
        }

        public int GetMacSize() => this.macSize;

        public void Update( byte input )
        {
            if (this.bufOff == this.buf.Length)
            {
                this.cipher.ProcessBlock( this.buf, 0, this.buf, 0 );
                this.bufOff = 0;
            }
            this.buf[this.bufOff++] = input;
        }

        public void BlockUpdate( byte[] input, int inOff, int len )
        {
            if (len < 0)
                throw new ArgumentException( "Can't have a negative input length!" );
            int blockSize = this.cipher.GetBlockSize();
            int length = blockSize - this.bufOff;
            if (len > length)
            {
                Array.Copy( input, inOff, buf, this.bufOff, length );
                this.cipher.ProcessBlock( this.buf, 0, this.buf, 0 );
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > blockSize)
                {
                    this.cipher.ProcessBlock( input, inOff, this.buf, 0 );
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
                    this.cipher.ProcessBlock( this.buf, 0, this.buf, 0 );
                    this.bufOff = 0;
                }
                this.padding.AddPadding( this.buf, this.bufOff );
            }
            this.cipher.ProcessBlock( this.buf, 0, this.buf, 0 );
            Array.Copy( buf, 0, output, outOff, this.macSize );
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
