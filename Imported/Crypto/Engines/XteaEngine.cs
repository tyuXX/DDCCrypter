// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.XteaEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class XteaEngine : IBlockCipher
    {
        private const int rounds = 32;
        private const int block_size = 8;
        private const int delta = -1640531527;
        private uint[] _S = new uint[4];
        private uint[] _sum0 = new uint[32];
        private uint[] _sum1 = new uint[32];
        private bool _initialised;
        private bool _forEncryption;

        public XteaEngine() => this._initialised = false;

        public virtual string AlgorithmName => "XTEA";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 8;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "invalid parameter passed to TEA init - " + Platform.GetTypeName( parameters ) );
            this._forEncryption = forEncryption;
            this._initialised = true;
            this.setKey( ((KeyParameter)parameters).GetKey() );
        }

        public virtual int ProcessBlock( byte[] inBytes, int inOff, byte[] outBytes, int outOff )
        {
            if (!this._initialised)
                throw new InvalidOperationException( this.AlgorithmName + " not initialised" );
            Check.DataLength( inBytes, inOff, 8, "input buffer too short" );
            Check.OutputLength( outBytes, outOff, 8, "output buffer too short" );
            return !this._forEncryption ? this.decryptBlock( inBytes, inOff, outBytes, outOff ) : this.encryptBlock( inBytes, inOff, outBytes, outOff );
        }

        public virtual void Reset()
        {
        }

        private void setKey( byte[] key )
        {
            int off;
            int index1 = off = 0;
            while (index1 < 4)
            {
                this._S[index1] = Pack.BE_To_UInt32( key, off );
                ++index1;
                off += 4;
            }
            int num;
            for (int index2 = num = 0; index2 < 32; ++index2)
            {
                this._sum0[index2] = (uint)num + this._S[num & 3];
                num += -1640531527;
                this._sum1[index2] = (uint)num + this._S[(num >> 11) & 3];
            }
        }

        private int encryptBlock( byte[] inBytes, int inOff, byte[] outBytes, int outOff )
        {
            uint uint32_1 = Pack.BE_To_UInt32( inBytes, inOff );
            uint uint32_2 = Pack.BE_To_UInt32( inBytes, inOff + 4 );
            for (int index = 0; index < 32; ++index)
            {
                uint32_1 += (((uint32_2 << 4) ^ (uint32_2 >> 5)) + uint32_2) ^ this._sum0[index];
                uint32_2 += (((uint32_1 << 4) ^ (uint32_1 >> 5)) + uint32_1) ^ this._sum1[index];
            }
            Pack.UInt32_To_BE( uint32_1, outBytes, outOff );
            Pack.UInt32_To_BE( uint32_2, outBytes, outOff + 4 );
            return 8;
        }

        private int decryptBlock( byte[] inBytes, int inOff, byte[] outBytes, int outOff )
        {
            uint uint32_1 = Pack.BE_To_UInt32( inBytes, inOff );
            uint uint32_2 = Pack.BE_To_UInt32( inBytes, inOff + 4 );
            for (int index = 31; index >= 0; --index)
            {
                uint32_2 -= (((uint32_1 << 4) ^ (uint32_1 >> 5)) + uint32_1) ^ this._sum1[index];
                uint32_1 -= (((uint32_2 << 4) ^ (uint32_2 >> 5)) + uint32_2) ^ this._sum0[index];
            }
            Pack.UInt32_To_BE( uint32_1, outBytes, outOff );
            Pack.UInt32_To_BE( uint32_2, outBytes, outOff + 4 );
            return 8;
        }
    }
}
