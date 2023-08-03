// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsSignedDataStreamGenerator
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
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.X509;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsSignedDataStreamGenerator : CmsSignedGenerator
    {
        private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;
        private readonly IList _signerInfs = Platform.CreateArrayList();
        private readonly ISet _messageDigestOids = new HashSet();
        private readonly IDictionary _messageDigests = Platform.CreateHashtable();
        private readonly IDictionary _messageHashes = Platform.CreateHashtable();
        private bool _messageDigestsLocked;
        private int _bufferSize;

        public CmsSignedDataStreamGenerator()
        {
        }

        public CmsSignedDataStreamGenerator( SecureRandom rand )
          : base( rand )
        {
        }

        public void SetBufferSize( int bufferSize ) => this._bufferSize = bufferSize;

        public void AddDigests( params string[] digestOids ) => this.AddDigests( (IEnumerable)digestOids );

        public void AddDigests( IEnumerable digestOids )
        {
            foreach (string digestOid in digestOids)
                this.ConfigureDigest( digestOid );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string digestOid )
        {
            this.AddSigner( privateKey, cert, digestOid, new DefaultSignedAttributeTableGenerator(), null );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string encryptionOid,
          string digestOid )
        {
            this.AddSigner( privateKey, cert, encryptionOid, digestOid, new DefaultSignedAttributeTableGenerator(), null );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string digestOid,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.AddSigner( privateKey, cert, digestOid, new DefaultSignedAttributeTableGenerator( signedAttr ), new SimpleAttributeTableGenerator( unsignedAttr ) );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string encryptionOid,
          string digestOid,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.AddSigner( privateKey, cert, encryptionOid, digestOid, new DefaultSignedAttributeTableGenerator( signedAttr ), new SimpleAttributeTableGenerator( unsignedAttr ) );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string digestOid,
          CmsAttributeTableGenerator signedAttrGenerator,
          CmsAttributeTableGenerator unsignedAttrGenerator )
        {
            this.AddSigner( privateKey, cert, Helper.GetEncOid( privateKey, digestOid ), digestOid, signedAttrGenerator, unsignedAttrGenerator );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string encryptionOid,
          string digestOid,
          CmsAttributeTableGenerator signedAttrGenerator,
          CmsAttributeTableGenerator unsignedAttrGenerator )
        {
            this.DoAddSigner( privateKey, GetSignerIdentifier( cert ), encryptionOid, digestOid, signedAttrGenerator, unsignedAttrGenerator );
        }

        public void AddSigner( AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOid ) => this.AddSigner( privateKey, subjectKeyID, digestOid, new DefaultSignedAttributeTableGenerator(), null );

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string encryptionOid,
          string digestOid )
        {
            this.AddSigner( privateKey, subjectKeyID, encryptionOid, digestOid, new DefaultSignedAttributeTableGenerator(), null );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string digestOid,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.AddSigner( privateKey, subjectKeyID, digestOid, new DefaultSignedAttributeTableGenerator( signedAttr ), new SimpleAttributeTableGenerator( unsignedAttr ) );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string digestOid,
          CmsAttributeTableGenerator signedAttrGenerator,
          CmsAttributeTableGenerator unsignedAttrGenerator )
        {
            this.AddSigner( privateKey, subjectKeyID, Helper.GetEncOid( privateKey, digestOid ), digestOid, signedAttrGenerator, unsignedAttrGenerator );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string encryptionOid,
          string digestOid,
          CmsAttributeTableGenerator signedAttrGenerator,
          CmsAttributeTableGenerator unsignedAttrGenerator )
        {
            this.DoAddSigner( privateKey, GetSignerIdentifier( subjectKeyID ), encryptionOid, digestOid, signedAttrGenerator, unsignedAttrGenerator );
        }

        private void DoAddSigner(
          AsymmetricKeyParameter privateKey,
          SignerIdentifier signerIdentifier,
          string encryptionOid,
          string digestOid,
          CmsAttributeTableGenerator signedAttrGenerator,
          CmsAttributeTableGenerator unsignedAttrGenerator )
        {
            this.ConfigureDigest( digestOid );
            this._signerInfs.Add( new CmsSignedDataStreamGenerator.DigestAndSignerInfoGeneratorHolder( new CmsSignedDataStreamGenerator.SignerInfoGeneratorImpl( this, privateKey, signerIdentifier, digestOid, encryptionOid, signedAttrGenerator, unsignedAttrGenerator ), digestOid ) );
        }

        internal override void AddSignerCallback( SignerInformation si ) => this.RegisterDigestOid( si.DigestAlgorithmID.Algorithm.Id );

        public Stream Open( Stream outStream ) => this.Open( outStream, false );

        public Stream Open( Stream outStream, bool encapsulate ) => this.Open( outStream, Data, encapsulate );

        public Stream Open( Stream outStream, bool encapsulate, Stream dataOutputStream ) => this.Open( outStream, Data, encapsulate, dataOutputStream );

        public Stream Open( Stream outStream, string signedContentType, bool encapsulate ) => this.Open( outStream, signedContentType, encapsulate, null );

        public Stream Open(
          Stream outStream,
          string signedContentType,
          bool encapsulate,
          Stream dataOutputStream )
        {
            if (outStream == null)
                throw new ArgumentNullException( nameof( outStream ) );
            if (!outStream.CanWrite)
                throw new ArgumentException( "Expected writeable stream", nameof( outStream ) );
            if (dataOutputStream != null && !dataOutputStream.CanWrite)
                throw new ArgumentException( "Expected writeable stream", nameof( dataOutputStream ) );
            this._messageDigestsLocked = true;
            BerSequenceGenerator sGen = new( outStream );
            sGen.AddObject( CmsObjectIdentifiers.SignedData );
            BerSequenceGenerator sigGen = new( sGen.GetRawOutputStream(), 0, true );
            DerObjectIdentifier contentOid = signedContentType == null ? null : new DerObjectIdentifier( signedContentType );
            sigGen.AddObject( this.CalculateVersion( contentOid ) );
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            foreach (string messageDigestOid in (IEnumerable)this._messageDigestOids)
                v.Add( new AlgorithmIdentifier( new DerObjectIdentifier( messageDigestOid ), DerNull.Instance ) );
            byte[] encoded = new DerSet( v ).GetEncoded();
            sigGen.GetRawOutputStream().Write( encoded, 0, encoded.Length );
            BerSequenceGenerator eiGen = new( sigGen.GetRawOutputStream() );
            eiGen.AddObject( contentOid );
            Stream octetOutputStream = encapsulate ? CmsUtilities.CreateBerOctetOutputStream( eiGen.GetRawOutputStream(), 0, true, this._bufferSize ) : null;
            return new CmsSignedDataStreamGenerator.CmsSignedDataOutputStream( this, AttachDigestsToOutputStream( this._messageDigests.Values, GetSafeTeeOutputStream( dataOutputStream, octetOutputStream ) ), signedContentType, sGen, sigGen, eiGen );
        }

        private void RegisterDigestOid( string digestOid )
        {
            if (this._messageDigestsLocked)
            {
                if (!this._messageDigestOids.Contains( digestOid ))
                    throw new InvalidOperationException( "Cannot register new digest OIDs after the data stream is opened" );
            }
            else
                this._messageDigestOids.Add( digestOid );
        }

        private void ConfigureDigest( string digestOid )
        {
            this.RegisterDigestOid( digestOid );
            string digestAlgName = Helper.GetDigestAlgName( digestOid );
            if ((IDigest)this._messageDigests[digestAlgName] != null)
                return;
            if (this._messageDigestsLocked)
                throw new InvalidOperationException( "Cannot configure new digests after the data stream is opened" );
            IDigest digestInstance = Helper.GetDigestInstance( digestAlgName );
            this._messageDigests[digestAlgName] = digestInstance;
        }

        internal void Generate(
          Stream outStream,
          string eContentType,
          bool encapsulate,
          Stream dataOutputStream,
          CmsProcessable content )
        {
            Stream stream = this.Open( outStream, eContentType, encapsulate, dataOutputStream );
            content?.Write( stream );
            Platform.Dispose( stream );
        }

        private DerInteger CalculateVersion( DerObjectIdentifier contentOid )
        {
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            if (this._certs != null)
            {
                foreach (object cert in (IEnumerable)this._certs)
                {
                    if (cert is Asn1TaggedObject)
                    {
                        Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)cert;
                        if (asn1TaggedObject.TagNo == 1)
                            flag3 = true;
                        else if (asn1TaggedObject.TagNo == 2)
                            flag4 = true;
                        else if (asn1TaggedObject.TagNo == 3)
                        {
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            if (flag1)
                return new DerInteger( 5 );
            if (this._crls != null)
            {
                foreach (object crl in (IEnumerable)this._crls)
                {
                    if (crl is Asn1TaggedObject)
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (flag2)
                return new DerInteger( 5 );
            if (flag4)
                return new DerInteger( 4 );
            return flag3 || !CmsObjectIdentifiers.Data.Equals( contentOid ) || this.CheckForVersion3( this._signers ) ? new DerInteger( 3 ) : new DerInteger( 1 );
        }

        private bool CheckForVersion3( IList signerInfos )
        {
            foreach (SignerInformation signerInfo in (IEnumerable)signerInfos)
            {
                if (SignerInfo.GetInstance( signerInfo.ToSignerInfo() ).Version.Value.IntValue == 3)
                    return true;
            }
            return false;
        }

        private static Stream AttachDigestsToOutputStream( ICollection digests, Stream s )
        {
            Stream s1 = s;
            foreach (IDigest digest in (IEnumerable)digests)
                s1 = GetSafeTeeOutputStream( s1, new DigOutputStream( digest ) );
            return s1;
        }

        private static Stream GetSafeOutputStream( Stream s ) => s == null ? new NullOutputStream() : s;

        private static Stream GetSafeTeeOutputStream( Stream s1, Stream s2 )
        {
            if (s1 == null)
                return GetSafeOutputStream( s2 );
            return s2 == null ? GetSafeOutputStream( s1 ) : new TeeOutputStream( s1, s2 );
        }

        private class DigestAndSignerInfoGeneratorHolder
        {
            internal readonly ISignerInfoGenerator signerInf;
            internal readonly string digestOID;

            internal DigestAndSignerInfoGeneratorHolder( ISignerInfoGenerator signerInf, string digestOID )
            {
                this.signerInf = signerInf;
                this.digestOID = digestOID;
            }

            internal AlgorithmIdentifier DigestAlgorithm => new( new DerObjectIdentifier( this.digestOID ), DerNull.Instance );
        }

        private class SignerInfoGeneratorImpl : ISignerInfoGenerator
        {
            private readonly CmsSignedDataStreamGenerator outer;
            private readonly SignerIdentifier _signerIdentifier;
            private readonly string _digestOID;
            private readonly string _encOID;
            private readonly CmsAttributeTableGenerator _sAttr;
            private readonly CmsAttributeTableGenerator _unsAttr;
            private readonly string _encName;
            private readonly ISigner _sig;

            internal SignerInfoGeneratorImpl(
              CmsSignedDataStreamGenerator outer,
              AsymmetricKeyParameter key,
              SignerIdentifier signerIdentifier,
              string digestOID,
              string encOID,
              CmsAttributeTableGenerator sAttr,
              CmsAttributeTableGenerator unsAttr )
            {
                this.outer = outer;
                this._signerIdentifier = signerIdentifier;
                this._digestOID = digestOID;
                this._encOID = encOID;
                this._sAttr = sAttr;
                this._unsAttr = unsAttr;
                this._encName = Helper.GetEncryptionAlgName( this._encOID );
                string algorithm = Helper.GetDigestAlgName( this._digestOID ) + "with" + this._encName;
                if (this._sAttr != null)
                    this._sig = Helper.GetSignatureInstance( algorithm );
                else if (this._encName.Equals( "RSA" ))
                {
                    this._sig = Helper.GetSignatureInstance( "RSA" );
                }
                else
                {
                    if (!this._encName.Equals( "DSA" ))
                        throw new SignatureException( "algorithm: " + this._encName + " not supported in base signatures." );
                    this._sig = Helper.GetSignatureInstance( "NONEwithDSA" );
                }
                this._sig.Init( true, new ParametersWithRandom( key, outer.rand ) );
            }

            public SignerInfo Generate(
              DerObjectIdentifier contentType,
              AlgorithmIdentifier digestAlgorithm,
              byte[] calculatedDigest )
            {
                try
                {
                    string algorithm = Helper.GetDigestAlgName( this._digestOID ) + "with" + this._encName;
                    byte[] input = calculatedDigest;
                    Asn1Set authenticatedAttributes = null;
                    if (this._sAttr != null)
                    {
                        Org.BouncyCastle.Asn1.Cms.AttributeTable attr = this._sAttr.GetAttributes( this.outer.GetBaseParameters( contentType, digestAlgorithm, calculatedDigest ) );
                        if (contentType == null && attr != null && attr[CmsAttributes.ContentType] != null)
                        {
                            IDictionary dictionary = attr.ToDictionary();
                            dictionary.Remove( CmsAttributes.ContentType );
                            attr = new Org.BouncyCastle.Asn1.Cms.AttributeTable( dictionary );
                        }
                        authenticatedAttributes = this.outer.GetAttributeSet( attr );
                        input = authenticatedAttributes.GetEncoded( "DER" );
                    }
                    else if (this._encName.Equals( "RSA" ))
                        input = new DigestInfo( digestAlgorithm, calculatedDigest ).GetEncoded( "DER" );
                    this._sig.BlockUpdate( input, 0, input.Length );
                    byte[] signature = this._sig.GenerateSignature();
                    Asn1Set unauthenticatedAttributes = null;
                    if (this._unsAttr != null)
                    {
                        IDictionary baseParameters = this.outer.GetBaseParameters( contentType, digestAlgorithm, calculatedDigest );
                        baseParameters[CmsAttributeTableParameter.Signature] = signature.Clone();
                        unauthenticatedAttributes = this.outer.GetAttributeSet( this._unsAttr.GetAttributes( baseParameters ) );
                    }
                    Asn1Encodable defaultX509Parameters = SignerUtilities.GetDefaultX509Parameters( algorithm );
                    AlgorithmIdentifier algorithmIdentifier = Helper.GetEncAlgorithmIdentifier( new DerObjectIdentifier( this._encOID ), defaultX509Parameters );
                    return new SignerInfo( this._signerIdentifier, digestAlgorithm, authenticatedAttributes, algorithmIdentifier, new DerOctetString( signature ), unauthenticatedAttributes );
                }
                catch (IOException ex)
                {
                    throw new CmsStreamException( "encoding error.", ex );
                }
                catch (SignatureException ex)
                {
                    throw new CmsStreamException( "error creating signature.", ex );
                }
            }
        }

        private class CmsSignedDataOutputStream : BaseOutputStream
        {
            private readonly CmsSignedDataStreamGenerator outer;
            private Stream _out;
            private DerObjectIdentifier _contentOID;
            private BerSequenceGenerator _sGen;
            private BerSequenceGenerator _sigGen;
            private BerSequenceGenerator _eiGen;

            public CmsSignedDataOutputStream(
              CmsSignedDataStreamGenerator outer,
              Stream outStream,
              string contentOID,
              BerSequenceGenerator sGen,
              BerSequenceGenerator sigGen,
              BerSequenceGenerator eiGen )
            {
                this.outer = outer;
                this._out = outStream;
                this._contentOID = new DerObjectIdentifier( contentOID );
                this._sGen = sGen;
                this._sigGen = sigGen;
                this._eiGen = eiGen;
            }

            public override void WriteByte( byte b ) => this._out.WriteByte( b );

            public override void Write( byte[] bytes, int off, int len ) => this._out.Write( bytes, off, len );

            public override void Close()
            {
                this.DoClose();
                base.Close();
            }

            private void DoClose()
            {
                Platform.Dispose( this._out );
                this._eiGen.Close();
                this.outer._digests.Clear();
                if (this.outer._certs.Count > 0)
                    WriteToGenerator( _sigGen, new BerTaggedObject( false, 0, CmsUtilities.CreateBerSetFromList( this.outer._certs ) ) );
                if (this.outer._crls.Count > 0)
                    WriteToGenerator( _sigGen, new BerTaggedObject( false, 1, CmsUtilities.CreateBerSetFromList( this.outer._crls ) ) );
                foreach (DictionaryEntry messageDigest in this.outer._messageDigests)
                    this.outer._messageHashes.Add( messageDigest.Key, DigestUtilities.DoFinal( (IDigest)messageDigest.Value ) );
                Asn1EncodableVector v = new( new Asn1Encodable[0] );
                foreach (CmsSignedDataStreamGenerator.DigestAndSignerInfoGeneratorHolder signerInf in (IEnumerable)this.outer._signerInfs)
                {
                    AlgorithmIdentifier digestAlgorithm = signerInf.DigestAlgorithm;
                    byte[] messageHash = (byte[])this.outer._messageHashes[Helper.GetDigestAlgName( signerInf.digestOID )];
                    this.outer._digests[signerInf.digestOID] = messageHash.Clone();
                    v.Add( signerInf.signerInf.Generate( this._contentOID, digestAlgorithm, messageHash ) );
                }
                foreach (SignerInformation signer in (IEnumerable)this.outer._signers)
                    v.Add( signer.ToSignerInfo() );
                WriteToGenerator( _sigGen, new DerSet( v ) );
                this._sigGen.Close();
                this._sGen.Close();
            }

            private static void WriteToGenerator( Asn1Generator ag, Asn1Encodable ae )
            {
                byte[] encoded = ae.GetEncoded();
                ag.GetRawOutputStream().Write( encoded, 0, encoded.Length );
            }
        }
    }
}
