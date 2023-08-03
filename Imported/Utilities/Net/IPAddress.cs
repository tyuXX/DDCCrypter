// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Net.IPAddress
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Globalization;

namespace Org.BouncyCastle.Utilities.Net
{
    public class IPAddress
    {
        public static bool IsValid( string address ) => IsValidIPv4( address ) || IsValidIPv6( address );

        public static bool IsValidWithNetMask( string address ) => IsValidIPv4WithNetmask( address ) || IsValidIPv6WithNetmask( address );

        public static bool IsValidIPv4( string address )
        {
            try
            {
                return unsafeIsValidIPv4( address );
            }
            catch (FormatException ex)
            {
            }
            catch (OverflowException ex)
            {
            }
            return false;
        }

        private static bool unsafeIsValidIPv4( string address )
        {
            if (address.Length == 0)
                return false;
            int num1 = 0;
            string str = address + ".";
            int startIndex = 0;
            int num2;
            while (startIndex < str.Length && (num2 = str.IndexOf( '.', startIndex )) > startIndex)
            {
                if (num1 == 4)
                    return false;
                int num3 = int.Parse( str.Substring( startIndex, num2 - startIndex ) );
                if (num3 < 0 || num3 > byte.MaxValue)
                    return false;
                startIndex = num2 + 1;
                ++num1;
            }
            return num1 == 4;
        }

        public static bool IsValidIPv4WithNetmask( string address )
        {
            int length = address.IndexOf( '/' );
            string str = address.Substring( length + 1 );
            if (length <= 0 || !IsValidIPv4( address.Substring( 0, length ) ))
                return false;
            return IsValidIPv4( str ) || IsMaskValue( str, 32 );
        }

        public static bool IsValidIPv6WithNetmask( string address )
        {
            int length = address.IndexOf( '/' );
            string str = address.Substring( length + 1 );
            if (length <= 0 || !IsValidIPv6( address.Substring( 0, length ) ))
                return false;
            return IsValidIPv6( str ) || IsMaskValue( str, 128 );
        }

        private static bool IsMaskValue( string component, int size )
        {
            int num = int.Parse( component );
            try
            {
                return num >= 0 && num <= size;
            }
            catch (FormatException ex)
            {
            }
            catch (OverflowException ex)
            {
            }
            return false;
        }

        public static bool IsValidIPv6( string address )
        {
            try
            {
                return unsafeIsValidIPv6( address );
            }
            catch (FormatException ex)
            {
            }
            catch (OverflowException ex)
            {
            }
            return false;
        }

        private static bool unsafeIsValidIPv6( string address )
        {
            if (address.Length == 0)
                return false;
            int num1 = 0;
            string str = address + ":";
            bool flag = false;
            int startIndex = 0;
            int num2;
            while (startIndex < str.Length && (num2 = str.IndexOf( ':', startIndex )) >= startIndex)
            {
                if (num1 == 8)
                    return false;
                if (startIndex != num2)
                {
                    string address1 = str.Substring( startIndex, num2 - startIndex );
                    if (num2 == str.Length - 1 && address1.IndexOf( '.' ) > 0)
                    {
                        if (!IsValidIPv4( address1 ))
                            return false;
                        ++num1;
                    }
                    else
                    {
                        int num3 = int.Parse( str.Substring( startIndex, num2 - startIndex ), NumberStyles.AllowHexSpecifier );
                        if (num3 < 0 || num3 > ushort.MaxValue)
                            return false;
                    }
                }
                else
                {
                    if (num2 != 1 && num2 != str.Length - 1 && flag)
                        return false;
                    flag = true;
                }
                startIndex = num2 + 1;
                ++num1;
            }
            return num1 == 8 || flag;
        }
    }
}
