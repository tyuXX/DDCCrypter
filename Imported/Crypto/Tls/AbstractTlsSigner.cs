// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsSigner : TlsSigner
    {
        protected TlsContext mContext;

        public virtual void Init( TlsContext context ) => this.mContext = context;

        public virtual byte[] GenerateRawSignature( AsymmetricKeyParameter privateKey, byte[] md5AndSha1 ) => this.GenerateRawSignature( null, privateKey, md5AndSha1 );

        public abstract byte[] GenerateRawSignature(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey,
          byte[] hash );

        public virtual bool VerifyRawSignature(
          byte[] sigBytes,
          AsymmetricKeyParameter publicKey,
          byte[] md5AndSha1 )
        {
            return this.VerifyRawSignature( null, sigBytes, publicKey, md5AndSha1 );
        }

        public abstract bool VerifyRawSignature(
          SignatureAndHashAlgorithm algorithm,
          byte[] sigBytes,
          AsymmetricKeyParameter publicKey,
          byte[] hash );

        public virtual ISigner CreateSigner( AsymmetricKeyParameter privateKey ) => this.CreateSigner( null, privateKey );

        public abstract ISigner CreateSigner(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey );

        public virtual ISigner CreateVerifyer( AsymmetricKeyParameter publicKey ) => this.CreateVerifyer( null, publicKey );

        public abstract ISigner CreateVerifyer(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter publicKey );

        public abstract bool IsValidPublicKey( AsymmetricKeyParameter publicKey );
    }
}
