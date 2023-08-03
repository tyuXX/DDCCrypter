// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.CipherKeyGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto
{
    public class CipherKeyGenerator
    {
        protected internal SecureRandom random;
        protected internal int strength;
        private bool uninitialised = true;
        private int defaultStrength;

        public CipherKeyGenerator()
        {
        }

        internal CipherKeyGenerator( int defaultStrength ) => this.defaultStrength = defaultStrength >= 1 ? defaultStrength : throw new ArgumentException( "strength must be a positive value", nameof( defaultStrength ) );

        public int DefaultStrength => this.defaultStrength;

        public void Init( KeyGenerationParameters parameters )
        {
            if (parameters == null)
                throw new ArgumentNullException( nameof( parameters ) );
            this.uninitialised = false;
            this.engineInit( parameters );
        }

        protected virtual void engineInit( KeyGenerationParameters parameters )
        {
            this.random = parameters.Random;
            this.strength = (parameters.Strength + 7) / 8;
        }

        public byte[] GenerateKey()
        {
            if (this.uninitialised)
            {
                if (this.defaultStrength < 1)
                    throw new InvalidOperationException( "Generator has not been initialised" );
                this.uninitialised = false;
                this.engineInit( new KeyGenerationParameters( new SecureRandom(), this.defaultStrength ) );
            }
            return this.engineGenerateKey();
        }

        protected virtual byte[] engineGenerateKey() => SecureRandom.GetNextBytes( this.random, this.strength );
    }
}
