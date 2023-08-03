// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsSrpUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsSrpUtilities
    {
        public static void AddSrpExtension( IDictionary extensions, byte[] identity ) => extensions[12] = CreateSrpExtension( identity );

        public static byte[] GetSrpExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, 12 );
            return extensionData != null ? ReadSrpExtension( extensionData ) : null;
        }

        public static byte[] CreateSrpExtension( byte[] identity ) => identity != null ? TlsUtilities.EncodeOpaque8( identity ) : throw new TlsFatalAlert( 80 );

        public static byte[] ReadSrpExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            byte[] numArray = TlsUtilities.ReadOpaque8( memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            return numArray;
        }

        public static BigInteger ReadSrpParameter( Stream input ) => new BigInteger( 1, TlsUtilities.ReadOpaque16( input ) );

        public static void WriteSrpParameter( BigInteger x, Stream output ) => TlsUtilities.WriteOpaque16( BigIntegers.AsUnsignedByteArray( x ), output );

        public static bool IsSrpCipherSuite( int cipherSuite )
        {
            switch (cipherSuite)
            {
                case 49178:
                case 49179:
                case 49180:
                case 49181:
                case 49182:
                case 49183:
                case 49184:
                case 49185:
                case 49186:
                    return true;
                default:
                    return false;
            }
        }
    }
}
