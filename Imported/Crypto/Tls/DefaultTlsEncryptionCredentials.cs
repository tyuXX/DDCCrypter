// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DefaultTlsEncryptionCredentials
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DefaultTlsEncryptionCredentials : AbstractTlsEncryptionCredentials
    {
        protected readonly TlsContext mContext;
        protected readonly Certificate mCertificate;
        protected readonly AsymmetricKeyParameter mPrivateKey;

        public DefaultTlsEncryptionCredentials(
          TlsContext context,
          Certificate certificate,
          AsymmetricKeyParameter privateKey )
        {
            if (certificate == null)
                throw new ArgumentNullException( nameof( certificate ) );
            if (certificate.IsEmpty)
                throw new ArgumentException( "cannot be empty", nameof( certificate ) );
            if (privateKey == null)
                throw new ArgumentNullException( "'privateKey' cannot be null" );
            if (!privateKey.IsPrivate)
                throw new ArgumentException( "must be private", nameof( privateKey ) );
            if (!(privateKey is RsaKeyParameters))
                throw new ArgumentException( "type not supported: " + Platform.GetTypeName( privateKey ), nameof( privateKey ) );
            this.mContext = context;
            this.mCertificate = certificate;
            this.mPrivateKey = privateKey;
        }

        public override Certificate Certificate => this.mCertificate;

        public override byte[] DecryptPreMasterSecret( byte[] encryptedPreMasterSecret ) => TlsRsaUtilities.SafeDecryptPreMasterSecret( this.mContext, (RsaKeyParameters)this.mPrivateKey, encryptedPreMasterSecret );
    }
}
