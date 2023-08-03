// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.Srp.Srp6Utilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Agreement.Srp
{
    public class Srp6Utilities
    {
        public static BigInteger CalculateK( IDigest digest, BigInteger N, BigInteger g ) => HashPaddedPair( digest, N, N, g );

        public static BigInteger CalculateU( IDigest digest, BigInteger N, BigInteger A, BigInteger B ) => HashPaddedPair( digest, N, A, B );

        public static BigInteger CalculateX(
          IDigest digest,
          BigInteger N,
          byte[] salt,
          byte[] identity,
          byte[] password )
        {
            byte[] numArray = new byte[digest.GetDigestSize()];
            digest.BlockUpdate( identity, 0, identity.Length );
            digest.Update( 58 );
            digest.BlockUpdate( password, 0, password.Length );
            digest.DoFinal( numArray, 0 );
            digest.BlockUpdate( salt, 0, salt.Length );
            digest.BlockUpdate( numArray, 0, numArray.Length );
            digest.DoFinal( numArray, 0 );
            return new BigInteger( 1, numArray );
        }

        public static BigInteger GeneratePrivateValue(
          IDigest digest,
          BigInteger N,
          BigInteger g,
          SecureRandom random )
        {
            int num = System.Math.Min( 256, N.BitLength / 2 );
            return BigIntegers.CreateRandomInRange( BigInteger.One.ShiftLeft( num - 1 ), N.Subtract( BigInteger.One ), random );
        }

        public static BigInteger ValidatePublicValue( BigInteger N, BigInteger val )
        {
            val = val.Mod( N );
            return !val.Equals( BigInteger.Zero ) ? val : throw new CryptoException( "Invalid public value: 0" );
        }

        public static BigInteger CalculateM1(
          IDigest digest,
          BigInteger N,
          BigInteger A,
          BigInteger B,
          BigInteger S )
        {
            return HashPaddedTriplet( digest, N, A, B, S );
        }

        public static BigInteger CalculateM2(
          IDigest digest,
          BigInteger N,
          BigInteger A,
          BigInteger M1,
          BigInteger S )
        {
            return HashPaddedTriplet( digest, N, A, M1, S );
        }

        public static BigInteger CalculateKey( IDigest digest, BigInteger N, BigInteger S )
        {
            int length = (N.BitLength + 7) / 8;
            byte[] padded = GetPadded( S, length );
            digest.BlockUpdate( padded, 0, padded.Length );
            byte[] numArray = new byte[digest.GetDigestSize()];
            digest.DoFinal( numArray, 0 );
            return new BigInteger( 1, numArray );
        }

        private static BigInteger HashPaddedTriplet(
          IDigest digest,
          BigInteger N,
          BigInteger n1,
          BigInteger n2,
          BigInteger n3 )
        {
            int length = (N.BitLength + 7) / 8;
            byte[] padded1 = GetPadded( n1, length );
            byte[] padded2 = GetPadded( n2, length );
            byte[] padded3 = GetPadded( n3, length );
            digest.BlockUpdate( padded1, 0, padded1.Length );
            digest.BlockUpdate( padded2, 0, padded2.Length );
            digest.BlockUpdate( padded3, 0, padded3.Length );
            byte[] numArray = new byte[digest.GetDigestSize()];
            digest.DoFinal( numArray, 0 );
            return new BigInteger( 1, numArray );
        }

        private static BigInteger HashPaddedPair(
          IDigest digest,
          BigInteger N,
          BigInteger n1,
          BigInteger n2 )
        {
            int length = (N.BitLength + 7) / 8;
            byte[] padded1 = GetPadded( n1, length );
            byte[] padded2 = GetPadded( n2, length );
            digest.BlockUpdate( padded1, 0, padded1.Length );
            digest.BlockUpdate( padded2, 0, padded2.Length );
            byte[] numArray = new byte[digest.GetDigestSize()];
            digest.DoFinal( numArray, 0 );
            return new BigInteger( 1, numArray );
        }

        private static byte[] GetPadded( BigInteger n, int length )
        {
            byte[] sourceArray = BigIntegers.AsUnsignedByteArray( n );
            if (sourceArray.Length < length)
            {
                byte[] destinationArray = new byte[length];
                Array.Copy( sourceArray, 0, destinationArray, length - sourceArray.Length, sourceArray.Length );
                sourceArray = destinationArray;
            }
            return sourceArray;
        }
    }
}
