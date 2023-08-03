// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsContext
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsContext
    {
        IRandomGenerator NonceRandomGenerator { get; }

        SecureRandom SecureRandom { get; }

        SecurityParameters SecurityParameters { get; }

        bool IsServer { get; }

        ProtocolVersion ClientVersion { get; }

        ProtocolVersion ServerVersion { get; }

        TlsSession ResumableSession { get; }

        object UserObject { get; set; }

        byte[] ExportKeyingMaterial( string asciiLabel, byte[] context_value, int length );
    }
}
