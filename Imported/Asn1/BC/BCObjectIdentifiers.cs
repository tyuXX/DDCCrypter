// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BC.BCObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.BC
{
    public abstract class BCObjectIdentifiers
    {
        public static readonly DerObjectIdentifier bc = new DerObjectIdentifier( "1.3.6.1.4.1.22554" );
        public static readonly DerObjectIdentifier bc_pbe = new DerObjectIdentifier( bc.ToString() + ".1" );
        public static readonly DerObjectIdentifier bc_pbe_sha1 = new DerObjectIdentifier( bc_pbe.ToString() + ".1" );
        public static readonly DerObjectIdentifier bc_pbe_sha256 = new DerObjectIdentifier( bc_pbe.ToString() + ".2.1" );
        public static readonly DerObjectIdentifier bc_pbe_sha384 = new DerObjectIdentifier( bc_pbe.ToString() + ".2.2" );
        public static readonly DerObjectIdentifier bc_pbe_sha512 = new DerObjectIdentifier( bc_pbe.ToString() + ".2.3" );
        public static readonly DerObjectIdentifier bc_pbe_sha224 = new DerObjectIdentifier( bc_pbe.ToString() + ".2.4" );
        public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs5 = new DerObjectIdentifier( bc_pbe_sha1.ToString() + ".1" );
        public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12 = new DerObjectIdentifier( bc_pbe_sha1.ToString() + ".2" );
        public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs5 = new DerObjectIdentifier( bc_pbe_sha256.ToString() + ".1" );
        public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12 = new DerObjectIdentifier( bc_pbe_sha256.ToString() + ".2" );
        public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes128_cbc = new DerObjectIdentifier( bc_pbe_sha1_pkcs12.ToString() + ".1.2" );
        public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes192_cbc = new DerObjectIdentifier( bc_pbe_sha1_pkcs12.ToString() + ".1.22" );
        public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes256_cbc = new DerObjectIdentifier( bc_pbe_sha1_pkcs12.ToString() + ".1.42" );
        public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes128_cbc = new DerObjectIdentifier( bc_pbe_sha256_pkcs12.ToString() + ".1.2" );
        public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes192_cbc = new DerObjectIdentifier( bc_pbe_sha256_pkcs12.ToString() + ".1.22" );
        public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes256_cbc = new DerObjectIdentifier( bc_pbe_sha256_pkcs12.ToString() + ".1.42" );
    }
}
