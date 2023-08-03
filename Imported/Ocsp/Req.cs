// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.Req
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;

namespace Org.BouncyCastle.Ocsp
{
    public class Req : X509ExtensionBase
    {
        private Request req;

        public Req( Request req ) => this.req = req;

        public CertificateID GetCertID() => new CertificateID( this.req.ReqCert );

        public X509Extensions SingleRequestExtensions => this.req.SingleRequestExtensions;

        protected override X509Extensions GetX509Extensions() => this.SingleRequestExtensions;
    }
}
