// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.KeyTransRecipientInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class KeyTransRecipientInformation : RecipientInformation
    {
        private KeyTransRecipientInfo info;

        internal KeyTransRecipientInformation(
          KeyTransRecipientInfo info,
          CmsSecureReadable secureReadable )
          : base( info.KeyEncryptionAlgorithm, secureReadable )
        {
            this.info = info;
            this.rid = new RecipientID();
            RecipientIdentifier recipientIdentifier = info.RecipientIdentifier;
            try
            {
                if (recipientIdentifier.IsTagged)
                {
                    this.rid.SubjectKeyIdentifier = Asn1OctetString.GetInstance( recipientIdentifier.ID ).GetOctets();
                }
                else
                {
                    Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber instance = Asn1.Cms.IssuerAndSerialNumber.GetInstance( recipientIdentifier.ID );
                    this.rid.Issuer = instance.Name;
                    this.rid.SerialNumber = instance.SerialNumber.Value;
                }
            }
            catch (IOException ex)
            {
                throw new ArgumentException( "invalid rid in KeyTransRecipientInformation" );
            }
        }

        private string GetExchangeEncryptionAlgorithmName( DerObjectIdentifier oid ) => PkcsObjectIdentifiers.RsaEncryption.Equals( oid ) ? "RSA//PKCS1Padding" : oid.Id;

        internal KeyParameter UnwrapKey( ICipherParameters key )
        {
            byte[] octets = this.info.EncryptedKey.GetOctets();
            string encryptionAlgorithmName = this.GetExchangeEncryptionAlgorithmName( this.keyEncAlg.Algorithm );
            try
            {
                IWrapper wrapper = WrapperUtilities.GetWrapper( encryptionAlgorithmName );
                wrapper.Init( false, key );
                return ParameterUtilities.CreateKeyParameter( this.GetContentAlgorithmName(), wrapper.Unwrap( octets, 0, octets.Length ) );
            }
            catch (SecurityUtilityException ex)
            {
                throw new CmsException( "couldn't create cipher.", ex );
            }
            catch (InvalidKeyException ex)
            {
                throw new CmsException( "key invalid in message.", ex );
            }
            catch (DataLengthException ex)
            {
                throw new CmsException( "illegal blocksize in message.", ex );
            }
            catch (InvalidCipherTextException ex)
            {
                throw new CmsException( "bad padding in message.", ex );
            }
        }

        public override CmsTypedStream GetContentStream( ICipherParameters key ) => this.GetContentFromSessionKey( this.UnwrapKey( key ) );
    }
}
