// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.PemParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System.IO;

namespace Org.BouncyCastle.X509
{
    internal class PemParser
    {
        private readonly string _header1;
        private readonly string _header2;
        private readonly string _footer1;
        private readonly string _footer2;

        internal PemParser( string type )
        {
            this._header1 = "-----BEGIN " + type + "-----";
            this._header2 = "-----BEGIN X509 " + type + "-----";
            this._footer1 = "-----END " + type + "-----";
            this._footer2 = "-----END X509 " + type + "-----";
        }

        private string ReadLine( Stream inStream )
        {
            StringBuilder stringBuilder = new();
            while (true)
            {
                int num;
                do
                {
                    while ((num = inStream.ReadByte()) == 13 || num == 10 || num < 0)
                    {
                        if (num < 0 || stringBuilder.Length != 0)
                            return num < 0 ? null : stringBuilder.ToString();
                    }
                }
                while (num == 13);
                stringBuilder.Append( (char)num );
            }
        }

        internal Asn1Sequence ReadPemObject( Stream inStream )
        {
            StringBuilder stringBuilder = new();
            string source1;
            do
                ;
            while ((source1 = this.ReadLine( inStream )) != null && !Platform.StartsWith( source1, this._header1 ) && !Platform.StartsWith( source1, this._header2 ));
            string source2;
            while ((source2 = this.ReadLine( inStream )) != null && !Platform.StartsWith( source2, this._footer1 ) && !Platform.StartsWith( source2, this._footer2 ))
                stringBuilder.Append( source2 );
            if (stringBuilder.Length == 0)
                return null;
            Asn1Object asn1Object = Asn1Object.FromByteArray( Base64.Decode( stringBuilder.ToString() ) );
            return asn1Object is Asn1Sequence ? (Asn1Sequence)asn1Object : throw new IOException( "malformed PEM data encountered" );
        }
    }
}
