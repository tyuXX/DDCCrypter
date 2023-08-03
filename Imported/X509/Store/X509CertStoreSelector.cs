// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509CertStoreSelector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;

namespace Org.BouncyCastle.X509.Store
{
    public class X509CertStoreSelector : IX509Selector, ICloneable
    {
        private byte[] authorityKeyIdentifier;
        private int basicConstraints = -1;
        private X509Certificate certificate;
        private DateTimeObject certificateValid;
        private ISet extendedKeyUsage;
        private X509Name issuer;
        private bool[] keyUsage;
        private ISet policy;
        private DateTimeObject privateKeyValid;
        private BigInteger serialNumber;
        private X509Name subject;
        private byte[] subjectKeyIdentifier;
        private SubjectPublicKeyInfo subjectPublicKey;
        private DerObjectIdentifier subjectPublicKeyAlgID;

        public X509CertStoreSelector()
        {
        }

        public X509CertStoreSelector( X509CertStoreSelector o )
        {
            this.authorityKeyIdentifier = o.AuthorityKeyIdentifier;
            this.basicConstraints = o.BasicConstraints;
            this.certificate = o.Certificate;
            this.certificateValid = o.CertificateValid;
            this.extendedKeyUsage = o.ExtendedKeyUsage;
            this.issuer = o.Issuer;
            this.keyUsage = o.KeyUsage;
            this.policy = o.Policy;
            this.privateKeyValid = o.PrivateKeyValid;
            this.serialNumber = o.SerialNumber;
            this.subject = o.Subject;
            this.subjectKeyIdentifier = o.SubjectKeyIdentifier;
            this.subjectPublicKey = o.SubjectPublicKey;
            this.subjectPublicKeyAlgID = o.SubjectPublicKeyAlgID;
        }

        public virtual object Clone() => new X509CertStoreSelector( this );

        public byte[] AuthorityKeyIdentifier
        {
            get => Arrays.Clone( this.authorityKeyIdentifier );
            set => this.authorityKeyIdentifier = Arrays.Clone( value );
        }

        public int BasicConstraints
        {
            get => this.basicConstraints;
            set => this.basicConstraints = value >= -2 ? value : throw new ArgumentException( "value can't be less than -2", nameof( value ) );
        }

        public X509Certificate Certificate
        {
            get => this.certificate;
            set => this.certificate = value;
        }

        public DateTimeObject CertificateValid
        {
            get => this.certificateValid;
            set => this.certificateValid = value;
        }

        public ISet ExtendedKeyUsage
        {
            get => CopySet( this.extendedKeyUsage );
            set => this.extendedKeyUsage = CopySet( value );
        }

        public X509Name Issuer
        {
            get => this.issuer;
            set => this.issuer = value;
        }

        [Obsolete( "Avoid working with X509Name objects in string form" )]
        public string IssuerAsString => this.issuer == null ? null : this.issuer.ToString();

        public bool[] KeyUsage
        {
            get => CopyBoolArray( this.keyUsage );
            set => this.keyUsage = CopyBoolArray( value );
        }

        public ISet Policy
        {
            get => CopySet( this.policy );
            set => this.policy = CopySet( value );
        }

        public DateTimeObject PrivateKeyValid
        {
            get => this.privateKeyValid;
            set => this.privateKeyValid = value;
        }

        public BigInteger SerialNumber
        {
            get => this.serialNumber;
            set => this.serialNumber = value;
        }

        public X509Name Subject
        {
            get => this.subject;
            set => this.subject = value;
        }

        public string SubjectAsString => this.subject == null ? null : this.subject.ToString();

        public byte[] SubjectKeyIdentifier
        {
            get => Arrays.Clone( this.subjectKeyIdentifier );
            set => this.subjectKeyIdentifier = Arrays.Clone( value );
        }

        public SubjectPublicKeyInfo SubjectPublicKey
        {
            get => this.subjectPublicKey;
            set => this.subjectPublicKey = value;
        }

        public DerObjectIdentifier SubjectPublicKeyAlgID
        {
            get => this.subjectPublicKeyAlgID;
            set => this.subjectPublicKeyAlgID = value;
        }

