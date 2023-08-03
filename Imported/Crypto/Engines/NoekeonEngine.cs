// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.NoekeonEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class NoekeonEngine : IBlockCipher
    {
        private const int GenericSize = 16;
        private static readonly uint[] nullVector = new uint[4];
        private static readonly uint[] roundConstants = new uint[17]
        {
      128U,
      27U,
      54U,
      108U,
      216U,
      171U,
      77U,
      154U,
      47U,
      94U,
      188U,
      99U,
      198U,
      151U,
      53U,
      106U,
      212U
        };
        private uint[] state = new uint[4];
        private uint[] subKeys = new uint[4];
        private uint[] decryptKeys = new uint[4];
        private bool _initialised;
        private bool _forEncryption;

        public NoekeonEngine() => this._initialised = false;

        public virtual string AlgorithmName => "Noekeon";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 16;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "Invalid parameters passed to Noekeon init - " + Platform.GetTypeName( parameters ), nameof( parameters ) );
            this._forEncryption = forEncryption;
            this._initialised = true;
            this.setKey( ((KeyParameter)parameters).GetKey() );
        }

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (!this._initialised)
                throw new InvalidOperationException( this.AlgorithmName + " not initialised" );
            Check.DataLength( input, inOff, 16, "input buffer too short" );
            Check.OutputLength( output, outOff, 16, "output buffer too short" );
            return !this._forEncryption ? this.decryptBlock( input, inOff, output, outOff ) : this.encryptBlock( input, inOff, output, outOff );
        }

        public virtual void Reset()
        {
        }

        private void setKey( byte[] key )
        {
            this.subKeys[0] = Pack.BE_To_UInt32( key, 0 );
            this.subKeys[1] = Pack.BE_To_UInt32( key, 4 );
            this.subKeys[2] = Pack.BE_To_UInt32( key, 8 );
            this.subKeys[3] = Pack.BE_To_UInt32( key, 12 );
        }

        private int encryptBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            this.state[0] = Pack.BE_To_UInt32( input, inOff );
            this.state[1] = Pack.BE_To_UInt32( input, inOff + 4 );
            this.state[2] = Pack.BE_To_UInt32( input, inOff + 8 );
            this.state[3] = Pack.BE_To_UInt32( input, inOff + 12 );
            int index;
            for (index = 0; index < 16; ++index)
            {
                uint[] state;
                (state = this.state)[0] = state[0] ^ roundConstants[index];
                this.theta( this.state, this.subKeys );
                this.pi1( this.state );
                this.gamma( this.state );
                this.pi2( this.state );
            }
            uint[] state1;
            (state1 = this.state)[0] = state1[0] ^ roundConstants[index];
            this.theta( this.state, this.subKeys );
            Pack.UInt32_To_BE( this.state[0], output, outOff );
            Pack.UInt32_To_BE( this.state[1], output, outOff + 4 );
            Pack.UInt32_To_BE( this.state[2], output, outOff + 8 );
            Pack.UInt32_To_BE( this.state[3], output, outOff + 12 );
            return 16;
        }

        private int decryptBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            this.state[0] = Pack.BE_To_UInt32( input, inOff );
            this.state[1] = Pack.BE_To_UInt32( input, inOff + 4 );
            this.state[2] = Pack.BE_To_UInt32( input, inOff + 8 );
            this.state[3] = Pack.BE_To_UInt32( input, inOff + 12 );
            Array.Copy( subKeys, 0, decryptKeys, 0, this.subKeys.Length );
            this.theta( this.decryptKeys, nullVector );
            int index;
            for (index = 16; index > 0; --index)
            {
                this.theta( this.state, this.decryptKeys );
                uint[] state;
                (state = this.state)[0] = state[0] ^ roundConstants[index];
                this.pi1( this.state );
                this.gamma( this.state );
                this.pi2( this.state );
            }
            this.theta( this.state, this.decryptKeys );
            uint[] state1;
            (state1 = this.state)[0] = state1[0] ^ roundConstants[index];
            Pack.UInt32_To_BE( this.state[0], output, outOff );
            Pack.UInt32_To_BE( this.state[1], output, outOff + 4 );
            Pack.UInt32_To_BE( this.state[2], output, outOff + 8 );
            Pack.UInt32_To_BE( this.state[3], output, outOff + 12 );
            return 16;
        }

        private void gamma( uint[] a )
        {
            uint[] numArray1;
            (numArray1 = a)[1] = numArray1[1] ^ (uint)(~(int)a[3] & ~(int)a[2]);
            uint[] numArray2;
            (numArray2 = a)[0] = numArray2[0] ^ (a[2] & a[1]);
            uint num = a[3];
            a[3] = a[0];
            a[0] = num;
            uint[] numArray3;
            (numArray3 = a)[2] = numArray3[2] ^ a[0] ^ a[1] ^ a[3];
            uint[] numArray4;
            (numArray4 = a)[1] = numArray4[1] ^ (uint)(~(int)a[3] & ~(int)a[2]);
            uint[] numArray5;
            (numArray5 = a)[0] = numArray5[0] ^ (a[2] & a[1]);
        }

        private void theta( uint[] a, uint[] k )
        {
            uint x1 = a[0] ^ a[2];
            uint num1 = x1 ^ this.rotl( x1, 8 ) ^ this.rotl( x1, 24 );
            uint[] numArray1;
            (numArray1 = a)[1] = numArray1[1] ^ num1;
            uint[] numArray2;
            (numArray2 = a)[3] = numArray2[3] ^ num1;
            for (int index = 0; index < 4; ++index)
                a[index] ^= k[index];
            uint x2 = a[1] ^ a[3];
            uint num2 = x2 ^ this.rotl( x2, 8 ) ^ this.rotl( x2, 24 );
            uint[] numArray3;
            (numArray3 = a)[0] = numArray3[0] ^ num2;
            uint[] numArray4;
            (numArray4 = a)[2] = numArray4[2] ^ num2;
        }

        private void pi1( uint[] a )
        {
            a[1] = this.rotl( a[1], 1 );
            a[2] = this.rotl( a[2], 5 );
            a[3] = this.rotl( a[3], 2 );
        }

        private void pi2( uint[] a )
        {
            a[1] = this.rotl( a[1], 31 );
            a[2] = this.rotl( a[2], 27 );
            a[3] = this.rotl( a[3], 30 );
        }

        private uint rotl( uint x, int y ) => (x << y) | (x >> (32 - y));
    }
}
