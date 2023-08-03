// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsSignedDataGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsSignedDataGenerator : CmsSignedGenerator
    {
        private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;
        private readonly IList signerInfs = Platform.CreateArrayList();

        public CmsSignedDataGenerator()
        {
        }

        public CmsSignedDataGenerator( SecureRandom rand )
          : base( rand )
        {
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string digestOID )
        {
            this.AddSigner( privateKey, cert, Helper.GetEncOid( privateKey, digestOID ), digestOID );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string encryptionOID,
          string digestOID )
        {
            this.doAddSigner( privateKey, GetSignerIdentifier( cert ), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator(), null, null );
        }

        public void AddSigner( AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOID ) => this.AddSigner( privateKey, subjectKeyID, Helper.GetEncOid( privateKey, digestOID ), digestOID );

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string encryptionOID,
          string digestOID )
        {
            this.doAddSigner( privateKey, GetSignerIdentifier( subjectKeyID ), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator(), null, null );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string digestOID,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.AddSigner( privateKey, cert, Helper.GetEncOid( privateKey, digestOID ), digestOID, signedAttr, unsignedAttr );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string encryptionOID,
          string digestOID,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.doAddSigner( privateKey, GetSignerIdentifier( cert ), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator( signedAttr ), new SimpleAttributeTableGenerator( unsignedAttr ), signedAttr );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string digestOID,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.AddSigner( privateKey, subjectKeyID, Helper.GetEncOid( privateKey, digestOID ), digestOID, signedAttr, unsignedAttr );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string encryptionOID,
          string digestOID,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.doAddSigner( privateKey, GetSignerIdentifier( subjectKeyID ), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator( signedAttr ), new SimpleAttributeTableGenerator( unsignedAttr ), signedAttr );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string digestOID,
          CmsAttributeTableGenerator signedAttrGen,
          CmsAttributeTableGenerator unsignedAttrGen )
        {
            this.AddSigner( privateKey, cert, Helper.GetEncOid( privateKey, digestOID ), digestOID, signedAttrGen, unsignedAttrGen );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          X509Certificate cert,
          string encryptionOID,
          string digestOID,
          CmsAttributeTableGenerator signedAttrGen,
          CmsAttributeTableGenerator unsignedAttrGen )
        {
            this.doAddSigner( privateKey, GetSignerIdentifier( cert ), encryptionOID, digestOID, signedAttrGen, unsignedAttrGen, null );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string digestOID,
          CmsAttributeTableGenerator signedAttrGen,
          CmsAttributeTableGenerator unsignedAttrGen )
        {
            this.AddSigner( privateKey, subjectKeyID, Helper.GetEncOid( privateKey, digestOID ), digestOID, signedAttrGen, unsignedAttrGen );
        }

        public void AddSigner(
          AsymmetricKeyParameter privateKey,
          byte[] subjectKeyID,
          string encryptionOID,
          string digestOID,
          CmsAttributeTableGenerator signedAttrGen,
          CmsAttributeTableGenerator unsignedAttrGen )
        {
            this.doAddSigner( privateKey, GetSignerIdentifier( subjectKeyID ), encryptionOID, digestOID, signedAttrGen, unsignedAttrGen, null );
        }

        public void AddSignerInfoGenerator( SignerInfoGenerator signerInfoGenerator ) => this.signerInfs.Add( new CmsSignedDataGenerator.SignerInf( this, signerInfoGenerator.contentSigner, signerInfoGenerator.sigId, signerInfoGenerator.signedGen, signerInfoGenerator.unsignedGen, null ) );

        private void doAddSigner(
          AsymmetricKeyParameter privateKey,
          SignerIdentifier signerIdentifier,
          string encryptionOID,
          string digestOID,
          CmsAttributeTableGenerator signedAttrGen,
          CmsAttributeTableGenerator unsignedAttrGen,
          Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable )
        {
            this.signerInfs.Add( new CmsSignedDataGenerator.SignerInf( this, privateKey, signerIdentifier, digestOID, encryptionOID, signedAttrGen, unsignedAttrGen, baseSignedTable ) );
        }

        public CmsSignedData Generate( CmsProcessable content ) => this.Generate( content, false );

        public CmsSignedData Generate(
          string signedContentType,
          CmsProcessable content,
          bool encapsulate )
        {
            Asn1EncodableVector v1 = new Asn1EncodableVector( new Asn1Encodable[0] );
            Asn1EncodableVector v2 = new Asn1EncodableVector( new Asn1Encodable[0] );
            this._digests.Clear();
            foreach (SignerInformation signer in (IEnumerable)this._signers)
            {
                v1.Add( Helper.FixAlgID( signer.DigestAlgorithmID ) );
                v2.Add( signer.ToSignerInfo() );
            }
            DerObjectIdentifier contentType = signedContentType == null ? null : new DerObjectIdentifier( signedContentType );
            foreach (CmsSignedDataGenerator.SignerInf signerInf in (IEnumerable)this.signerInfs)
            {
                try
                {
                    v1.Add( signerInf.DigestAlgorithmID );
                    v2.Add( signerInf.ToSignerInfo( contentType, content, this.rand ) );
                }
                catch (IOException ex)
                {
                    throw new CmsException( "encoding error.", ex );
                }
                catch (InvalidKeyException ex)
                {
                    throw new CmsException( "key inappropriate for signature.", ex );
                }
                catch (SignatureException ex)
                {
                    throw new CmsException( "error creating signature.", ex );
                }
                catch (CertificateEncodingException ex)
                {
                    throw new CmsException( "error creating sid.", ex );
                }
            }
            Asn1Set certificates = null;
            if (this._certs.Count != 0)
                certificates = CmsUtilities.CreateBerSetFromList( this._certs );
            Asn1Set crls = null;
            if (this._crls.Count != 0)
                crls = CmsUtilities.CreateBerSetFromList( this._crls );
            Asn1OctetString content1 = null;
            if (encapsulate)
            {
                MemoryStream outStream = new MemoryStream();
                if (content != null)
                {
                    try
                    {
                        content.Write( outStream );
                    }
                    catch (IOException ex)
                    {
                        throw new CmsException( "encapsulation error.", ex );
                    }
                }
                content1 = new BerOctetString( outStream.ToArray() );
            }
            ContentInfo contentInfo = new ContentInfo( contentType, content1 );
            SignedData content2 = new SignedData( new DerSet( v1 ), contentInfo, certificates, crls, new DerSet( v2 ) );
            ContentInfo sigData = new ContentInfo( CmsObjectIdentifiers.SignedData, content2 );
            return new CmsSignedData( content, sigData );
        }

        public CmsSignedData Generate( CmsProcessable content, bool encapsulate ) => this.Generate( Data, content, encapsulate );

        public SignerInformationStore GenerateCounterSigners( SignerInformation signer ) => this.Generate( null, new CmsProcessableByteArray( signer.GetSignature() ), false ).GetSignerInfos();

        private class SignerInf
        {
            private readonly CmsSignedGenerator outer;
            private readonly ISignatureFactory sigCalc;
            private readonly SignerIdentifier signerIdentifier;
            private readonly string digestOID;
            private readonly string encOID;
            private readonly CmsAttributeTableGenerator sAttr;
            private readonly CmsAttributeTableGenerator unsAttr;
            private readonly Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable;

            internal SignerInf(
              CmsSignedGenerator outer,
              AsymmetricKeyParameter key,
              SignerIdentifier signerIdentifier,
              string digestOID,
              string encOID,
              CmsAttributeTableGenerator sAttr,
              CmsAttributeTableGenerator unsAttr,
              Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable )
            {
                string algorithm = Helper.GetDigestAlgName( digestOID ) + "with" + Helper.GetEncryptionAlgName( encOID );
                this.outer = outer;
                this.sigCalc = new Asn1SignatureFactory( algorithm, key );
                this.signerIdentifier = signerIdentifier;
                this.digestOID = digestOID;
                this.encOID = encOID;
                this.sAttr = sAttr;
                this.unsAttr = unsAttr;
                this.baseSignedTable = baseSignedTable;
            }

            internal SignerInf(
              CmsSignedGenerator outer,
              ISignatureFactory sigCalc,
              SignerIdentifier signerIdentifier,
              CmsAttributeTableGenerator sAttr,
              CmsAttributeTableGenerator unsAttr,
              Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable )
            {
                this.outer = outer;
                this.sigCalc = sigCalc;
                this.signerIdentifier = signerIdentifier;
                this.digestOID = new DefaultDigestAlgorithmIdentifierFinder().find( (AlgorithmIdentifier)sigCalc.AlgorithmDetails ).Algorithm.Id;
                this.encOID = ((AlgorithmIdentifier)sigCalc.AlgorithmDetails).Algorithm.Id;
                this.sAttr = sAttr;
                this.unsAttr = unsAttr;
                this.baseSignedTable = baseSignedTable;
            }

            internal AlgorithmIdentifier DigestAlgorithmID => new AlgorithmIdentifier( new DerObjectIdentifier( this.digestOID ), DerNull.Instance );

            internal CmsAttributeTableGenerator SignedAttributes => this.sAttr;

            internal CmsAttributeTableGenerator UnsignedAttributes => this.unsAttr;

            internal SignerInfo ToSignerInfo(
              DerObjectIdentifier contentType,
              CmsProcessable content,
              SecureRandom random )
            {
                AlgorithmIdentifier digestAlgorithmId = this.DigestAlgorithmID;
                string digestAlgName = Helper.GetDigestAlgName( this.digestOID );
                string algorithm = digestAlgName + "with" + Helper.GetEncryptionAlgName( this.encOID );
                byte[] hash;
                if (this.outer._digests.Contains( digestOID ))
                {
                    hash = (byte[])this.outer._digests[digestOID];
                }
                else
                {
                    IDigest digestInstance = Helper.GetDigestInstance( digestAlgName );
                    content?.Write( new DigOutputStream( digestInstance ) );
                    hash = DigestUtilities.DoFinal( digestInstance );
                    this.outer._digests.Add( digestOID, hash.Clone() );
                }
                IStreamCalculator calculator = this.sigCalc.CreateCalculator();
                Stream stream = new BufferedStream( calculator.Stream );
                Asn1Set authenticatedAttributes = null;
                if (this.sAttr != null)
                {
                    Org.BouncyCastle.Asn1.Cms.AttributeTable attr = this.sAttr.GetAttributes( this.outer.GetBaseParameters( contentType, digestAlgorithmId, hash ) );
                    if (contentType == null && attr != null && attr[CmsAttributes.ContentType] != null)
                    {
                        IDictionary dictionary = attr.ToDictionary();
                        dictionary.Remove( CmsAttributes.ContentType );
                        attr = new Org.BouncyCastle.Asn1.Cms.AttributeTable( dictionary );
                    }
                    authenticatedAttributes = this.outer.GetAttributeSet( attr );
                    new DerOutputStream( stream ).WriteObject( authenticatedAttributes );
                }
                else
                    content?.Write( stream );
                Platform.Dispose( stream );
                byte[] str = ((IBlockResult)calculator.GetResult()).Collect();
                Asn1Set unauthenticatedAttributes = null;
                if (this.unsAttr != null)
                {
                    IDictionary baseParameters = this.outer.GetBaseParameters( contentType, digestAlgorithmId, hash );
                    baseParameters[CmsAttributeTableParameter.Signature] = str.Clone();
                    unauthenticatedAttributes = this.outer.GetAttributeSet( this.unsAttr.GetAttributes( baseParameters ) );
                }
                Asn1Encodable defaultX509Parameters = SignerUtilities.GetDefaultX509Parameters( algorithm );
                AlgorithmIdentifier algorithmIdentifier = Helper.GetEncAlgorithmIdentifier( new DerObjectIdentifier( this.encOID ), defaultX509Parameters );
                return new SignerInfo( this.signerIdentifier, digestAlgorithmId, authenticatedAttributes, algorithmIdentifier, new DerOctetString( str ), unauthenticatedAttributes );
            }
        }
    }
}
