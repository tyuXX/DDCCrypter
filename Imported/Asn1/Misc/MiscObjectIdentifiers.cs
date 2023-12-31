﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Misc.MiscObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Misc
{
    public abstract class MiscObjectIdentifiers
    {
        public static readonly DerObjectIdentifier Netscape = new( "2.16.840.1.113730.1" );
        public static readonly DerObjectIdentifier NetscapeCertType = Netscape.Branch( "1" );
        public static readonly DerObjectIdentifier NetscapeBaseUrl = Netscape.Branch( "2" );
        public static readonly DerObjectIdentifier NetscapeRevocationUrl = Netscape.Branch( "3" );
        public static readonly DerObjectIdentifier NetscapeCARevocationUrl = Netscape.Branch( "4" );
        public static readonly DerObjectIdentifier NetscapeRenewalUrl = Netscape.Branch( "7" );
        public static readonly DerObjectIdentifier NetscapeCAPolicyUrl = Netscape.Branch( "8" );
        public static readonly DerObjectIdentifier NetscapeSslServerName = Netscape.Branch( "12" );
        public static readonly DerObjectIdentifier NetscapeCertComment = Netscape.Branch( "13" );
        public static readonly DerObjectIdentifier Verisign = new( "2.16.840.1.113733.1" );
        public static readonly DerObjectIdentifier VerisignCzagExtension = Verisign.Branch( "6.3" );
        public static readonly DerObjectIdentifier VerisignPrivate_6_9 = Verisign.Branch( "6.9" );
        public static readonly DerObjectIdentifier VerisignOnSiteJurisdictionHash = Verisign.Branch( "6.11" );
        public static readonly DerObjectIdentifier VerisignBitString_6_13 = Verisign.Branch( "6.13" );
        public static readonly DerObjectIdentifier VerisignDnbDunsNumber = Verisign.Branch( "6.15" );
        public static readonly DerObjectIdentifier VerisignIssStrongCrypto = Verisign.Branch( "8.1" );
        public static readonly string Novell = "2.16.840.1.113719";
        public static readonly DerObjectIdentifier NovellSecurityAttribs = new( Novell + ".1.9.4.1" );
        public static readonly string Entrust = "1.2.840.113533.7";
        public static readonly DerObjectIdentifier EntrustVersionExtension = new( Entrust + ".65.0" );
        public static readonly DerObjectIdentifier as_sys_sec_alg_ideaCBC = new( "1.3.6.1.4.1.188.7.1.1.2" );
        public static readonly DerObjectIdentifier cryptlib = new( "1.3.6.1.4.1.3029" );
        public static readonly DerObjectIdentifier cryptlib_algorithm = cryptlib.Branch( "1" );
        public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_ECB = cryptlib_algorithm.Branch( "1.1" );
        public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_CBC = cryptlib_algorithm.Branch( "1.2" );
        public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_CFB = cryptlib_algorithm.Branch( "1.3" );
        public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_OFB = cryptlib_algorithm.Branch( "1.4" );
        public static readonly DerObjectIdentifier blake2 = new( "1.3.6.1.4.1.1722.12.2" );
        public static readonly DerObjectIdentifier id_blake2b160 = blake2.Branch( "1.5" );
        public static readonly DerObjectIdentifier id_blake2b256 = blake2.Branch( "1.8" );
        public static readonly DerObjectIdentifier id_blake2b384 = blake2.Branch( "1.12" );
        public static readonly DerObjectIdentifier id_blake2b512 = blake2.Branch( "1.16" );
    }
}
