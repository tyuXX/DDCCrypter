// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsSigner
    {
        void Init( TlsContext context );

        byte[] GenerateRawSignature( AsymmetricKeyParameter privateKey, byte[] md5AndSha1 );

        byte[] GenerateRawSignature(
          SignatureAndHashAlgorithm algorithm,
          AsymmetricKeyParameter privateKey,
          byte[] hash );

        bool VerifyRawSignature( byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1 );

        bool VerifyRawSignature(
          SignatureAndHashAlgorithm algorithm,
          byte[] sigBytes,
          AsymmetricKeyParameter publicKey,
          byte[] hash );

        ISigner CreateSigner( AsymmetricKeyParameter privateKey );

        ISigner CreateSigner( SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey );

        ISigner CreateVerifyer( AsymmetricKeyParameter publicKey );

        ISigner CreateVerifyer( SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey );

        bool IsValidPublicKey( AsymmetricKeyParameter publicKey );
    }
}