        public virtual bool Match( object obj )
        {
            if (!(obj is X509Certificate c) || !MatchExtension( this.authorityKeyIdentifier, c, X509Extensions.AuthorityKeyIdentifier ))
                return false;
            if (this.basicConstraints != -1)
            {
                int basicConstraints = c.GetBasicConstraints();
                if (this.basicConstraints == -2)
                {
                    if (basicConstraints != -1)
                        return false;
                }
                else if (basicConstraints < this.basicConstraints)
                    return false;
            }
            if ((this.certificate != null && !this.certificate.Equals( c )) || (this.certificateValid != null && !c.IsValid( this.certificateValid.Value )))
                return false;
            if (this.extendedKeyUsage != null)
            {
                IList extendedKeyUsage = c.GetExtendedKeyUsage();
                if (extendedKeyUsage != null)
                {
                    foreach (DerObjectIdentifier objectIdentifier in (IEnumerable)this.extendedKeyUsage)
                    {
                        if (!extendedKeyUsage.Contains( objectIdentifier.Id ))
                            return false;
                    }
                }
            }
            if (this.issuer != null && !this.issuer.Equivalent( c.IssuerDN, true ))
                return false;
            if (this.keyUsage != null)
            {
                bool[] keyUsage = c.GetKeyUsage();
                if (keyUsage != null)
                {
                    for (int index = 0; index < 9; ++index)
                    {
                        if (this.keyUsage[index] && !keyUsage[index])
                            return false;
                    }
                }
            }
            if (this.policy != null)
            {
                Asn1OctetString extensionValue = c.GetExtensionValue( X509Extensions.CertificatePolicies );
                if (extensionValue == null)
                    return false;
                Asn1Sequence instance = Asn1Sequence.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
                if (this.policy.Count < 1 && instance.Count < 1)
                    return false;
                bool flag = false;
                foreach (PolicyInformation policyInformation in instance)
                {
                    if (this.policy.Contains( policyInformation.PolicyIdentifier ))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return false;
            }
            if (this.privateKeyValid != null)
            {
                Asn1OctetString extensionValue = c.GetExtensionValue( X509Extensions.PrivateKeyUsagePeriod );
                if (extensionValue == null)
                    return false;
                PrivateKeyUsagePeriod instance = PrivateKeyUsagePeriod.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
                DateTime dateTime1 = this.privateKeyValid.Value;
                DateTime dateTime2 = instance.NotAfter.ToDateTime();
                DateTime dateTime3 = instance.NotBefore.ToDateTime();
                if (dateTime1.CompareTo( (object)dateTime2 ) > 0 || dateTime1.CompareTo( (object)dateTime3 ) < 0)
                    return false;
            }
            return (this.serialNumber == null || this.serialNumber.Equals( c.SerialNumber )) && (this.subject == null || this.subject.Equivalent( c.SubjectDN, true )) && MatchExtension( this.subjectKeyIdentifier, c, X509Extensions.SubjectKeyIdentifier ) && (this.subjectPublicKey == null || this.subjectPublicKey.Equals( GetSubjectPublicKey( c ) )) && (this.subjectPublicKeyAlgID == null || this.subjectPublicKeyAlgID.Equals( GetSubjectPublicKey( c ).AlgorithmID ));
        }

        internal static bool IssuersMatch( X509Name a, X509Name b ) => a != null ? a.Equivalent( b, true ) : b == null;

        private static bool[] CopyBoolArray( bool[] b ) => b != null ? (bool[])b.Clone() : null;

        private static ISet CopySet( ISet s ) => s != null ? new HashSet( s ) : (ISet)null;

        private static SubjectPublicKeyInfo GetSubjectPublicKey( X509Certificate c ) => SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( c.GetPublicKey() );

        private static bool MatchExtension( byte[] b, X509Certificate c, DerObjectIdentifier oid )
        {
            if (b == null)
                return true;
            Asn1OctetString extensionValue = c.GetExtensionValue( oid );
            return extensionValue != null && Arrays.AreEqual( b, extensionValue.GetOctets() );
        }
    }
}
