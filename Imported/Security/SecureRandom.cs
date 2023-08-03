// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.SecureRandom
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Utilities;
using System.Threading;

namespace Org.BouncyCastle.Security
{
    public class SecureRandom : Random
    {
        private static long counter = Times.NanoTime();
        private static readonly SecureRandom master = new( new CryptoApiRandomGenerator() );
        protected readonly IRandomGenerator generator;
        private static readonly double DoubleScale = System.Math.Pow( 2.0, 64.0 );

        private static long NextCounterValue() => Interlocked.Increment( ref counter );

        private static SecureRandom Master => master;

        private static DigestRandomGenerator CreatePrng( string digestName, bool autoSeed )
        {
            IDigest digest = DigestUtilities.GetDigest( digestName );
            if (digest == null)
                return null;
            DigestRandomGenerator prng = new( digest );
            if (autoSeed)
            {
                prng.AddSeedMaterial( NextCounterValue() );
                prng.AddSeedMaterial( GetNextBytes( Master, digest.GetDigestSize() ) );
            }
            return prng;
        }

        public static byte[] GetNextBytes( SecureRandom secureRandom, int length )
        {
            byte[] buffer = new byte[length];
            secureRandom.NextBytes( buffer );
            return buffer;
        }

        public static SecureRandom GetInstance( string algorithm ) => GetInstance( algorithm, true );

        public static SecureRandom GetInstance( string algorithm, bool autoSeed )
        {
            string upperInvariant = Platform.ToUpperInvariant( algorithm );
            if (Platform.EndsWith( upperInvariant, "PRNG" ))
            {
                DigestRandomGenerator prng = CreatePrng( upperInvariant.Substring( 0, upperInvariant.Length - "PRNG".Length ), autoSeed );
                if (prng != null)
                    return new SecureRandom( prng );
            }
            throw new ArgumentException( "Unrecognised PRNG algorithm: " + algorithm, nameof( algorithm ) );
        }

        [Obsolete( "Call GenerateSeed() on a SecureRandom instance instead" )]
        public static byte[] GetSeed( int length ) => GetNextBytes( Master, length );

        public SecureRandom()
          : this( CreatePrng( "SHA256", true ) )
        {
        }

        [Obsolete( "Use GetInstance/SetSeed instead" )]
        public SecureRandom( byte[] seed )
          : this( CreatePrng( "SHA1", false ) )
        {
            this.SetSeed( seed );
        }

        public SecureRandom( IRandomGenerator generator )
          : base( 0 )
        {
            this.generator = generator;
        }

        public virtual byte[] GenerateSeed( int length ) => GetNextBytes( Master, length );

        public virtual void SetSeed( byte[] seed ) => this.generator.AddSeedMaterial( seed );

        public virtual void SetSeed( long seed ) => this.generator.AddSeedMaterial( seed );

        public override int Next() => this.NextInt() & int.MaxValue;

        public override int Next( int maxValue )
        {
            if (maxValue < 2)
            {
                if (maxValue < 0)
                    throw new ArgumentOutOfRangeException( nameof( maxValue ), "cannot be negative" );
                return 0;
            }
            if ((maxValue & (maxValue - 1)) == 0)
                return this.NextInt() & (maxValue - 1);
            int num1;
            int num2;
            do
            {
                num1 = this.NextInt() & int.MaxValue;
                num2 = num1 % maxValue;
            }
            while (num1 - num2 + (maxValue - 1) < 0);
            return num2;
        }

        public override int Next( int minValue, int maxValue )
        {
            if (maxValue <= minValue)
                return maxValue == minValue ? minValue : throw new ArgumentException( "maxValue cannot be less than minValue" );
            int maxValue1 = maxValue - minValue;
            if (maxValue1 > 0)
                return minValue + this.Next( maxValue1 );
            int num;
            do
            {
                num = this.NextInt();
            }
            while (num < minValue || num >= maxValue);
            return num;
        }

        public override void NextBytes( byte[] buf ) => this.generator.NextBytes( buf );

        public virtual void NextBytes( byte[] buf, int off, int len ) => this.generator.NextBytes( buf, off, len );

        public override double NextDouble() => Convert.ToDouble( (ulong)this.NextLong() ) / DoubleScale;

        public virtual int NextInt()
        {
            byte[] buffer = new byte[4];
            this.NextBytes( buffer );
            return (int)(((((((uint)buffer[0] << 8) | buffer[1]) << 8) | buffer[2]) << 8) | buffer[3]);
        }

        public virtual long NextLong() => ((long)(uint)this.NextInt() << 32) | (uint)this.NextInt();
    }
}
