// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SignerInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class SignerInformation
    {
        private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;
        private SignerID sid;
        private Org.BouncyCastle.Asn1.Cms.SignerInfo info;
        private AlgorithmIdentifier digestAlgorithm;
        private AlgorithmIdentifier encryptionAlgorithm;
        private readonly Asn1Set signedAttributeSet;
        private readonly Asn1Set unsignedAttributeSet;
        private CmsProcessable content;
        private byte[] signature;
        private DerObjectIdentifier contentType;
        private IDigestCalculator digestCalculator;
        private byte[] resultDigest;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributeTable;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributeTable;
        private readonly bool isCounterSignature;

        internal SignerInformation(
          Org.BouncyCastle.Asn1.Cms.SignerInfo info,
          DerObjectIdentifier contentType,
          CmsProcessable content,
          IDigestCalculator digestCalculator )
        {
            this.info = info;
            this.sid = new SignerID();
            this.contentType = contentType;
            this.isCounterSignature = contentType == null;
            try
            {
                SignerIdentifier signerId = info.SignerID;
                if (signerId.IsTagged)
                {
                    this.sid.SubjectKeyIdentifier = Asn1OctetString.GetInstance( signerId.ID ).GetEncoded();
                }
                else
                {
                    Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber instance = Asn1.Cms.IssuerAndSerialNumber.GetInstance( signerId.ID );
                    this.sid.Issuer = instance.Name;
                    this.sid.SerialNumber = instance.SerialNumber.Value;
                }
            }
            catch (IOException ex)
            {
                throw new ArgumentException( "invalid sid in SignerInfo" );
            }
            this.digestAlgorithm = info.DigestAlgorithm;
            this.signedAttributeSet = info.AuthenticatedAttributes;
            this.unsignedAttributeSet = info.UnauthenticatedAttributes;
            this.encryptionAlgorithm = info.DigestEncryptionAlgorithm;
            this.signature = info.EncryptedDigest.GetOctets();
            this.content = content;
            this.digestCalculator = digestCalculator;
        }

        public bool IsCounterSignature => this.isCounterSignature;

        public DerObjectIdentifier ContentType => this.contentType;

        public SignerID SignerID => this.sid;

        public int Version => this.info.Version.Value.IntValue;

        public AlgorithmIdentifier DigestAlgorithmID => this.digestAlgorithm;

        public string DigestAlgOid => this.digestAlgorithm.Algorithm.Id;

        public Asn1Object DigestAlgParams => this.digestAlgorithm.Parameters?.ToAsn1Object();

        public byte[] GetContentDigest() => this.resultDigest != null ? (byte[])this.resultDigest.Clone() : throw new InvalidOperationException( "method can only be called after verify." );

        public AlgorithmIdentifier EncryptionAlgorithmID => this.encryptionAlgorithm;

        public string EncryptionAlgOid => this.encryptionAlgorithm.Algorithm.Id;

        public Asn1Object EncryptionAlgParams => this.encryptionAlgorithm.Parameters?.ToAsn1Object();

        public Org.BouncyCastle.Asn1.Cms.AttributeTable SignedAttributes
        {
            get
            {
                if (this.signedAttributeSet != null && this.signedAttributeTable == null)
                    this.signedAttributeTable = new Org.BouncyCastle.Asn1.Cms.AttributeTable( this.signedAttributeSet );
                return this.signedAttributeTable;
            }
        }

        public Org.BouncyCastle.Asn1.Cms.AttributeTable UnsignedAttributes
        {
            get
            {
                if (this.unsignedAttributeSet != null && this.unsignedAttributeTable == null)
                    this.unsignedAttributeTable = new Org.BouncyCastle.Asn1.Cms.AttributeTable( this.unsignedAttributeSet );
                return this.unsignedAttributeTable;
            }
        }

        public byte[] GetSignature() => (byte[])this.signature.Clone();

        public SignerInformationStore GetCounterSignatures()
        {
            Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = this.UnsignedAttributes;
            if (unsignedAttributes == null)
                return new SignerInformationStore( Platform.CreateArrayList( 0 ) );
            IList arrayList = Platform.CreateArrayList();
            foreach (Org.BouncyCastle.Asn1.Cms.Attribute attribute in unsignedAttributes.GetAll( CmsAttributes.CounterSignature ))
            {
                Asn1Set attrValues = attribute.AttrValues;
                int count = attrValues.Count;
                foreach (Asn1Encodable asn1Encodable in attrValues)
                {
                    Org.BouncyCastle.Asn1.Cms.SignerInfo instance = Asn1.Cms.SignerInfo.GetInstance( asn1Encodable.ToAsn1Object() );
                    string digestAlgName = CmsSignedHelper.Instance.GetDigestAlgName( instance.DigestAlgorithm.Algorithm.Id );
                    arrayList.Add( new SignerInformation( instance, null, null, new CounterSignatureDigestCalculator( digestAlgName, this.GetSignature() ) ) );
                }
            }
            return new SignerInformationStore( arrayList );
        }

        public byte[] GetEncodedSignedAttributes() => this.signedAttributeSet != null ? this.signedAttributeSet.GetEncoded( "DER" ) : null;

        private bool DoVerify( AsymmetricKeyParameter key )
        {
            string digestAlgName = Helper.GetDigestAlgName( this.DigestAlgOid );
            IDigest digestInstance = Helper.GetDigestInstance( digestAlgName );
            DerObjectIdentifier algorithm1 = this.encryptionAlgorithm.Algorithm;
            Asn1Encodable parameters = this.encryptionAlgorithm.Parameters;
            ISigner sig;
            if (algorithm1.Equals( PkcsObjectIdentifiers.IdRsassaPss ))
            {
                if (parameters == null)
                    throw new CmsException( "RSASSA-PSS signature must specify algorithm parameters" );
                try
                {
                    RsassaPssParameters instance = RsassaPssParameters.GetInstance( parameters.ToAsn1Object() );
                    if (!instance.HashAlgorithm.Algorithm.Equals( digestAlgorithm.Algorithm ))
                        throw new CmsException( "RSASSA-PSS signature parameters specified incorrect hash algorithm" );
                    if (!instance.MaskGenAlgorithm.Algorithm.Equals( PkcsObjectIdentifiers.IdMgf1 ))
                        throw new CmsException( "RSASSA-PSS signature parameters specified unknown MGF" );
                    IDigest digest = DigestUtilities.GetDigest( instance.HashAlgorithm.Algorithm );
                    int intValue = instance.SaltLength.Value.IntValue;
                    if ((byte)instance.TrailerField.Value.IntValue != 1)
                        throw new CmsException( "RSASSA-PSS signature parameters must have trailerField of 1" );
                    sig = new PssSigner( new RsaBlindedEngine(), digest, intValue );
                }
                catch (Exception ex)
                {
                    throw new CmsException( "failed to set RSASSA-PSS signature parameters", ex );
                }
            }
            else
            {
                string algorithm2 = digestAlgName + "with" + Helper.GetEncryptionAlgName( this.EncryptionAlgOid );
                sig = Helper.GetSignatureInstance( algorithm2 );
            }
            try
            {
                if (this.digestCalculator != null)
                {
                    this.resultDigest = this.digestCalculator.GetDigest();
                }
                else
                {
                    if (this.content != null)
                        this.content.Write( new DigOutputStream( digestInstance ) );
                    else if (this.signedAttributeSet == null)
                        throw new CmsException( "data not encapsulated in signature - use detached constructor." );
                    this.resultDigest = DigestUtilities.DoFinal( digestInstance );
                }
            }
            catch (IOException ex)
            {
                throw new CmsException( "can't process mime object to create signature.", ex );
            }
            Asn1Object valuedSignedAttribute1 = this.GetSingleValuedSignedAttribute( CmsAttributes.ContentType, "content-type" );
            if (valuedSignedAttribute1 == null)
            {
                if (!this.isCounterSignature && this.signedAttributeSet != null)
                    throw new CmsException( "The content-type attribute type MUST be present whenever signed attributes are present in signed-data" );
            }
            else
            {
                if (this.isCounterSignature)
                    throw new CmsException( "[For counter signatures,] the signedAttributes field MUST NOT contain a content-type attribute" );
                if (!(valuedSignedAttribute1 is DerObjectIdentifier))
                    throw new CmsException( "content-type attribute value not of ASN.1 type 'OBJECT IDENTIFIER'" );
                if (!valuedSignedAttribute1.Equals( contentType ))
                    throw new CmsException( "content-type attribute value does not match eContentType" );
            }
            Asn1Object valuedSignedAttribute2 = this.GetSingleValuedSignedAttribute( CmsAttributes.MessageDigest, "message-digest" );
            if (valuedSignedAttribute2 == null)
            {
                if (this.signedAttributeSet != null)
                    throw new CmsException( "the message-digest signed attribute type MUST be present when there are any signed attributes present" );
            }
            else
            {
                if (!(valuedSignedAttribute2 is Asn1OctetString))
                    throw new CmsException( "message-digest attribute value not of ASN.1 type 'OCTET STRING'" );
                if (!Arrays.AreEqual( this.resultDigest, ((Asn1OctetString)valuedSignedAttribute2).GetOctets() ))
                    throw new CmsException( "message-digest attribute value does not match calculated value" );
            }
            Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributes1 = this.SignedAttributes;
            if (signedAttributes1 != null && signedAttributes1.GetAll( CmsAttributes.CounterSignature ).Count > 0)
                throw new CmsException( "A countersignature attribute MUST NOT be a signed attribute" );
            Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = this.UnsignedAttributes;
            if (unsignedAttributes != null)
            {
                foreach (Org.BouncyCastle.Asn1.Cms.Attribute attribute in unsignedAttributes.GetAll( CmsAttributes.CounterSignature ))
                {
                    if (attribute.AttrValues.Count < 1)
                        throw new CmsException( "A countersignature attribute MUST contain at least one AttributeValue" );
                }
            }
            try
            {
                sig.Init( false, key );
                if (this.signedAttributeSet == null)
                {
                    if (this.digestCalculator != null)
                        return this.VerifyDigest( this.resultDigest, key, this.GetSignature() );
                    if (this.content != null)
                        this.content.Write( new SigOutputStream( sig ) );
                }
                else
                {
                    byte[] signedAttributes2 = this.GetEncodedSignedAttributes();
                    sig.BlockUpdate( signedAttributes2, 0, signedAttributes2.Length );
                }
                return sig.VerifySignature( this.GetSignature() );
            }
            catch (InvalidKeyException ex)
            {
                throw new CmsException( "key not appropriate to signature in message.", ex );
            }
            catch (IOException ex)
            {
                throw new CmsException( "can't process mime object to create signature.", ex );
            }
            catch (SignatureException ex)
            {
                throw new CmsException( "invalid signature format in message: " + ex.Message, ex );
            }
        }

        private bool IsNull( Asn1Encodable o ) => o is Asn1Null || o == null;

        private DigestInfo DerDecode( byte[] encoding )
        {
            DigestInfo digestInfo = encoding[0] == 48 ? DigestInfo.GetInstance( Asn1Object.FromByteArray( encoding ) ) : throw new IOException( "not a digest info object" );
            if (digestInfo.GetEncoded().Length != encoding.Length)
                throw new CmsException( "malformed RSA signature" );
            return digestInfo;
        }

        private bool VerifyDigest( byte[] digest, AsymmetricKeyParameter key, byte[] signature )
        {
            string encryptionAlgName = Helper.GetEncryptionAlgName( this.EncryptionAlgOid );
            try
            {
                switch (encryptionAlgName)
                {
                    case "RSA":
                        IBufferedCipher asymmetricCipher = CmsEnvelopedHelper.Instance.CreateAsymmetricCipher( "RSA/ECB/PKCS1Padding" );
                        asymmetricCipher.Init( false, key );
                        DigestInfo digestInfo = this.DerDecode( asymmetricCipher.DoFinal( signature ) );
                        if (!digestInfo.AlgorithmID.Algorithm.Equals( digestAlgorithm.Algorithm ) || !this.IsNull( digestInfo.AlgorithmID.Parameters ))
                            return false;
                        byte[] digest1 = digestInfo.GetDigest();
                        return Arrays.ConstantTimeAreEqual( digest, digest1 );
                    case "DSA":
                        ISigner signer = SignerUtilities.GetSigner( "NONEwithDSA" );
                        signer.Init( false, key );
                        signer.BlockUpdate( digest, 0, digest.Length );
                        return signer.VerifySignature( signature );
                    default:
                        throw new CmsException( "algorithm: " + encryptionAlgName + " not supported in base signatures." );
                }
            }
            catch (SecurityUtilityException ex)
            {
                throw ex;
            }
            catch (GeneralSecurityException ex)
            {
                throw new CmsException( "Exception processing signature: " + ex, ex );
            }
            catch (IOException ex)
            {
                throw new CmsException( "Exception decoding signature: " + ex, ex );
            }
        }

        public bool Verify( AsymmetricKeyParameter pubKey )
        {
            if (pubKey.IsPrivate)
                throw new ArgumentException( "Expected public key", nameof( pubKey ) );
            this.GetSigningTime();
            return this.DoVerify( pubKey );
        }

        public bool Verify( X509Certificate cert )
        {
            Org.BouncyCastle.Asn1.Cms.Time signingTime = this.GetSigningTime();
            if (signingTime != null)
                cert.CheckValidity( signingTime.Date );
            return this.DoVerify( cert.GetPublicKey() );
        }

        public Org.BouncyCastle.Asn1.Cms.SignerInfo ToSignerInfo() => this.info;

        private Asn1Object GetSingleValuedSignedAttribute(
          DerObjectIdentifier attrOID,
          string printableName )
        {
            Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = this.UnsignedAttributes;
            if (unsignedAttributes != null && unsignedAttributes.GetAll( attrOID ).Count > 0)
                throw new CmsException( "The " + printableName + " attribute MUST NOT be an unsigned attribute" );
            Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributes = this.SignedAttributes;
            if (signedAttributes == null)
                return null;
            Asn1EncodableVector all = signedAttributes.GetAll( attrOID );
            switch (all.Count)
            {
                case 0:
                    return null;
                case 1:
                    Asn1Set attrValues = ((Org.BouncyCastle.Asn1.Cms.Attribute)all[0]).AttrValues;
                    return attrValues.Count == 1 ? attrValues[0].ToAsn1Object() : throw new CmsException( "A " + printableName + " attribute MUST have a single attribute value" );
                default:
                    throw new CmsException( "The SignedAttributes in a signerInfo MUST NOT include multiple instances of the " + printableName + " attribute" );
            }
        }

        private Org.BouncyCastle.Asn1.Cms.Time GetSigningTime()
        {
            Asn1Object valuedSignedAttribute = this.GetSingleValuedSignedAttribute( CmsAttributes.SigningTime, "signing-time" );
            if (valuedSignedAttribute == null)
                return null;
            try
            {
                return Asn1.Cms.Time.GetInstance( valuedSignedAttribute );
            }
            catch (ArgumentException ex)
            {
                throw new CmsException( "signing-time attribute value not a valid 'Time' structure" );
            }
        }

        public static SignerInformation ReplaceUnsignedAttributes(
          SignerInformation signerInformation,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes )
        {
            Org.BouncyCastle.Asn1.Cms.SignerInfo info = signerInformation.info;
            Asn1Set unauthenticatedAttributes = null;
            if (unsignedAttributes != null)
                unauthenticatedAttributes = new DerSet( unsignedAttributes.ToAsn1EncodableVector() );
            return new SignerInformation( new Org.BouncyCastle.Asn1.Cms.SignerInfo( info.SignerID, info.DigestAlgorithm, info.AuthenticatedAttributes, info.DigestEncryptionAlgorithm, info.EncryptedDigest, unauthenticatedAttributes ), signerInformation.contentType, signerInformation.content, null );
        }

        public static SignerInformation AddCounterSigners(
          SignerInformation signerInformation,
          SignerInformationStore counterSigners )
        {
            Org.BouncyCastle.Asn1.Cms.SignerInfo info = signerInformation.info;
            Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = signerInformation.UnsignedAttributes;
            Asn1EncodableVector v1 = unsignedAttributes == null ? new Asn1EncodableVector( new Asn1Encodable[0] ) : unsignedAttributes.ToAsn1EncodableVector();
            Asn1EncodableVector v2 = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (SignerInformation signer in (IEnumerable)counterSigners.GetSigners())
                v2.Add( signer.ToSignerInfo() );
            v1.Add( new Org.BouncyCastle.Asn1.Cms.Attribute( CmsAttributes.CounterSignature, new DerSet( v2 ) ) );
            return new SignerInformation( new Org.BouncyCastle.Asn1.Cms.SignerInfo( info.SignerID, info.DigestAlgorithm, info.AuthenticatedAttributes, info.DigestEncryptionAlgorithm, info.EncryptedDigest, new DerSet( v1 ) ), signerInformation.contentType, signerInformation.content, null );
        }
    }
}
