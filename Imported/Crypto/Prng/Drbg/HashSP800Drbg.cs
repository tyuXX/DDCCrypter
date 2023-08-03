// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.Drbg.HashSP800Drbg
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Prng.Drbg
{
    public class HashSP800Drbg : ISP80090Drbg
    {
        private static readonly byte[] ONE = new byte[1]
        {
       1
        };
        private static readonly long RESEED_MAX = 140737488355328;
        private static readonly int MAX_BITS_REQUEST = 262144;
        private static readonly IDictionary seedlens = Platform.CreateHashtable();
        private readonly IDigest mDigest;
        private readonly IEntropySource mEntropySource;
        private readonly int mSecurityStrength;
        private readonly int mSeedLength;
        private byte[] mV;
        private byte[] mC;
        private long mReseedCounter;

        static HashSP800Drbg()
        {
            seedlens.Add( "SHA-1", 440 );
            seedlens.Add( "SHA-224", 440 );
            seedlens.Add( "SHA-256", 440 );
            seedlens.Add( "SHA-512/256", 440 );
            seedlens.Add( "SHA-512/224", 440 );
            seedlens.Add( "SHA-384", 888 );
            seedlens.Add( "SHA-512", 888 );
        }

        public HashSP800Drbg(
          IDigest digest,
          int securityStrength,
          IEntropySource entropySource,
          byte[] personalizationString,
          byte[] nonce )
        {
            if (securityStrength > DrbgUtilities.GetMaxSecurityStrength( digest ))
                throw new ArgumentException( "Requested security strength is not supported by the derivation function" );
            if (entropySource.EntropySize < securityStrength)
                throw new ArgumentException( "Not enough entropy for security strength required" );
            this.mDigest = digest;
            this.mEntropySource = entropySource;
            this.mSecurityStrength = securityStrength;
            this.mSeedLength = (int)seedlens[digest.AlgorithmName];
            this.mV = DrbgUtilities.HashDF( this.mDigest, Arrays.ConcatenateAll( this.GetEntropy(), nonce, personalizationString ), this.mSeedLength );
            byte[] numArray = new byte[this.mV.Length + 1];
            Array.Copy( mV, 0, numArray, 1, this.mV.Length );
            this.mC = DrbgUtilities.HashDF( this.mDigest, numArray, this.mSeedLength );
            this.mReseedCounter = 1L;
        }

        public int BlockSize => this.mDigest.GetDigestSize() * 8;

        public int Generate( byte[] output, byte[] additionalInput, bool predictionResistant )
        {
            int lengthInBits = output.Length * 8;
            if (lengthInBits > MAX_BITS_REQUEST)
                throw new ArgumentException( "Number of bits per request limited to " + MAX_BITS_REQUEST, nameof( output ) );
            if (this.mReseedCounter > RESEED_MAX)
                return -1;
            if (predictionResistant)
            {
                this.Reseed( additionalInput );
                additionalInput = null;
            }
            if (additionalInput != null)
            {
                byte[] numArray = new byte[1 + this.mV.Length + additionalInput.Length];
                numArray[0] = 2;
                Array.Copy( mV, 0, numArray, 1, this.mV.Length );
                Array.Copy( additionalInput, 0, numArray, 1 + this.mV.Length, additionalInput.Length );
                this.AddTo( this.mV, this.Hash( numArray ) );
            }
            byte[] sourceArray = this.hashgen( this.mV, lengthInBits );
            byte[] numArray1 = new byte[this.mV.Length + 1];
            Array.Copy( mV, 0, numArray1, 1, this.mV.Length );
            numArray1[0] = 3;
            this.AddTo( this.mV, this.Hash( numArray1 ) );
            this.AddTo( this.mV, this.mC );
            this.AddTo( this.mV, new byte[4]
            {
        (byte) (this.mReseedCounter >> 24),
        (byte) (this.mReseedCounter >> 16),
        (byte) (this.mReseedCounter >> 8),
        (byte) this.mReseedCounter
            } );
            ++this.mReseedCounter;
            Array.Copy( sourceArray, 0, output, 0, output.Length );
            return lengthInBits;
        }

        private byte[] GetEntropy()
        {
            byte[] entropy = this.mEntropySource.GetEntropy();
            if (entropy.Length < (this.mSecurityStrength + 7) / 8)
                throw new InvalidOperationException( "Insufficient entropy provided by entropy source" );
            return entropy;
        }

        private void AddTo( byte[] longer, byte[] shorter )
        {
            int num1 = longer.Length - shorter.Length;
            uint num2 = 0;
            int length = shorter.Length;
            while (--length >= 0)
            {
                uint num3 = num2 + longer[num1 + length] + shorter[length];
                longer[num1 + length] = (byte)num3;
                num2 = num3 >> 8;
            }
            int index = num1;
            while (--index >= 0)
            {
                uint num4 = num2 + longer[index];
                longer[index] = (byte)num4;
                num2 = num4 >> 8;
            }
        }

        public void Reseed( byte[] additionalInput )
        {
            byte[] entropy = this.GetEntropy();
            this.mV = DrbgUtilities.HashDF( this.mDigest, Arrays.ConcatenateAll( ONE, this.mV, entropy, additionalInput ), this.mSeedLength );
            byte[] numArray = new byte[this.mV.Length + 1];
            numArray[0] = 0;
            Array.Copy( mV, 0, numArray, 1, this.mV.Length );
            this.mC = DrbgUtilities.HashDF( this.mDigest, numArray, this.mSeedLength );
            this.mReseedCounter = 1L;
        }

        private byte[] Hash( byte[] input )
        {
            byte[] output = new byte[this.mDigest.GetDigestSize()];
            this.DoHash( input, output );
            return output;
        }

        private void DoHash( byte[] input, byte[] output )
        {
            this.mDigest.BlockUpdate( input, 0, input.Length );
            this.mDigest.DoFinal( output, 0 );
        }

        private byte[] hashgen( byte[] input, int lengthInBits )
        {
            int digestSize = this.mDigest.GetDigestSize();
            int num = lengthInBits / 8 / digestSize;
            byte[] numArray1 = new byte[input.Length];
            Array.Copy( input, 0, numArray1, 0, input.Length );
            byte[] destinationArray = new byte[lengthInBits / 8];
            byte[] numArray2 = new byte[this.mDigest.GetDigestSize()];
            for (int index = 0; index <= num; ++index)
            {
                this.DoHash( numArray1, numArray2 );
                int length = destinationArray.Length - (index * numArray2.Length) > numArray2.Length ? numArray2.Length : destinationArray.Length - (index * numArray2.Length);
                Array.Copy( numArray2, 0, destinationArray, index * numArray2.Length, length );
                this.AddTo( numArray1, ONE );
            }
            return destinationArray;
        }
    }
}
