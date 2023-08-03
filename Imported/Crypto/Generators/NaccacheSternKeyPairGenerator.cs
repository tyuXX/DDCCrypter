// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.NaccacheSternKeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class NaccacheSternKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private static readonly int[] smallPrimes = new int[101]
        {
      3,
      5,
      7,
      11,
      13,
      17,
      19,
      23,
      29,
      31,
      37,
      41,
      43,
      47,
      53,
      59,
      61,
      67,
      71,
      73,
      79,
      83,
      89,
      97,
      101,
      103,
      107,
      109,
      113,
       sbyte.MaxValue,
      131,
      137,
      139,
      149,
      151,
      157,
      163,
      167,
      173,
      179,
      181,
      191,
      193,
      197,
      199,
      211,
      223,
      227,
      229,
      233,
      239,
      241,
      251,
      257,
      263,
      269,
      271,
      277,
      281,
      283,
      293,
      307,
      311,
      313,
      317,
      331,
      337,
      347,
      349,
      353,
      359,
      367,
      373,
      379,
      383,
      389,
      397,
      401,
      409,
      419,
      421,
      431,
      433,
      439,
      443,
      449,
      457,
      461,
      463,
      467,
      479,
      487,
      491,
      499,
      503,
      509,
      521,
      523,
      541,
      547,
      557
        };
        private NaccacheSternKeyGenerationParameters param;

        public void Init( KeyGenerationParameters parameters ) => this.param = (NaccacheSternKeyGenerationParameters)parameters;

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            int strength = this.param.Strength;
            SecureRandom random = this.param.Random;
            int certainty = this.param.Certainty;
            IList smallPrimes = permuteList( findFirstPrimes( this.param.CountSmallPrimes ), random );
            BigInteger val1 = BigInteger.One;
            BigInteger val2 = BigInteger.One;
            for (int index = 0; index < smallPrimes.Count / 2; ++index)
                val1 = val1.Multiply( (BigInteger)smallPrimes[index] );
            for (int index = smallPrimes.Count / 2; index < smallPrimes.Count; ++index)
                val2 = val2.Multiply( (BigInteger)smallPrimes[index] );
            BigInteger bigInteger1 = val1.Multiply( val2 );
            int num1 = strength - bigInteger1.BitLength - 48;
            BigInteger prime1 = generatePrime( (num1 / 2) + 1, certainty, random );
            BigInteger prime2 = generatePrime( (num1 / 2) + 1, certainty, random );
            long num2 = 0;
            BigInteger val3 = prime1.Multiply( val1 ).ShiftLeft( 1 );
            BigInteger val4 = prime2.Multiply( val2 ).ShiftLeft( 1 );
            BigInteger prime3;
            BigInteger bigInteger2;
            BigInteger prime4;
            BigInteger val5;
            do
            {
                do
                {
                    ++num2;
                    prime3 = generatePrime( 24, certainty, random );
                    bigInteger2 = prime3.Multiply( val3 ).Add( BigInteger.One );
                }
                while (!bigInteger2.IsProbablePrime( certainty, true ));
                do
                {
                    do
                    {
                        prime4 = generatePrime( 24, certainty, random );
                    }
                    while (prime3.Equals( prime4 ));
                    val5 = prime4.Multiply( val4 ).Add( BigInteger.One );
                }
                while (!val5.IsProbablePrime( certainty, true ));
            }
            while (!bigInteger1.Gcd( prime3.Multiply( prime4 ) ).Equals( BigInteger.One ) || bigInteger2.Multiply( val5 ).BitLength < strength);
            BigInteger bigInteger3 = bigInteger2.Multiply( val5 );
            BigInteger phiN = bigInteger2.Subtract( BigInteger.One ).Multiply( val5.Subtract( BigInteger.One ) );
            long num3 = 0;
            BigInteger g;
            bool flag;
            do
            {
                IList arrayList = Platform.CreateArrayList();
                for (int index = 0; index != smallPrimes.Count; ++index)
                {
                    BigInteger val6 = (BigInteger)smallPrimes[index];
                    BigInteger e = phiN.Divide( val6 );
                    BigInteger prime5;
                    do
                    {
                        ++num3;
                        prime5 = generatePrime( strength, certainty, random );
                    }
                    while (prime5.ModPow( e, bigInteger3 ).Equals( BigInteger.One ));
                    arrayList.Add( prime5 );
                }
                g = BigInteger.One;
                for (int index = 0; index < smallPrimes.Count; ++index)
                {
                    BigInteger bigInteger4 = (BigInteger)arrayList[index];
                    BigInteger val7 = (BigInteger)smallPrimes[index];
                    g = g.Multiply( bigInteger4.ModPow( bigInteger1.Divide( val7 ), bigInteger3 ) ).Mod( bigInteger3 );
                }
                flag = false;
                for (int index = 0; index < smallPrimes.Count; ++index)
                {
                    if (g.ModPow( phiN.Divide( (BigInteger)smallPrimes[index] ), bigInteger3 ).Equals( BigInteger.One ))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            while (flag || g.ModPow( phiN.ShiftRight( 2 ), bigInteger3 ).Equals( BigInteger.One ) || g.ModPow( phiN.Divide( prime3 ), bigInteger3 ).Equals( BigInteger.One ) || g.ModPow( phiN.Divide( prime4 ), bigInteger3 ).Equals( BigInteger.One ) || g.ModPow( phiN.Divide( prime1 ), bigInteger3 ).Equals( BigInteger.One ) || g.ModPow( phiN.Divide( prime2 ), bigInteger3 ).Equals( BigInteger.One ));
            return new AsymmetricCipherKeyPair( new NaccacheSternKeyParameters( false, g, bigInteger3, bigInteger1.BitLength ), new NaccacheSternPrivateKeyParameters( g, bigInteger3, bigInteger1.BitLength, smallPrimes, phiN ) );
        }

        private static BigInteger generatePrime( int bitLength, int certainty, SecureRandom rand ) => new BigInteger( bitLength, certainty, rand );

        private static IList permuteList( IList arr, SecureRandom rand )
        {
            IList arrayList = Platform.CreateArrayList( arr.Count );
            foreach (object obj in (IEnumerable)arr)
            {
                int index = rand.Next( arrayList.Count + 1 );
                arrayList.Insert( index, obj );
            }
            return arrayList;
        }

        private static IList findFirstPrimes( int count )
        {
            IList arrayList = Platform.CreateArrayList( count );
            for (int index = 0; index != count; ++index)
                arrayList.Add( BigInteger.ValueOf( smallPrimes[index] ) );
            return arrayList;
        }
    }
}
