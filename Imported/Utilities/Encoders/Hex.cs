﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Encoders.Hex
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
    public sealed class Hex
    {
        private static readonly IEncoder encoder = new HexEncoder();

        private Hex()
        {
        }

        public static string ToHexString( byte[] data ) => ToHexString( data, 0, data.Length );

        public static string ToHexString( byte[] data, int off, int length ) => Strings.FromAsciiByteArray( Encode( data, off, length ) );

        public static byte[] Encode( byte[] data ) => Encode( data, 0, data.Length );

        public static byte[] Encode( byte[] data, int off, int length )
        {
            MemoryStream outStream = new MemoryStream( length * 2 );
            encoder.Encode( data, off, length, outStream );
            return outStream.ToArray();
        }

        public static int Encode( byte[] data, Stream outStream ) => encoder.Encode( data, 0, data.Length, outStream );

        public static int Encode( byte[] data, int off, int length, Stream outStream ) => encoder.Encode( data, off, length, outStream );

        public static byte[] Decode( byte[] data )
        {
            MemoryStream outStream = new MemoryStream( (data.Length + 1) / 2 );
            encoder.Decode( data, 0, data.Length, outStream );
            return outStream.ToArray();
        }

        public static byte[] Decode( string data )
        {
            MemoryStream outStream = new MemoryStream( (data.Length + 1) / 2 );
            encoder.DecodeString( data, outStream );
            return outStream.ToArray();
        }

        public static int Decode( string data, Stream outStream ) => encoder.DecodeString( data, outStream );
    }
}
