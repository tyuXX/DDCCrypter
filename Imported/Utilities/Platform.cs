// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Platform
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;

namespace Org.BouncyCastle.Utilities
{
    internal abstract class Platform
    {
        private static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;
        internal static readonly string NewLine = GetNewLine();

        private static string GetNewLine() => Environment.NewLine;

        internal static bool EqualsIgnoreCase( string a, string b ) => ToUpperInvariant( a ) == ToUpperInvariant( b );

        internal static string GetEnvironmentVariable( string variable )
        {
            try
            {
                return Environment.GetEnvironmentVariable( variable );
            }
            catch (SecurityException ex)
            {
                return null;
            }
        }

        internal static Exception CreateNotImplementedException( string message ) => new NotImplementedException( message );

        internal static IList CreateArrayList() => new ArrayList();

        internal static IList CreateArrayList( int capacity ) => new ArrayList( capacity );

        internal static IList CreateArrayList( ICollection collection ) => new ArrayList( collection );

        internal static IList CreateArrayList( IEnumerable collection )
        {
            ArrayList arrayList = new();
            foreach (object obj in collection)
                arrayList.Add( obj );
            return arrayList;
        }

        internal static IDictionary CreateHashtable() => new Hashtable();

        internal static IDictionary CreateHashtable( int capacity ) => new Hashtable( capacity );

        internal static IDictionary CreateHashtable( IDictionary dictionary ) => new Hashtable( dictionary );

        internal static string ToLowerInvariant( string s ) => s.ToLower( CultureInfo.InvariantCulture );

        internal static string ToUpperInvariant( string s ) => s.ToUpper( CultureInfo.InvariantCulture );

        internal static void Dispose( Stream s ) => s.Close();

        internal static void Dispose( TextWriter t ) => t.Close();

        internal static int IndexOf( string source, string value ) => InvariantCompareInfo.IndexOf( source, value, CompareOptions.Ordinal );

        internal static int LastIndexOf( string source, string value ) => InvariantCompareInfo.LastIndexOf( source, value, CompareOptions.Ordinal );

        internal static bool StartsWith( string source, string prefix ) => InvariantCompareInfo.IsPrefix( source, prefix, CompareOptions.Ordinal );

        internal static bool EndsWith( string source, string suffix ) => InvariantCompareInfo.IsSuffix( source, suffix, CompareOptions.Ordinal );

        internal static string GetTypeName( object obj ) => obj.GetType().FullName;
    }
}
