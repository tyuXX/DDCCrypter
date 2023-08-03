// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Arrays
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Utilities
{
    public abstract class Arrays
    {
        public static bool AreEqual( bool[] a, bool[] b )
        {
            if (a == b)
                return true;
            return a != null && b != null && HaveSameContents( a, b );
        }

        public static bool AreEqual( char[] a, char[] b )
        {
            if (a == b)
                return true;
            return a != null && b != null && HaveSameContents( a, b );
        }

        public static bool AreEqual( byte[] a, byte[] b )
        {
            if (a == b)
                return true;
            return a != null && b != null && HaveSameContents( a, b );
        }

        [Obsolete( "Use 'AreEqual' method instead" )]
        public static bool AreSame( byte[] a, byte[] b ) => AreEqual( a, b );

        public static bool ConstantTimeAreEqual( byte[] a, byte[] b )
        {
            int length = a.Length;
            if (length != b.Length)
                return false;
            int num = 0;
            while (length != 0)
            {
                --length;
                num |= a[length] ^ b[length];
            }
            return num == 0;
        }

        public static bool AreEqual( int[] a, int[] b )
        {
            if (a == b)
                return true;
            return a != null && b != null && HaveSameContents( a, b );
        }

        [CLSCompliant( false )]
        public static bool AreEqual( uint[] a, uint[] b )
        {
            if (a == b)
                return true;
            return a != null && b != null && HaveSameContents( a, b );
        }

        private static bool HaveSameContents( bool[] a, bool[] b )
        {
            int length = a.Length;
            if (length != b.Length)
                return false;
            while (length != 0)
            {
                --length;
                if (a[length] != b[length])
                    return false;
            }
            return true;
        }

        private static bool HaveSameContents( char[] a, char[] b )
        {
            int length = a.Length;
            if (length != b.Length)
                return false;
            while (length != 0)
            {
                --length;
                if (a[length] != b[length])
                    return false;
            }
            return true;
        }

        private static bool HaveSameContents( byte[] a, byte[] b )
        {
            int length = a.Length;
            if (length != b.Length)
                return false;
            while (length != 0)
            {
                --length;
                if (a[length] != b[length])
                    return false;
            }
            return true;
        }

        private static bool HaveSameContents( int[] a, int[] b )
        {
            int length = a.Length;
            if (length != b.Length)
                return false;
            while (length != 0)
            {
                --length;
                if (a[length] != b[length])
                    return false;
            }
            return true;
        }

        private static bool HaveSameContents( uint[] a, uint[] b )
        {
            int length = a.Length;
            if (length != b.Length)
                return false;
            while (length != 0)
            {
                --length;
                if ((int)a[length] != (int)b[length])
                    return false;
            }
            return true;
        }

        public static string ToString( object[] a )
        {
            StringBuilder stringBuilder = new( 91 );
            if (a.Length > 0)
            {
                stringBuilder.Append( a[0] );
                for (int index = 1; index < a.Length; ++index)
                    stringBuilder.Append( ", " ).Append( a[index] );
            }
            stringBuilder.Append( ']' );
            return stringBuilder.ToString();
        }

        public static int GetHashCode( byte[] data )
        {
            if (data == null)
                return 0;
            int length = data.Length;
            int hashCode = length + 1;
            while (--length >= 0)
                hashCode = (hashCode * 257) ^ data[length];
            return hashCode;
        }

        public static int GetHashCode( byte[] data, int off, int len )
        {
            if (data == null)
                return 0;
            int num = len;
            int hashCode = num + 1;
            while (--num >= 0)
                hashCode = (hashCode * 257) ^ data[off + num];
            return hashCode;
        }

        public static int GetHashCode( int[] data )
        {
            if (data == null)
                return 0;
            int length = data.Length;
            int hashCode = length + 1;
            while (--length >= 0)
                hashCode = (hashCode * 257) ^ data[length];
            return hashCode;
        }

        public static int GetHashCode( int[] data, int off, int len )
        {
            if (data == null)
                return 0;
            int num = len;
            int hashCode = num + 1;
            while (--num >= 0)
                hashCode = (hashCode * 257) ^ data[off + num];
            return hashCode;
        }

        [CLSCompliant( false )]
        public static int GetHashCode( uint[] data )
        {
            if (data == null)
                return 0;
            int length = data.Length;
            int hashCode = length + 1;
            while (--length >= 0)
                hashCode = (hashCode * 257) ^ (int)data[length];
            return hashCode;
        }

        [CLSCompliant( false )]
        public static int GetHashCode( uint[] data, int off, int len )
        {
            if (data == null)
                return 0;
            int num = len;
            int hashCode = num + 1;
            while (--num >= 0)
                hashCode = (hashCode * 257) ^ (int)data[off + num];
            return hashCode;
        }

        [CLSCompliant( false )]
        public static int GetHashCode( ulong[] data )
        {
            if (data == null)
                return 0;
            int length = data.Length;
            int hashCode = length + 1;
            while (--length >= 0)
            {
                ulong num = data[length];
                hashCode = (((hashCode * 257) ^ (int)num) * 257) ^ (int)(num >> 32);
            }
            return hashCode;
        }

        [CLSCompliant( false )]
        public static int GetHashCode( ulong[] data, int off, int len )
        {
            if (data == null)
                return 0;
            int num1 = len;
            int hashCode = num1 + 1;
            while (--num1 >= 0)
            {
                ulong num2 = data[off + num1];
                hashCode = (((hashCode * 257) ^ (int)num2) * 257) ^ (int)(num2 >> 32);
            }
            return hashCode;
        }

        public static byte[] Clone( byte[] data ) => data != null ? (byte[])data.Clone() : null;

        public static byte[] Clone( byte[] data, byte[] existing )
        {
            if (data == null)
                return null;
            if (existing == null || existing.Length != data.Length)
                return Clone( data );
            Array.Copy( data, 0, existing, 0, existing.Length );
            return existing;
        }

        public static int[] Clone( int[] data ) => data != null ? (int[])data.Clone() : null;

        internal static uint[] Clone( uint[] data ) => data != null ? (uint[])data.Clone() : null;

        public static long[] Clone( long[] data ) => data != null ? (long[])data.Clone() : null;

        [CLSCompliant( false )]
        public static ulong[] Clone( ulong[] data ) => data != null ? (ulong[])data.Clone() : null;

        [CLSCompliant( false )]
        public static ulong[] Clone( ulong[] data, ulong[] existing )
        {
            if (data == null)
                return null;
            if (existing == null || existing.Length != data.Length)
                return Clone( data );
            Array.Copy( data, 0, existing, 0, existing.Length );
            return existing;
        }

        public static bool Contains( byte[] a, byte n )
        {
            for (int index = 0; index < a.Length; ++index)
            {
                if (a[index] == n)
                    return true;
            }
            return false;
        }

        public static bool Contains( short[] a, short n )
        {
            for (int index = 0; index < a.Length; ++index)
            {
                if (a[index] == n)
                    return true;
            }
            return false;
        }

        public static bool Contains( int[] a, int n )
        {
            for (int index = 0; index < a.Length; ++index)
            {
                if (a[index] == n)
                    return true;
            }
            return false;
        }

        public static void Fill( byte[] buf, byte b )
        {
            int length = buf.Length;
            while (length > 0)
                buf[--length] = b;
        }

        public static byte[] CopyOf( byte[] data, int newLength )
        {
            byte[] destinationArray = new byte[newLength];
            Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
            return destinationArray;
        }

        public static char[] CopyOf( char[] data, int newLength )
        {
            char[] destinationArray = new char[newLength];
            Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
            return destinationArray;
        }

        public static int[] CopyOf( int[] data, int newLength )
        {
            int[] destinationArray = new int[newLength];
            Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
            return destinationArray;
        }

        public static long[] CopyOf( long[] data, int newLength )
        {
            long[] destinationArray = new long[newLength];
            Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
            return destinationArray;
        }

        public static BigInteger[] CopyOf( BigInteger[] data, int newLength )
        {
            BigInteger[] destinationArray = new BigInteger[newLength];
            Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
            return destinationArray;
        }

        public static byte[] CopyOfRange( byte[] data, int from, int to )
        {
            int length = GetLength( from, to );
            byte[] destinationArray = new byte[length];
            Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
            return destinationArray;
        }

        public static int[] CopyOfRange( int[] data, int from, int to )
        {
            int length = GetLength( from, to );
            int[] destinationArray = new int[length];
            Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
            return destinationArray;
        }

        public static long[] CopyOfRange( long[] data, int from, int to )
        {
            int length = GetLength( from, to );
            long[] destinationArray = new long[length];
            Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
            return destinationArray;
        }

        public static BigInteger[] CopyOfRange( BigInteger[] data, int from, int to )
        {
            int length = GetLength( from, to );
            BigInteger[] destinationArray = new BigInteger[length];
            Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
            return destinationArray;
        }

        private static int GetLength( int from, int to )
        {
            int num = to - from;
            return num >= 0 ? num : throw new ArgumentException( from.ToString() + " > " + to );
        }

        public static byte[] Append( byte[] a, byte b )
        {
            if (a == null)
                return new byte[1] { b };
            int length = a.Length;
            byte[] destinationArray = new byte[length + 1];
            Array.Copy( a, 0, destinationArray, 0, length );
            destinationArray[length] = b;
            return destinationArray;
        }

        public static short[] Append( short[] a, short b )
        {
            if (a == null)
                return new short[1] { b };
            int length = a.Length;
            short[] destinationArray = new short[length + 1];
            Array.Copy( a, 0, destinationArray, 0, length );
            destinationArray[length] = b;
            return destinationArray;
        }

        public static int[] Append( int[] a, int b )
        {
            if (a == null)
                return new int[1] { b };
            int length = a.Length;
            int[] destinationArray = new int[length + 1];
            Array.Copy( a, 0, destinationArray, 0, length );
            destinationArray[length] = b;
            return destinationArray;
        }

        public static byte[] Concatenate( byte[] a, byte[] b )
        {
            if (a == null)
                return Clone( b );
            if (b == null)
                return Clone( a );
            byte[] destinationArray = new byte[a.Length + b.Length];
            Array.Copy( a, 0, destinationArray, 0, a.Length );
            Array.Copy( b, 0, destinationArray, a.Length, b.Length );
            return destinationArray;
        }

        public static byte[] ConcatenateAll( params byte[][] vs )
        {
            byte[][] numArray = new byte[vs.Length][];
            int num = 0;
            int length = 0;
            for (int index = 0; index < vs.Length; ++index)
            {
                byte[] v = vs[index];
                if (v != null)
                {
                    numArray[num++] = v;
                    length += v.Length;
                }
            }
            byte[] destinationArray = new byte[length];
            int destinationIndex = 0;
            for (int index = 0; index < num; ++index)
            {
                byte[] sourceArray = numArray[index];
                Array.Copy( sourceArray, 0, destinationArray, destinationIndex, sourceArray.Length );
                destinationIndex += sourceArray.Length;
            }
            return destinationArray;
        }

        public static int[] Concatenate( int[] a, int[] b )
        {
            if (a == null)
                return Clone( b );
            if (b == null)
                return Clone( a );
            int[] destinationArray = new int[a.Length + b.Length];
            Array.Copy( a, 0, destinationArray, 0, a.Length );
            Array.Copy( b, 0, destinationArray, a.Length, b.Length );
            return destinationArray;
        }

        public static byte[] Prepend( byte[] a, byte b )
        {
            if (a == null)
                return new byte[1] { b };
            int length = a.Length;
            byte[] destinationArray = new byte[length + 1];
            Array.Copy( a, 0, destinationArray, 1, length );
            destinationArray[0] = b;
            return destinationArray;
        }

        public static short[] Prepend( short[] a, short b )
        {
            if (a == null)
                return new short[1] { b };
            int length = a.Length;
            short[] destinationArray = new short[length + 1];
            Array.Copy( a, 0, destinationArray, 1, length );
            destinationArray[0] = b;
            return destinationArray;
        }

        public static int[] Prepend( int[] a, int b )
        {
            if (a == null)
                return new int[1] { b };
            int length = a.Length;
            int[] destinationArray = new int[length + 1];
            Array.Copy( a, 0, destinationArray, 1, length );
            destinationArray[0] = b;
            return destinationArray;
        }

        public static byte[] Reverse( byte[] a )
        {
            if (a == null)
                return null;
            int num = 0;
            int length = a.Length;
            byte[] numArray = new byte[length];
            while (--length >= 0)
                numArray[length] = a[num++];
            return numArray;
        }

        public static int[] Reverse( int[] a )
        {
            if (a == null)
                return null;
            int num = 0;
            int length = a.Length;
            int[] numArray = new int[length];
            while (--length >= 0)
                numArray[length] = a[num++];
            return numArray;
        }
    }
}
