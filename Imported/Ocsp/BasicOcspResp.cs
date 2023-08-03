// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.BasicOcspResp
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Ocsp
{
    public class BasicOcspResp : X509ExtensionBase
    {
        private readonly BasicOcspResponse resp;
        private readonly ResponseData data;

        public BasicOcspResp( BasicOcspResponse resp )
        {
            this.resp = resp;
            this.data = resp.TbsResponseData;
        }

        public byte[] GetTbsResponseData()
        {
            try
            {
                return this.data.GetDerEncoded();
            }
            catch (IOException ex)
            {
                throw new OcspException( "problem encoding tbsResponseData", ex );
            }
        }

        public int Version => this.data.Version.Value.IntValue + 1;

        public RespID ResponderId => new RespID( this.data.ResponderID );

        public DateTime ProducedAt => this.data.ProducedAt.ToDateTime();

        public SingleResp[] Responses
        {
            get
            {
                Asn1Sequence responses1 = this.data.Responses;
                SingleResp[] responses2 = new SingleResp[responses1.Count];
                for (int index = 0; index != responses2.Length; ++index)
                    responses2[index] = new SingleResp( SingleResponse.GetInstance( responses1[index] ) );
                return responses2;
            }
        }

        public X509Extensions ResponseExtensions => this.data.ResponseExtensions;

        protected override X509Extensions GetX509Extensions() => this.ResponseExtensions;

        public string SignatureAlgName => OcspUtilities.GetAlgorithmName( this.resp.SignatureAlgorithm.Algorithm );

        public string SignatureAlgOid => this.resp.SignatureAlgorithm.Algorithm.Id;

        [Obsolete( "RespData class is no longer required as all functionality is available on this class" )]
        public RespData GetResponseData() => new RespData( this.data );

        public byte[] GetSignature() => this.resp.GetSignatureOctets();

        private IList GetCertList()
        {
            IList arrayList = Platform.CreateArrayList();
            Asn1Sequence certs = this.resp.Certs;
            if (certs != null)
            {
                foreach (Asn1Encodable asn1Encodable in certs)
                {
                    try
                    {
                        arrayList.Add( new X509CertificateParser().ReadCertificate( asn1Encodable.GetEncoded() ) );
                    }
                    catch (IOException ex)
                    {
                        throw new OcspException( "can't re-encode certificate!", ex );
                    }
                    catch (CertificateException ex)
                    {
                        throw new OcspException( "can't re-encode certificate!", ex );
                    }
                }
            }
            return arrayList;
        }

        public X509Certificate[] GetCerts()
        {
            IList certList = this.GetCertList();
            X509Certificate[] certs = new X509Certificate[certList.Count];
            for (int index = 0; index < certList.Count; ++index)
                certs[index] = (X509Certificate)certList[index];
            return certs;
        }

        public IX509Store GetCertificates( string type )
        {
            try
            {
                return X509StoreFactory.Create( "Certificate/" + type, new X509CollectionStoreParameters( this.GetCertList() ) );
            }
            catch (Exception ex)
            {
                throw new OcspException( "can't setup the CertStore", ex );
            }
        }

        public bool Verify( AsymmetricKeyParameter publicKey )
        {
            try
            {
                ISigner signer = SignerUtilities.GetSigner( this.SignatureAlgName );
                signer.Init( false, publicKey );
                byte[] derEncoded = this.data.GetDerEncoded();
                signer.BlockUpdate( derEncoded, 0, derEncoded.Length );
                return signer.VerifySignature( this.GetSignature() );
            }
            catch (Exception ex)
            {
                throw new OcspException( "exception processing sig: " + ex, ex );
            }
        }

        public byte[] GetEncoded() => this.resp.GetEncoded();

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is BasicOcspResp basicOcspResp && this.resp.Equals( basicOcspResp.resp );
        }

        public override int GetHashCode() => this.resp.GetHashCode();
    }
}
