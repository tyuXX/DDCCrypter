// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.RevokedStatus
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Ocsp
{
    public class RevokedStatus : CertificateStatus
    {
        internal readonly RevokedInfo info;

        public RevokedStatus( RevokedInfo info ) => this.info = info;

        public RevokedStatus( DateTime revocationDate, int reason ) => this.info = new RevokedInfo( new DerGeneralizedTime( revocationDate ), new CrlReason( reason ) );

        public DateTime RevocationTime => this.info.RevocationTime.ToDateTime();

        public bool HasRevocationReason => this.info.RevocationReason != null;

        public int RevocationReason
        {
            get
            {
                if (this.info.RevocationReason == null)
                    throw new InvalidOperationException( "attempt to get a reason where none is available" );
                return this.info.RevocationReason.Value.IntValue;
            }
        }
    }
}
