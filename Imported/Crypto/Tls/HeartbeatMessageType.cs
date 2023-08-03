// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.HeartbeatMessageType
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class HeartbeatMessageType
    {
        public const byte heartbeat_request = 1;
        public const byte heartbeat_response = 2;

        public static bool IsValid( byte heartbeatMessageType ) => heartbeatMessageType >= 1 && heartbeatMessageType <= 2;
    }
}
