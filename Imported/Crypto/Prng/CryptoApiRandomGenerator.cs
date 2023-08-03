// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.CryptoApiRandomGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Security.Cryptography;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class CryptoApiRandomGenerator : IRandomGenerator
    {
        private readonly RandomNumberGenerator rndProv;

        public CryptoApiRandomGenerator()
          : this( new RNGCryptoServiceProvider() )
        {
        }

        public CryptoApiRandomGenerator( RandomNumberGenerator rng ) => this.rndProv = rng;

        public virtual void AddSeedMaterial( byte[] seed )
        {
        }

        public virtual void AddSeedMaterial( long seed )
        {
        }

        public virtual void NextBytes( byte[] bytes ) => this.rndProv.GetBytes( bytes );

        public virtual void NextBytes( byte[] bytes, int start, int len )
        {
            if (start < 0)
                throw new ArgumentException( "Start offset cannot be negative", nameof( start ) );
            if (bytes.Length < start + len)
                throw new ArgumentException( "Byte array too small for requested offset and length" );
            if (bytes.Length == len && start == 0)
            {
                this.NextBytes( bytes );
            }
            else
            {
                byte[] numArray = new byte[len];
                this.NextBytes( numArray );
                Array.Copy( numArray, 0, bytes, start, len );
            }
        }
    }
}
