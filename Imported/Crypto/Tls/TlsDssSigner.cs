// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsDssSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsDssSigner : TlsDsaSigner
    {
        public override bool IsValidPublicKey( AsymmetricKeyParameter publicKey ) => publicKey is DsaPublicKeyParameters;

        protected override IDsa CreateDsaImpl( byte hashAlgorithm ) => new DsaSigner( new HMacDsaKCalculator( TlsUtilities.CreateHash( hashAlgorithm ) ) );

        protected override byte SignatureAlgorithm => 2;
    }
}
