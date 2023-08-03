// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.TeleTrust.TeleTrusTObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.TeleTrust
{
    public sealed class TeleTrusTObjectIdentifiers
    {
        public static readonly DerObjectIdentifier TeleTrusTAlgorithm = new( "1.3.36.3" );
        public static readonly DerObjectIdentifier RipeMD160 = new( TeleTrusTAlgorithm.ToString() + ".2.1" );
        public static readonly DerObjectIdentifier RipeMD128 = new( TeleTrusTAlgorithm.ToString() + ".2.2" );
        public static readonly DerObjectIdentifier RipeMD256 = new( TeleTrusTAlgorithm.ToString() + ".2.3" );
        public static readonly DerObjectIdentifier TeleTrusTRsaSignatureAlgorithm = new( TeleTrusTAlgorithm.ToString() + ".3.1" );
        public static readonly DerObjectIdentifier RsaSignatureWithRipeMD160 = new( TeleTrusTRsaSignatureAlgorithm.ToString() + ".2" );
        public static readonly DerObjectIdentifier RsaSignatureWithRipeMD128 = new( TeleTrusTRsaSignatureAlgorithm.ToString() + ".3" );
        public static readonly DerObjectIdentifier RsaSignatureWithRipeMD256 = new( TeleTrusTRsaSignatureAlgorithm.ToString() + ".4" );
        public static readonly DerObjectIdentifier ECSign = new( TeleTrusTAlgorithm.ToString() + ".3.2" );
        public static readonly DerObjectIdentifier ECSignWithSha1 = new( ECSign.ToString() + ".1" );
        public static readonly DerObjectIdentifier ECSignWithRipeMD160 = new( ECSign.ToString() + ".2" );
        public static readonly DerObjectIdentifier EccBrainpool = new( TeleTrusTAlgorithm.ToString() + ".3.2.8" );
        public static readonly DerObjectIdentifier EllipticCurve = new( EccBrainpool.ToString() + ".1" );
        public static readonly DerObjectIdentifier VersionOne = new( EllipticCurve.ToString() + ".1" );
        public static readonly DerObjectIdentifier BrainpoolP160R1 = new( VersionOne.ToString() + ".1" );
        public static readonly DerObjectIdentifier BrainpoolP160T1 = new( VersionOne.ToString() + ".2" );
        public static readonly DerObjectIdentifier BrainpoolP192R1 = new( VersionOne.ToString() + ".3" );
        public static readonly DerObjectIdentifier BrainpoolP192T1 = new( VersionOne.ToString() + ".4" );
        public static readonly DerObjectIdentifier BrainpoolP224R1 = new( VersionOne.ToString() + ".5" );
        public static readonly DerObjectIdentifier BrainpoolP224T1 = new( VersionOne.ToString() + ".6" );
        public static readonly DerObjectIdentifier BrainpoolP256R1 = new( VersionOne.ToString() + ".7" );
        public static readonly DerObjectIdentifier BrainpoolP256T1 = new( VersionOne.ToString() + ".8" );
        public static readonly DerObjectIdentifier BrainpoolP320R1 = new( VersionOne.ToString() + ".9" );
        public static readonly DerObjectIdentifier BrainpoolP320T1 = new( VersionOne.ToString() + ".10" );
        public static readonly DerObjectIdentifier BrainpoolP384R1 = new( VersionOne.ToString() + ".11" );
        public static readonly DerObjectIdentifier BrainpoolP384T1 = new( VersionOne.ToString() + ".12" );
        public static readonly DerObjectIdentifier BrainpoolP512R1 = new( VersionOne.ToString() + ".13" );
        public static readonly DerObjectIdentifier BrainpoolP512T1 = new( VersionOne.ToString() + ".14" );

        private TeleTrusTObjectIdentifiers()
        {
        }
    }
}
