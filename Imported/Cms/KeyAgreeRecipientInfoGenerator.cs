// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.KeyAgreeRecipientInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    internal class KeyAgreeRecipientInfoGenerator : RecipientInfoGenerator
    {
        private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;
        private DerObjectIdentifier keyAgreementOID;
        private DerObjectIdentifier keyEncryptionOID;
        private IList recipientCerts;
        private AsymmetricCipherKeyPair senderKeyPair;

        internal KeyAgreeRecipientInfoGenerator()
        {
        }

        internal DerObjectIdentifier KeyAgreementOID
        {
            set => this.keyAgreementOID = value;
        }

        internal DerObjectIdentifier KeyEncryptionOID
        {
            set => this.keyEncryptionOID = value;
        }

        internal ICollection RecipientCerts
        {
            set => this.recipientCerts = Platform.CreateArrayList( value );
        }

        internal AsymmetricCipherKeyPair SenderKeyPair
        {
            set => this.senderKeyPair = value;
        }

        public RecipientInfo Generate( KeyParameter contentEncryptionKey, SecureRandom random )
        {
            byte[] key = contentEncryptionKey.GetKey();
            AsymmetricKeyParameter publicKey = this.senderKeyPair.Public;
            ICipherParameters cipherParameters1 = senderKeyPair.Private;
            OriginatorIdentifierOrKey originator;
            try
            {
                originator = new OriginatorIdentifierOrKey( CreateOriginatorPublicKey( publicKey ) );
            }
            catch (IOException ex)
            {
                throw new InvalidKeyException( "cannot extract originator public key: " + ex );
            }
            Asn1OctetString ukm = null;
            if (this.keyAgreementOID.Id.Equals( CmsEnvelopedGenerator.ECMqvSha1Kdf ))
            {
                try
                {
                    IAsymmetricCipherKeyPairGenerator keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator( this.keyAgreementOID );
                    keyPairGenerator.Init( ((ECKeyParameters)publicKey).CreateKeyGenerationParameters( random ) );
                    AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
                    ukm = new DerOctetString( new MQVuserKeyingMaterial( CreateOriginatorPublicKey( keyPair.Public ), null ) );
                    cipherParameters1 = new MqvPrivateParameters( (ECPrivateKeyParameters)cipherParameters1, (ECPrivateKeyParameters)keyPair.Private, (ECPublicKeyParameters)keyPair.Public );
                }
                catch (IOException ex)
                {
                    throw new InvalidKeyException( "cannot extract MQV ephemeral public key: " + ex );
                }
                catch (SecurityUtilityException ex)
                {
                    throw new InvalidKeyException( "cannot determine MQV ephemeral key pair parameters from public key: " + ex );
                }
            }
            AlgorithmIdentifier keyEncryptionAlgorithm = new( this.keyAgreementOID, new DerSequence( new Asn1Encodable[2]
            {
         keyEncryptionOID,
         DerNull.Instance
            } ) );
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            foreach (X509Certificate recipientCert in (IEnumerable)this.recipientCerts)
            {
                TbsCertificateStructure instance;
                try
                {
                    instance = TbsCertificateStructure.GetInstance( Asn1Object.FromByteArray( recipientCert.GetTbsCertificate() ) );
                }
                catch (Exception ex)
                {
                    throw new ArgumentException( "can't extract TBS structure from certificate" );
                }
                KeyAgreeRecipientIdentifier id = new( new IssuerAndSerialNumber( instance.Issuer, instance.SerialNumber.Value ) );
                ICipherParameters cipherParameters2 = recipientCert.GetPublicKey();
                if (this.keyAgreementOID.Id.Equals( CmsEnvelopedGenerator.ECMqvSha1Kdf ))
                    cipherParameters2 = new MqvPublicParameters( (ECPublicKeyParameters)cipherParameters2, (ECPublicKeyParameters)cipherParameters2 );
                IBasicAgreement agreementWithKdf = AgreementUtilities.GetBasicAgreementWithKdf( this.keyAgreementOID, this.keyEncryptionOID.Id );
                agreementWithKdf.Init( new ParametersWithRandom( cipherParameters1, random ) );
                KeyParameter keyParameter = ParameterUtilities.CreateKeyParameter( this.keyEncryptionOID, X9IntegerConverter.IntegerToBytes( agreementWithKdf.CalculateAgreement( cipherParameters2 ), GeneratorUtilities.GetDefaultKeySize( this.keyEncryptionOID ) / 8 ) );
                IWrapper wrapper = Helper.CreateWrapper( this.keyEncryptionOID.Id );
                wrapper.Init( true, new ParametersWithRandom( keyParameter, random ) );
                Asn1OctetString encryptedKey = new DerOctetString( wrapper.Wrap( key, 0, key.Length ) );
                v.Add( new RecipientEncryptedKey( id, encryptedKey ) );
            }
            return new RecipientInfo( new KeyAgreeRecipientInfo( originator, ukm, keyEncryptionAlgorithm, new DerSequence( v ) ) );
        }

        private static OriginatorPublicKey CreateOriginatorPublicKey( AsymmetricKeyParameter publicKey )
        {
            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( publicKey );
            return new OriginatorPublicKey( new AlgorithmIdentifier( subjectPublicKeyInfo.AlgorithmID.Algorithm, DerNull.Instance ), subjectPublicKeyInfo.PublicKeyData.GetBytes() );
        }
    }
}
