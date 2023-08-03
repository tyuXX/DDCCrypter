// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.SP800SecureRandomBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Prng.Drbg;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class SP800SecureRandomBuilder
    {
        private readonly SecureRandom mRandom;
        private readonly IEntropySourceProvider mEntropySourceProvider;
        private byte[] mPersonalizationString = null;
        private int mSecurityStrength = 256;
        private int mEntropyBitsRequired = 256;

        public SP800SecureRandomBuilder()
          : this( new SecureRandom(), false )
        {
        }

        public SP800SecureRandomBuilder( SecureRandom entropySource, bool predictionResistant )
        {
            this.mRandom = entropySource;
            this.mEntropySourceProvider = new BasicEntropySourceProvider( entropySource, predictionResistant );
        }

        public SP800SecureRandomBuilder( IEntropySourceProvider entropySourceProvider )
        {
            this.mRandom = null;
            this.mEntropySourceProvider = entropySourceProvider;
        }

        public SP800SecureRandomBuilder SetPersonalizationString( byte[] personalizationString )
        {
            this.mPersonalizationString = personalizationString;
            return this;
        }

        public SP800SecureRandomBuilder SetSecurityStrength( int securityStrength )
        {
            this.mSecurityStrength = securityStrength;
            return this;
        }

        public SP800SecureRandomBuilder SetEntropyBitsRequired( int entropyBitsRequired )
        {
            this.mEntropyBitsRequired = entropyBitsRequired;
            return this;
        }

        public SP800SecureRandom BuildHash( IDigest digest, byte[] nonce, bool predictionResistant ) => new( this.mRandom, this.mEntropySourceProvider.Get( this.mEntropyBitsRequired ), new SP800SecureRandomBuilder.HashDrbgProvider( digest, nonce, this.mPersonalizationString, this.mSecurityStrength ), predictionResistant );

        public SP800SecureRandom BuildCtr(
          IBlockCipher cipher,
          int keySizeInBits,
          byte[] nonce,
          bool predictionResistant )
        {
            return new SP800SecureRandom( this.mRandom, this.mEntropySourceProvider.Get( this.mEntropyBitsRequired ), new SP800SecureRandomBuilder.CtrDrbgProvider( cipher, keySizeInBits, nonce, this.mPersonalizationString, this.mSecurityStrength ), predictionResistant );
        }

        public SP800SecureRandom BuildHMac( IMac hMac, byte[] nonce, bool predictionResistant ) => new( this.mRandom, this.mEntropySourceProvider.Get( this.mEntropyBitsRequired ), new SP800SecureRandomBuilder.HMacDrbgProvider( hMac, nonce, this.mPersonalizationString, this.mSecurityStrength ), predictionResistant );

        private class HashDrbgProvider : IDrbgProvider
        {
            private readonly IDigest mDigest;
            private readonly byte[] mNonce;
            private readonly byte[] mPersonalizationString;
            private readonly int mSecurityStrength;

            public HashDrbgProvider(
              IDigest digest,
              byte[] nonce,
              byte[] personalizationString,
              int securityStrength )
            {
                this.mDigest = digest;
                this.mNonce = nonce;
                this.mPersonalizationString = personalizationString;
                this.mSecurityStrength = securityStrength;
            }

            public ISP80090Drbg Get( IEntropySource entropySource ) => new HashSP800Drbg( this.mDigest, this.mSecurityStrength, entropySource, this.mPersonalizationString, this.mNonce );
        }

        private class HMacDrbgProvider : IDrbgProvider
        {
            private readonly IMac mHMac;
            private readonly byte[] mNonce;
            private readonly byte[] mPersonalizationString;
            private readonly int mSecurityStrength;

            public HMacDrbgProvider(
              IMac hMac,
              byte[] nonce,
              byte[] personalizationString,
              int securityStrength )
            {
                this.mHMac = hMac;
                this.mNonce = nonce;
                this.mPersonalizationString = personalizationString;
                this.mSecurityStrength = securityStrength;
            }

            public ISP80090Drbg Get( IEntropySource entropySource ) => new HMacSP800Drbg( this.mHMac, this.mSecurityStrength, entropySource, this.mPersonalizationString, this.mNonce );
        }

        private class CtrDrbgProvider : IDrbgProvider
        {
            private readonly IBlockCipher mBlockCipher;
            private readonly int mKeySizeInBits;
            private readonly byte[] mNonce;
            private readonly byte[] mPersonalizationString;
            private readonly int mSecurityStrength;

            public CtrDrbgProvider(
              IBlockCipher blockCipher,
              int keySizeInBits,
              byte[] nonce,
              byte[] personalizationString,
              int securityStrength )
            {
                this.mBlockCipher = blockCipher;
                this.mKeySizeInBits = keySizeInBits;
                this.mNonce = nonce;
                this.mPersonalizationString = personalizationString;
                this.mSecurityStrength = securityStrength;
            }

            public ISP80090Drbg Get( IEntropySource entropySource ) => new CtrSP800Drbg( this.mBlockCipher, this.mKeySizeInBits, this.mSecurityStrength, entropySource, this.mPersonalizationString, this.mNonce );
        }
    }
}
