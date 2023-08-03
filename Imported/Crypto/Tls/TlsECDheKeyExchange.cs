// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsECDheKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsECDheKeyExchange : TlsECDHKeyExchange
    {
        protected TlsSignerCredentials mServerCredentials = null;

        public TlsECDheKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          int[] namedCurves,
          byte[] clientECPointFormats,
          byte[] serverECPointFormats )
          : base( keyExchange, supportedSignatureAlgorithms, namedCurves, clientECPointFormats, serverECPointFormats )
        {
        }

        public override void ProcessServerCredentials( TlsCredentials serverCredentials )
        {
            if (!(serverCredentials is TlsSignerCredentials))
                throw new TlsFatalAlert( 80 );
            this.ProcessServerCertificate( serverCredentials.Certificate );
            this.mServerCredentials = (TlsSignerCredentials)serverCredentials;
        }

        public override byte[] GenerateServerKeyExchange()
        {
            DigestInputBuffer output = new();
            this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange( this.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, output );
            SignatureAndHashAlgorithm andHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm( this.mContext, this.mServerCredentials );
            IDigest hash = TlsUtilities.CreateHash( andHashAlgorithm );
            SecurityParameters securityParameters = this.mContext.SecurityParameters;
            hash.BlockUpdate( securityParameters.clientRandom, 0, securityParameters.clientRandom.Length );
            hash.BlockUpdate( securityParameters.serverRandom, 0, securityParameters.serverRandom.Length );
            output.UpdateDigest( hash );
            byte[] certificateSignature = this.mServerCredentials.GenerateCertificateSignature( DigestUtilities.DoFinal( hash ) );
            new DigitallySigned( andHashAlgorithm, certificateSignature ).Encode( output );
            return output.ToArray();
        }

        public override void ProcessServerKeyExchange( Stream input )
        {
            SecurityParameters securityParameters = this.mContext.SecurityParameters;
            SignerInputBuffer tee = new();
            Stream input1 = new TeeInputStream( input, tee );
            ECDomainParameters curve_params = TlsEccUtilities.ReadECParameters( this.mNamedCurves, this.mClientECPointFormats, input1 );
            byte[] encoding = TlsUtilities.ReadOpaque8( input1 );
            DigitallySigned signature = this.ParseSignature( input );
            ISigner s = this.InitVerifyer( this.mTlsSigner, signature.Algorithm, securityParameters );
            tee.UpdateSigner( s );
            if (!s.VerifySignature( signature.Signature ))
                throw new TlsFatalAlert( 51 );
            this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey( TlsEccUtilities.DeserializeECPublicKey( this.mClientECPointFormats, curve_params, encoding ) );
        }

        public override void ValidateCertificateRequest( CertificateRequest certificateRequest )
        {
            foreach (byte certificateType in certificateRequest.CertificateTypes)
            {
                switch (certificateType)
                {
                    case 1:
                    case 2:
                    case 64:
                        continue;
                    default:
                        throw new TlsFatalAlert( 47 );
                }
            }
        }

        public override void ProcessClientCredentials( TlsCredentials clientCredentials )
        {
            if (!(clientCredentials is TlsSignerCredentials))
                throw new TlsFatalAlert( 80 );
        }

        protected virtual ISigner InitVerifyer(
          TlsSigner tlsSigner,
          SignatureAndHashAlgorithm algorithm,
          SecurityParameters securityParameters )
        {
            ISigner verifyer = tlsSigner.CreateVerifyer( algorithm, this.mServerPublicKey );
            verifyer.BlockUpdate( securityParameters.clientRandom, 0, securityParameters.clientRandom.Length );
            verifyer.BlockUpdate( securityParameters.serverRandom, 0, securityParameters.serverRandom.Length );
            return verifyer;
        }
    }
}
