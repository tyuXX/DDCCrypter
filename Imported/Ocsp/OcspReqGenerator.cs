// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.OcspReqGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Ocsp
{
    public class OcspReqGenerator
    {
        private IList list = Platform.CreateArrayList();
        private GeneralName requestorName = null;
        private X509Extensions requestExtensions = null;

        public void AddRequest( CertificateID certId ) => this.list.Add( new OcspReqGenerator.RequestObject( certId, null ) );

        public void AddRequest( CertificateID certId, X509Extensions singleRequestExtensions ) => this.list.Add( new OcspReqGenerator.RequestObject( certId, singleRequestExtensions ) );

        public void SetRequestorName( X509Name requestorName )
        {
            try
            {
                this.requestorName = new GeneralName( 4, requestorName );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "cannot encode principal", ex );
            }
        }

        public void SetRequestorName( GeneralName requestorName ) => this.requestorName = requestorName;

        public void SetRequestExtensions( X509Extensions requestExtensions ) => this.requestExtensions = requestExtensions;

        private OcspReq GenerateRequest(
          DerObjectIdentifier signingAlgorithm,
          AsymmetricKeyParameter privateKey,
          X509Certificate[] chain,
          SecureRandom random )
        {
            Asn1EncodableVector v1 = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (OcspReqGenerator.RequestObject requestObject in (IEnumerable)this.list)
            {
                try
                {
                    v1.Add( requestObject.ToRequest() );
                }
                catch (Exception ex)
                {
                    throw new OcspException( "exception creating Request", ex );
                }
            }
            TbsRequest tbsRequest = new TbsRequest( this.requestorName, new DerSequence( v1 ), this.requestExtensions );
            Org.BouncyCastle.Asn1.Ocsp.Signature optionalSignature = null;
            if (signingAlgorithm != null)
            {
                if (this.requestorName == null)
                    throw new OcspException( "requestorName must be specified if request is signed." );
                ISigner signer;
                try
                {
                    signer = SignerUtilities.GetSigner( signingAlgorithm.Id );
                    if (random != null)
                        signer.Init( true, new ParametersWithRandom( privateKey, random ) );
                    else
                        signer.Init( true, privateKey );
                }
                catch (Exception ex)
                {
                    throw new OcspException( "exception creating signature: " + ex, ex );
                }
                DerBitString signatureValue;
                try
                {
                    byte[] encoded = tbsRequest.GetEncoded();
                    signer.BlockUpdate( encoded, 0, encoded.Length );
                    signatureValue = new DerBitString( signer.GenerateSignature() );
                }
                catch (Exception ex)
                {
                    throw new OcspException( "exception processing TBSRequest: " + ex, ex );
                }
                AlgorithmIdentifier signatureAlgorithm = new AlgorithmIdentifier( signingAlgorithm, DerNull.Instance );
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
                    optionalSignature = new Org.BouncyCastle.Asn1.Ocsp.Signature( signatureAlgorithm, signatureValue, new DerSequence( v2 ) );
                }
                else
                    optionalSignature = new Org.BouncyCastle.Asn1.Ocsp.Signature( signatureAlgorithm, signatureValue );
            }
            return new OcspReq( new OcspRequest( tbsRequest, optionalSignature ) );
        }

        public OcspReq Generate() => this.GenerateRequest( null, null, null, null );

        public OcspReq Generate(
          string signingAlgorithm,
          AsymmetricKeyParameter privateKey,
          X509Certificate[] chain )
        {
            return this.Generate( signingAlgorithm, privateKey, chain, null );
        }

        public OcspReq Generate(
          string signingAlgorithm,
          AsymmetricKeyParameter privateKey,
          X509Certificate[] chain,
          SecureRandom random )
        {
            if (signingAlgorithm == null)
                throw new ArgumentException( "no signing algorithm specified" );
            try
            {
                return this.GenerateRequest( OcspUtilities.GetAlgorithmOid( signingAlgorithm ), privateKey, chain, random );
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException( "unknown signing algorithm specified: " + signingAlgorithm );
            }
        }

        public IEnumerable SignatureAlgNames => OcspUtilities.AlgNames;

        private class RequestObject
        {
            internal CertificateID certId;
            internal X509Extensions extensions;

            public RequestObject( CertificateID certId, X509Extensions extensions )
            {
                this.certId = certId;
                this.extensions = extensions;
            }

            public Request ToRequest() => new Request( this.certId.ToAsn1Object(), this.extensions );
        }
    }
}
