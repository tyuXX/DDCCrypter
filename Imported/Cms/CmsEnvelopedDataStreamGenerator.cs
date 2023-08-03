// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsEnvelopedDataStreamGenerator
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
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsEnvelopedDataStreamGenerator : CmsEnvelopedGenerator
    {
        private object _originatorInfo = null;
        private object _unprotectedAttributes = null;
        private int _bufferSize;
        private bool _berEncodeRecipientSet;

        public CmsEnvelopedDataStreamGenerator()
        {
        }

        public CmsEnvelopedDataStreamGenerator( SecureRandom rand )
          : base( rand )
        {
        }

        public void SetBufferSize( int bufferSize ) => this._bufferSize = bufferSize;

        public void SetBerEncodeRecipients( bool berEncodeRecipientSet ) => this._berEncodeRecipientSet = berEncodeRecipientSet;

        private DerInteger Version => new DerInteger( this._originatorInfo != null || this._unprotectedAttributes != null ? 2 : 0 );

        private Stream Open( Stream outStream, string encryptionOid, CipherKeyGenerator keyGen )
        {
            byte[] key = keyGen.GenerateKey();
            KeyParameter keyParameter = ParameterUtilities.CreateKeyParameter( encryptionOid, key );
            Asn1Encodable asn1Parameters = this.GenerateAsn1Parameters( encryptionOid, key );
            ICipherParameters cipherParameters;
            AlgorithmIdentifier algorithmIdentifier = this.GetAlgorithmIdentifier( encryptionOid, keyParameter, asn1Parameters, out cipherParameters );
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
            return this.Open( outStream, algorithmIdentifier, cipherParameters, recipientInfos );
        }

        private Stream Open(
          Stream outStream,
          AlgorithmIdentifier encAlgID,
          ICipherParameters cipherParameters,
          Asn1EncodableVector recipientInfos )
        {
            try
            {
                BerSequenceGenerator cGen = new BerSequenceGenerator( outStream );
                cGen.AddObject( CmsObjectIdentifiers.EnvelopedData );
                BerSequenceGenerator envGen = new BerSequenceGenerator( cGen.GetRawOutputStream(), 0, true );
                envGen.AddObject( Version );
                Stream rawOutputStream = envGen.GetRawOutputStream();
                Asn1Generator asn1Generator = this._berEncodeRecipientSet ? new BerSetGenerator( rawOutputStream ) : (Asn1Generator)new DerSetGenerator( rawOutputStream );
                foreach (Asn1Encodable recipientInfo in recipientInfos)
                    asn1Generator.AddObject( recipientInfo );
                asn1Generator.Close();
                BerSequenceGenerator eiGen = new BerSequenceGenerator( rawOutputStream );
                eiGen.AddObject( CmsObjectIdentifiers.Data );
                eiGen.AddObject( encAlgID );
                Stream octetOutputStream = CmsUtilities.CreateBerOctetOutputStream( eiGen.GetRawOutputStream(), 0, false, this._bufferSize );
                IBufferedCipher cipher = CipherUtilities.GetCipher( encAlgID.Algorithm );
                cipher.Init( true, new ParametersWithRandom( cipherParameters, this.rand ) );
                return new CmsEnvelopedDataStreamGenerator.CmsEnvelopedDataOutputStream( this, new CipherStream( octetOutputStream, null, cipher ), cGen, envGen, eiGen );
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

        public Stream Open( Stream outStream, string encryptionOid )
        {
            CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator( encryptionOid );
            keyGenerator.Init( new KeyGenerationParameters( this.rand, keyGenerator.DefaultStrength ) );
            return this.Open( outStream, encryptionOid, keyGenerator );
        }

        public Stream Open( Stream outStream, string encryptionOid, int keySize )
        {
            CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator( encryptionOid );
            keyGenerator.Init( new KeyGenerationParameters( this.rand, keySize ) );
            return this.Open( outStream, encryptionOid, keyGenerator );
        }

        private class CmsEnvelopedDataOutputStream : BaseOutputStream
        {
            private readonly CmsEnvelopedGenerator _outer;
            private readonly CipherStream _out;
            private readonly BerSequenceGenerator _cGen;
            private readonly BerSequenceGenerator _envGen;
            private readonly BerSequenceGenerator _eiGen;

            public CmsEnvelopedDataOutputStream(
              CmsEnvelopedGenerator outer,
              CipherStream outStream,
              BerSequenceGenerator cGen,
              BerSequenceGenerator envGen,
              BerSequenceGenerator eiGen )
            {
                this._outer = outer;
                this._out = outStream;
                this._cGen = cGen;
                this._envGen = envGen;
                this._eiGen = eiGen;
            }

            public override void WriteByte( byte b ) => this._out.WriteByte( b );

            public override void Write( byte[] bytes, int off, int len ) => this._out.Write( bytes, off, len );

            public override void Close()
            {
                Platform.Dispose( _out );
                this._eiGen.Close();
                if (this._outer.unprotectedAttributeGenerator != null)
                    this._envGen.AddObject( new DerTaggedObject( false, 1, new BerSet( this._outer.unprotectedAttributeGenerator.GetAttributes( Platform.CreateHashtable() ).ToAsn1EncodableVector() ) ) );
                this._envGen.Close();
                this._cGen.Close();
                base.Close();
            }
        }
    }
}
