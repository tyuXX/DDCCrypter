// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.HC128Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class HC128Engine : IStreamCipher
    {
        private uint[] p = new uint[512];
        private uint[] q = new uint[512];
        private uint cnt = 0;
        private byte[] key;
        private byte[] iv;
        private bool initialised;
        private byte[] buf = new byte[4];
        private int idx = 0;

        private static uint F1( uint x ) => RotateRight( x, 7 ) ^ RotateRight( x, 18 ) ^ (x >> 3);

        private static uint F2( uint x ) => RotateRight( x, 17 ) ^ RotateRight( x, 19 ) ^ (x >> 10);

        private uint G1( uint x, uint y, uint z ) => (RotateRight( x, 10 ) ^ RotateRight( z, 23 )) + RotateRight( y, 8 );

        private uint G2( uint x, uint y, uint z ) => (RotateLeft( x, 10 ) ^ RotateLeft( z, 23 )) + RotateLeft( y, 8 );

        private static uint RotateLeft( uint x, int bits ) => (x << bits) | (x >> -bits);

        private static uint RotateRight( uint x, int bits ) => (x >> bits) | (x << -bits);

        private uint H1( uint x ) => this.q[(int)(IntPtr)(x & byte.MaxValue)] + this.q[(int)(IntPtr)(uint)(((int)(x >> 16) & byte.MaxValue) + 256)];

        private uint H2( uint x ) => this.p[(int)(IntPtr)(x & byte.MaxValue)] + this.p[(int)(IntPtr)(uint)(((int)(x >> 16) & byte.MaxValue) + 256)];

        private static uint Mod1024( uint x ) => x & 1023U;

        private static uint Mod512( uint x ) => x & 511U;

        private static uint Dim( uint x, uint y ) => Mod512( x - y );

        private uint Step()
        {
            uint x = Mod512( this.cnt );
            uint num;
            if (this.cnt < 512U)
            {
                uint[] p;
                IntPtr index;
                (p = this.p)[(int)(index = (IntPtr)x)] = p[(int)index] + this.G1( this.p[(int)(IntPtr)Dim( x, 3U )], this.p[(int)(IntPtr)Dim( x, 10U )], this.p[(int)(IntPtr)Dim( x, 511U )] );
                num = this.H1( this.p[(int)(IntPtr)Dim( x, 12U )] ) ^ this.p[(int)(IntPtr)x];
            }
            else
            {
                uint[] q;
                IntPtr index;
                (q = this.q)[(int)(index = (IntPtr)x)] = q[(int)index] + this.G2( this.q[(int)(IntPtr)Dim( x, 3U )], this.q[(int)(IntPtr)Dim( x, 10U )], this.q[(int)(IntPtr)Dim( x, 511U )] );
                num = this.H2( this.q[(int)(IntPtr)Dim( x, 12U )] ) ^ this.q[(int)(IntPtr)x];
            }
            this.cnt = Mod1024( this.cnt + 1U );
            return num;
        }

        private void Init()
        {
            if (this.key.Length != 16)
                throw new ArgumentException( "The key must be 128 bits long" );
            this.cnt = 0U;
            uint[] numArray = new uint[1280];
            for (int index = 0; index < 16; ++index)
                numArray[index >> 2] |= (uint)this.key[index] << (8 * (index & 3));
            Array.Copy( numArray, 0, numArray, 4, 4 );
            for (int index = 0; index < this.iv.Length && index < 16; ++index)
                numArray[(index >> 2) + 8] |= (uint)this.iv[index] << (8 * (index & 3));
            Array.Copy( numArray, 8, numArray, 12, 4 );
            for (uint index = 16; index < 1280U; ++index)
                numArray[(int)(IntPtr)index] = F2( numArray[(int)(IntPtr)(index - 2U)] ) + numArray[(int)(IntPtr)(index - 7U)] + F1( numArray[(int)(IntPtr)(index - 15U)] ) + numArray[(int)(IntPtr)(index - 16U)] + index;
            Array.Copy( numArray, 256, p, 0, 512 );
            Array.Copy( numArray, 768, q, 0, 512 );
            for (int index = 0; index < 512; ++index)
                this.p[index] = this.Step();
            for (int index = 0; index < 512; ++index)
                this.q[index] = this.Step();
            this.cnt = 0U;
        }

        public virtual string AlgorithmName => "HC-128";

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            ICipherParameters cipherParameters = parameters;
            if (parameters is ParametersWithIV)
            {
                this.iv = ((ParametersWithIV)parameters).GetIV();
                cipherParameters = ((ParametersWithIV)parameters).Parameters;
            }
            else
                this.iv = new byte[0];
            this.key = cipherParameters is KeyParameter ? ((KeyParameter)cipherParameters).GetKey() : throw new ArgumentException( "Invalid parameter passed to HC128 init - " + Platform.GetTypeName( parameters ), nameof( parameters ) );
            this.Init();
            this.initialised = true;
        }

        private byte GetByte()
        {
            if (this.idx == 0)
                Pack.UInt32_To_LE( this.Step(), this.buf );
            byte num = this.buf[this.idx];
            this.idx = (this.idx + 1) & 3;
            return num;
        }

        public virtual void ProcessBytes( byte[] input, int inOff, int len, byte[] output, int outOff )
        {
            if (!this.initialised)
                throw new InvalidOperationException( this.AlgorithmName + " not initialised" );
            Check.DataLength( input, inOff, len, "input buffer too short" );
            Check.OutputLength( output, outOff, len, "output buffer too short" );
            for (int index = 0; index < len; ++index)
                output[outOff + index] = (byte)(input[inOff + index] ^ (uint)this.GetByte());
        }

        public virtual void Reset()
        {
            this.idx = 0;
            this.Init();
        }

        public virtual byte ReturnByte( byte input ) => (byte)(input ^ (uint)this.GetByte());
    }
}
