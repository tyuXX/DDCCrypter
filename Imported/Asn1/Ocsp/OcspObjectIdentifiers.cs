// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.OcspObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public abstract class OcspObjectIdentifiers
    {
        internal const string PkixOcspId = "1.3.6.1.5.5.7.48.1";
        public static readonly DerObjectIdentifier PkixOcsp = new DerObjectIdentifier( "1.3.6.1.5.5.7.48.1" );
        public static readonly DerObjectIdentifier PkixOcspBasic = new DerObjectIdentifier( "1.3.6.1.5.5.7.48.1.1" );
        public static readonly DerObjectIdentifier PkixOcspNonce = new DerObjectIdentifier( PkixOcsp.ToString() + ".2" );
        public static readonly DerObjectIdentifier PkixOcspCrl = new DerObjectIdentifier( PkixOcsp.ToString() + ".3" );
        public static readonly DerObjectIdentifier PkixOcspResponse = new DerObjectIdentifier( PkixOcsp.ToString() + ".4" );
        public static readonly DerObjectIdentifier PkixOcspNocheck = new DerObjectIdentifier( PkixOcsp.ToString() + ".5" );
        public static readonly DerObjectIdentifier PkixOcspArchiveCutoff = new DerObjectIdentifier( PkixOcsp.ToString() + ".6" );
        public static readonly DerObjectIdentifier PkixOcspServiceLocator = new DerObjectIdentifier( PkixOcsp.ToString() + ".7" );
    }
}
