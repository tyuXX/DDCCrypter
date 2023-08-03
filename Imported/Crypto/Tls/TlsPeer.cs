// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsPeer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsPeer
    {
        bool ShouldUseGmtUnixTime();

        void NotifySecureRenegotiation( bool secureRenegotiation );

        TlsCompression GetCompression();

        TlsCipher GetCipher();

        void NotifyAlertRaised(
          byte alertLevel,
          byte alertDescription,
          string message,
          Exception cause );

        void NotifyAlertReceived( byte alertLevel, byte alertDescription );

        void NotifyHandshakeComplete();
    }
}
