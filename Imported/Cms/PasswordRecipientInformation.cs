// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.PasswordRecipientInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Cms
{
    public class PasswordRecipientInformation : RecipientInformation
    {
        private readonly PasswordRecipientInfo info;

        internal PasswordRecipientInformation(
          PasswordRecipientInfo info,
          CmsSecureReadable secureReadable )
          : base( info.KeyEncryptionAlgorithm, secureReadable )
        {
            this.info = info;
            this.rid = new RecipientID();
        }

        public virtual AlgorithmIdentifier KeyDerivationAlgorithm => this.info.KeyDerivationAlgorithm;

        public override CmsTypedStream GetContentStream( ICipherParameters key )
        {
            try
            {
                Asn1Sequence parameters1 = (Asn1Sequence)AlgorithmIdentifier.GetInstance( info.KeyEncryptionAlgorithm ).Parameters;
                byte[] octets1 = this.info.EncryptedKey.GetOctets();
                string id = DerObjectIdentifier.GetInstance( parameters1[0] ).Id;
                IWrapper wrapper = WrapperUtilities.GetWrapper( CmsEnvelopedHelper.Instance.GetRfc3211WrapperName( id ) );
                byte[] octets2 = Asn1OctetString.GetInstance( parameters1[1] ).GetOctets();
                ICipherParameters parameters2 = new ParametersWithIV( ((CmsPbeKey)key).GetEncoded( id ), octets2 );
                wrapper.Init( false, parameters2 );
                return this.GetContentFromSessionKey( ParameterUtilities.CreateKeyParameter( this.GetContentAlgorithmName(), wrapper.Unwrap( octets1, 0, octets1.Length ) ) );
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
