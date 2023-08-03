// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.BasicOcspRespGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Ocsp
{
    public class BasicOcspRespGenerator
    {
        private readonly IList list = Platform.CreateArrayList();
        private X509Extensions responseExtensions;
        private RespID responderID;

        public BasicOcspRespGenerator( RespID responderID ) => this.responderID = responderID;

        public BasicOcspRespGenerator( AsymmetricKeyParameter publicKey ) => this.responderID = new RespID( publicKey );

        public void AddResponse( CertificateID certID, CertificateStatus certStatus ) => this.list.Add( new BasicOcspRespGenerator.ResponseObject( certID, certStatus, DateTime.UtcNow, null ) );

        public void AddResponse(
          CertificateID certID,
          CertificateStatus certStatus,
          X509Extensions singleExtensions )
        {
            this.list.Add( new BasicOcspRespGenerator.ResponseObject( certID, certStatus, DateTime.UtcNow, singleExtensions ) );
        }

        public void AddResponse(
          CertificateID certID,
          CertificateStatus certStatus,
          DateTime nextUpdate,
          X509Extensions singleExtensions )
        {
            this.list.Add( new BasicOcspRespGenerator.ResponseObject( certID, certStatus, DateTime.UtcNow, nextUpdate, singleExtensions ) );
        }

        public void AddResponse(
          CertificateID certID,
          CertificateStatus certStatus,
          DateTime thisUpdate,
          DateTime nextUpdate,
          X509Extensions singleExtensions )
        {
            this.list.Add( new BasicOcspRespGenerator.ResponseObject( certID, certStatus, thisUpdate, nextUpdate, singleExtensions ) );
        }

        public void SetResponseExtensions( X509Extensions responseExtensions ) => this.responseExtensions = responseExtensions;

        private BasicOcspResp GenerateResponse(
          ISignatureFactory signatureCalculator,
          X509Certificate[] chain,
          DateTime producedAt )
        {
            DerObjectIdentifier algorithm = ((AlgorithmIdentifier)signatureCalculator.AlgorithmDetails).Algorithm;
            Asn1EncodableVector v1 = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (BasicOcspRespGenerator.ResponseObject responseObject in (IEnumerable)this.list)
            {
                try
                {
                    v1.Add( responseObject.ToResponse() );
                }
                catch (Exception ex)
                {
                    throw new OcspException( "exception creating Request", ex );
                }
            }
            ResponseData tbsResponseData = new ResponseData( this.responderID.ToAsn1Object(), new DerGeneralizedTime( producedAt ), new DerSequence( v1 ), this.responseExtensions );
            DerBitString signature;
            try
            {
                IStreamCalculator calculator = signatureCalculator.CreateCalculator();
                byte[] derEncoded = tbsResponseData.GetDerEncoded();
                calculator.Stream.Write( derEncoded, 0, derEncoded.Length );
                Platform.Dispose( calculator.Stream );
                signature = new DerBitString( ((IBlockResult)calculator.GetResult()).Collect() );
            }
            catch (Exception ex)
            {
                throw new OcspException( "exception processing TBSRequest: " + ex, ex );
            }
            AlgorithmIdentifier sigAlgId = OcspUtilities.GetSigAlgID( algorithm );
            DerSequence certs = null;
            if (chain != null && chain.Length > 0)
            {
                Asn1EncodableVector v2 = new Asn1EncodableVector( new Asn1Encodable[0] );
                try
                {
                    for (int index = 0; index != chain.Length; ++index)
                        v2.Add( X509CertificateStructure.GetInstance( Asn1Object.FromByteArray( chain[index].GetEncoded() ) ) );
                }
                catch (IOException ex)
                {
                    throw new OcspException( "error processing certs", ex );
                }
                catch (CertificateEncodingException ex)
                {
                    throw new OcspException( "error encoding certs", ex );
                }
                certs = new DerSequence( v2 );
            }
            return new BasicOcspResp( new BasicOcspResponse( tbsResponseData, sigAlgId, signature, certs ) );
        }

        public BasicOcspResp Generate(
          string signingAlgorithm,
          AsymmetricKeyParameter privateKey,
          X509Certificate[] chain,
          DateTime thisUpdate )
        {
            return this.Generate( signingAlgorithm, privateKey, chain, thisUpdate, null );
        }

        public BasicOcspResp Generate(
          string signingAlgorithm,
          AsymmetricKeyParameter privateKey,
          X509Certificate[] chain,
          DateTime producedAt,
          SecureRandom random )
        {
            if (signingAlgorithm == null)
                throw new ArgumentException( "no signing algorithm specified" );
            return this.GenerateResponse( new Asn1SignatureFactory( signingAlgorithm, privateKey, random ), chain, producedAt );
        }

        public BasicOcspResp Generate(
          ISignatureFactory signatureCalculatorFactory,
          X509Certificate[] chain,
          DateTime producedAt )
        {
            if (signatureCalculatorFactory == null)
                throw new ArgumentException( "no signature calculator specified" );
            return this.GenerateResponse( signatureCalculatorFactory, chain, producedAt );
        }

        public IEnumerable SignatureAlgNames => OcspUtilities.AlgNames;

        private class ResponseObject
        {
            internal CertificateID certId;
            internal CertStatus certStatus;
            internal DerGeneralizedTime thisUpdate;
            internal DerGeneralizedTime nextUpdate;
            internal X509Extensions extensions;

            public ResponseObject(
              CertificateID certId,
              CertificateStatus certStatus,
              DateTime thisUpdate,
              X509Extensions extensions )
              : this( certId, certStatus, new DerGeneralizedTime( thisUpdate ), null, extensions )
            {
            }

            public ResponseObject(
              CertificateID certId,
              CertificateStatus certStatus,
              DateTime thisUpdate,
              DateTime nextUpdate,
              X509Extensions extensions )
              : this( certId, certStatus, new DerGeneralizedTime( thisUpdate ), new DerGeneralizedTime( nextUpdate ), extensions )
            {
            }

            private ResponseObject(
              CertificateID certId,
              CertificateStatus certStatus,
              DerGeneralizedTime thisUpdate,
              DerGeneralizedTime nextUpdate,
              X509Extensions extensions )
            {
                this.certId = certId;
                if (certStatus == null)
                    this.certStatus = new CertStatus();
                else if (certStatus is UnknownStatus)
                {
                    this.certStatus = new CertStatus( 2, DerNull.Instance );
                }
                else
                {
                    RevokedStatus revokedStatus = (RevokedStatus)certStatus;
                    CrlReason revocationReason = revokedStatus.HasRevocationReason ? new CrlReason( revokedStatus.RevocationReason ) : null;
                    this.certStatus = new CertStatus( new RevokedInfo( new DerGeneralizedTime( revokedStatus.RevocationTime ), revocationReason ) );
                }
                this.thisUpdate = thisUpdate;
                this.nextUpdate = nextUpdate;
                this.extensions = extensions;
            }

            public SingleResponse ToResponse() => new SingleResponse( this.certId.ToAsn1Object(), this.certStatus, this.thisUpdate, this.nextUpdate, this.extensions );
        }
    }
}
