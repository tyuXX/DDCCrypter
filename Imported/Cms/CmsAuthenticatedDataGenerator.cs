// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsAuthenticatedDataGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsAuthenticatedDataGenerator : CmsAuthenticatedGenerator
    {
        public CmsAuthenticatedDataGenerator()
        {
        }

        public CmsAuthenticatedDataGenerator( SecureRandom rand )
          : base( rand )
        {
        }

        private CmsAuthenticatedData Generate(
          CmsProcessable content,
          string macOid,
          CipherKeyGenerator keyGen )
        {
            KeyParameter keyParameter;
            AlgorithmIdentifier algorithmIdentifier;
            Asn1OctetString content1;
            Asn1OctetString mac1;
            try
            {
                byte[] key = keyGen.GenerateKey();
                keyParameter = ParameterUtilities.CreateKeyParameter( macOid, key );
                Asn1Encodable asn1Parameters = this.GenerateAsn1Parameters( macOid, key );
                algorithmIdentifier = this.GetAlgorithmIdentifier( macOid, keyParameter, asn1Parameters, out ICipherParameters _ );
                IMac mac2 = MacUtilities.GetMac( macOid );
                mac2.Init( keyParameter );
                MemoryStream output = new MemoryStream();
                Stream stream = new TeeOutputStream( output, new MacOutputStream( mac2 ) );
                content.Write( stream );
                Platform.Dispose( stream );
                content1 = new BerOctetString( output.ToArray() );
                mac1 = new DerOctetString( MacUtilities.DoFinal( mac2 ) );
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
            ContentInfo encapsulatedContent = new ContentInfo( CmsObjectIdentifiers.Data, content1 );
            return new CmsAuthenticatedData( new ContentInfo( CmsObjectIdentifiers.AuthenticatedData, new AuthenticatedData( null, new DerSet( v ), algorithmIdentifier, null, encapsulatedContent, null, mac1, null ) ) );
        }

        public CmsAuthenticatedData Generate( CmsProcessable content, string encryptionOid )
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
    }
}
