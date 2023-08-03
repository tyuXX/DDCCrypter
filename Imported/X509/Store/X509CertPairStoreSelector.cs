// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509CertPairStoreSelector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.X509.Store
{
    public class X509CertPairStoreSelector : IX509Selector, ICloneable
    {
        private X509CertificatePair certPair;
        private X509CertStoreSelector forwardSelector;
        private X509CertStoreSelector reverseSelector;

        private static X509CertStoreSelector CloneSelector( X509CertStoreSelector s ) => s != null ? (X509CertStoreSelector)s.Clone() : null;

        public X509CertPairStoreSelector()
        {
        }

        private X509CertPairStoreSelector( X509CertPairStoreSelector o )
        {
            this.certPair = o.CertPair;
            this.forwardSelector = o.ForwardSelector;
            this.reverseSelector = o.ReverseSelector;
        }

        public X509CertificatePair CertPair
        {
            get => this.certPair;
            set => this.certPair = value;
        }

        public X509CertStoreSelector ForwardSelector
        {
            get => CloneSelector( this.forwardSelector );
            set => this.forwardSelector = CloneSelector( value );
        }

        public X509CertStoreSelector ReverseSelector
        {
            get => CloneSelector( this.reverseSelector );
            set => this.reverseSelector = CloneSelector( value );
        }

        public bool Match( object obj )
        {
            if (obj == null)
                throw new ArgumentNullException( nameof( obj ) );
            return obj is X509CertificatePair x509CertificatePair && (this.certPair == null || this.certPair.Equals( x509CertificatePair )) && (this.forwardSelector == null || this.forwardSelector.Match( x509CertificatePair.Forward )) && (this.reverseSelector == null || this.reverseSelector.Match( x509CertificatePair.Reverse ));
        }

        public object Clone() => new X509CertPairStoreSelector( this );
    }
}
