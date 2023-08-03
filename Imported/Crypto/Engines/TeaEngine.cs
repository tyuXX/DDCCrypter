// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.TeaEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class TeaEngine : IBlockCipher
    {
        private const int rounds = 32;
        private const int block_size = 8;
        private const uint delta = 2654435769;
        private const uint d_sum = 3337565984;
        private uint _a;
        private uint _b;
        private uint _c;
        private uint _d;
        private bool _initialised;
        private bool _forEncryption;

        public TeaEngine() => this._initialised = false;

        public virtual string AlgorithmName => "TEA";

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
            this._a = Pack.BE_To_UInt32( key, 0 );
            this._b = Pack.BE_To_UInt32( key, 4 );
            this._c = Pack.BE_To_UInt32( key, 8 );
            this._d = Pack.BE_To_UInt32( key, 12 );
        }

        private int encryptBlock( byte[] inBytes, int inOff, byte[] outBytes, int outOff )
        {
            uint uint32_1 = Pack.BE_To_UInt32( inBytes, inOff );
            uint uint32_2 = Pack.BE_To_UInt32( inBytes, inOff + 4 );
            uint num = 0;
            for (int index = 0; index != 32; ++index)
            {
                num += 2654435769U;
                uint32_1 += (uint)((((int)uint32_2 << 4) + (int)this._a) ^ ((int)uint32_2 + (int)num) ^ ((int)(uint32_2 >> 5) + (int)this._b));
                uint32_2 += (uint)((((int)uint32_1 << 4) + (int)this._c) ^ ((int)uint32_1 + (int)num) ^ ((int)(uint32_1 >> 5) + (int)this._d));
            }
            Pack.UInt32_To_BE( uint32_1, outBytes, outOff );
            Pack.UInt32_To_BE( uint32_2, outBytes, outOff + 4 );
            return 8;
        }

        private int decryptBlock( byte[] inBytes, int inOff, byte[] outBytes, int outOff )
        {
            uint uint32_1 = Pack.BE_To_UInt32( inBytes, inOff );
            uint uint32_2 = Pack.BE_To_UInt32( inBytes, inOff + 4 );
            uint num = 3337565984;
            for (int index = 0; index != 32; ++index)
            {
                uint32_2 -= (uint)((((int)uint32_1 << 4) + (int)this._c) ^ ((int)uint32_1 + (int)num) ^ ((int)(uint32_1 >> 5) + (int)this._d));
                uint32_1 -= (uint)((((int)uint32_2 << 4) + (int)this._a) ^ ((int)uint32_2 + (int)num) ^ ((int)(uint32_2 >> 5) + (int)this._b));
                num -= 2654435769U;
            }
            Pack.UInt32_To_BE( uint32_1, outBytes, outOff );
            Pack.UInt32_To_BE( uint32_2, outBytes, outOff + 4 );
            return 8;
        }
    }
}
