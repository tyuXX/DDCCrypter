// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DHParametersHelper
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    internal class DHParametersHelper
    {
        private static readonly BigInteger Six = BigInteger.ValueOf( 6L );
        private static readonly int[][] primeLists = BigInteger.primeLists;
        private static readonly int[] primeProducts = BigInteger.primeProducts;
        private static readonly BigInteger[] BigPrimeProducts = ConstructBigPrimeProducts( primeProducts );

        private static BigInteger[] ConstructBigPrimeProducts( int[] primeProducts )
        {
            BigInteger[] bigIntegerArray = new BigInteger[primeProducts.Length];
            for (int index = 0; index < bigIntegerArray.Length; ++index)
                bigIntegerArray[index] = BigInteger.ValueOf( primeProducts[index] );
            return bigIntegerArray;
        }

        internal static BigInteger[] GenerateSafePrimes( int size, int certainty, SecureRandom random )
        {
            int bitLength = size - 1;
            int num1 = size >> 2;
            BigInteger bigInteger;
            BigInteger k;
            if (size <= 32)
            {
                do
                {
                    bigInteger = new BigInteger( bitLength, 2, random );
                    k = bigInteger.ShiftLeft( 1 ).Add( BigInteger.One );
                }
                while (!k.IsProbablePrime( certainty, true ) || (certainty > 2 && !bigInteger.IsProbablePrime( certainty, true )));
            }
            else
            {
                do
                {
                    do
                    {
                        bigInteger = new BigInteger( bitLength, 0, random );
                    label_3:
                        for (int index = 0; index < primeLists.Length; ++index)
                        {
                            int num2 = bigInteger.Remainder( BigPrimeProducts[index] ).IntValue;
                            if (index == 0)
                            {
                                int num3 = num2 % 3;
                                if (num3 != 2)
                                {
                                    int num4 = (2 * num3) + 2;
                                    bigInteger = bigInteger.Add( BigInteger.ValueOf( num4 ) );
                                    num2 = (num2 + num4) % primeProducts[index];
                                }
                            }
                            foreach (int num5 in primeLists[index])
                            {
                                int num6 = num2 % num5;
                                if (num6 == 0 || num6 == num5 >> 1)
                                {
                                    bigInteger = bigInteger.Add( Six );
                                    goto label_3;
                                }
                            }
                        }
                    }
                    while (bigInteger.BitLength != bitLength || !bigInteger.RabinMillerTest( 2, random, true ));
                    k = bigInteger.ShiftLeft( 1 ).Add( BigInteger.One );
                }
                while (!k.RabinMillerTest( certainty, random, true ) || (certainty > 2 && !bigInteger.RabinMillerTest( certainty - 2, random, true )) || WNafUtilities.GetNafWeight( k ) < num1);
            }
            return new BigInteger[2] { k, bigInteger };
        }

        internal static BigInteger SelectGenerator( BigInteger p, BigInteger q, SecureRandom random )
        {
            BigInteger max = p.Subtract( BigInteger.Two );
            BigInteger bigInteger;
            do
            {
                bigInteger = BigIntegers.CreateRandomInRange( BigInteger.Two, max, random ).ModPow( BigInteger.Two, p );
            }
            while (bigInteger.Equals( BigInteger.One ));
            return bigInteger;
        }
    }
}
