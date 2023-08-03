// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509V2AttributeCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.X509
{
    public class X509V2AttributeCertificate :
    X509ExtensionBase,
    IX509AttributeCertificate,
    IX509Extension
    {
        private readonly AttributeCertificate cert;
        private readonly DateTime notBefore;
        private readonly DateTime notAfter;

        private static AttributeCertificate GetObject( Stream input )
        {
            try
            {
                return AttributeCertificate.GetInstance( Asn1Object.FromStream( input ) );
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IOException( "exception decoding certificate structure", ex );
            }
        }

        public X509V2AttributeCertificate( Stream encIn )
          : this( GetObject( encIn ) )
        {
        }

        public X509V2AttributeCertificate( byte[] encoded )
          : this( new MemoryStream( encoded, false ) )
        {
        }

        internal X509V2AttributeCertificate( AttributeCertificate cert )
        {
            this.cert = cert;
            try
            {
                this.notAfter = cert.ACInfo.AttrCertValidityPeriod.NotAfterTime.ToDateTime();
                this.notBefore = cert.ACInfo.AttrCertValidityPeriod.NotBeforeTime.ToDateTime();
            }
            catch (Exception ex)
            {
                throw new IOException( "invalid data structure in certificate!", ex );
            }
        }

        public virtual int Version => this.cert.ACInfo.Version.Value.IntValue + 1;

        public virtual BigInteger SerialNumber => this.cert.ACInfo.SerialNumber.Value;

        public virtual AttributeCertificateHolder Holder => new AttributeCertificateHolder( (Asn1Sequence)this.cert.ACInfo.Holder.ToAsn1Object() );

        public virtual AttributeCertificateIssuer Issuer => new AttributeCertificateIssuer( this.cert.ACInfo.Issuer );

        public virtual DateTime NotBefore => this.notBefore;

        public virtual DateTime NotAfter => this.notAfter;

        public virtual bool[] GetIssuerUniqueID()
        {
            DerBitString issuerUniqueId1 = this.cert.ACInfo.IssuerUniqueID;
            if (issuerUniqueId1 == null)
                return null;
            byte[] bytes = issuerUniqueId1.GetBytes();
            bool[] issuerUniqueId2 = new bool[(bytes.Length * 8) - issuerUniqueId1.PadBits];
            for (int index = 0; index != issuerUniqueId2.Length; ++index)
                issuerUniqueId2[index] = (bytes[index / 8] & (128 >> (index % 8))) != 0;
            return issuerUniqueId2;
        }

        public virtual bool IsValidNow => this.IsValid( DateTime.UtcNow );

        public virtual bool IsValid( DateTime date ) => date.CompareTo( (object)this.NotBefore ) >= 0 && date.CompareTo( (object)this.NotAfter ) <= 0;

        public virtual void CheckValidity() => this.CheckValidity( DateTime.UtcNow );

        public virtual void CheckValidity( DateTime date )
        {
            if (date.CompareTo( (object)this.NotAfter ) > 0)
                throw new CertificateExpiredException( "certificate expired on " + NotAfter );
            if (date.CompareTo( (object)this.NotBefore ) < 0)
                throw new CertificateNotYetValidException( "certificate not valid until " + NotBefore );
        }

        public virtual AlgorithmIdentifier SignatureAlgorithm => this.cert.SignatureAlgorithm;

        public virtual byte[] GetSignature() => this.cert.GetSignatureOctets();

        public virtual void Verify( AsymmetricKeyParameter key ) => this.CheckSignature( new Asn1VerifierFactory( this.cert.SignatureAlgorithm, key ) );

        public virtual void Verify( IVerifierFactoryProvider verifierProvider ) => this.CheckSignature( verifierProvider.CreateVerifierFactory( cert.SignatureAlgorithm ) );

        protected virtual void CheckSignature( IVerifierFactory verifier )
        {
            if (!this.cert.SignatureAlgorithm.Equals( cert.ACInfo.Signature ))
                throw new CertificateException( "Signature algorithm in certificate info not same as outer certificate" );
            IStreamCalculator calculator = verifier.CreateCalculator();
            try
            {
                byte[] encoded = this.cert.ACInfo.GetEncoded();
                calculator.Stream.Write( encoded, 0, encoded.Length );
                Platform.Dispose( calculator.Stream );
            }
            catch (IOException ex)
            {
                throw new SignatureException( "Exception encoding certificate info object", ex );
            }
            if (!((IVerifier)calculator.GetResult()).IsVerified( this.GetSignature() ))
                throw new InvalidKeyException( "Public key presented not for certificate signature" );
        }

        public virtual byte[] GetEncoded() => this.cert.GetEncoded();

        protected override X509Extensions GetX509Extensions() => this.cert.ACInfo.Extensions;

        public virtual X509Attribute[] GetAttributes()
        {
            Asn1Sequence attributes1 = this.cert.ACInfo.Attributes;
            X509Attribute[] attributes2 = new X509Attribute[attributes1.Count];
            for (int index = 0; index != attributes1.Count; ++index)
                attributes2[index] = new X509Attribute( attributes1[index] );
            return attributes2;
        }

        public virtual X509Attribute[] GetAttributes( string oid )
        {
            Asn1Sequence attributes1 = this.cert.ACInfo.Attributes;
            IList arrayList = Platform.CreateArrayList();
            for (int index = 0; index != attributes1.Count; ++index)
            {
                X509Attribute x509Attribute = new X509Attribute( attributes1[index] );
                if (x509Attribute.Oid.Equals( oid ))
                    arrayList.Add( x509Attribute );
            }
            if (arrayList.Count < 1)
                return null;
            X509Attribute[] attributes2 = new X509Attribute[arrayList.Count];
            for (int index = 0; index < arrayList.Count; ++index)
                attributes2[index] = (X509Attribute)arrayList[index];
            return attributes2;
        }

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is X509V2AttributeCertificate attributeCertificate && this.cert.Equals( attributeCertificate.cert );
        }

        public override int GetHashCode() => this.cert.GetHashCode();
    }
}
