// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsSrpKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsSrpKeyExchange : AbstractTlsKeyExchange
    {
        protected TlsSigner mTlsSigner;
        protected TlsSrpGroupVerifier mGroupVerifier;
        protected byte[] mIdentity;
        protected byte[] mPassword;
        protected AsymmetricKeyParameter mServerPublicKey = null;
        protected Srp6GroupParameters mSrpGroup = null;
        protected Srp6Client mSrpClient = null;
        protected Srp6Server mSrpServer = null;
        protected BigInteger mSrpPeerCredentials = null;
        protected BigInteger mSrpVerifier = null;
        protected byte[] mSrpSalt = null;
        protected TlsSignerCredentials mServerCredentials = null;

        protected static TlsSigner CreateSigner( int keyExchange )
        {
            switch (keyExchange)
            {
                case 21:
                    return null;
                case 22:
                    return new TlsDssSigner();
                case 23:
                    return new TlsRsaSigner();
                default:
                    throw new ArgumentException( "unsupported key exchange algorithm" );
            }
        }

        [Obsolete( "Use constructor taking an explicit 'groupVerifier' argument" )]
        public TlsSrpKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          byte[] identity,
          byte[] password )
          : this( keyExchange, supportedSignatureAlgorithms, new DefaultTlsSrpGroupVerifier(), identity, password )
        {
        }

        public TlsSrpKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          TlsSrpGroupVerifier groupVerifier,
          byte[] identity,
          byte[] password )
          : base( keyExchange, supportedSignatureAlgorithms )
        {
            this.mTlsSigner = CreateSigner( keyExchange );
            this.mGroupVerifier = groupVerifier;
            this.mIdentity = identity;
            this.mPassword = password;
            this.mSrpClient = new Srp6Client();
        }

        public TlsSrpKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          byte[] identity,
          TlsSrpLoginParameters loginParameters )
          : base( keyExchange, supportedSignatureAlgorithms )
        {
            this.mTlsSigner = CreateSigner( keyExchange );
            this.mIdentity = identity;
            this.mSrpServer = new Srp6Server();
            this.mSrpGroup = loginParameters.Group;
            this.mSrpVerifier = loginParameters.Verifier;
            this.mSrpSalt = loginParameters.Salt;
        }

        public override void Init( TlsContext context )
        {
            base.Init( context );
            if (this.mTlsSigner == null)
                return;
            this.mTlsSigner.Init( context );
        }

        public override void SkipServerCredentials()
        {
            if (this.mTlsSigner != null)
                throw new TlsFatalAlert( 10 );
        }

        public override void ProcessServerCertificate( Certificate serverCertificate )
        {
            if (this.mTlsSigner == null)
                throw new TlsFatalAlert( 10 );
            X509CertificateStructure c = !serverCertificate.IsEmpty ? serverCertificate.GetCertificateAt( 0 ) : throw new TlsFatalAlert( 42 );
            SubjectPublicKeyInfo subjectPublicKeyInfo = c.SubjectPublicKeyInfo;
            try
            {
                this.mServerPublicKey = PublicKeyFactory.CreateKey( subjectPublicKeyInfo );
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 43, ex );
            }
            if (!this.mTlsSigner.IsValidPublicKey( this.mServerPublicKey ))
                throw new TlsFatalAlert( 46 );
            TlsUtilities.ValidateKeyUsage( c, 128 );
            base.ProcessServerCertificate( serverCertificate );
        }

        public override void ProcessServerCredentials( TlsCredentials serverCredentials )
        {
            if (this.mKeyExchange == 21 || !(serverCredentials is TlsSignerCredentials))
                throw new TlsFatalAlert( 80 );
            this.ProcessServerCertificate( serverCredentials.Certificate );
            this.mServerCredentials = (TlsSignerCredentials)serverCredentials;
        }

        public override bool RequiresServerKeyExchange => true;

        public override byte[] GenerateServerKeyExchange()
        {
            this.mSrpServer.Init( this.mSrpGroup, this.mSrpVerifier, TlsUtilities.CreateHash( 2 ), this.mContext.SecureRandom );
            ServerSrpParams serverSrpParams = new( this.mSrpGroup.N, this.mSrpGroup.G, this.mSrpSalt, this.mSrpServer.GenerateServerCredentials() );
            DigestInputBuffer output = new();
            serverSrpParams.Encode( output );
            if (this.mServerCredentials != null)
            {
                SignatureAndHashAlgorithm andHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm( this.mContext, this.mServerCredentials );
                IDigest hash = TlsUtilities.CreateHash( andHashAlgorithm );
                SecurityParameters securityParameters = this.mContext.SecurityParameters;
                hash.BlockUpdate( securityParameters.clientRandom, 0, securityParameters.clientRandom.Length );
                hash.BlockUpdate( securityParameters.serverRandom, 0, securityParameters.serverRandom.Length );
                output.UpdateDigest( hash );
                byte[] numArray = new byte[hash.GetDigestSize()];
                hash.DoFinal( numArray, 0 );
                byte[] certificateSignature = this.mServerCredentials.GenerateCertificateSignature( numArray );
                new DigitallySigned( andHashAlgorithm, certificateSignature ).Encode( output );
            }
            return output.ToArray();
        }

        public override void ProcessServerKeyExchange( Stream input )
        {
            SecurityParameters securityParameters = this.mContext.SecurityParameters;
            SignerInputBuffer tee = null;
            Stream input1 = input;
            if (this.mTlsSigner != null)
            {
                tee = new SignerInputBuffer();
                input1 = new TeeInputStream( input, tee );
            }
            ServerSrpParams serverSrpParams = ServerSrpParams.Parse( input1 );
            if (tee != null)
            {
                DigitallySigned signature = this.ParseSignature( input );
                ISigner s = this.InitVerifyer( this.mTlsSigner, signature.Algorithm, securityParameters );
                tee.UpdateSigner( s );
                if (!s.VerifySignature( signature.Signature ))
                    throw new TlsFatalAlert( 51 );
            }
            this.mSrpGroup = new Srp6GroupParameters( serverSrpParams.N, serverSrpParams.G );
            if (!this.mGroupVerifier.Accept( this.mSrpGroup ))
                throw new TlsFatalAlert( 71 );
            this.mSrpSalt = serverSrpParams.S;
            try
            {
                this.mSrpPeerCredentials = Srp6Utilities.ValidatePublicValue( this.mSrpGroup.N, serverSrpParams.B );
            }
            catch (CryptoException ex)
            {
                throw new TlsFatalAlert( 47, ex );
            }
            this.mSrpClient.Init( this.mSrpGroup, TlsUtilities.CreateHash( 2 ), this.mContext.SecureRandom );
        }

        public override void ValidateCertificateRequest( CertificateRequest certificateRequest ) => throw new TlsFatalAlert( 10 );

        public override void ProcessClientCredentials( TlsCredentials clientCredentials ) => throw new TlsFatalAlert( 80 );

        public override void GenerateClientKeyExchange( Stream output )
        {
            TlsSrpUtilities.WriteSrpParameter( this.mSrpClient.GenerateClientCredentials( this.mSrpSalt, this.mIdentity, this.mPassword ), output );
            this.mContext.SecurityParameters.srpIdentity = Arrays.Clone( this.mIdentity );
        }

        public override void ProcessClientKeyExchange( Stream input )
        {
            try
            {
                this.mSrpPeerCredentials = Srp6Utilities.ValidatePublicValue( this.mSrpGroup.N, TlsSrpUtilities.ReadSrpParameter( input ) );
            }
            catch (CryptoException ex)
            {
                throw new TlsFatalAlert( 47, ex );
            }
            this.mContext.SecurityParameters.srpIdentity = Arrays.Clone( this.mIdentity );
        }

        public override byte[] GeneratePremasterSecret()
        {
            try
            {
                return BigIntegers.AsUnsignedByteArray( this.mSrpServer != null ? this.mSrpServer.CalculateSecret( this.mSrpPeerCredentials ) : this.mSrpClient.CalculateSecret( this.mSrpPeerCredentials ) );
            }
            catch (CryptoException ex)
            {
                throw new TlsFatalAlert( 47, ex );
            }
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
