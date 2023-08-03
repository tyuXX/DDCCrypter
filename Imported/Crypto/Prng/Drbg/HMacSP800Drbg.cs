// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.Drbg.HMacSP800Drbg
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Prng.Drbg
{
    public class HMacSP800Drbg : ISP80090Drbg
    {
        private static readonly long RESEED_MAX = 140737488355328;
        private static readonly int MAX_BITS_REQUEST = 262144;
        private readonly byte[] mK;
        private readonly byte[] mV;
        private readonly IEntropySource mEntropySource;
        private readonly IMac mHMac;
        private readonly int mSecurityStrength;
        private long mReseedCounter;

        public HMacSP800Drbg(
          IMac hMac,
          int securityStrength,
          IEntropySource entropySource,
          byte[] personalizationString,
          byte[] nonce )
        {
            if (securityStrength > DrbgUtilities.GetMaxSecurityStrength( hMac ))
                throw new ArgumentException( "Requested security strength is not supported by the derivation function" );
            if (entropySource.EntropySize < securityStrength)
                throw new ArgumentException( "Not enough entropy for security strength required" );
            this.mHMac = hMac;
            this.mSecurityStrength = securityStrength;
            this.mEntropySource = entropySource;
            byte[] seedMaterial = Arrays.ConcatenateAll( this.GetEntropy(), nonce, personalizationString );
            this.mK = new byte[hMac.GetMacSize()];
            this.mV = new byte[this.mK.Length];
            Arrays.Fill( this.mV, 1 );
            this.hmac_DRBG_Update( seedMaterial );
            this.mReseedCounter = 1L;
        }

        private void hmac_DRBG_Update( byte[] seedMaterial )
        {
            this.hmac_DRBG_Update_Func( seedMaterial, 0 );
            if (seedMaterial == null)
                return;
            this.hmac_DRBG_Update_Func( seedMaterial, 1 );
        }

        private void hmac_DRBG_Update_Func( byte[] seedMaterial, byte vValue )
        {
            this.mHMac.Init( new KeyParameter( this.mK ) );
            this.mHMac.BlockUpdate( this.mV, 0, this.mV.Length );
            this.mHMac.Update( vValue );
            if (seedMaterial != null)
                this.mHMac.BlockUpdate( seedMaterial, 0, seedMaterial.Length );
            this.mHMac.DoFinal( this.mK, 0 );
            this.mHMac.Init( new KeyParameter( this.mK ) );
            this.mHMac.BlockUpdate( this.mV, 0, this.mV.Length );
            this.mHMac.DoFinal( this.mV, 0 );
        }

        public int BlockSize => this.mV.Length * 8;

        public int Generate( byte[] output, byte[] additionalInput, bool predictionResistant )
        {
            int num1 = output.Length * 8;
            if (num1 > MAX_BITS_REQUEST)
                throw new ArgumentException( "Number of bits per request limited to " + MAX_BITS_REQUEST, nameof( output ) );
            if (this.mReseedCounter > RESEED_MAX)
                return -1;
            if (predictionResistant)
            {
                this.Reseed( additionalInput );
                additionalInput = null;
            }
            if (additionalInput != null)
                this.hmac_DRBG_Update( additionalInput );
            byte[] numArray = new byte[output.Length];
            int num2 = output.Length / this.mV.Length;
            this.mHMac.Init( new KeyParameter( this.mK ) );
            for (int index = 0; index < num2; ++index)
            {
                this.mHMac.BlockUpdate( this.mV, 0, this.mV.Length );
                this.mHMac.DoFinal( this.mV, 0 );
                Array.Copy( mV, 0, numArray, index * this.mV.Length, this.mV.Length );
            }
            if (num2 * this.mV.Length < numArray.Length)
            {
                this.mHMac.BlockUpdate( this.mV, 0, this.mV.Length );
                this.mHMac.DoFinal( this.mV, 0 );
                Array.Copy( mV, 0, numArray, num2 * this.mV.Length, numArray.Length - (num2 * this.mV.Length) );
            }
            this.hmac_DRBG_Update( additionalInput );
            ++this.mReseedCounter;
            Array.Copy( numArray, 0, output, 0, output.Length );
            return num1;
        }

        public void Reseed( byte[] additionalInput )
        {
            this.hmac_DRBG_Update( Arrays.Concatenate( this.GetEntropy(), additionalInput ) );
            this.mReseedCounter = 1L;
        }

        private byte[] GetEntropy()
        {
            byte[] entropy = this.mEntropySource.GetEntropy();
            if (entropy.Length < (this.mSecurityStrength + 7) / 8)
                throw new InvalidOperationException( "Insufficient entropy provided by entropy source" );
            return entropy;
        }
    }
}
