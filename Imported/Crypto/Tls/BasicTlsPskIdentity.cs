// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.BasicTlsPskIdentity
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class BasicTlsPskIdentity : TlsPskIdentity
    {
        protected byte[] mIdentity;
        protected byte[] mPsk;

        public BasicTlsPskIdentity( byte[] identity, byte[] psk )
        {
            this.mIdentity = Arrays.Clone( identity );
            this.mPsk = Arrays.Clone( psk );
        }

        public BasicTlsPskIdentity( string identity, byte[] psk )
        {
            this.mIdentity = Strings.ToUtf8ByteArray( identity );
            this.mPsk = Arrays.Clone( psk );
        }

        public virtual void SkipIdentityHint()
        {
        }

        public virtual void NotifyIdentityHint( byte[] psk_identity_hint )
        {
        }

        public virtual byte[] GetPskIdentity() => this.mIdentity;

        public virtual byte[] GetPsk() => this.mPsk;
    }
}
