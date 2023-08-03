// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.CertificationRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class CertificationRequest : Asn1Encodable
    {
        protected CertificationRequestInfo reqInfo;
        protected AlgorithmIdentifier sigAlgId;
        protected DerBitString sigBits;

        public static CertificationRequest GetInstance( object obj )
        {
            if (obj is CertificationRequest)
                return (CertificationRequest)obj;
            return obj != null ? new CertificationRequest( (Asn1Sequence)obj ) : null;
        }

        protected CertificationRequest()
        {
        }

        public CertificationRequest(
          CertificationRequestInfo requestInfo,
          AlgorithmIdentifier algorithm,
          DerBitString signature )
        {
            this.reqInfo = requestInfo;
            this.sigAlgId = algorithm;
            this.sigBits = signature;
        }

        public CertificationRequest( Asn1Sequence seq )
        {
            this.reqInfo = seq.Count == 3 ? CertificationRequestInfo.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.sigAlgId = AlgorithmIdentifier.GetInstance( seq[1] );
            this.sigBits = DerBitString.GetInstance( seq[2] );
        }

        public CertificationRequestInfo GetCertificationRequestInfo() => this.reqInfo;

        public AlgorithmIdentifier SignatureAlgorithm => this.sigAlgId;

        public DerBitString Signature => this.sigBits;

        public byte[] GetSignatureOctets() => this.sigBits.GetOctets();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       reqInfo,
       sigAlgId,
       sigBits
        } );
    }
}
