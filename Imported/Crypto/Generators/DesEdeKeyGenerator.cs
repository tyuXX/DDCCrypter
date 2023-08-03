// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DesEdeKeyGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class DesEdeKeyGenerator : DesKeyGenerator
    {
        public DesEdeKeyGenerator()
        {
        }

        internal DesEdeKeyGenerator( int defaultStrength )
          : base( defaultStrength )
        {
        }

        protected override void engineInit( KeyGenerationParameters parameters )
        {
            this.random = parameters.Random;
            this.strength = (parameters.Strength + 7) / 8;
            if (this.strength == 0 || this.strength == 21)
                this.strength = 24;
            else if (this.strength == 14)
                this.strength = 16;
            else if (this.strength != 24 && this.strength != 16)
                throw new ArgumentException( "DESede key must be " + 192 + " or " + 128 + " bits long." );
        }

        protected override byte[] engineGenerateKey()
        {
            byte[] key = new byte[this.strength];
            do
            {
                this.random.NextBytes( key );
                DesParameters.SetOddParity( key );
            }
            while (DesEdeParameters.IsWeakKey( key, 0, key.Length ) || !DesEdeParameters.IsRealEdeKey( key, 0 ));
            return key;
        }
    }
}
