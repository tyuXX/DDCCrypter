// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509AttrCertStoreSelector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.X509.Extension;
using System.Collections;

namespace Org.BouncyCastle.X509.Store
{
    public class X509AttrCertStoreSelector : IX509Selector, ICloneable
    {
        private IX509AttributeCertificate attributeCert;
        private DateTimeObject attributeCertificateValid;
        private AttributeCertificateHolder holder;
        private AttributeCertificateIssuer issuer;
        private BigInteger serialNumber;
        private ISet targetNames = new HashSet();
        private ISet targetGroups = new HashSet();

        public X509AttrCertStoreSelector()
        {
        }

        private X509AttrCertStoreSelector( X509AttrCertStoreSelector o )
        {
            this.attributeCert = o.attributeCert;
            this.attributeCertificateValid = o.attributeCertificateValid;
            this.holder = o.holder;
            this.issuer = o.issuer;
            this.serialNumber = o.serialNumber;
            this.targetGroups = new HashSet( o.targetGroups );
            this.targetNames = new HashSet( o.targetNames );
        }

        public bool Match( object obj )
        {
            if (obj == null)
                throw new ArgumentNullException( nameof( obj ) );
            if (!(obj is IX509AttributeCertificate attributeCertificate) || (this.attributeCert != null && !this.attributeCert.Equals( attributeCertificate )) || (this.serialNumber != null && !attributeCertificate.SerialNumber.Equals( serialNumber )) || (this.holder != null && !attributeCertificate.Holder.Equals( holder )) || (this.issuer != null && !attributeCertificate.Issuer.Equals( issuer )) || (this.attributeCertificateValid != null && !attributeCertificate.IsValid( this.attributeCertificateValid.Value )))
                return false;
            if (this.targetNames.Count > 0 || this.targetGroups.Count > 0)
            {
                Asn1OctetString extensionValue = attributeCertificate.GetExtensionValue( X509Extensions.TargetInformation );
                if (extensionValue != null)
                {
                    TargetInformation instance;
                    try
                    {
                        instance = TargetInformation.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    Targets[] targetsObjects = instance.GetTargetsObjects();
                    if (this.targetNames.Count > 0)
                    {
                        bool flag = false;
                        for (int index = 0; index < targetsObjects.Length && !flag; ++index)
                        {
                            foreach (Target target in targetsObjects[index].GetTargets())
                            {
                                GeneralName targetName = target.TargetName;
                                if (targetName != null && this.targetNames.Contains( targetName ))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                            return false;
                    }
                    if (this.targetGroups.Count > 0)
                    {
                        bool flag = false;
                        for (int index = 0; index < targetsObjects.Length && !flag; ++index)
                        {
                            foreach (Target target in targetsObjects[index].GetTargets())
                            {
                                GeneralName targetGroup = target.TargetGroup;
                                if (targetGroup != null && this.targetGroups.Contains( targetGroup ))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                            return false;
                    }
                }
            }
            return true;
        }

        public object Clone() => new X509AttrCertStoreSelector( this );

        public IX509AttributeCertificate AttributeCert
        {
            get => this.attributeCert;
            set => this.attributeCert = value;
        }

        [Obsolete( "Use AttributeCertificateValid instead" )]
        public DateTimeObject AttribueCertificateValid
        {
            get => this.attributeCertificateValid;
            set => this.attributeCertificateValid = value;
        }

        public DateTimeObject AttributeCertificateValid
        {
            get => this.attributeCertificateValid;
            set => this.attributeCertificateValid = value;
        }

        public AttributeCertificateHolder Holder
        {
            get => this.holder;
            set => this.holder = value;
        }

        public AttributeCertificateIssuer Issuer
        {
            get => this.issuer;
            set => this.issuer = value;
        }

        public BigInteger SerialNumber
        {
            get => this.serialNumber;
            set => this.serialNumber = value;
        }

        public void AddTargetName( GeneralName name ) => this.targetNames.Add( name );

        public void AddTargetName( byte[] name ) => this.AddTargetName( GeneralName.GetInstance( Asn1Object.FromByteArray( name ) ) );

        public void SetTargetNames( IEnumerable names ) => this.targetNames = this.ExtractGeneralNames( names );

        public IEnumerable GetTargetNames() => new EnumerableProxy( targetNames );

        public void AddTargetGroup( GeneralName group ) => this.targetGroups.Add( group );

        public void AddTargetGroup( byte[] name ) => this.AddTargetGroup( GeneralName.GetInstance( Asn1Object.FromByteArray( name ) ) );

        public void SetTargetGroups( IEnumerable names ) => this.targetGroups = this.ExtractGeneralNames( names );

        public IEnumerable GetTargetGroups() => new EnumerableProxy( targetGroups );

        private ISet ExtractGeneralNames( IEnumerable names )
        {
            ISet generalNames = new HashSet();
            if (names != null)
            {
                foreach (object name in names)
                {
                    if (name is GeneralName)
                        generalNames.Add( name );
                    else
                        generalNames.Add( GeneralName.GetInstance( Asn1Object.FromByteArray( (byte[])name ) ) );
                }
            }
            return generalNames;
        }
    }
}
