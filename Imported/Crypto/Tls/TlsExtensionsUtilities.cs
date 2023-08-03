// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsExtensionsUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsExtensionsUtilities
    {
        public static IDictionary EnsureExtensionsInitialised( IDictionary extensions ) => extensions != null ? extensions : Platform.CreateHashtable();

        public static void AddEncryptThenMacExtension( IDictionary extensions ) => extensions[22] = CreateEncryptThenMacExtension();

        public static void AddExtendedMasterSecretExtension( IDictionary extensions ) => extensions[23] = CreateExtendedMasterSecretExtension();

        public static void AddHeartbeatExtension(
          IDictionary extensions,
          HeartbeatExtension heartbeatExtension )
        {
            extensions[15] = CreateHeartbeatExtension( heartbeatExtension );
        }

        public static void AddMaxFragmentLengthExtension( IDictionary extensions, byte maxFragmentLength ) => extensions[1] = CreateMaxFragmentLengthExtension( maxFragmentLength );

        public static void AddServerNameExtension( IDictionary extensions, ServerNameList serverNameList ) => extensions[0] = CreateServerNameExtension( serverNameList );

        public static void AddStatusRequestExtension(
          IDictionary extensions,
          CertificateStatusRequest statusRequest )
        {
            extensions[5] = CreateStatusRequestExtension( statusRequest );
        }

        public static void AddTruncatedHMacExtension( IDictionary extensions ) => extensions[4] = CreateTruncatedHMacExtension();

        public static HeartbeatExtension GetHeartbeatExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 15 );
            return extensionData != null ? ReadHeartbeatExtension( extensionData ) : null;
        }

        public static short GetMaxFragmentLengthExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 1 );
            return extensionData != null ? ReadMaxFragmentLengthExtension( extensionData ) : (short)-1;
        }

        public static ServerNameList GetServerNameExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 0 );
            return extensionData != null ? ReadServerNameExtension( extensionData ) : null;
        }

        public static CertificateStatusRequest GetStatusRequestExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 5 );
            return extensionData != null ? ReadStatusRequestExtension( extensionData ) : null;
        }

        public static bool HasEncryptThenMacExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 22 );
            return extensionData != null && ReadEncryptThenMacExtension( extensionData );
        }

        public static bool HasExtendedMasterSecretExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 23 );
            return extensionData != null && ReadExtendedMasterSecretExtension( extensionData );
        }

        public static bool HasTruncatedHMacExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 4 );
            return extensionData != null && ReadTruncatedHMacExtension( extensionData );
        }

        public static byte[] CreateEmptyExtensionData() => TlsUtilities.EmptyBytes;

        public static byte[] CreateEncryptThenMacExtension() => CreateEmptyExtensionData();

        public static byte[] CreateExtendedMasterSecretExtension() => CreateEmptyExtensionData();

        public static byte[] CreateHeartbeatExtension( HeartbeatExtension heartbeatExtension )
        {
            if (heartbeatExtension == null)
                throw new TlsFatalAlert( 80 );
            MemoryStream output = new MemoryStream();
            heartbeatExtension.Encode( output );
            return output.ToArray();
        }

        public static byte[] CreateMaxFragmentLengthExtension( byte maxFragmentLength ) => new byte[1]
        {
      maxFragmentLength
        };

        public static byte[] CreateServerNameExtension( ServerNameList serverNameList )
        {
            if (serverNameList == null)
                throw new TlsFatalAlert( 80 );
            MemoryStream output = new MemoryStream();
            serverNameList.Encode( output );
            return output.ToArray();
        }

        public static byte[] CreateStatusRequestExtension( CertificateStatusRequest statusRequest )
        {
            if (statusRequest == null)
                throw new TlsFatalAlert( 80 );
            MemoryStream output = new MemoryStream();
            statusRequest.Encode( output );
            return output.ToArray();
        }

        public static byte[] CreateTruncatedHMacExtension() => CreateEmptyExtensionData();

        private static bool ReadEmptyExtensionData( byte[] extensionData )
        {
            if (extensionData == null)
                throw new ArgumentNullException( nameof( extensionData ) );
            if (extensionData.Length != 0)
                throw new TlsFatalAlert( 47 );
            return true;
        }

        public static bool ReadEncryptThenMacExtension( byte[] extensionData ) => ReadEmptyExtensionData( extensionData );

        public static bool ReadExtendedMasterSecretExtension( byte[] extensionData ) => ReadEmptyExtensionData( extensionData );

        public static HeartbeatExtension ReadHeartbeatExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            HeartbeatExtension heartbeatExtension = HeartbeatExtension.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            return heartbeatExtension;
        }

        public static short ReadMaxFragmentLengthExtension( byte[] extensionData )
        {
            if (extensionData == null)
                throw new ArgumentNullException( nameof( extensionData ) );
            return extensionData.Length == 1 ? extensionData[0] : throw new TlsFatalAlert( 50 );
        }

        public static ServerNameList ReadServerNameExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            ServerNameList serverNameList = ServerNameList.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            return serverNameList;
        }

        public static CertificateStatusRequest ReadStatusRequestExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            CertificateStatusRequest certificateStatusRequest = CertificateStatusRequest.Parse( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            return certificateStatusRequest;
        }

        public static bool ReadTruncatedHMacExtension( byte[] extensionData ) => ReadEmptyExtensionData( extensionData );
    }
}
