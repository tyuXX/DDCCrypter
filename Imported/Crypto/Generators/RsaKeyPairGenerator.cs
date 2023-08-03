// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.RsaKeyPairGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class RsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        protected const int DefaultTests = 100;
        private static readonly int[] SPECIAL_E_VALUES = new int[5]
        {
      3,
      5,
      17,
      257,
      65537
        };
        private static readonly int SPECIAL_E_HIGHEST = SPECIAL_E_VALUES[SPECIAL_E_VALUES.Length - 1];
        private static readonly int SPECIAL_E_BITS = BigInteger.ValueOf( SPECIAL_E_HIGHEST ).BitLength;
        protected static readonly BigInteger One = BigInteger.One;
        protected static readonly BigInteger DefaultPublicExponent = BigInteger.ValueOf( 65537L );
        protected RsaKeyGenerationParameters parameters;

        public virtual void Init( KeyGenerationParameters parameters )
        {
            if (parameters is RsaKeyGenerationParameters)
                this.parameters = (RsaKeyGenerationParameters)parameters;
            else
                this.parameters = new RsaKeyGenerationParameters( DefaultPublicExponent, parameters.Random, parameters.Strength, 100 );
        }

        public virtual AsymmetricCipherKeyPair GenerateKeyPair()
        {
            int bitlength1;
            BigInteger publicExponent;
            BigInteger bigInteger1;
            BigInteger bigInteger2;
            BigInteger bigInteger3;
            BigInteger n;
            BigInteger bigInteger4;
            BigInteger privateExponent;
            do
            {
                int strength = this.parameters.Strength;
                int bitlength2 = (strength + 1) / 2;
                bitlength1 = strength - bitlength2;
                int num1 = strength / 3;
                int num2 = strength >> 2;
                publicExponent = this.parameters.PublicExponent;
                bigInteger1 = this.ChooseRandomPrime( bitlength2, publicExponent );
                while (true)
                {
                    do
                    {
                        bigInteger2 = this.ChooseRandomPrime( bitlength1, publicExponent );
                    }
                    while (bigInteger2.Subtract( bigInteger1 ).Abs().BitLength < num1);
                    bigInteger3 = bigInteger1.Multiply( bigInteger2 );
                    if (bigInteger3.BitLength != strength)
                        bigInteger1 = bigInteger1.Max( bigInteger2 );
                    else if (WNafUtilities.GetNafWeight( bigInteger3 ) < num2)
                        bigInteger1 = this.ChooseRandomPrime( bitlength2, publicExponent );
                    else
                        break;
                }
                if (bigInteger1.CompareTo( bigInteger2 ) < 0)
                {
                    BigInteger bigInteger5 = bigInteger1;
                    bigInteger1 = bigInteger2;
                    bigInteger2 = bigInteger5;
                }
                n = bigInteger1.Subtract( One );
                bigInteger4 = bigInteger2.Subtract( One );
                BigInteger val = n.Gcd( bigInteger4 );
                BigInteger m = n.Divide( val ).Multiply( bigInteger4 );
                privateExponent = publicExponent.ModInverse( m );
            }
            while (privateExponent.BitLength <= bitlength1);
            BigInteger dP = privateExponent.Remainder( n );
            BigInteger dQ = privateExponent.Remainder( bigInteger4 );
            BigInteger qInv = bigInteger2.ModInverse( bigInteger1 );
            return new AsymmetricCipherKeyPair( new RsaKeyParameters( false, bigInteger3, publicExponent ), new RsaPrivateCrtKeyParameters( bigInteger3, publicExponent, privateExponent, bigInteger1, bigInteger2, dP, dQ, qInv ) );
        }

        protected virtual BigInteger ChooseRandomPrime( int bitlength, BigInteger e )
        {
            bool flag = e.BitLength <= SPECIAL_E_BITS && Arrays.Contains( SPECIAL_E_VALUES, e.IntValue );
            BigInteger bigInteger;
            do
            {
                bigInteger = new BigInteger( bitlength, 1, parameters.Random );
            }
            while (bigInteger.Mod( e ).Equals( One ) || !bigInteger.IsProbablePrime( this.parameters.Certainty, true ) || (!flag && !e.Gcd( bigInteger.Subtract( One ) ).Equals( One )));
            return bigInteger;
        }
    }
}
