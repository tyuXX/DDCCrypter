// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Encoders.UrlBase64
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
    public class UrlBase64
    {
        private static readonly IEncoder encoder = new UrlBase64Encoder();

        public static byte[] Encode( byte[] data )
        {
            MemoryStream outStream = new();
            try
            {
                encoder.Encode( data, 0, data.Length, outStream );
            }
            catch (IOException ex)
            {
                throw new Exception( "exception encoding URL safe base64 string: " + ex.Message, ex );
            }
            return outStream.ToArray();
        }

        public static int Encode( byte[] data, Stream outStr ) => encoder.Encode( data, 0, data.Length, outStr );

        public static byte[] Decode( byte[] data )
        {
            MemoryStream outStream = new();
            try
            {
                encoder.Decode( data, 0, data.Length, outStream );
            }
            catch (IOException ex)
            {
                throw new Exception( "exception decoding URL safe base64 string: " + ex.Message, ex );
            }
            return outStream.ToArray();
        }

        public static int Decode( byte[] data, Stream outStr ) => encoder.Decode( data, 0, data.Length, outStr );

        public static byte[] Decode( string data )
        {
            MemoryStream outStream = new();
            try
            {
                encoder.DecodeString( data, outStream );
            }
            catch (IOException ex)
            {
                throw new Exception( "exception decoding URL safe base64 string: " + ex.Message, ex );
            }
            return outStream.ToArray();
        }

        public static int Decode( string data, Stream outStr ) => encoder.DecodeString( data, outStr );
    }
}
