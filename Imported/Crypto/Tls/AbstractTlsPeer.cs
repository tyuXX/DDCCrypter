// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsPeer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsPeer : TlsPeer
    {
        public virtual bool ShouldUseGmtUnixTime() => false;

        public virtual void NotifySecureRenegotiation( bool secureRenegotiation )
        {
            if (!secureRenegotiation)
                throw new TlsFatalAlert( 40 );
        }

        public abstract TlsCompression GetCompression();

        public abstract TlsCipher GetCipher();

        public virtual void NotifyAlertRaised(
          byte alertLevel,
          byte alertDescription,
          string message,
          Exception cause )
        {
        }

        public virtual void NotifyAlertReceived( byte alertLevel, byte alertDescription )
        {
        }

        public virtual void NotifyHandshakeComplete()
        {
        }
    }
}
