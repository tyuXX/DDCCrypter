// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsServerContextImpl
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class TlsServerContextImpl : AbstractTlsContext, TlsServerContext, TlsContext
    {
        internal TlsServerContextImpl( SecureRandom secureRandom, SecurityParameters securityParameters )
          : base( secureRandom, securityParameters )
        {
        }

        public override bool IsServer => true;
    }
}
