// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ArmoredOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using DDCCrypter;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class ArmoredOutputStream : BaseOutputStream
    {
        private static readonly byte[] encodingTable = new byte[64]
        {
       65,
       66,
       67,
       68,
       69,
       70,
       71,
       72,
       73,
       74,
       75,
       76,
       77,
       78,
       79,
       80,
       81,
       82,
       83,
       84,
       85,
       86,
       87,
       88,
       89,
       90,
       97,
       98,
       99,
       100,
       101,
       102,
       103,
       104,
       105,
       106,
       107,
       108,
       109,
       110,
       111,
       112,
       113,
       114,
       115,
       116,
       117,
       118,
       119,
       120,
       121,
       122,
       48,
       49,
       50,
       51,
       52,
       53,
       54,
       55,
       56,
       57,
       43,
       47
        };
        private readonly Stream outStream;
        private int[] buf = new int[3];
        private int bufPtr = 0;
        private Crc24 crc = new();
        private int chunkCount = 0;
        private int lastb;
        private bool start = true;
        private bool clearText = false;
        private bool newLine = false;
        private string type;
        private static readonly string nl = Platform.NewLine;
        private static readonly string headerStart = "-----BEGIN PGP ";
        private static readonly string headerTail = "-----";
        private static readonly string footerStart = "-----END PGP ";
        private static readonly string footerTail = "-----";
        private static readonly string version = "BCPG C# v" + Engine.Version;
        private readonly IDictionary headers;

        private static void Encode( Stream outStream, int[] data, int len )
        {
            byte[] buffer = new byte[4];
            int num1 = data[0];
            buffer[0] = encodingTable[(num1 >> 2) & 63];
            switch (len)
            {
                case 1:
                    buffer[1] = encodingTable[(num1 << 4) & 63];
                    buffer[2] = 61;
                    buffer[3] = 61;
                    break;
                case 2:
                    int num2 = data[1];
                    buffer[1] = encodingTable[((num1 << 4) | (num2 >> 4)) & 63];
                    buffer[2] = encodingTable[(num2 << 2) & 63];
                    buffer[3] = 61;
                    break;
                case 3:
                    int num3 = data[1];
                    int num4 = data[2];
                    buffer[1] = encodingTable[((num1 << 4) | (num3 >> 4)) & 63];
                    buffer[2] = encodingTable[((num3 << 2) | (num4 >> 6)) & 63];
                    buffer[3] = encodingTable[num4 & 63];
                    break;
            }
            outStream.Write( buffer, 0, buffer.Length );
        }

        public ArmoredOutputStream( Stream outStream )
        {
            this.outStream = outStream;
            this.headers = Platform.CreateHashtable();
            this.headers["Version"] = version;
        }

        public ArmoredOutputStream( Stream outStream, IDictionary headers )
        {
            this.outStream = outStream;
            this.headers = Platform.CreateHashtable( headers );
            this.headers["Version"] = version;
        }

        public void SetHeader( string name, string v ) => this.headers[name] = v;

        public void ResetHeaders()
        {
            this.headers.Clear();
            this.headers["Version"] = version;
        }

        public void BeginClearText( HashAlgorithmTag hashAlgorithm )
        {
            string str;
            switch (hashAlgorithm)
            {
                case HashAlgorithmTag.MD5:
                    str = "MD5";
                    break;
                case HashAlgorithmTag.Sha1:
                    str = "SHA1";
                    break;
                case HashAlgorithmTag.RipeMD160:
                    str = "RIPEMD160";
                    break;
                case HashAlgorithmTag.MD2:
                    str = "MD2";
                    break;
                case HashAlgorithmTag.Sha256:
                    str = "SHA256";
                    break;
                case HashAlgorithmTag.Sha384:
                    str = "SHA384";
                    break;
                case HashAlgorithmTag.Sha512:
                    str = "SHA512";
                    break;
                default:
                    throw new IOException( "unknown hash algorithm tag in beginClearText: " + hashAlgorithm );
            }
            this.DoWrite( "-----BEGIN PGP SIGNED MESSAGE-----" + nl );
            this.DoWrite( "Hash: " + str + nl + nl );
            this.clearText = true;
            this.newLine = true;
            this.lastb = 0;
        }

        public void EndClearText() => this.clearText = false;

        public override void WriteByte( byte b )
        {
            if (this.clearText)
            {
                this.outStream.WriteByte( b );
                if (this.newLine)
                {
                    if (b != 10 || this.lastb != 13)
                        this.newLine = false;
                    if (b == 45)
                    {
                        this.outStream.WriteByte( 32 );
                        this.outStream.WriteByte( 45 );
                    }
                }
                if (b == 13 || (b == 10 && this.lastb != 13))
                    this.newLine = true;
                this.lastb = b;
            }
            else
            {
                if (this.start)
                {
                    switch ((b & 64) == 0 ? (b & 63) >> 2 : b & 63)
                    {
                        case 2:
                            this.type = "SIGNATURE";
                            break;
                        case 5:
                            this.type = "PRIVATE KEY BLOCK";
                            break;
                        case 6:
                            this.type = "PUBLIC KEY BLOCK";
                            break;
                        default:
                            this.type = "MESSAGE";
                            break;
                    }
                    this.DoWrite( headerStart + this.type + headerTail + nl );
                    this.WriteHeaderEntry( "Version", (string)this.headers["Version"] );
                    foreach (DictionaryEntry header in this.headers)
                    {
                        string key = (string)header.Key;
                        if (key != "Version")
                        {
                            string v = (string)header.Value;
                            this.WriteHeaderEntry( key, v );
                        }
                    }
                    this.DoWrite( nl );
                    this.start = false;
                }
                if (this.bufPtr == 3)
                {
                    Encode( this.outStream, this.buf, this.bufPtr );
                    this.bufPtr = 0;
                    if ((++this.chunkCount & 15) == 0)
                        this.DoWrite( nl );
                }
                this.crc.Update( b );
                this.buf[this.bufPtr++] = b & byte.MaxValue;
            }
        }

        public override void Close()
        {
            if (this.type == null)
                return;
            this.DoClose();
            this.type = null;
            this.start = true;
            base.Close();
        }

        private void DoClose()
        {
            if (this.bufPtr > 0)
                Encode( this.outStream, this.buf, this.bufPtr );
            this.DoWrite( nl + '=' );
            int num = this.crc.Value;
            this.buf[0] = (num >> 16) & byte.MaxValue;
            this.buf[1] = (num >> 8) & byte.MaxValue;
            this.buf[2] = num & byte.MaxValue;
            Encode( this.outStream, this.buf, 3 );
            this.DoWrite( nl );
            this.DoWrite( footerStart );
            this.DoWrite( this.type );
            this.DoWrite( footerTail );
            this.DoWrite( nl );
            this.outStream.Flush();
        }

        private void WriteHeaderEntry( string name, string v ) => this.DoWrite( name + ": " + v + nl );

        private void DoWrite( string s )
        {
            byte[] asciiByteArray = Strings.ToAsciiByteArray( s );
            this.outStream.Write( asciiByteArray, 0, asciiByteArray.Length );
        }
    }
}
