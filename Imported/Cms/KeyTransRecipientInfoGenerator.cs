// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.KeyTransRecipientInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    internal class KeyTransRecipientInfoGenerator : RecipientInfoGenerator
    {
        private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;
        private TbsCertificateStructure recipientTbsCert;
        private AsymmetricKeyParameter recipientPublicKey;
        private Asn1OctetString subjectKeyIdentifier;
        private SubjectPublicKeyInfo info;

        internal KeyTransRecipientInfoGenerator()
        {
        }

        internal X509Certificate RecipientCert
        {
            set
            {
                this.recipientTbsCert = CmsUtilities.GetTbsCertificateStructure( value );
                this.recipientPublicKey = value.GetPublicKey();
                this.info = this.recipientTbsCert.SubjectPublicKeyInfo;
            }
        }

        internal AsymmetricKeyParameter RecipientPublicKey
        {
            set
            {
                this.recipientPublicKey = value;
                try
                {
                    this.info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( this.recipientPublicKey );
                }
                catch (IOException ex)
                {
                    throw new ArgumentException( "can't extract key algorithm from this key" );
                }
            }
        }

        internal Asn1OctetString SubjectKeyIdentifier
        {
            set => this.subjectKeyIdentifier = value;
        }

        public RecipientInfo Generate( KeyParameter contentEncryptionKey, SecureRandom random )
        {
            byte[] key = contentEncryptionKey.GetKey();
            AlgorithmIdentifier algorithmId = this.info.AlgorithmID;
            IWrapper wrapper = Helper.CreateWrapper( algorithmId.Algorithm.Id );
            wrapper.Init( true, new ParametersWithRandom( recipientPublicKey, random ) );
            byte[] str = wrapper.Wrap( key, 0, key.Length );
            return new RecipientInfo( new KeyTransRecipientInfo( this.recipientTbsCert == null ? new RecipientIdentifier( this.subjectKeyIdentifier ) : new RecipientIdentifier( new IssuerAndSerialNumber( this.recipientTbsCert.Issuer, this.recipientTbsCert.SerialNumber.Value ) ), algorithmId, new DerOctetString( str ) ) );
        }
    }
}
