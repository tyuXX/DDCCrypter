// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CrmfObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Crmf
{
    public abstract class CrmfObjectIdentifiers
    {
        public static readonly DerObjectIdentifier id_pkix = new( "1.3.6.1.5.5.7" );
        public static readonly DerObjectIdentifier id_pkip = id_pkix.Branch( "5" );
        public static readonly DerObjectIdentifier id_regCtrl = id_pkip.Branch( "1" );
        public static readonly DerObjectIdentifier id_regCtrl_regToken = id_regCtrl.Branch( "1" );
        public static readonly DerObjectIdentifier id_regCtrl_authenticator = id_regCtrl.Branch( "2" );
        public static readonly DerObjectIdentifier id_regCtrl_pkiPublicationInfo = id_regCtrl.Branch( "3" );
        public static readonly DerObjectIdentifier id_regCtrl_pkiArchiveOptions = id_regCtrl.Branch( "4" );
        public static readonly DerObjectIdentifier id_ct_encKeyWithID = new( "1.2.840.113549.1.9.16.1.21" );
    }
}
