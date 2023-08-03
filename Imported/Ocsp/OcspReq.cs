// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.OcspReq
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Ocsp
{
    public class OcspReq : X509ExtensionBase
    {
        private OcspRequest req;

        public OcspReq( OcspRequest req ) => this.req = req;

        public OcspReq( byte[] req )
          : this( new Asn1InputStream( req ) )
        {
        }

        public OcspReq( Stream inStr )
          : this( new Asn1InputStream( inStr ) )
        {
        }

        private OcspReq( Asn1InputStream aIn )
        {
            try
            {
                this.req = OcspRequest.GetInstance( aIn.ReadObject() );
            }
            catch (ArgumentException ex)
            {
                throw new IOException( "malformed request: " + ex.Message );
            }
            catch (InvalidCastException ex)
            {
                throw new IOException( "malformed request: " + ex.Message );
            }
        }

        public byte[] GetTbsRequest()
        {
            try
            {
                return this.req.TbsRequest.GetEncoded();
            }
            catch (IOException ex)
            {
                throw new OcspException( "problem encoding tbsRequest", ex );
            }
        }

        public int Version => this.req.TbsRequest.Version.Value.IntValue + 1;

        public GeneralName RequestorName => GeneralName.GetInstance( req.TbsRequest.RequestorName );

        public Req[] GetRequestList()
        {
            Asn1Sequence requestList1 = this.req.TbsRequest.RequestList;
            Req[] requestList2 = new Req[requestList1.Count];
            for (int index = 0; index != requestList2.Length; ++index)
                requestList2[index] = new Req( Request.GetInstance( requestList1[index] ) );
            return requestList2;
        }

        public X509Extensions RequestExtensions => X509Extensions.GetInstance( req.TbsRequest.RequestExtensions );

        protected override X509Extensions GetX509Extensions() => this.RequestExtensions;

        public string SignatureAlgOid => !this.IsSigned ? null : this.req.OptionalSignature.SignatureAlgorithm.Algorithm.Id;

        public byte[] GetSignature() => !this.IsSigned ? null : this.req.OptionalSignature.GetSignatureOctets();

        private IList GetCertList()
        {
            IList arrayList = Platform.CreateArrayList();
            Asn1Sequence certs = this.req.OptionalSignature.Certs;
            if (certs != null)
            {
                foreach (Asn1Encodable asn1Encodable in certs)
                {
                    try
                    {
                        arrayList.Add( new X509CertificateParser().ReadCertificate( asn1Encodable.GetEncoded() ) );
                    }
                    catch (Exception ex)
                    {
                        throw new OcspException( "can't re-encode certificate!", ex );
                    }
                }
            }
            return arrayList;
        }

        public X509Certificate[] GetCerts()
        {
            if (!this.IsSigned)
                return null;
            IList certList = this.GetCertList();
            X509Certificate[] certs = new X509Certificate[certList.Count];
            for (int index = 0; index < certList.Count; ++index)
                certs[index] = (X509Certificate)certList[index];
            return certs;
        }

        public IX509Store GetCertificates( string type )
        {
            if (!this.IsSigned)
                return null;
            try
            {
                return X509StoreFactory.Create( "Certificate/" + type, new X509CollectionStoreParameters( this.GetCertList() ) );
            }
            catch (Exception ex)
            {
                throw new OcspException( "can't setup the CertStore", ex );
            }
        }

        public bool IsSigned => this.req.OptionalSignature != null;

        public bool Verify( AsymmetricKeyParameter publicKey )
        {
            if (!this.IsSigned)
                throw new OcspException( "attempt to Verify signature on unsigned object" );
            try
            {
                ISigner signer = SignerUtilities.GetSigner( this.SignatureAlgOid );
                signer.Init( false, publicKey );
                byte[] encoded = this.req.TbsRequest.GetEncoded();
                signer.BlockUpdate( encoded, 0, encoded.Length );
                return signer.VerifySignature( this.GetSignature() );
            }
            catch (Exception ex)
            {
                throw new OcspException( "exception processing sig: " + ex, ex );
            }
        }

        public byte[] GetEncoded() => this.req.GetEncoded();
    }
}
