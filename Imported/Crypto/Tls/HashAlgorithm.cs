// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.HashAlgorithm
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class HashAlgorithm
    {
        public const byte none = 0;
        public const byte md5 = 1;
        public const byte sha1 = 2;
        public const byte sha224 = 3;
        public const byte sha256 = 4;
        public const byte sha384 = 5;
        public const byte sha512 = 6;

        public static string GetName( byte hashAlgorithm )
        {
            switch (hashAlgorithm)
            {
                case 0:
                    return "none";
                case 1:
                    return "md5";
                case 2:
                    return "sha1";
                case 3:
                    return "sha224";
                case 4:
                    return "sha256";
                case 5:
                    return "sha384";
                case 6:
                    return "sha512";
                default:
                    return "UNKNOWN";
            }
        }

        public static string GetText( byte hashAlgorithm ) => GetName( hashAlgorithm ) + "(" + hashAlgorithm + ")";
    }
}
