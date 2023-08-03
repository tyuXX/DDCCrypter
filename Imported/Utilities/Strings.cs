// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Strings
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Text;

namespace Org.BouncyCastle.Utilities
{
    public abstract class Strings
    {
        internal static bool IsOneOf( string s, params string[] candidates )
        {
            foreach (string candidate in candidates)
            {
                if (s == candidate)
                    return true;
            }
            return false;
        }

        public static string FromByteArray( byte[] bs )
        {
            char[] chArray = new char[bs.Length];
            for (int index = 0; index < chArray.Length; ++index)
                chArray[index] = Convert.ToChar( bs[index] );
            return new string( chArray );
        }

        public static byte[] ToByteArray( char[] cs )
        {
            byte[] byteArray = new byte[cs.Length];
            for (int index = 0; index < byteArray.Length; ++index)
                byteArray[index] = Convert.ToByte( cs[index] );
            return byteArray;
        }

        public static byte[] ToByteArray( string s )
        {
            byte[] byteArray = new byte[s.Length];
            for (int index = 0; index < byteArray.Length; ++index)
                byteArray[index] = Convert.ToByte( s[index] );
            return byteArray;
        }

        public static string FromAsciiByteArray( byte[] bytes ) => Encoding.ASCII.GetString( bytes, 0, bytes.Length );

        public static byte[] ToAsciiByteArray( char[] cs ) => Encoding.ASCII.GetBytes( cs );

        public static byte[] ToAsciiByteArray( string s ) => Encoding.ASCII.GetBytes( s );

        public static string FromUtf8ByteArray( byte[] bytes ) => Encoding.UTF8.GetString( bytes, 0, bytes.Length );

        public static byte[] ToUtf8ByteArray( char[] cs ) => Encoding.UTF8.GetBytes( cs );

        public static byte[] ToUtf8ByteArray( string s ) => Encoding.UTF8.GetBytes( s );
    }
}
