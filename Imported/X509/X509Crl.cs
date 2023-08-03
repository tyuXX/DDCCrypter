// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509Crl
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.X509
{
    public class X509Crl : X509ExtensionBase
    {
        private readonly CertificateList c;
        private readonly string sigAlgName;
        private readonly byte[] sigAlgParams;
        private readonly bool isIndirect;

        public X509Crl( CertificateList c )
        {
            this.c = c;
            try
            {
                this.sigAlgName = X509SignatureUtilities.GetSignatureName( c.SignatureAlgorithm );
                this.sigAlgParams = c.SignatureAlgorithm.Parameters == null ? null : c.SignatureAlgorithm.Parameters.GetDerEncoded();
                this.isIndirect = this.IsIndirectCrl;
            }
            catch (Exception ex)
            {
                throw new CrlException( "CRL contents invalid: " + ex );
            }
        }

        protected override X509Extensions GetX509Extensions() => this.c.Version < 2 ? null : this.c.TbsCertList.Extensions;

        public virtual byte[] GetEncoded()
        {
            try
            {
                return this.c.GetDerEncoded();
            }
            catch (Exception ex)
            {
                throw new CrlException( ex.ToString() );
            }
        }

        public virtual void Verify( AsymmetricKeyParameter publicKey ) => this.Verify( new Asn1VerifierFactoryProvider( publicKey ) );

        public virtual void Verify( IVerifierFactoryProvider verifierProvider ) => this.CheckSignature( verifierProvider.CreateVerifierFactory( c.SignatureAlgorithm ) );

        protected virtual void CheckSignature( IVerifierFactory verifier )
        {
            if (!this.c.SignatureAlgorithm.Equals( c.TbsCertList.Signature ))
                throw new CrlException( "Signature algorithm on CertificateList does not match TbsCertList." );
            Asn1Encodable parameters = this.c.SignatureAlgorithm.Parameters;
            IStreamCalculator calculator = verifier.CreateCalculator();
            byte[] tbsCertList = this.GetTbsCertList();
            calculator.Stream.Write( tbsCertList, 0, tbsCertList.Length );
            Platform.Dispose( calculator.Stream );
            if (!((IVerifier)calculator.GetResult()).IsVerified( this.GetSignature() ))
                throw new InvalidKeyException( "CRL does not verify with supplied public key." );
        }

        public virtual int Version => this.c.Version;

        public virtual X509Name IssuerDN => this.c.Issuer;

        public virtual DateTime ThisUpdate => this.c.ThisUpdate.ToDateTime();

        public virtual DateTimeObject NextUpdate => this.c.NextUpdate != null ? new DateTimeObject( this.c.NextUpdate.ToDateTime() ) : null;

        private ISet LoadCrlEntries()
        {
            ISet set = new HashSet();
            IEnumerable certificateEnumeration = this.c.GetRevokedCertificateEnumeration();
            X509Name previousCertificateIssuer = this.IssuerDN;
            foreach (CrlEntry c in certificateEnumeration)
            {
                X509CrlEntry o = new X509CrlEntry( c, this.isIndirect, previousCertificateIssuer );
                set.Add( o );
                previousCertificateIssuer = o.GetCertificateIssuer();
            }
            return set;
        }

        public virtual X509CrlEntry GetRevokedCertificate( BigInteger serialNumber )
        {
            IEnumerable certificateEnumeration = this.c.GetRevokedCertificateEnumeration();
            X509Name previousCertificateIssuer = this.IssuerDN;
            foreach (CrlEntry c in certificateEnumeration)
            {
                X509CrlEntry revokedCertificate = new X509CrlEntry( c, this.isIndirect, previousCertificateIssuer );
                if (serialNumber.Equals( c.UserCertificate.Value ))
                    return revokedCertificate;
                previousCertificateIssuer = revokedCertificate.GetCertificateIssuer();
            }
            return null;
        }

        public virtual ISet GetRevokedCertificates()
        {
            ISet set = this.LoadCrlEntries();
            return set.Count > 0 ? set : null;
        }

        public virtual byte[] GetTbsCertList()
        {
            try
            {
                return this.c.TbsCertList.GetDerEncoded();
            }
            catch (Exception ex)
            {
                throw new CrlException( ex.ToString() );
            }
        }

        public virtual byte[] GetSignature() => this.c.GetSignatureOctets();

        public virtual string SigAlgName => this.sigAlgName;

        public virtual string SigAlgOid => this.c.SignatureAlgorithm.Algorithm.Id;

        public virtual byte[] GetSigAlgParams() => Arrays.Clone( this.sigAlgParams );

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is X509Crl x509Crl && this.c.Equals( x509Crl.c );
        }

        public override int GetHashCode() => this.c.GetHashCode();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string newLine = Platform.NewLine;
            stringBuilder.Append( "              Version: " ).Append( this.Version ).Append( newLine );
            stringBuilder.Append( "             IssuerDN: " ).Append( IssuerDN ).Append( newLine );
            stringBuilder.Append( "          This update: " ).Append( ThisUpdate ).Append( newLine );
            stringBuilder.Append( "          Next update: " ).Append( NextUpdate ).Append( newLine );
            stringBuilder.Append( "  Signature Algorithm: " ).Append( this.SigAlgName ).Append( newLine );
            byte[] signature = this.GetSignature();
            stringBuilder.Append( "            Signature: " );
            stringBuilder.Append( Hex.ToHexString( signature, 0, 20 ) ).Append( newLine );
            for (int off = 20; off < signature.Length; off += 20)
            {
                int length = System.Math.Min( 20, signature.Length - off );
                stringBuilder.Append( "                       " );
                stringBuilder.Append( Hex.ToHexString( signature, off, length ) ).Append( newLine );
            }
            X509Extensions extensions = this.c.TbsCertList.Extensions;
            if (extensions != null)
            {
                IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
                if (enumerator.MoveNext())
                    stringBuilder.Append( "           Extensions: " ).Append( newLine );
                do
                {
                    DerObjectIdentifier current = (DerObjectIdentifier)enumerator.Current;
                    X509Extension extension = extensions.GetExtension( current );
                    if (extension.Value != null)
                    {
                        Asn1Object asn1Object = X509ExtensionUtilities.FromExtensionValue( extension.Value );
                        stringBuilder.Append( "                       critical(" ).Append( extension.IsCritical ).Append( ") " );
                        try
                        {
                            if (current.Equals( X509Extensions.CrlNumber ))
                                stringBuilder.Append( new CrlNumber( DerInteger.GetInstance( asn1Object ).PositiveValue ) ).Append( newLine );
                            else if (current.Equals( X509Extensions.DeltaCrlIndicator ))
                                stringBuilder.Append( "Base CRL: " + new CrlNumber( DerInteger.GetInstance( asn1Object ).PositiveValue ) ).Append( newLine );
                            else if (current.Equals( X509Extensions.IssuingDistributionPoint ))
                                stringBuilder.Append( IssuingDistributionPoint.GetInstance( (Asn1Sequence)asn1Object ) ).Append( newLine );
                            else if (current.Equals( X509Extensions.CrlDistributionPoints ))
                                stringBuilder.Append( CrlDistPoint.GetInstance( (Asn1Sequence)asn1Object ) ).Append( newLine );
                            else if (current.Equals( X509Extensions.FreshestCrl ))
                            {
                                stringBuilder.Append( CrlDistPoint.GetInstance( (Asn1Sequence)asn1Object ) ).Append( newLine );
                            }
                            else
                            {
                                stringBuilder.Append( current.Id );
                                stringBuilder.Append( " value = " ).Append( Asn1Dump.DumpAsString( asn1Object ) ).Append( newLine );
                            }
                        }
                        catch (Exception ex)
                        {
                            stringBuilder.Append( current.Id );
                            stringBuilder.Append( " value = " ).Append( "*****" ).Append( newLine );
                        }
                    }
                    else
                        stringBuilder.Append( newLine );
                }
                while (enumerator.MoveNext());
            }
            ISet revokedCertificates = this.GetRevokedCertificates();
            if (revokedCertificates != null)
            {
                foreach (X509CrlEntry x509CrlEntry in (IEnumerable)revokedCertificates)
                {
                    stringBuilder.Append( x509CrlEntry );
                    stringBuilder.Append( newLine );
                }
            }
            return stringBuilder.ToString();
        }

        public virtual bool IsRevoked( X509Certificate cert )
        {
            CrlEntry[] revokedCertificates = this.c.GetRevokedCertificates();
            if (revokedCertificates != null)
            {
                BigInteger serialNumber = cert.SerialNumber;
                for (int index = 0; index < revokedCertificates.Length; ++index)
                {
                    if (revokedCertificates[index].UserCertificate.Value.Equals( serialNumber ))
                        return true;
                }
            }
            return false;
        }

        protected virtual bool IsIndirectCrl
        {
            get
            {
                Asn1OctetString extensionValue = this.GetExtensionValue( X509Extensions.IssuingDistributionPoint );
                bool isIndirectCrl = false;
                try
                {
                    if (extensionValue != null)
                        isIndirectCrl = IssuingDistributionPoint.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) ).IsIndirectCrl;
                }
                catch (Exception ex)
                {
                    throw new CrlException( "Exception reading IssuingDistributionPoint" + ex );
                }
                return isIndirectCrl;
            }
        }
    }
}
