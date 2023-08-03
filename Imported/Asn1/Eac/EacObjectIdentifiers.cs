// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Eac.EacObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Eac
{
    public abstract class EacObjectIdentifiers
    {
        public static readonly DerObjectIdentifier bsi_de = new( "0.4.0.127.0.7" );
        public static readonly DerObjectIdentifier id_PK = new( bsi_de.ToString() + ".2.2.1" );
        public static readonly DerObjectIdentifier id_PK_DH = new( id_PK.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_PK_ECDH = new( id_PK.ToString() + ".2" );
        public static readonly DerObjectIdentifier id_CA = new( bsi_de.ToString() + ".2.2.3" );
        public static readonly DerObjectIdentifier id_CA_DH = new( id_CA.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_CA_DH_3DES_CBC_CBC = new( id_CA_DH.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_CA_ECDH = new( id_CA.ToString() + ".2" );
        public static readonly DerObjectIdentifier id_CA_ECDH_3DES_CBC_CBC = new( id_CA_ECDH.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_TA = new( bsi_de.ToString() + ".2.2.2" );
        public static readonly DerObjectIdentifier id_TA_RSA = new( id_TA.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_TA_RSA_v1_5_SHA_1 = new( id_TA_RSA.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_TA_RSA_v1_5_SHA_256 = new( id_TA_RSA.ToString() + ".2" );
        public static readonly DerObjectIdentifier id_TA_RSA_PSS_SHA_1 = new( id_TA_RSA.ToString() + ".3" );
        public static readonly DerObjectIdentifier id_TA_RSA_PSS_SHA_256 = new( id_TA_RSA.ToString() + ".4" );
        public static readonly DerObjectIdentifier id_TA_ECDSA = new( id_TA.ToString() + ".2" );
        public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_1 = new( id_TA_ECDSA.ToString() + ".1" );
        public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_224 = new( id_TA_ECDSA.ToString() + ".2" );
        public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_256 = new( id_TA_ECDSA.ToString() + ".3" );
        public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_384 = new( id_TA_ECDSA.ToString() + ".4" );
        public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_512 = new( id_TA_ECDSA.ToString() + ".5" );
    }
}
