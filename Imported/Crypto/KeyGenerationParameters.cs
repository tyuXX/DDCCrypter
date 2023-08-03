// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.KeyGenerationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto
{
    public class KeyGenerationParameters
    {
        private SecureRandom random;
        private int strength;

        public KeyGenerationParameters( SecureRandom random, int strength )
        {
            if (random == null)
                throw new ArgumentNullException( nameof( random ) );
            if (strength < 1)
                throw new ArgumentException( "strength must be a positive value", nameof( strength ) );
            this.random = random;
            this.strength = strength;
        }

        public SecureRandom Random => this.random;

        public int Strength => this.strength;
    }
}
