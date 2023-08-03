// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.BigIntegers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Utilities
{
    public abstract class BigIntegers
    {
        private const int MaxIterations = 1000;

        public static byte[] AsUnsignedByteArray( BigInteger n ) => n.ToByteArrayUnsigned();

        public static byte[] AsUnsignedByteArray( int length, BigInteger n )
        {
            byte[] byteArrayUnsigned = n.ToByteArrayUnsigned();
            if (byteArrayUnsigned.Length > length)
                throw new ArgumentException( "standard length exceeded", nameof( n ) );
            if (byteArrayUnsigned.Length == length)
                return byteArrayUnsigned;
            byte[] destinationArray = new byte[length];
            Array.Copy( byteArrayUnsigned, 0, destinationArray, destinationArray.Length - byteArrayUnsigned.Length, byteArrayUnsigned.Length );
            return destinationArray;
        }

        public static BigInteger CreateRandomInRange(
          BigInteger min,
          BigInteger max,
          SecureRandom random )
        {
            int num = min.CompareTo( max );
            if (num >= 0)
            {
                if (num > 0)
                    throw new ArgumentException( "'min' may not be greater than 'max'" );
                return min;
            }
            if (min.BitLength > max.BitLength / 2)
                return CreateRandomInRange( BigInteger.Zero, max.Subtract( min ), random ).Add( min );
            for (int index = 0; index < 1000; ++index)
            {
                BigInteger randomInRange = new( max.BitLength, random );
                if (randomInRange.CompareTo( min ) >= 0 && randomInRange.CompareTo( max ) <= 0)
                    return randomInRange;
            }
            return new BigInteger( max.Subtract( min ).BitLength - 1, random ).Add( min );
        }
    }
}
