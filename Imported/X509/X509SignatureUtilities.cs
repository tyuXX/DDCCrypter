// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509SignatureUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;

namespace Org.BouncyCastle.X509
{
    internal class X509SignatureUtilities
    {
        private static readonly Asn1Null derNull = DerNull.Instance;

        internal static void SetSignatureParameters( ISigner signature, Asn1Encodable parameters )
        {
            if (parameters == null)
                return;
            derNull.Equals( parameters );
        }

        internal static string GetSignatureName( AlgorithmIdentifier sigAlgId )
        {
            Asn1Encodable parameters = sigAlgId.Parameters;
            if (parameters != null && !derNull.Equals( parameters ))
            {
                if (sigAlgId.Algorithm.Equals( PkcsObjectIdentifiers.IdRsassaPss ))
                    return GetDigestAlgName( RsassaPssParameters.GetInstance( parameters ).HashAlgorithm.Algorithm ) + "withRSAandMGF1";
                if (sigAlgId.Algorithm.Equals( X9ObjectIdentifiers.ECDsaWithSha2 ))
                    return GetDigestAlgName( (DerObjectIdentifier)Asn1Sequence.GetInstance( parameters )[0] ) + "withECDSA";
            }
            return sigAlgId.Algorithm.Id;
        }

        private static string GetDigestAlgName( DerObjectIdentifier digestAlgOID )
        {
            if (PkcsObjectIdentifiers.MD5.Equals( digestAlgOID ))
                return "MD5";
            if (OiwObjectIdentifiers.IdSha1.Equals( digestAlgOID ))
                return "SHA1";
            if (NistObjectIdentifiers.IdSha224.Equals( digestAlgOID ))
                return "SHA224";
            if (NistObjectIdentifiers.IdSha256.Equals( digestAlgOID ))
                return "SHA256";
            if (NistObjectIdentifiers.IdSha384.Equals( digestAlgOID ))
                return "SHA384";
            if (NistObjectIdentifiers.IdSha512.Equals( digestAlgOID ))
                return "SHA512";
            if (TeleTrusTObjectIdentifiers.RipeMD128.Equals( digestAlgOID ))
                return "RIPEMD128";
            if (TeleTrusTObjectIdentifiers.RipeMD160.Equals( digestAlgOID ))
                return "RIPEMD160";
            if (TeleTrusTObjectIdentifiers.RipeMD256.Equals( digestAlgOID ))
                return "RIPEMD256";
            return CryptoProObjectIdentifiers.GostR3411.Equals( digestAlgOID ) ? "GOST3411" : digestAlgOID.Id;
        }
    }
}
