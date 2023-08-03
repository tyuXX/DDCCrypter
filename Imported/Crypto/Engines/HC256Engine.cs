// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.HC256Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class HC256Engine : IStreamCipher
    {
        private uint[] p = new uint[1024];
        private uint[] q = new uint[1024];
        private uint cnt = 0;
        private byte[] key;
        private byte[] iv;
        private bool initialised;
        private byte[] buf = new byte[4];
        private int idx = 0;

        private uint Step()
        {
            uint index1 = this.cnt & 1023U;
            uint num1;
            if (this.cnt < 1024U)
            {
                uint x1 = this.p[(int)(IntPtr)(uint)(((int)index1 - 3) & 1023)];
                uint x2 = this.p[(int)(IntPtr)(uint)(((int)index1 - 1023) & 1023)];
                uint[] p;
                IntPtr index2;
                (p = this.p)[(int)(index2 = (IntPtr)index1)] = p[(int)index2] + this.p[(int)(IntPtr)(uint)(((int)index1 - 10) & 1023)] + (RotateRight( x1, 10 ) ^ RotateRight( x2, 23 )) + this.q[(int)(IntPtr)(uint)(((int)x1 ^ (int)x2) & 1023)];
                uint num2 = this.p[(int)(IntPtr)(uint)(((int)index1 - 12) & 1023)];
                num1 = (this.q[(int)(IntPtr)(num2 & byte.MaxValue)] + this.q[(int)(IntPtr)(uint)(((int)(num2 >> 8) & byte.MaxValue) + 256)] + this.q[(int)(IntPtr)(uint)(((int)(num2 >> 16) & byte.MaxValue) + 512)] + this.q[(int)(IntPtr)(uint)(((int)(num2 >> 24) & byte.MaxValue) + 768)]) ^ this.p[(int)(IntPtr)index1];
            }
            else
            {
                uint x3 = this.q[(int)(IntPtr)(uint)(((int)index1 - 3) & 1023)];
                uint x4 = this.q[(int)(IntPtr)(uint)(((int)index1 - 1023) & 1023)];
                uint[] q;
                IntPtr index3;
                (q = this.q)[(int)(index3 = (IntPtr)index1)] = q[(int)index3] + this.q[(int)(IntPtr)(uint)(((int)index1 - 10) & 1023)] + (RotateRight( x3, 10 ) ^ RotateRight( x4, 23 )) + this.p[(int)(IntPtr)(uint)(((int)x3 ^ (int)x4) & 1023)];
                uint num3 = this.q[(int)(IntPtr)(uint)(((int)index1 - 12) & 1023)];
                num1 = (this.p[(int)(IntPtr)(num3 & byte.MaxValue)] + this.p[(int)(IntPtr)(uint)(((int)(num3 >> 8) & byte.MaxValue) + 256)] + this.p[(int)(IntPtr)(uint)(((int)(num3 >> 16) & byte.MaxValue) + 512)] + this.p[(int)(IntPtr)(uint)(((int)(num3 >> 24) & byte.MaxValue) + 768)]) ^ this.q[(int)(IntPtr)index1];
            }
            this.cnt = (uint)(((int)this.cnt + 1) & 2047);
            return num1;
        }

        private void Init()
        {
            if (this.key.Length != 32 && this.key.Length != 16)
                throw new ArgumentException( "The key must be 128/256 bits long" );
            if (this.iv.Length < 16)
                throw new ArgumentException( "The IV must be at least 128 bits long" );
            if (this.key.Length != 32)
            {
                byte[] destinationArray = new byte[32];
                Array.Copy( key, 0, destinationArray, 0, this.key.Length );
                Array.Copy( key, 0, destinationArray, 16, this.key.Length );
                this.key = destinationArray;
            }
            if (this.iv.Length < 32)
            {
                byte[] destinationArray = new byte[32];
                Array.Copy( iv, 0, destinationArray, 0, this.iv.Length );
                Array.Copy( iv, 0, destinationArray, this.iv.Length, destinationArray.Length - this.iv.Length );
                this.iv = destinationArray;
            }
            this.cnt = 0U;
            uint[] sourceArray = new uint[2560];
            for (int index = 0; index < 32; ++index)
                sourceArray[index >> 2] |= (uint)this.key[index] << (8 * (index & 3));
            for (int index = 0; index < 32; ++index)
                sourceArray[(index >> 2) + 8] |= (uint)this.iv[index] << (8 * (index & 3));
            for (uint index = 16; index < 2560U; ++index)
            {
                uint x1 = sourceArray[(int)(IntPtr)(index - 2U)];
                uint x2 = sourceArray[(int)(IntPtr)(index - 15U)];
                sourceArray[(int)(IntPtr)index] = (uint)(((int)RotateRight( x1, 17 ) ^ (int)RotateRight( x1, 19 ) ^ (int)(x1 >> 10)) + (int)sourceArray[(int)(IntPtr)(index - 7U)] + ((int)RotateRight( x2, 7 ) ^ (int)RotateRight( x2, 18 ) ^ (int)(x2 >> 3))) + sourceArray[(int)(IntPtr)(index - 16U)] + index;
            }
            Array.Copy( sourceArray, 512, p, 0, 1024 );
            Array.Copy( sourceArray, 1536, q, 0, 1024 );
            for (int index = 0; index < 4096; ++index)
            {
                int num = (int)this.Step();
            }
            this.cnt = 0U;
        }

        public virtual string AlgorithmName => "HC-256";

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
            this.key = cipherParameters is KeyParameter ? ((KeyParameter)cipherParameters).GetKey() : throw new ArgumentException( "Invalid parameter passed to HC256 init - " + Platform.GetTypeName( parameters ), nameof( parameters ) );
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

        private static uint RotateRight( uint x, int bits ) => (x >> bits) | (x << -bits);
    }
}
