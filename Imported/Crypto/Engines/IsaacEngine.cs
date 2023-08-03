// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.IsaacEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class IsaacEngine : IStreamCipher
    {
        private static readonly int sizeL = 8;
        private static readonly int stateArraySize = sizeL << 5;
        private uint[] engineState = null;
        private uint[] results = null;
        private uint a = 0;
        private uint b = 0;
        private uint c = 0;
        private int index = 0;
        private byte[] keyStream = new byte[stateArraySize << 2];
        private byte[] workingKey = null;
        private bool initialised = false;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "invalid parameter passed to ISAAC Init - " + Platform.GetTypeName( parameters ), nameof( parameters ) );
            this.setKey( ((KeyParameter)parameters).GetKey() );
        }

        public virtual byte ReturnByte( byte input )
        {
            if (this.index == 0)
            {
                this.isaac();
                this.keyStream = Pack.UInt32_To_BE( this.results );
            }
            byte num = (byte)(this.keyStream[this.index] ^ (uint)input);
            this.index = (this.index + 1) & 1023;
            return num;
        }

        public virtual void ProcessBytes( byte[] input, int inOff, int len, byte[] output, int outOff )
        {
            if (!this.initialised)
                throw new InvalidOperationException( this.AlgorithmName + " not initialised" );
            Check.DataLength( input, inOff, len, "input buffer too short" );
            Check.OutputLength( output, outOff, len, "output buffer too short" );
            for (int index = 0; index < len; ++index)
            {
                if (this.index == 0)
                {
                    this.isaac();
                    this.keyStream = Pack.UInt32_To_BE( this.results );
                }
                output[index + outOff] = (byte)(this.keyStream[this.index] ^ (uint)input[index + inOff]);
                this.index = (this.index + 1) & 1023;
            }
        }

        public virtual string AlgorithmName => "ISAAC";

        public virtual void Reset() => this.setKey( this.workingKey );

        private void setKey( byte[] keyBytes )
        {
            this.workingKey = keyBytes;
            if (this.engineState == null)
                this.engineState = new uint[stateArraySize];
            if (this.results == null)
                this.results = new uint[stateArraySize];
            for (int index = 0; index < stateArraySize; ++index)
                this.engineState[index] = this.results[index] = 0U;
            this.a = this.b = this.c = 0U;
            this.index = 0;
            byte[] numArray1 = new byte[keyBytes.Length + (keyBytes.Length & 3)];
            Array.Copy( keyBytes, 0, numArray1, 0, keyBytes.Length );
            for (int off = 0; off < numArray1.Length; off += 4)
                this.results[off >> 2] = Pack.LE_To_UInt32( numArray1, off );
            uint[] x = new uint[sizeL];
            for (int index = 0; index < sizeL; ++index)
                x[index] = 2654435769U;
            for (int index = 0; index < 4; ++index)
                this.mix( x );
            for (int index1 = 0; index1 < 2; ++index1)
            {
                for (int index2 = 0; index2 < stateArraySize; index2 += sizeL)
                {
                    for (int index3 = 0; index3 < sizeL; ++index3)
                    {
                        uint[] numArray2;
                        int index4;
                        int num = (int)(numArray2 = x)[(int)(IntPtr)(index4 = index3)] + (index1 < 1 ? (int)this.results[index2 + index3] : (int)this.engineState[index2 + index3]);
                        numArray2[index4] = (uint)num;
                    }
                    this.mix( x );
                    for (int index5 = 0; index5 < sizeL; ++index5)
                        this.engineState[index2 + index5] = x[index5];
                }
            }
            this.isaac();
            this.initialised = true;
        }

        private void isaac()
        {
            this.b += ++this.c;
            for (int index = 0; index < stateArraySize; ++index)
            {
                uint num1 = this.engineState[index];
                switch (index & 3)
                {
                    case 0:
                        this.a ^= this.a << 13;
                        break;
                    case 1:
                        this.a ^= this.a >> 6;
                        break;
                    case 2:
                        this.a ^= this.a << 2;
                        break;
                    case 3:
                        this.a ^= this.a >> 16;
                        break;
                }
                this.a += this.engineState[(index + 128) & byte.MaxValue];
                uint num2;
                this.engineState[index] = num2 = this.engineState[(int)(num1 >> 2) & byte.MaxValue] + this.a + this.b;
                this.results[index] = this.b = this.engineState[(int)(num2 >> 10) & byte.MaxValue] + num1;
            }
        }

        private void mix( uint[] x )
        {
            uint[] numArray1;
            (numArray1 = x)[0] = numArray1[0] ^ (x[1] << 11);
            uint[] numArray2;
            (numArray2 = x)[3] = numArray2[3] + x[0];
            uint[] numArray3;
            (numArray3 = x)[1] = numArray3[1] + x[2];
            uint[] numArray4;
            (numArray4 = x)[1] = numArray4[1] ^ (x[2] >> 2);
            uint[] numArray5;
            (numArray5 = x)[4] = numArray5[4] + x[1];
            uint[] numArray6;
            (numArray6 = x)[2] = numArray6[2] + x[3];
            uint[] numArray7;
            (numArray7 = x)[2] = numArray7[2] ^ (x[3] << 8);
            uint[] numArray8;
            (numArray8 = x)[5] = numArray8[5] + x[2];
            uint[] numArray9;
            (numArray9 = x)[3] = numArray9[3] + x[4];
            uint[] numArray10;
            (numArray10 = x)[3] = numArray10[3] ^ (x[4] >> 16);
            uint[] numArray11;
            (numArray11 = x)[6] = numArray11[6] + x[3];
            uint[] numArray12;
            (numArray12 = x)[4] = numArray12[4] + x[5];
            uint[] numArray13;
            (numArray13 = x)[4] = numArray13[4] ^ (x[5] << 10);
            uint[] numArray14;
            (numArray14 = x)[7] = numArray14[7] + x[4];
            uint[] numArray15;
            (numArray15 = x)[5] = numArray15[5] + x[6];
            uint[] numArray16;
            (numArray16 = x)[5] = numArray16[5] ^ (x[6] >> 4);
            uint[] numArray17;
            (numArray17 = x)[0] = numArray17[0] + x[5];
            uint[] numArray18;
            (numArray18 = x)[6] = numArray18[6] + x[7];
            uint[] numArray19;
            (numArray19 = x)[6] = numArray19[6] ^ (x[7] << 8);
            uint[] numArray20;
            (numArray20 = x)[1] = numArray20[1] + x[6];
            uint[] numArray21;
            (numArray21 = x)[7] = numArray21[7] + x[0];
            uint[] numArray22;
            (numArray22 = x)[7] = numArray22[7] ^ (x[0] >> 9);
            uint[] numArray23;
            (numArray23 = x)[2] = numArray23[2] + x[7];
            uint[] numArray24;
            (numArray24 = x)[0] = numArray24[0] + x[1];
        }
    }
}
