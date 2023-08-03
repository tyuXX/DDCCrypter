// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.OriginatorInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class OriginatorInfoGenerator
    {
        private readonly IList origCerts;
        private readonly IList origCrls;

        public OriginatorInfoGenerator( X509Certificate origCert )
        {
            this.origCerts = Platform.CreateArrayList( 1 );
            this.origCrls = null;
            this.origCerts.Add( origCert.CertificateStructure );
        }

        public OriginatorInfoGenerator( IX509Store origCerts )
          : this( origCerts, null )
        {
        }

        public OriginatorInfoGenerator( IX509Store origCerts, IX509Store origCrls )
        {
            this.origCerts = CmsUtilities.GetCertificatesFromStore( origCerts );
            this.origCrls = origCrls == null ? null : CmsUtilities.GetCrlsFromStore( origCrls );
        }

        public virtual OriginatorInfo Generate() => new OriginatorInfo( CmsUtilities.CreateDerSetFromList( this.origCerts ), this.origCrls == null ? null : CmsUtilities.CreateDerSetFromList( this.origCrls ) );
    }
}
