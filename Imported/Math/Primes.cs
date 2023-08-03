// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Primes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Math
{
    public abstract class Primes
    {
        public static readonly int SmallFactorLimit = 211;
        private static readonly BigInteger One = BigInteger.One;
        private static readonly BigInteger Two = BigInteger.Two;
        private static readonly BigInteger Three = BigInteger.Three;

        public static Primes.STOutput GenerateSTRandomPrime( IDigest hash, int length, byte[] inputSeed )
        {
            if (hash == null)
                throw new ArgumentNullException( nameof( hash ) );
            if (length < 2)
                throw new ArgumentException( "must be >= 2", nameof( length ) );
            if (inputSeed == null)
                throw new ArgumentNullException( nameof( inputSeed ) );
            if (inputSeed.Length == 0)
                throw new ArgumentException( "cannot be empty", nameof( inputSeed ) );
            return ImplSTRandomPrime( hash, length, Arrays.Clone( inputSeed ) );
        }

        public static Primes.MROutput EnhancedMRProbablePrimeTest(
          BigInteger candidate,
          SecureRandom random,
          int iterations )
        {
            CheckCandidate( candidate, nameof( candidate ) );
            if (random == null)
                throw new ArgumentNullException( nameof( random ) );
            if (iterations < 1)
                throw new ArgumentException( "must be > 0", nameof( iterations ) );
            if (candidate.BitLength == 2)
                return MROutput.ProbablyPrime();
            if (!candidate.TestBit( 0 ))
                return MROutput.ProvablyCompositeWithFactor( Two );
            BigInteger m = candidate;
            BigInteger bigInteger1 = candidate.Subtract( One );
            BigInteger max = candidate.Subtract( Two );
            int lowestSetBit = bigInteger1.GetLowestSetBit();
            BigInteger e = bigInteger1.ShiftRight( lowestSetBit );
            for (int index1 = 0; index1 < iterations; ++index1)
            {
                BigInteger randomInRange = BigIntegers.CreateRandomInRange( Two, max, random );
                BigInteger factor1 = randomInRange.Gcd( m );
                if (factor1.CompareTo( One ) > 0)
                    return MROutput.ProvablyCompositeWithFactor( factor1 );
                BigInteger bigInteger2 = randomInRange.ModPow( e, m );
                if (!bigInteger2.Equals( One ) && !bigInteger2.Equals( bigInteger1 ))
                {
                    bool flag = false;
                    BigInteger bigInteger3 = bigInteger2;
                    for (int index2 = 1; index2 < lowestSetBit; ++index2)
                    {
                        bigInteger2 = bigInteger2.ModPow( Two, m );
                        if (bigInteger2.Equals( bigInteger1 ))
                        {
                            flag = true;
                            break;
                        }
                        if (!bigInteger2.Equals( One ))
                            bigInteger3 = bigInteger2;
                        else
                            break;
                    }
                    if (!flag)
                    {
                        if (!bigInteger2.Equals( One ))
                        {
                            bigInteger3 = bigInteger2;
                            BigInteger bigInteger4 = bigInteger2.ModPow( Two, m );
                            if (!bigInteger4.Equals( One ))
                                bigInteger3 = bigInteger4;
                        }
                        BigInteger factor2 = bigInteger3.Subtract( One ).Gcd( m );
                        return factor2.CompareTo( One ) > 0 ? MROutput.ProvablyCompositeWithFactor( factor2 ) : MROutput.ProvablyCompositeNotPrimePower();
                    }
                }
            }
            return MROutput.ProbablyPrime();
        }

        public static bool HasAnySmallFactors( BigInteger candidate )
        {
            CheckCandidate( candidate, nameof( candidate ) );
            return ImplHasAnySmallFactors( candidate );
        }

        public static bool IsMRProbablePrime( BigInteger candidate, SecureRandom random, int iterations )
        {
            CheckCandidate( candidate, nameof( candidate ) );
            if (random == null)
                throw new ArgumentException( "cannot be null", nameof( random ) );
            if (iterations < 1)
                throw new ArgumentException( "must be > 0", nameof( iterations ) );
            if (candidate.BitLength == 2)
                return true;
            if (!candidate.TestBit( 0 ))
                return false;
            BigInteger w = candidate;
            BigInteger wSubOne = candidate.Subtract( One );
            BigInteger max = candidate.Subtract( Two );
            int lowestSetBit = wSubOne.GetLowestSetBit();
            BigInteger m = wSubOne.ShiftRight( lowestSetBit );
            for (int index = 0; index < iterations; ++index)
            {
                BigInteger randomInRange = BigIntegers.CreateRandomInRange( Two, max, random );
                if (!ImplMRProbablePrimeToBase( w, wSubOne, m, lowestSetBit, randomInRange ))
                    return false;
            }
            return true;
        }

        public static bool IsMRProbablePrimeToBase( BigInteger candidate, BigInteger baseValue )
        {
            CheckCandidate( candidate, nameof( candidate ) );
            CheckCandidate( baseValue, nameof( baseValue ) );
            if (baseValue.CompareTo( candidate.Subtract( One ) ) >= 0)
                throw new ArgumentException( "must be < ('candidate' - 1)", nameof( baseValue ) );
            if (candidate.BitLength == 2)
                return true;
            BigInteger w = candidate;
            BigInteger wSubOne = candidate.Subtract( One );
            int lowestSetBit = wSubOne.GetLowestSetBit();
            BigInteger m = wSubOne.ShiftRight( lowestSetBit );
            return ImplMRProbablePrimeToBase( w, wSubOne, m, lowestSetBit, baseValue );
        }

        private static void CheckCandidate( BigInteger n, string name )
        {
            if (n == null || n.SignValue < 1 || n.BitLength < 2)
                throw new ArgumentException( "must be non-null and >= 2", name );
        }

        private static bool ImplHasAnySmallFactors( BigInteger x )
        {
            int num1 = 223092870;
            int intValue1 = x.Mod( BigInteger.ValueOf( num1 ) ).IntValue;
            if (intValue1 % 2 == 0 || intValue1 % 3 == 0 || intValue1 % 5 == 0 || intValue1 % 7 == 0 || intValue1 % 11 == 0 || intValue1 % 13 == 0 || intValue1 % 17 == 0 || intValue1 % 19 == 0 || intValue1 % 23 == 0)
                return true;
            int num2 = 58642669;
            int intValue2 = x.Mod( BigInteger.ValueOf( num2 ) ).IntValue;
            if (intValue2 % 29 == 0 || intValue2 % 31 == 0 || intValue2 % 37 == 0 || intValue2 % 41 == 0 || intValue2 % 43 == 0)
                return true;
            int num3 = 600662303;
            int intValue3 = x.Mod( BigInteger.ValueOf( num3 ) ).IntValue;
            if (intValue3 % 47 == 0 || intValue3 % 53 == 0 || intValue3 % 59 == 0 || intValue3 % 61 == 0 || intValue3 % 67 == 0)
                return true;
            int num4 = 33984931;
            int intValue4 = x.Mod( BigInteger.ValueOf( num4 ) ).IntValue;
            if (intValue4 % 71 == 0 || intValue4 % 73 == 0 || intValue4 % 79 == 0 || intValue4 % 83 == 0)
                return true;
            int num5 = 89809099;
            int intValue5 = x.Mod( BigInteger.ValueOf( num5 ) ).IntValue;
            if (intValue5 % 89 == 0 || intValue5 % 97 == 0 || intValue5 % 101 == 0 || intValue5 % 103 == 0)
                return true;
            int num6 = 167375713;
            int intValue6 = x.Mod( BigInteger.ValueOf( num6 ) ).IntValue;
            if (intValue6 % 107 == 0 || intValue6 % 109 == 0 || intValue6 % 113 == 0 || intValue6 % sbyte.MaxValue == 0)
                return true;
            int num7 = 371700317;
            int intValue7 = x.Mod( BigInteger.ValueOf( num7 ) ).IntValue;
            if (intValue7 % 131 == 0 || intValue7 % 137 == 0 || intValue7 % 139 == 0 || intValue7 % 149 == 0)
                return true;
            int num8 = 645328247;
            int intValue8 = x.Mod( BigInteger.ValueOf( num8 ) ).IntValue;
            if (intValue8 % 151 == 0 || intValue8 % 157 == 0 || intValue8 % 163 == 0 || intValue8 % 167 == 0)
                return true;
            int num9 = 1070560157;
            int intValue9 = x.Mod( BigInteger.ValueOf( num9 ) ).IntValue;
            if (intValue9 % 173 == 0 || intValue9 % 179 == 0 || intValue9 % 181 == 0 || intValue9 % 191 == 0)
                return true;
            int num10 = 1596463769;
            int intValue10 = x.Mod( BigInteger.ValueOf( num10 ) ).IntValue;
            return intValue10 % 193 == 0 || intValue10 % 197 == 0 || intValue10 % 199 == 0 || intValue10 % 211 == 0;
        }

        private static bool ImplMRProbablePrimeToBase(
          BigInteger w,
          BigInteger wSubOne,
          BigInteger m,
          int a,
          BigInteger b )
        {
            BigInteger bigInteger = b.ModPow( m, w );
            if (bigInteger.Equals( One ) || bigInteger.Equals( wSubOne ))
                return true;
            bool flag = false;
            for (int index = 1; index < a; ++index)
            {
                bigInteger = bigInteger.ModPow( Two, w );
                if (bigInteger.Equals( wSubOne ))
                {
                    flag = true;
                    break;
                }
                if (bigInteger.Equals( One ))
                    return false;
            }
            return flag;
        }

        private static Primes.STOutput ImplSTRandomPrime( IDigest d, int length, byte[] primeSeed )
        {
            int digestSize = d.GetDigestSize();
            if (length < 33)
            {
                int primeGenCounter = 0;
                byte[] numArray1 = new byte[digestSize];
                byte[] numArray2 = new byte[digestSize];
                do
                {
                    Hash( d, primeSeed, numArray1, 0 );
                    Inc( primeSeed, 1 );
                    Hash( d, primeSeed, numArray2, 0 );
                    Inc( primeSeed, 1 );
                    uint x = ((Extract32( numArray1 ) ^ Extract32( numArray2 )) & (uint.MaxValue >> (32 - length))) | (uint)((1 << (length - 1)) | 1);
                    ++primeGenCounter;
                    if (IsPrime32( x ))
                        return new Primes.STOutput( BigInteger.ValueOf( x ), primeSeed, primeGenCounter );
                }
                while (primeGenCounter <= 4 * length);
                throw new InvalidOperationException( "Too many iterations in Shawe-Taylor Random_Prime Routine" );
            }
            Primes.STOutput stOutput = ImplSTRandomPrime( d, (length + 3) / 2, primeSeed );
            BigInteger prime = stOutput.Prime;
            primeSeed = stOutput.PrimeSeed;
            int primeGenCounter1 = stOutput.PrimeGenCounter;
            int num1 = 8 * digestSize;
            int num2 = (length - 1) / num1;
            int num3 = primeGenCounter1;
            BigInteger bigInteger1 = HashGen( d, primeSeed, num2 + 1 ).Mod( One.ShiftLeft( length - 1 ) ).SetBit( length - 1 );
            BigInteger val = prime.ShiftLeft( 1 );
            BigInteger e = bigInteger1.Subtract( One ).Divide( val ).Add( One ).ShiftLeft( 1 );
            int num4 = 0;
            BigInteger bigInteger2 = e.Multiply( prime ).Add( One );
            while (true)
            {
                if (bigInteger2.BitLength > length)
                {
                    e = One.ShiftLeft( length - 1 ).Subtract( One ).Divide( val ).Add( One ).ShiftLeft( 1 );
                    bigInteger2 = e.Multiply( prime ).Add( One );
                }
                ++primeGenCounter1;
                if (!ImplHasAnySmallFactors( bigInteger2 ))
                {
                    BigInteger bigInteger3 = HashGen( d, primeSeed, num2 + 1 ).Mod( bigInteger2.Subtract( Three ) ).Add( Two );
                    e = e.Add( BigInteger.ValueOf( num4 ) );
                    num4 = 0;
                    BigInteger bigInteger4 = bigInteger3.ModPow( e, bigInteger2 );
                    if (bigInteger2.Gcd( bigInteger4.Subtract( One ) ).Equals( One ) && bigInteger4.ModPow( prime, bigInteger2 ).Equals( One ))
                        break;
                }
                else
                    Inc( primeSeed, num2 + 1 );
                if (primeGenCounter1 < (4 * length) + num3)
                {
                    num4 += 2;
                    bigInteger2 = bigInteger2.Add( val );
                }
                else
                    goto label_14;
            }
            return new Primes.STOutput( bigInteger2, primeSeed, primeGenCounter1 );
        label_14:
            throw new InvalidOperationException( "Too many iterations in Shawe-Taylor Random_Prime Routine" );
        }

        private static uint Extract32( byte[] bs )
        {
            uint num1 = 0;
            int num2 = System.Math.Min( 4, bs.Length );
            for (int index = 0; index < num2; ++index)
            {
                uint b = bs[bs.Length - (index + 1)];
                num1 |= b << (8 * index);
            }
            return num1;
        }

        private static void Hash( IDigest d, byte[] input, byte[] output, int outPos )
        {
            d.BlockUpdate( input, 0, input.Length );
            d.DoFinal( output, outPos );
        }

        private static BigInteger HashGen( IDigest d, byte[] seed, int count )
        {
            int digestSize = d.GetDigestSize();
            int outPos = count * digestSize;
            byte[] numArray = new byte[outPos];
            for (int index = 0; index < count; ++index)
            {
                outPos -= digestSize;
                Hash( d, seed, numArray, outPos );
                Inc( seed, 1 );
            }
            return new BigInteger( 1, numArray );
        }

        private static void Inc( byte[] seed, int c )
        {
            for (int length = seed.Length; c > 0 && --length >= 0; c >>= 8)
            {
                c += seed[length];
                seed[length] = (byte)c;
            }
        }

        private static bool IsPrime32( uint x )
        {
            switch (x)
            {
                case 0:
                case 1:
                case 4:
                case 5:
                    return x == 5U;
                case 2:
                case 3:
                    return true;
                default:
                    if (((int)x & 1) == 0 || x % 3U == 0U || x % 5U == 0U)
                        return false;
                    uint[] numArray = new uint[8]
                    {
            1U,
            7U,
            11U,
            13U,
            17U,
            19U,
            23U,
            29U
                    };
                    uint num1 = 0;
                    int index = 1;
                    while (true)
                    {
                        for (; index < numArray.Length; ++index)
                        {
                            uint num2 = num1 + numArray[index];
                            if (x % num2 == 0U)
                                return x < 30U;
                        }
                        num1 += 30U;
                        if (num1 >> 16 == 0U && num1 * num1 < x)
                            index = 0;
                        else
                            break;
                    }
                    return true;
            }
        }

        public class MROutput
        {
            private readonly bool mProvablyComposite;
            private readonly BigInteger mFactor;

            internal static Primes.MROutput ProbablyPrime() => new Primes.MROutput( false, null );

            internal static Primes.MROutput ProvablyCompositeWithFactor( BigInteger factor ) => new Primes.MROutput( true, factor );

            internal static Primes.MROutput ProvablyCompositeNotPrimePower() => new Primes.MROutput( true, null );

            private MROutput( bool provablyComposite, BigInteger factor )
            {
                this.mProvablyComposite = provablyComposite;
                this.mFactor = factor;
            }

            public BigInteger Factor => this.mFactor;

            public bool IsProvablyComposite => this.mProvablyComposite;

            public bool IsNotPrimePower => this.mProvablyComposite && this.mFactor == null;
        }

        public class STOutput
        {
            private readonly BigInteger mPrime;
            private readonly byte[] mPrimeSeed;
            private readonly int mPrimeGenCounter;

            internal STOutput( BigInteger prime, byte[] primeSeed, int primeGenCounter )
            {
                this.mPrime = prime;
                this.mPrimeSeed = primeSeed;
                this.mPrimeGenCounter = primeGenCounter;
            }

            public BigInteger Prime => this.mPrime;

            public byte[] PrimeSeed => this.mPrimeSeed;

            public int PrimeGenCounter => this.mPrimeGenCounter;
        }
    }
}
