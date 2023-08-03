// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.KekRecipientInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Kisa;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Ntt;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Cms
{
    internal class KekRecipientInfoGenerator : RecipientInfoGenerator
    {
        private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;
        private KeyParameter keyEncryptionKey;
        private string keyEncryptionKeyOID;
        private KekIdentifier kekIdentifier;
        private AlgorithmIdentifier keyEncryptionAlgorithm;

        internal KekRecipientInfoGenerator()
        {
        }

        internal KekIdentifier KekIdentifier
        {
            set => this.kekIdentifier = value;
        }

        internal KeyParameter KeyEncryptionKey
        {
            set
            {
                this.keyEncryptionKey = value;
                this.keyEncryptionAlgorithm = DetermineKeyEncAlg( this.keyEncryptionKeyOID, this.keyEncryptionKey );
            }
        }

        internal string KeyEncryptionKeyOID
        {
            set => this.keyEncryptionKeyOID = value;
        }

        public RecipientInfo Generate( KeyParameter contentEncryptionKey, SecureRandom random )
        {
            byte[] key = contentEncryptionKey.GetKey();
            IWrapper wrapper = Helper.CreateWrapper( this.keyEncryptionAlgorithm.Algorithm.Id );
            wrapper.Init( true, new ParametersWithRandom( keyEncryptionKey, random ) );
            return new RecipientInfo( new KekRecipientInfo( this.kekIdentifier, this.keyEncryptionAlgorithm, new DerOctetString( wrapper.Wrap( key, 0, key.Length ) ) ) );
        }

        private static AlgorithmIdentifier DetermineKeyEncAlg( string algorithm, KeyParameter key )
        {
            if (Platform.StartsWith( algorithm, "DES" ))
                return new AlgorithmIdentifier( PkcsObjectIdentifiers.IdAlgCms3DesWrap, DerNull.Instance );
            if (Platform.StartsWith( algorithm, "RC2" ))
                return new AlgorithmIdentifier( PkcsObjectIdentifiers.IdAlgCmsRC2Wrap, new DerInteger( 58 ) );
            if (Platform.StartsWith( algorithm, "AES" ))
            {
                DerObjectIdentifier algorithm1;
                switch (key.GetKey().Length * 8)
                {
                    case 128:
                        algorithm1 = NistObjectIdentifiers.IdAes128Wrap;
                        break;
                    case 192:
                        algorithm1 = NistObjectIdentifiers.IdAes192Wrap;
                        break;
                    case 256:
                        algorithm1 = NistObjectIdentifiers.IdAes256Wrap;
                        break;
                    default:
                        throw new ArgumentException( "illegal keysize in AES" );
                }
                return new AlgorithmIdentifier( algorithm1 );
            }
            if (Platform.StartsWith( algorithm, "SEED" ))
                return new AlgorithmIdentifier( KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap );
            if (!Platform.StartsWith( algorithm, "CAMELLIA" ))
                throw new ArgumentException( "unknown algorithm" );
            DerObjectIdentifier algorithm2;
            switch (key.GetKey().Length * 8)
            {
                case 128:
                    algorithm2 = NttObjectIdentifiers.IdCamellia128Wrap;
                    break;
                case 192:
                    algorithm2 = NttObjectIdentifiers.IdCamellia192Wrap;
                    break;
                case 256:
                    algorithm2 = NttObjectIdentifiers.IdCamellia256Wrap;
                    break;
                default:
                    throw new ArgumentException( "illegal keysize in Camellia" );
            }
            return new AlgorithmIdentifier( algorithm2 );
        }
    }
}
