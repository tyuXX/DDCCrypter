// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.Cast6Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public sealed class Cast6Engine : Cast5Engine
    {
        private const int ROUNDS = 12;
        private const int BLOCK_SIZE = 16;
        private int[] _Kr = new int[48];
        private uint[] _Km = new uint[48];
        private int[] _Tr = new int[192];
        private uint[] _Tm = new uint[192];
        private uint[] _workingKey = new uint[8];

        public override string AlgorithmName => "CAST6";

        public override void Reset()
        {
        }

        public override int GetBlockSize() => 16;

        internal override void SetKey( byte[] key )
        {
            uint num1 = 1518500249;
            uint num2 = 1859775393;
            int num3 = 19;
            int num4 = 17;
            for (int index1 = 0; index1 < 24; ++index1)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    this._Tm[(index1 * 8) + index2] = num1;
                    num1 += num2;
                    this._Tr[(index1 * 8) + index2] = num3;
                    num3 = (num3 + num4) & 31;
                }
            }
            byte[] bs = new byte[64];
            key.CopyTo( bs, 0 );
            for (int index = 0; index < 8; ++index)
                this._workingKey[index] = Pack.BE_To_UInt32( bs, index * 4 );
            for (int index3 = 0; index3 < 12; ++index3)
            {
                int index4 = index3 * 2 * 8;
                uint[] workingKey1;
                (workingKey1 = this._workingKey)[6] = workingKey1[6] ^ F1( this._workingKey[7], this._Tm[index4], this._Tr[index4] );
                uint[] workingKey2;
                (workingKey2 = this._workingKey)[5] = workingKey2[5] ^ F2( this._workingKey[6], this._Tm[index4 + 1], this._Tr[index4 + 1] );
                uint[] workingKey3;
                (workingKey3 = this._workingKey)[4] = workingKey3[4] ^ F3( this._workingKey[5], this._Tm[index4 + 2], this._Tr[index4 + 2] );
                uint[] workingKey4;
                (workingKey4 = this._workingKey)[3] = workingKey4[3] ^ F1( this._workingKey[4], this._Tm[index4 + 3], this._Tr[index4 + 3] );
                uint[] workingKey5;
                (workingKey5 = this._workingKey)[2] = workingKey5[2] ^ F2( this._workingKey[3], this._Tm[index4 + 4], this._Tr[index4 + 4] );
                uint[] workingKey6;
                (workingKey6 = this._workingKey)[1] = workingKey6[1] ^ F3( this._workingKey[2], this._Tm[index4 + 5], this._Tr[index4 + 5] );
                uint[] workingKey7;
                (workingKey7 = this._workingKey)[0] = workingKey7[0] ^ F1( this._workingKey[1], this._Tm[index4 + 6], this._Tr[index4 + 6] );
                uint[] workingKey8;
                (workingKey8 = this._workingKey)[7] = workingKey8[7] ^ F2( this._workingKey[0], this._Tm[index4 + 7], this._Tr[index4 + 7] );
                int index5 = ((index3 * 2) + 1) * 8;
                uint[] workingKey9;
                (workingKey9 = this._workingKey)[6] = workingKey9[6] ^ F1( this._workingKey[7], this._Tm[index5], this._Tr[index5] );
                uint[] workingKey10;
                (workingKey10 = this._workingKey)[5] = workingKey10[5] ^ F2( this._workingKey[6], this._Tm[index5 + 1], this._Tr[index5 + 1] );
                uint[] workingKey11;
                (workingKey11 = this._workingKey)[4] = workingKey11[4] ^ F3( this._workingKey[5], this._Tm[index5 + 2], this._Tr[index5 + 2] );
                uint[] workingKey12;
                (workingKey12 = this._workingKey)[3] = workingKey12[3] ^ F1( this._workingKey[4], this._Tm[index5 + 3], this._Tr[index5 + 3] );
                uint[] workingKey13;
                (workingKey13 = this._workingKey)[2] = workingKey13[2] ^ F2( this._workingKey[3], this._Tm[index5 + 4], this._Tr[index5 + 4] );
                uint[] workingKey14;
                (workingKey14 = this._workingKey)[1] = workingKey14[1] ^ F3( this._workingKey[2], this._Tm[index5 + 5], this._Tr[index5 + 5] );
                uint[] workingKey15;
                (workingKey15 = this._workingKey)[0] = workingKey15[0] ^ F1( this._workingKey[1], this._Tm[index5 + 6], this._Tr[index5 + 6] );
                uint[] workingKey16;
                (workingKey16 = this._workingKey)[7] = workingKey16[7] ^ F2( this._workingKey[0], this._Tm[index5 + 7], this._Tr[index5 + 7] );
                this._Kr[index3 * 4] = (int)this._workingKey[0] & 31;
                this._Kr[(index3 * 4) + 1] = (int)this._workingKey[2] & 31;
                this._Kr[(index3 * 4) + 2] = (int)this._workingKey[4] & 31;
                this._Kr[(index3 * 4) + 3] = (int)this._workingKey[6] & 31;
                this._Km[index3 * 4] = this._workingKey[7];
                this._Km[(index3 * 4) + 1] = this._workingKey[5];
                this._Km[(index3 * 4) + 2] = this._workingKey[3];
                this._Km[(index3 * 4) + 3] = this._workingKey[1];
            }
        }

        internal override int EncryptBlock( byte[] src, int srcIndex, byte[] dst, int dstIndex )
        {
            uint uint32_1 = Pack.BE_To_UInt32( src, srcIndex );
            uint uint32_2 = Pack.BE_To_UInt32( src, srcIndex + 4 );
            uint uint32_3 = Pack.BE_To_UInt32( src, srcIndex + 8 );
            uint uint32_4 = Pack.BE_To_UInt32( src, srcIndex + 12 );
            uint[] result = new uint[4];
            this.CAST_Encipher( uint32_1, uint32_2, uint32_3, uint32_4, result );
            Pack.UInt32_To_BE( result[0], dst, dstIndex );
            Pack.UInt32_To_BE( result[1], dst, dstIndex + 4 );
            Pack.UInt32_To_BE( result[2], dst, dstIndex + 8 );
            Pack.UInt32_To_BE( result[3], dst, dstIndex + 12 );
            return 16;
        }

        internal override int DecryptBlock( byte[] src, int srcIndex, byte[] dst, int dstIndex )
        {
            uint uint32_1 = Pack.BE_To_UInt32( src, srcIndex );
            uint uint32_2 = Pack.BE_To_UInt32( src, srcIndex + 4 );
            uint uint32_3 = Pack.BE_To_UInt32( src, srcIndex + 8 );
            uint uint32_4 = Pack.BE_To_UInt32( src, srcIndex + 12 );
            uint[] result = new uint[4];
            this.CAST_Decipher( uint32_1, uint32_2, uint32_3, uint32_4, result );
            Pack.UInt32_To_BE( result[0], dst, dstIndex );
            Pack.UInt32_To_BE( result[1], dst, dstIndex + 4 );
            Pack.UInt32_To_BE( result[2], dst, dstIndex + 8 );
            Pack.UInt32_To_BE( result[3], dst, dstIndex + 12 );
            return 16;
        }

        private void CAST_Encipher( uint A, uint B, uint C, uint D, uint[] result )
        {
            for (int index1 = 0; index1 < 6; ++index1)
            {
                int index2 = index1 * 4;
                C ^= F1( D, this._Km[index2], this._Kr[index2] );
                B ^= F2( C, this._Km[index2 + 1], this._Kr[index2 + 1] );
                A ^= F3( B, this._Km[index2 + 2], this._Kr[index2 + 2] );
                D ^= F1( A, this._Km[index2 + 3], this._Kr[index2 + 3] );
            }
            for (int index3 = 6; index3 < 12; ++index3)
            {
                int index4 = index3 * 4;
                D ^= F1( A, this._Km[index4 + 3], this._Kr[index4 + 3] );
                A ^= F3( B, this._Km[index4 + 2], this._Kr[index4 + 2] );
                B ^= F2( C, this._Km[index4 + 1], this._Kr[index4 + 1] );
                C ^= F1( D, this._Km[index4], this._Kr[index4] );
            }
            result[0] = A;
            result[1] = B;
            result[2] = C;
            result[3] = D;
        }

        private void CAST_Decipher( uint A, uint B, uint C, uint D, uint[] result )
        {
            for (int index1 = 0; index1 < 6; ++index1)
            {
                int index2 = (11 - index1) * 4;
                C ^= F1( D, this._Km[index2], this._Kr[index2] );
                B ^= F2( C, this._Km[index2 + 1], this._Kr[index2 + 1] );
                A ^= F3( B, this._Km[index2 + 2], this._Kr[index2 + 2] );
                D ^= F1( A, this._Km[index2 + 3], this._Kr[index2 + 3] );
            }
            for (int index3 = 6; index3 < 12; ++index3)
            {
                int index4 = (11 - index3) * 4;
                D ^= F1( A, this._Km[index4 + 3], this._Kr[index4 + 3] );
                A ^= F3( B, this._Km[index4 + 2], this._Kr[index4 + 2] );
                B ^= F2( C, this._Km[index4 + 1], this._Kr[index4 + 1] );
                C ^= F1( D, this._Km[index4], this._Kr[index4] );
            }
            result[0] = A;
            result[1] = B;
            result[2] = C;
            result[3] = D;
        }
    }
}
