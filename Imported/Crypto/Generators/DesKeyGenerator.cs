// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DesKeyGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class DesKeyGenerator : CipherKeyGenerator
    {
        public DesKeyGenerator()
        {
        }

        internal DesKeyGenerator( int defaultStrength )
          : base( defaultStrength )
        {
        }

        protected override void engineInit( KeyGenerationParameters parameters )
        {
            base.engineInit( parameters );
            if (this.strength == 0 || this.strength == 7)
                this.strength = 8;
            else if (this.strength != 8)
                throw new ArgumentException( "DES key must be " + 64 + " bits long." );
        }

        protected override byte[] engineGenerateKey()
        {
            byte[] key = new byte[8];
            do
            {
                this.random.NextBytes( key );
                DesParameters.SetOddParity( key );
            }
            while (DesParameters.IsWeakKey( key, 0 ));
            return key;
        }
    }
}
