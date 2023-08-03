// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509ObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public abstract class X509ObjectIdentifiers
    {
        internal const string ID = "2.5.4";
        public static readonly DerObjectIdentifier CommonName = new( "2.5.4.3" );
        public static readonly DerObjectIdentifier CountryName = new( "2.5.4.6" );
        public static readonly DerObjectIdentifier LocalityName = new( "2.5.4.7" );
        public static readonly DerObjectIdentifier StateOrProvinceName = new( "2.5.4.8" );
        public static readonly DerObjectIdentifier Organization = new( "2.5.4.10" );
        public static readonly DerObjectIdentifier OrganizationalUnitName = new( "2.5.4.11" );
        public static readonly DerObjectIdentifier id_at_telephoneNumber = new( "2.5.4.20" );
        public static readonly DerObjectIdentifier id_at_name = new( "2.5.4.41" );
        public static readonly DerObjectIdentifier IdSha1 = new( "1.3.14.3.2.26" );
        public static readonly DerObjectIdentifier RipeMD160 = new( "1.3.36.3.2.1" );
        public static readonly DerObjectIdentifier RipeMD160WithRsaEncryption = new( "1.3.36.3.3.1.2" );
        public static readonly DerObjectIdentifier IdEARsa = new( "2.5.8.1.1" );
        public static readonly DerObjectIdentifier IdPkix = new( "1.3.6.1.5.5.7" );
        public static readonly DerObjectIdentifier IdPE = new( IdPkix.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdAD = new( IdPkix.ToString() + ".48" );
        public static readonly DerObjectIdentifier IdADCAIssuers = new( IdAD.ToString() + ".2" );
        public static readonly DerObjectIdentifier IdADOcsp = new( IdAD.ToString() + ".1" );
        public static readonly DerObjectIdentifier OcspAccessMethod = IdADOcsp;
        public static readonly DerObjectIdentifier CrlAccessMethod = IdADCAIssuers;
    }
}
