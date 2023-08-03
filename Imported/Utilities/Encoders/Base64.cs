// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Encoders.Base64
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
    public sealed class Base64
    {
        private Base64()
        {
        }

        public static string ToBase64String( byte[] data ) => Convert.ToBase64String( data, 0, data.Length );

        public static string ToBase64String( byte[] data, int off, int length ) => Convert.ToBase64String( data, off, length );

        public static byte[] Encode( byte[] data ) => Encode( data, 0, data.Length );

        public static byte[] Encode( byte[] data, int off, int length ) => Strings.ToAsciiByteArray( Convert.ToBase64String( data, off, length ) );

        public static int Encode( byte[] data, Stream outStream )
        {
            byte[] buffer = Encode( data );
            outStream.Write( buffer, 0, buffer.Length );
            return buffer.Length;
        }

        public static int Encode( byte[] data, int off, int length, Stream outStream )
        {
            byte[] buffer = Encode( data, off, length );
            outStream.Write( buffer, 0, buffer.Length );
            return buffer.Length;
        }

        public static byte[] Decode( byte[] data ) => Convert.FromBase64String( Strings.FromAsciiByteArray( data ) );

        public static byte[] Decode( string data ) => Convert.FromBase64String( data );

        public static int Decode( string data, Stream outStream )
        {
            byte[] buffer = Decode( data );
            outStream.Write( buffer, 0, buffer.Length );
            return buffer.Length;
        }
    }
}
