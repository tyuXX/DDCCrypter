// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixBuilderParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.Pkix
{
    public class PkixBuilderParameters : PkixParameters
    {
        private int maxPathLength = 5;
        private ISet excludedCerts = new HashSet();

        public static PkixBuilderParameters GetInstance( PkixParameters pkixParams )
        {
            PkixBuilderParameters instance = new PkixBuilderParameters( pkixParams.GetTrustAnchors(), new X509CertStoreSelector( pkixParams.GetTargetCertConstraints() ) );
            instance.SetParams( pkixParams );
            return instance;
        }

        public PkixBuilderParameters( ISet trustAnchors, IX509Selector targetConstraints )
          : base( trustAnchors )
        {
            this.SetTargetCertConstraints( targetConstraints );
        }

        public virtual int MaxPathLength
        {
            get => this.maxPathLength;
            set => this.maxPathLength = value >= -1 ? value : throw new InvalidParameterException( "The maximum path length parameter can not be less than -1." );
        }

        public virtual ISet GetExcludedCerts() => new HashSet( excludedCerts );

        public virtual void SetExcludedCerts( ISet excludedCerts )
        {
            if (excludedCerts == null)
                excludedCerts = new HashSet();
            else
                this.excludedCerts = new HashSet( excludedCerts );
        }

        protected override void SetParams( PkixParameters parameters )
        {
            base.SetParams( parameters );
            if (!(parameters is PkixBuilderParameters))
                return;
            PkixBuilderParameters builderParameters = (PkixBuilderParameters)parameters;
            this.maxPathLength = builderParameters.maxPathLength;
            this.excludedCerts = new HashSet( builderParameters.excludedCerts );
        }

        public override object Clone()
        {
            PkixBuilderParameters builderParameters = new PkixBuilderParameters( this.GetTrustAnchors(), this.GetTargetCertConstraints() );
            builderParameters.SetParams( this );
            return builderParameters;
        }

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( "PkixBuilderParameters [" + newLine );
            stringBuilder.Append( base.ToString() );
            stringBuilder.Append( "  Maximum Path Length: " );
            stringBuilder.Append( this.MaxPathLength );
            stringBuilder.Append( newLine + "]" + newLine );
            return stringBuilder.ToString();
        }
    }
}
