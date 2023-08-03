// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsEnvelopedHelper
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    internal class CmsEnvelopedHelper
    {
        internal static readonly CmsEnvelopedHelper Instance = new();
        private static readonly IDictionary KeySizes = Platform.CreateHashtable();
        private static readonly IDictionary BaseCipherNames = Platform.CreateHashtable();

        static CmsEnvelopedHelper()
        {
            KeySizes.Add( CmsEnvelopedGenerator.DesEde3Cbc, 192 );
            KeySizes.Add( CmsEnvelopedGenerator.Aes128Cbc, 128 );
            KeySizes.Add( CmsEnvelopedGenerator.Aes192Cbc, 192 );
            KeySizes.Add( CmsEnvelopedGenerator.Aes256Cbc, 256 );
            BaseCipherNames.Add( CmsEnvelopedGenerator.DesEde3Cbc, "DESEDE" );
            BaseCipherNames.Add( CmsEnvelopedGenerator.Aes128Cbc, "AES" );
            BaseCipherNames.Add( CmsEnvelopedGenerator.Aes192Cbc, "AES" );
            BaseCipherNames.Add( CmsEnvelopedGenerator.Aes256Cbc, "AES" );
        }

        private string GetAsymmetricEncryptionAlgName( string encryptionAlgOid ) => PkcsObjectIdentifiers.RsaEncryption.Id.Equals( encryptionAlgOid ) ? "RSA/ECB/PKCS1Padding" : encryptionAlgOid;

        internal IBufferedCipher CreateAsymmetricCipher( string encryptionOid )
        {
            string encryptionAlgName = this.GetAsymmetricEncryptionAlgName( encryptionOid );
            if (!encryptionAlgName.Equals( encryptionOid ))
            {
                try
                {
                    return CipherUtilities.GetCipher( encryptionAlgName );
                }
                catch (SecurityUtilityException ex)
                {
                }
            }
            return CipherUtilities.GetCipher( encryptionOid );
        }

        internal IWrapper CreateWrapper( string encryptionOid )
        {
            try
            {
                return WrapperUtilities.GetWrapper( encryptionOid );
            }
            catch (SecurityUtilityException ex)
            {
                return WrapperUtilities.GetWrapper( this.GetAsymmetricEncryptionAlgName( encryptionOid ) );
            }
        }

        internal string GetRfc3211WrapperName( string oid )
        {
            string str = oid != null ? (string)BaseCipherNames[oid] : throw new ArgumentNullException( nameof( oid ) );
            if (str == null)
                throw new ArgumentException( "no name for " + oid, nameof( oid ) );
            return str + "RFC3211Wrap";
        }

        internal int GetKeySize( string oid ) => KeySizes.Contains( oid ) ? (int)KeySizes[oid] : throw new ArgumentException( "no keysize for " + oid, nameof( oid ) );

        internal static RecipientInformationStore BuildRecipientInformationStore(
          Asn1Set recipientInfos,
          CmsSecureReadable secureReadable )
        {
            IList arrayList = Platform.CreateArrayList();
            for (int index = 0; index != recipientInfos.Count; ++index)
            {
                RecipientInfo instance = RecipientInfo.GetInstance( recipientInfos[index] );
                ReadRecipientInfo( arrayList, instance, secureReadable );
            }
            return new RecipientInformationStore( arrayList );
        }

        private static void ReadRecipientInfo(
          IList infos,
          RecipientInfo info,
          CmsSecureReadable secureReadable )
        {
            Asn1Encodable info1 = info.Info;
            switch (info1)
            {
                case KeyTransRecipientInfo _:
                    infos.Add( new KeyTransRecipientInformation( (KeyTransRecipientInfo)info1, secureReadable ) );
                    break;
                case KekRecipientInfo _:
                    infos.Add( new KekRecipientInformation( (KekRecipientInfo)info1, secureReadable ) );
                    break;
                case KeyAgreeRecipientInfo _:
                    KeyAgreeRecipientInformation.ReadRecipientInfo( infos, (KeyAgreeRecipientInfo)info1, secureReadable );
                    break;
                case PasswordRecipientInfo _:
                    infos.Add( new PasswordRecipientInformation( (PasswordRecipientInfo)info1, secureReadable ) );
                    break;
            }
        }

        internal class CmsAuthenticatedSecureReadable : CmsSecureReadable
        {
            private AlgorithmIdentifier algorithm;
            private IMac mac;
            private CmsReadable readable;

            internal CmsAuthenticatedSecureReadable( AlgorithmIdentifier algorithm, CmsReadable readable )
            {
                this.algorithm = algorithm;
                this.readable = readable;
            }

            public AlgorithmIdentifier Algorithm => this.algorithm;

            public object CryptoObject => mac;

            public CmsReadable GetReadable( KeyParameter sKey )
            {
                string id = this.algorithm.Algorithm.Id;
                try
                {
                    this.mac = MacUtilities.GetMac( id );
                    this.mac.Init( sKey );
                }
                catch (SecurityUtilityException ex)
                {
                    throw new CmsException( "couldn't create cipher.", ex );
                }
                catch (InvalidKeyException ex)
                {
                    throw new CmsException( "key invalid in message.", ex );
                }
                catch (IOException ex)
                {
                    throw new CmsException( "error decoding algorithm parameters.", ex );
                }
                try
                {
                    return new CmsProcessableInputStream( new TeeInputStream( this.readable.GetInputStream(), new MacOutputStream( this.mac ) ) );
                }
                catch (IOException ex)
                {
                    throw new CmsException( "error reading content.", ex );
                }
            }
        }

        internal class CmsEnvelopedSecureReadable : CmsSecureReadable
        {
            private AlgorithmIdentifier algorithm;
            private IBufferedCipher cipher;
            private CmsReadable readable;

            internal CmsEnvelopedSecureReadable( AlgorithmIdentifier algorithm, CmsReadable readable )
            {
                this.algorithm = algorithm;
                this.readable = readable;
            }

            public AlgorithmIdentifier Algorithm => this.algorithm;

            public object CryptoObject => cipher;

            public CmsReadable GetReadable( KeyParameter sKey )
            {
                try
                {
                    this.cipher = CipherUtilities.GetCipher( this.algorithm.Algorithm );
                    Asn1Encodable parameters = this.algorithm.Parameters;
                    Asn1Object asn1Object = parameters == null ? null : parameters.ToAsn1Object();
                    ICipherParameters cipherParameters = sKey;
                    switch (asn1Object)
                    {
                        case null:
                        case Asn1Null _:
                            string id = this.algorithm.Algorithm.Id;
                            if (id.Equals( CmsEnvelopedGenerator.DesEde3Cbc ) || id.Equals( "1.3.6.1.4.1.188.7.1.1.2" ) || id.Equals( "1.2.840.113533.7.66.10" ))
                            {
                                cipherParameters = new ParametersWithIV( cipherParameters, new byte[8] );
                                break;
                            }
                            break;
                        default:
                            cipherParameters = ParameterUtilities.GetCipherParameters( this.algorithm.Algorithm, cipherParameters, asn1Object );
                            break;
                    }
                    this.cipher.Init( false, cipherParameters );
                }
                catch (SecurityUtilityException ex)
                {
                    throw new CmsException( "couldn't create cipher.", ex );
                }
                catch (InvalidKeyException ex)
                {
                    throw new CmsException( "key invalid in message.", ex );
                }
                catch (IOException ex)
                {
                    throw new CmsException( "error decoding algorithm parameters.", ex );
                }
                try
                {
                    return new CmsProcessableInputStream( new CipherStream( this.readable.GetInputStream(), this.cipher, null ) );
                }
                catch (IOException ex)
                {
                    throw new CmsException( "error reading content.", ex );
                }
            }
        }
    }
}
