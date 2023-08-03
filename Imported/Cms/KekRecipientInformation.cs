// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.KekRecipientInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Cms
{
    public class KekRecipientInformation : RecipientInformation
    {
        private KekRecipientInfo info;

        internal KekRecipientInformation( KekRecipientInfo info, CmsSecureReadable secureReadable )
          : base( info.KeyEncryptionAlgorithm, secureReadable )
        {
            this.info = info;
            this.rid = new RecipientID
            {
                KeyIdentifier = info.KekID.KeyIdentifier.GetOctets()
            };
        }

        public override CmsTypedStream GetContentStream( ICipherParameters key )
        {
            try
            {
                byte[] octets = this.info.EncryptedKey.GetOctets();
                IWrapper wrapper = WrapperUtilities.GetWrapper( this.keyEncAlg.Algorithm.Id );
                wrapper.Init( false, key );
                return this.GetContentFromSessionKey( ParameterUtilities.CreateKeyParameter( this.GetContentAlgorithmName(), wrapper.Unwrap( octets, 0, octets.Length ) ) );
            }
            catch (SecurityUtilityException ex)
            {
                throw new CmsException( "couldn't create cipher.", ex );
            }
            catch (InvalidKeyException ex)
            {
                throw new CmsException( "key invalid in message.", ex );
            }
        }
    }
}
