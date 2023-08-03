// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsRsaSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsRsaSigner : AbstractTlsSigner
    {
        public override byte[] GenerateRawSignature(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey,
          byte[] hash )
        {
            ISigner signer = this.MakeSigner( algorithm, true, true, new ParametersWithRandom( privateKey, this.mContext.SecureRandom ) );
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
            signer.BlockUpdate( hash, 0, hash.Length );
            return signer.VerifySignature( sigBytes );
        }

        public override ISigner CreateSigner(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey )
        {
            return this.MakeSigner( algorithm, false, true, new ParametersWithRandom( privateKey, this.mContext.SecureRandom ) );
        }

        public override ISigner CreateVerifyer(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter publicKey )
        {
            return this.MakeSigner( algorithm, false, false, publicKey );
        }

        public override bool IsValidPublicKey( AsymmetricKeyParameter publicKey ) => publicKey is RsaKeyParameters && !publicKey.IsPrivate;

        protected virtual ISigner MakeSigner(
          SignatureAndHashAlgorithm algorithm,
          bool raw,
          bool forSigning,
          ICipherParameters cp )
        {
            if (algorithm != null != TlsUtilities.IsTlsV12( this.mContext ))
                throw new InvalidOperationException();
            if (algorithm != null && algorithm.Signature != 1)
                throw new InvalidOperationException();
            IDigest digest = !raw ? (algorithm != null ? TlsUtilities.CreateHash( algorithm.Hash ) : new CombinedHash()) : new NullDigest();
            ISigner signer = algorithm == null ? new GenericSigner( this.CreateRsaImpl(), digest ) : (ISigner)new RsaDigestSigner( digest, TlsUtilities.GetOidForHashAlgorithm( algorithm.Hash ) );
            signer.Init( forSigning, cp );
            return signer;
        }

        protected virtual IAsymmetricBlockCipher CreateRsaImpl() => new Pkcs1Encoding( new RsaBlindedEngine() );
    }
}
