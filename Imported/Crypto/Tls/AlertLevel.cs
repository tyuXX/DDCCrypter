// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AlertLevel
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AlertLevel
    {
        public const byte warning = 1;
        public const byte fatal = 2;

        public static string GetName( byte alertDescription )
        {
            switch (alertDescription)
            {
                case 1:
                    return "warning";
                case 2:
                    return "fatal";
                default:
                    return "UNKNOWN";
            }
        }

        public static string GetText( byte alertDescription ) => GetName( alertDescription ) + "(" + alertDescription + ")";
    }
}
