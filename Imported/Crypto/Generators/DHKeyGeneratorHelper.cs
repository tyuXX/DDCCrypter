// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DHKeyGeneratorHelper
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    internal class DHKeyGeneratorHelper
    {
        internal static readonly DHKeyGeneratorHelper Instance = new DHKeyGeneratorHelper();

        private DHKeyGeneratorHelper()
        {
        }

        internal BigInteger CalculatePrivate( DHParameters dhParams, SecureRandom random )
        {
            int l = dhParams.L;
            if (l != 0)
            {
                int num = l >> 2;
                BigInteger k;
                do
                {
                    k = new BigInteger( l, random ).SetBit( l - 1 );
                }
                while (WNafUtilities.GetNafWeight( k ) < num);
                return k;
            }
            BigInteger min = BigInteger.Two;
            int m = dhParams.M;
            if (m != 0)
                min = BigInteger.One.ShiftLeft( m - 1 );
            BigInteger max = (dhParams.Q ?? dhParams.P).Subtract( BigInteger.Two );
            int num1 = max.BitLength >> 2;
            BigInteger randomInRange;
            do
            {
                randomInRange = BigIntegers.CreateRandomInRange( min, max, random );
            }
            while (WNafUtilities.GetNafWeight( randomInRange ) < num1);
            return randomInRange;
        }

        internal BigInteger CalculatePublic( DHParameters dhParams, BigInteger x ) => dhParams.G.ModPow( x, dhParams.P );
    }
}
