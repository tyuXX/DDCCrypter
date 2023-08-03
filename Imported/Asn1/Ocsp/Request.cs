// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.Request
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class Request : Asn1Encodable
    {
        private readonly CertID reqCert;
        private readonly X509Extensions singleRequestExtensions;

        public static Request GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static Request GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Request _:
                    return (Request)obj;
                case Asn1Sequence _:
                    return new Request( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Request( CertID reqCert, X509Extensions singleRequestExtensions )
        {
            this.reqCert = reqCert != null ? reqCert : throw new ArgumentNullException( nameof( reqCert ) );
            this.singleRequestExtensions = singleRequestExtensions;
        }

        private Request( Asn1Sequence seq )
        {
            this.reqCert = CertID.GetInstance( seq[0] );
            if (seq.Count != 2)
                return;
            this.singleRequestExtensions = X509Extensions.GetInstance( (Asn1TaggedObject)seq[1], true );
        }

        public CertID ReqCert => this.reqCert;

        public X509Extensions SingleRequestExtensions => this.singleRequestExtensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         reqCert
            } );
            if (this.singleRequestExtensions != null)
                v.Add( new DerTaggedObject( true, 0, singleRequestExtensions ) );
            return new DerSequence( v );
        }
    }
}
