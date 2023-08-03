// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Enums
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Date;
using System;

namespace Org.BouncyCastle.Utilities
{
    internal abstract class Enums
    {
        internal static Enum GetEnumValue( Type enumType, string s )
        {
            if (!IsEnumType( enumType ))
                throw new ArgumentException( "Not an enumeration type", nameof( enumType ) );
            s = s.Length > 0 && char.IsLetter( s[0] ) && s.IndexOf( ',' ) < 0 ? s.Replace( '-', '_' ) : throw new ArgumentException();
            s = s.Replace( '/', '_' );
            return (Enum)Enum.Parse( enumType, s, false );
        }

        internal static Array GetEnumValues( Type enumType ) => IsEnumType( enumType ) ? Enum.GetValues( enumType ) : throw new ArgumentException( "Not an enumeration type", nameof( enumType ) );

        internal static Enum GetArbitraryValue( Type enumType )
        {
            Array enumValues = GetEnumValues( enumType );
            int index = (int)(DateTimeUtilities.CurrentUnixMs() & int.MaxValue) % enumValues.Length;
            return (Enum)enumValues.GetValue( index );
        }

        internal static bool IsEnumType( Type t ) => t.IsEnum;
    }
}
