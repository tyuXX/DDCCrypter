// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.SingleResp
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.X509;

namespace Org.BouncyCastle.Ocsp
{
    public class SingleResp : X509ExtensionBase
    {
        internal readonly SingleResponse resp;

        public SingleResp( SingleResponse resp ) => this.resp = resp;

        public CertificateID GetCertID() => new( this.resp.CertId );

        public object GetCertStatus()
        {
            CertStatus certStatus = this.resp.CertStatus;
            if (certStatus.TagNo == 0)
                return null;
            return certStatus.TagNo == 1 ? new RevokedStatus( RevokedInfo.GetInstance( certStatus.Status ) ) : (object)new UnknownStatus();
        }

        public DateTime ThisUpdate => this.resp.ThisUpdate.ToDateTime();

        public DateTimeObject NextUpdate => this.resp.NextUpdate != null ? new DateTimeObject( this.resp.NextUpdate.ToDateTime() ) : null;

        public X509Extensions SingleExtensions => this.resp.SingleExtensions;

        protected override X509Extensions GetX509Extensions() => this.SingleExtensions;
    }
}
