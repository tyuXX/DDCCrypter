﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat320
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat320
    {
        public static void Copy64( ulong[] x, ulong[] z )
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
        }

        public static ulong[] Create64() => new ulong[5];

        public static ulong[] CreateExt64() => new ulong[10];

        public static bool Eq64( ulong[] x, ulong[] y )
        {
            for (int index = 4; index >= 0; --index)
            {
                if ((long)x[index] != (long)y[index])
                    return false;
            }
            return true;
        }

        public static ulong[] FromBigInteger64( BigInteger x )
        {
            if (x.SignValue < 0 || x.BitLength > 320)
                throw new ArgumentException();
            ulong[] numArray = Create64();
            int num = 0;
            for (; x.SignValue != 0; x = x.ShiftRight( 64 ))
                numArray[num++] = (ulong)x.LongValue;
            return numArray;
        }

        public static bool IsOne64( ulong[] x )
        {
            if (x[0] != 1UL)
                return false;
            for (int index = 1; index < 5; ++index)
            {
                if (x[index] != 0UL)
                    return false;
            }
            return true;
        }

        public static bool IsZero64( ulong[] x )
        {
            for (int index = 0; index < 5; ++index)
            {
                if (x[index] != 0UL)
                    return false;
            }
            return true;
        }

        public static BigInteger ToBigInteger64( ulong[] x )
        {
            byte[] numArray = new byte[40];
            for (int index = 0; index < 5; ++index)
            {
                ulong n = x[index];
                if (n != 0UL)
                    Pack.UInt64_To_BE( n, numArray, (4 - index) << 3 );
            }
            return new BigInteger( 1, numArray );
        }
    }
}
