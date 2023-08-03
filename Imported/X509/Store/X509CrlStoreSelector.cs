// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509CrlStoreSelector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.X509.Extension;
using System.Collections;

namespace Org.BouncyCastle.X509.Store
{
    public class X509CrlStoreSelector : IX509Selector, ICloneable
    {
        private X509Certificate certificateChecking;
        private DateTimeObject dateAndTime;
        private ICollection issuers;
        private BigInteger maxCrlNumber;
        private BigInteger minCrlNumber;
        private IX509AttributeCertificate attrCertChecking;
        private bool completeCrlEnabled;
        private bool deltaCrlIndicatorEnabled;
        private byte[] issuingDistributionPoint;
        private bool issuingDistributionPointEnabled;
        private BigInteger maxBaseCrlNumber;

        public X509CrlStoreSelector()
        {
        }

        public X509CrlStoreSelector( X509CrlStoreSelector o )
        {
            this.certificateChecking = o.CertificateChecking;
            this.dateAndTime = o.DateAndTime;
            this.issuers = o.Issuers;
            this.maxCrlNumber = o.MaxCrlNumber;
            this.minCrlNumber = o.MinCrlNumber;
            this.deltaCrlIndicatorEnabled = o.DeltaCrlIndicatorEnabled;
            this.completeCrlEnabled = o.CompleteCrlEnabled;
            this.maxBaseCrlNumber = o.MaxBaseCrlNumber;
            this.attrCertChecking = o.AttrCertChecking;
            this.issuingDistributionPointEnabled = o.IssuingDistributionPointEnabled;
            this.issuingDistributionPoint = o.IssuingDistributionPoint;
        }

        public virtual object Clone() => new X509CrlStoreSelector( this );

        public X509Certificate CertificateChecking
        {
            get => this.certificateChecking;
            set => this.certificateChecking = value;
        }

        public DateTimeObject DateAndTime
        {
            get => this.dateAndTime;
            set => this.dateAndTime = value;
        }

        public ICollection Issuers
        {
            get => Platform.CreateArrayList( this.issuers );
            set => this.issuers = Platform.CreateArrayList( value );
        }

        public BigInteger MaxCrlNumber
        {
            get => this.maxCrlNumber;
            set => this.maxCrlNumber = value;
        }

        public BigInteger MinCrlNumber
        {
            get => this.minCrlNumber;
            set => this.minCrlNumber = value;
        }

        public IX509AttributeCertificate AttrCertChecking
        {
            get => this.attrCertChecking;
            set => this.attrCertChecking = value;
        }

        public bool CompleteCrlEnabled
        {
            get => this.completeCrlEnabled;
            set => this.completeCrlEnabled = value;
        }

        public bool DeltaCrlIndicatorEnabled
        {
            get => this.deltaCrlIndicatorEnabled;
            set => this.deltaCrlIndicatorEnabled = value;
        }

        public byte[] IssuingDistributionPoint
        {
            get => Arrays.Clone( this.issuingDistributionPoint );
            set => this.issuingDistributionPoint = Arrays.Clone( value );
        }

        public bool IssuingDistributionPointEnabled
        {
            get => this.issuingDistributionPointEnabled;
            set => this.issuingDistributionPointEnabled = value;
        }

        public BigInteger MaxBaseCrlNumber
        {
            get => this.maxBaseCrlNumber;
            set => this.maxBaseCrlNumber = value;
        }

        public virtual bool Match( object obj )
        {
            if (!(obj is X509Crl x509Crl))
                return false;
            if (this.dateAndTime != null)
            {
                DateTime dateTime = this.dateAndTime.Value;
                DateTime thisUpdate = x509Crl.ThisUpdate;
                DateTimeObject nextUpdate = x509Crl.NextUpdate;
                if (dateTime.CompareTo( (object)thisUpdate ) < 0 || nextUpdate == null || dateTime.CompareTo( (object)nextUpdate.Value ) >= 0)
                    return false;
            }
            if (this.issuers != null)
            {
                X509Name issuerDn = x509Crl.IssuerDN;
                bool flag = false;
                foreach (X509Name issuer in (IEnumerable)this.issuers)
                {
                    if (issuer.Equivalent( issuerDn, true ))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return false;
            }
            if (this.maxCrlNumber != null || this.minCrlNumber != null)
            {
                Asn1OctetString extensionValue = x509Crl.GetExtensionValue( X509Extensions.CrlNumber );
                if (extensionValue == null)
                    return false;
                BigInteger positiveValue = DerInteger.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) ).PositiveValue;
                if ((this.maxCrlNumber != null && positiveValue.CompareTo( this.maxCrlNumber ) > 0) || (this.minCrlNumber != null && positiveValue.CompareTo( this.minCrlNumber ) < 0))
                    return false;
            }
            DerInteger derInteger = null;
            try
            {
                Asn1OctetString extensionValue = x509Crl.GetExtensionValue( X509Extensions.DeltaCrlIndicator );
                if (extensionValue != null)
                    derInteger = DerInteger.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
            }
            catch (Exception ex)
            {
                return false;
            }
            if (derInteger == null)
            {
                if (this.DeltaCrlIndicatorEnabled)
                    return false;
            }
            else if (this.CompleteCrlEnabled || (this.maxBaseCrlNumber != null && derInteger.PositiveValue.CompareTo( this.maxBaseCrlNumber ) > 0))
                return false;
            if (this.issuingDistributionPointEnabled)
            {
                Asn1OctetString extensionValue = x509Crl.GetExtensionValue( X509Extensions.IssuingDistributionPoint );
                if (this.issuingDistributionPoint == null)
                {
                    if (extensionValue != null)
                        return false;
                }
                else if (!Arrays.AreEqual( extensionValue.GetOctets(), this.issuingDistributionPoint ))
                    return false;
            }
            return true;
        }
    }
}
