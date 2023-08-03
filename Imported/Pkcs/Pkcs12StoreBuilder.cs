// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.Pkcs12StoreBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Org.BouncyCastle.Pkcs
{
    public class Pkcs12StoreBuilder
    {
        private DerObjectIdentifier keyAlgorithm = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc;
        private DerObjectIdentifier certAlgorithm = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc;
        private bool useDerEncoding = false;

        public Pkcs12Store Build() => new Pkcs12Store( this.keyAlgorithm, this.certAlgorithm, this.useDerEncoding );

        public Pkcs12StoreBuilder SetCertAlgorithm( DerObjectIdentifier certAlgorithm )
        {
            this.certAlgorithm = certAlgorithm;
            return this;
        }

        public Pkcs12StoreBuilder SetKeyAlgorithm( DerObjectIdentifier keyAlgorithm )
        {
            this.keyAlgorithm = keyAlgorithm;
            return this;
        }

        public Pkcs12StoreBuilder SetUseDerEncoding( bool useDerEncoding )
        {
            this.useDerEncoding = useDerEncoding;
            return this;
        }
    }
}
