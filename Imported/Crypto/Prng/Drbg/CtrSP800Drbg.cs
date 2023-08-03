// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.Drbg.CtrSP800Drbg
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Crypto.Prng.Drbg
{
    public class CtrSP800Drbg : ISP80090Drbg
    {
        private static readonly long TDEA_RESEED_MAX = 2147483648;
        private static readonly long AES_RESEED_MAX = 140737488355328;
        private static readonly int TDEA_MAX_BITS_REQUEST = 4096;
        private static readonly int AES_MAX_BITS_REQUEST = 262144;
        private readonly IEntropySource mEntropySource;
        private readonly IBlockCipher mEngine;
        private readonly int mKeySizeInBits;
        private readonly int mSeedLength;
        private readonly int mSecurityStrength;
        private byte[] mKey;
        private byte[] mV;
        private long mReseedCounter = 0;
        private bool mIsTdea = false;
        private static readonly byte[] K_BITS = Hex.Decode( "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F" );

        public CtrSP800Drbg(
          IBlockCipher engine,
          int keySizeInBits,
          int securityStrength,
          IEntropySource entropySource,
          byte[] personalizationString,
          byte[] nonce )
        {
            if (securityStrength > 256)
                throw new ArgumentException( "Requested security strength is not supported by the derivation function" );
            if (this.GetMaxSecurityStrength( engine, keySizeInBits ) < securityStrength)
                throw new ArgumentException( "Requested security strength is not supported by block cipher and key size" );
            if (entropySource.EntropySize < securityStrength)
                throw new ArgumentException( "Not enough entropy for security strength required" );
            this.mEntropySource = entropySource;
            this.mEngine = engine;
            this.mKeySizeInBits = keySizeInBits;
            this.mSecurityStrength = securityStrength;
            this.mSeedLength = keySizeInBits + (engine.GetBlockSize() * 8);
            this.mIsTdea = this.IsTdea( engine );
            this.CTR_DRBG_Instantiate_algorithm( this.GetEntropy(), nonce, personalizationString );
        }

        private void CTR_DRBG_Instantiate_algorithm(
          byte[] entropy,
          byte[] nonce,
          byte[] personalisationString )
        {
            byte[] seed = this.Block_Cipher_df( Arrays.ConcatenateAll( entropy, nonce, personalisationString ), this.mSeedLength );
            int blockSize = this.mEngine.GetBlockSize();
            this.mKey = new byte[(this.mKeySizeInBits + 7) / 8];
            this.mV = new byte[blockSize];
            this.CTR_DRBG_Update( seed, this.mKey, this.mV );
            this.mReseedCounter = 1L;
        }

        private void CTR_DRBG_Update( byte[] seed, byte[] key, byte[] v )
        {
            byte[] numArray1 = new byte[seed.Length];
            byte[] numArray2 = new byte[this.mEngine.GetBlockSize()];
            int num = 0;
            int blockSize = this.mEngine.GetBlockSize();
            this.mEngine.Init( true, new KeyParameter( this.ExpandKey( key ) ) );
            for (; num * blockSize < seed.Length; ++num)
            {
                this.AddOneTo( v );
                this.mEngine.ProcessBlock( v, 0, numArray2, 0 );
                int length = numArray1.Length - (num * blockSize) > blockSize ? blockSize : numArray1.Length - (num * blockSize);
                Array.Copy( numArray2, 0, numArray1, num * blockSize, length );
            }
            this.XOR( numArray1, seed, numArray1, 0 );
            Array.Copy( numArray1, 0, key, 0, key.Length );
            Array.Copy( numArray1, key.Length, v, 0, v.Length );
        }

        private void CTR_DRBG_Reseed_algorithm( byte[] additionalInput )
        {
            this.CTR_DRBG_Update( this.Block_Cipher_df( Arrays.Concatenate( this.GetEntropy(), additionalInput ), this.mSeedLength ), this.mKey, this.mV );
            this.mReseedCounter = 1L;
        }

        private void XOR( byte[] output, byte[] a, byte[] b, int bOff )
        {
            for (int index = 0; index < output.Length; ++index)
                output[index] = (byte)(a[index] ^ (uint)b[bOff + index]);
        }

        private void AddOneTo( byte[] longer )
        {
            uint num1 = 1;
            int length = longer.Length;
            while (--length >= 0)
            {
                uint num2 = num1 + longer[length];
                longer[length] = (byte)num2;
                num1 = num2 >> 8;
            }
        }

        private byte[] GetEntropy()
        {
            byte[] entropy = this.mEntropySource.GetEntropy();
            if (entropy.Length < (this.mSecurityStrength + 7) / 8)
                throw new InvalidOperationException( "Insufficient entropy provided by entropy source" );
            return entropy;
        }

        private byte[] Block_Cipher_df( byte[] inputString, int bitLength )
        {
            int blockSize = this.mEngine.GetBlockSize();
            int length1 = inputString.Length;
            int num1 = bitLength / 8;
            byte[] numArray1 = new byte[(8 + length1 + 1 + blockSize - 1) / blockSize * blockSize];
            this.copyIntToByteArray( numArray1, length1, 0 );
            this.copyIntToByteArray( numArray1, num1, 4 );
            Array.Copy( inputString, 0, numArray1, 8, length1 );
            numArray1[8 + length1] = 128;
            byte[] numArray2 = new byte[(this.mKeySizeInBits / 8) + blockSize];
            byte[] numArray3 = new byte[blockSize];
            byte[] numArray4 = new byte[blockSize];
            int num2 = 0;
            byte[] numArray5 = new byte[this.mKeySizeInBits / 8];
            Array.Copy( K_BITS, 0, numArray5, 0, numArray5.Length );
            for (; num2 * blockSize * 8 < this.mKeySizeInBits + (blockSize * 8); ++num2)
            {
                this.copyIntToByteArray( numArray4, num2, 0 );
                this.BCC( numArray3, numArray5, numArray4, numArray1 );
                int length2 = numArray2.Length - (num2 * blockSize) > blockSize ? blockSize : numArray2.Length - (num2 * blockSize);
                Array.Copy( numArray3, 0, numArray2, num2 * blockSize, length2 );
            }
            byte[] numArray6 = new byte[blockSize];
            Array.Copy( numArray2, 0, numArray5, 0, numArray5.Length );
            Array.Copy( numArray2, numArray5.Length, numArray6, 0, numArray6.Length );
            byte[] destinationArray = new byte[bitLength / 2];
            int num3 = 0;
            this.mEngine.Init( true, new KeyParameter( this.ExpandKey( numArray5 ) ) );
            for (; num3 * blockSize < destinationArray.Length; ++num3)
            {
                this.mEngine.ProcessBlock( numArray6, 0, numArray6, 0 );
                int length3 = destinationArray.Length - (num3 * blockSize) > blockSize ? blockSize : destinationArray.Length - (num3 * blockSize);
                Array.Copy( numArray6, 0, destinationArray, num3 * blockSize, length3 );
            }
            return destinationArray;
        }

        private void BCC( byte[] bccOut, byte[] k, byte[] iV, byte[] data )
        {
            int blockSize = this.mEngine.GetBlockSize();
            byte[] numArray1 = new byte[blockSize];
            int num = data.Length / blockSize;
            byte[] numArray2 = new byte[blockSize];
            this.mEngine.Init( true, new KeyParameter( this.ExpandKey( k ) ) );
            this.mEngine.ProcessBlock( iV, 0, numArray1, 0 );
            for (int index = 0; index < num; ++index)
            {
                this.XOR( numArray2, numArray1, data, index * blockSize );
                this.mEngine.ProcessBlock( numArray2, 0, numArray1, 0 );
            }
            Array.Copy( numArray1, 0, bccOut, 0, bccOut.Length );
        }

        private void copyIntToByteArray( byte[] buf, int value, int offSet )
        {
            buf[offSet] = (byte)(value >> 24);
            buf[offSet + 1] = (byte)(value >> 16);
            buf[offSet + 2] = (byte)(value >> 8);
            buf[offSet + 3] = (byte)value;
        }

        public int BlockSize => this.mV.Length * 8;

        public int Generate( byte[] output, byte[] additionalInput, bool predictionResistant )
        {
            if (this.mIsTdea)
            {
                if (this.mReseedCounter > TDEA_RESEED_MAX)
                    return -1;
                if (DrbgUtilities.IsTooLarge( output, TDEA_MAX_BITS_REQUEST / 8 ))
                    throw new ArgumentException( "Number of bits per request limited to " + TDEA_MAX_BITS_REQUEST, nameof( output ) );
            }
            else
            {
                if (this.mReseedCounter > AES_RESEED_MAX)
                    return -1;
                if (DrbgUtilities.IsTooLarge( output, AES_MAX_BITS_REQUEST / 8 ))
                    throw new ArgumentException( "Number of bits per request limited to " + AES_MAX_BITS_REQUEST, nameof( output ) );
            }
            if (predictionResistant)
            {
                this.CTR_DRBG_Reseed_algorithm( additionalInput );
                additionalInput = null;
            }
            if (additionalInput != null)
            {
                additionalInput = this.Block_Cipher_df( additionalInput, this.mSeedLength );
                this.CTR_DRBG_Update( additionalInput, this.mKey, this.mV );
            }
            else
                additionalInput = new byte[this.mSeedLength];
            byte[] numArray = new byte[this.mV.Length];
            this.mEngine.Init( true, new KeyParameter( this.ExpandKey( this.mKey ) ) );
            for (int index = 0; index <= output.Length / numArray.Length; ++index)
            {
                int length = output.Length - (index * numArray.Length) > numArray.Length ? numArray.Length : output.Length - (index * this.mV.Length);
                if (length != 0)
                {
                    this.AddOneTo( this.mV );
                    this.mEngine.ProcessBlock( this.mV, 0, numArray, 0 );
                    Array.Copy( numArray, 0, output, index * numArray.Length, length );
                }
            }
            this.CTR_DRBG_Update( additionalInput, this.mKey, this.mV );
            ++this.mReseedCounter;
            return output.Length * 8;
        }

        public void Reseed( byte[] additionalInput ) => this.CTR_DRBG_Reseed_algorithm( additionalInput );

        private bool IsTdea( IBlockCipher cipher ) => cipher.AlgorithmName.Equals( "DESede" ) || cipher.AlgorithmName.Equals( "TDEA" );

        private int GetMaxSecurityStrength( IBlockCipher cipher, int keySizeInBits )
        {
            if (this.IsTdea( cipher ) && keySizeInBits == 168)
                return 112;
            return cipher.AlgorithmName.Equals( "AES" ) ? keySizeInBits : -1;
        }

        private byte[] ExpandKey( byte[] key )
        {
            if (!this.mIsTdea)
                return key;
            byte[] tmp = new byte[24];
            this.PadKey( key, 0, tmp, 0 );
            this.PadKey( key, 7, tmp, 8 );
            this.PadKey( key, 14, tmp, 16 );
            return tmp;
        }

        private void PadKey( byte[] keyMaster, int keyOff, byte[] tmp, int tmpOff )
        {
            tmp[tmpOff] = (byte)(keyMaster[keyOff] & 254U);
            tmp[tmpOff + 1] = (byte)((keyMaster[keyOff] << 7) | ((keyMaster[keyOff + 1] & 252) >> 1));
            tmp[tmpOff + 2] = (byte)((keyMaster[keyOff + 1] << 6) | ((keyMaster[keyOff + 2] & 248) >> 2));
            tmp[tmpOff + 3] = (byte)((keyMaster[keyOff + 2] << 5) | ((keyMaster[keyOff + 3] & 240) >> 3));
            tmp[tmpOff + 4] = (byte)((keyMaster[keyOff + 3] << 4) | ((keyMaster[keyOff + 4] & 224) >> 4));
            tmp[tmpOff + 5] = (byte)((keyMaster[keyOff + 4] << 3) | ((keyMaster[keyOff + 5] & 192) >> 5));
            tmp[tmpOff + 6] = (byte)((keyMaster[keyOff + 5] << 2) | ((keyMaster[keyOff + 6] & 128) >> 6));
            tmp[tmpOff + 7] = (byte)((uint)keyMaster[keyOff + 6] << 1);
            DesParameters.SetOddParity( tmp, tmpOff, 8 );
        }
    }
}
