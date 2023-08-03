// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.Pem.PemReader
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
    public class PemReader
    {
        private const string BeginString = "-----BEGIN ";
        private const string EndString = "-----END ";
        private readonly TextReader reader;

        public PemReader( TextReader reader ) => this.reader = reader != null ? reader : throw new ArgumentNullException( nameof( reader ) );

        public TextReader Reader => this.reader;

        public PemObject ReadPemObject()
        {
            string source = this.reader.ReadLine();
            if (source != null && Platform.StartsWith( source, "-----BEGIN " ))
            {
                string str = source.Substring( "-----BEGIN ".Length );
                int length = str.IndexOf( '-' );
                string type = str.Substring( 0, length );
                if (length > 0)
                    return this.LoadObject( type );
            }
            return null;
        }

        private PemObject LoadObject( string type )
        {
            string str1 = "-----END " + type;
            IList arrayList = Platform.CreateArrayList();
            StringBuilder stringBuilder = new StringBuilder();
            string source;
            while ((source = this.reader.ReadLine()) != null && Platform.IndexOf( source, str1 ) == -1)
            {
                int length = source.IndexOf( ':' );
                if (length == -1)
                {
                    stringBuilder.Append( source.Trim() );
                }
                else
                {
                    string str2 = source.Substring( 0, length ).Trim();
                    if (Platform.StartsWith( str2, "X-" ))
                        str2 = str2.Substring( 2 );
                    string val = source.Substring( length + 1 ).Trim();
                    arrayList.Add( new PemHeader( str2, val ) );
                }
            }
            if (source == null)
                throw new IOException( str1 + " not found" );
            if (stringBuilder.Length % 4 != 0)
                throw new IOException( "base64 data appears to be truncated" );
            return new PemObject( type, arrayList, Base64.Decode( stringBuilder.ToString() ) );
        }
    }
}
