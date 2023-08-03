// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.PkcsObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public abstract class PkcsObjectIdentifiers
    {
        public const string Pkcs1 = "1.2.840.113549.1.1";
        public const string Pkcs3 = "1.2.840.113549.1.3";
        public const string Pkcs5 = "1.2.840.113549.1.5";
        public const string EncryptionAlgorithm = "1.2.840.113549.3";
        public const string DigestAlgorithm = "1.2.840.113549.2";
        public const string Pkcs7 = "1.2.840.113549.1.7";
        public const string Pkcs9 = "1.2.840.113549.1.9";
        public const string CertTypes = "1.2.840.113549.1.9.22";
        public const string CrlTypes = "1.2.840.113549.1.9.23";
        public const string IdCT = "1.2.840.113549.1.9.16.1";
        public const string IdCti = "1.2.840.113549.1.9.16.6";
        public const string IdAA = "1.2.840.113549.1.9.16.2";
        public const string IdSpq = "1.2.840.113549.1.9.16.5";
        public const string Pkcs12 = "1.2.840.113549.1.12";
        public const string BagTypes = "1.2.840.113549.1.12.10.1";
        public const string Pkcs12PbeIds = "1.2.840.113549.1.12.1";
        public static readonly DerObjectIdentifier RsaEncryption = new( "1.2.840.113549.1.1.1" );
        public static readonly DerObjectIdentifier MD2WithRsaEncryption = new( "1.2.840.113549.1.1.2" );
        public static readonly DerObjectIdentifier MD4WithRsaEncryption = new( "1.2.840.113549.1.1.3" );
        public static readonly DerObjectIdentifier MD5WithRsaEncryption = new( "1.2.840.113549.1.1.4" );
        public static readonly DerObjectIdentifier Sha1WithRsaEncryption = new( "1.2.840.113549.1.1.5" );
        public static readonly DerObjectIdentifier SrsaOaepEncryptionSet = new( "1.2.840.113549.1.1.6" );
        public static readonly DerObjectIdentifier IdRsaesOaep = new( "1.2.840.113549.1.1.7" );
        public static readonly DerObjectIdentifier IdMgf1 = new( "1.2.840.113549.1.1.8" );
        public static readonly DerObjectIdentifier IdPSpecified = new( "1.2.840.113549.1.1.9" );
        public static readonly DerObjectIdentifier IdRsassaPss = new( "1.2.840.113549.1.1.10" );
        public static readonly DerObjectIdentifier Sha256WithRsaEncryption = new( "1.2.840.113549.1.1.11" );
        public static readonly DerObjectIdentifier Sha384WithRsaEncryption = new( "1.2.840.113549.1.1.12" );
        public static readonly DerObjectIdentifier Sha512WithRsaEncryption = new( "1.2.840.113549.1.1.13" );
        public static readonly DerObjectIdentifier Sha224WithRsaEncryption = new( "1.2.840.113549.1.1.14" );
        public static readonly DerObjectIdentifier DhKeyAgreement = new( "1.2.840.113549.1.3.1" );
        public static readonly DerObjectIdentifier PbeWithMD2AndDesCbc = new( "1.2.840.113549.1.5.1" );
        public static readonly DerObjectIdentifier PbeWithMD2AndRC2Cbc = new( "1.2.840.113549.1.5.4" );
        public static readonly DerObjectIdentifier PbeWithMD5AndDesCbc = new( "1.2.840.113549.1.5.3" );
        public static readonly DerObjectIdentifier PbeWithMD5AndRC2Cbc = new( "1.2.840.113549.1.5.6" );
        public static readonly DerObjectIdentifier PbeWithSha1AndDesCbc = new( "1.2.840.113549.1.5.10" );
        public static readonly DerObjectIdentifier PbeWithSha1AndRC2Cbc = new( "1.2.840.113549.1.5.11" );
        public static readonly DerObjectIdentifier IdPbeS2 = new( "1.2.840.113549.1.5.13" );
        public static readonly DerObjectIdentifier IdPbkdf2 = new( "1.2.840.113549.1.5.12" );
        public static readonly DerObjectIdentifier DesEde3Cbc = new( "1.2.840.113549.3.7" );
        public static readonly DerObjectIdentifier RC2Cbc = new( "1.2.840.113549.3.2" );
        public static readonly DerObjectIdentifier MD2 = new( "1.2.840.113549.2.2" );
        public static readonly DerObjectIdentifier MD4 = new( "1.2.840.113549.2.4" );
        public static readonly DerObjectIdentifier MD5 = new( "1.2.840.113549.2.5" );
        public static readonly DerObjectIdentifier IdHmacWithSha1 = new( "1.2.840.113549.2.7" );
        public static readonly DerObjectIdentifier IdHmacWithSha224 = new( "1.2.840.113549.2.8" );
        public static readonly DerObjectIdentifier IdHmacWithSha256 = new( "1.2.840.113549.2.9" );
        public static readonly DerObjectIdentifier IdHmacWithSha384 = new( "1.2.840.113549.2.10" );
        public static readonly DerObjectIdentifier IdHmacWithSha512 = new( "1.2.840.113549.2.11" );
        public static readonly DerObjectIdentifier Data = new( "1.2.840.113549.1.7.1" );
        public static readonly DerObjectIdentifier SignedData = new( "1.2.840.113549.1.7.2" );
        public static readonly DerObjectIdentifier EnvelopedData = new( "1.2.840.113549.1.7.3" );
        public static readonly DerObjectIdentifier SignedAndEnvelopedData = new( "1.2.840.113549.1.7.4" );
        public static readonly DerObjectIdentifier DigestedData = new( "1.2.840.113549.1.7.5" );
        public static readonly DerObjectIdentifier EncryptedData = new( "1.2.840.113549.1.7.6" );
        public static readonly DerObjectIdentifier Pkcs9AtEmailAddress = new( "1.2.840.113549.1.9.1" );
        public static readonly DerObjectIdentifier Pkcs9AtUnstructuredName = new( "1.2.840.113549.1.9.2" );
        public static readonly DerObjectIdentifier Pkcs9AtContentType = new( "1.2.840.113549.1.9.3" );
        public static readonly DerObjectIdentifier Pkcs9AtMessageDigest = new( "1.2.840.113549.1.9.4" );
        public static readonly DerObjectIdentifier Pkcs9AtSigningTime = new( "1.2.840.113549.1.9.5" );
        public static readonly DerObjectIdentifier Pkcs9AtCounterSignature = new( "1.2.840.113549.1.9.6" );
        public static readonly DerObjectIdentifier Pkcs9AtChallengePassword = new( "1.2.840.113549.1.9.7" );
        public static readonly DerObjectIdentifier Pkcs9AtUnstructuredAddress = new( "1.2.840.113549.1.9.8" );
        public static readonly DerObjectIdentifier Pkcs9AtExtendedCertificateAttributes = new( "1.2.840.113549.1.9.9" );
        public static readonly DerObjectIdentifier Pkcs9AtSigningDescription = new( "1.2.840.113549.1.9.13" );
        public static readonly DerObjectIdentifier Pkcs9AtExtensionRequest = new( "1.2.840.113549.1.9.14" );
        public static readonly DerObjectIdentifier Pkcs9AtSmimeCapabilities = new( "1.2.840.113549.1.9.15" );
        public static readonly DerObjectIdentifier IdSmime = new( "1.2.840.113549.1.9.16" );
        public static readonly DerObjectIdentifier Pkcs9AtFriendlyName = new( "1.2.840.113549.1.9.20" );
        public static readonly DerObjectIdentifier Pkcs9AtLocalKeyID = new( "1.2.840.113549.1.9.21" );
        [Obsolete( "Use X509Certificate instead" )]
        public static readonly DerObjectIdentifier X509CertType = new( "1.2.840.113549.1.9.22.1" );
        public static readonly DerObjectIdentifier X509Certificate = new( "1.2.840.113549.1.9.22.1" );
        public static readonly DerObjectIdentifier SdsiCertificate = new( "1.2.840.113549.1.9.22.2" );
        public static readonly DerObjectIdentifier X509Crl = new( "1.2.840.113549.1.9.23.1" );
        public static readonly DerObjectIdentifier IdAlg = IdSmime.Branch( "3" );
        public static readonly DerObjectIdentifier IdAlgEsdh = IdAlg.Branch( "5" );
        public static readonly DerObjectIdentifier IdAlgCms3DesWrap = IdAlg.Branch( "6" );
        public static readonly DerObjectIdentifier IdAlgCmsRC2Wrap = IdAlg.Branch( "7" );
        public static readonly DerObjectIdentifier IdAlgPwriKek = IdAlg.Branch( "9" );
        public static readonly DerObjectIdentifier IdAlgSsdh = IdAlg.Branch( "10" );
        public static readonly DerObjectIdentifier IdRsaKem = IdAlg.Branch( "14" );
        public static readonly DerObjectIdentifier PreferSignedData = Pkcs9AtSmimeCapabilities.Branch( "1" );
        public static readonly DerObjectIdentifier CannotDecryptAny = Pkcs9AtSmimeCapabilities.Branch( "2" );
        public static readonly DerObjectIdentifier SmimeCapabilitiesVersions = Pkcs9AtSmimeCapabilities.Branch( "3" );
        public static readonly DerObjectIdentifier IdAAReceiptRequest = IdSmime.Branch( "2.1" );
        public static readonly DerObjectIdentifier IdCTAuthData = new( "1.2.840.113549.1.9.16.1.2" );
        public static readonly DerObjectIdentifier IdCTTstInfo = new( "1.2.840.113549.1.9.16.1.4" );
        public static readonly DerObjectIdentifier IdCTCompressedData = new( "1.2.840.113549.1.9.16.1.9" );
        public static readonly DerObjectIdentifier IdCTAuthEnvelopedData = new( "1.2.840.113549.1.9.16.1.23" );
        public static readonly DerObjectIdentifier IdCTTimestampedData = new( "1.2.840.113549.1.9.16.1.31" );
        public static readonly DerObjectIdentifier IdCtiEtsProofOfOrigin = new( "1.2.840.113549.1.9.16.6.1" );
        public static readonly DerObjectIdentifier IdCtiEtsProofOfReceipt = new( "1.2.840.113549.1.9.16.6.2" );
        public static readonly DerObjectIdentifier IdCtiEtsProofOfDelivery = new( "1.2.840.113549.1.9.16.6.3" );
        public static readonly DerObjectIdentifier IdCtiEtsProofOfSender = new( "1.2.840.113549.1.9.16.6.4" );
        public static readonly DerObjectIdentifier IdCtiEtsProofOfApproval = new( "1.2.840.113549.1.9.16.6.5" );
        public static readonly DerObjectIdentifier IdCtiEtsProofOfCreation = new( "1.2.840.113549.1.9.16.6.6" );
        public static readonly DerObjectIdentifier IdAAContentHint = new( "1.2.840.113549.1.9.16.2.4" );
        public static readonly DerObjectIdentifier IdAAMsgSigDigest = new( "1.2.840.113549.1.9.16.2.5" );
        public static readonly DerObjectIdentifier IdAAContentReference = new( "1.2.840.113549.1.9.16.2.10" );
        public static readonly DerObjectIdentifier IdAAEncrypKeyPref = new( "1.2.840.113549.1.9.16.2.11" );
        public static readonly DerObjectIdentifier IdAASigningCertificate = new( "1.2.840.113549.1.9.16.2.12" );
        public static readonly DerObjectIdentifier IdAASigningCertificateV2 = new( "1.2.840.113549.1.9.16.2.47" );
        public static readonly DerObjectIdentifier IdAAContentIdentifier = new( "1.2.840.113549.1.9.16.2.7" );
        public static readonly DerObjectIdentifier IdAASignatureTimeStampToken = new( "1.2.840.113549.1.9.16.2.14" );
        public static readonly DerObjectIdentifier IdAAEtsSigPolicyID = new( "1.2.840.113549.1.9.16.2.15" );
        public static readonly DerObjectIdentifier IdAAEtsCommitmentType = new( "1.2.840.113549.1.9.16.2.16" );
        public static readonly DerObjectIdentifier IdAAEtsSignerLocation = new( "1.2.840.113549.1.9.16.2.17" );
        public static readonly DerObjectIdentifier IdAAEtsSignerAttr = new( "1.2.840.113549.1.9.16.2.18" );
        public static readonly DerObjectIdentifier IdAAEtsOtherSigCert = new( "1.2.840.113549.1.9.16.2.19" );
        public static readonly DerObjectIdentifier IdAAEtsContentTimestamp = new( "1.2.840.113549.1.9.16.2.20" );
        public static readonly DerObjectIdentifier IdAAEtsCertificateRefs = new( "1.2.840.113549.1.9.16.2.21" );
        public static readonly DerObjectIdentifier IdAAEtsRevocationRefs = new( "1.2.840.113549.1.9.16.2.22" );
        public static readonly DerObjectIdentifier IdAAEtsCertValues = new( "1.2.840.113549.1.9.16.2.23" );
        public static readonly DerObjectIdentifier IdAAEtsRevocationValues = new( "1.2.840.113549.1.9.16.2.24" );
        public static readonly DerObjectIdentifier IdAAEtsEscTimeStamp = new( "1.2.840.113549.1.9.16.2.25" );
        public static readonly DerObjectIdentifier IdAAEtsCertCrlTimestamp = new( "1.2.840.113549.1.9.16.2.26" );
        public static readonly DerObjectIdentifier IdAAEtsArchiveTimestamp = new( "1.2.840.113549.1.9.16.2.27" );
        [Obsolete( "Use 'IdAAEtsSigPolicyID' instead" )]
        public static readonly DerObjectIdentifier IdAASigPolicyID = IdAAEtsSigPolicyID;
        [Obsolete( "Use 'IdAAEtsCommitmentType' instead" )]
        public static readonly DerObjectIdentifier IdAACommitmentType = IdAAEtsCommitmentType;
        [Obsolete( "Use 'IdAAEtsSignerLocation' instead" )]
        public static readonly DerObjectIdentifier IdAASignerLocation = IdAAEtsSignerLocation;
        [Obsolete( "Use 'IdAAEtsOtherSigCert' instead" )]
        public static readonly DerObjectIdentifier IdAAOtherSigCert = IdAAEtsOtherSigCert;
        public static readonly DerObjectIdentifier IdSpqEtsUri = new( "1.2.840.113549.1.9.16.5.1" );
        public static readonly DerObjectIdentifier IdSpqEtsUNotice = new( "1.2.840.113549.1.9.16.5.2" );
        public static readonly DerObjectIdentifier KeyBag = new( "1.2.840.113549.1.12.10.1.1" );
        public static readonly DerObjectIdentifier Pkcs8ShroudedKeyBag = new( "1.2.840.113549.1.12.10.1.2" );
        public static readonly DerObjectIdentifier CertBag = new( "1.2.840.113549.1.12.10.1.3" );
        public static readonly DerObjectIdentifier CrlBag = new( "1.2.840.113549.1.12.10.1.4" );
        public static readonly DerObjectIdentifier SecretBag = new( "1.2.840.113549.1.12.10.1.5" );
        public static readonly DerObjectIdentifier SafeContentsBag = new( "1.2.840.113549.1.12.10.1.6" );
        public static readonly DerObjectIdentifier PbeWithShaAnd128BitRC4 = new( "1.2.840.113549.1.12.1.1" );
        public static readonly DerObjectIdentifier PbeWithShaAnd40BitRC4 = new( "1.2.840.113549.1.12.1.2" );
        public static readonly DerObjectIdentifier PbeWithShaAnd3KeyTripleDesCbc = new( "1.2.840.113549.1.12.1.3" );
        public static readonly DerObjectIdentifier PbeWithShaAnd2KeyTripleDesCbc = new( "1.2.840.113549.1.12.1.4" );
        public static readonly DerObjectIdentifier PbeWithShaAnd128BitRC2Cbc = new( "1.2.840.113549.1.12.1.5" );
        public static readonly DerObjectIdentifier PbewithShaAnd40BitRC2Cbc = new( "1.2.840.113549.1.12.1.6" );
    }
}
