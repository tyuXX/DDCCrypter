// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.BasicOcspResponse
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class BasicOcspResponse : Asn1Encodable
    {
        private readonly ResponseData tbsResponseData;
        private readonly AlgorithmIdentifier signatureAlgorithm;
        private readonly DerBitString signature;
        private readonly Asn1Sequence certs;

        public static BasicOcspResponse GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static BasicOcspResponse GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case BasicOcspResponse _:
                    return (BasicOcspResponse)obj;
                case Asn1Sequence _:
                    return new BasicOcspResponse( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public BasicOcspResponse(
          ResponseData tbsResponseData,
          AlgorithmIdentifier signatureAlgorithm,
          DerBitString signature,
          Asn1Sequence certs )
        {
            this.tbsResponseData = tbsResponseData;
            this.signatureAlgorithm = signatureAlgorithm;
            this.signature = signature;
            this.certs = certs;
        }

        private BasicOcspResponse( Asn1Sequence seq )
        {
            this.tbsResponseData = ResponseData.GetInstance( seq[0] );
            this.signatureAlgorithm = AlgorithmIdentifier.GetInstance( seq[1] );
            this.signature = (DerBitString)seq[2];
            if (seq.Count <= 3)
                return;
            this.certs = Asn1Sequence.GetInstance( (Asn1TaggedObject)seq[3], true );
        }

        [Obsolete( "Use TbsResponseData property instead" )]
        public ResponseData GetTbsResponseData() => this.tbsResponseData;

        public ResponseData TbsResponseData => this.tbsResponseData;

        [Obsolete( "Use SignatureAlgorithm property instead" )]
        public AlgorithmIdentifier GetSignatureAlgorithm() => this.signatureAlgorithm;

        public AlgorithmIdentifier SignatureAlgorithm => this.signatureAlgorithm;

        [Obsolete( "Use Signature property instead" )]
        public DerBitString GetSignature() => this.signature;

        public DerBitString Signature => this.signature;

        public byte[] GetSignatureOctets() => this.signature.GetOctets();

        [Obsolete( "Use Certs property instead" )]
        public Asn1Sequence GetCerts() => this.certs;

        public Asn1Sequence Certs => this.certs;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         tbsResponseData,
         signatureAlgorithm,
         signature
            } );
            if (this.certs != null)
                v.Add( new DerTaggedObject( true, 0, certs ) );
            return new DerSequence( v );
        }
    }
}
