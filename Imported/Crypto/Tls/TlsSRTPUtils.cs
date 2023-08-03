// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsSRTPUtils
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsSRTPUtils
    {
        public static void AddUseSrtpExtension( IDictionary extensions, UseSrtpData useSRTPData ) => extensions[14] = CreateUseSrtpExtension( useSRTPData );

        public static UseSrtpData GetUseSrtpExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 14 );
            return extensionData != null ? ReadUseSrtpExtension( extensionData ) : null;
        }

        public static byte[] CreateUseSrtpExtension( UseSrtpData useSrtpData )
        {
            if (useSrtpData == null)
                throw new ArgumentNullException( nameof( useSrtpData ) );
            MemoryStream output = new();
            TlsUtilities.WriteUint16ArrayWithUint16Length( useSrtpData.ProtectionProfiles, output );
            TlsUtilities.WriteOpaque8( useSrtpData.Mki, output );
            return output.ToArray();
        }

        public static UseSrtpData ReadUseSrtpExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, true ) : throw new ArgumentNullException( nameof( extensionData ) );
            int num = TlsUtilities.ReadUint16( memoryStream );
            int[] protectionProfiles = num >= 2 && (num & 1) == 0 ? TlsUtilities.ReadUint16Array( num / 2, memoryStream ) : throw new TlsFatalAlert( 50 );
            byte[] mki = TlsUtilities.ReadOpaque8( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            return new UseSrtpData( protectionProfiles, mki );
        }
    }
}
