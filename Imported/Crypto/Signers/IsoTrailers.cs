// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.IsoTrailers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class IsoTrailers
    {
        public const int TRAILER_IMPLICIT = 188;
        public const int TRAILER_RIPEMD160 = 12748;
        public const int TRAILER_RIPEMD128 = 13004;
        public const int TRAILER_SHA1 = 13260;
        public const int TRAILER_SHA256 = 13516;
        public const int TRAILER_SHA512 = 13772;
        public const int TRAILER_SHA384 = 14028;
        public const int TRAILER_WHIRLPOOL = 14284;
        public const int TRAILER_SHA224 = 14540;
        public const int TRAILER_SHA512_224 = 14796;
        public const int TRAILER_SHA512_256 = 16588;
        private static readonly IDictionary trailerMap = CreateTrailerMap();

        private static IDictionary CreateTrailerMap()
        {
            IDictionary hashtable = Platform.CreateHashtable();
            hashtable.Add( "RIPEMD128", 13004 );
            hashtable.Add( "RIPEMD160", 12748 );
            hashtable.Add( "SHA-1", 13260 );
            hashtable.Add( "SHA-224", 14540 );
            hashtable.Add( "SHA-256", 13516 );
            hashtable.Add( "SHA-384", 14028 );
            hashtable.Add( "SHA-512", 13772 );
            hashtable.Add( "SHA-512/224", 14796 );
            hashtable.Add( "SHA-512/256", 16588 );
            hashtable.Add( "Whirlpool", 14284 );
            return CollectionUtilities.ReadOnly( hashtable );
        }

        public static int GetTrailer( IDigest digest ) => (int)trailerMap[digest.AlgorithmName];

        public static bool NoTrailerAvailable( IDigest digest ) => !trailerMap.Contains( digest.AlgorithmName );
    }
}
