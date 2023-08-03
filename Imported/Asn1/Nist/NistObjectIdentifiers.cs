// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Nist.NistObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Nist
{
    public sealed class NistObjectIdentifiers
    {
        public static readonly DerObjectIdentifier NistAlgorithm = new( "2.16.840.1.101.3.4" );
        public static readonly DerObjectIdentifier HashAlgs = NistAlgorithm.Branch( "2" );
        public static readonly DerObjectIdentifier IdSha256 = HashAlgs.Branch( "1" );
        public static readonly DerObjectIdentifier IdSha384 = HashAlgs.Branch( "2" );
        public static readonly DerObjectIdentifier IdSha512 = HashAlgs.Branch( "3" );
        public static readonly DerObjectIdentifier IdSha224 = HashAlgs.Branch( "4" );
        public static readonly DerObjectIdentifier IdSha512_224 = HashAlgs.Branch( "5" );
        public static readonly DerObjectIdentifier IdSha512_256 = HashAlgs.Branch( "6" );
        public static readonly DerObjectIdentifier IdSha3_224 = HashAlgs.Branch( "7" );
        public static readonly DerObjectIdentifier IdSha3_256 = HashAlgs.Branch( "8" );
        public static readonly DerObjectIdentifier IdSha3_384 = HashAlgs.Branch( "9" );
        public static readonly DerObjectIdentifier IdSha3_512 = HashAlgs.Branch( "10" );
        public static readonly DerObjectIdentifier IdShake128 = HashAlgs.Branch( "11" );
        public static readonly DerObjectIdentifier IdShake256 = HashAlgs.Branch( "12" );
        public static readonly DerObjectIdentifier Aes = new( NistAlgorithm.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdAes128Ecb = new( Aes.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdAes128Cbc = new( Aes.ToString() + ".2" );
        public static readonly DerObjectIdentifier IdAes128Ofb = new( Aes.ToString() + ".3" );
        public static readonly DerObjectIdentifier IdAes128Cfb = new( Aes.ToString() + ".4" );
        public static readonly DerObjectIdentifier IdAes128Wrap = new( Aes.ToString() + ".5" );
        public static readonly DerObjectIdentifier IdAes128Gcm = new( Aes.ToString() + ".6" );
        public static readonly DerObjectIdentifier IdAes128Ccm = new( Aes.ToString() + ".7" );
        public static readonly DerObjectIdentifier IdAes192Ecb = new( Aes.ToString() + ".21" );
        public static readonly DerObjectIdentifier IdAes192Cbc = new( Aes.ToString() + ".22" );
        public static readonly DerObjectIdentifier IdAes192Ofb = new( Aes.ToString() + ".23" );
        public static readonly DerObjectIdentifier IdAes192Cfb = new( Aes.ToString() + ".24" );
        public static readonly DerObjectIdentifier IdAes192Wrap = new( Aes.ToString() + ".25" );
        public static readonly DerObjectIdentifier IdAes192Gcm = new( Aes.ToString() + ".26" );
        public static readonly DerObjectIdentifier IdAes192Ccm = new( Aes.ToString() + ".27" );
        public static readonly DerObjectIdentifier IdAes256Ecb = new( Aes.ToString() + ".41" );
        public static readonly DerObjectIdentifier IdAes256Cbc = new( Aes.ToString() + ".42" );
        public static readonly DerObjectIdentifier IdAes256Ofb = new( Aes.ToString() + ".43" );
        public static readonly DerObjectIdentifier IdAes256Cfb = new( Aes.ToString() + ".44" );
        public static readonly DerObjectIdentifier IdAes256Wrap = new( Aes.ToString() + ".45" );
        public static readonly DerObjectIdentifier IdAes256Gcm = new( Aes.ToString() + ".46" );
        public static readonly DerObjectIdentifier IdAes256Ccm = new( Aes.ToString() + ".47" );
        public static readonly DerObjectIdentifier IdDsaWithSha2 = new( NistAlgorithm.ToString() + ".3" );
        public static readonly DerObjectIdentifier DsaWithSha224 = new( IdDsaWithSha2.ToString() + ".1" );
        public static readonly DerObjectIdentifier DsaWithSha256 = new( IdDsaWithSha2.ToString() + ".2" );
        public static readonly DerObjectIdentifier DsaWithSha384 = new( IdDsaWithSha2.ToString() + ".3" );
        public static readonly DerObjectIdentifier DsaWithSha512 = new( IdDsaWithSha2.ToString() + ".4" );

        private NistObjectIdentifiers()
        {
        }
    }
}
