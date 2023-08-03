// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.KeccakDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class KeccakDigest : IDigest, IMemoable
    {
        private static readonly ulong[] KeccakRoundConstants = KeccakInitializeRoundConstants();
        private static readonly int[] KeccakRhoOffsets = KeccakInitializeRhoOffsets();
        protected byte[] state = new byte[200];
        protected byte[] dataQueue = new byte[192];
        protected int rate;
        protected int bitsInQueue;
        protected int fixedOutputLength;
        protected bool squeezing;
        protected int bitsAvailableForSqueezing;
        protected byte[] chunk;
        protected byte[] oneByte;
        private ulong[] C = new ulong[5];
        private ulong[] tempA = new ulong[25];
        private ulong[] chiC = new ulong[5];

        private static ulong[] KeccakInitializeRoundConstants()
        {
            ulong[] numArray = new ulong[24];
            byte num1 = 1;
            for (int index1 = 0; index1 < 24; ++index1)
            {
                numArray[index1] = 0UL;
                for (int index2 = 0; index2 < 7; ++index2)
                {
                    int num2 = (1 << index2) - 1;
                    if ((num1 & 1) != 0)
                        numArray[index1] ^= (ulong)(1L << num2);
                    bool flag = (num1 & 128) != 0;
                    num1 <<= 1;
                    if (flag)
                        num1 ^= 113;
                }
            }
            return numArray;
        }

        private static int[] KeccakInitializeRhoOffsets()
        {
            int[] numArray = new int[25];
            int num1 = 0;
            numArray[0] = num1;
            int num2 = 1;
            int num3 = 0;
            for (int index = 1; index < 25; ++index)
            {
                num1 = (num1 + index) & 63;
                numArray[(num2 % 5) + (5 * (num3 % 5))] = num1;
                int num4 = num3 % 5;
                int num5 = ((2 * num2) + (3 * num3)) % 5;
                num2 = num4;
                num3 = num5;
            }
            return numArray;
        }

        private void ClearDataQueueSection( int off, int len )
        {
            for (int index = off; index != off + len; ++index)
                this.dataQueue[index] = 0;
        }

        public KeccakDigest()
          : this( 288 )
        {
        }

        public KeccakDigest( int bitLength ) => this.Init( bitLength );

        public KeccakDigest( KeccakDigest source ) => this.CopyIn( source );

        private void CopyIn( KeccakDigest source )
        {
            Array.Copy( source.state, 0, state, 0, source.state.Length );
            Array.Copy( source.dataQueue, 0, dataQueue, 0, source.dataQueue.Length );
            this.rate = source.rate;
            this.bitsInQueue = source.bitsInQueue;
            this.fixedOutputLength = source.fixedOutputLength;
            this.squeezing = source.squeezing;
            this.bitsAvailableForSqueezing = source.bitsAvailableForSqueezing;
            this.chunk = Arrays.Clone( source.chunk );
            this.oneByte = Arrays.Clone( source.oneByte );
        }

        public virtual string AlgorithmName => "Keccak-" + fixedOutputLength;

        public virtual int GetDigestSize() => this.fixedOutputLength / 8;

        public virtual void Update( byte input )
        {
            this.oneByte[0] = input;
            this.Absorb( this.oneByte, 0, 8L );
        }

        public virtual void BlockUpdate( byte[] input, int inOff, int len ) => this.Absorb( input, inOff, len * 8L );

        public virtual int DoFinal( byte[] output, int outOff )
        {
            this.Squeeze( output, outOff, fixedOutputLength );
            this.Reset();
            return this.GetDigestSize();
        }

        protected virtual int DoFinal( byte[] output, int outOff, byte partialByte, int partialBits )
        {
            if (partialBits > 0)
            {
                this.oneByte[0] = partialByte;
                this.Absorb( this.oneByte, 0, partialBits );
            }
            this.Squeeze( output, outOff, fixedOutputLength );
            this.Reset();
            return this.GetDigestSize();
        }

        public virtual void Reset() => this.Init( this.fixedOutputLength );

        public virtual int GetByteLength() => this.rate / 8;

        private void Init( int bitLength )
        {
            switch (bitLength)
            {
                case 128:
                    this.InitSponge( 1344, 256 );
                    break;
                case 224:
                    this.InitSponge( 1152, 448 );
                    break;
                case 256:
                    this.InitSponge( 1088, 512 );
                    break;
                case 288:
                    this.InitSponge( 1024, 576 );
                    break;
                case 384:
                    this.InitSponge( 832, 768 );
                    break;
                case 512:
                    this.InitSponge( 576, 1024 );
                    break;
                default:
                    throw new ArgumentException( "must be one of 128, 224, 256, 288, 384, or 512.", nameof( bitLength ) );
            }
        }

        private void InitSponge( int rate, int capacity )
        {
            if (rate + capacity != 1600)
                throw new InvalidOperationException( "rate + capacity != 1600" );
            this.rate = rate > 0 && rate < 1600 && rate % 64 == 0 ? rate : throw new InvalidOperationException( "invalid rate value" );
            this.fixedOutputLength = 0;
            Arrays.Fill( this.state, 0 );
            Arrays.Fill( this.dataQueue, 0 );
            this.bitsInQueue = 0;
            this.squeezing = false;
            this.bitsAvailableForSqueezing = 0;
            this.fixedOutputLength = capacity / 2;
            this.chunk = new byte[rate / 8];
            this.oneByte = new byte[1];
        }

        private void AbsorbQueue()
        {
            this.KeccakAbsorb( this.state, this.dataQueue, this.rate / 8 );
            this.bitsInQueue = 0;
        }

        protected virtual void Absorb( byte[] data, int off, long databitlen )
        {
            if (this.bitsInQueue % 8 != 0)
                throw new InvalidOperationException( "attempt to absorb with odd length queue." );
            if (this.squeezing)
                throw new InvalidOperationException( "attempt to absorb while squeezing." );
            long num1 = 0;
            while (num1 < databitlen)
            {
                if (this.bitsInQueue == 0 && databitlen >= rate && num1 <= databitlen - rate)
                {
                    long num2 = (databitlen - num1) / rate;
                    for (long index = 0; index < num2; ++index)
                    {
                        Array.Copy( data, (int)(off + (num1 / 8L) + (index * chunk.Length)), chunk, 0, this.chunk.Length );
                        this.KeccakAbsorb( this.state, this.chunk, this.chunk.Length );
                    }
                    num1 += num2 * rate;
                }
                else
                {
                    int num3 = (int)(databitlen - num1);
                    if (num3 + this.bitsInQueue > this.rate)
                        num3 = this.rate - this.bitsInQueue;
                    int num4 = num3 % 8;
                    int num5 = num3 - num4;
                    Array.Copy( data, off + (int)(num1 / 8L), dataQueue, this.bitsInQueue / 8, num5 / 8 );
                    this.bitsInQueue += num5;
                    num1 += num5;
                    if (this.bitsInQueue == this.rate)
                        this.AbsorbQueue();
                    if (num4 > 0)
                    {
                        int num6 = (1 << num4) - 1;
                        this.dataQueue[this.bitsInQueue / 8] = (byte)(data[off + (int)(num1 / 8L)] & (uint)num6);
                        this.bitsInQueue += num4;
                        num1 += num4;
                    }
                }
            }
        }

        private void PadAndSwitchToSqueezingPhase()
        {
            if (this.bitsInQueue + 1 == this.rate)
            {
                byte[] dataQueue;
                IntPtr index;
                (dataQueue = this.dataQueue)[(int)(index = (IntPtr)(this.bitsInQueue / 8))] = (byte)(dataQueue[(int)index] | (uint)(byte)(1 << (this.bitsInQueue % 8)));
                this.AbsorbQueue();
                this.ClearDataQueueSection( 0, this.rate / 8 );
            }
            else
            {
                this.ClearDataQueueSection( (this.bitsInQueue + 7) / 8, (this.rate / 8) - ((this.bitsInQueue + 7) / 8) );
                byte[] dataQueue;
                IntPtr index;
                (dataQueue = this.dataQueue)[(int)(index = (IntPtr)(this.bitsInQueue / 8))] = (byte)(dataQueue[(int)index] | (uint)(byte)(1 << (this.bitsInQueue % 8)));
            }
            byte[] dataQueue1;
            IntPtr index1;
            (dataQueue1 = this.dataQueue)[(int)(index1 = (IntPtr)((this.rate - 1) / 8))] = (byte)(dataQueue1[(int)index1] | (uint)(byte)(1 << ((this.rate - 1) % 8)));
            this.AbsorbQueue();
            if (this.rate == 1024)
            {
                this.KeccakExtract1024bits( this.state, this.dataQueue );
                this.bitsAvailableForSqueezing = 1024;
            }
            else
            {
                this.KeccakExtract( this.state, this.dataQueue, this.rate / 64 );
                this.bitsAvailableForSqueezing = this.rate;
            }
            this.squeezing = true;
        }

        protected virtual void Squeeze( byte[] output, int offset, long outputLength )
        {
            if (!this.squeezing)
                this.PadAndSwitchToSqueezingPhase();
            if (outputLength % 8L != 0L)
                throw new InvalidOperationException( "outputLength not a multiple of 8" );
            int num;
            for (long index = 0; index < outputLength; index += num)
            {
                if (this.bitsAvailableForSqueezing == 0)
                {
                    this.KeccakPermutation( this.state );
                    if (this.rate == 1024)
                    {
                        this.KeccakExtract1024bits( this.state, this.dataQueue );
                        this.bitsAvailableForSqueezing = 1024;
                    }
                    else
                    {
                        this.KeccakExtract( this.state, this.dataQueue, this.rate / 64 );
                        this.bitsAvailableForSqueezing = this.rate;
                    }
                }
                num = this.bitsAvailableForSqueezing;
                if (num > outputLength - index)
                    num = (int)(outputLength - index);
                Array.Copy( dataQueue, (this.rate - this.bitsAvailableForSqueezing) / 8, output, offset + (int)(index / 8L), num / 8 );
                this.bitsAvailableForSqueezing -= num;
            }
        }

        private static void FromBytesToWords( ulong[] stateAsWords, byte[] state )
        {
            for (int index1 = 0; index1 < 25; ++index1)
            {
                stateAsWords[index1] = 0UL;
                int num = index1 * 8;
                for (int index2 = 0; index2 < 8; ++index2)
                    stateAsWords[index1] |= (ulong)((state[num + index2] & (long)byte.MaxValue) << (8 * index2));
            }
        }

        private static void FromWordsToBytes( byte[] state, ulong[] stateAsWords )
        {
            for (int index1 = 0; index1 < 25; ++index1)
            {
                int num = index1 * 8;
                for (int index2 = 0; index2 < 8; ++index2)
                    state[num + index2] = (byte)(stateAsWords[index1] >> (8 * index2));
            }
        }

        private void KeccakPermutation( byte[] state )
        {
            ulong[] numArray = new ulong[state.Length / 8];
            FromBytesToWords( numArray, state );
            this.KeccakPermutationOnWords( numArray );
            FromWordsToBytes( state, numArray );
        }

        private void KeccakPermutationAfterXor( byte[] state, byte[] data, int dataLengthInBytes )
        {
            for (int index1 = 0; index1 < dataLengthInBytes; ++index1)
            {
                byte[] numArray;
                IntPtr index2;
                (numArray = state)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray[(int)index2] ^ (uint)data[index1]);
            }
            this.KeccakPermutation( state );
        }

        private void KeccakPermutationOnWords( ulong[] state )
        {
            for (int indexRound = 0; indexRound < 24; ++indexRound)
            {
                this.Theta( state );
                this.Rho( state );
                this.Pi( state );
                this.Chi( state );
                Iota( state, indexRound );
            }
        }

        private void Theta( ulong[] A )
        {
            for (int index1 = 0; index1 < 5; ++index1)
            {
                this.C[index1] = 0UL;
                for (int index2 = 0; index2 < 5; ++index2)
                    this.C[index1] ^= A[index1 + (5 * index2)];
            }
            for (int index3 = 0; index3 < 5; ++index3)
            {
                ulong num = (this.C[(index3 + 1) % 5] << 1) ^ (this.C[(index3 + 1) % 5] >> 63) ^ this.C[(index3 + 4) % 5];
                for (int index4 = 0; index4 < 5; ++index4)
                    A[index3 + (5 * index4)] ^= num;
            }
        }

        private void Rho( ulong[] A )
        {
            for (int index1 = 0; index1 < 5; ++index1)
            {
                for (int index2 = 0; index2 < 5; ++index2)
                {
                    int index3 = index1 + (5 * index2);
                    A[index3] = KeccakRhoOffsets[index3] != 0 ? (A[index3] << KeccakRhoOffsets[index3]) ^ (A[index3] >> (64 - KeccakRhoOffsets[index3])) : A[index3];
                }
            }
        }

        private void Pi( ulong[] A )
        {
            Array.Copy( A, 0, tempA, 0, this.tempA.Length );
            for (int index1 = 0; index1 < 5; ++index1)
            {
                for (int index2 = 0; index2 < 5; ++index2)
                    A[index2 + (5 * (((2 * index1) + (3 * index2)) % 5))] = this.tempA[index1 + (5 * index2)];
            }
        }

        private void Chi( ulong[] A )
        {
            for (int index1 = 0; index1 < 5; ++index1)
            {
                for (int index2 = 0; index2 < 5; ++index2)
                    this.chiC[index2] = A[index2 + (5 * index1)] ^ (~A[((index2 + 1) % 5) + (5 * index1)] & A[((index2 + 2) % 5) + (5 * index1)]);
                for (int index3 = 0; index3 < 5; ++index3)
                    A[index3 + (5 * index1)] = this.chiC[index3];
            }
        }

        private static void Iota( ulong[] A, int indexRound )
        {
            ulong[] numArray;
            (numArray = A)[0] = numArray[0] ^ KeccakRoundConstants[indexRound];
        }

        private void KeccakAbsorb( byte[] byteState, byte[] data, int dataInBytes ) => this.KeccakPermutationAfterXor( byteState, data, dataInBytes );

        private void KeccakExtract1024bits( byte[] byteState, byte[] data ) => Array.Copy( byteState, 0, data, 0, 128 );

        private void KeccakExtract( byte[] byteState, byte[] data, int laneCount ) => Array.Copy( byteState, 0, data, 0, laneCount * 8 );

        public virtual IMemoable Copy() => new KeccakDigest( this );

        public virtual void Reset( IMemoable other ) => this.CopyIn( (KeccakDigest)other );
    }
}
