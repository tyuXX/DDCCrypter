// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsKeyExchange
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsKeyExchange
    {
        void Init( TlsContext context );

        void SkipServerCredentials();

        void ProcessServerCredentials( TlsCredentials serverCredentials );

        void ProcessServerCertificate( Certificate serverCertificate );

        bool RequiresServerKeyExchange { get; }

        byte[] GenerateServerKeyExchange();

        void SkipServerKeyExchange();

        void ProcessServerKeyExchange( Stream input );

        void ValidateCertificateRequest( CertificateRequest certificateRequest );

        void SkipClientCredentials();

        void ProcessClientCredentials( TlsCredentials clientCredentials );

        void ProcessClientCertificate( Certificate clientCertificate );

        void GenerateClientKeyExchange( Stream output );

        void ProcessClientKeyExchange( Stream input );

        byte[] GeneratePremasterSecret();
    }
}
