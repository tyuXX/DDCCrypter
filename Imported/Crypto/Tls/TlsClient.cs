// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsClient
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsClient : TlsPeer
    {
        void Init( TlsClientContext context );

        TlsSession GetSessionToResume();

        ProtocolVersion ClientHelloRecordLayerVersion { get; }

        ProtocolVersion ClientVersion { get; }

        bool IsFallback { get; }

        int[] GetCipherSuites();

        byte[] GetCompressionMethods();

        IDictionary GetClientExtensions();

        void NotifyServerVersion( ProtocolVersion selectedVersion );

        void NotifySessionID( byte[] sessionID );

        void NotifySelectedCipherSuite( int selectedCipherSuite );

        void NotifySelectedCompressionMethod( byte selectedCompressionMethod );

        void ProcessServerExtensions( IDictionary serverExtensions );

        void ProcessServerSupplementalData( IList serverSupplementalData );

        TlsKeyExchange GetKeyExchange();

        TlsAuthentication GetAuthentication();

        IList GetClientSupplementalData();

        void NotifyNewSessionTicket( NewSessionTicket newSessionTicket );
    }
}
