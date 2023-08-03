// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.KeyAgreeRecipientInformation
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
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class KeyAgreeRecipientInformation : RecipientInformation
    {
        private KeyAgreeRecipientInfo info;
        private Asn1OctetString encryptedKey;

        internal static void ReadRecipientInfo(
          IList infos,
          KeyAgreeRecipientInfo info,
          CmsSecureReadable secureReadable )
        {
            try
            {
                foreach (Asn1Encodable recipientEncryptedKey in info.RecipientEncryptedKeys)
                {
                    RecipientEncryptedKey instance = RecipientEncryptedKey.GetInstance( recipientEncryptedKey.ToAsn1Object() );
                    RecipientID rid = new();
                    KeyAgreeRecipientIdentifier identifier = instance.Identifier;
                    IssuerAndSerialNumber issuerAndSerialNumber = identifier.IssuerAndSerialNumber;
                    if (issuerAndSerialNumber != null)
                    {
                        rid.Issuer = issuerAndSerialNumber.Name;
                        rid.SerialNumber = issuerAndSerialNumber.SerialNumber.Value;
                    }
                    else
                    {
                        RecipientKeyIdentifier rkeyId = identifier.RKeyID;
                        rid.SubjectKeyIdentifier = rkeyId.SubjectKeyIdentifier.GetOctets();
                    }
                    infos.Add( new KeyAgreeRecipientInformation( info, rid, instance.EncryptedKey, secureReadable ) );
                }
            }
            catch (IOException ex)
            {
                throw new ArgumentException( "invalid rid in KeyAgreeRecipientInformation", ex );
            }
        }

        internal KeyAgreeRecipientInformation(
          KeyAgreeRecipientInfo info,
          RecipientID rid,
          Asn1OctetString encryptedKey,
          CmsSecureReadable secureReadable )
          : base( info.KeyEncryptionAlgorithm, secureReadable )
        {
            this.info = info;
            this.rid = rid;
            this.encryptedKey = encryptedKey;
        }

        private AsymmetricKeyParameter GetSenderPublicKey(
          AsymmetricKeyParameter receiverPrivateKey,
          OriginatorIdentifierOrKey originator )
        {
            OriginatorPublicKey originatorPublicKey = originator.OriginatorPublicKey;
            if (originatorPublicKey != null)
                return this.GetPublicKeyFromOriginatorPublicKey( receiverPrivateKey, originatorPublicKey );
            OriginatorID origID = new();
            IssuerAndSerialNumber issuerAndSerialNumber = originator.IssuerAndSerialNumber;
            if (issuerAndSerialNumber != null)
            {
                origID.Issuer = issuerAndSerialNumber.Name;
                origID.SerialNumber = issuerAndSerialNumber.SerialNumber.Value;
            }
            else
            {
                SubjectKeyIdentifier subjectKeyIdentifier = originator.SubjectKeyIdentifier;
                origID.SubjectKeyIdentifier = subjectKeyIdentifier.GetKeyIdentifier();
            }
            return this.GetPublicKeyFromOriginatorID( origID );
        }

        private AsymmetricKeyParameter GetPublicKeyFromOriginatorPublicKey(
          AsymmetricKeyParameter receiverPrivateKey,
          OriginatorPublicKey originatorPublicKey )
        {
            return PublicKeyFactory.CreateKey( new SubjectPublicKeyInfo( PrivateKeyInfoFactory.CreatePrivateKeyInfo( receiverPrivateKey ).PrivateKeyAlgorithm, originatorPublicKey.PublicKey.GetBytes() ) );
        }

        private AsymmetricKeyParameter GetPublicKeyFromOriginatorID( OriginatorID origID ) => throw new CmsException( "No support for 'originator' as IssuerAndSerialNumber or SubjectKeyIdentifier" );

        private KeyParameter CalculateAgreedWrapKey(
          string wrapAlg,
          AsymmetricKeyParameter senderPublicKey,
          AsymmetricKeyParameter receiverPrivateKey )
        {
            DerObjectIdentifier algorithm = this.keyEncAlg.Algorithm;
            ICipherParameters cipherParameters1 = senderPublicKey;
            ICipherParameters cipherParameters2 = receiverPrivateKey;
            if (algorithm.Id.Equals( CmsEnvelopedGenerator.ECMqvSha1Kdf ))
            {
                MQVuserKeyingMaterial instance = MQVuserKeyingMaterial.GetInstance( Asn1Object.FromByteArray( this.info.UserKeyingMaterial.GetOctets() ) );
                AsymmetricKeyParameter originatorPublicKey = this.GetPublicKeyFromOriginatorPublicKey( receiverPrivateKey, instance.EphemeralPublicKey );
                cipherParameters1 = new MqvPublicParameters( (ECPublicKeyParameters)cipherParameters1, (ECPublicKeyParameters)originatorPublicKey );
                cipherParameters2 = new MqvPrivateParameters( (ECPrivateKeyParameters)cipherParameters2, (ECPrivateKeyParameters)cipherParameters2 );
            }
            IBasicAgreement agreementWithKdf = AgreementUtilities.GetBasicAgreementWithKdf( algorithm, wrapAlg );
            agreementWithKdf.Init( cipherParameters2 );
            byte[] bytes = X9IntegerConverter.IntegerToBytes( agreementWithKdf.CalculateAgreement( cipherParameters1 ), GeneratorUtilities.GetDefaultKeySize( wrapAlg ) / 8 );
            return ParameterUtilities.CreateKeyParameter( wrapAlg, bytes );
        }

        private KeyParameter UnwrapSessionKey( string wrapAlg, KeyParameter agreedKey )
        {
            byte[] octets = this.encryptedKey.GetOctets();
            IWrapper wrapper = WrapperUtilities.GetWrapper( wrapAlg );
            wrapper.Init( false, agreedKey );
            byte[] keyBytes = wrapper.Unwrap( octets, 0, octets.Length );
            return ParameterUtilities.CreateKeyParameter( this.GetContentAlgorithmName(), keyBytes );
        }

        internal KeyParameter GetSessionKey( AsymmetricKeyParameter receiverPrivateKey )
        {
            try
            {
                string id = DerObjectIdentifier.GetInstance( Asn1Sequence.GetInstance( keyEncAlg.Parameters )[0] ).Id;
                AsymmetricKeyParameter senderPublicKey = this.GetSenderPublicKey( receiverPrivateKey, this.info.Originator );
                KeyParameter agreedWrapKey = this.CalculateAgreedWrapKey( id, senderPublicKey, receiverPrivateKey );
                return this.UnwrapSessionKey( id, agreedWrapKey );
            }
            catch (SecurityUtilityException ex)
            {
                throw new CmsException( "couldn't create cipher.", ex );
            }
            catch (InvalidKeyException ex)
            {
                throw new CmsException( "key invalid in message.", ex );
            }
            catch (Exception ex)
            {
                throw new CmsException( "originator key invalid.", ex );
            }
        }

        public override CmsTypedStream GetContentStream( ICipherParameters key )
        {
            AsymmetricKeyParameter receiverPrivateKey = key is AsymmetricKeyParameter ? (AsymmetricKeyParameter)key : throw new ArgumentException( "KeyAgreement requires asymmetric key", nameof( key ) );
            return receiverPrivateKey.IsPrivate ? this.GetContentFromSessionKey( this.GetSessionKey( receiverPrivateKey ) ) : throw new ArgumentException( "Expected private key", nameof( key ) );
        }
    }
}
