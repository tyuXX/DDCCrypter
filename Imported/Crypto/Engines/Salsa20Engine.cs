// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.Salsa20Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class Salsa20Engine : IStreamCipher
    {
        private const int StateSize = 16;
        public static readonly int DEFAULT_ROUNDS = 20;
        protected static readonly byte[] sigma = Strings.ToAsciiByteArray( "expand 32-byte k" );
        protected static readonly byte[] tau = Strings.ToAsciiByteArray( "expand 16-byte k" );
        protected int rounds;
        private int index = 0;
        internal uint[] engineState = new uint[16];
        internal uint[] x = new uint[16];
        private byte[] keyStream = new byte[64];
        private bool initialised = false;
        private uint cW0;
        private uint cW1;
        private uint cW2;

        public Salsa20Engine()
          : this( DEFAULT_ROUNDS )
        {
        }

        public Salsa20Engine( int rounds ) => this.rounds = rounds > 0 && (rounds & 1) == 0 ? rounds : throw new ArgumentException( "'rounds' must be a positive, even number" );

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            byte[] ivBytes = parameters is ParametersWithIV parametersWithIv ? parametersWithIv.GetIV() : throw new ArgumentException( this.AlgorithmName + " Init requires an IV", nameof( parameters ) );
            if (ivBytes == null || ivBytes.Length != this.NonceSize)
                throw new ArgumentException( this.AlgorithmName + " requires exactly " + NonceSize + " bytes of IV" );
            if (!(parametersWithIv.Parameters is KeyParameter parameters1))
                throw new ArgumentException( this.AlgorithmName + " Init requires a key", nameof( parameters ) );
            this.SetKey( parameters1.GetKey(), ivBytes );
            this.Reset();
            this.initialised = true;
        }

        protected virtual int NonceSize => 8;

        public virtual string AlgorithmName
        {
            get
            {
                string algorithmName = "Salsa20";
                if (this.rounds != DEFAULT_ROUNDS)
                    algorithmName = algorithmName + "/" + rounds;
                return algorithmName;
            }
        }

        public virtual byte ReturnByte( byte input )
        {
            if (this.LimitExceeded())
                throw new MaxBytesExceededException( "2^70 byte limit per IV; Change IV" );
            if (this.index == 0)
            {
                this.GenerateKeyStream( this.keyStream );
                this.AdvanceCounter();
            }
            byte num = (byte)(this.keyStream[this.index] ^ (uint)input);
            this.index = (this.index + 1) & 63;
            return num;
        }

        protected virtual void AdvanceCounter()
        {
            uint[] engineState1;
            if (((engineState1 = this.engineState)[8] = engineState1[8] + 1U) != 0U)
                return;
            uint[] engineState2;
            (engineState2 = this.engineState)[9] = engineState2[9] + 1U;
        }

        public virtual void ProcessBytes(
          byte[] inBytes,
          int inOff,
          int len,
          byte[] outBytes,
          int outOff )
        {
            if (!this.initialised)
                throw new InvalidOperationException( this.AlgorithmName + " not initialised" );
            Check.DataLength( inBytes, inOff, len, "input buffer too short" );
            Check.OutputLength( outBytes, outOff, len, "output buffer too short" );
            if (this.LimitExceeded( (uint)len ))
                throw new MaxBytesExceededException( "2^70 byte limit per IV would be exceeded; Change IV" );
            for (int index = 0; index < len; ++index)
            {
                if (this.index == 0)
                {
                    this.GenerateKeyStream( this.keyStream );
                    this.AdvanceCounter();
                }
                outBytes[index + outOff] = (byte)(this.keyStream[this.index] ^ (uint)inBytes[index + inOff]);
                this.index = (this.index + 1) & 63;
            }
        }

        public virtual void Reset()
        {
            this.index = 0;
            this.ResetLimitCounter();
            this.ResetCounter();
        }

        protected virtual void ResetCounter() => this.engineState[8] = this.engineState[9] = 0U;

        protected virtual void SetKey( byte[] keyBytes, byte[] ivBytes )
        {
            if (keyBytes.Length != 16 && keyBytes.Length != 32)
                throw new ArgumentException( this.AlgorithmName + " requires 128 bit or 256 bit key" );
            int off = 0;
            this.engineState[1] = Pack.LE_To_UInt32( keyBytes, 0 );
            this.engineState[2] = Pack.LE_To_UInt32( keyBytes, 4 );
            this.engineState[3] = Pack.LE_To_UInt32( keyBytes, 8 );
            this.engineState[4] = Pack.LE_To_UInt32( keyBytes, 12 );
            byte[] bs;
            if (keyBytes.Length == 32)
            {
                bs = sigma;
                off = 16;
            }
            else
                bs = tau;
            this.engineState[11] = Pack.LE_To_UInt32( keyBytes, off );
            this.engineState[12] = Pack.LE_To_UInt32( keyBytes, off + 4 );
            this.engineState[13] = Pack.LE_To_UInt32( keyBytes, off + 8 );
            this.engineState[14] = Pack.LE_To_UInt32( keyBytes, off + 12 );
            this.engineState[0] = Pack.LE_To_UInt32( bs, 0 );
            this.engineState[5] = Pack.LE_To_UInt32( bs, 4 );
            this.engineState[10] = Pack.LE_To_UInt32( bs, 8 );
            this.engineState[15] = Pack.LE_To_UInt32( bs, 12 );
            this.engineState[6] = Pack.LE_To_UInt32( ivBytes, 0 );
            this.engineState[7] = Pack.LE_To_UInt32( ivBytes, 4 );
            this.ResetCounter();
        }

        protected virtual void GenerateKeyStream( byte[] output )
        {
            SalsaCore( this.rounds, this.engineState, this.x );
            Pack.UInt32_To_LE( this.x, output, 0 );
        }

        internal static void SalsaCore( int rounds, uint[] input, uint[] x )
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
                uint num17 = num5 ^ R( num1 + num13, 7 );
                uint num18 = num9 ^ R( num17 + num1, 9 );
                uint num19 = num13 ^ R( num18 + num17, 13 );
                uint num20 = num1 ^ R( num19 + num18, 18 );
                uint num21 = num10 ^ R( num6 + num2, 7 );
                uint num22 = num14 ^ R( num21 + num6, 9 );
                uint num23 = num2 ^ R( num22 + num21, 13 );
                uint num24 = num6 ^ R( num23 + num22, 18 );
                uint num25 = num15 ^ R( num11 + num7, 7 );
                uint num26 = num3 ^ R( num25 + num11, 9 );
                uint num27 = num7 ^ R( num26 + num25, 13 );
                uint num28 = num11 ^ R( num27 + num26, 18 );
                uint num29 = num4 ^ R( num16 + num12, 7 );
                uint num30 = num8 ^ R( num29 + num16, 9 );
                uint num31 = num12 ^ R( num30 + num29, 13 );
                uint num32 = num16 ^ R( num31 + num30, 18 );
                num2 = num23 ^ R( num20 + num29, 7 );
                num3 = num26 ^ R( num2 + num20, 9 );
                num4 = num29 ^ R( num3 + num2, 13 );
                num1 = num20 ^ R( num4 + num3, 18 );
                num7 = num27 ^ R( num24 + num17, 7 );
                num8 = num30 ^ R( num7 + num24, 9 );
                num5 = num17 ^ R( num8 + num7, 13 );
                num6 = num24 ^ R( num5 + num8, 18 );
                num12 = num31 ^ R( num28 + num21, 7 );
                num9 = num18 ^ R( num12 + num28, 9 );
                num10 = num21 ^ R( num9 + num12, 13 );
                num11 = num28 ^ R( num10 + num9, 18 );
                num13 = num19 ^ R( num32 + num25, 7 );
                num14 = num22 ^ R( num13 + num32, 9 );
                num15 = num25 ^ R( num14 + num13, 13 );
                num16 = num32 ^ R( num15 + num14, 18 );
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

        internal static uint R( uint x, int y ) => (x << y) | (x >> (32 - y));

        private void ResetLimitCounter()
        {
            this.cW0 = 0U;
            this.cW1 = 0U;
            this.cW2 = 0U;
        }

        private bool LimitExceeded() => ++this.cW0 == 0U && ++this.cW1 == 0U && ((int)++this.cW2 & 32) != 0;

        private bool LimitExceeded( uint len )
        {
            uint cW0 = this.cW0;
            this.cW0 += len;
            return this.cW0 < cW0 && ++this.cW1 == 0U && ((int)++this.cW2 & 32) != 0;
        }
    }
}
