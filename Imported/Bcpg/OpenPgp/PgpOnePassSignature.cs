// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpOnePassSignature
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpOnePassSignature
    {
        private OnePassSignaturePacket sigPack;
        private int signatureType;
        private ISigner sig;
        private byte lastb;

        internal PgpOnePassSignature( BcpgInputStream bcpgInput )
          : this( (OnePassSignaturePacket)bcpgInput.ReadPacket() )
        {
        }

        internal PgpOnePassSignature( OnePassSignaturePacket sigPack )
        {
            this.sigPack = sigPack;
            this.signatureType = sigPack.SignatureType;
        }

        public void InitVerify( PgpPublicKey pubKey )
        {
            this.lastb = 0;
            try
            {
                this.sig = SignerUtilities.GetSigner( PgpUtilities.GetSignatureName( this.sigPack.KeyAlgorithm, this.sigPack.HashAlgorithm ) );
            }
            catch (Exception ex)
            {
                throw new PgpException( "can't set up signature object.", ex );
            }
            try
            {
                this.sig.Init( false, pubKey.GetKey() );
            }
            catch (InvalidKeyException ex)
            {
                throw new PgpException( "invalid key.", ex );
            }
        }

        public void Update( byte b )
        {
            if (this.signatureType == 1)
                this.doCanonicalUpdateByte( b );
            else
                this.sig.Update( b );
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
                    this.sig.Update( b );
                    break;
            }
            this.lastb = b;
        }

        private void doUpdateCRLF()
        {
            this.sig.Update( 13 );
            this.sig.Update( 10 );
        }

        public void Update( byte[] bytes )
        {
            if (this.signatureType == 1)
            {
                for (int index = 0; index != bytes.Length; ++index)
                    this.doCanonicalUpdateByte( bytes[index] );
            }
            else
                this.sig.BlockUpdate( bytes, 0, bytes.Length );
        }

        public void Update( byte[] bytes, int off, int length )
        {
            if (this.signatureType == 1)
            {
                int num = off + length;
                for (int index = off; index != num; ++index)
                    this.doCanonicalUpdateByte( bytes[index] );
            }
            else
                this.sig.BlockUpdate( bytes, off, length );
        }

        public bool Verify( PgpSignature pgpSig )
        {
            byte[] signatureTrailer = pgpSig.GetSignatureTrailer();
            this.sig.BlockUpdate( signatureTrailer, 0, signatureTrailer.Length );
            return this.sig.VerifySignature( pgpSig.GetSignature() );
        }

        public long KeyId => this.sigPack.KeyId;

        public int SignatureType => this.sigPack.SignatureType;

        public HashAlgorithmTag HashAlgorithm => this.sigPack.HashAlgorithm;

        public PublicKeyAlgorithmTag KeyAlgorithm => this.sigPack.KeyAlgorithm;

        public byte[] GetEncoded()
        {
            MemoryStream outStr = new();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public void Encode( Stream outStr ) => BcpgOutputStream.Wrap( outStr ).WritePacket( sigPack );
    }
}
