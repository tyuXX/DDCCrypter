// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsDsaSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsDsaSigner : AbstractTlsSigner
    {
        public override byte[] GenerateRawSignature(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey,
          byte[] hash )
        {
            ISigner signer = this.MakeSigner( algorithm, true, true, new ParametersWithRandom( privateKey, this.mContext.SecureRandom ) );
            if (algorithm == null)
                signer.BlockUpdate( hash, 16, 20 );
            else
                signer.BlockUpdate( hash, 0, hash.Length );
            return signer.GenerateSignature();
        }

        public override bool VerifyRawSignature(
          SignatureAndHashAlgorithm algorithm,
          byte[] sigBytes,
          AsymmetricKeyParameter publicKey,
          byte[] hash )
        {
            ISigner signer = this.MakeSigner( algorithm, true, false, publicKey );
            if (algorithm == null)
                signer.BlockUpdate( hash, 16, 20 );
            else
                signer.BlockUpdate( hash, 0, hash.Length );
            return signer.VerifySignature( sigBytes );
        }

        public override ISigner CreateSigner(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey )
        {
            return this.MakeSigner( algorithm, false, true, privateKey );
        }

        public override ISigner CreateVerifyer(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter publicKey )
        {
            return this.MakeSigner( algorithm, false, false, publicKey );
        }

        protected virtual ICipherParameters MakeInitParameters( bool forSigning, ICipherParameters cp ) => cp;

        protected virtual ISigner MakeSigner(
          SignatureAndHashAlgorithm algorithm,
          bool raw,
          bool forSigning,
          ICipherParameters cp )
        {
            if (algorithm != null != TlsUtilities.IsTlsV12( this.mContext ))
                throw new InvalidOperationException();
            if (algorithm != null && algorithm.Signature != SignatureAlgorithm)
                throw new InvalidOperationException();
            byte hashAlgorithm = algorithm == null ? (byte)2 : algorithm.Hash;
            IDigest digest = raw ? new NullDigest() : TlsUtilities.CreateHash( hashAlgorithm );
            ISigner signer = new DsaDigestSigner( this.CreateDsaImpl( hashAlgorithm ), digest );
            signer.Init( forSigning, this.MakeInitParameters( forSigning, cp ) );
            return signer;
        }

        protected abstract byte SignatureAlgorithm { get; }

        protected abstract IDsa CreateDsaImpl( byte hashAlgorithm );
    }
}
