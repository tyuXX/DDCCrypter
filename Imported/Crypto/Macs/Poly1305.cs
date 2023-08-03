// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.Poly1305
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class Poly1305 : IMac
    {
        private const int BLOCK_SIZE = 16;
        private readonly IBlockCipher cipher;
        private readonly byte[] singleByte = new byte[1];
        private uint r0;
        private uint r1;
        private uint r2;
        private uint r3;
        private uint r4;
        private uint s1;
        private uint s2;
        private uint s3;
        private uint s4;
        private uint k0;
        private uint k1;
        private uint k2;
        private uint k3;
        private byte[] currentBlock = new byte[16];
        private int currentBlockOffset = 0;
        private uint h0;
        private uint h1;
        private uint h2;
        private uint h3;
        private uint h4;

        public Poly1305() => this.cipher = null;

        public Poly1305( IBlockCipher cipher ) => this.cipher = cipher.GetBlockSize() == 16 ? cipher : throw new ArgumentException( "Poly1305 requires a 128 bit block cipher." );

        public void Init( ICipherParameters parameters )
        {
            byte[] nonce = null;
            if (this.cipher != null)
            {
                ParametersWithIV parametersWithIv = parameters is ParametersWithIV ? (ParametersWithIV)parameters : throw new ArgumentException( "Poly1305 requires an IV when used with a block cipher.", nameof( parameters ) );
                nonce = parametersWithIv.GetIV();
                parameters = parametersWithIv.Parameters;
            }
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "Poly1305 requires a key." );
            this.SetKey( ((KeyParameter)parameters).GetKey(), nonce );
            this.Reset();
        }

        private void SetKey( byte[] key, byte[] nonce )
        {
            if (this.cipher != null && (nonce == null || nonce.Length != 16))
                throw new ArgumentException( "Poly1305 requires a 128 bit IV." );
            Poly1305KeyGenerator.CheckKey( key );
            uint uint32_1 = Pack.LE_To_UInt32( key, 16 );
            uint uint32_2 = Pack.LE_To_UInt32( key, 20 );
            uint uint32_3 = Pack.LE_To_UInt32( key, 24 );
            uint uint32_4 = Pack.LE_To_UInt32( key, 28 );
            this.r0 = uint32_1 & 67108863U;
            this.r1 = ((uint32_1 >> 26) | (uint32_2 << 6)) & 67108611U;
            this.r2 = ((uint32_2 >> 20) | (uint32_3 << 12)) & 67092735U;
            this.r3 = ((uint32_3 >> 14) | (uint32_4 << 18)) & 66076671U;
            this.r4 = (uint32_4 >> 8) & 1048575U;
            this.s1 = this.r1 * 5U;
            this.s2 = this.r2 * 5U;
            this.s3 = this.r3 * 5U;
            this.s4 = this.r4 * 5U;
            byte[] numArray;
            if (this.cipher == null)
            {
                numArray = key;
            }
            else
            {
                numArray = new byte[16];
                this.cipher.Init( true, new KeyParameter( key, 0, 16 ) );
                this.cipher.ProcessBlock( nonce, 0, numArray, 0 );
            }
            this.k0 = Pack.LE_To_UInt32( numArray, 0 );
            this.k1 = Pack.LE_To_UInt32( numArray, 4 );
            this.k2 = Pack.LE_To_UInt32( numArray, 8 );
            this.k3 = Pack.LE_To_UInt32( numArray, 12 );
        }

        public string AlgorithmName => this.cipher != null ? "Poly1305-" + this.cipher.AlgorithmName : nameof( Poly1305 );

        public int GetMacSize() => 16;

        public void Update( byte input )
        {
            this.singleByte[0] = input;
            this.BlockUpdate( this.singleByte, 0, 1 );
        }

        public void BlockUpdate( byte[] input, int inOff, int len )
        {
            int num = 0;
            while (len > num)
            {
                if (this.currentBlockOffset == 16)
                {
                    this.processBlock();
                    this.currentBlockOffset = 0;
                }
                int length = System.Math.Min( len - num, 16 - this.currentBlockOffset );
                Array.Copy( input, num + inOff, currentBlock, this.currentBlockOffset, length );
                num += length;
                this.currentBlockOffset += length;
            }
        }

        private void processBlock()
        {
            if (this.currentBlockOffset < 16)
            {
                this.currentBlock[this.currentBlockOffset] = 1;
                for (int index = this.currentBlockOffset + 1; index < 16; ++index)
                    this.currentBlock[index] = 0;
            }
            ulong uint32_1 = Pack.LE_To_UInt32( this.currentBlock, 0 );
            ulong uint32_2 = Pack.LE_To_UInt32( this.currentBlock, 4 );
            ulong uint32_3 = Pack.LE_To_UInt32( this.currentBlock, 8 );
            ulong uint32_4 = Pack.LE_To_UInt32( this.currentBlock, 12 );
            this.h0 += (uint)(uint32_1 & 67108863UL);
            this.h1 += (uint)((((uint32_2 << 32) | uint32_1) >> 26) & 67108863UL);
            this.h2 += (uint)((((uint32_3 << 32) | uint32_2) >> 20) & 67108863UL);
            this.h3 += (uint)((((uint32_4 << 32) | uint32_3) >> 14) & 67108863UL);
            this.h4 += (uint)(uint32_4 >> 8);
            if (this.currentBlockOffset == 16)
                this.h4 += 16777216U;
            ulong num1 = mul32x32_64( this.h0, this.r0 ) + mul32x32_64( this.h1, this.s4 ) + mul32x32_64( this.h2, this.s3 ) + mul32x32_64( this.h3, this.s2 ) + mul32x32_64( this.h4, this.s1 );
            ulong num2 = mul32x32_64( this.h0, this.r1 ) + mul32x32_64( this.h1, this.r0 ) + mul32x32_64( this.h2, this.s4 ) + mul32x32_64( this.h3, this.s3 ) + mul32x32_64( this.h4, this.s2 );
            ulong num3 = mul32x32_64( this.h0, this.r2 ) + mul32x32_64( this.h1, this.r1 ) + mul32x32_64( this.h2, this.r0 ) + mul32x32_64( this.h3, this.s4 ) + mul32x32_64( this.h4, this.s3 );
            ulong num4 = mul32x32_64( this.h0, this.r3 ) + mul32x32_64( this.h1, this.r2 ) + mul32x32_64( this.h2, this.r1 ) + mul32x32_64( this.h3, this.r0 ) + mul32x32_64( this.h4, this.s4 );
            ulong num5 = mul32x32_64( this.h0, this.r4 ) + mul32x32_64( this.h1, this.r3 ) + mul32x32_64( this.h2, this.r2 ) + mul32x32_64( this.h3, this.r1 ) + mul32x32_64( this.h4, this.r0 );
            this.h0 = (uint)num1 & 67108863U;
            ulong num6 = num1 >> 26;
            ulong num7 = num2 + num6;
            this.h1 = (uint)num7 & 67108863U;
            ulong num8 = num7 >> 26;
            ulong num9 = num3 + num8;
            this.h2 = (uint)num9 & 67108863U;
            ulong num10 = num9 >> 26;
            ulong num11 = num4 + num10;
            this.h3 = (uint)num11 & 67108863U;
            ulong num12 = num11 >> 26;
            ulong num13 = num5 + num12;
            this.h4 = (uint)num13 & 67108863U;
            this.h0 += (uint)((num13 >> 26) * 5UL);
        }

        public int DoFinal( byte[] output, int outOff )
        {
            if (outOff + 16 > output.Length)
                throw new DataLengthException( "Output buffer is too short." );
            if (this.currentBlockOffset > 0)
                this.processBlock();
            uint num1 = this.h0 >> 26;
            this.h0 &= 67108863U;
            this.h1 += num1;
            uint num2 = this.h1 >> 26;
            this.h1 &= 67108863U;
            this.h2 += num2;
            uint num3 = this.h2 >> 26;
            this.h2 &= 67108863U;
            this.h3 += num3;
            uint num4 = this.h3 >> 26;
            this.h3 &= 67108863U;
            this.h4 += num4;
            uint num5 = this.h4 >> 26;
            this.h4 &= 67108863U;
            this.h0 += num5 * 5U;
            uint num6 = this.h0 + 5U;
            uint num7 = num6 >> 26;
            uint num8 = num6 & 67108863U;
            uint num9 = this.h1 + num7;
            uint num10 = num9 >> 26;
            uint num11 = num9 & 67108863U;
            uint num12 = this.h2 + num10;
            uint num13 = num12 >> 26;
            uint num14 = num12 & 67108863U;
            uint num15 = this.h3 + num13;
            uint num16 = num15 >> 26;
            uint num17 = num15 & 67108863U;
            uint num18 = (uint)((int)this.h4 + (int)num16 - 67108864);
            uint num19 = (num18 >> 31) - 1U;
            uint num20 = ~num19;
            this.h0 = (uint)(((int)this.h0 & (int)num20) | ((int)num8 & (int)num19));
            this.h1 = (uint)(((int)this.h1 & (int)num20) | ((int)num11 & (int)num19));
            this.h2 = (uint)(((int)this.h2 & (int)num20) | ((int)num14 & (int)num19));
            this.h3 = (uint)(((int)this.h3 & (int)num20) | ((int)num17 & (int)num19));
            this.h4 = (uint)(((int)this.h4 & (int)num20) | ((int)num18 & (int)num19));
            ulong n1 = (this.h0 | (this.h1 << 26)) + (ulong)this.k0;
            ulong num21 = ((this.h1 >> 6) | (this.h2 << 20)) + (ulong)this.k1;
            ulong num22 = ((this.h2 >> 12) | (this.h3 << 14)) + (ulong)this.k2;
            ulong num23 = ((this.h3 >> 18) | (this.h4 << 8)) + (ulong)this.k3;
            Pack.UInt32_To_LE( (uint)n1, output, outOff );
            ulong n2 = num21 + (n1 >> 32);
            Pack.UInt32_To_LE( (uint)n2, output, outOff + 4 );
            ulong n3 = num22 + (n2 >> 32);
            Pack.UInt32_To_LE( (uint)n3, output, outOff + 8 );
            Pack.UInt32_To_LE( (uint)(num23 + (n3 >> 32)), output, outOff + 12 );
            this.Reset();
            return 16;
        }

        public void Reset()
        {
            this.currentBlockOffset = 0;
            this.h0 = this.h1 = this.h2 = this.h3 = this.h4 = 0U;
        }

        private static ulong mul32x32_64( uint i1, uint i2 ) => i1 * (ulong)i2;
    }
}
