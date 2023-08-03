// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsAuthenticatedDataStreamGenerator
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
    public class CmsAuthenticatedDataStreamGenerator : CmsAuthenticatedGenerator
    {
        private int _bufferSize;
        private bool _berEncodeRecipientSet;

        public CmsAuthenticatedDataStreamGenerator()
        {
        }

        public CmsAuthenticatedDataStreamGenerator( SecureRandom rand )
          : base( rand )
        {
        }

        public void SetBufferSize( int bufferSize ) => this._bufferSize = bufferSize;

        public void SetBerEncodeRecipients( bool berEncodeRecipientSet ) => this._berEncodeRecipientSet = berEncodeRecipientSet;

        private Stream Open( Stream outStr, string macOid, CipherKeyGenerator keyGen )
        {
            byte[] key = keyGen.GenerateKey();
            KeyParameter keyParameter = ParameterUtilities.CreateKeyParameter( macOid, key );
            Asn1Encodable asn1Parameters = this.GenerateAsn1Parameters( macOid, key );
            AlgorithmIdentifier algorithmIdentifier = this.GetAlgorithmIdentifier( macOid, keyParameter, asn1Parameters, out ICipherParameters _ );
            Asn1EncodableVector recipientInfos = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (RecipientInfoGenerator recipientInfoGenerator in (IEnumerable)this.recipientInfoGenerators)
            {
                try
                {
                    recipientInfos.Add( recipientInfoGenerator.Generate( keyParameter, this.rand ) );
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
            return this.Open( outStr, algorithmIdentifier, keyParameter, recipientInfos );
        }

        protected Stream Open(
          Stream outStr,
          AlgorithmIdentifier macAlgId,
          ICipherParameters cipherParameters,
          Asn1EncodableVector recipientInfos )
        {
            try
            {
                BerSequenceGenerator cGen = new BerSequenceGenerator( outStr );
                cGen.AddObject( CmsObjectIdentifiers.AuthenticatedData );
                BerSequenceGenerator authGen = new BerSequenceGenerator( cGen.GetRawOutputStream(), 0, true );
                authGen.AddObject( new DerInteger( AuthenticatedData.CalculateVersion( null ) ) );
                Stream rawOutputStream = authGen.GetRawOutputStream();
                Asn1Generator asn1Generator = this._berEncodeRecipientSet ? new BerSetGenerator( rawOutputStream ) : (Asn1Generator)new DerSetGenerator( rawOutputStream );
                foreach (Asn1Encodable recipientInfo in recipientInfos)
                    asn1Generator.AddObject( recipientInfo );
                asn1Generator.Close();
                authGen.AddObject( macAlgId );
                BerSequenceGenerator eiGen = new BerSequenceGenerator( rawOutputStream );
                eiGen.AddObject( CmsObjectIdentifiers.Data );
                Stream octetOutputStream = CmsUtilities.CreateBerOctetOutputStream( eiGen.GetRawOutputStream(), 0, false, this._bufferSize );
                IMac mac = MacUtilities.GetMac( macAlgId.Algorithm );
                mac.Init( cipherParameters );
                return new CmsAuthenticatedDataStreamGenerator.CmsAuthenticatedDataOutputStream( new TeeOutputStream( octetOutputStream, new MacOutputStream( mac ) ), mac, cGen, authGen, eiGen );
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
        }

        public Stream Open( Stream outStr, string encryptionOid )
        {
            CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator( encryptionOid );
            keyGenerator.Init( new KeyGenerationParameters( this.rand, keyGenerator.DefaultStrength ) );
            return this.Open( outStr, encryptionOid, keyGenerator );
        }

        public Stream Open( Stream outStr, string encryptionOid, int keySize )
        {
            CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator( encryptionOid );
            keyGenerator.Init( new KeyGenerationParameters( this.rand, keySize ) );
            return this.Open( outStr, encryptionOid, keyGenerator );
        }

        private class CmsAuthenticatedDataOutputStream : BaseOutputStream
        {
            private readonly Stream macStream;
            private readonly IMac mac;
            private readonly BerSequenceGenerator cGen;
            private readonly BerSequenceGenerator authGen;
            private readonly BerSequenceGenerator eiGen;

            public CmsAuthenticatedDataOutputStream(
              Stream macStream,
              IMac mac,
              BerSequenceGenerator cGen,
              BerSequenceGenerator authGen,
              BerSequenceGenerator eiGen )
            {
                this.macStream = macStream;
                this.mac = mac;
                this.cGen = cGen;
                this.authGen = authGen;
                this.eiGen = eiGen;
            }

            public override void WriteByte( byte b ) => this.macStream.WriteByte( b );

            public override void Write( byte[] bytes, int off, int len ) => this.macStream.Write( bytes, off, len );

            public override void Close()
            {
                Platform.Dispose( this.macStream );
                this.eiGen.Close();
                this.authGen.AddObject( new DerOctetString( MacUtilities.DoFinal( this.mac ) ) );
                this.authGen.Close();
                this.cGen.Close();
                base.Close();
            }
        }
    }
}
