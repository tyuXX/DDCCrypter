// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.AgreementUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class AgreementUtilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();

        private AgreementUtilities()
        {
        }

        static AgreementUtilities()
        {
            algorithms[X9ObjectIdentifiers.DHSinglePassCofactorDHSha1KdfScheme.Id] = "ECCDHWITHSHA1KDF";
            algorithms[X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme.Id] = "ECDHWITHSHA1KDF";
            algorithms[X9ObjectIdentifiers.MqvSinglePassSha1KdfScheme.Id] = "ECMQVWITHSHA1KDF";
        }

        public static IBasicAgreement GetBasicAgreement( DerObjectIdentifier oid ) => GetBasicAgreement( oid.Id );

        public static IBasicAgreement GetBasicAgreement( string algorithm )
        {
            string upperInvariant = Platform.ToUpperInvariant( algorithm );
            string str = (string)algorithms[upperInvariant] ?? upperInvariant;
            if (str == "DH" || str == "DIFFIEHELLMAN")
                return new DHBasicAgreement();
            if (str == "ECDH")
                return new ECDHBasicAgreement();
            if (str == "ECDHC" || str == "ECCDH")
                return new ECDHCBasicAgreement();
            if (str == "ECMQV")
                return new ECMqvBasicAgreement();
            throw new SecurityUtilityException( "Basic Agreement " + algorithm + " not recognised." );
        }

        public static IBasicAgreement GetBasicAgreementWithKdf(
          DerObjectIdentifier oid,
          string wrapAlgorithm )
        {
            return GetBasicAgreementWithKdf( oid.Id, wrapAlgorithm );
        }

        public static IBasicAgreement GetBasicAgreementWithKdf(
          string agreeAlgorithm,
          string wrapAlgorithm )
        {
            string upperInvariant = Platform.ToUpperInvariant( agreeAlgorithm );
            string str = (string)algorithms[upperInvariant] ?? upperInvariant;
            if (str == "DHWITHSHA1KDF" || str == "ECDHWITHSHA1KDF")
                return new ECDHWithKdfBasicAgreement( wrapAlgorithm, new ECDHKekGenerator( new Sha1Digest() ) );
            if (str == "ECMQVWITHSHA1KDF")
                return new ECMqvWithKdfBasicAgreement( wrapAlgorithm, new ECDHKekGenerator( new Sha1Digest() ) );
            throw new SecurityUtilityException( "Basic Agreement (with KDF) " + agreeAlgorithm + " not recognised." );
        }

        public static string GetAlgorithmName( DerObjectIdentifier oid ) => (string)algorithms[oid.Id];
    }
}
