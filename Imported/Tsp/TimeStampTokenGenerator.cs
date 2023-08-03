// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampTokenGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampTokenGenerator
    {
        private int accuracySeconds = -1;
        private int accuracyMillis = -1;
        private int accuracyMicros = -1;
        private bool ordering = false;
        private GeneralName tsa = null;
        private string tsaPolicyOID;
        private AsymmetricKeyParameter key;
        private X509Certificate cert;
        private string digestOID;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr;
        private Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr;
        private IX509Store x509Certs;
        private IX509Store x509Crls;

        public TimeStampTokenGenerator(
          AsymmetricKeyParameter key,
          X509Certificate cert,
          string digestOID,
          string tsaPolicyOID )
          : this( key, cert, digestOID, tsaPolicyOID, null, null )
        {
        }

        public TimeStampTokenGenerator(
          AsymmetricKeyParameter key,
          X509Certificate cert,
          string digestOID,
          string tsaPolicyOID,
          Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr,
          Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr )
        {
            this.key = key;
            this.cert = cert;
            this.digestOID = digestOID;
            this.tsaPolicyOID = tsaPolicyOID;
            this.unsignedAttr = unsignedAttr;
            TspUtil.ValidateCertificate( cert );
            IDictionary attrs = signedAttr == null ? Platform.CreateHashtable() : signedAttr.ToDictionary();
            try
            {
                EssCertID essCertID = new( DigestUtilities.CalculateDigest( "SHA-1", cert.GetEncoded() ) );
                Org.BouncyCastle.Asn1.Cms.Attribute attribute = new( PkcsObjectIdentifiers.IdAASigningCertificate, new DerSet( new SigningCertificate( essCertID ) ) );
                attrs[attribute.AttrType] = attribute;
            }
            catch (CertificateEncodingException ex)
            {
                throw new TspException( "Exception processing certificate.", ex );
            }
            catch (SecurityUtilityException ex)
            {
                throw new TspException( "Can't find a SHA-1 implementation.", ex );
            }
            this.signedAttr = new Org.BouncyCastle.Asn1.Cms.AttributeTable( attrs );
        }

        public void SetCertificates( IX509Store certificates ) => this.x509Certs = certificates;

        public void SetCrls( IX509Store crls ) => this.x509Crls = crls;

        public void SetAccuracySeconds( int accuracySeconds ) => this.accuracySeconds = accuracySeconds;

        public void SetAccuracyMillis( int accuracyMillis ) => this.accuracyMillis = accuracyMillis;

        public void SetAccuracyMicros( int accuracyMicros ) => this.accuracyMicros = accuracyMicros;

        public void SetOrdering( bool ordering ) => this.ordering = ordering;

        public void SetTsa( GeneralName tsa ) => this.tsa = tsa;

        public TimeStampToken Generate(
          TimeStampRequest request,
          BigInteger serialNumber,
          DateTime genTime )
        {
            MessageImprint messageImprint = new( new AlgorithmIdentifier( new DerObjectIdentifier( request.MessageImprintAlgOid ), DerNull.Instance ), request.GetMessageImprintDigest() );
            Accuracy accuracy = null;
            if (this.accuracySeconds > 0 || this.accuracyMillis > 0 || this.accuracyMicros > 0)
            {
                DerInteger seconds = null;
                if (this.accuracySeconds > 0)
                    seconds = new DerInteger( this.accuracySeconds );
                DerInteger millis = null;
                if (this.accuracyMillis > 0)
                    millis = new DerInteger( this.accuracyMillis );
                DerInteger micros = null;
                if (this.accuracyMicros > 0)
                    micros = new DerInteger( this.accuracyMicros );
                accuracy = new Accuracy( seconds, millis, micros );
            }
            DerBoolean ordering = null;
            if (this.ordering)
                ordering = DerBoolean.GetInstance( this.ordering );
            DerInteger nonce = null;
            if (request.Nonce != null)
                nonce = new DerInteger( request.Nonce );
            DerObjectIdentifier tsaPolicyId = new( this.tsaPolicyOID );
            if (request.ReqPolicy != null)
                tsaPolicyId = new DerObjectIdentifier( request.ReqPolicy );
            TstInfo tstInfo = new( tsaPolicyId, messageImprint, new DerInteger( serialNumber ), new DerGeneralizedTime( genTime ), accuracy, ordering, nonce, this.tsa, request.Extensions );
            try
            {
                CmsSignedDataGenerator signedDataGenerator = new();
                byte[] derEncoded = tstInfo.GetDerEncoded();
                if (request.CertReq)
                    signedDataGenerator.AddCertificates( this.x509Certs );
                signedDataGenerator.AddCrls( this.x509Crls );
                signedDataGenerator.AddSigner( this.key, this.cert, this.digestOID, this.signedAttr, this.unsignedAttr );
                return new TimeStampToken( signedDataGenerator.Generate( PkcsObjectIdentifiers.IdCTTstInfo.Id, new CmsProcessableByteArray( derEncoded ), true ) );
            }
            catch (CmsException ex)
            {
                throw new TspException( "Error generating time-stamp token", ex );
            }
            catch (IOException ex)
            {
                throw new TspException( "Exception encoding info", ex );
            }
            catch (X509StoreException ex)
            {
                throw new TspException( "Exception handling CertStore", ex );
            }
        }
    }
}
