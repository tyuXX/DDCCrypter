// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.RespData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;

namespace Org.BouncyCastle.Ocsp
{
    public class RespData : X509ExtensionBase
    {
        internal readonly ResponseData data;

        public RespData( ResponseData data ) => this.data = data;

        public int Version => this.data.Version.Value.IntValue + 1;

        public RespID GetResponderId() => new( this.data.ResponderID );

        public DateTime ProducedAt => this.data.ProducedAt.ToDateTime();

        public SingleResp[] GetResponses()
        {
            Asn1Sequence responses1 = this.data.Responses;
            SingleResp[] responses2 = new SingleResp[responses1.Count];
            for (int index = 0; index != responses2.Length; ++index)
                responses2[index] = new SingleResp( SingleResponse.GetInstance( responses1[index] ) );
            return responses2;
        }

        public X509Extensions ResponseExtensions => this.data.ResponseExtensions;

        protected override X509Extensions GetX509Extensions() => this.ResponseExtensions;
    }
}
