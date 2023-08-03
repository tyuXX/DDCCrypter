// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509ExtensionBase
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.X509
{
    public abstract class X509ExtensionBase : IX509Extension
    {
        protected abstract X509Extensions GetX509Extensions();

        protected virtual ISet GetExtensionOids( bool critical )
        {
            X509Extensions x509Extensions = this.GetX509Extensions();
            if (x509Extensions == null)
                return null;
            HashSet extensionOids = new();
            foreach (DerObjectIdentifier extensionOid in x509Extensions.ExtensionOids)
            {
                if (x509Extensions.GetExtension( extensionOid ).IsCritical == critical)
                    extensionOids.Add( extensionOid.Id );
            }
            return extensionOids;
        }

        public virtual ISet GetNonCriticalExtensionOids() => this.GetExtensionOids( false );

        public virtual ISet GetCriticalExtensionOids() => this.GetExtensionOids( true );

        [Obsolete( "Use version taking a DerObjectIdentifier instead" )]
        public Asn1OctetString GetExtensionValue( string oid ) => this.GetExtensionValue( new DerObjectIdentifier( oid ) );

        public virtual Asn1OctetString GetExtensionValue( DerObjectIdentifier oid )
        {
            X509Extensions x509Extensions = this.GetX509Extensions();
            if (x509Extensions != null)
            {
                X509Extension extension = x509Extensions.GetExtension( oid );
                if (extension != null)
                    return extension.Value;
            }
            return null;
        }
    }
}
