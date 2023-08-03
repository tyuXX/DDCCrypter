// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpV3SignatureGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Date;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpV3SignatureGenerator
    {
        private PublicKeyAlgorithmTag keyAlgorithm;
        private HashAlgorithmTag hashAlgorithm;
        private PgpPrivateKey privKey;
        private ISigner sig;
        private IDigest dig;
        private int signatureType;
        private byte lastb;

        public PgpV3SignatureGenerator(
          PublicKeyAlgorithmTag keyAlgorithm,
          HashAlgorithmTag hashAlgorithm )
        {
            this.keyAlgorithm = keyAlgorithm;
            this.hashAlgorithm = hashAlgorithm;
            this.dig = DigestUtilities.GetDigest( PgpUtilities.GetDigestName( hashAlgorithm ) );
            this.sig = SignerUtilities.GetSigner( PgpUtilities.GetSignatureName( keyAlgorithm, hashAlgorithm ) );
        }

        public void InitSign( int sigType, PgpPrivateKey key ) => this.InitSign( sigType, key, null );

        public void InitSign( int sigType, PgpPrivateKey key, SecureRandom random )
        {
            this.privKey = key;
            this.signatureType = sigType;
            try
            {
                ICipherParameters parameters = key.Key;
                if (random != null)
                    parameters = new ParametersWithRandom( key.Key, random );
                this.sig.Init( true, parameters );
            }
            catch (InvalidKeyException ex)
            {
                throw new PgpException( "invalid key.", ex );
            }
            this.dig.Reset();
            this.lastb = 0;
        }

        public void Update( byte b )
        {
            if (this.signatureType == 1)
                this.doCanonicalUpdateByte( b );
            else
                this.doUpdateByte( b );
        }

        private void doCanonicalUpdateByte( byte b )
        {
            switch (b)
            {
                case 10:
                    if (this.lastb != 13)
                    {
                        this.doUpdateCRLF();
                        break;
                    }
                    break;
                case 13:
                    this.doUpdateCRLF();
                    break;
                default:
                    this.doUpdateByte( b );
                    break;
            }
            this.lastb = b;
        }

        private void doUpdateCRLF()
        {
            this.doUpdateByte( 13 );
            this.doUpdateByte( 10 );
        }

        private void doUpdateByte( byte b )
        {
            this.sig.Update( b );
            this.dig.Update( b );
        }

        public void Update( byte[] b )
        {
            if (this.signatureType == 1)
            {
                for (int index = 0; index != b.Length; ++index)
                    this.doCanonicalUpdateByte( b[index] );
            }
            else
            {
                this.sig.BlockUpdate( b, 0, b.Length );
                this.dig.BlockUpdate( b, 0, b.Length );
            }
        }

        public void Update( byte[] b, int off, int len )
        {
            if (this.signatureType == 1)
            {
                int num = off + len;
                for (int index = off; index != num; ++index)
                    this.doCanonicalUpdateByte( b[index] );
            }
            else
            {
                this.sig.BlockUpdate( b, off, len );
                this.dig.BlockUpdate( b, off, len );
            }
        }

        public PgpOnePassSignature GenerateOnePassVersion( bool isNested ) => new( new OnePassSignaturePacket( this.signatureType, this.hashAlgorithm, this.keyAlgorithm, this.privKey.KeyId, isNested ) );

        public PgpSignature Generate()
        {
            long num = DateTimeUtilities.CurrentUnixMs() / 1000L;
            byte[] input = new byte[5]
            {
        (byte) this.signatureType,
        (byte) (num >> 24),
        (byte) (num >> 16),
        (byte) (num >> 8),
        (byte) num
            };
            this.sig.BlockUpdate( input, 0, input.Length );
            this.dig.BlockUpdate( input, 0, input.Length );
            byte[] signature1 = this.sig.GenerateSignature();
            byte[] numArray = DigestUtilities.DoFinal( this.dig );
            byte[] fingerprint = new byte[2]
            {
        numArray[0],
        numArray[1]
            };
            MPInteger[] signature2 = this.keyAlgorithm == PublicKeyAlgorithmTag.RsaSign || this.keyAlgorithm == PublicKeyAlgorithmTag.RsaGeneral ? PgpUtilities.RsaSigToMpi( signature1 ) : PgpUtilities.DsaSigToMpi( signature1 );
            return new PgpSignature( new SignaturePacket( 3, this.signatureType, this.privKey.KeyId, this.keyAlgorithm, this.hashAlgorithm, num * 1000L, fingerprint, signature2 ) );
        }
    }
}
