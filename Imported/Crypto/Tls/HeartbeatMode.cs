// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.HeartbeatMode
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class HeartbeatMode
    {
        public const byte peer_allowed_to_send = 1;
        public const byte peer_not_allowed_to_send = 2;

        public static bool IsValid( byte heartbeatMode ) => heartbeatMode >= 1 && heartbeatMode <= 2;
    }
}
