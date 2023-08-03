// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsEnvelopedDataGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsEnvelopedDataGenerator : CmsEnvelopedGenerator
    {
        public CmsEnvelopedDataGenerator()
        {
        }

        public CmsEnvelopedDataGenerator( SecureRandom rand )
          : base( rand )
        {
        }

        private CmsEnvelopedData Generate(
          CmsProcessable content,
          string encryptionOid,
          CipherKeyGenerator keyGen )
        {
            KeyParameter keyParameter;
            AlgorithmIdentifier algorithmIdentifier;
            Asn1OctetString encryptedContent;
            try
            {
                byte[] key = keyGen.GenerateKey();
                keyParameter = ParameterUtilities.CreateKeyParameter( encryptionOid, key );
                Asn1Encodable asn1Parameters = this.GenerateAsn1Parameters( encryptionOid, key );
                ICipherParameters cipherParameters;
                algorithmIdentifier = this.GetAlgorithmIdentifier( encryptionOid, keyParameter, asn1Parameters, out cipherParameters );
                IBufferedCipher cipher = CipherUtilities.GetCipher( encryptionOid );
                cipher.Init( true, new ParametersWithRandom( cipherParameters, this.rand ) );
                MemoryStream memoryStream = new MemoryStream();
                CipherStream cipherStream = new CipherStream( memoryStream, null, cipher );
                content.Write( cipherStream );
                Platform.Dispose( cipherStream );
                encryptedContent = new BerOctetString( memoryStream.ToArray() );
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
                throw new CmsException( "exception decoding algorithm parameters.", ex );
            }
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (RecipientInfoGenerator recipientInfoGenerator in (IEnumerable)this.recipientInfoGenerators)
            {
                try
                {
                    v.Add( recipientInfoGenerator.Generate( keyParameter, this.rand ) );
                }
                catch (InvalidKeyException ex)
                {
                    throw new CmsException( "key inappropriate for algorithm.", ex );
                }
                catch (GeneralSecurityException ex)
                {
                    throw new CmsException( "error making encrypted content.", ex );
                }
            }
            EncryptedContentInfo encryptedContentInfo = new EncryptedContentInfo( CmsObjectIdentifiers.Data, algorithmIdentifier, encryptedContent );
            Asn1Set unprotectedAttrs = null;
            if (this.unprotectedAttributeGenerator != null)
                unprotectedAttrs = new BerSet( this.unprotectedAttributeGenerator.GetAttributes( Platform.CreateHashtable() ).ToAsn1EncodableVector() );
            return new CmsEnvelopedData( new ContentInfo( CmsObjectIdentifiers.EnvelopedData, new EnvelopedData( null, new DerSet( v ), encryptedContentInfo, unprotectedAttrs ) ) );
        }

        public CmsEnvelopedData Generate( CmsProcessable content, string encryptionOid )
        {
            try
            {
                CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator( encryptionOid );
                keyGenerator.Init( new KeyGenerationParameters( this.rand, keyGenerator.DefaultStrength ) );
                return this.Generate( content, encryptionOid, keyGenerator );
            }
            catch (SecurityUtilityException ex)
            {
                throw new CmsException( "can't find key generation algorithm.", ex );
            }
        }

        public CmsEnvelopedData Generate( CmsProcessable content, string encryptionOid, int keySize )
        {
            try
            {
                CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator( encryptionOid );
                keyGenerator.Init( new KeyGenerationParameters( this.rand, keySize ) );
                return this.Generate( content, encryptionOid, keyGenerator );
            }
            catch (SecurityUtilityException ex)
            {
                throw new CmsException( "can't find key generation algorithm.", ex );
            }
        }
    }
}
