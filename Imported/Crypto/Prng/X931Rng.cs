// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.X931Rng
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Prng
{
    internal class X931Rng
    {
        private const long BLOCK64_RESEED_MAX = 32768;
        private const long BLOCK128_RESEED_MAX = 8388608;
        private const int BLOCK64_MAX_BITS_REQUEST = 4096;
        private const int BLOCK128_MAX_BITS_REQUEST = 262144;
        private readonly IBlockCipher mEngine;
        private readonly IEntropySource mEntropySource;
        private readonly byte[] mDT;
        private readonly byte[] mI;
        private readonly byte[] mR;
        private byte[] mV;
        private long mReseedCounter = 1;

        internal X931Rng( IBlockCipher engine, byte[] dateTimeVector, IEntropySource entropySource )
        {
            this.mEngine = engine;
            this.mEntropySource = entropySource;
            this.mDT = new byte[engine.GetBlockSize()];
            Array.Copy( dateTimeVector, 0, mDT, 0, this.mDT.Length );
            this.mI = new byte[engine.GetBlockSize()];
            this.mR = new byte[engine.GetBlockSize()];
        }

        internal int Generate( byte[] output, bool predictionResistant )
        {
            if (this.mR.Length == 8)
            {
                if (this.mReseedCounter > 32768L)
                    return -1;
                if (IsTooLarge( output, 512 ))
                    throw new ArgumentException( "Number of bits per request limited to " + 4096, nameof( output ) );
            }
            else
            {
                if (this.mReseedCounter > 8388608L)
                    return -1;
                if (IsTooLarge( output, 32768 ))
                    throw new ArgumentException( "Number of bits per request limited to " + 262144, nameof( output ) );
            }
            if (predictionResistant || this.mV == null)
            {
                this.mV = this.mEntropySource.GetEntropy();
                if (this.mV.Length != this.mEngine.GetBlockSize())
                    throw new InvalidOperationException( "Insufficient entropy returned" );
            }
            int num = output.Length / this.mR.Length;
            for (int index = 0; index < num; ++index)
            {
                this.mEngine.ProcessBlock( this.mDT, 0, this.mI, 0 );
                this.Process( this.mR, this.mI, this.mV );
                this.Process( this.mV, this.mR, this.mI );
                Array.Copy( mR, 0, output, index * this.mR.Length, this.mR.Length );
                this.Increment( this.mDT );
            }
            int length = output.Length - (num * this.mR.Length);
            if (length > 0)
            {
                this.mEngine.ProcessBlock( this.mDT, 0, this.mI, 0 );
                this.Process( this.mR, this.mI, this.mV );
                this.Process( this.mV, this.mR, this.mI );
                Array.Copy( mR, 0, output, num * this.mR.Length, length );
                this.Increment( this.mDT );
            }
            ++this.mReseedCounter;
            return output.Length;
        }

        internal void Reseed()
        {
            this.mV = this.mEntropySource.GetEntropy();
            if (this.mV.Length != this.mEngine.GetBlockSize())
                throw new InvalidOperationException( "Insufficient entropy returned" );
            this.mReseedCounter = 1L;
        }

        internal IEntropySource EntropySource => this.mEntropySource;

        private void Process( byte[] res, byte[] a, byte[] b )
        {
            for (int index = 0; index != res.Length; ++index)
                res[index] = (byte)(a[index] ^ (uint)b[index]);
            this.mEngine.ProcessBlock( res, 0, res, 0 );
        }

        private void Increment( byte[] val )
        {
            int num = val.Length - 1;
            byte[] numArray;
            IntPtr index;
            while (num >= 0 && ((numArray = val)[(int)(index = (IntPtr)num)] = (byte)(numArray[(int)index] + 1U)) == 0)
                --num;
        }

        private static bool IsTooLarge( byte[] bytes, int maxBytes ) => bytes != null && bytes.Length > maxBytes;
    }
}
