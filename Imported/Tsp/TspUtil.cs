// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TspUtil
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
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Tsp
{
    public class TspUtil
    {
        private static ISet EmptySet = CollectionUtilities.ReadOnly( new HashSet() );
        private static IList EmptyList = CollectionUtilities.ReadOnly( Platform.CreateArrayList() );
        private static readonly IDictionary digestLengths = Platform.CreateHashtable();
        private static readonly IDictionary digestNames = Platform.CreateHashtable();

        static TspUtil()
        {
            digestLengths.Add( PkcsObjectIdentifiers.MD5.Id, 16 );
            digestLengths.Add( OiwObjectIdentifiers.IdSha1.Id, 20 );
            digestLengths.Add( NistObjectIdentifiers.IdSha224.Id, 28 );
            digestLengths.Add( NistObjectIdentifiers.IdSha256.Id, 32 );
            digestLengths.Add( NistObjectIdentifiers.IdSha384.Id, 48 );
            digestLengths.Add( NistObjectIdentifiers.IdSha512.Id, 64 );
            digestLengths.Add( TeleTrusTObjectIdentifiers.RipeMD128.Id, 16 );
            digestLengths.Add( TeleTrusTObjectIdentifiers.RipeMD160.Id, 20 );
            digestLengths.Add( TeleTrusTObjectIdentifiers.RipeMD256.Id, 32 );
            digestLengths.Add( CryptoProObjectIdentifiers.GostR3411.Id, 32 );
            digestNames.Add( PkcsObjectIdentifiers.MD5.Id, "MD5" );
            digestNames.Add( OiwObjectIdentifiers.IdSha1.Id, "SHA1" );
            digestNames.Add( NistObjectIdentifiers.IdSha224.Id, "SHA224" );
            digestNames.Add( NistObjectIdentifiers.IdSha256.Id, "SHA256" );
            digestNames.Add( NistObjectIdentifiers.IdSha384.Id, "SHA384" );
            digestNames.Add( NistObjectIdentifiers.IdSha512.Id, "SHA512" );
            digestNames.Add( PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id, "SHA1" );
            digestNames.Add( PkcsObjectIdentifiers.Sha224WithRsaEncryption.Id, "SHA224" );
            digestNames.Add( PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id, "SHA256" );
            digestNames.Add( PkcsObjectIdentifiers.Sha384WithRsaEncryption.Id, "SHA384" );
            digestNames.Add( PkcsObjectIdentifiers.Sha512WithRsaEncryption.Id, "SHA512" );
            digestNames.Add( TeleTrusTObjectIdentifiers.RipeMD128.Id, "RIPEMD128" );
            digestNames.Add( TeleTrusTObjectIdentifiers.RipeMD160.Id, "RIPEMD160" );
            digestNames.Add( TeleTrusTObjectIdentifiers.RipeMD256.Id, "RIPEMD256" );
            digestNames.Add( CryptoProObjectIdentifiers.GostR3411.Id, "GOST3411" );
        }

        public static ICollection GetSignatureTimestamps( SignerInformation signerInfo )
        {
            IList arrayList = Platform.CreateArrayList();
            Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = signerInfo.UnsignedAttributes;
            if (unsignedAttributes != null)
            {
                foreach (Org.BouncyCastle.Asn1.Cms.Attribute attribute in unsignedAttributes.GetAll( PkcsObjectIdentifiers.IdAASignatureTimeStampToken ))
                {
                    foreach (Asn1Encodable attrValue in attribute.AttrValues)
                    {
                        try
                        {
                            TimeStampToken timeStampToken = new TimeStampToken( Asn1.Cms.ContentInfo.GetInstance( attrValue.ToAsn1Object() ) );
                            TimeStampTokenInfo timeStampInfo = timeStampToken.TimeStampInfo;
                            if (!Arrays.ConstantTimeAreEqual( DigestUtilities.CalculateDigest( GetDigestAlgName( timeStampInfo.MessageImprintAlgOid ), signerInfo.GetSignature() ), timeStampInfo.GetMessageImprintDigest() ))
                                throw new TspValidationException( "Incorrect digest in message imprint" );
                            arrayList.Add( timeStampToken );
                        }
                        catch (SecurityUtilityException ex)
                        {
                            throw new TspValidationException( "Unknown hash algorithm specified in timestamp" );
                        }
                        catch (Exception ex)
                        {
                            throw new TspValidationException( "Timestamp could not be parsed" );
                        }
                    }
                }
            }
            return arrayList;
        }

        public static void ValidateCertificate( X509Certificate cert )
        {
            Asn1OctetString asn1OctetString = cert.Version == 3 ? cert.GetExtensionValue( X509Extensions.ExtendedKeyUsage ) : throw new ArgumentException( "Certificate must have an ExtendedKeyUsage extension." );
            if (asn1OctetString == null)
                throw new TspValidationException( "Certificate must have an ExtendedKeyUsage extension." );
            if (!cert.GetCriticalExtensionOids().Contains( X509Extensions.ExtendedKeyUsage.Id ))
                throw new TspValidationException( "Certificate must have an ExtendedKeyUsage extension marked as critical." );
            try
            {
                ExtendedKeyUsage instance = ExtendedKeyUsage.GetInstance( Asn1Object.FromByteArray( asn1OctetString.GetOctets() ) );
                if (!instance.HasKeyPurposeId( KeyPurposeID.IdKPTimeStamping ) || instance.Count != 1)
                    throw new TspValidationException( "ExtendedKeyUsage not solely time stamping." );
            }
            catch (IOException ex)
            {
                throw new TspValidationException( "cannot process ExtendedKeyUsage extension" );
            }
        }

        internal static string GetDigestAlgName( string digestAlgOID ) => (string)digestNames[digestAlgOID] ?? digestAlgOID;

        internal static int GetDigestLength( string digestAlgOID ) => digestLengths.Contains( digestAlgOID ) ? (int)digestLengths[digestAlgOID] : throw new TspException( "digest algorithm cannot be found." );

        internal static IDigest CreateDigestInstance( string digestAlgOID ) => DigestUtilities.GetDigest( GetDigestAlgName( digestAlgOID ) );

        internal static ISet GetCriticalExtensionOids( X509Extensions extensions ) => extensions == null ? EmptySet : CollectionUtilities.ReadOnly( new HashSet( extensions.GetCriticalExtensionOids() ) );

        internal static ISet GetNonCriticalExtensionOids( X509Extensions extensions ) => extensions == null ? EmptySet : CollectionUtilities.ReadOnly( new HashSet( extensions.GetNonCriticalExtensionOids() ) );

        internal static IList GetExtensionOids( X509Extensions extensions ) => extensions == null ? EmptyList : CollectionUtilities.ReadOnly( Platform.CreateArrayList( extensions.GetExtensionOids() ) );
    }
}
