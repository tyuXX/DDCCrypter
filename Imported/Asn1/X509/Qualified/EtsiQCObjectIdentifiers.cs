// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.EtsiQCObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public abstract class EtsiQCObjectIdentifiers
    {
        public static readonly DerObjectIdentifier IdEtsiQcs = new( "0.4.0.1862.1" );
        public static readonly DerObjectIdentifier IdEtsiQcsQcCompliance = new( IdEtsiQcs.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdEtsiQcsLimitValue = new( IdEtsiQcs.ToString() + ".2" );
        public static readonly DerObjectIdentifier IdEtsiQcsRetentionPeriod = new( IdEtsiQcs.ToString() + ".3" );
        public static readonly DerObjectIdentifier IdEtsiQcsQcSscd = new( IdEtsiQcs.ToString() + ".4" );
    }
}
