// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsServer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsServer : TlsPeer
    {
        void Init( TlsServerContext context );

        void NotifyClientVersion( ProtocolVersion clientVersion );

        void NotifyFallback( bool isFallback );

        void NotifyOfferedCipherSuites( int[] offeredCipherSuites );

        void NotifyOfferedCompressionMethods( byte[] offeredCompressionMethods );

        void ProcessClientExtensions( IDictionary clientExtensions );

        ProtocolVersion GetServerVersion();

        int GetSelectedCipherSuite();

        byte GetSelectedCompressionMethod();

        IDictionary GetServerExtensions();

        IList GetServerSupplementalData();

        TlsCredentials GetCredentials();

        CertificateStatus GetCertificateStatus();

        TlsKeyExchange GetKeyExchange();

        CertificateRequest GetCertificateRequest();

        void ProcessClientSupplementalData( IList clientSupplementalData );

        void NotifyClientCertificate( Certificate clientCertificate );

        NewSessionTicket GetNewSessionTicket();
    }
}
