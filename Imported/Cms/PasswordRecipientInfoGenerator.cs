// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.PasswordRecipientInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Cms
{
    internal class PasswordRecipientInfoGenerator : RecipientInfoGenerator
    {
        private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;
        private AlgorithmIdentifier keyDerivationAlgorithm;
        private KeyParameter keyEncryptionKey;
        private string keyEncryptionKeyOID;

        internal PasswordRecipientInfoGenerator()
        {
        }

        internal AlgorithmIdentifier KeyDerivationAlgorithm
        {
            set => this.keyDerivationAlgorithm = value;
        }

        internal KeyParameter KeyEncryptionKey
        {
            set => this.keyEncryptionKey = value;
        }

        internal string KeyEncryptionKeyOID
        {
            set => this.keyEncryptionKeyOID = value;
        }

        public RecipientInfo Generate( KeyParameter contentEncryptionKey, SecureRandom random )
        {
            byte[] key = contentEncryptionKey.GetKey();
            string rfc3211WrapperName = Helper.GetRfc3211WrapperName( this.keyEncryptionKeyOID );
            IWrapper wrapper = Helper.CreateWrapper( rfc3211WrapperName );
            byte[] numArray = new byte[Platform.StartsWith( rfc3211WrapperName, "DESEDE" ) ? 8 : 16];
            random.NextBytes( numArray );
            ICipherParameters parameters1 = new ParametersWithIV( keyEncryptionKey, numArray );
            wrapper.Init( true, new ParametersWithRandom( parameters1, random ) );
            Asn1OctetString encryptedKey = new DerOctetString( wrapper.Wrap( key, 0, key.Length ) );
            DerSequence parameters2 = new DerSequence( new Asn1Encodable[2]
            {
         new DerObjectIdentifier(this.keyEncryptionKeyOID),
         new DerOctetString(numArray)
            } );
            return new RecipientInfo( new PasswordRecipientInfo( this.keyDerivationAlgorithm, new AlgorithmIdentifier( PkcsObjectIdentifiers.IdAlgPwriKek, parameters2 ), encryptedKey ) );
        }
    }
}
