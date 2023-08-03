// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.ChaChaEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class ChaChaEngine : Salsa20Engine
    {
        public ChaChaEngine()
        {
        }

        public ChaChaEngine( int rounds )
          : base( rounds )
        {
        }

        public override string AlgorithmName => "ChaCha" + rounds;

        protected override void AdvanceCounter()
        {
            uint[] engineState1;
            if (((engineState1 = this.engineState)[12] = engineState1[12] + 1U) != 0U)
                return;
            uint[] engineState2;
            (engineState2 = this.engineState)[13] = engineState2[13] + 1U;
        }

        protected override void ResetCounter() => this.engineState[12] = this.engineState[13] = 0U;

        protected override void SetKey( byte[] keyBytes, byte[] ivBytes )
        {
            if (keyBytes.Length != 16 && keyBytes.Length != 32)
                throw new ArgumentException( this.AlgorithmName + " requires 128 bit or 256 bit key" );
            int off = 0;
            this.engineState[4] = Pack.LE_To_UInt32( keyBytes, 0 );
            this.engineState[5] = Pack.LE_To_UInt32( keyBytes, 4 );
            this.engineState[6] = Pack.LE_To_UInt32( keyBytes, 8 );
            this.engineState[7] = Pack.LE_To_UInt32( keyBytes, 12 );
            byte[] bs;
            if (keyBytes.Length == 32)
            {
                bs = sigma;
                off = 16;
            }
            else
                bs = tau;
            this.engineState[8] = Pack.LE_To_UInt32( keyBytes, off );
            this.engineState[9] = Pack.LE_To_UInt32( keyBytes, off + 4 );
            this.engineState[10] = Pack.LE_To_UInt32( keyBytes, off + 8 );
            this.engineState[11] = Pack.LE_To_UInt32( keyBytes, off + 12 );
            this.engineState[0] = Pack.LE_To_UInt32( bs, 0 );
            this.engineState[1] = Pack.LE_To_UInt32( bs, 4 );
            this.engineState[2] = Pack.LE_To_UInt32( bs, 8 );
            this.engineState[3] = Pack.LE_To_UInt32( bs, 12 );
            this.engineState[12] = this.engineState[13] = 0U;
            this.engineState[14] = Pack.LE_To_UInt32( ivBytes, 0 );
            this.engineState[15] = Pack.LE_To_UInt32( ivBytes, 4 );
        }

        protected override void GenerateKeyStream( byte[] output )
        {
            ChachaCore( this.rounds, this.engineState, this.x );
            Pack.UInt32_To_LE( this.x, output, 0 );
        }

        internal static void ChachaCore( int rounds, uint[] input, uint[] x )
        {
            if (input.Length != 16)
                throw new ArgumentException();
            if (x.Length != 16)
                throw new ArgumentException();
            if (rounds % 2 != 0)
                throw new ArgumentException( "Number of rounds must be even" );
            uint num1 = input[0];
            uint num2 = input[1];
            uint num3 = input[2];
            uint num4 = input[3];
            uint num5 = input[4];
            uint num6 = input[5];
            uint num7 = input[6];
            uint num8 = input[7];
            uint num9 = input[8];
            uint num10 = input[9];
            uint num11 = input[10];
            uint num12 = input[11];
            uint num13 = input[12];
            uint num14 = input[13];
            uint num15 = input[14];
            uint num16 = input[15];
            for (int index = rounds; index > 0; index -= 2)
            {
                uint num17 = num1 + num5;
                uint num18 = R( num13 ^ num17, 16 );
                uint num19 = num9 + num18;
                uint num20 = R( num5 ^ num19, 12 );
                uint num21 = num17 + num20;
                uint num22 = R( num18 ^ num21, 8 );
                uint num23 = num19 + num22;
                uint num24 = R( num20 ^ num23, 7 );
                uint num25 = num2 + num6;
                uint num26 = R( num14 ^ num25, 16 );
                uint num27 = num10 + num26;
                uint num28 = R( num6 ^ num27, 12 );
                uint num29 = num25 + num28;
                uint num30 = R( num26 ^ num29, 8 );
                uint num31 = num27 + num30;
                uint num32 = R( num28 ^ num31, 7 );
                uint num33 = num3 + num7;
                uint num34 = R( num15 ^ num33, 16 );
                uint num35 = num11 + num34;
                uint num36 = R( num7 ^ num35, 12 );
                uint num37 = num33 + num36;
                uint num38 = R( num34 ^ num37, 8 );
                uint num39 = num35 + num38;
                uint num40 = R( num36 ^ num39, 7 );
                uint num41 = num4 + num8;
                uint num42 = R( num16 ^ num41, 16 );
                uint num43 = num12 + num42;
                uint num44 = R( num8 ^ num43, 12 );
                uint num45 = num41 + num44;
                uint num46 = R( num42 ^ num45, 8 );
                uint num47 = num43 + num46;
                uint num48 = R( num44 ^ num47, 7 );
                uint num49 = num21 + num32;
                uint num50 = R( num46 ^ num49, 16 );
                uint num51 = num39 + num50;
                uint num52 = R( num32 ^ num51, 12 );
                num1 = num49 + num52;
                num16 = R( num50 ^ num1, 8 );
                num11 = num51 + num16;
                num6 = R( num52 ^ num11, 7 );
                uint num53 = num29 + num40;
                uint num54 = R( num22 ^ num53, 16 );
                uint num55 = num47 + num54;
                uint num56 = R( num40 ^ num55, 12 );
                num2 = num53 + num56;
                num13 = R( num54 ^ num2, 8 );
                num12 = num55 + num13;
                num7 = R( num56 ^ num12, 7 );
                uint num57 = num37 + num48;
                uint num58 = R( num30 ^ num57, 16 );
                uint num59 = num23 + num58;
                uint num60 = R( num48 ^ num59, 12 );
                num3 = num57 + num60;
                num14 = R( num58 ^ num3, 8 );
                num9 = num59 + num14;
                num8 = R( num60 ^ num9, 7 );
                uint num61 = num45 + num24;
                uint num62 = R( num38 ^ num61, 16 );
                uint num63 = num31 + num62;
                uint num64 = R( num24 ^ num63, 12 );
                num4 = num61 + num64;
                num15 = R( num62 ^ num4, 8 );
                num10 = num63 + num15;
                num5 = R( num64 ^ num10, 7 );
            }
            x[0] = num1 + input[0];
            x[1] = num2 + input[1];
            x[2] = num3 + input[2];
            x[3] = num4 + input[3];
            x[4] = num5 + input[4];
            x[5] = num6 + input[5];
            x[6] = num7 + input[6];
            x[7] = num8 + input[7];
            x[8] = num9 + input[8];
            x[9] = num10 + input[9];
            x[10] = num11 + input[10];
            x[11] = num12 + input[11];
            x[12] = num13 + input[12];
            x[13] = num14 + input[13];
            x[14] = num15 + input[14];
            x[15] = num16 + input[15];
        }
    }
}
