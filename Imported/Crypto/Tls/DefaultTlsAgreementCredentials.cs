// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DefaultTlsAgreementCredentials
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DefaultTlsAgreementCredentials : AbstractTlsAgreementCredentials
    {
        protected readonly Certificate mCertificate;
        protected readonly AsymmetricKeyParameter mPrivateKey;
        protected readonly IBasicAgreement mBasicAgreement;
        protected readonly bool mTruncateAgreement;

        public DefaultTlsAgreementCredentials(
          Certificate certificate,
          AsymmetricKeyParameter privateKey )
        {
            if (certificate == null)
                throw new ArgumentNullException( nameof( certificate ) );
            if (certificate.IsEmpty)
                throw new ArgumentException( "cannot be empty", nameof( certificate ) );
            if (privateKey == null)
                throw new ArgumentNullException( nameof( privateKey ) );
            if (!privateKey.IsPrivate)
                throw new ArgumentException( "must be private", nameof( privateKey ) );
            switch (privateKey)
            {
                case DHPrivateKeyParameters _:
                    this.mBasicAgreement = new DHBasicAgreement();
                    this.mTruncateAgreement = true;
                    break;
                case ECPrivateKeyParameters _:
                    this.mBasicAgreement = new ECDHBasicAgreement();
                    this.mTruncateAgreement = false;
                    break;
                default:
                    throw new ArgumentException( "type not supported: " + Platform.GetTypeName( privateKey ), nameof( privateKey ) );
            }
            this.mCertificate = certificate;
            this.mPrivateKey = privateKey;
        }

        public override Certificate Certificate => this.mCertificate;

        public override byte[] GenerateAgreement( AsymmetricKeyParameter peerPublicKey )
        {
            this.mBasicAgreement.Init( mPrivateKey );
            BigInteger agreement = this.mBasicAgreement.CalculateAgreement( peerPublicKey );
            return this.mTruncateAgreement ? BigIntegers.AsUnsignedByteArray( agreement ) : BigIntegers.AsUnsignedByteArray( this.mBasicAgreement.GetFieldSize(), agreement );
        }
    }
}
