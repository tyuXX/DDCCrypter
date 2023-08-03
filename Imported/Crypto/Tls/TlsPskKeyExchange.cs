// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsPskKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsPskKeyExchange : AbstractTlsKeyExchange
    {
        protected TlsPskIdentity mPskIdentity;
        protected TlsPskIdentityManager mPskIdentityManager;
        protected DHParameters mDHParameters;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected byte[] mPskIdentityHint = null;
        protected byte[] mPsk = null;
        protected DHPrivateKeyParameters mDHAgreePrivateKey = null;
        protected DHPublicKeyParameters mDHAgreePublicKey = null;
        protected ECPrivateKeyParameters mECAgreePrivateKey = null;
        protected ECPublicKeyParameters mECAgreePublicKey = null;
        protected AsymmetricKeyParameter mServerPublicKey = null;
        protected RsaKeyParameters mRsaServerPublicKey = null;
        protected TlsEncryptionCredentials mServerCredentials = null;
        protected byte[] mPremasterSecret;

        public TlsPskKeyExchange(
          int keyExchange,
          IList supportedSignatureAlgorithms,
          TlsPskIdentity pskIdentity,
          TlsPskIdentityManager pskIdentityManager,
          DHParameters dhParameters,
          int[] namedCurves,
          byte[] clientECPointFormats,
          byte[] serverECPointFormats )
          : base( keyExchange, supportedSignatureAlgorithms )
        {
            switch (keyExchange)
            {
                case 13:
                case 14:
                case 15:
                case 24:
                    this.mPskIdentity = pskIdentity;
                    this.mPskIdentityManager = pskIdentityManager;
                    this.mDHParameters = dhParameters;
                    this.mNamedCurves = namedCurves;
                    this.mClientECPointFormats = clientECPointFormats;
                    this.mServerECPointFormats = serverECPointFormats;
                    break;
                default:
                    throw new InvalidOperationException( "unsupported key exchange algorithm" );
            }
        }

        public override void SkipServerCredentials()
        {
            if (this.mKeyExchange == 15)
                throw new TlsFatalAlert( 10 );
        }

        public override void ProcessServerCredentials( TlsCredentials serverCredentials )
        {
            if (!(serverCredentials is TlsEncryptionCredentials))
                throw new TlsFatalAlert( 80 );
            this.ProcessServerCertificate( serverCredentials.Certificate );
            this.mServerCredentials = (TlsEncryptionCredentials)serverCredentials;
        }

        public override byte[] GenerateServerKeyExchange()
        {
            this.mPskIdentityHint = this.mPskIdentityManager.GetHint();
            if (this.mPskIdentityHint == null && !this.RequiresServerKeyExchange)
                return null;
            MemoryStream output = new MemoryStream();
            if (this.mPskIdentityHint == null)
                TlsUtilities.WriteOpaque16( TlsUtilities.EmptyBytes, output );
            else
                TlsUtilities.WriteOpaque16( this.mPskIdentityHint, output );
            if (this.mKeyExchange == 14)
            {
                if (this.mDHParameters == null)
                    throw new TlsFatalAlert( 80 );
                this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange( this.mContext.SecureRandom, this.mDHParameters, output );
            }
            else if (this.mKeyExchange == 24)
                this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange( this.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, output );
            return output.ToArray();
        }

        public override void ProcessServerCertificate( Certificate serverCertificate )
        {
            if (this.mKeyExchange != 15)
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
            this.mRsaServerPublicKey = !this.mServerPublicKey.IsPrivate ? this.ValidateRsaPublicKey( (RsaKeyParameters)this.mServerPublicKey ) : throw new TlsFatalAlert( 80 );
            TlsUtilities.ValidateKeyUsage( c, 32 );
            base.ProcessServerCertificate( serverCertificate );
        }

        public override bool RequiresServerKeyExchange
        {
            get
            {
                switch (this.mKeyExchange)
                {
                    case 14:
                    case 24:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override void ProcessServerKeyExchange( Stream input )
        {
            this.mPskIdentityHint = TlsUtilities.ReadOpaque16( input );
            if (this.mKeyExchange == 14)
            {
                this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey( ServerDHParams.Parse( input ).PublicKey );
                this.mDHParameters = this.mDHAgreePublicKey.Parameters;
            }
            else
            {
                if (this.mKeyExchange != 24)
                    return;
                this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey( TlsEccUtilities.DeserializeECPublicKey( this.mClientECPointFormats, TlsEccUtilities.ReadECParameters( this.mNamedCurves, this.mClientECPointFormats, input ), TlsUtilities.ReadOpaque8( input ) ) );
            }
        }

        public override void ValidateCertificateRequest( CertificateRequest certificateRequest ) => throw new TlsFatalAlert( 10 );

        public override void ProcessClientCredentials( TlsCredentials clientCredentials ) => throw new TlsFatalAlert( 80 );

        public override void GenerateClientKeyExchange( Stream output )
        {
            if (this.mPskIdentityHint == null)
                this.mPskIdentity.SkipIdentityHint();
            else
                this.mPskIdentity.NotifyIdentityHint( this.mPskIdentityHint );
            byte[] pskIdentity = this.mPskIdentity.GetPskIdentity();
            if (pskIdentity == null)
                throw new TlsFatalAlert( 80 );
            this.mPsk = this.mPskIdentity.GetPsk();
            if (this.mPsk == null)
                throw new TlsFatalAlert( 80 );
            TlsUtilities.WriteOpaque16( pskIdentity, output );
            this.mContext.SecurityParameters.pskIdentity = pskIdentity;
            if (this.mKeyExchange == 14)
                this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange( this.mContext.SecureRandom, this.mDHParameters, output );
            else if (this.mKeyExchange == 24)
            {
                this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange( this.mContext.SecureRandom, this.mServerECPointFormats, this.mECAgreePublicKey.Parameters, output );
            }
            else
            {
                if (this.mKeyExchange != 15)
                    return;
                this.mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret( this.mContext, this.mRsaServerPublicKey, output );
            }
        }

        public override void ProcessClientKeyExchange( Stream input )
        {
            byte[] identity = TlsUtilities.ReadOpaque16( input );
            this.mPsk = this.mPskIdentityManager.GetPsk( identity );
            if (this.mPsk == null)
                throw new TlsFatalAlert( 115 );
            this.mContext.SecurityParameters.pskIdentity = identity;
            if (this.mKeyExchange == 14)
                this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey( new DHPublicKeyParameters( TlsDHUtilities.ReadDHParameter( input ), this.mDHParameters ) );
            else if (this.mKeyExchange == 24)
            {
                this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey( TlsEccUtilities.DeserializeECPublicKey( this.mServerECPointFormats, this.mECAgreePrivateKey.Parameters, TlsUtilities.ReadOpaque8( input ) ) );
            }
            else
            {
                if (this.mKeyExchange != 15)
                    return;
                this.mPremasterSecret = this.mServerCredentials.DecryptPreMasterSecret( !TlsUtilities.IsSsl( this.mContext ) ? TlsUtilities.ReadOpaque16( input ) : Streams.ReadAll( input ) );
            }
        }

        public override byte[] GeneratePremasterSecret()
        {
            byte[] otherSecret = this.GenerateOtherSecret( this.mPsk.Length );
            MemoryStream output = new MemoryStream( 4 + otherSecret.Length + this.mPsk.Length );
            TlsUtilities.WriteOpaque16( otherSecret, output );
            TlsUtilities.WriteOpaque16( this.mPsk, output );
            Arrays.Fill( this.mPsk, 0 );
            this.mPsk = null;
            return output.ToArray();
        }

        protected virtual byte[] GenerateOtherSecret( int pskLength )
        {
            if (this.mKeyExchange == 14)
                return this.mDHAgreePrivateKey != null ? TlsDHUtilities.CalculateDHBasicAgreement( this.mDHAgreePublicKey, this.mDHAgreePrivateKey ) : throw new TlsFatalAlert( 80 );
            if (this.mKeyExchange == 24)
                return this.mECAgreePrivateKey != null ? TlsEccUtilities.CalculateECDHBasicAgreement( this.mECAgreePublicKey, this.mECAgreePrivateKey ) : throw new TlsFatalAlert( 80 );
            return this.mKeyExchange == 15 ? this.mPremasterSecret : new byte[pskLength];
        }

        protected virtual RsaKeyParameters ValidateRsaPublicKey( RsaKeyParameters key ) => key.Exponent.IsProbablePrime( 2 ) ? key : throw new TlsFatalAlert( 47 );
    }
}
