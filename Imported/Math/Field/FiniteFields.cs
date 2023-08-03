// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Field.FiniteFields
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.Field
{
    public abstract class FiniteFields
    {
        internal static readonly IFiniteField GF_2 = new PrimeField( BigInteger.ValueOf( 2L ) );
        internal static readonly IFiniteField GF_3 = new PrimeField( BigInteger.ValueOf( 3L ) );

        public static IPolynomialExtensionField GetBinaryExtensionField( int[] exponents )
        {
            if (exponents[0] != 0)
                throw new ArgumentException( "Irreducible polynomials in GF(2) must have constant term", nameof( exponents ) );
            for (int index = 1; index < exponents.Length; ++index)
            {
                if (exponents[index] <= exponents[index - 1])
                    throw new ArgumentException( "Polynomial exponents must be montonically increasing", nameof( exponents ) );
            }
            return new GenericPolynomialExtensionField( GF_2, new GF2Polynomial( exponents ) );
        }

        public static IFiniteField GetPrimeField( BigInteger characteristic )
        {
            int bitLength = characteristic.BitLength;
            if (characteristic.SignValue <= 0 || bitLength < 2)
                throw new ArgumentException( "Must be >= 2", nameof( characteristic ) );
            if (bitLength < 3)
            {
                switch (characteristic.IntValue)
                {
                    case 2:
                        return GF_2;
                    case 3:
                        return GF_3;
                }
            }
            return new PrimeField( characteristic );
        }
    }
}
